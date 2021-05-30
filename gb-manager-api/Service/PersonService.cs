using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using gb_manager.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace gb_manager.Service
{
    public class PersonService : IPersonService
    {

        private readonly ILogger<PersonService> logger;
        private readonly IPersonRepository repository;

        public PersonService(
            ILogger<PersonService> _logger
            , IPersonRepository _repository)
        {
            logger = _logger;
            repository = _repository;
        }

        public async Task<Person> GetByLogin(string userName)
        {
            return await repository.GetByLogin(userName);
        }
    }
}
