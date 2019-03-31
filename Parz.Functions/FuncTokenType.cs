namespace Parz.Functions
{
    public class FuncTokenType
    {
        /// <summary>
        /// A token that symbolizes a function name.
        /// </summary>
        public static readonly TokenType Function = nameof(Function);

        /// <summary>
        /// A token that is used to separate values,
        /// such as a comma.
        /// </summary>
        public static readonly TokenType Separator = nameof(Separator);
    }
}
