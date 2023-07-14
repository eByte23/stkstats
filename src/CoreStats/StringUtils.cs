using System.Text;
namespace StatSys.CoreStats;

public static class StringUtils
{
    private static string ToSlug(params string[] value)
    {
        if (value.Length == 0)
        {
            return string.Empty;
        }

        var result = new StringBuilder();

        foreach (var v in value)
        {
            if (string.IsNullOrWhiteSpace(v))
            {
                continue;
            }

            if (result.Length > 0)
            {
                result.Append("-");
            }

            var trimmedValue = v.Trim().ToLowerInvariant();

            foreach (var c in trimmedValue)
            {
                switch (c)
                {
                    case ' ':
                    case '_':
                    case '.':
                    case '/':
                    case '\\':
                    case '-':
                        result.Append('-');
                        break;
                    default:
                        if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                        {
                            result.Append(c);
                        }
                        break;
                }
            }
        }

        return result.ToString();
    }

    public static string TrimName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmedValue = value.Trim();

        string[] parts = trimmedValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
        {
            return trimmedValue;
        }

        return parts.Aggregate((x, y) => $"{x} {y}");
    }


    public static string GetShortId(Guid id, params string[] parts)
    {
        var last12 = new string(id.ToString().TakeLast(12).ToArray());

        // return string.Join("-", parts.Select(p => p.Replace("  ", " ").Replace(" ", "-")).Append(last12)).Replace("`", "").Replace("'", "").ToLower();
        var allParts = parts.Select(x => x).Append(last12).ToArray();

        return ToSlug(allParts);
    }

    public static string BuildName(string firstName, string lastName)
    {
        string fname = firstName;
        string lname = firstName;

        if (string.IsNullOrWhiteSpace(fname) && string.IsNullOrWhiteSpace(lname))
        {
            return string.Empty;
        }

        if (string.IsNullOrWhiteSpace(fname))
        {
            fname = "[MISSING]";
        }

        if (string.IsNullOrWhiteSpace(lname))
        {
            lname = "[MISSING]";
        }

        return $"{firstName} {lastName}".Split(' ', StringSplitOptions.RemoveEmptyEntries).Aggregate((x, y) => $"{x} {y}");
    }

}