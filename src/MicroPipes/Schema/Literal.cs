using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MicroPipes.Schema
{
    public abstract class Literal : IEquatable<Literal>
    {
        private Literal()
        {
        }

        public sealed class MapLiteral : Literal
        {

            public MapLiteral(ImmutableDictionary<string, Literal> map)
            {
                Value = map;
            }

            public MapLiteral(params (string, Literal)[] values)
            {
                Value = ImmutableDictionary.CreateRange(values.Select(p =>
                    new KeyValuePair<string, Literal>(p.Item1, p.Item2)));
            }

            public ImmutableDictionary<string, Literal> Value { get; }
        }
        
        public sealed class ArrayLiteral : Literal
        {

            public ArrayLiteral(ImmutableList<Literal> array)
            {
                Value = array;
            }

            public ArrayLiteral(params Literal[] values)
            {
                Value = ImmutableList.CreateRange(values);
            }

            public ImmutableList<Literal> Value { get; }
        }
        
        public sealed class BasicLiteral : Literal
        {
            public Schema.BasicLiteral Value { get; }

            public BasicLiteral(Schema.BasicLiteral value)
            {
                Value = value;
            }
            
        }
        
        public sealed class IdentifierLiteral : Literal
        {
            public QualifiedIdentifier Value { get; }

            public IdentifierLiteral(QualifiedIdentifier value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
        
        public static Literal Map(ImmutableDictionary<string, Literal> map)
            => new MapLiteral(map);
        
        public static Literal Map(params (string, Literal)[] values)
            => new MapLiteral(values);
        
        public static Literal Array(ImmutableList<Literal> array)
            => new ArrayLiteral(array);
        
        public static Literal Array(params Literal[] values)
            => new ArrayLiteral(values);
        
        public static Literal Basic(Schema.BasicLiteral value)
            => new BasicLiteral(value);
        
        public static Literal Identifier(QualifiedIdentifier value)
            => new IdentifierLiteral(value);
        

        public bool Equals(Literal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (other.GetType() != GetType()) return false;
            switch (this)
            {
                case ArrayLiteral array:
                    return array.Value.Equals(((ArrayLiteral) other).Value);
                case BasicLiteral basic:
                    return basic.Value.Equals(((BasicLiteral) other).Value);
                case IdentifierLiteral id:
                    return id.Value == ((IdentifierLiteral) other).Value;
                case MapLiteral map:
                    return map.Value.Equals(((MapLiteral) other).Value);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Literal) obj);
        }

        public override int GetHashCode()
        {
            switch (this)
            {
                case ArrayLiteral array:
                    return array.Value.GetHashCode();
                case BasicLiteral basic:
                    return basic.Value.GetHashCode();
                case IdentifierLiteral id:
                    return id.Value.GetHashCode();
                case MapLiteral map:
                    return map.Value.GetHashCode();
            }

            return 0;
        }

        public static bool operator ==(Literal left, Literal right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Literal left, Literal right)
        {
            return !Equals(left, right);
        }
    }
}