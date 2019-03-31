using System;
using System.Collections.Generic;

namespace Parz
{
    /// <summary>
    /// A class that acts in a similar way to an enum, except
    /// that it's not solved at compile time, meaning that its
    /// performance when comparring isn't as great. However it
    /// can be extended by defining new types and acts in the
    /// same way.
    /// </summary>
    public sealed class TokenType
    {
        private static readonly IEqualityComparer<string> _comparer = StringComparer
            .InvariantCultureIgnoreCase;

        private readonly string _idValue;

        public TokenType(string idValue)
        {
            _idValue = idValue;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            var tokenType = (TokenType)obj;
            return _comparer.Equals(_idValue, tokenType._idValue);
        }

        public static bool operator ==(TokenType a, TokenType b)
        {
            if (a is null)
            {
                return b is null;
            }

            return a.Equals(b);
        }

        public static bool operator !=(TokenType a, TokenType b)
        {
            return !(a == b);
        }

        public static implicit operator TokenType(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new TokenType(value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return _idValue;
        }
    }
}
