using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public static class PowerArrangers
	{
		public static void Run()
		{
			var startString = Console.ReadLine();
			var splitS = startString.Split(' ');

			int cases = int.Parse(splitS[0]);
			int f = int.Parse(splitS[1]);

			for (int i = 0; i < cases; i++)
			{
				var lists1 = Enumerable.Range(0, 5).Select(x => new List<int>()).ToArray(); ;
				for (int j = 0; j < 119; j++)
				{
					Console.WriteLine(5 * j + 1);
					lists1[Console.ReadLine()[0] - 'A'].Add(j);
				}
				var list1 = lists1.Where(x => x.Count == 23).Single();
				var lists2 = Enumerable.Range(0, 5).Select(x => new List<int>()).ToArray(); ;

				foreach (var j in list1)
				{
					Console.WriteLine(5 * j + 2);
					lists2[Console.ReadLine()[0] - 'A'].Add(j);
				}

				var list2 = lists2.Where(x => x.Count == 5).Single();
				var lists3 = Enumerable.Range(0, 5).Select(x => new List<int>()).ToArray(); ;

				foreach (var j in list2)
				{
					Console.WriteLine(5 * j + 3);
					lists3[Console.ReadLine()[0] - 'A'].Add(j);
				}
				var list3 = lists3.Where(x => x.Count == 1).Single();
				Console.WriteLine(list3[0] * 5 + 4);
				var lastChar = Console.ReadLine()[0];

				var firstChar = (char)('A' + lists1.ToList().IndexOf(list1));
				var secondChar = (char)('A' + lists2.ToList().IndexOf(list2));
				var thirdChar = (char)('A' + lists3.ToList().IndexOf(list3));
				var fourthChar = "ABCDE".Where(x => !new[] { firstChar, secondChar, thirdChar, lastChar }.Contains(x)).Single();
				Console.WriteLine($"{firstChar}{secondChar}{thirdChar}{fourthChar}{lastChar}");
				if (Console.ReadLine() == "N")
					return;
			}
		}
	}
}
