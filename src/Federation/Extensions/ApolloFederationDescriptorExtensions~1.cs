using ApolloGraphQL.HotChocolate.Federation.Constants;
using ApolloGraphQL.HotChocolate.Federation.Descriptors;
using HotChocolate.Language;
using static ApolloGraphQL.HotChocolate.Federation.Properties.FederationResources;
using static ApolloGraphQL.HotChocolate.Federation.Constants.WellKnownContextData;

namespace HotChocolate.Types;

/// <summary>
/// Provides extensions for type system descriptors.
/// </summary>
public static partial class ApolloFederationDescriptorExtensions
{
    /// <summary>
    /// Adds the @key directive which is used to indicate a combination of fields that can be used to uniquely
    /// identify and fetch an object or interface. The specified field set can represent single field (e.g. "id"),
    /// multiple fields (e.g. "id name") or nested selection sets (e.g. "id user { name }"). Multiple keys can
    /// be specified on a target type.
    /// <example>
    /// type Foo @key(fields: "id") {
    ///   id: ID!
    ///   field: String
    /// }
    /// </example>
    /// </summary>
    /// <param name="descriptor">
    /// The object type descriptor on which this directive shall be annotated.
    /// </param>
    /// <param name="fieldSet">
    /// The field set that describes the key.
    /// Grammatically, a field set is a selection set minus the braces.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="descriptor"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="fieldSet"/> is <c>null</c> or <see cref="string.Empty"/>.
    /// </exception>
    /// <summary>
    public static IEntityResolverDescriptor<T> Key<T>(
        this IObjectTypeDescriptor<T> descriptor,
        string fieldSet)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        if (string.IsNullOrEmpty(fieldSet))
        {
            throw new ArgumentException(
                FieldDescriptorExtensions_Key_FieldSet_CannotBeNullOrEmpty,
                nameof(fieldSet));
        }

        descriptor.Directive(
            WellKnownTypeNames.Key,
            new ArgumentNode(
                WellKnownArgumentNames.Fields,
                new StringValueNode(fieldSet)));

        return new EntityResolverDescriptor<T>(descriptor);
    }

    /// <summary>
    /// Mark the type as an extension
    /// of a type that is defined by another service when
    /// using apollo federation.
    /// </summary>
    [Obsolete("Use ExtendsType type instead")]
    public static IObjectTypeDescriptor<T> ExtendServiceType<T>(
        this IObjectTypeDescriptor<T> descriptor)
    {
        if (descriptor is null)
        {
            throw new ArgumentNullException(nameof(descriptor));
        }

        descriptor
            .Extend()
            .OnBeforeCreate(d => d.ContextData[ExtendMarker] = true);

        return descriptor;
    }
}
