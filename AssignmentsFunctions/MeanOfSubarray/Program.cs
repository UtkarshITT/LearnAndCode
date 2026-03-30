using MeanOfSubarray.Services;

/*
 * Mean of Subarray
 * 
 * Given an array of N numbers and Q queries, for each query print the floor 
 * of the expected value (mean) of the subarray from L to R.
 * 
 * Input:
 *   - First line: N (array size) and Q (number of queries)
 *   - Second line: N space-separated array elements
 *   - Next Q lines: L and R indices for each query
 * 
 * Output:
 *   - Floor of mean for each query
 */

// Read N (array size) and Q (number of queries)
int[] sizeAndQueryCount = InputReader.ReadIntArray();
int arraySize = sizeAndQueryCount[0];
int queryCount = sizeAndQueryCount[1];

// Read array elements
long[] array = InputReader.ReadLongArray();

// Initialize calculator with prefix sum for efficient range queries
var meanCalculator = new SubarrayMeanCalculator(array);

// Process each query
for (int i = 0; i < queryCount; i++)
{
    int[] queryIndices = InputReader.ReadIntArray();
    int leftIndex = queryIndices[0];
    int rightIndex = queryIndices[1];

    long floorMean = meanCalculator.CalculateFloorMean(leftIndex, rightIndex);
    Console.WriteLine(floorMean);
}
