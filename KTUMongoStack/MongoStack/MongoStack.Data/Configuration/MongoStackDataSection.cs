using System;
using System.Configuration;

namespace MongoStack.Data.Configuration
{
    internal class MongoStackDataSection : ConfigurationSection
    {
        [ConfigurationProperty("connectionUrl", IsRequired = true)]
        public string ConnectionUrl
        {
            get
            {
                return (String)this["connectionUrl"];
            }
            set
            {
                this["connectionUrl"] = value;
            }
        }

        [ConfigurationProperty("databaseName", IsRequired = true)]
        public string DatabaseName
        {
            get
            {
                return (String)this["databaseName"];
            }
            set
            {
                this["databaseName"] = value;
            }
        }
    }
}
