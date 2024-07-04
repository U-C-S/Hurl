namespace Hurl.Library.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T>? self)
    {
        if (self == null)
        {
            return Enumerable.Empty<(T, int)>();
        }
        else
            return self.Select((item, index) => (item, index));
    }
}