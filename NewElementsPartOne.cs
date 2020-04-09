using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public class NewElementsPartOne
	{
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new int[cases];

			for (int i = 0; i < cases; i++)
			{
				var n = int.Parse(Console.ReadLine());
				var points = Enumerable.Range(0, n).Select(x => new Point(Console.ReadLine())).ToArray();
				solutions[i] = SolveRational(points);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
		}

		public static int SolveRational(Point[] points)
		{
			var ratios = new HashSet<Point>();
			for (int i = 0; i < points.Length; i++)
				for (int j = i + 1; j < points.Length; j++)
				{
					var p1 = points[i];
					var p2 = points[j];
					if (p1.X >= p2.X && p1.Y >= p2.Y ||
						p1.X <= p2.X && p1.Y <= p2.Y)
						continue;

					var x = Math.Abs(p1.X - p2.X);
					var y = Math.Abs(p1.Y - p2.Y);
					var gcd = Helpers.GCD(x, y);
					x /= gcd;
					y /= gcd;
					ratios.Add(new Point(x, y));
				}

			return ratios.Count + 1;
		}

		public static int SolveDouble(Point[] points)
		{
			var ratios = new HashSet<double>();
			for (int i = 0; i < points.Length; i++)
				for (int j = i + 1; j < points.Length; j++)
				{
					var p1 = points[i];
					var p2 = points[j];
					if (p1.X >= p2.X && p1.Y >= p2.Y ||
						p1.X <= p2.X && p1.Y <= p2.Y)
						continue;

					var x = Math.Abs(p1.X - p2.X);
					var y = Math.Abs(p1.Y - p2.Y);
					ratios.Add((double)x / y);
				}

			return ratios.Count + 1;
		}
	}
}
