using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

class Program
{
    static ulong W;
    const int N = 500;
    static List<bool> resheto = Enumerable.Repeat(true, N).ToList();
    static List<int> simpleNums = new List<int>();

    static Random rng = new Random();

    static int BitLength(ulong x)
    {
        int bits = 0;
        while (x > 0)
        {
            x >>= 1;
            bits++;
        }
        return bits;
    }

    static ulong IntPow(ulong baseVal, int exp)
    {
        ulong result = 1;
        for (int i = 0; i < exp; i++)
        {
            if (result > ulong.MaxValue / baseVal)
                return 0; // Overflow
            result *= baseVal;
        }
        return result;
    }

    static ulong ModPow(ulong a, ulong x, ulong m)
    {
        BigInteger result = 1;
        BigInteger baseVal = a % m;
        while (x > 0)
        {
            if ((x & 1) == 1)
                result = (result * baseVal) % m;
            baseVal = (baseVal * baseVal) % m;
            x >>= 1;
        }
        return (ulong)result;
    }

    static bool MillerRabin(ulong n, int t)
    {
        if (n < 2) return false;

        ulong d = n - 1;
        int s = 0;
        while ((d & 1) == 0)
        {
            d >>= 1;
            s++;
        }

        for (int i = 0; i < t; i++)
        {
            ulong a = (ulong)rng.Next(2, (int)Math.Min(n - 2, int.MaxValue));
            ulong x = ModPow(a, d, n);
            if (x == 1 || x == n - 1) continue;

            bool composite = true;
            for (int r = 1; r < s; r++)
            {
                x = (ulong)((BigInteger)x * x % n);
                if (x == n - 1)
                {
                    composite = false;
                    break;
                }
            }

            if (composite)
                return false;
        }

        return true;
    }

    static void Eratosthenes()
    {
        resheto[0] = resheto[1] = false;
        for (int i = 2; i * i < N; i++)
        {
            if (resheto[i])
            {
                for (int j = i * i; j < N; j += i)
                    resheto[j] = false;
            }
        }
        for (int i = 2; i < N; i++)
        {
            if (resheto[i])
                simpleNums.Add(i);
        }
    }

    static List<ulong> millerOtbros = new List<ulong>();

    static bool MillerTest(ulong n, int t, List<int> qs)
    {
        var randomBint = new List<ulong>();
        for (int i = 0; i < t; i++)
        {
            ulong elem;
            do
            {
                elem = (ulong)rng.Next(2, (int)Math.Min(n - 1, int.MaxValue));
            }
            while (randomBint.Contains(elem));

            if (ModPow(elem, n - 1, n) != 1)
                return false;

            randomBint.Add(elem);
        }

        foreach (var qi in qs)
        {
            bool found = false;
            foreach (var aj in randomBint)
            {
                if (ModPow(aj, (n - 1) / (ulong)qi, n) != 1)
                {
                    found = true;
                    break;
                }
            }
            if (!found) return false;
        }

        return true;
    }

    static (ulong, List<int>) MillerGen(int k)
    {
        ulong m = 1;
        var qs = new List<int>();
        var dis = new Random();

        while (true)
        {
            m = 1;
            qs.Clear();
            for (int i = 0; i < k; i++)
            {
                int q = simpleNums[dis.Next(simpleNums.Count)];
                qs.Add(q);
                int a = dis.Next(1, 4);
                ulong elem = IntPow((ulong)q, a);
                if (elem == 0 || BitLength(m) + BitLength(elem) > (int)(W - 1))
                {
                    m = 0;
                    break;
                }
                m *= elem;
            }
            if (BitLength(m) == (int)(W - 1) && m != 0)
                break;
        }

        qs.Add(2);
        qs = qs.Distinct().OrderBy(x => x).ToList();
        ulong n = 2 * m + 1;
        return (n, qs);
    }

    static ulong Miller(int k, int t)
    {
        ulong final;
        while (true)
        {
            var (n, qs) = MillerGen(k);
            final = n;
            if (MillerTest(n, t, qs))
                break;
            else
                millerOtbros.Add(n);
        }
        return final;
    }
    static List<ulong> polingtonOtbros = new List<ulong>();

    static bool PolingtonTest(ulong n, int t, List<int> qs)
    {
        var randomBint = new List<ulong>();
        for (int i = 0; i < t; i++)
        {
            ulong elem;
            do
            {
                elem = (ulong)rng.Next(2, (int)Math.Min(n - 1, int.MaxValue));
            }
            while (randomBint.Contains(elem));

            if (ModPow(elem, n - 1, n) != 1)
                return false;

            randomBint.Add(elem);
        }

        foreach (var aj in randomBint)
        {
            bool found = false;
            foreach (var qi in qs)
            {
                if (ModPow(aj, (n - 1) / (ulong)qi, n) == 1)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return true;
        }

        return false;
    }

    static (ulong, List<int>) PolingtonGen(int k)
    {
        ulong F = 1;
        var qs = new List<int>();
        var dis = new Random();

        while (true)
        {
            qs.Clear();
            F = 1;
            for (int i = 0; i < k; i++)
            {
                int q = simpleNums[dis.Next(simpleNums.Count)];
                int a = dis.Next(1, 4);
                ulong elem = IntPow((ulong)q, a);
                if (elem == 0 || BitLength(F) + BitLength(elem) > (int)(W / 2))

                {
                    F = 0;
                    break;
                }
                F *= elem;
                qs.Add(q);
            }

            if (BitLength(F) == (int)(W / 2 - 1) && F != 0)
                break;
        }

        qs.Add(2);
        qs = qs.Distinct().OrderBy(x => x).ToList();

        ulong R;
        while (true)
        {
            ulong a = (ulong)rng.Next() | ((ulong)rng.Next() << 32);
            if (a % 2 == 0 && BitLength(a) == (int)W - BitLength(F))
            {
                R = a;
                break;
            }
        }

        ulong n = R * F + 1;
        return (n, qs);
    }

    static ulong Polington(int k, int t)
    {
        ulong final;
        while (true)
        {
            var (n, qs) = PolingtonGen(k);
            final = n;
            if (PolingtonTest(n, t, qs))
                break;
            else
                polingtonOtbros.Add(n);
        }
        return final;
    }

    static List<ulong> gostOtbros = new List<ulong>();

    static ulong Gost(int q, int t, int c_flag)
    {
        var dist = new Random();

        while (true)
        {
            double xi = dist.NextDouble();
            if (c_flag == 0)
                xi = 0;

            double A = Math.Ceiling(Math.Pow(2, t - 1) / (double)q);
            double B = Math.Ceiling(Math.Pow(2, t - 1) * xi / (double)q);
            ulong N = (ulong)(A + B);

            if (N % 2 == 1) N++;

            int u = 0;
            while (true)
            {
                ulong p = ((ulong)(N + (ulong)u)) * (ulong)q + 1;
                if (BitLength(p) > t)
                    break;

                if (ModPow(2, p - 1, p) == 1 && ModPow(2, N + (ulong)u, p) != 1)
                    return p;

                gostOtbros.Add(p);
                u += 2;
            }
        }
    }

    static void Tables()
    {
        List<ulong> millerGens = new List<ulong>();
        List<List<ulong>> millerSost = new List<List<ulong>>();

        for (int i = 0; i < 10; i++)
        {
            millerOtbros.Clear();
            millerGens.Add(Miller(3, 5));
            millerSost.Add(new List<ulong>(millerOtbros));
        }

        Console.WriteLine("Таблица для метода Миллера");
        Console.Write("№");
        for (int i = 1; i <= 10; i++)
            Console.Write($"{i,20}");
        Console.WriteLine();

        Console.Write("P");
        foreach (var num in millerGens)
            Console.Write($"{num,20}");
        Console.WriteLine();

        Console.Write("Rs");
        foreach (var num in millerGens)
            Console.Write($"{(MillerRabin(num, 2) ? "+ " : "- "),20}");
        Console.WriteLine();

        Console.Write("K");
        foreach (var group in millerSost)
        {
            int c = group.Count(x => MillerRabin(x, 2));
            Console.Write($"{c,20}");
        }
        Console.WriteLine();

        List<ulong> polingtonGens = new List<ulong>();
        List<List<ulong>> polingtonSost = new List<List<ulong>>();

        for (int i = 0; i < 10; i++)
        {
            polingtonOtbros.Clear();
            polingtonGens.Add(Polington(3, 5));
            polingtonSost.Add(new List<ulong>(polingtonOtbros));
        }

        Console.WriteLine("Таблица для метода Полингтона");
        Console.Write("№");
        for (int i = 1; i <= 10; i++)
            Console.Write($"{i,20}");
        Console.WriteLine();

        Console.Write("P");
        foreach (var num in polingtonGens)
            Console.Write($"{num,20}");
        Console.WriteLine();

        Console.Write("Rs");
        foreach (var num in polingtonGens)
            Console.Write($"{(MillerRabin(num, 2) ? "+ " : "- "),20}");
        Console.WriteLine();

        Console.Write("K");
        foreach (var group in polingtonSost)
        {
            int c = group.Count(x => MillerRabin(x, 2));
            Console.Write($"{c,20}");
        }
        Console.WriteLine();

        List<ulong> gostGens = new List<ulong>();
        List<List<ulong>> gostSost = new List<List<ulong>>();

        for (int i = 0; i < 10; i++)
        {
            gostOtbros.Clear();
            gostGens.Add(Gost(7, (int)W, 1));
            gostSost.Add(new List<ulong>(gostOtbros));
        }

        Console.WriteLine("Таблица для метода по ГОСТ");
        Console.Write("№");
        for (int i = 1; i <= 10; i++)
            Console.Write($"{i,20}");
        Console.WriteLine();

        Console.Write("P");
        foreach (var num in gostGens)
            Console.Write($"{num,20}");
        Console.WriteLine();

        Console.Write("Rs");
        foreach (var num in gostGens)
            Console.Write($"{(MillerRabin(num, 2) ? "+ " : "- "),20}");
        Console.WriteLine();

        Console.Write("K");
        foreach (var group in gostSost)
        {
            int c = group.Count(x => MillerRabin(x, 2));
            Console.Write($"{c,20}");
        }
        Console.WriteLine();
    }

    static void FinalTest()
    {
        Console.WriteLine("Желаете проверить корректность алгоритмов на данных из приложения? (1 - Да, 2 - Нет)");
        if (!int.TryParse(Console.ReadLine(), out int cmd) || cmd != 1)
            return;

        // ======= МИЛЛЕР =======
        Console.WriteLine("\nМиллер:");
        var millerTests = new Dictionary<int, List<int>>()
    {
        {13, new List<int>{2, 3}},
        {29, new List<int>{2, 7}},
        {61, new List<int>{2, 3, 5}},
        {97, new List<int>{2, 3}},
        {157, new List<int>{2, 13}},
        {173, new List<int>{2, 43}},
        {179, new List<int>{2, 89}},
        {353, new List<int>{2, 11}},
        {419, new List<int>{2, 11, 19}},
        {461, new List<int>{2, 5, 23}},
        {617, new List<int>{2, 7, 11}},
        {821, new List<int>{2, 5, 41}},
        {1069, new List<int>{2, 3, 89}},
        {5953, new List<int>{2, 3, 31}},
        {6121, new List<int>{2, 3, 5, 17}},
        {6197, new List<int>{2, 1549}},
        {6373, new List<int>{2, 3, 59}},
        {335, new List<int>{2, 167}},
        {437, new List<int>{2, 109}},
        {657, new List<int>{2, 41}},
        {779, new List<int>{2, 389}},
        {1189, new List<int>{2, 3, 11}},
        {1191, new List<int>{2, 5, 7, 17}},
        {1533, new List<int>{2, 383}},
        {1785, new List<int>{2, 223}},
        {2071, new List<int>{2, 3, 5, 23}},
        {2327, new List<int>{2, 1163}},
        {2249, new List<int>{2, 281}},
        {3057, new List<int>{2, 191}},
        {3379, new List<int>{2, 3, 563}},
        {4009, new List<int>{2, 3, 167}},
        {4647, new List<int>{2, 23, 101}},
        {5007, new List<int>{2, 2503}},
        {5211, new List<int>{2, 5, 521}},
        {8891, new List<int>{2, 5, 7, 127}},
        {9451, new List<int>{2, 3, 5, 7}},
        {9837, new List<int>{2, 2459}},
        {9943, new List<int>{2, 3, 1657}},
        {6141, new List<int>{2, 5, 307}},
        {6259, new List<int>{2, 3, 7, 149}},
        {6951, new List<int>{2, 5, 139}},
        {7157, new List<int>{2, 1789}},
        {7483, new List<int>{2, 3, 29, 43}}
    };

        foreach (var kvp in millerTests)
        {
            int n = kvp.Key;
            List<int> qs = kvp.Value;
            double c = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (!MillerTest((ulong)n, 1, qs))
                    c += 1;
            }
            Console.WriteLine($"{n} {c / 1000.0}");
        }

        // ======= ПОЛИНГТОН =======
        Console.WriteLine("\nПолингтон:");
        var polingtonTests = new Dictionary<int, List<int>>()
    {
        {13, new List<int>{2}},
        {29, new List<int>{7}},
        {61, new List<int>{3, 5}},
        {97, new List<int>{3, 2}},
        {157, new List<int>{13}},
        {173, new List<int>{43}},
        {179, new List<int>{89}},
        {353, new List<int>{2, 11}},
        {419, new List<int>{11, 19}},
        {461, new List<int>{23}},
        {617, new List<int>{7, 11}},
        {821, new List<int>{41}},
        {1069, new List<int>{89}},
        {5953, new List<int>{3, 31}},
        {6121, new List<int>{5, 17}},
        {6197, new List<int>{1549}},
        {6373, new List<int>{3, 59}},
        {335, new List<int>{167}},
        {437, new List<int>{109}},
        {657, new List<int>{41}},
        {779, new List<int>{389}},
        {1189, new List<int>{3, 11}},
        {1191, new List<int>{7, 17}},
        {1533, new List<int>{383}},
        {1785, new List<int>{223}},
        {2071, new List<int>{5, 23}},
        {2327, new List<int>{1163}},
        {2249, new List<int>{281}},
        {3057, new List<int>{191}},
        {3379, new List<int>{563}},
        {4009, new List<int>{167}},
        {4647, new List<int>{101}},
        {5007, new List<int>{2503}},
        {5211, new List<int>{521}},
        {8891, new List<int>{127}},
        {9451, new List<int>{5, 7}},
        {9837, new List<int>{2459}},
        {9943, new List<int>{1657}},
        {6141, new List<int>{307}},
        {6259, new List<int>{149}},
        {6951, new List<int>{139}},
        {7157, new List<int>{1789}},
        {7483, new List<int>{29, 43}}
    };

        foreach (var kvp in polingtonTests)
        {
            int n = kvp.Key;
            List<int> qs = kvp.Value;
            double c = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (!PolingtonTest((ulong)n, 1, qs))
                    c += 1;
            }
            Console.WriteLine($"{n} {c / 1000.0}");
        }

        // ======= ГОСТ =======
        Console.WriteLine("\nГОСТ:");
        var gostTests = new List<(int f, int q, int t)>
    {
        (13, 3, 4), (41, 5, 6), (29, 7, 5), (31, 5, 5),
        (67, 11, 7), (199, 11, 8), (79, 13, 7), (131, 13, 8),
        (307, 17, 9), (613, 17, 10), (419, 19, 9), (277, 23, 9),
        (349, 29, 9), (311, 31, 9), (2221, 37, 11), (2297, 41, 11),
        (571, 19, 10), (599, 23, 10)
    };

        foreach (var (f, q, t) in gostTests)
        {
            ulong gostres = Gost(q, t, 0);
            string result = (gostres == (ulong)f) ? " = " : " != ";
            Console.WriteLine($"{q} {t} {gostres}{result}{f}");
        }
    }


    static void Main()
    {
        Eratosthenes();

        Console.Write("Введите требуемую битность чисел > ");
        W = ulong.Parse(Console.ReadLine());

        Tables();
        FinalTest();
    }
}
