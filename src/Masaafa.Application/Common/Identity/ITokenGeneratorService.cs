using Masaafa.Domain.Entities;

namespace Masaafa.Application.Common.Identity;

public interface ITokenGeneratorService
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}