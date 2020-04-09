using System;
using System.Linq;

namespace CodeJam
{
	public static class Draupnir
	{
		public static void Run(IConsole console)
		{
			var strs = console.ReadLine().Split(' ');
			var cases = int.Parse(strs[0]);

			for (int i = 0; i < cases; i++)
			{
				console.WriteLine(220.ToString());
				var n1 = ulong.Parse(console.ReadLine());
				console.WriteLine(54.ToString());
				var n2 = ulong.Parse(console.ReadLine());

				ulong[] solution = Solve(n1, n2);

				console.WriteLine($"{solution[0]} {solution[1]} {solution[2]} {solution[3]} {solution[4]} {solution[5]}");

				var result = int.Parse(console.ReadLine());
				if (result == -1)
					return;
			}
		}

		public static ulong[] Solve(ulong n1, ulong n2)
		{
			var r4 = n1 / (1ul << 55);
			var r5 = n1 / (1ul << 44) - r4 * (1ul << 11);
			var r6 = n1 / (1ul << 36) - r4 * (1ul << 19) - r5 * (1ul << 8);
			var r1 = n2 / (1ul << 54);
			var r2 = n2 / (1ul << 27) - r1 * (1ul << 27);
			var r3 = n2 / (1ul << 18) - r1 * (1ul << 36) - r2 * (1ul << 9) - (r4 * 16 + r5 * 2 + r6) / (1ul << 9);

			var ar = new[] { r1, r2, r3, r4, r5, r6 };
			return ar;
		}
	}

	public class DraupnirTest : IConsole
	{
		static Random rand = new Random();
		int cases = rand.Next() % 10000;
		bool start = true;
		int receivedTries = 0;
		ulong[] currentValues;
		int lastTry = 0;
		bool success = false;

		void UpdateValues()
		{
			currentValues = new ulong[6];
			while (currentValues.All(x => x == 0))
				currentValues = new ulong[] { (ulong)rand.Next() % 101, (ulong)rand.Next() % 101, (ulong)rand.Next() % 101,
					(ulong)rand.Next() % 101, (ulong)rand.Next() % 101, (ulong)rand.Next() % 101 };
		}

		public string ReadLine()
		{
			if (start)
			{
				UpdateValues();
				start = false;
				return cases.ToString();
			}

			if (receivedTries == 0)
			{
				UpdateValues();
				return success ? "1" : "0";
			}
			ulong val = GetVal(lastTry);

			return val.ToString();
		}

		private ulong GetVal(int day)
		{
			return (day > 63 ? 0 : (currentValues[0] << day)) +
				(day > 127 ? 0 : (currentValues[1] << (day / 2))) +
				(day > 191 ? 0 : (currentValues[2] << (day / 3))) +
				(day > 255 ? 0 : (currentValues[3] << (day / 4))) +
				(day > 319 ? 0 : (currentValues[4] << (day / 5))) +
				(day > 383 ? 0 : (currentValues[5] << (day / 6)));
		}

		public void WriteLine(string s)
		{
			if (receivedTries == 2)
			{
				receivedTries = 0;
				success = s.Split(' ').Select(x => ulong.Parse(x)).SequenceEqual(currentValues);
				if (!success)
					throw new Exception("Wrong answer");
			}
			else
			{
				receivedTries++;
				lastTry = int.Parse(s);
			}
		}

		public void WriteLine(int i) => WriteLine(i.ToString());
	}
}