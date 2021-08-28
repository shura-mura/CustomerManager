using CustomerManager.BusinessLogic.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CustomerManager.Data
{
    public class DataStorageFactory : IDataStorageFactory
    {
        private DataStorage _rootInstance;

        public DataStorageFactory() => _rootInstance = new DataStorage(true);

        ~DataStorageFactory() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CreateStorage()
        {
            _rootInstance.Database.OpenConnection();
            _rootInstance.Database.EnsureCreated();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _rootInstance.Dispose();
                _rootInstance = null;
            }
        }
    }
}