namespace Domain.Enumeration;

/// <summary>
/// For more information see the <see cref="https://www.rfc-editor.org/info/rfc6750"/>
/// </summary>
public enum BearerTokenUsageTypeEnum : byte
{
    AuthorizationRequestHeader,
    FormEncodedBodyParameter,
    UriQueryParameter
}
