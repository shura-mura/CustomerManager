using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomerManager.Data
{
    [TestFixture]
    public class DataStorageTests
    {
        private IDataStorage _instance;

        [SetUp]
        public void SetUp()
        {
            var dataStorage = new DataStorage(false);
            dataStorage.Database.OpenConnection();
            dataStorage.Database.EnsureCreated();
            _instance = dataStorage;
        }

        [TearDown]
        public void TearDown() => _instance?.Dispose();

        [Test]
        public void Ctor_Test()
        {
            // assert
            foreach (PropertyInfo info in typeof(DataStorage).GetProperties())
            {
                Type type = info.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DbSet<>))
                {
                    object value = info.GetValue(_instance);
                    Assert.That(value, Is.Not.Null);

                    PropertyInfo propertyInfo = typeof(IReadOnlyDataStorage).GetProperty(info.Name);
                    Assert.That(propertyInfo, Is.Not.Null, "Property: {0}", info.Name);
                }
            }
        }

        [Test]
        public async Task AddAsync_Test()
        {
            // act
            var entity = await _instance.AddAsync(CreateCustomer());

            // assert
            await _instance.SaveAsync();

            var result = _instance.Customers.SingleOrDefault(x => x.Id == entity.Id);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task FindAsync_Test()
        {
            // arrange
            var entity = await _instance.AddAsync(CreateCustomer());
            await _instance.SaveAsync();

            // act
            var result = await _instance.FindAsync<Customer>(entity.Id);

            // assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Remove_Test()
        {
            // arrange
            var entity = await _instance.AddAsync(CreateCustomer());
            await _instance.SaveAsync();

            entity = await _instance.FindAsync<Customer>(entity.Id);

            // act
            var result = _instance.Remove(entity);

            // assert
            await _instance.SaveAsync();
            Assert.That(result, Is.Not.Null);
            entity = await _instance.FindAsync<Customer>(entity.Id);
            Assert.That(entity, Is.Null);
        }

        [TestCase(true, "First Name")]
        [TestCase(false, "New First Name")]
        public async Task Readonly_Test(bool isReadonly, string expected)
        {
            // arrange
            Customer entity;

            using (IDataStorage instance = new DataStorage(false))
            {
                // act
                entity = await instance.AddAsync(CreateCustomer());
                await instance.SaveAsync();
            }

            using (var instance = new DataStorage(isReadonly))
            {
                // act
                entity = await instance.Customers.FirstAsync(x => x.Id == entity.Id);
                entity.FirstName = "New First Name";
                await instance.SaveAsync();
            }

            // assert
            entity = await _instance.FindAsync<Customer>(entity.Id);
            Assert.That(entity.FirstName, Is.EqualTo(expected));
        }

        private static Customer CreateCustomer() => new Customer { FirstName = "First Name", LastName = "Last Name", BirthDate = DateTime.Today };
    }
}