namespace Masaafa.Persistence.UnitOfWork.Interfaces;

public interface IUserContext
{
    Guid SystemId { get; }

    Guid? UserId { get; }

    Guid GetRequiredUserId();
}