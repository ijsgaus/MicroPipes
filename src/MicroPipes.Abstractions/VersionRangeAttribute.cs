using System;
using NuGet.Versioning;

namespace MicroPipes
{
    [AttributeUsage(AttributeTargets.All)]
    public class VersionRangeAttribute : Attribute
    {
        public VersionRangeAttribute(string range)
        {
            Range = VersionRange.Parse(range);
        }

        public VersionRange Range { get; }


        public bool Satisfies(SemanticVersion version)
            => Range.Satisfies(
                new NuGetVersion(version.Major, version.Minor, version.Patch, version.ReleaseLabels, version.Metadata),
                VersionComparison.Version);
    }
}