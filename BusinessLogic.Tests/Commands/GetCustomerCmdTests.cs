using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    [TestFixture]
    public class GetCustomerCmdTests : BaseTests
    {
        [Test]
        public async Task Handler_Test()
        {
            // arrange
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
                IReadOnlyDataStorage storage = sp.GetRequiredService<IReadOnlyDataStorage>();
                var request = new GetCustomerCmd(entity.Id);
                GetCustomerCmd.Handler handler = new GetCustomerCmd.Handler(storage);

                // act
                return await handler.Handle(request, default);
            });

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(entity.Id));
            Assert.That(result.FirstName, Is.EqualTo(entity.FirstName));
            Assert.That(result.LastName, Is.EqualTo(entity.LastName));
            Assert.That(result.BirthDate, Is.EqualTo(entity.BirthDate));
        }

        [Test]
        public async Task Handler_CustomerDoesNotExist_Test()
        {
            // arrange
            var result = await Execute(async sp =>
            {
                IReadOnlyDataStorage storage = sp.GetRequiredService<IDataStorage>();
                var request = new GetCustomerCmd(100);
                GetCustomerCmd.Handler handler = new GetCustomerCmd.Handler(storage);

                // act
                return await handler.Handle(request, default);
            });

            // assert
            Assert.That(result, Is.Null);
        }
    }
}