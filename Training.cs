using System;
using System.Linq;

namespace CodeJam
{
	public static class Training
	{
		public static void Run()
		{
			var sCases = Console.ReadLine();
			int cases = int.Parse(sCases);
			var solutions = new int[cases];

			for (int i = 0; i < cases; i++)
			{
				var first = Console.ReadLine();

				var split = first.Split(' ');
				var N = int.Parse(split[0]);
				var P = int.Parse(split[1]);
				var SArray = Console.ReadLine().Split(' ').Take(N).
					Select(x => int.Parse(x)).ToArray();

				solutions[i] = SolveCase(SArray, P);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		public static int SolveCase(int[] SArray, int P)
		{
			Array.Sort(SArray);
			var prefixSums = new int[SArray.Length];
			prefixSums[0] = SArray[0];

			for (int i = 1; i < SArray.Length; i++)
				prefixSums[i] = prefixSums[i - 1] + SArray[i];

			var min = SArray[P - 1] * P - prefixSums[P - 1];
			for (int i = P; i < SArray.Length; i++)
			{
				var currentRequired = SArray[i] * P - prefixSums[i] + prefixSums[i - P];
				if (currentRequired < min)
					min = currentRequired;
			}

			return min;
		}
	}
}
