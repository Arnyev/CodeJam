using System;
using System.Linq;

namespace CodeJam
{
	public class Vestigium
	{
		public static void Run()
		{
			var cases = int.Parse(Console.ReadLine());
			var solutions = new string[cases];

			for (int i = 0; i < cases; i++)
			{
				var n = int.Parse(Console.ReadLine());
				var matrix = new int[n][];
				for (int j = 0; j < n; j++)
					matrix[j] = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();

				var k = Enumerable.Range(0, n).Sum(x => matrix[x][x]);
				var r = n - Enumerable.Range(0, n).Count(x => CheckRow(matrix[x]));
				var c = n - Enumerable.Range(0, n).Count(x => CheckColumn(matrix, x));

				solutions[i] = $"{k} {r} {c}";
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		public static bool CheckRow(int[] row)
		{
			var good = 0;
			var exists = new bool[row.Length];
			foreach (var x in row)
				if (!exists[x - 1])
				{
					exists[x - 1] = true;
					good++;
				}
			return good == row.Length;
		}

		public static bool CheckColumn(int[][] matrix, int column)
		{
			var good = 0;
			var exists = new bool[matrix.Length];
			for (int i = 0; i < matrix.Length; i++)
				if (!exists[matrix[i][column] - 1])
				{
					exists[matrix[i][column] - 1] = true;
					good++;
				}
			return good == matrix.Length;
		}
	}
}
