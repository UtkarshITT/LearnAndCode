using MeanOfSubarray.Models;

namespace MeanOfSubarray.Services;
public class SubarrayMeanCalculator
{
    private readonly PrefixSumCalculator _prefixSumCalculator;

    public SubarrayMeanCalculator(long[] array)
    {
        _prefixSumCalculator = new PrefixSumCalculator(array);
    }

    public long CalculateFloorMean(SubarrayQuery query)
    {
        long rangeSum = _prefixSumCalculator.GetRangeSum(query.LeftIndex, query.RightIndex);
        return rangeSum / query.SubarrayLength;
    }

    public long CalculateFloorMean(int leftIndex, int rightIndex)
    {
        var query = new SubarrayQuery(leftIndex, rightIndex);
        return CalculateFloorMean(query);
    }
}
