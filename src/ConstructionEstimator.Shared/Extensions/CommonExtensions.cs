namespace ConstructionEstimator.Shared.Extensions;

public static class DecimalExtensions
{
    /// <summary>
    /// Format Vietnamese currency (VND)
    /// </summary>
    public static string ToVietnameseCurrency(this decimal value)
    {
        return $"{value:N0} VND";
    }
    
    /// <summary>
    /// Format with Vietnamese number formatting
    /// </summary>
    public static string ToVietnameseNumber(this decimal value, int decimals = 0)
    {
        return value.ToString($"N{decimals}");
    }
}

public static class DateTimeExtensions
{
    /// <summary>
    /// Format Vietnamese date
    /// </summary>
    public static string ToVietnameseDate(this DateTime date)
    {
        return date.ToString("dd/MM/yyyy");
    }
    
    /// <summary>
    /// Format Vietnamese datetime
    /// </summary>
    public static string ToVietnameseDateTime(this DateTime date)
    {
        return date.ToString("dd/MM/yyyy HH:mm");
    }
    
    /// <summary>
    /// Format Vietnamese full datetime
    /// </summary>
    public static string ToVietnameseFullDateTime(this DateTime date)
    {
        return date.ToString("dddd, dd/MM/yyyy HH:mm:ss");
    }
}

public static class StringExtensions
{
    /// <summary>
    /// Check if string is null or empty with Vietnamese-specific validation
    /// </summary>
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
    
    /// <summary>
    /// Capitalize first letter (useful for Vietnamese names)
    /// </summary>
    public static string CapitalizeFirst(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;
            
        return char.ToUpper(value[0]) + value[1..].ToLower();
    }
    
    /// <summary>
    /// Remove Vietnamese diacritics for search functionality
    /// </summary>
    public static string RemoveVietnameseDiacritics(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Vietnamese character mapping
        var vietnameseChars = new Dictionary<char, char>
        {
            {'á', 'a'}, {'à', 'a'}, {'ả', 'a'}, {'ã', 'a'}, {'ạ', 'a'},
            {'ă', 'a'}, {'ắ', 'a'}, {'ằ', 'a'}, {'ẳ', 'a'}, {'ẵ', 'a'}, {'ặ', 'a'},
            {'â', 'a'}, {'ấ', 'a'}, {'ầ', 'a'}, {'ẩ', 'a'}, {'ẫ', 'a'}, {'ậ', 'a'},
            {'é', 'e'}, {'è', 'e'}, {'ẻ', 'e'}, {'ẽ', 'e'}, {'ẹ', 'e'},
            {'ê', 'e'}, {'ế', 'e'}, {'ề', 'e'}, {'ể', 'e'}, {'ễ', 'e'}, {'ệ', 'e'},
            {'í', 'i'}, {'ì', 'i'}, {'ỉ', 'i'}, {'ĩ', 'i'}, {'ị', 'i'},
            {'ó', 'o'}, {'ò', 'o'}, {'ỏ', 'o'}, {'õ', 'o'}, {'ọ', 'o'},
            {'ô', 'o'}, {'ố', 'o'}, {'ồ', 'o'}, {'ổ', 'o'}, {'ỗ', 'o'}, {'ộ', 'o'},
            {'ơ', 'o'}, {'ớ', 'o'}, {'ờ', 'o'}, {'ở', 'o'}, {'ỡ', 'o'}, {'ợ', 'o'},
            {'ú', 'u'}, {'ù', 'u'}, {'ủ', 'u'}, {'ũ', 'u'}, {'ụ', 'u'},
            {'ư', 'u'}, {'ứ', 'u'}, {'ừ', 'u'}, {'ử', 'u'}, {'ữ', 'u'}, {'ự', 'u'},
            {'ý', 'y'}, {'ỳ', 'y'}, {'ỷ', 'y'}, {'ỹ', 'y'}, {'ỵ', 'y'},
            {'đ', 'd'},
            {'Á', 'A'}, {'À', 'A'}, {'Ả', 'A'}, {'Ã', 'A'}, {'Ạ', 'A'},
            {'Ă', 'A'}, {'Ắ', 'A'}, {'Ằ', 'A'}, {'Ẳ', 'A'}, {'Ẵ', 'A'}, {'Ặ', 'A'},
            {'Â', 'A'}, {'Ấ', 'A'}, {'Ầ', 'A'}, {'Ẩ', 'A'}, {'Ẫ', 'A'}, {'Ậ', 'A'},
            {'É', 'E'}, {'È', 'E'}, {'Ẻ', 'E'}, {'Ẽ', 'E'}, {'Ẹ', 'E'},
            {'Ê', 'E'}, {'Ế', 'E'}, {'Ề', 'E'}, {'Ể', 'E'}, {'Ễ', 'E'}, {'Ệ', 'E'},
            {'Í', 'I'}, {'Ì', 'I'}, {'Ỉ', 'I'}, {'Ĩ', 'I'}, {'Ị', 'I'},
            {'Ó', 'O'}, {'Ò', 'O'}, {'Ỏ', 'O'}, {'Õ', 'O'}, {'Ọ', 'O'},
            {'Ô', 'O'}, {'Ố', 'O'}, {'Ồ', 'O'}, {'Ổ', 'O'}, {'Ỗ', 'O'}, {'Ộ', 'O'},
            {'Ơ', 'O'}, {'Ớ', 'O'}, {'Ờ', 'O'}, {'Ở', 'O'}, {'Ỡ', 'O'}, {'Ợ', 'O'},
            {'Ú', 'U'}, {'Ù', 'U'}, {'Ủ', 'U'}, {'Ũ', 'U'}, {'Ụ', 'U'},
            {'Ư', 'U'}, {'Ứ', 'U'}, {'Ừ', 'U'}, {'Ử', 'U'}, {'Ữ', 'U'}, {'Ự', 'U'},
            {'Ý', 'Y'}, {'Ỳ', 'Y'}, {'Ỷ', 'Y'}, {'Ỹ', 'Y'}, {'Ỵ', 'Y'},
            {'Đ', 'D'}
        };

        var result = text;
        foreach (var kvp in vietnameseChars)
        {
            result = result.Replace(kvp.Key, kvp.Value);
        }

        return result;
    }
}