using System;

namespace HodStudio.EfDiffLog.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LoggedEntityAttribute : Attribute
    {
        /// <summary>
        /// Defines that the entity will be logged when using the EfDiffLog.
        /// </summary>
        /// <param name="idPropertyName">Id Property name to get the id from the entity</param>
        public LoggedEntityAttribute(string idPropertyName) => IdPropertyName = idPropertyName;

        public string IdPropertyName { get; set; }
    }
}
