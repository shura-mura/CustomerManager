using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    [TestFixture]
    public class CreateCustomerCmdTests : BaseTests
    {
        [Test]
        public async Task Handler_Test()
        {
            // arrange
            var firstName = Guid.NewGuid().ToString();
            var lastName = Guid.NewGuid().ToString();
            var birthDate = DateTime.Today;

            var result = await Execute(async sp =>
            {
                var request = new CreateCustomerCmd(firstName, lastName, birthDate);
                CreateCustomerCmd.Handler handler = new CreateCustomerCmd.Handler(sp.GetService<IDataStorage>());

                // act
                return await handler.Handle(request, default);
            });

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.GreaterThanOrEqualTo(1));

            await Execute(sp =>
            {
                IReadOnlyDataStorage storage = sp.GetRequiredService<IReadOnlyDataStorage>();
                var entity = storage.FindAsync<Customer>(result.Id);
                Assert.That(result.FirstName, Is.EqualTo(firstName));
                Assert.That(result.LastName, Is.EqualTo(lastName));
                Assert.That(result.BirthDate, Is.EqualTo(birthDate));
                return Task.CompletedTask;
            });
        }
    }
}