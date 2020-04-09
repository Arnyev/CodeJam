using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public class ParentingPartneringReturns
	{
		public static void Run()
		{
			var cases = int.Parse(Console.ReadLine());
			var solutions = new string[cases];

			for (int i = 0; i < cases; i++)
			{
				var n = int.Parse(Console.ReadLine());
				var activities = new Point[n];
				for (int j = 0; j < n; j++)
				{
					var ints = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
					activities[j] = new Point(ints[0], ints[1]);
				}

				var colors = new char[n];
				bool fail = false;
				for (int v = 0; v < n; v++)
				{
					if (colors[v] != 0)
						continue;
					if (fail)
						break;

					colors[v] = 'C';
					Queue<Point> queue = new Queue<Point>();
					queue.Enqueue(new Point(v, 1));

					while (queue.Count != 0)
					{
						var point = queue.Dequeue();
						var myColor = point.Y;
						var index = point.X;
						var otherColor = myColor == 'J' ? 'C' : 'J';
						var myStart = activities[index].X;
						var myEnd = activities[index].Y;

						for (int w = 0; w < n; w++)
						{
							if (w == index)
								continue;

							var otherStart = activities[w].X;
							var otherEnd = activities[w].Y;
							bool collides = CheckCollision(myStart, myEnd, otherStart, otherEnd);

							if (!collides)
								continue;
							if (colors[w] == myColor)
							{
								fail = true;
								break;
							}

							if (colors[w] == otherColor)
								continue;

							colors[w] = otherColor;
							queue.Enqueue(new Point(w, otherColor));
						}

						if (fail)
							break;
					}
				}

				if (fail)
					solutions[i] = "IMPOSSIBLE";
				else
					solutions[i] = new string(colors);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		private static bool CheckCollision(int myStart, int myEnd, int otherStart, int otherEnd)
		{
			var commonStart = myStart == otherStart;
			var commonEnd = myEnd == otherEnd;
			var outside = otherStart < myStart && otherEnd > myEnd;
			var startsInside = otherStart > myStart && otherStart < myEnd;
			var endsInside = otherEnd > myStart && otherEnd < myEnd;

			var collides = commonStart | commonEnd | outside | startsInside | endsInside;
			return collides;
		}
	}
}
