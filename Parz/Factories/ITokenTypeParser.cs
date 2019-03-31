namespace Parz.Factories
{
    public interface ITokenTypeParser
    {
        bool TryParse(string symbol, out TokenType tokenType,
            out string adapted);
    }
}
