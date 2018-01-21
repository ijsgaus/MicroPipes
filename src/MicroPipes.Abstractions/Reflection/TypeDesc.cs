using System;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public abstract class TypeDesc 
    {
        protected TypeDesc([NotNull] QualifiedIdentifier name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public QualifiedIdentifier Name { get; }

        

        protected internal virtual bool Equals(TypeDesc other)
        {
            return Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TypeDesc) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(TypeDesc left, TypeDesc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TypeDesc left, TypeDesc right)
        {
            return !Equals(left, right);
        }
        

        internal abstract void Anchor();
        
        public static readonly TypeDesc Unit = new ScalarDesc(ScalarKind.Unit);
        public static readonly TypeDesc Bool = new ScalarDesc(ScalarKind.Bool);
        public static readonly TypeDesc U8 = new ScalarDesc(ScalarKind.U8);
        public static readonly TypeDesc I8 = new ScalarDesc(ScalarKind.I8);
        public static readonly TypeDesc U16 = new ScalarDesc(ScalarKind.U16);
        public static readonly TypeDesc I16 = new ScalarDesc(ScalarKind.I16);
        public static readonly TypeDesc U32 = new ScalarDesc(ScalarKind.U32);
        public static readonly TypeDesc I32 = new ScalarDesc(ScalarKind.I32);
        public static readonly TypeDesc U64 = new ScalarDesc(ScalarKind.U64);
        public static readonly TypeDesc I64 = new ScalarDesc(ScalarKind.I64);
        public static readonly TypeDesc F64 = new ScalarDesc(ScalarKind.F64);
        public static readonly TypeDesc F32 = new ScalarDesc(ScalarKind.F32);
        public static readonly TypeDesc DateTime = new ScalarDesc(ScalarKind.DateTime);
        public static readonly TypeDesc TimeSpam = new ScalarDesc(ScalarKind.TimeSpan);
        public static readonly TypeDesc String = new ScalarDesc(ScalarKind.String);
        


                

        
    }
}