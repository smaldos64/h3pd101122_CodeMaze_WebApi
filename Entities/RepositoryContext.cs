using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        private static string _sQLConnectionString = String.Empty;

        public static string SQLConnectionString
        {
            get
            {
                return _sQLConnectionString;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _sQLConnectionString = value;
                }
            }
        }

        private readonly IConfiguration _configuration;
        public RepositoryContext(DbContextOptions options,
                                 IConfiguration configuration) 
            : base(options) 
        {
            this._configuration = configuration;
        }

        public DbSet<Owner>? Owners { get; set; }
        public DbSet<Account>? Accounts { get; set; }
        public DbSet<TestDynamic>? TestDynamics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString;

#if ENABLED_FOR_LAZY_LOADING_USAGE
            if (!String.IsNullOrEmpty(_sQLConnectionString))
            {
                connectionString = _sQLConnectionString;
            }
            else
            {
                connectionString = this._configuration.GetConnectionString("connectionString");
            }
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);
#endif
        }
    }
}
