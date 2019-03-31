namespace Parz.Models
{
    public interface IToken
    {
        int Level { get; }

        string Symbol { get; }

        TokenType TokenType { get; }
    }
}
