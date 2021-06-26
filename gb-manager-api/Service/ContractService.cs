using gb_manager.Data;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using gb_manager.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace gb_manager.Service
{
    public class ContractService : IContractService
    {
        private readonly ILogger<ContractService> logger;
        private readonly IContractRepository repository;
        private readonly Persistence persistencia;
        private readonly IPlanService planService;
        private readonly IPersonService personService;

        public ContractService(
            ILogger<ContractService> _logger
            , Persistence _persistencia
            , IPlanService _planService
            , IPersonService _personService
            , IContractRepository _repository)
        {
            logger = _logger;
            repository = _repository;
            persistencia = _persistencia;
            planService = _planService;
            personService = _personService;
        }

        public async Task<CommandResult> Create(CreateContractCommand cmd)
        {
            if (!cmd.PersonRecordId.HasValue || !cmd.PlanRecordId.HasValue)
            {
                logger.LogError($"{Messages.ERROR_PARAM_NOT_FOUND} {new[] { "Contract.PersonRecordId", "Contract.PlanId" }}");
                return new CommandResult(false, Messages.ERROR_PARAM_NOT_FOUND, new[] { "Contract.PersonId", "Contract.PlanId" });
            }

            var _planData = (await planService.GetByRecordId(cmd.PlanRecordId.Value)).Data as Plan;
            if (_planData == null)
            {
                logger.LogError($"{Messages.ERROR_PLAN_NOT_FOUND} {cmd.PlanRecordId}");
                return new CommandResult(false, Messages.ERROR_PLAN_NOT_FOUND, cmd.PlanRecordId);
            }

            var _personData = (await personService.GetByRecordId(cmd.PersonRecordId.Value)).Data as Person;
            if (_personData == null)
            {
                logger.LogError($"{Messages.ERROR_PERSON_NOT_EXISTS_RECORDID} {cmd.PersonRecordId}");
                return new CommandResult(false, Messages.ERROR_PERSON_NOT_EXISTS_RECORDID, cmd.PersonRecordId);
            }

            var _installments = (_planData.Code?.ToUpper().Substring(0, 1)) switch
            {
                "T" => 3,
                "S" => 6,
                "A" => 12,
                _ => 1,
            };

            var _contract = new Contract
            {
                RecordId = System.Guid.NewGuid(),
                PersonId = _personData.Id,
                PlanId = _planData.Id,
                Amount = cmd.Amount,
                BillingDay = cmd.BillingDay,
                Installments = _installments,
                Starts = cmd.Starts,
                Ends = cmd.Ends,
                Active = false
            };
            //var _amount = _plaData.
            var recordId = await Task.FromResult(
                persistencia.SalvarNaBase(
                    "contract",
                    _contract,
                    Persistence.PersistenceMetodos.Inserir));

            logger.LogInformation($"{Messages.SUCCESS_INSERT_CONTRACT}, {_contract.RecordId}");

            return new CommandResult(true, Messages.SUCCESS_INSERT_CONTRACT, _contract);
        }

        public async Task<CommandResult> Update(CreateContractCommand cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}