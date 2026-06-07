namespace DustInTheWind.ConsoleTools.Commando;

internal static class StringExtensions
{
    public static IEnumerable<string> ToLowerCaseWords(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            yield break;

        int startIndex = -1;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (!char.IsLetter(c))
            {
                if (startIndex != -1)
                {
                    int wordLength = i - startIndex;
                    yield return text.Substring(startIndex, wordLength).ToLower();
                    startIndex = -1;
                }
            }
            else if (char.IsUpper(c))
            {
                if (startIndex != -1)
                {
                    int wordLength = i - startIndex;
                    yield return text.Substring(startIndex, wordLength).ToLower();
                }

                startIndex = i;
            }
        }

        if (startIndex != -1)
            yield return text[startIndex..].ToLower();
    }

    public static string ToKebabCase(this string text)
    {
        IEnumerable<string> words = text.ToLowerCaseWords();
        return string.Join('-', words);
    }
}