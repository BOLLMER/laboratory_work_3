using System;
using System.Text;

class Program
{
    static string Sokr(int chisl, int znam)
    {
        int g = GCD(chisl, znam);
        return (chisl / g).ToString() + "/" + (znam / g).ToString();
    }

    static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static string CalcSum(int a, int b)
    {
        if (b == 1) return "infinity";

        if (a == 1)
        {
            int znam = (b - 1) * (b - 1);
            return Sokr(b, znam);
        }

        if (a == 2)
        {
            int chisl = b * (b + 1);
            int znam = (b - 1) * (b - 1) * (b - 1);
            return Sokr(chisl, znam);
        }

        if (a == 3)
        {
            int num = b * (b * b + 4 * b + 1);
            int den = (b - 1) * (b - 1) * (b - 1) * (b - 1);
            return Sokr(num, den);
        }

        double sum = 0;
        int maxTerms = 10000;
        double prevSum = 0;
        bool converged = false;

        for (int n = 1; n <= maxTerms; n++)
        {
            sum += Math.Pow(n, a) / Math.Pow(b, n);
            if (Math.Abs(sum - prevSum) < 1e-10)
            {
                converged = true;
                break;
            }
            prevSum = sum;
        }

        if (!converged) return "irrational";

        if (Math.Abs(sum - Math.Round(sum)) < 1e-10)
        {
            return Math.Round(sum).ToString() + "/1";
        }

        for (int denom = 2; denom <= 1000; ++denom)
        {
            double numerator = sum * denom;
            if (Math.Abs(numerator - Math.Round(numerator)) < 1e-10)
            {
                return Sokr((int)Math.Round(numerator), denom);
            }
        }
        return "irrational";
    }

    static void Main(string[] args)
    {
        int a, b;
        Console.Write("Введите значения a и b через пробел: ");
        while (true)
        {
            string[] input = Console.ReadLine().Split();
            if (input.Length != 2 || !int.TryParse(input[0], out a) || !int.TryParse(input[1], out b))
            {
                Console.WriteLine("Ошибка ввода. Введите два целых числа через пробел.");
                continue;
            }

            if (a < 1 || a > 10 || b < 1 || b > 10)
            {
                Console.WriteLine("Числа должны быть в диапазоне от 1 до 10!");
            }
            else
            {
                break;
            }
        }
        Console.WriteLine("Результат: " + CalcSum(a, b));
    }
}