using System;
using NuGet.Versioning;

namespace MicroPipes
{
    [AttributeUsage(AttributeTargets.All)]
    public class VersionRangeAttribute : Attribute
    {
        public VersionRangeAttribute(string @from = null, string to = null)
        {
            From = @from != null ? SemanticVersion.Parse(@from) : null;
            To = to != null ? SemanticVersion.Parse(@to) : null;
        }

        public SemanticVersion From { get; }
        public SemanticVersion To { get; }

        public bool IsInRange(SemanticVersion version)
        {
            if(version.IsPrerelease)
                version = new SemanticVersion(version.Major, version.Minor, version.Patch);
            if (From != null)
            {
                if (From > version) return false;
            }

            return To == null || To > version;
        }
    }
}