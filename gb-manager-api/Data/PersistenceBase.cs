using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace gb_manager.Data
{
    public class PersistenceBase<TEntity>
    {
        public static string ConnectionString { get; set; }
        public static ILogger<TEntity> logger;

        public PersistenceBase(
            IConfiguration _configuration,
            ILogger<TEntity> _logger)
        {
            ConnectionString = _configuration["ConnectionStrings:MysqlConnectionString"];
            logger = _logger;
        }
    }
}