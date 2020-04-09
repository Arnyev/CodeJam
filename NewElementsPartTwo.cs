using System;
using System.Linq;

namespace CodeJam
{
	class NewElementsPartTwo
	{
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new Point[cases];

			for (int i = 0; i < cases; i++)
			{
				var n = int.Parse(Console.ReadLine());
				var points = Enumerable.Range(0, n).Select(x => new Point(Console.ReadLine())).ToArray();
				solutions[i] = Solve(points);
			}

			for (int i = 0; i < cases; i++)
			{
				if (solutions[i].X > 0)
					Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
				else
					Console.WriteLine($"Case #{i + 1}: IMPOSSIBLE");
			}
		}

		public static Point Solve(Point[] points)
		{
			Fraction minf, maxf;
			FindInterval(points, out minf, out maxf);

			if (minf >= maxf)
				return new Point();

			if (minf.p == 0 && maxf.p > maxf.q)
				return new Point(1, 1);

			var floorMax = maxf.p / maxf.q;

			if (minf < new Fraction(floorMax, 1) && (minf < new Fraction(maxf.p - maxf.q, maxf.q) || maxf > new Fraction(floorMax, 1)))
			{
				if (minf.q == 1)
					return new Point((int)minf.p + 1, 1);
				else
				{
					var floorMin = minf.p / minf.q;
					return new Point((int)floorMin + 1, 1);
				}
			}

			if (minf.q == 1 && maxf.q == 1)
				return new Point((int)(minf.p * 2 + 1), 2);

			return GetRational(minf, maxf);
		}

		public static void FindInterval(Point[] points, out Fraction min, out Fraction max)
		{
			min = new Fraction(1, long.MaxValue);
			max = new Fraction(long.MaxValue, 1);

			for (int i = 0; i < points.Length; i++)
				for (int j = i + 1; j < points.Length; j++)
				{
					var p1 = points[i];
					var p2 = points[j];
					if (p1.X <= p2.X && p1.Y <= p2.Y)
						continue;
					if (p1.X >= p2.X && p1.Y >= p2.Y)
					{
						min = new Fraction(1);
						max = new Fraction(1);
						return;
					}

					var x = Math.Abs((long)p1.X - p2.X);
					var y = Math.Abs((long)p1.Y - p2.Y);
					var frac = new Fraction(y, x);

					if (p1.X > p2.X && frac < max)
						max = frac;
					else if (p1.X < p2.X && frac > min)
						min = frac;
				}
		}

		public static Point GetRational(Fraction min, Fraction max)
		{
			var avg = (min + max) / new Fraction(2);

			long lo = 0;
			long hi = (1L << 61) + 5;
			long best = GetBestQLimit(min, max, avg, ref lo, ref hi);

			if (best == -1)
				return new Point();

			var fStart = avg.LimitDenominator(best);

			var q = (long)fStart.q;
			hi = (long)fStart.p;
			lo = 0;
			best = -1;

			while (lo <= hi)
			{
				var mid = (lo + hi) / 2;
				var frac = new Fraction(mid, q);

				if (frac < max && frac > min)
				{
					best = mid;
					hi = mid - 1;
				}
				else
					lo = mid + 1;
			}

			if (best == -1)
				return new Point((int)fStart.p, (int)q);
			else
				return new Point((int)best, (int)q);
		}

		private static long GetBestQLimit(Fraction min, Fraction max, Fraction avg, ref long lo, ref long hi)
		{
			long best = -1;
			var init = avg.LimitDenominator(1);
			if (init > min && init < max)
			{
				hi = -1;
				best = 1;
			}

			while (lo <= hi)
			{
				var mid = (lo + hi) / 2;
				var frac = avg.LimitDenominator(mid);
				if (frac < max && frac > min)
				{
					best = mid;
					hi = mid - 1;
				}
				else
					lo = mid + 1;
			}

			return best;
		}

		public static int FindQ(long p, double min, double max)
		{
			int ans = -1;
			long s = 0;
			long e = int.MaxValue;
			while (s < e && s > 0)
			{
				long mid = (s + e) / 2;
				var res = (double)p / mid;
				if (res > min && res < max)
				{
					ans = (int)mid;
					e = mid - 1;
				}
				else if (res <= min)
					e = mid - 1;
				else
					s = mid + 1;
			}

			return ans;
		}
	}
}
