namespace Parz.Models
{
    public class LeveledToken : ILeveledToken
    {
        public string Token { get; set; }

        public int Level { get; set; }
    }
}
