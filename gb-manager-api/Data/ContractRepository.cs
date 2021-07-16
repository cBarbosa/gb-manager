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
    public class ContractRepository
        : PersistenceBase<Contract>, IContractRepository
    {
        private readonly Dictionary<int, Contract> ContractDictionary;
        private readonly Dictionary<int, Installment> InstallmentDictionary;

        public ContractRepository(
            IConfiguration _configuration
            , ILogger<Contract> _logger)
            : base(_configuration, _logger)
        {
            ContractDictionary = new Dictionary<int, Contract>();
            InstallmentDictionary = new Dictionary<int, Installment>();
        }

        public async Task<IEnumerable<Contract>> GetByPersonRecordId(Guid personRecordId)
        {
            try
            {
                string query = @"select
                                    contract.id
                                    , contract.recordid
                                    , contract.amount
                                    , contract.discountpercent
                                    , contract.billingday
                                    , contract.montlyamount
                                    , contract.installments
                                    , contract.starts
                                    , contract.ends
                                    , contract.active
                                    , plan.id
                                    , plan.recordid
                                    , plan.code
                                    , plan.name
                                    , plan.description
                                    , plan.discount
                                    , plan.discountPercent
                                    , plan.active
                                    , person.id
                                    , person.recordid
                                    , person.name
                                    , person.document
                                    , person.gender
                                    , person.active
                                    , persons.id
                                    , persons.recordid
                                    , persons.name
                                    , persons.document
                                    , persons.gender
                                    , persons.active
                                    , installment.id
                                    , installment.recordid
                                    , installment.type
                                    , installment.duedate
                                    , installment.amount
                                    , installment.receipt
                                    , transaction.id
                                From contract
                                Inner join plan on contract.planid = plan.id
                                Inner join person on contract.personid = person.id
                                Left join contractperson on contract.id = contractperson.contractid
                                Left join person As persons on contractperson.personid = persons.id
                                Left Join installment on contract.id = installment.contractid
                                Left Join transaction on contract.id = transaction.contractid
                            Where person.recordid = @personRecordId";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Contract, Plan, Person, Person, Installment, Transaction, Contract>(query, MapContractResults, param: new { personRecordId });

                return result.Distinct();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Contract> GetByRecordId(Guid recordId)
        {
            try
            {
                string query = @"select
                                    contract.id
                                    , contract.recordid
                                    , contract.amount
                                    , contract.discountpercent
                                    , contract.billingday
                                    , contract.montlyamount
                                    , contract.installments
                                    , contract.starts
                                    , contract.ends
                                    , contract.active
                                    , plan.id
                                    , plan.recordid
                                    , plan.code
                                    , plan.name
                                    , plan.description
                                    , plan.discount
                                    , plan.discountPercent
                                    , plan.active
                                    , person.id
                                    , person.recordid
                                    , person.name
                                    , person.document
                                    , persons.id
                                    , persons.recordid
                                    , persons.name
                                    , persons.document
                                    , installment.id
                                    , installment.recordid
                                    , installment.type
                                    , installment.duedate
                                    , installment.amount
                                    , installment.receipt
                                    , transaction.id
                                From contract
                                Inner join plan on contract.planid = plan.id
                                Inner join person on contract.personid = person.id
                                Left join contractperson on contract.id = contractperson.contractid
                                Left join person As persons on contractperson.personid = persons.id
                                Left Join installment on contract.id = installment.contractid
                                Left Join transaction on contract.id = transaction.contractid
                                Where contract.recordId = @recordId";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Contract, Plan, Person, Person, Installment, Transaction, Contract>(query, MapContractResults, param: new { recordId });

                return result.Distinct().FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Installment> GetInstallmentsByRecordId(Guid recordId)
        {
            try
            {
                string query = @"Select
                                    installment.id
                                    , installment.recordid
                                    , installment.type
                                    , installment.duedate
                                    , installment.amount
                                    , installment.receipt
                                    , contract.id
                                    , contract.recordid
                                    , contract.amount
                                    , contract.discountpercent
                                    , contract.billingday
                                    , contract.montlyamount
                                    , contract.installments
                                    , contract.starts
                                    , contract.ends
                                    , contract.active
                                    , plan.id
                                    , plan.recordid
                                    , plan.code
                                    , plan.name
                                    , plan.description
                                    , plan.discount
                                    , plan.discountPercent
                                    , plan.active
                                    , person.id
                                    , person.recordid
                                    , person.name
                                    , person.document
                                    , persons.id
                                    , persons.recordid
                                    , persons.name
                                    , persons.document
                                    From installment
                                    Inner join contract on installment.contractid = contract.id
                                    Inner join plan on contract.planid = plan.id
                                    Inner join person on contract.personid = person.id
                                    Left join contractperson on contract.id = contractperson.contractid
                                    Left join person As persons on contractperson.personid = persons.id
                                Where contract.recordId = @recordId";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Installment, Contract, Plan, Person, Person, Transaction, Installment>(query, MapInstallmentResults, param: new { recordId });
                return result.Distinct().FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        private Contract MapContractResults(
            Contract contract,
            Plan plan,
            Person person,
            Person persons,
            Installment installments,
            Transaction transactions)
        {
            if (!ContractDictionary.TryGetValue(contract.Id.Value, out Contract contractEntry))
            {
                contractEntry = contract;
                contractEntry.Persons = new List<Person>();
                contractEntry.InstallmentList = new List<Installment>();
                contractEntry.TransactionList = new List<Transaction>();
                contractEntry.Person = person;
                contractEntry.Plan = plan;
                ContractDictionary.Add(contractEntry.Id.Value, contractEntry);
            }

            if (persons != null)
            {
                if (!contractEntry.Persons.Any(x => x.Id.Equals(persons.Id)))
                {
                    contractEntry.Persons = contractEntry.Persons.Concat(
                    new List<Person> { persons });
                }
            }

            if (installments != null)
            {
                if (!contractEntry.InstallmentList.Any(x => x.Id.Equals(installments.Id)))
                {
                    contractEntry.InstallmentList = contractEntry.InstallmentList.Concat(
                    new List<Installment> { installments });
                }
            }

            if (transactions != null)
            {
                if (!contractEntry.TransactionList.Any(x => x.Id.Equals(transactions.Id)))
                {
                    contractEntry.TransactionList = contractEntry.TransactionList.Concat(
                    new List<Transaction> { transactions });
                }
            }

            return contractEntry;
        }

        private Installment MapInstallmentResults(
            Installment installment,
            Contract contract,
            Plan plan,
            Person person,
            Person persons,
            Transaction transactions)
        {
            if (!InstallmentDictionary.TryGetValue(contract.Id.Value, out Installment installmentEntry))
            {
                installmentEntry = installment;
                installmentEntry.Contract = contract;
                installmentEntry.Contract.Persons = new List<Person>();
                installmentEntry.Contract.TransactionList = new List<Transaction>();
                installmentEntry.Contract.Person = person;
                installmentEntry.Contract.Plan = plan;
                InstallmentDictionary.Add(installmentEntry.Id.Value, installmentEntry);
            }

            if (persons != null)
            {
                if (!installmentEntry.Contract.Persons.Any(x => x.Id.Equals(persons.Id)))
                {
                    installmentEntry.Contract.Persons = installmentEntry.Contract.Persons.Concat(
                    new List<Person> { persons });
                }
            }
            
            if (transactions != null)
            {
                if (!installmentEntry.Contract.TransactionList.Any(x => x.Id.Equals(transactions.Id)))
                {
                    installmentEntry.Contract.TransactionList = installmentEntry.Contract.TransactionList.Concat(
                    new List<Transaction> { transactions });
                }
            }

            return installmentEntry;
        }
    }
}