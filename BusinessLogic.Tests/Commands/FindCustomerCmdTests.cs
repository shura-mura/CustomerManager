using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    [TestFixture]
    public class FindCustomerCmdTests : BaseTests
    {
        [TestCase("ir", "random", 2)]
        [TestCase("random", "ix", 1)]
        [TestCase("ir", "ix", 3)]
        public async Task Handler_Test(string firstName, string lastName, int expected)
        {
            // arrange
            await Execute(async sp =>
            {
                IDataStorage storage = sp.GetRequiredService<IDataStorage>();
                await storage.AddAsync(new Customer
                {
                    FirstName = "First",
                    LastName = "Second"
                });
                await storage.AddAsync(new Customer
                {
                    FirstName = "Third",
                    LastName = "Four"
                });
                await storage.AddAsync(new Customer
                {
                    FirstName = "Five",
                    LastName = "Six"
                });
                await storage.SaveAsync();
            });

            var result = await Execute(async sp =>
            {
                IReadOnlyDataStorage storage = sp.GetRequiredService<IReadOnlyDataStorage>();
                var request = new FindCustomerCmd(firstName, lastName);
                FindCustomerCmd.Handler handler = new FindCustomerCmd.Handler(storage);

                // act
                return await handler.Handle(request, default);
            });
            

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Length.EqualTo(expected));
        }
    }
}