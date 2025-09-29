namespace Chroma.Domain.Exceptions;

public class UnsupportedColorException : Exception
{
    public UnsupportedColorException(string hexCode)
        : base($"Colour \"{hexCode}\" is unsupported.")
    {
    }
}