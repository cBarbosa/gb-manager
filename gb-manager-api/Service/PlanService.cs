using gb_manager.Data;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Shared;
using gb_manager.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace gb_manager.Service
{
    public class PlanService : IPlanService
    {
        private readonly ILogger<PersonService> logger;
        private readonly Persistence persistencia;
        private readonly IPlanRepository repository;

        public PlanService(
            Persistence _persistencia
            , IPlanRepository _repository
            , ILogger<PersonService> _logger)
        {
            persistencia = _persistencia;
            logger = _logger;
            repository = _repository;
        }

        public async Task<CommandResult> GetActives()
        {
            var result = await repository.GetActives();

            return new CommandResult(
                result != null,
                (result != null
                    ? Messages.SUCCESS_QUERY
                    : Messages.ERROR_QUERY),
                result);
        }

        public async Task<CommandResult> GetByRecordId(Guid recordId)
        {
            var result = await repository.GetByRecordId(recordId);

            return new CommandResult(
                result != null,
                (result != null
                    ? Messages.SUCCESS_QUERY
                    : Messages.ERROR_QUERY),
                result);
        }

        public async Task<CommandResult> GetClassesByRecordId(Guid recordId)
        {
            if ((await GetByRecordId(recordId)).Data is not Domain.Models.Plan _plan)
            {
                logger.LogError($"{Messages.ERROR_PLAN_NOT_FOUND} {recordId}");
                return new CommandResult(false, Messages.ERROR_PLAN_NOT_FOUND, recordId);
            }

            var result = await repository.GetClassesById(_plan.Id.Value);

            return new CommandResult(
                result != null,
                (result != null
                    ? Messages.SUCCESS_QUERY
                    : Messages.ERROR_QUERY),
                result);
        }
    }
}