using System;
using System.Linq;

namespace CodeJam
{
	public class ESAbATAdTest : IConsole
	{
		static Random rand = new Random();
		int cases = rand.Next() % 10000;
		bool start = true;
		int receivedTries = 0;
		int[] currentValues;
		int lastTry = 0;
		bool success = false;
		int valuesSize = 100;

		void UpdateValues()
		{
			currentValues = Enumerable.Range(0, valuesSize).Select(x => rand.Next() % 2).ToArray();
		}

		public string ReadLine()
		{
			if (start)
			{
				UpdateValues();
				start = false;
				return $"{cases} {valuesSize}";
			}

			if (lastTry == 0)
			{
				UpdateValues();
				return success ? "Y" : "N";
			}

			return currentValues[lastTry - 1].ToString();
		}

		public void WriteLine(string s)
		{
			if (s.Length == valuesSize)
			{
				receivedTries = 0;
				lastTry = 0;
				success = s.Select(x => x == '1' ? 1 : 0).SequenceEqual(currentValues);
				if (!success)
					throw new Exception();
				return;
			}

			receivedTries++;
			if (receivedTries > 149)
				throw new Exception();

			if (receivedTries % 10 == 1)
			{
				var move = rand.Next() % 4;
				if (move % 2 == 1)
					currentValues = currentValues.Select(x => x == 1 ? 0 : 1).ToArray();
				if (move / 2 == 1)
					currentValues = currentValues.Reverse().ToArray();
			}

			lastTry = int.Parse(s);
		}

		public void WriteLine(int i) => WriteLine(i.ToString());
	}

	public static class ESAbATAd
	{
		public static void Run(IConsole console)
		{
			var ints = console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
			var cases = ints[0];
			var bits = ints[1];
			var pairs = bits / 2;
			for (int i = 0; i < cases; i++)
			{
				var values = new int[pairs];
				var matching = new bool[pairs];
				var known = 0;
				var knownDifferent = -1;
				var knownMatching = -1;

				int tryNumber = 0;
				while (known < pairs)
				{
					if (tryNumber != 0)
					{
						tryNumber += 2;
						if (knownDifferent != -1)
						{
							console.WriteLine(knownDifferent + 1);
							var val = int.Parse(console.ReadLine());
							if (val != values[knownDifferent])
								for (int j = 0; j < known; j++)
									if (!matching[j])
										values[j] = values[j] == 1 ? 0 : 1;
						}
						if (knownMatching != -1)
						{
							console.WriteLine(knownMatching + 1);
							var val = int.Parse(console.ReadLine());
							if (val != values[knownMatching])
								for (int j = 0; j < known; j++)
									if (matching[j])
										values[j] = values[j] == 1 ? 0 : 1;
						}

						if (knownDifferent == -1 || knownMatching == -1)
						{
							console.WriteLine(1);
							console.ReadLine();
						}
					}

					for (; tryNumber == 0 || tryNumber % 10 != 0 && known < pairs; tryNumber += 2, known++)
					{
						console.WriteLine(known + 1);
						var valL = int.Parse(console.ReadLine());
						console.WriteLine(bits - known);
						var valR = int.Parse(console.ReadLine());
						values[known] = valL;

						if (valL == valR)
						{
							knownMatching = known;
							matching[known] = true;
						}
						else
							knownDifferent = known;
					}
				}

				var chars = new char[bits];
				for (int j = 0; j < pairs; j++)
				{
					chars[j] = values[j] == 1 ? '1' : '0';
					if (matching[j])
						chars[bits - 1 - j] = chars[j];
					else
						chars[bits - 1 - j] = values[j] == 1 ? '0' : '1';
				}
				console.WriteLine(new string(chars));
				var result = console.ReadLine();
				if (result[0] == 'N')
					return;
			}
		}
	}
}
