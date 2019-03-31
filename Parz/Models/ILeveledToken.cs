namespace Parz.Models
{
    public interface ILeveledToken
    {
        string Token { get; }

        int Level { get; }
    }
}
