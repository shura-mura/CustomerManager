using System;

namespace CustomerManager.BusinessLogic.Data.Entities
{
    public class Customer : IEntity<int>
    {
        public int Id { get; protected set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}