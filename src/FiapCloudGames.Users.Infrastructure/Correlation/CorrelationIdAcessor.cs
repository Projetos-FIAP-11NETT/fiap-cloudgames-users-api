using FiapCloudGames.Users.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace FiapCloudGames.Users.Infrastructure.Correlation
{
    public sealed class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CorrelationId =>
            Guid.TryParse(_httpContextAccessor.HttpContext?.Items["X-Correlation-Id"]?.ToString(), out var correlationId)
                    ? correlationId
                    : Guid.NewGuid();
    }
}
