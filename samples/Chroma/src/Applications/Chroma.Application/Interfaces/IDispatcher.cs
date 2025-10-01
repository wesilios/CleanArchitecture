using Chroma.Application.Commands;
using Chroma.Application.Queries;

namespace Chroma.Application.Interfaces;

public interface IDispatcher
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
}