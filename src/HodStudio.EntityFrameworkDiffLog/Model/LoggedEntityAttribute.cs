using System;

namespace HodStudio.EntityFrameworkDiffLog.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LoggedEntityAttribute : Attribute
    {
        public const string DefaultIdPropertyName = "Id";

        /// <summary>
        /// Defines that the entity will be logged when using the EntityFrameworkDiffLog.
        /// It will use the default id property name ("Id"), with case sensitive.
        /// If your class has a different id property name, use the overload, where you can provide the Id property name.
        /// </summary>
        public LoggedEntityAttribute() : this(DefaultIdPropertyName) { }

        /// <summary>
        /// Defines that the entity will be logged when using the EntityFrameworkDiffLog.
        /// Please, pay attention to the fact that the property name is case sensitive.
        /// Recommendation: use nameof(property) to avoid problems
        /// </summary>
        /// <param name="idPropertyName">Id Property name to get the id from the entity</param>
        public LoggedEntityAttribute(string idPropertyName) => IdPropertyName = idPropertyName;

        public string IdPropertyName { get; set; }
    }
}
