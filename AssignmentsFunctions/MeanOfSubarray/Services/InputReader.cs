namespace MeanOfSubarray.Services;

public static class InputReader
{
    public static int[] ReadIntArray()
    {
        return Array.ConvertAll(Console.ReadLine()!.Split(' '), int.Parse);
    }

    public static long[] ReadLongArray()
    {
        return Array.ConvertAll(Console.ReadLine()!.Split(' '), long.Parse);
    }
}
