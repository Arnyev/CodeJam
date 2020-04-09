using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public static class PotteryLottery
	{
		public static void Run(IConsole console)
		{
			var cases = int.Parse(console.ReadLine());

			for (int i = 0; i < cases; i++)
			{
				int[][] vases = new int[20][];
				for (int j = 0; j < vases.Length; j++)
					vases[j] = Array.Empty<int>();

				for (int k = 0; k < 10; k++)
					for (int j = 0; j < 5; j++)
					{
						var str = console.ReadLine();
						var dayNr = int.Parse(str);
						if (dayNr == -1)
							return;
						console.WriteLine($"{k + 1} 1");
					}

				for (int k = 0; k < 10; k++)
				{
					var dayNr = int.Parse(console.ReadLine());
					console.WriteLine($"{k + 11} 0");
					var str = console.ReadLine().Split(' ');
					vases[k + 10] = str.Skip(1).Select(x => int.Parse(x)).ToArray();
				}

				var badVases = vases.Select((x, index) => new Point(index, x.Length)).ToArray().OrderBy(x => x.Y)
					.Skip(15).Select(x => x.X).ToList();

				for (int k = 0; k < 4; k++)
					foreach (var vase in badVases)
					{
						var dayNr = int.Parse(console.ReadLine());
						console.WriteLine($"{vase + 1} 1");
					}

				var goodVases = Enumerable.Range(10, 10).Where(x => !badVases.Contains(x)).ToList();
				var goodVaseContents = goodVases.Select(x =>
				{
					var dayNr = int.Parse(console.ReadLine());
					console.WriteLine($"{x + 1} 0");
					return console.ReadLine().Split(' ').Skip(1).Select(y => int.Parse(y)).ToArray();
				}).ToArray();

				var sorted = goodVases.Zip(goodVaseContents, (v, cv) => new Point(v, cv.Length)).OrderBy(x => x.Y).ToList();
				var goodVase = sorted[0].X;
				badVases = sorted.Skip(1).Select(x => x.X).ToList();

				for (int k = 0; k < 4; k++)
				{
					var dayNr = int.Parse(console.ReadLine());
					console.WriteLine($"{badVases[k] + 1} 1");
					var dayNr2 = int.Parse(console.ReadLine());
					console.WriteLine($"{badVases[k] + 1} 1");
					var dayNr3 = int.Parse(console.ReadLine());
					console.WriteLine($"{badVases[k] + 1} 1");
				}
				var dayNra = int.Parse(console.ReadLine());
				console.WriteLine($"{badVases[0] + 1} 1");
				var dayNrb = int.Parse(console.ReadLine());
				console.WriteLine($"{badVases[0] + 1} 1");
				var dayNrc = int.Parse(console.ReadLine());

				console.WriteLine($"{goodVase + 1} 100");
			}
		}
	}

	public class PotteryRunTest : IConsole
	{
		public int cases = 100;
		Random rand = new Random(0);

		public List<int>[] vaseContents;
		string lastQuery = string.Empty;
		int queriesNumber = 0;
		string day = "day";
		public int fails = 0;
		public int wins = 0;

		public void RefreshVases()
		{
			vaseContents = Enumerable.Range(0, 20).Select(x => new List<int>()).ToArray();
		}

		public string ReadLine()
		{
			if (lastQuery == string.Empty)
			{
				RefreshVases();
				lastQuery = day;
				return cases.ToString();
			}

			if (lastQuery == "day")
				return (queriesNumber + 1).ToString();

			vaseContents[rand.Next() % vaseContents.Length].Add(queriesNumber + 1);
			var ints = lastQuery.Split().Select(x => int.Parse(x)).ToArray();
			var v = ints[0];
			var nr = ints[1];
			queriesNumber++;
			if (queriesNumber == 100)
			{
				vaseContents[v - 1].Add(nr);

				var vasesWithLengths = vaseContents.Select((x, i) => new Point(i, x.Count)).OrderBy(x => x.Y).ToArray();
				var bestVase = vaseContents[vasesWithLengths[0].X];

				var lastwins = wins;
				if (vasesWithLengths[0].Y == vasesWithLengths[1].Y)
					fails++;
				else if (!bestVase.Contains(100))
					fails++;
				else if (bestVase.Distinct().Count() != bestVase.Count)
					fails++;
				else
					wins++;

				if (wins + fails == cases - 1)
				{
					Console.WriteLine(wins);
					Console.WriteLine(fails);
				}

				RefreshVases();
				queriesNumber = 0;
				return 1.ToString();
			}
			else if (nr == 0)
			{
				lastQuery = day;
				if (vaseContents[v - 1].Count == 0)
					return "0";
				return $"{vaseContents[v - 1].Count} {string.Join(' ', vaseContents[v - 1])}";
			}
			else
			{
				vaseContents[v - 1].Add(nr);
				return (queriesNumber + 1).ToString();
			}
		}

		public void WriteLine(string s)
		{
			lastQuery = s;
		}

		public void WriteLine(int i)
		{
			WriteLine(i.ToString());
		}
	}
}
