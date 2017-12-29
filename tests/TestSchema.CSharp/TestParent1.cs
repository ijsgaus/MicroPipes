namespace TestSchema.CSharp
{
    public class TestParent1
    {
        public string B { get; }
    }

    public class TestChield1 : TestParent1
    {
        public int A { get; }
    }
    
    public class TestChield2 : TestParent1
    {
        public long A { get; }
    }
    
}