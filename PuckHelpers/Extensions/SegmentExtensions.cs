namespace PuckHelpers.Extensions;

/// <summary>
/// Extensions targeting <see cref="ArraySegment{T}"/>.
/// </summary>
public static class SegmentExtensions
{
    /// <summary>
    /// Returns the argument at the specified index.
    /// </summary>
    /// <param name="segment">The target segment.</param>
    /// <param name="index">The target index.</param>
    /// <typeparam name="T">The type.</typeparam>
    /// <returns>The value of the target index.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static T At<T>(this ArraySegment<T> segment, int index)
    {
        if (index < 0 || index >= segment.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        return segment.Array[segment.Offset + index];
    }
}