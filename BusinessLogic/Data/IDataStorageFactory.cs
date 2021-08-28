using System;

namespace CustomerManager.BusinessLogic.Data
{
    public interface IDataStorageFactory : IDisposable
    {
        void CreateStorage();
    }
}