using System.Text.RegularExpressions;

namespace CleanArchitecture.Application.Queries;

public abstract partial class SearchQuery<T> : IQuery<T>
{
    public string? SearchTerm
    {
        get => _searchTerm;
        init => _searchTerm = SanitizeSearchTerm(value);
    }

    private readonly string? _searchTerm;

    protected static string? SanitizeSearchTerm(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // Remove HTML tags
        input = HtmlTags().Replace(input, string.Empty);

        // Remove potentially dangerous characters - keep only safe ones
        input = QueryDangerousCharacter().Replace(input.Trim(), "");

        // Limit length
        return input.Length > 100 ? input[..100] : input;
    }

    protected static string? SanitizeText(string? input, int maxLength = 100)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // Basic HTML tag removal
        input = HtmlTags().Replace(input, string.Empty);

        // Remove control characters but keep more characters for general text
        input = ControlCharacters().Replace(input.Trim(), "");

        return input.Length > maxLength ? input[..maxLength] : input;
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex HtmlTags();

    [GeneratedRegex(@"[^a-zA-Z0-9\s\-_']")]
    private static partial Regex QueryDangerousCharacter();

    [GeneratedRegex(@"[\x00-\x1F\x7F]")]
    private static partial Regex ControlCharacters();
}