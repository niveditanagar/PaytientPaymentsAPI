using Microsoft.AspNetCore.Mvc;
using PaytientPaymentsAPI.Data;
using PaytientPaymentsAPI.Models;
using PaytientPaymentsAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaytientPaymentsAPI.Repository
{
    public class PersonsRepo : IPersonsRepo
    {
        private readonly PaytientPaymentsAPIDbContext _dbContext;

        public PersonsRepo(PaytientPaymentsAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PersonsModel> Post(AddPersonRequestModel addPersonRequest)
        {
            var personModel = new PersonsModel()
            {
                FirstName = addPersonRequest.FirstName,
                LastName = addPersonRequest.LastName
            };

            await _dbContext.Persons.AddAsync(personModel);
            await _dbContext.SaveChangesAsync();

            return personModel;
            
        }
    }
}
