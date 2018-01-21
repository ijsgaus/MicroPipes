using System;
using MicroPipes;
using MicroPipes.Reflection;
using Xunit;

namespace Tests.MicroPipes.Reflection
{
    public class IdentifierTests
    {
        [Fact]
        public void MustParseValidIdentifier()
        {
            Identifier.Parse("Abcd");
            Identifier.Parse("Ab_cd");
            Identifier.Parse("Ab123_cd");
        }

        [Fact]
        public void MustThrowOnInvalidIdentifier()
        {
            Assert.Throws<ArgumentException>(() => Identifier.Parse("123"));
            Assert.Throws<ArgumentException>(() => Identifier.Parse("Физ"));
            Assert.Throws<ArgumentException>(() => Identifier.Parse("Abs.dfg"));
        }

        [Fact]
        public void MustParseValidQualifiedIdentifier()
        {
            QualifiedIdentifier.Parse("Abcd.Efgh");
            QualifiedIdentifier.Parse("Abcd.Ef_12gh");
            QualifiedIdentifier.Parse("Abcd.Efgh.F456");
        }

        [Fact]
        public void MustThrowOnInvalidQualifiedIdentifier()
        {
            Assert.Throws<ArgumentException>(() => QualifiedIdentifier.Parse("Abcd:1234"));
            Assert.Throws<ArgumentException>(() => QualifiedIdentifier.Parse("Вап.Fghj"));
            Assert.Throws<ArgumentException>(() => QualifiedIdentifier.Parse("Abs..dfg"));
            Assert.Throws<ArgumentException>(() => QualifiedIdentifier.Parse("Abs.dfg."));
        }

        [Fact]
        public void MustGetQualifiedIdentifierNamePart()
        {
            var qid = QualifiedIdentifier.Parse("Hellow.The.Best.World");
            var name = Identifier.Parse("World");
            Assert.Equal(name, qid.Name);
        }

        [Fact]
        public void MustGetQualifiedIdentifierNamespace()
        {
            var qid = QualifiedIdentifier.Parse("Hellow.The.Best.World");
            var ns = QualifiedIdentifier.Parse("Hellow.The.Best");
            Assert.Equal(ns, qid.Namespace);
        }

        [Fact]
        public void MustCompareIdentifierCaseInsensivity()
        {
            var id1 = Identifier.Parse("Hellow");
            var id2 = Identifier.Parse("helloW");
            Assert.Equal(id1, id2);
            var qid1 = QualifiedIdentifier.Parse("Hellow.The.Best.World");
            var qid2 = QualifiedIdentifier.Parse("helloW.the.BesT.WorlD");
            Assert.Equal(qid1, qid2);
        }
    }
}