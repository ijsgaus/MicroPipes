using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MicroPipes.Schema
{
    public class SemanticVersion : IEquatable<SemanticVersion>, IComparable<SemanticVersion>, IComparable
    {
        public SemanticVersion(ushort major, ushort minor, uint patch, params string[] prerelease)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            
            if(prerelease.Length > 0 && !prerelease.Select(p => Alpha.Match(p).Success).All(p => p))
                throw new ArgumentException("Invalid prerelease value", nameof(prerelease));
            Prerelease = prerelease;
        }

        public ushort Major { get; }
        public ushort Minor { get; }
        public uint Patch { get; }
        public string[] Prerelease { get; } // split and compare

        public static bool TryParse(string version, out SemanticVersion prsd)
        {
            prsd = null;
            if (version == null) return false;
            
            var parsed = Regex.Match(version);
            if (!parsed.Success) return false;
            if (!ushort.TryParse(parsed.Groups["major"].Value, out var major))
                return false;
            if (!ushort.TryParse(parsed.Groups["minor"].Value, out var minor))
                return false;
            if (!uint.TryParse(parsed.Groups["patch"].Value, out var patch))
                return false;
            if (parsed.Groups["pre0"].Value == "")
            {
                prsd = new SemanticVersion(major, minor, patch);
                return true;
            }

            var pre =
                new[] {parsed.Groups["pre0"].Value}
                    .Union(
                        parsed.Groups["pre1"].Captures
                            .OfType<Capture>()
                            .Select(p => p.Value)
                            .Where(p => p != ""))
                    .ToArray();
            prsd = new SemanticVersion(major, minor, patch, pre);
            return true;
        }

        public static SemanticVersion Parse(string version)
        {
            if (TryParse(version, out var v))
                return v;
            throw new ArgumentException("Invalid semantic version", nameof(version));
        }

        public int CompareTo(SemanticVersion other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;
            var minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;
            var pathComparison = Patch.CompareTo(other.Patch);
            if (pathComparison != 0) return pathComparison;
            if (Prerelease.Length == 0 && other.Prerelease.Length == 0)
                return 0;
            if (Prerelease.Length == 0) return 1;
            if (other.Prerelease.Length == 0) return -1;
            for (var i = 0; i < Math.Min(Prerelease.Length, other.Prerelease.Length); i++)
            {
                if (Digits.Match(Prerelease[i]).Success && Digits.Match(other.Prerelease[i]).Success)
                {
                    var cmp = int.Parse(Prerelease[i]).CompareTo(int.Parse(other.Prerelease[i]));
                    if (cmp != 0) return cmp;
                }
                var cmp1 = StringComparer.InvariantCulture.Compare(Prerelease[i],other.Prerelease[i]);
                if (cmp1 != 0) return cmp1;
            }

            return Prerelease.Length.CompareTo(other.Prerelease.Length);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is SemanticVersion)) throw new ArgumentException($"Object must be of type {nameof(SemanticVersion)}");
            return CompareTo((SemanticVersion) obj);
        }

        public static bool operator <(SemanticVersion left, SemanticVersion right)
        {
            return Comparer<SemanticVersion>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(SemanticVersion left, SemanticVersion right)
        {
            return Comparer<SemanticVersion>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(SemanticVersion left, SemanticVersion right)
        {
            return Comparer<SemanticVersion>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(SemanticVersion left, SemanticVersion right)
        {
            return Comparer<SemanticVersion>.Default.Compare(left, right) >= 0;
        }

        public bool Equals(SemanticVersion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Major == other.Major && Minor == other.Minor && Patch == other.Patch
                   && Prerelease.Length == other.Prerelease.Length &&
                   Prerelease.Zip(other.Prerelease, (a, b) => a == b).All(t => t);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SemanticVersion) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Major.GetHashCode();
                hashCode = (hashCode * 397) ^ Minor.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Patch;
                hashCode = (hashCode * 397) ^ Prerelease.Aggregate(0, (a, v) => a ^ v.GetHashCode());
                return hashCode;
            }
        }

        public static bool operator ==(SemanticVersion left, SemanticVersion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SemanticVersion left, SemanticVersion right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            var st = $"{Major}.{Minor}.{Patch}";
            if (Prerelease.Length == 0) return st;
            return st + "-" + Prerelease.Aggregate(new StringBuilder(),
                       (a, s) => a.Length == 0 ? a.Append(s) : a.Append(".").Append(s), a => a.ToString());
        }


        private static readonly Regex Digits = new Regex("^[0-9]+$");
        private static readonly Regex Alpha = new Regex("^[a-zA-Z0-9]+$");
        private static readonly Regex Regex = 
            new Regex(@"^(?<major>[0-9]+).(?<minor>[0-9]*).(?<patch>[0-9]*)(?:-(?<pre0>[a-zA-Z0-9]+)(?:.(?<pre1>[a-zA-Z0-9]+))*)?$"); 
    }
}