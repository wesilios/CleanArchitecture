namespace CleanArchitecture.Application.Handlers.Abstractions;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}