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

        public ContractService(
            ILogger<ContractService> _logger
            , Persistence _persistencia
            , IPlanService _planService
            , IContractRepository _repository)
        {
            logger = _logger;
            repository = _repository;
            persistencia = _persistencia;
            planService = _planService;
        }

        public async Task<CommandResult> Create(CreateContractCommand cmd)
        {
            if (!cmd.PersonId.HasValue || !cmd.PlanId.HasValue)
                return new CommandResult(false, $"Parametro não encontrado", null);

            var _plan = await planService.GetById(cmd.PlanId.Value);
            var _plaData = (Plan)_plan.Data;

            var _installments = (_plaData.Code?.ToUpper().Substring(0, 1))switch
            {
                "T" => 3,
                "S" => 6,
                "A" => 12,
                _ => 1,
            };

            var _contract = new Contract
            {
                PersonId = cmd.PersonId,
                PlanId = cmd.PlanId,
                RecordId = System.Guid.NewGuid(),
                Amount = cmd.Amount,
                BillingDay = cmd.BillingDay,
                Installments = _installments,
                Starts = cmd.Starts,
                Ends = cmd.Ends,
                Active = false
            };
            //var _amount = _plaData.
            _contract.Id = await Task.FromResult(persistencia.SalvarNaBase("contract", _contract, Persistence.PersistenceMetodos.Inserir));

            logger.LogInformation($"Cadastrado com sucesso, {_contract.RecordId}");

            return new CommandResult(true, $"Cadastrado com sucesso", _contract);
        }

        public async Task<CommandResult> Update(CreateContractCommand cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}