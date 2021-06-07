using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data
{
    public class ContractRepository
        : PersistenceBase<Contract>, IContractRepository
    {

        public ContractRepository(
            IConfiguration _configuration
            , ILogger<Contract> _logger)
            : base(_configuration, _logger)
        {

        }

        public async Task<Contract> Create(Contract contract)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Contract>> GetByPersonId(int id)
        {
            throw new NotImplementedException();
        }
    }
}