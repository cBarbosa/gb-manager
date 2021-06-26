using Dapper;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gb_manager.Data
{
    public class PlanRepository
        : PersistenceBase<Plan>, IPlanRepository
    {
        private readonly Dictionary<int, Plan> PlanDictionary;

        public PlanRepository(
            IConfiguration _configuration
            , ILogger<Plan> _logger)
            : base(_configuration, _logger)
        {
            PlanDictionary = new Dictionary<int, Plan>();
        }

        public async Task<IEnumerable<Plan>> GetActives()
        {
            try
            {
                string query = @"Select
                                    plan.id
	                                , plan.recordid
	                                , plan.code
	                                , plan.name
	                                , plan.description
	                                , plan.discount
	                                , plan.discountPercent
	                                , plan.active
	                                , class.id
	                                , class.recordid
	                                , class.code
	                                , class.name
	                                , class.description
	                                , class.value
                                From plan
                                Inner Join planclass On plan.id = planclass.planid
                                Inner Join class On planclass.classid = class.id
                                Where plan.active = 1;";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Plan, Class, Plan>(query, MapPlanResults);
                return result.Distinct();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Plan> GetByRecordId(Guid recordId)
        {
            try
            {
                string query = @"Select
                                    plan.id
	                                , plan.recordid
	                                , plan.code
	                                , plan.name
	                                , plan.description
	                                , plan.discount
	                                , plan.discountPercent
	                                , plan.active
	                                , class.id
	                                , class.recordid
	                                , class.code
	                                , class.name
	                                , class.description
	                                , class.value
                                From plan
                                Inner Join planclass On plan.id = planclass.planid
                                Inner Join class On planclass.classid = class.id
                                Where plan.active = 1
                                And recordid = @recordId;";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Plan, Class, Plan>(query, MapPlanResults);
                return result.Distinct().FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        private Plan MapPlanResults(
            Plan plan,
            Class classItem)
        {
            if (!PlanDictionary.TryGetValue(plan.Id.Value, out Plan planEntry))
            {
                planEntry = plan;
                planEntry.Classes = new List<Class>();
                PlanDictionary.Add(planEntry.Id.Value, planEntry);
            }

            if (classItem != null)
            {
                if (!planEntry.Classes.Any(x => x.Id.Equals(classItem.Id)))
                {
                    planEntry.Classes = planEntry.Classes.Concat(
                    new List<Class> { classItem });
                }
            }

            return planEntry;
        }
    }
}