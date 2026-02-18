using FiapCloudGames.Users.Application.Abstractions;
using FiapCloudGames.Users.Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;

namespace FiapCloudGames.Users.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICorrelationIdAccessor _correlation;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        ICorrelationIdAccessor correlation)
    {
        _logger = logger;
        _correlation = correlation;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var correlationId = _correlation.CorrelationId;
        var stopwatch = Stopwatch.StartNew();
        var safeRequest = DataMasker.Mask(request!);
        // New Relic
        var agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        var transaction = agent.CurrentTransaction;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            transaction.AddCustomAttribute("CorrelationId", correlationId);
            transaction.AddCustomAttribute("RequestName", typeof(TRequest).Name);
            transaction.AddCustomAttribute("request.payload", safeRequest);
        }
            // REQUEST
            _logger.LogInformation(
            "CorrelationId {CorrelationId} | Iniciando {RequestName} Parametros: {request.payload}",
            correlationId,
            requestName,
            safeRequest                
        );

        try
        {
            var response = await next();

            transaction.AddCustomAttribute("response.payload", response!);

            stopwatch.Stop();

            // RESPONSE
            _logger.LogInformation(
                "CorrelationId {CorrelationId} | Finalizando {RequestName} em {Elapsed}ms Response: {response.payload}",
                correlationId,
                requestName,
                stopwatch.ElapsedMilliseconds,
                response
            );

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "CorrelationId {CorrelationId} | Erro em {RequestName} em {Elapsed}ms",
                correlationId,
                requestName,
                stopwatch.ElapsedMilliseconds
            );

            throw;
        }
    }
}
