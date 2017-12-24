using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LanguageExt;

namespace MicroPipes.Schema
{
    public abstract class Literal 
    {
        private Literal()
        {
        }

        public sealed class MapLiteral : Literal
        {

            public MapLiteral(HashMap<string, Literal> map) => Value = map;

            public MapLiteral(params (string, Literal)[] values) : this((IEnumerable<(string, Literal)>) values)
            {
                
            }
                 
            public MapLiteral(IEnumerable<(string, Literal)> values)
            {
                Value = HashMap.createRange(values);
            }

            public HashMap<string, Literal> Value { get; }

            protected override bool Equals(Literal other)
                => Equals(Value, ((MapLiteral) other).Value);

            public override int GetHashCode() => Value.GetHashCode();

        }
        
        public sealed class ArrayLiteral : Literal
        {

            public ArrayLiteral(Arr<Literal> array) => Value = array;

            public ArrayLiteral(params Literal[] values) => Value = Arr.createRange(values);
            
            public ArrayLiteral(IEnumerable<Literal> values) => Value = Arr.createRange(values);

            public Arr<Literal> Value { get; }

            protected override bool Equals(Literal other)
                => Equals(Value, ((ArrayLiteral) other).Value);
            
            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class BasicLiteral : Literal
        {
            public Schema.BasicLiteral Value { get; }

            public BasicLiteral(Schema.BasicLiteral value) => Value = value;

            protected override bool Equals(Literal other)
                => Value == ((BasicLiteral) other).Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public static Literal Map(HashMap<string, Literal> map) => new MapLiteral(map);
        public static Literal Map(IEnumerable<(string, Literal)> map) => new MapLiteral(map);
        public static Literal Map(params (string, Literal)[] map) => new MapLiteral(map);
        public static Literal Map(IEnumerable<KeyValuePair<string, Literal>> map) => new MapLiteral(map.Select(p => (p.Key, p.Value)));
        
        public static Literal Array(Arr<Literal> arr) => new ArrayLiteral(arr);
        public static Literal Array(IEnumerable<Literal> arr) => new ArrayLiteral(arr);
        public static Literal Array(params Literal[] arr) => new ArrayLiteral(arr);
        
        public static Literal Basic(Schema.BasicLiteral literal) => new BasicLiteral(literal);

        public static implicit operator Literal(Schema.BasicLiteral literal) => Basic(literal);
        
        public static implicit operator Literal(HashMap<string, Literal> map) => new MapLiteral(map);
        public static implicit operator Literal((string, Literal)[] map) => new MapLiteral(map);
        public static implicit operator Literal(Dictionary<string, Literal> map) => new MapLiteral(map.Select(p => (p.Key, p.Value)));
        
        public static implicit operator Literal(Arr<Literal> arr) => new ArrayLiteral(arr);
        public static implicit operator Literal(Collection<Literal> arr) => new ArrayLiteral(arr.AsEnumerable());
        public static implicit operator Literal(Literal[] arr) => new ArrayLiteral(arr);
        
        
        

        protected abstract bool Equals(Literal other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Literal) obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
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