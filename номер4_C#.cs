using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int n, m;
        List<int> numbers = new List<int>();
        Random random = new Random();

        Console.Write("Введите N и M через пробел: "); // N (количество чисел) и M (максимальное количество чисел, которые можно взять за ход)
        string[] input = Console.ReadLine().Split();
        n = int.Parse(input[0]);
        m = int.Parse(input[1]);

        if (n < 0)
        {
            n = Math.Abs(n);
            for (int i = 0; i < n; i++)
            {
                numbers.Add(random.Next(5, 50001));
            }
            Console.Write("Случайно сгенерированный список:");
            foreach (var num in numbers)
            {
                Console.Write(" " + num);
            }
            Console.WriteLine();
        }
        else
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Введите число №{i + 1} > ");
                int z = int.Parse(Console.ReadLine());
                numbers.Add(z);
            }
        }

        int[] dp = new int[n + 1];
        dp[n] = 0;
        for (int i = n - 1; i >= 0; --i)
        {
            dp[i] = int.MinValue;
            int sum = 0;
            for (int k = 1; k <= m && i + k <= n; ++k)
            {
                sum += numbers[i + k - 1];
                dp[i] = Math.Max(dp[i], sum - dp[i + k]);
            }
        }

        Console.WriteLine(dp[0] > 0 ? 1 : 0);
    }
}