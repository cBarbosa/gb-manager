using gb_manager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gb_manager.Data.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetByLogin(string userName);
        Task<Person> GetByRecordId(Guid recordId);
        Task<IEnumerable<Person>> GetByDocument(string document);
        Task<IEnumerable<Person>> GetByName(string name);
    }
}