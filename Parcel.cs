using System;
using System.Collections.Generic;

namespace CodeJam
{
	public static class Parcel
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
				var R = int.Parse(split[0]);
				var C = int.Parse(split[1]);
				var currentParcels = new bool[R, C];
				for (int j = 0; j < R; j++)
				{
					var s = Console.ReadLine();
					for (int k = 0; k < C; k++)
						currentParcels[j, k] = s[k] == '1';
				}

				solutions[i] = SolveCase(currentParcels);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		public static int SolveCase(bool[,] currentParcels)
		{
			var R = currentParcels.GetLength(0);
			var C = currentParcels.GetLength(1);

			int[,] distances = GetDistances(currentParcels);
			var maxDist = GetMaxDistance(distances);
			if (maxDist == 0)
				return 0;

			var listsByDist = new List<Point>[maxDist + 1];
			for (int i = 0; i <= maxDist; i++)
				listsByDist[i] = new List<Point>();

			for (int i = 0; i < distances.GetLength(0); i++)
				for (int j = 0; j < distances.GetLength(1); j++)
					listsByDist[distances[i, j]].Add(new Point(i, j));

			return GetDist(maxDist, listsByDist, R, C);
		}

		private static int GetDist(int maxDist, List<Point>[] listsByDist, int R, int C)
		{
			var minP = 0;
			var maxP = R + C - 2;
			var minN = 0;
			var maxN = R + C - 2;

			for (int expectedDist = maxDist - 1; expectedDist >= 0; expectedDist--)
			{
				for (int analysedDist = maxDist; analysedDist > expectedDist; analysedDist--)
				{
					foreach (var p in listsByDist[analysedDist])
					{
						var x = p.X;
						var y = p.Y;
						var diagP = x - y + C - 1;
						var diagN = x + y;

						minP = Math.Max(minP, diagP - expectedDist);
						maxP = Math.Min(maxP, diagP + expectedDist);
						minN = Math.Max(minN, diagN - expectedDist);
						maxN = Math.Min(maxN, diagN + expectedDist);

						if (minP > maxP || minN > maxN)
							return expectedDist + 1;
						if (minN == maxN && minP == maxP && (minP - C + 1 + minN) % 2 == 1)
							return expectedDist + 1;
					}
				}
			}

			return 0;
		}

		private static int GetMaxDistance(int[,] distances)
		{
			var maxDist = 0;

			for (int i = 0; i < distances.GetLength(0); i++)
				for (int j = 0; j < distances.GetLength(1); j++)
					if (distances[i, j] > maxDist)
						maxDist = distances[i, j];
			return maxDist;
		}

		private static int[,] GetDistances(bool[,] currentParcels)
		{
			var R = currentParcels.GetLength(0);
			var C = currentParcels.GetLength(1);

			var distances = new int[R, C];
			for (int i = 0; i < R; i++)
				for (int j = 0; j < C; j++)
					distances[i, j] = int.MaxValue;

			var queue = new Queue<Point>();
			for (int i = 0; i < R; i++)
				for (int j = 0; j < C; j++)
					if (currentParcels[i, j])
					{
						distances[i, j] = 0;
						queue.Enqueue(new Point(i, j));
					}

			while (queue.Count != 0)
			{
				var p = queue.Dequeue();
				var x = p.X;
				var y = p.Y;

				var curVal = distances[x, y];
				if (x > 0 && distances[x - 1, y] > curVal + 1)
				{
					distances[x - 1, y] = curVal + 1;
					queue.Enqueue(new Point(x - 1, y));
				}
				if (y > 0 && distances[x, y - 1] > curVal + 1)
				{
					distances[x, y - 1] = curVal + 1;
					queue.Enqueue(new Point(x, y - 1));
				}
				if (x < R - 1 && distances[x + 1, y] > curVal + 1)
				{
					distances[x + 1, y] = curVal + 1;
					queue.Enqueue(new Point(x + 1, y));
				}
				if (y < C - 1 && distances[x, y + 1] > curVal + 1)
				{
					distances[x, y + 1] = curVal + 1;
					queue.Enqueue(new Point(x, y + 1));
				}
			}

			return distances;
		}
	}
}
