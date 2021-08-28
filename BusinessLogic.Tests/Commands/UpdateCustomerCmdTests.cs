using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    [TestFixture]
    public class UpdateCustomerCmdTests : BaseTests
    {
        [Test]
        public async Task Handler_Test()
        {
            // arrange
            var firstName = Guid.NewGuid().ToString();
            var lastName = Guid.NewGuid().ToString();
            var birthDate = DateTime.Now;
            var entity = await Execute(async sp =>
            {
                IDataStorage storage = sp.GetRequiredService<IDataStorage>();
                var item = await storage.AddAsync(new Customer
                {
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                    BirthDate = DateTime.Today
                });
                await storage.SaveAsync();
                return item;
            });

            var result = await Execute(async sp =>
            {
                IDataStorage storage = sp.GetRequiredService<IDataStorage>();
                var request = new UpdateCustomerCmd(entity.Id, firstName, lastName, birthDate);
                UpdateCustomerCmd.Handler handler = new UpdateCustomerCmd.Handler(storage);

                // act
                return await handler.Handle(request, default);
            });

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(entity.Id));
            Assert.That(result.FirstName, Is.EqualTo(firstName));
            Assert.That(result.LastName, Is.EqualTo(lastName));
            Assert.That(result.BirthDate, Is.EqualTo(birthDate));
        }

        [Test]
        public async Task Handler_CustomerDoesNotExist_Test()
        {
            // arrange
            var result = await Execute(async sp =>
            {
                IDataStorage storage = sp.GetRequiredService<IDataStorage>();
                var request = new UpdateCustomerCmd(100, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.Now);
                UpdateCustomerCmd.Handler handler = new UpdateCustomerCmd.Handler(storage);

                // act
                return await handler.Handle(request, default);
            });

            // assert
            Assert.That(result, Is.Null);
        }
    }
}
