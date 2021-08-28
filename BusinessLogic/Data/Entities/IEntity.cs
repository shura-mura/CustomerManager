namespace CustomerManager.BusinessLogic.Data.Entities
{
	public interface IEntity
	{
	}

	public interface IEntity<out TId> : IEntity
	{
		TId Id { get; }
	}
}