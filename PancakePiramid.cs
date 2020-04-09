using System;
using System.Collections.Generic;

namespace CodeJam
{
	class PancakePiramid
	{
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new int[cases];

			for (int i = 0; i < cases; i++)
			{
				var n = int.Parse(Console.ReadLine());

				var strings = Console.ReadLine().Split(' ');
				var list = new List<int>();
				list.Add(int.MinValue);
				foreach (var s in strings)
					list.Add(-int.Parse(s));

				solutions[i] = Solve(list);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		public static int Solve(List<int> stacks)
		{
			var rmq = new RMQHelper(stacks.ToArray());
			var handled = new bool[stacks.Count];
			HashSet<Point> handledIntervals = new HashSet<Point>();

			int mod = (int)1e9 + 7;
			int result = 0;
			for (int i = 2; i < handled.Length - 1; i++)
			{
				var current = -stacks[i];
				var maxL = -rmq.GetMinimum(1, i - 1).Y;
				if (maxL <= current)
					continue;
				var maxR = -rmq.GetMinimum(i + 1, handled.Length - 1).Y;
				if (maxR <= current)
					continue;

				var closestIndexLeft = UpperBound(1, i - 1, current, rmq);
				var closestIndexRight = LowerBound(i + 1, handled.Length, current, rmq);

				var interval = new Point(closestIndexLeft, closestIndexRight);
				if (!handledIntervals.Add(interval))
					continue;

				var left = -stacks[closestIndexLeft];
				var right = -stacks[closestIndexRight];
				var minMax = Math.Min(left, right);
				var size = closestIndexRight - closestIndexLeft - 1;
				long intervalsToRight = handled.Length - closestIndexRight;
				var diff = (long)(minMax - current);
				long resForCurrent = diff * size % mod * closestIndexLeft % mod * intervalsToRight % mod;
				resForCurrent %= mod;
				result = (result + (int)resForCurrent) % mod;
			}

			return result;
		}

		public static int UpperBound(int s, int endIndex, int seeked, RMQHelper h)
		{
			int ans = s;
			int e = endIndex;
			while (s <= e)
			{
				int mid = (s + e) / 2;
				var maxL = -h.GetMinimum(mid, endIndex).Y;

				if (maxL > seeked)
				{
					ans = mid;
					s = mid + 1;
				}
				else
				{
					e = mid - 1;
				}
			}

			return ans;
		}

		public static int LowerBound(int startIndex, int e, int seeked, RMQHelper h)
		{
			int ans = e;
			int s = startIndex;
			while (s <= e)
			{
				int mid = (s + e) / 2;
				var maxR = -h.GetMinimum(startIndex, mid).Y;

				if (maxR > seeked)
				{
					ans = mid;
					e = mid - 1;
				}
				else
					s = mid + 1;
			}

			return ans;
		}
	}
}
