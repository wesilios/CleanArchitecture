using CleanArchitecture.Application.Commands;
using CleanArchitecture.Application.Handlers.Abstractions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    // Method for Commands (Write Operations)
    public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        // Use GetRequiredService to ensure handler registration failure is clear
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return handler.HandleAsync(command); // Assuming your handlers are async
    }

    // Method for Queries (Read Operations)
    public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
    {
        // The service provider resolves the specific handler based on the input query and expected result.
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return handler.HandleAsync(query); // Assuming your handlers are async
    }
}