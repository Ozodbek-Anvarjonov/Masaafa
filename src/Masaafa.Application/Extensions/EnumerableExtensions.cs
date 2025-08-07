namespace Masaafa.Application.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int size)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size), "Batch size must be greater than 0.");

        List<T> batch = new(size);

        foreach (var item in source)
        {
            batch.Add(item);
            if (batch.Count == size)
            {
                yield return batch;
                batch = new(size);
            }
        }

        if (batch.Count > 0)
            yield return batch;
    }
}