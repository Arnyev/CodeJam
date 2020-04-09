using System;
using System.Linq;

namespace CodeJam
{
	public static class GolfGophers
	{
		public static void Run()
		{
			var startString = Console.ReadLine();
			var splitS = startString.Split(' ');

			int cases = int.Parse(splitS[0]);
			int n = int.Parse(splitS[1]);
			int m = int.Parse(splitS[2]);
			
			var vals = new[] { 17, 13, 11, 16, 9, 7 };
			var rests = new int[vals.Length];

			for (int i = 0; i < cases; i++)
			{
				for (int j = 0; j < vals.Length; j++)
				{
					Console.WriteLine(string.Join(" ", Enumerable.Repeat(vals[j], 18).Select(x => x.ToString())));
					rests[j] = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).Sum() % vals[j];
				}

				var result = ChineseRemainderTheorem(vals, rests);
				Console.WriteLine(result);
				var responseFin = Console.ReadLine();
				if (int.Parse(responseFin) == -1)
					return;
			}
		}

		public static int ChineseRemainderTheorem(int[] n, int[] a)
		{
			int prod = 1;
			foreach (var i in n)
				prod *= i;

			int sm = 0;
			for (int i = 0; i < n.Length; i++)
			{
				int p = prod / n[i];
				sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
			}
			return sm % prod;
		}

		private static int ModularMultiplicativeInverse(int a, int mod)
		{
			int m0 = mod;
			int x0 = 0;
			int x1 = 1;

			if (mod == 1)
				return 0;

			while (a > 1)
			{
				int q = a / mod;
				int t = mod;
				mod = a % mod;
				a = t;
				t = x0;
				x0 = x1 - q * x0;
				x1 = t;
			}

			if (x1 < 0)
				x1 += m0;

			return x1;
		}
	}
}
