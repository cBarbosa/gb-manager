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
            if (cmd.RecordId.HasValue)
                return await Update(cmd);

            var result = await repository.GetByDocument(cmd.Document);
            if (result != null)
            {
                logger.LogError($"{Messages.ERROR_PERSON_ALREADY_EXISTS} ({cmd.Document})");
                return new CommandResult(false, Messages.ERROR_PERSON_ALREADY_EXISTS, cmd);
            }

            try
            {
                var _person = BindPerson(cmd);
                _person.Active = false;

                var returnId = await Task.FromResult(persistencia.SalvarNaBase("person", _person, Persistence.PersistenceMetodos.Inserir));

                logger.LogInformation($"{Messages.SUCCESS_INSERT_PERSON} ({returnId})");

                return new CommandResult(true, Messages.SUCCESS_INSERT_PERSON, _person);
            }
            catch (Exception ex)
            {
                return new CommandResult(false, Messages.ERROR_INSERT_PERSON, ex);
            }
        }

        public async Task<CommandResult> Update(CreatePersonCommand cmd)
        {
            Person _person = BindPerson(cmd);
            _person.Active = cmd.Active;

            var _personRecorded = await repository.GetByRecordId(cmd.RecordId.Value);

            if (_personRecorded == null)
            {
                logger.LogError($"{Messages.ERROR_PERSON_NOT_EXISTS_RECORDID} ({cmd.RecordId})");
                return new CommandResult(false, Messages.ERROR_PERSON_NOT_EXISTS_RECORDID, cmd);
            }

            try
            {
                _ = await Task.FromResult(persistencia.SalvarNaBase("person", _person, Persistence.PersistenceMetodos.Alterar));

                logger.LogInformation($"{Messages.SUCCESS_UPDATE_PERSON} {_person.Document}");
                return new CommandResult(true, Messages.SUCCESS_UPDATE_PERSON, _person);
            }
            catch(Exception ex)
            {
                return new CommandResult(false, Messages.ERROR_UPDATE_PERSON, ex);
            }
        }

        public async Task<CommandResult> GetByLogin(string userName)
        {
            var result = await repository.GetByLogin(userName);

            return new CommandResult(
                result!=null,
                (result != null
                    ? Messages.SUCCESS_QUERY
                    : Messages.ERROR_QUERY),
                result);
        }

        public async Task<CommandResult> GetByDocument(string document)
        {
            var result = await repository.GetByDocument(document);

            return new CommandResult(
                result != null,
                (result != null
                    ? Messages.SUCCESS_QUERY
                    : Messages.ERROR_QUERY),
                result.ToList());
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

        private static Person BindPerson(CreatePersonCommand cmd)
        {
            return new Person
            {
                //Id = cmd.Id,
                RecordId = !cmd.RecordId.HasValue ? Guid.NewGuid() : null,
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
                Profile = cmd.Profile,
                //Active = true
            };
        }
    }
}
