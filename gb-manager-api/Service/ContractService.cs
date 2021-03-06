using gb_manager.Data;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using gb_manager.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if ((await planService.GetByRecordId(cmd.PlanRecordId.Value)).Data is not Plan _planData)
            {
                logger.LogError($"{Messages.ERROR_PLAN_NOT_FOUND} {cmd.PlanRecordId}");
                return new CommandResult(false, Messages.ERROR_PLAN_NOT_FOUND, cmd.PlanRecordId);
            }

            if ((await personService.GetByRecordId(cmd.PersonRecordId.Value)).Data is not Person _personData)
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

            var _startContract = GetContractInitialDate(cmd);

            try
            {
                var _contract = new Contract
                {
                    RecordId = Guid.NewGuid(),
                    PersonId = _personData.Id,
                    PlanId = _planData.Id,
                    BillingDay = cmd.BillingDay,
                    Installments = _installments,
                    Starts = _startContract,
                    Ends = _startContract.AddMonths(_installments),
                    Active = false
                };

                _contract.Id = await Task.FromResult(
                    persistencia.SalvarNaBase(
                        "contract",
                        _contract,
                        Persistence.PersistenceMetodos.Inserir));
                logger.LogInformation($"{Messages.SUCCESS_INSERT_CONTRACT}, {_contract}");

                return new CommandResult(true, Messages.SUCCESS_INSERT_CONTRACT, _contract);
            }
            catch (Exception ex)
            {
                logger.LogError($"{Messages.ERROR_INSERT_CONTRACT} {ex}");
                return new CommandResult(false, Messages.ERROR_INSERT_CONTRACT, ex);
            }
        }

        public async Task<CommandResult> Update(CreateContractCommand cmd)
        {
            if ((await GetByRecordId(cmd.RecordId.Value)).Data is not Contract _contract)
            {
                logger.LogError($"{Messages.ERROR_CONTRACT_NOT_EXISTS_RECORDID} {cmd.RecordId}");
                return new CommandResult(false, Messages.ERROR_CONTRACT_NOT_EXISTS_RECORDID, cmd.RecordId);
            }

            if (_contract.Active)
            {
                logger.LogError($"{Messages.ERROR_UPDATE_CONTRACT_IS_ALREADY_ACTIVE} {cmd.RecordId}");
                return new CommandResult(false, Messages.ERROR_UPDATE_CONTRACT_IS_ALREADY_ACTIVE, cmd.PlanRecordId);
            }

            if ((await planService.GetClassesByRecordId(_contract.Plan.RecordId.Value)).Data is not IEnumerable<Class> _classes)
            {
                logger.LogError($"{Messages.ERROR_CLASSES_NOT_FOUND_TO_PLAN} {cmd.PlanRecordId}");
                return new CommandResult(false, Messages.ERROR_CLASSES_NOT_FOUND_TO_PLAN, cmd.PlanRecordId);
            }

            cmd.BillingDay = cmd.BillingDay.HasValue ? cmd.BillingDay : 10;
            cmd.Installments = _contract.Installments;
            cmd.Starts = GetContractInitialDate(cmd);
            cmd.Ends = cmd.Starts.Value.AddMonths(_contract.Installments.Value);
            _contract.DiscountPercent = cmd.DiscountPercent;

            if (!_contract.Active && cmd.Active)
            {
                _contract.Active = cmd.Active;
                _contract.Updated = DateTime.Now;
                _ = await GenerateInstallments(_contract);
            }

            UpdateAmount(_contract, _classes);

            try
            {
                _ = await Task.FromResult(
                persistencia.SalvarNaBase(
                    "contract",
                    _contract,
                    Persistence.PersistenceMetodos.Alterar));

                logger.LogInformation($"{Messages.SUCCESS_UPDATE_CONTRACT}, {cmd}");
                return new CommandResult(true, Messages.SUCCESS_UPDATE_CONTRACT, _contract);
            }
            catch (Exception ex)
            {
                logger.LogError($"{Messages.ERROR_UPDATE_CONTRACT} {ex}");
                return new CommandResult(false, Messages.ERROR_UPDATE_CONTRACT, ex);
            }
        }

        public async Task<CommandResult> AddPerson(
            Guid contractRecordId,
            Guid personRecordId)
        {
            if ((await GetByRecordId(contractRecordId)).Data is not Contract _contract)
            {
                logger.LogError($"{Messages.ERROR_CONTRACT_NOT_EXISTS_RECORDID} {contractRecordId}");
                return new CommandResult(false, Messages.ERROR_CONTRACT_NOT_EXISTS_RECORDID, contractRecordId);
            }

            if ((await personService.GetByRecordId(personRecordId)).Data is not Person _person)
            {
                logger.LogError($"{Messages.ERROR_PERSON_NOT_EXISTS_RECORDID} {personRecordId}");
                return new CommandResult(false, Messages.ERROR_PERSON_NOT_EXISTS_RECORDID, personRecordId);
            }

            try
            {
                _ = await Task.FromResult(
                    persistencia.SalvarNaBase(
                        "contractperson",
                        new ContractPerson
                        {
                            ContractId = _contract.Id,
                            PersonId = _person.Id
                        },
                        Persistence.PersistenceMetodos.Inserir));

                logger.LogInformation($"{Messages.SUCCESS_CONTRACT_ADD_PERSON}, {new { contractRecordId, personRecordId }}");
                return new CommandResult(true, Messages.SUCCESS_CONTRACT_ADD_PERSON, new { contractRecordId, personRecordId });
            }
            catch (Exception ex)
            {
                logger.LogError($"{Messages.ERROR_CONTRACT_ADD_PERSON} {ex}");
                return new CommandResult(false, Messages.ERROR_CONTRACT_ADD_PERSON, ex);
            }
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

        public async Task<CommandResult> GetByPersonRecordId(Guid personRecordId)
        {
            var result = await repository.GetByPersonRecordId(personRecordId);

            return new CommandResult(
                result != null,
                (result != null
                    ? Messages.SUCCESS_QUERY
                    : Messages.ERROR_QUERY),
                result);
        }

        public async Task<CommandResult> GetInstallmentsByRecordId(Guid recordId)
        {
            var result = await repository.GetInstallmentsByRecordId(recordId);

            return new CommandResult(
                result != null,
                (result != null
                    ? Messages.SUCCESS_QUERY
                    : Messages.ERROR_QUERY),
                result);
        }

        private static DateTime GetContractInitialDate(CreateContractCommand cmd)
        {
            var startContract = new DateTime(DateTime.Now.Year, DateTime.Now.Month, cmd.BillingDay.Value);

            if (cmd.Installments.Value.Equals(1))
                return DateTime.Today;

            if (cmd.BillingDay.Value > DateTime.Now.Day)
                startContract = startContract.AddMonths(1);

            return startContract;
        }

        private static void UpdateAmount(Contract _contract, IEnumerable<Class> _classes)
        {
            decimal amount = 0.0m;
            foreach (var classItem in _classes)
                amount += classItem.Value.Value - (_contract.DiscountPercent.HasValue ? classItem.Value.Value * (_contract.DiscountPercent.Value / 100) : 0);
            amount *= _contract.Persons.Count();

            decimal montlyAmount = (amount / _contract.Installments.Value);

            _contract.Amount = amount;
            _contract.MontlyAmount = montlyAmount;
        }

        private async Task<IEnumerable<Installment>> GenerateInstallments(Contract contract)
        {
            var installmentList = new List<Installment>();

            for (int i = 0; i < contract.Installments.Value; i++)
            {
                installmentList.Add(await CreateInstallment(new Installment
                {
                    RecordId = Guid.NewGuid(),
                    ContractId = contract.Id,
                    Amount = contract.MontlyAmount,
                    DueDate = contract.Starts.Value.AddMonths(i),
                    Type = "O"
                }));
            }

            return installmentList;
        }

        private async Task<Installment> CreateInstallment(Installment installment)
        {
            try
            {
                installment.Id = await Task.FromResult(
                    persistencia.SalvarNaBase(
                        "installment",
                        installment,
                        Persistence.PersistenceMetodos.Inserir));

                return installment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Billet> CreateBillet(Billet billet)
        {
            try
            {
                billet.Id = await Task.FromResult(
                    persistencia.SalvarNaBase(
                        "billet",
                        billet,
                        Persistence.PersistenceMetodos.Inserir));

                return billet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}