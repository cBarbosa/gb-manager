using Dapper;
using gb_manager.Data.Interfaces;
using gb_manager.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data
{
    public class PersonRepository
        : PersistenceBase<Person>, IPersonRepository
    {

        public PersonRepository(
            IConfiguration _configuration
            , ILogger<Person> _logger)
            : base(_configuration, _logger)
        {

        }

        public async Task<IEnumerable<Person>> GetByDocument(string document)
        {
            try
            {
                string query = @"Select
	                            -- person.id
                                person.recordid
                                , person.name
                                , person.document
                                , person.birthdate
                                , person.gender
                                , person.email
                                , person.phone
                                , person.zipcode
                                , person.street
                                , person.number
                                , person.neighborhood
                                , person.city
                                , person.federativeunit
                                , person.complement
                                -- , person.password
                                , person.verified
                                , person.profile
                                , person.active
                                , person.created
                                , person.updated
                            From person
                            Where active = 1
                            And document Like CONCAT('%',@document,'%');";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                return await conexaoBD.QueryAsync<Person>(query, param: new { document });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Person> GetByLogin(string userName)
        {
            try
            {
                string query = @"Select
	                            -- person.id
                                person.recordid
                                , person.name
                                , person.document
                                , person.birthdate
                                , person.gender
                                , person.email
                                , person.phone
                                , person.zipcode
                                , person.street
                                , person.number
                                , person.neighborhood
                                , person.city
                                , person.federativeunit
                                , person.complement
                                , person.password
                                , person.verified
                                , person.profile
                                , person.active
                                , person.created
                                , person.updated
                            From person
                            Where active = 1
                            And LOWER(email) = LOWER(@userName)";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                return await conexaoBD.QueryFirstOrDefaultAsync<Person>(query, param: new { userName });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Person> GetByRecordId(Guid recordId)
        {
            try
            {
                string query = @"Select
	                            -- person.id
                                person.recordid
                                , person.name
                                , person.document
                                , person.birthdate
                                , person.gender
                                , person.email
                                , person.phone
                                , person.zipcode
                                , person.street
                                , person.number
                                , person.neighborhood
                                , person.city
                                , person.federativeunit
                                , person.complement
                                , person.password
                                , person.verified
                                , person.profile
                                , person.active
                                , person.created
                                , person.updated
                            From person
                            Where active = 1
                            And recordId = recordId";

                using var conexaoBD = new MySqlConnection(ConnectionString);
                return await conexaoBD.QueryFirstOrDefaultAsync<Person>(query, param: new { recordId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}