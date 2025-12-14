using System.Diagnostics;

namespace PlinqCP
{
    internal class Program
    {
        private const int DataSize = 10000;          
        private const int LoadIterations = 20000;    

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Генерируем коллекцию");
            var data = Enumerable.Range(0, DataSize).ToArray();   

            Console.WriteLine("\nLINQ:");
            var sw = Stopwatch.StartNew();
            var sequentialResult = data.Where(x => x % 2 == 0).Select(HeavyOperation).OrderBy(x => x).ToList();
            sw.Stop();
            Console.WriteLine($"Элементов в результате: {sequentialResult.Count}");
            Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс");

            Console.WriteLine("\nPLINQ");
            sw.Restart();
            var parallelResult = data.AsParallel().Where(x => x % 2 == 0).Select(HeavyOperation).OrderBy(x => x).ToList();
            sw.Stop();
            Console.WriteLine($"Элементов в результате: {parallelResult.Count}");
            Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс");
        }

        private static int HeavyOperation(int x)
        {
            var t = x;

            for (int i = 0; i < LoadIterations; i++)
            {
                t = (t * 7 + 13) % 9973;
            }  

            return t;
        }
    }
}