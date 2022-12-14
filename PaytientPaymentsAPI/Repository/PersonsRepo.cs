using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<PersonsModel> CreatePerson(string LastName, string FirstName)
        {
            var personModel = new PersonsModel()
            {
                FirstName = FirstName,
                LastName = LastName
            };

            await _dbContext.Persons.AddAsync(personModel);
            await _dbContext.SaveChangesAsync();

            return personModel;
            
        }

        public async Task<bool> PersonExists(int personId)
        {
            return await _dbContext.Persons.AnyAsync(x => x.Id == personId);
        }
    }
}
