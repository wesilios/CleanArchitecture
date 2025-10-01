namespace Chroma.Application.Handlers.Abstractions;

public interface ICommandHandler<in TCommand>
{
    Task HandleAsync(TCommand command);
}