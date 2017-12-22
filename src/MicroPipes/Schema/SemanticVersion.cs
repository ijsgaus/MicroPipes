namespace MicroPipes.Schema
{
    public class SemanticVersion
    {
        public ushort Major { get; }
        public ushort Minor { get; }
        public uint Patch { get; }
        public string Prerelease { get; } // split and compare
}
}