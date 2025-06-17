using System;

class Program
{
    static double F(double x)
    {
        if (-5 <= x && x < -3) { return 1; }
        else if (-3 <= x && x <= -1) { return -Math.Sqrt(4 - Math.Pow(x + 1, 2)); }
        else if (-1 < x && x < 2) { return -2; }
        else if (2 <= x && x <= 5) { return x - 4; }
        return 13;
    }

    static void Main()
    {
        double x_start = -5;
        double x_end = 5;
        double dx = 0.5;

        Console.WriteLine("|     x    |     y    |");
        Console.WriteLine("|----------|----------|");

        for (double x = x_start; x <= x_end; x += dx)
        {
            Console.WriteLine($"| {x,8:F1} | {F(x),8:F3} |");
        }
    }
}