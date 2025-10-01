namespace Chroma.Application.Interfaces;

public interface IDispatcher
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
}