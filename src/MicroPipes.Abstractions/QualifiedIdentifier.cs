using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MicroPipes.Schema;

namespace MicroPipes
{
    public class QualifiedIdentifier : IEquatable<QualifiedIdentifier>, IComparable<QualifiedIdentifier>, IComparable
    {
        private static readonly Regex Regex =
            new Regex("^(?:(?<ns>[A-Za-z][A-Za-z0-9_]*)[.])*(?<name>[A-Za-z][A-Za-z0-9_]*)$");

        public QualifiedIdentifier([NotNull] Identifier name, [NotNull] params Identifier[] nameSpace)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (nameSpace == null) throw new ArgumentNullException(nameof(nameSpace));
            if (nameSpace.Any(p => p == null)) throw new ArgumentNullException(nameof(nameSpace));
            Names = nameSpace.Append(name).ToArray();
        }

        private QualifiedIdentifier(Identifier[] parts)
        {
            Names = parts ?? throw new ArgumentNullException(nameof(parts));
        }


        public int Count => Names.Length;

        [NotNull]
        public Identifier Name => Names[Names.Length - 1];

        [CanBeNull]
        public QualifiedIdentifier Namespace
        {
            get
            {
                if (Names.Length == 1) return null;
                return new QualifiedIdentifier(Names[Names.Length - 2], 
                       Names.Take(Names.Length - 2).ToArray());
            }
        } 
            
        private Identifier[] Names { get; }

        public bool Equals(QualifiedIdentifier other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Names.Length == other.Names.Length && Names.Zip(other.Names, (a, b) => a == b).All(p => p);
        }

        public override string ToString()
        {
            return Names.Aggregate(new StringBuilder(),
                (a, id) => a.Length == 0 ? a.Append(id) : a.Append(".").Append(id),
                a => a.ToString());
        }

        public static bool TryParse(string value, out QualifiedIdentifier identifier)
        {
            identifier = null;
            var v = ParseInternal(value);
            if (v == null || v.Length == 0) return false;
            identifier = new QualifiedIdentifier(v);
            return true;
        }

        public static QualifiedIdentifier Parse(string value)
        {
            return TryParse(value, out var id)
                ? id
                : throw new ArgumentException($"Invalid qualified identifier {value}");
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((QualifiedIdentifier) obj);
        }

        public override int GetHashCode()
        {
            return Names.Aggregate(0, (a, id) => a ^ id.GetHashCode());
        }

        public static bool operator ==(QualifiedIdentifier left, QualifiedIdentifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(QualifiedIdentifier left, QualifiedIdentifier right)
        {
            return !Equals(left, right);
        }

        private static Identifier[] ParseInternal(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var match = Regex.Match(value);
            if (!match.Success) return null;
            return
                match.Groups["ns"].Captures
                    .Cast<Capture>()
                    .Select(p => new Identifier(p.Value))
                    .Union(new[] {new Identifier(match.Groups["name"].Value)})
                    .ToArray();
        }

        public int CompareTo(QualifiedIdentifier other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            var cmp = Names.Zip(other.Names, (a, b) => a.CompareTo(b)).FirstOrDefault(p => p != 0);
            return cmp == 0 ? Count.CompareTo(other.Count) : cmp;
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is QualifiedIdentifier)) throw new ArgumentException($"Object must be of type {nameof(QualifiedIdentifier)}");
            return CompareTo((QualifiedIdentifier) obj);
        }

        public IEnumerable<Identifier> Parts => Names;

    }
}