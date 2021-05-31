using Dapper;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data
{
    public class PlanRepository : PersistenceBase<Plan>, IPlanRepository
    {

        public PlanRepository(
            IConfiguration _configuration
            , ILogger<Plan> _logger)
            : base(_configuration, _logger)
        {

        }

        public async Task<IEnumerable<Plan>> GetActive()
        {
            try
            {
                string query = @"Select
	                            plan.id
                                , plan.code
                                , plan.name
                                , plan.description
                                , plan.discount
                                , plan.discountPercent
                                , plan.active
                            From plan
                            Where active = 1;";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                return await conexaoBD.QueryAsync<Plan>(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}