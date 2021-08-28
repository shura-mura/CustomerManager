using CustomerManager.BusinessLogic.Data;
using CustomerManager.Data.TestData;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerManager.Data
{
    [TestFixture]
    public class EntityCollectionTests
    {
        [Test]
        public void CtorReadonly_Test()
        {
            // arrange
            Type type = GetType();
            Expression expression = Expression.Empty();
            IQueryProvider queryProvider = Mock.Of<IQueryProvider>();

            Mock<IQueryable<MockEntity>> mockQueryable = new Mock<IQueryable<MockEntity>>();
            mockQueryable.Setup(x => x.ElementType).Returns(type);
            mockQueryable.Setup(x => x.Expression).Returns(expression);
            mockQueryable.Setup(x => x.Provider).Returns(queryProvider);

            // act
            EntityCollection<MockEntity> instance = new EntityCollection<MockEntity>(mockQueryable.Object, true);

            // assert
            Assert.That(instance.ElementType, Is.EqualTo(type));
            Assert.That(instance.Expression, Is.EqualTo(expression));
            Assert.That(instance.Provider, Is.EqualTo(queryProvider));
        }

        [Test]
        public void GetEnumeratorGeneric_Test()
        {
            // arrange
            Mock<IQueryable<MockEntity>> mockQueryable = new Mock<IQueryable<MockEntity>>();
            IEnumerator<MockEntity> enumerator = Mock.Of<IEnumerator<MockEntity>>();
            mockQueryable.Setup(x => x.GetEnumerator()).Returns(enumerator).Verifiable();
            EntityCollection<MockEntity> instance = new EntityCollection<MockEntity>(mockQueryable.Object, false);

            // act
            IEnumerator<MockEntity> result = instance.GetEnumerator();

            // assert
            mockQueryable.Verify();
            Assert.That(result, Is.EqualTo(enumerator));
        }

        [Test]
        public void GetEnumerator_Test()
        {
            // arrange
            Mock<IQueryable<MockEntity>> mockQueryable = new Mock<IQueryable<MockEntity>>();
            IEnumerator enumerator = Mock.Of<IEnumerator>();
            mockQueryable.As<IEnumerable>().Setup(x => x.GetEnumerator()).Returns(enumerator).Verifiable();
            EntityCollection<MockEntity> instance = new EntityCollection<MockEntity>(mockQueryable.Object, false);

            // act
            IEnumerator result = ((IEnumerable)instance).GetEnumerator();

            // assert
            mockQueryable.Verify();
            Assert.That(result, Is.EqualTo(enumerator));
        }

        [Test]
        public void Include_Test()
        {
            // arrange
            Mock<IQueryable<MockEntity>> mockQueryable = new Mock<IQueryable<MockEntity>>();
            EntityCollection<MockEntity> instance = new EntityCollection<MockEntity>(mockQueryable.Object, false);

            // act
            IEntityCollection<MockEntity> result = instance.Include(x => x.Id);

            // assert
            Assert.That(result, Is.Not.Null & Is.InstanceOf<EntityCollection<MockEntity>>());
        }
    }
}