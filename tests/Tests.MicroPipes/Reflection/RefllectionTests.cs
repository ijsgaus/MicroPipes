using System;
using MicroPipes.Reflection;
using Xunit;

namespace Tests.MicroPipes.Reflection
{
    public class RefllectionTests
    {
        /*
        [Fact]
        public void MustThrowDuplicateEndpointNames()
        {
            Assert.Throws<ArgumentException>(() =>
                new ServiceDesc("Test.Service", new EndpointDesc[]
                {
                    new EventEndpointDesc("Call"),
                    new CallEndpointDesc("Call")
                }));
        }


        [Fact]
        public void MustThrowDuplicateCallOverloads()
        {
            Assert.Throws<ArgumentException>(() => new CallEndpointDesc("Event",
                new (QualifiedIdentifier, QualifiedIdentifier)[]
                {
                    ("First", "Second"),
                    ("First", "Third")
                }));
        }

        [Fact]
        public void MustEqualEventEndpoints()
        {
            var e1 = new EventEndpointDesc("First", new QualifiedIdentifier[] {"Hello1", "Hello2"});
            var e2 = new EventEndpointDesc("First", new QualifiedIdentifier[] {"Hello2", "Hello1"});
            Assert.Equal(e1, e2);
            Assert.Equal(e1.GetHashCode(), e2.GetHashCode());
        }

        [Fact]
        public void MustEqualCallEndpoints()
        {
            var e1 = new CallEndpointDesc("First", new (QualifiedIdentifier, QualifiedIdentifier)[] { ("Hello1", "Answer1"), ("Hello2", "Answer1")});
            var e2 = new CallEndpointDesc("First", new (QualifiedIdentifier, QualifiedIdentifier)[] { ("Hello1", "Answer1"), ("Hello2", "Answer1")});
            Assert.Equal(e1, e2);
            Assert.Equal(e1.GetHashCode(), e2.GetHashCode());
        }

        [Fact]
        public void MustEqualServices()
        {
            var s1 = new ServiceDesc("Service", new EndpointDesc[]
            {
                new CallEndpointDesc("First", new (QualifiedIdentifier, QualifiedIdentifier)[] { ("Hello1", "Answer1"), ("Hello2", "Answer1")}),
                new EventEndpointDesc("Second", new QualifiedIdentifier[] {"Hello2", "Hello1"})
                
            });
            var s2 = new ServiceDesc("Service", new EndpointDesc[]
            {
                new EventEndpointDesc("Second", new QualifiedIdentifier[] {"Hello2", "Hello1"}),
                new CallEndpointDesc("First", new (QualifiedIdentifier, QualifiedIdentifier)[] { ("Hello1", "Answer1"), ("Hello2", "Answer1")}),
            });
            Assert.Equal(s1, s2);
            Assert.Equal(s1.GetHashCode(), s2.GetHashCode());
        }

        [Fact]
        public void ScalarsEqualities()
        {
            Assert.Equal(new ScalarDesc(ScalarKind.Unit), TypeDesc.Unit);
            Assert.Equal(new ScalarDesc(ScalarKind.Bool), TypeDesc.Bool);
            Assert.Equal(new ScalarDesc(ScalarKind.U8), TypeDesc.U8);
            Assert.Equal(new ScalarDesc(ScalarKind.I8), TypeDesc.I8);
            Assert.Equal(new ScalarDesc(ScalarKind.U16), TypeDesc.U16);
            Assert.Equal(new ScalarDesc(ScalarKind.I16), TypeDesc.I16);
            Assert.Equal(new ScalarDesc(ScalarKind.U32), TypeDesc.U32);
            Assert.Equal(new ScalarDesc(ScalarKind.I32), TypeDesc.I32);
            Assert.Equal(new ScalarDesc(ScalarKind.U64), TypeDesc.U64);
            Assert.Equal(new ScalarDesc(ScalarKind.I64), TypeDesc.I64);
        }
        
        [Fact]
        public void ScalarsNotEqualities()
        {
            Assert.NotEqual<TypeDesc>(TypeDesc.Bool, TypeDesc.DateTime);
        }
        */
    }
}