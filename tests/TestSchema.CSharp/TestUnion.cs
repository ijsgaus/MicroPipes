using System;
using MicroPipes;

namespace TestSchema.CSharp
{
    [OneOfRoot]
    public abstract class TestUnion
    {
        [OneOfMember]
        public sealed class NoValueType : TestUnion
        {
            protected override bool Equals(TestUnion other) => true;
            public override int GetHashCode() => typeof(NoValueType).GetHashCode();

        }
        
        [OneOfMember]
        public sealed class IntValueType : TestUnion
        {
            public IntValueType(int value)
            {
                Value = value;
            }

            public int Value { get;  }
            protected override bool Equals(TestUnion other) => ((IntValueType) other).Value == Value;
            public override int GetHashCode() => Value.GetHashCode();
        }

        protected abstract bool Equals(TestUnion other);
        

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestUnion) obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(TestUnion left, TestUnion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TestUnion left, TestUnion right)
        {
            return !Equals(left, right);
        }

        private TestUnion() {}
    }
}