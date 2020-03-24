using com.apthai.APTimeStamp.Repositories.Interfaces;

namespace com.apthai.APTimeStamp.Repositories
{
    public interface IUnitOfWork
    {
        IMasterRepository MasterRepository { get; }
        //ISyncRepository SyncRepository { get; }
        IUserRepository UserRepository { get; }
        void Save();
    }
}