namespace MeanOfSubarray.Models;
public class SubarrayQuery
{
    public int LeftIndex { get; }
    public int RightIndex { get; }

    public SubarrayQuery(int leftIndex, int rightIndex)
    {
        LeftIndex = leftIndex;
        RightIndex = rightIndex;
    }

    /// <summary>
    /// Calculates the length of the subarray
    /// </summary>
    public int SubarrayLength => RightIndex - LeftIndex + 1;
}
