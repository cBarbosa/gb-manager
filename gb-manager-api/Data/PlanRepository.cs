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
        private readonly Dictionary<int, Class> ClassDictionary;

        public PlanRepository(
            IConfiguration _configuration
            , ILogger<Plan> _logger)
            : base(_configuration, _logger)
        {
            PlanDictionary = new Dictionary<int, Plan>();
            ClassDictionary = new Dictionary<int, Class>();
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
                                And plan.recordid = @recordId;";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Plan, Class, Plan>(query, MapPlanResults, param: new { recordId });
                return result.Distinct().FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Class>> GetClassesById(int planId)
        {
            try
            {
                string query = @"Select
                                    class.id
	                                , class.recordid
	                                , class.code
	                                , class.name
	                                , class.description
	                                , class.value
	                                , grade.id
	                                , DATE_FORMAT(grade.start, '%H:%i') as start
	                                , DATE_FORMAT(grade.finish, '%H:%i') as finish
	                                , grade.weekday
				                From planclass
                                Inner join class On planclass.classid = class.id
                                Inner Join grade On class.id = grade.classid
                                Where planclass.planid =  @planId;";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Class, Grade, Class>(query, MapClassesResults, param: new{ planId });
                return result.Distinct();
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

        private Class MapClassesResults(
            Class classItem,
            Grade gradeItem)
        {
            if (!ClassDictionary.TryGetValue(classItem.Id.Value, out Class classEntry))
            {
                classEntry = classItem;
                classEntry.Grade = new List<Grade>();
                ClassDictionary.Add(classEntry.Id.Value, classEntry);
            }

            if (gradeItem != null)
            {
                if (!classEntry.Grade.Any(x => x.Id.Equals(gradeItem.Id)))
                {
                    classEntry.Grade = classEntry.Grade.Concat(
                    new List<Grade> { gradeItem });
                }
            }

            return classEntry;
        }
    }
}