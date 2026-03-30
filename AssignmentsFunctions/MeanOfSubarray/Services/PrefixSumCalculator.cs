namespace MeanOfSubarray.Services;

public class PrefixSumCalculator
{
    private readonly long[] _prefixSum;

    public PrefixSumCalculator(long[] array)
    {
        _prefixSum = BuildPrefixSum(array);
    }

    /// <summary>
    /// Builds prefix sum array where prefixSum[i] = sum of elements from index 0 to i-1
    /// </summary>
    private long[] BuildPrefixSum(long[] array)
    {
        var prefixSum = new long[array.Length + 1];
        prefixSum[0] = 0;

        for (int i = 1; i <= array.Length; i++)
        {
            prefixSum[i] = prefixSum[i - 1] + array[i - 1];
        }

        return prefixSum;
    }

    /// <summary>
    /// Calculates sum of elements from leftIndex to rightIndex (1-based indices)
    /// </summary>
    public long GetRangeSum(int leftIndex, int rightIndex)
    {
        return _prefixSum[rightIndex] - _prefixSum[leftIndex - 1];
    }
}
