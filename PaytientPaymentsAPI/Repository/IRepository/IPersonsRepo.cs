using PaytientPaymentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Repository.IRepository
{
    public interface IPersonsRepo
    {
        Task<PersonsModel> CreatePerson(string LastName, string FirstName);

        Task<bool> PersonExists(int personId);
    }
}
