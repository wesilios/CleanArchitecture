namespace CleanArchitecture.Application.DataObjects;

public interface IDto
{
    string Note { get; set; }
    bool Empty { get; set; }
}