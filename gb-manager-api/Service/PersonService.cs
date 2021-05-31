using gb_manager.Data;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using gb_manager.Domain.Shared;
using gb_manager.Domain.Shared.Command;
using gb_manager.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace gb_manager.Service
{
    public class PersonService : IPersonService
    {

        private readonly ILogger<PersonService> logger;
        private readonly IPersonRepository repository;
        private readonly Persistence persistencia;

        public PersonService(
            Persistence _persistencia
            , ILogger<PersonService> _logger
            , IPersonRepository _repository)
        {
            persistencia = _persistencia;
            logger = _logger;
            repository = _repository;
        }

        public async Task<CommandResult> Create(CreatePersonCommand cmd)
        {
            if (cmd.Id.HasValue)
                return await Update(cmd);

            var result = await repository.GetByDocument(cmd.Document);
            var _personData = result.FirstOrDefault();

            if (_personData != null)
            {
                logger.LogInformation($"Cadastro será atualizado, documento {_personData.Document}");
                cmd.Id = _personData.Id;
                return await Update(cmd);
            }

            var _person = BindPerson(cmd);
            _person.Active = true;

            _person.Id = await Task.FromResult(persistencia.SalvarNaBase("person", _person, Persistence.PersistenceMetodos.Inserir));

            logger.LogInformation($"Cadastrado com sucesso, documento {_person.Document}");

            return new CommandResult(true, $"Cadastrado com sucesso", _person);
        }

        public async Task<CommandResult> Update(CreatePersonCommand cmd)
        {
            Person _person = BindPerson(cmd);

            await Task.FromResult(persistencia.SalvarNaBase("person", _person, Persistence.PersistenceMetodos.Alterar));

            logger.LogInformation($"Atualizado com sucesso, documento = {_person.Document}");

            return new CommandResult(true, "Cadastrado com sucesso", _person);
        }

        public async Task<CommandResult> GetByLogin(string userName)
        {
            return new CommandResult(true, "Dados carregados com sucesso", await repository.GetByLogin(userName));
        }

        public async Task<CommandResult> GetByDocument(string document)
        {
            return new CommandResult(true, "Dados carregados com sucesso", await repository.GetByDocument(document));
        }

        private static Person BindPerson(CreatePersonCommand cmd)
        {
            return new Person
            {
                Id = cmd.Id,
                RecordId = !cmd.Id.HasValue ? Guid.NewGuid() : null,
                Name = cmd.Name,
                Document = cmd.Document,
                Email = cmd.Email,
                Gender = cmd.Gender,
                BirthDate = cmd.BirthDate,
                Phone = cmd.Phone,
                ZipCode = cmd.ZipCode,
                Street = cmd.Street,
                Number = cmd.Number,
                Neighborhood = cmd.Neighborhood,
                City = cmd.City,
                FederativeUnit = cmd.FederativeUnit,
                Complement = cmd.Complement,
                Profile = cmd.Profile
            };
        }
    }
}
