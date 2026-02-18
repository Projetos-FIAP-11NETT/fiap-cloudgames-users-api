using FiapCloudGames.Users.Application.Abstractions;
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

        public string CorrelationId =>
            _httpContextAccessor.HttpContext?.Items["X-Correlation-Id"]?.ToString()
            ?? "N/A";
    }

}
