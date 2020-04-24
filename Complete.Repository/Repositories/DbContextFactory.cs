using Complete.Domain;
using Complete.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complete.Repository.Repositories
{
    public class DbContextFactory : IDbContextFactory
    {
        private string contextName;
        private string connectionString;
        public DbContextFactory(string contextName, string connectionString)
        {
            this.contextName = contextName;
            this.connectionString = connectionString;
        }

        public DbContext GetContext()
        {
            switch (this.contextName)
            {
                case "SmartInventoryEntities":
                case "DefaultContext":
                default:
                    return new SmartInventoryEntities(this.connectionString);
            }
        }
    }
}
