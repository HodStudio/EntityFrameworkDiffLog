using HodStudio.EfDiffLog.Model;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#else
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
#endif

namespace HodStudio.EfDiffLog.Repository
{
    public static class LoggingContext
    {
        private static readonly List<EntityState> entityStates = new List<EntityState>() { EntityState.Added, EntityState.Modified, EntityState.Deleted };
        private const string emptyJson = "{}";
        private static Dictionary<string, string> IdColumnNames = new Dictionary<string, string>();

        public static void InitializeIdColumnNames(this LoggingDbContext context, Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(LoggedEntityAttribute), true).Length > 0)
                {
                    if (IdColumnNames.ContainsKey(type.Name))
                        throw new AmbiguousMatchException($"More than one entity with the same name ({type.Name}) using the LoggedEntityAttribute.");

                    var attributeValue = ((LoggedEntityAttribute)Attribute.GetCustomAttribute(type.GetType(), typeof(LoggedEntityAttribute))).IdPropertyName;
                    IdColumnNames.Add(type.Name, attributeValue);
                }
            }
        }

        public static async Task LogChanges(this LoggingDbContext context, string userId)
        {
            var logTime = DateTime.Now;

            var changes = context.ChangeTracker.Entries()
                .Where(x => entityStates.Contains(x.State)
                    && IdColumnNames.ContainsKey(x.Entity.GetType().Name))
                .ToList();

            var jdp = new JsonDiffPatch();

            foreach (var item in changes)
            {
                var original = emptyJson;

                var updated = GetValues(item.CurrentValues);
                var creationDate = DateTime.Now;
                var idColumnName = IdColumnNames[item.GetType().Name];

                if (item.State == EntityState.Modified)
                {
                    var dbValues = await item.GetDatabaseValuesAsync();
                    original = GetValues(dbValues);
                }

                string jsonDiff = jdp.Diff(original, updated);

                if (string.IsNullOrWhiteSpace(jsonDiff) == false)
                {
                    var EntityDiff = JToken.Parse(jsonDiff).ToString(Formatting.None);

                    var logEntry = new LogEntry()
                    {
                        EntityName = item.Entity.GetType().Name,
                        EntityId = item.CurrentValues[idColumnName].ToString(),
                        LogDateTime = logTime,
                        Operation = item.State.ToString(),
                        UserId = userId,
                        ValuesChanges = EntityDiff,
                    };

                    context.LogEntries.Add(logEntry);
                }
            }
        }

#if NETSTANDARD
        private static string GetValues(PropertyValues propertyValues)
        {
            var pairs = propertyValues.Properties.ToDictionary(pn => pn.Name, pn => propertyValues[pn]);
            return SerializeDictionary(pairs);
        }
#else
        private static string GetValues(DbPropertyValues propertyValues)
        {
            var pairs = propertyValues.PropertyNames.ToDictionary(x => x, x => propertyValues.GetValue<object>(x));
            return SerializeDictionary(pairs);
        }
#endif
        private static string SerializeDictionary(Dictionary<string, object> pairs) => JsonConvert.SerializeObject(pairs);
    }
}
