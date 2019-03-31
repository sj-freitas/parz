using Parz.Models;

namespace Parz.Factories
{
    public interface ITokenFactory
    {
        IToken ToToken(ILeveledToken token);
    }
}
