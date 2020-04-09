using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public static class FairFight
	{
		public static void Run()
		{
			var cases = int.Parse(Console.ReadLine());
			var solutions = new long[cases];

			for (int i = 0; i < cases; i++)
			{
				var ints = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
				var n = ints[0];
				var k = ints[1];

				var c = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
				var d = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();

				solutions[i] = Solve(c, d, k);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		public static long Solve(int[] c, int[] d, int k)
		{
			var cl = new List<int>(c);
			var dl = new List<int>(d);//todo ogarnac
			cl.Insert(0, int.MaxValue);
			dl.Insert(0, int.MaxValue);
			var c1 = cl.ToArray();
			var d1 = dl.ToArray();
			var hC = new RMQHelper(c1.Select(x => -x).ToArray());
			var hD = new RMQHelper(d1.Select(x => -x).ToArray());

			long score = 0;

			for (int i = 1; i < c1.Length; i++)
			{
				var swordScore = c1[i];
				var opponentsScore = d1[i];
				if (opponentsScore - swordScore > k)
					continue;

				long minL, maxL, minR, maxR;

				if (swordScore - opponentsScore <= k)
					minL = i;
				else if (i == 1 || -hD.GetMinimum(1, i - 1).Y < swordScore - k)
					minL = 1;
				else
					minL = LowerBound(1, i, swordScore, k, hD);

				if (swordScore - opponentsScore <= k)
					minR = i;
				else if (i == c1.Length - 1 || -hD.GetMinimum(i + 1, c1.Length - 1).Y < swordScore - k)
					minR = c1.Length - 1;
				else
					minR = UpperBound(i, c1.Length - 1, swordScore, k, hD);

				if (i == 1 || c1[i - 1] >= swordScore || d1[i - 1] - swordScore > k)
					maxL = i;
				else
					maxL = LowerBoundB(1, i - 1, swordScore, k, hC, hD);

				if (i == c1.Length - 1 || c1[i + 1] > swordScore || d1[i + 1] - swordScore > k)
					maxR = i;
				else
					maxR = UpperBoundB(i + 1, c1.Length - 1, swordScore, k, hC, hD);

				minL = Math.Max(minL, maxL);
				minR = Math.Min(minR, maxR);
				long wrongIntervals = (minR - i + 1) * (i - minL + 1);
				if (swordScore - opponentsScore <= k)
					wrongIntervals = 0;
				long rightIntervals = (maxR - i + 1) * (i - maxL + 1);

				long right = Math.Max(0, rightIntervals - wrongIntervals);
				score += right;
			}

			return score;
		}

		public static int UpperBoundB(int s, int e, int seeked, int k, RMQHelper hC, RMQHelper hD)
		{
			int startInterval = s;
			int ans = s;

			while (s <= e)
			{
				int mid = (s + e) / 2;
				var maxCL = -hC.GetMinimum(startInterval, mid).Y;
				var maxDL = -hD.GetMinimum(startInterval, mid).Y;

				if (maxCL <= seeked && maxDL - seeked <= k)
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

		public static int LowerBoundB(int s, int e, int seeked, int k, RMQHelper hC, RMQHelper hD)
		{
			int endInterval = e;
			int ans = e;

			while (s <= e)
			{
				int mid = (s + e) / 2;
				var maxCL = -hC.GetMinimum(mid, endInterval).Y;
				var maxDL = -hD.GetMinimum(mid, endInterval).Y;

				if (maxCL < seeked && maxDL - seeked <= k)
				{
					ans = mid;
					e = mid - 1;
				}
				else
				{
					s = mid + 1;
				}
			}

			return ans;
		}

		public static int UpperBound(int s, int e, int seeked, int k, RMQHelper h)
		{
			var startInterval = s;
			int ans = s;

			while (s <= e)
			{
				int mid = (s + e) / 2;
				var maxDL = -h.GetMinimum(startInterval, mid).Y;

				if (seeked - maxDL > k)
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

		public static int LowerBound(int s, int e, int seeked, int k, RMQHelper h)
		{
			int endInterval = e;
			int ans = e;

			while (s <= e)
			{
				int mid = (s + e) / 2;
				var maxDL = -h.GetMinimum(mid, endInterval).Y;

				if (seeked - maxDL > k)
				{
					ans = mid;
					e = mid - 1;
				}
				else
				{
					s = mid + 1;
				}
			}

			return ans;
		}

	
	}
}
