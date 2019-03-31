namespace Parz
{
    public static class ParzTokenType
    {
        /// <summary>
        /// A constant numeric value.
        /// </summary>
        public static readonly TokenType Constant = nameof(Constant);

        /// <summary>
        /// A token that symbolizes an operation.
        /// </summary>
        public static readonly TokenType Operator = nameof(Operator);

        /// <summary>
        /// A symbolic letter that is not a function.
        /// </summary>
        public static readonly TokenType Variable = nameof(Variable);

        /// <summary>
        /// A token that symbolizes the begining of a segment.
        /// </summary>
        public static readonly TokenType Opener = nameof(Opener);

        /// <summary>
        /// A token that symbolizes the end of a segment.
        /// </summary>
        public static readonly TokenType Closer = nameof(Closer);
    }
}
