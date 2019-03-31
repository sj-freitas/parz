namespace Parz.Models
{
    public class Token : IToken
    {
        public int Level { get; set; }

        public string Symbol { get; set; }

        public TokenType TokenType { get; set; }
    }
}
