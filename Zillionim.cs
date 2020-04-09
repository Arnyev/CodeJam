using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public static class Zillionim
	{
		public const long moveSize = 10000000000L;
		public const long blockSize = 19999999999L;
		public const long minChangeSize = 9999999999L;
		public const long maxInterval = 1000000000000L;
		public static long[] possibleMoves = GeneratePossibleMoves();
		public static int[,] grundyValues = new int[101, 101];

		public static void Run()
		{
			var splitS = Console.ReadLine().Split(' ');
			int games = int.Parse(splitS[0]);
			int needToWin = int.Parse(splitS[1]);
			Helpers.Memset(grundyValues, -1);

			for (int i = 0; i < games; i++)
			{
				var intervals = new List<PointL> { new PointL(1, maxInterval) };

				while (true)
				{
					var moveEnemy = long.Parse(Console.ReadLine());
					if (moveEnemy == -1)
						return;
					if (moveEnemy == -2 || moveEnemy == -3)
						break;

					ReplaceInterval(intervals, moveEnemy);
					long bestMove = FindBestMove(intervals);

					ReplaceInterval(intervals, bestMove);
					Console.WriteLine(bestMove);
				}
			}
		}

		public static long FindBestMove(List<PointL> intervals)
		{
			var intervalSizes = intervals.Select(x => x.Y - x.X + 1).ToArray();
			var grundies = intervalSizes.Select(GetGrundyValue).ToArray();
			var xorredValue = grundies.Aggregate((x, y) => x ^ y);
			long bestMove = -1;
			bool foundBest = false;
			foreach (var interval in intervals)
			{
				var intervalSize = interval.Y - interval.X + 1;
				var intervalGrundy = GetGrundyValue(intervalSize);
				var currentXor = xorredValue ^ intervalGrundy;
				foreach (var move in possibleMoves)
				{
					if (move > intervalSize)
						continue;

					if (bestMove == -1)
						bestMove = interval.X + move - moveSize;

					var grundyMove = GetGrundyOfMove(intervalSize, move);
					if ((grundyMove ^ currentXor) == 0)
					{
						bestMove = interval.X + move - moveSize;
						foundBest = true;
						break;
					}
				}
				if (foundBest)
					break;
			}

			return bestMove;
		}

		private static void ReplaceInterval(List<PointL> intervals, long moveEnemy)
		{
			int intervalIndex = 0;
			for (; intervalIndex < intervals.Count; intervalIndex++)
				if (intervals[intervalIndex].X <= moveEnemy && intervals[intervalIndex].Y > moveEnemy)
					break;

			var intervalToRemove = intervals[intervalIndex];
			intervals.RemoveAt(intervalIndex);
			var newIntervalL = new PointL(intervalToRemove.X, moveEnemy - 1);
			var newIntervalR = new PointL(moveEnemy + moveSize, intervalToRemove.Y);
			if (newIntervalL.Y - newIntervalL.X + 1 >= moveSize)
				intervals.Add(newIntervalL);
			if (newIntervalR.Y - newIntervalR.X + 1 >= moveSize)
				intervals.Add(newIntervalR);
		}

		public static long[] GeneratePossibleMoves()
		{
			var moves = new List<long>();
			for (int i = 1; i <= 100; i++)
			{
				moves.Add(i * moveSize);

				for (int j = 1; j <= i; j++)
					moves.Add((i + 1) * moveSize - j);
			}

			return moves.ToArray();
		}

		public static Point MapToMinMax(long intervalSize)
		{
			var max = intervalSize / moveSize;
			var min = intervalSize / blockSize;
			if (min * blockSize + moveSize <= intervalSize)
				min++;

			return new Point((int)min, (int)max);
		}


		public static int GetGrundyValue(long intervalSize)
		{
			var mapped = MapToMinMax(intervalSize);

			var currentlyComputed = grundyValues[mapped.X, mapped.Y];
			if (currentlyComputed >= 0)
				return currentlyComputed;

			SortedSet<int> moves = new SortedSet<int>();

			foreach (var move in possibleMoves)
			{
				if (move > intervalSize)
					break;
				int solution = GetGrundyOfMove(intervalSize, move);
				moves.Add(solution);
			}

			int myGrundy = 0;
			foreach (var x in moves)
			{
				if (myGrundy < x)
					break;

				++myGrundy;
			}

			grundyValues[mapped.X, mapped.Y] = myGrundy;
			return myGrundy;
		}

		private static int GetGrundyOfMove(long intervalSize, long move)
		{
			var intervalL = move - moveSize;
			var intervalR = intervalSize - move;
			var left = GetGrundyValue(intervalL);
			var right = GetGrundyValue(intervalR);
			var solution = left ^ right;
			return solution;
		}
	}
}
