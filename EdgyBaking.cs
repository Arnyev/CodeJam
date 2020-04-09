using System;
using System.Linq;

namespace CodeJam
{
	public static class EdgyBaking
	{
		public static void Run()
		{
			var startString = Console.ReadLine();
			int cases = int.Parse(startString);

			var solutions = new double[cases];

			for (int i = 0; i < cases; i++)
			{
				var s = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
				var n = s[0];
				var p = s[1];
				var points = Enumerable.Range(0, n).Select(x => Console.ReadLine().Split(' ')
					.Select(y => int.Parse(y)).ToArray())
					.Select(a => new Point(a[0], a[1])).ToArray();
				solutions[i] = Solve(points, p);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i].ToString()}");
		}

		public static double Solve(Point[] points, int p)
		{
			var baseSum = points.Sum(x => (x.X + x.Y) * 2);
			var minSum = points.Sum(x => Math.Min(x.X, x.Y) * 2);
			var maxSum = points.Sum(x => Math.Sqrt(x.X * x.X + x.Y * x.Y) * 2);

			if (maxSum + baseSum <= p)
				return maxSum + baseSum;

			if (minSum + baseSum <= p)
				return p;

			p -= baseSum;

			var possible = new bool[minSum + 1];
			var left = new double[minSum + 1];
			possible[0] = true;

			var possibleCopy = new bool[minSum + 1];
			var leftCopy = new double[minSum + 1];

			foreach (var point in points)
			{
				var min = 2 * Math.Min(point.X, point.Y);
				var max = 2 * Math.Sqrt(point.X * point.X + point.Y * point.Y);
				var diff = max - min;

				for (int i = 0; i < possible.Length; i++)
				{
					if (possible[i])
					{
						possibleCopy[i] = true;
						possibleCopy[i + min] = true;
						leftCopy[i + min] = Math.Max(left[i + min], left[i] + diff);
						leftCopy[i] = Math.Max(left[i], leftCopy[i]);
					}
				}

				Helpers.Swap(ref possible, ref possibleCopy);
				Helpers.Swap(ref left, ref leftCopy);
			}

			double best = 0;
			for (int i = 0; i < possible.Length; i++)
			{
				if (i > p)
					return best + baseSum;

				if (possible[i])
				{
					var leftI = left[i];
					if (i + leftI >= p)
						return p + baseSum;
					if (i + leftI > best)
						best = i + leftI;
				}
			}

			return best + baseSum;
		}
	}
}
