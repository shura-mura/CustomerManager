using CustomerManager.BusinessLogic.Data.Entities;

namespace CustomerManager.Data.TestData
{
    public class MockEntity : IEntity<int>
    {
        public int Id { get; set; }
    }
}