using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public class ServiceDesc : IReflectionExtensible
    {
        public ServiceDesc([NotNull] QualifiedIdentifier name, [NotNull] IEnumerable<EndpointDesc> endpoints,
            IEnumerable<(QualifiedIdentifier, object)> extensions = null)
        {
            if (endpoints == null) throw new ArgumentNullException(nameof(endpoints));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            try
            {
                Endpoints = ImmutableDictionary.CreateRange(endpoints.ToImmutableDictionary(p => p.Name));
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(
                    $"Endpoint name duplicated",
                    nameof(endpoints), ex);
            }


            Extensions = extensions.ToImmutableDictionary();
        }

        public QualifiedIdentifier Name { get; }
        public IReadOnlyDictionary<Identifier, EndpointDesc> Endpoints { get; }
        public IReadOnlyDictionary<QualifiedIdentifier, object> Extensions { get; }

        protected bool Equals(ServiceDesc other)
        {
            return
                Equals(Name, other.Name) &&
                Endpoints.DefaultComparerEqual(other.Endpoints) &&
                Extensions.DefaultComparerEqual(other.Extensions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ServiceDesc) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Endpoints.DefaultComparerHashCode();
                hashCode = (hashCode * 397) ^ Extensions.DefaultComparerHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ServiceDesc left, ServiceDesc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ServiceDesc left, ServiceDesc right)
        {
            return !Equals(left, right);
        }
    }
}