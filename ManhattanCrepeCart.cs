using System;
using System.Linq;

namespace CodeJam
{
	public static class ManhattanCrepeCart
	{
		public enum Direction
		{
			N = 0,
			S = 1,
			W = 2,
			E = 3
		}

		public struct PointWithDirection
		{
			public int X;
			public int Y;
			public Direction Dir;

			public PointWithDirection(int x, int y, Direction direction)
			{
				X = x;
				Y = y;
				Dir = direction;
			}

			public PointWithDirection(string[] s)
			{
				X = int.Parse(s[0]);
				Y = int.Parse(s[1]);
				Dir = (Direction)Enum.Parse(typeof(Direction), s[2]);
			}

			public override string ToString()
			{
				return $"{X} {Y} {Dir}";
			}
		}
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new Point[cases];

			for (int i = 0; i < cases; i++)
			{
				var split = Console.ReadLine().Split(' ');
				var P = int.Parse(split[0]);
				var Q = int.Parse(split[1]);
				var pointDirections = Enumerable.Range(0, P).Select(x => Console.ReadLine().Split(' '))
					.Select(s => new PointWithDirection(s)).ToArray();

				solutions[i] = Solve(pointDirections, Q);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		public static Point Solve(PointWithDirection[] points, int Q)
		{
			int minX = 0;
			int maxX = Q;
			int minY = 0;
			int maxY = Q;

			foreach (var p in points)
			{
				switch (p.Dir)
				{
					case Direction.N:
						minY = Math.Max(p.Y + 1, minY);
						break;
					case Direction.S:
						maxY = Math.Min(p.Y - 1, maxY);
						break;
					case Direction.W:
						maxX = Math.Min(p.X - 1, maxX);
						break;
					case Direction.E:
						minX = Math.Max(p.X + 1, minX);
						break;
				}
			}

			if (minX > maxX || minY > maxY)
				return SolveComplicated(points, Q);

			return new Point(minX, minY);
		}

		public static Point SolveComplicated(PointWithDirection[] points, int Q)
		{
			var valsX = points.Where(x => x.Dir == Direction.W).Select(x => x.X - 1)
				.Concat(points.Where(x => x.Dir == Direction.E).Select(x => x.X + 1))
				.ToList();

			valsX.Add(0);
			valsX.Add(Q);
			valsX.Add(Q + 1);

			valsX = valsX.Distinct().OrderBy(x => x).ToList();
			var intervalsX = valsX.Zip(valsX.Skip(1), (x, y) => new Point(x, y)).ToArray();

			var valsY = points.Where(x => x.Dir == Direction.S).Select(x => x.Y - 1)
				.Concat(points.Where(x => x.Dir == Direction.N).Select(x => x.Y + 1))
				.ToList();

			valsY.Add(0);
			valsY.Add(Q);
			valsY.Add(Q + 1);

			valsY = valsY.Distinct().OrderBy(x => x).ToList();
			var intervalsY = valsY.Zip(valsY.Skip(1), (x, y) => new Point(x, y)).ToArray();

			var peopleCountX = new int[intervalsX.Length];
			var peopleCountY = new int[intervalsY.Length];

			foreach (var p in points)
			{
				switch (p.Dir)
				{
					case Direction.N:
						for (int i = 0; i < intervalsY.Length; i++)
							if (p.Y + 1 < intervalsY[i].Y)
								peopleCountY[i]++;
						break;
					case Direction.S:
						for (int i = 0; i < intervalsY.Length; i++)
							if (p.Y - 1 >= intervalsY[i].X)
								peopleCountY[i]++;
						break;
					case Direction.W:
						for (int i = 0; i < intervalsX.Length; i++)
							if (p.X - 1 >= intervalsX[i].X)
								peopleCountX[i]++;
						break;
					case Direction.E:
						for (int i = 0; i < intervalsX.Length; i++)
							if (p.X + 1 < intervalsX[i].Y)
								peopleCountX[i]++;
						break;
				}
			}

			int indMaxX = 0;
			var valMaxX = 0;

			for (int i = 0; i < intervalsX.Length; i++)
				if (peopleCountX[i] > valMaxX)
				{
					indMaxX = i;
					valMaxX = peopleCountX[i];
				}

			int indMaxY = 0;
			var valMaxY = 0;
			for (int i = 0; i < intervalsY.Length; i++)
				if (peopleCountY[i] > valMaxY)
				{
					indMaxY = i;
					valMaxY = peopleCountY[i];
				}

			return new Point(intervalsX[indMaxX].X, intervalsY[indMaxY].X);
		}
	}
}