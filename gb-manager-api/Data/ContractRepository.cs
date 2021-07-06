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

        public ContractRepository(
            IConfiguration _configuration
            , ILogger<Contract> _logger)
            : base(_configuration, _logger)
        {
            ContractDictionary = new Dictionary<int, Contract>();
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
                                From contract
                                Inner join plan on contract.planid = plan.id
                                Inner join person on contract.personid = person.id
                                Left join contractperson on contract.id = contractperson.contractid
                                Left join person As persons on contractperson.personid = persons.id
                            Where person.recordid = @personRecordId";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Contract, Plan, Person, Person, Contract>(query, MapContractResults, param: new { personRecordId });

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
                                From contract
                                Inner join plan on contract.planid = plan.id
                                Inner join person on contract.personid = person.id
                                Left join contractperson on contract.id = contractperson.contractid
                                Left join person As persons on contractperson.personid = persons.id
                                Where contract.recordId = @recordId";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                var result = await conexaoBD.QueryAsync<Contract, Plan, Person, Person, Contract>(query, MapContractResults, param: new { recordId });

                return result.Distinct().FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public Task<Installment> GetInstallmentsByRecordId(Guid recordId)
        {
            throw new NotImplementedException();
        }

        private Contract MapContractResults(
            Contract contract,
            Plan plan,
            Person person,
            Person persons)
        {
            if (!ContractDictionary.TryGetValue(contract.Id.Value, out Contract contractEntry))
            {
                contractEntry = contract;
                contractEntry.Persons = new List<Person>();
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

            return contractEntry;
        }
    }
}