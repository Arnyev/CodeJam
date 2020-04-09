using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public static class Pylons
	{
		public static void Run()
		{
			var sCases = Console.ReadLine();
			int cases = int.Parse(sCases);
			var solutions = new string[cases];
			var solutionPaths = new Point[cases][];

			for (int i = 0; i < cases; i++)
			{
				var split = Console.ReadLine().Split(' ');

				var R = int.Parse(split[0]);
				var C = int.Parse(split[1]);

				solutions[i] = Solve(R, C, out solutionPaths[i]) ? "POSSIBLE" : "IMPOSSIBLE";
			}

			for (int i = 0; i < cases; i++)
			{
				Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
				if (solutions[i] == "POSSIBLE")
					solutionPaths[i].ToList().ForEach(x => Console.WriteLine($"{x.X} {x.Y}"));
			}
		}

		public static bool TestPath(Point[] path)
		{
			for (int i = 0; i < path.Length; i++)
				for (int j = i + 1; j < path.Length; j++)
					if (path[i].X == path[j].X && path[i].Y == path[j].Y)
						return false;

			for (int i = 1; i < path.Length; i++)
			{
				var p1 = path[i - 1];
				var p2 = path[i];
				if (p1.X == p2.X || p1.Y == p2.Y)
					return false;

				var d1a = p1.X + p1.Y;
				var d2a = p2.X + p2.Y;

				var d1b = p1.X - p1.Y;
				var d2b = p2.X - p2.Y;
				if (d1a == d2a || d1b == d2b)
					return false;
			}

			return true;
		}

		public static bool Solve(int R, int C, out Point[] path)
		{
			path = null;

			if (R == 2 && C < 5)
				return false;

			if (C == 2 && R < 5)
				return false;

			if (R == 3 && C == 3)
				return false;

			if (R == 2)
			{
				path = GetPathForTwos(C);
				return true;
			}
			if (C == 2)
			{
				path = GetPathForTwos(R).Select(x => new Point(x.Y, x.X)).ToArray();
				return true;
			}
			if (R == 3)
			{
				path = GetPathForThrees(C);
				return true;
			}
			if (C == 3)
			{
				path = GetPathForThrees(R).Select(x => new Point(x.Y, x.X)).ToArray();
				return true;
			}
			if (R == 4 && C > 4)
			{
				path = GetPathForFours(C);
				return true;
			}
			if (C == 4 && R > 4)
			{
				path = GetPathForFours(R).Select(x => new Point(x.Y, x.X)).ToArray();
				return true;
			}

			var g = CreateGraph(R, C);
			var degrees = GetVertexDegrees(g);

			var n = R * C;

			//if (degrees.All(x => x > n / 2) || CheckPosa(degrees) || CheckOre(degrees, g))
			//	return true;

			List<int> pathList;
			var possible = GetPath(g, out pathList);
			if (possible)
				path = pathList.Select(x => new Point(x / C + 1, x % C + 1)).ToArray();

			return possible;
		}

		public static Point[] GetPathForTwos(int C)
		{
			var points = new List<Point>();
			for (int i = 0; i < C; i++)
			{
				points.Add(new Point(2, (i + 2) % C + 1));
				points.Add(new Point(1, i + 1));
			}

			return points.ToArray();
		}

		public static Point[] GetPathForThrees(int C)
		{
			var points = new List<Point>();
			for (int i = 0; i < C; i++)
			{
				points.Add(new Point(1, i + 1	));
				points.Add(new Point(3, (i + 1) % C + 1));
				points.Add(new Point(2, (i + 3) % C + 1));
			}

			return points.ToArray();
		}

		public static Point[] GetPathForFours(int C)
		{
			var points = new List<Point>();
			for (int i = 0; i < C; i++)
			{
				points.Add(new Point(1, i + 1));
				points.Add(new Point(3, (i + 1) % C + 1));
				points.Add(new Point(2, (i + 3) % C + 1));
				points.Add(new Point(4, (i + 2) % C + 1));
			}

			return points.ToArray();
		}

		private static bool GetPath(bool[,] graph, out List<int> path)
		{
			path = new List<int>();
			var currentlyTaken = new bool[graph.GetLength(0)];

			var rand = new Random();
			for (int ind = 0; ind < graph.GetLength(0); ind++)
			{
				int i;
				if (currentlyTaken.Length <= 25)
					i = ind;
				else
					i = rand.Next() % currentlyTaken.Length;

				currentlyTaken[i] = true;

				path.Add(i);

				if (CheckBrutalRec(i, 1, rand, path, currentlyTaken, graph))
					return true;
				currentlyTaken[i] = false;
			}

			return false;
		}

		private static bool CheckBrutalRec(int currentIndex, int inCycle, Random rand, List<int> path, bool[] currentlyTaken, bool[,] graph)
		{
			if (inCycle == currentlyTaken.Length)
				return true;

			for (int ind = 0; ind < graph.GetLength(0); ind++)
			{
				int i;
				if (currentlyTaken.Length <= 25)
					i = ind;
				else
					i = rand.Next() % currentlyTaken.Length;

				if (!currentlyTaken[i] && graph[currentIndex, i])
				{
					currentlyTaken[i] = true;
					path.Add(i);

					if (CheckBrutalRec(i, inCycle + 1, rand, path, currentlyTaken, graph))
						return true;

					currentlyTaken[i] = false;
					path.RemoveAt(path.Count - 1);
				}
			}

			return false;
		}

		private static bool CheckOre(int[] degrees, bool[,] graph)
		{
			var n = degrees.Length;
			for (int i = 0; i < n - 1; i++)
				for (int j = i + 1; j < n; j++)
					if (!graph[i, j] && degrees[i] + degrees[j] < n)
						return false;

			return true;
		}

		private static bool CheckPosa(int[] degrees)
		{
			var degreesCopy = degrees.ToArray();
			Array.Sort(degreesCopy);
			return degreesCopy.Take(degreesCopy.Length / 2).Select((v, i) => new Point(v, i)).All(p => p.X > p.Y);
		}

		public static int[] GetVertexDegrees(bool[,] g)
		{
			var degrees = new int[g.GetLength(0)];
			for (int i = 0; i < degrees.Length; i++)
				for (int j = 0; j < degrees.Length; j++)
					if (g[i, j])
						degrees[i]++;

			return degrees;
		}

		public static bool[,] CreateGraph(int R, int C)
		{
			var g = new bool[R * C, R * C];
			for (int i = 0; i < R; i++)
				for (int j = 0; j < C; j++)
				{
					var d1a = j - i;
					var d1b = j + i;
					var i1 = i * C + j;

					for (int k = i + 1; k < R; k++)
						for (int l = 0; l < C; l++)
						{
							if (l == j)
								continue;
							var d2a = l - k;
							var d2b = l + k;
							var i2 = k * C + l;
							if (d1a != d2a && d1b != d2b)
								g[i1, i2] = g[i2, i1] = true;
						}
				}

			return g;
		}
	}
}
