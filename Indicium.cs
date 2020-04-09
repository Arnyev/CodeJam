using System;
using System.Linq;

namespace CodeJam
{
	public static class Indicium
	{
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new string[cases];
			var solutionArrays = new int[cases][][];

			for (int i = 0; i < cases; i++)
			{
				var split = Console.ReadLine().Split(' ');

				var n = int.Parse(split[0]);
				var k = int.Parse(split[1]);

				solutions[i] = Solve(n, k, out solutionArrays[i]) ? "POSSIBLE" : "IMPOSSIBLE";
			}

			for (int i = 0; i < cases; i++)
			{
				Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
				if (solutions[i] == "POSSIBLE")
				{
					foreach (var row in solutionArrays[i])
					{
						foreach (var value in row)
							Console.Write($"{value} ");
						Console.WriteLine();
					}
				}
			}
		}

		public static bool TestSolution(int[][] solution, int k)
		{
			var n = solution.Length;
			var trace = Enumerable.Range(0, n).Sum(x => solution[x][x]);
			if (trace != k)
				return false;

			var helper = new bool[n + 1];
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n + 1; j++)
					helper[j] = false;
				for (int j = 0; j < n; j++)
					if (helper[solution[i][j]])
						return false;
					else
						helper[solution[i][j]] = true;

				for (int j = 0; j < n + 1; j++)
					helper[j] = false;

				for (int j = 0; j < n; j++)
					if (helper[solution[j][i]])
						return false;
					else
						helper[solution[j][i]] = true;
			}

			return true;
		}

		public static bool Solve(int n, int k, out int[][] solution)
		{
			solution = null;
			if (k == n + 1)
				return false;
			if (k == n * n - 1)
				return false;
			if (n == 3 && (k == 5 || k == 7))
				return false;

			int[] permutation;

			if (k % n == 0)
			{
				solution = GetBaseSolution(n);
				permutation = Enumerable.Range(0, n + 1).ToArray();
				permutation[1] = k / n;
				permutation[k / n] = 1;
			}
			else if (k % n != 2 && k % n != n - 2)
			{
				solution = GetBaseSolution(n);
				permutation = GetPermutation(n, k);
				Helpers.Swap(ref solution[0], ref solution[1]);
			}
			else
			{
				solution = GetSolution2Vals(n);
				permutation = GetPermutation2Val(n, k);
			}

			solution = solution.Select(x => x.Select(y => permutation[y]).ToArray()).ToArray();

			return true;
		}

		private static int[] GetPermutation2Val(int n, int k)
		{
			var permutation = Enumerable.Range(0, n + 1).ToArray();

			if (k % n == 2)
			{
				var new1 = k / n;
				var new2 = new1 + 1;

				permutation[1] = new1;
				permutation[2] = new2;

				if (new1 != 1)
					permutation[new2] = 1;

				if (new1 != 2 && new1 != 1)
					permutation[new1] = 2;
			}
			else
			{
				var new1 = k / n + 1;
				var new2 = new1 - 1;
				permutation[1] = new1;
				permutation[2] = new2;

				if (new1 == 3)
					permutation[3] = 1;
				else
				{
					permutation[new1] = 1;
					permutation[new2] = 2;
				}
			}

			return permutation;
		}

		private static int[] GetPermutation(int n, int k)
		{
			var permutation = Enumerable.Range(0, n + 1).ToArray();

			if (k > (n * (n + 1) / 2))
			{
				int newVal = k / n + 1;

				var d = newVal * n - k;
				if (d == 1)
				{
					newVal++;
					d += n;
				}

				var v1 = Math.Max(1, newVal - d + 1);
				var v2 = 2 * newVal - d - v1;

				permutation[1] = newVal;
				permutation[2] = v1;
				permutation[n] = v2;

				if (v1 != 1 && v1 != 2)
					permutation[v1] = 1;
				if (v2 != 2 && v1 != 2)
					permutation[v2] = 2;
				else if (v1 == 2)
					permutation[v2] = 1;
				if (newVal != n)
					permutation[newVal] = n;
			}
			else
			{
				var newVal = k / n;
				var d = k - newVal * n;
				if (d == 1)
				{
					newVal--;
					d += n;
				}
				var v1 = Math.Min(n, newVal + d - 1);
				var v2 = 2 * newVal + d - v1;
				permutation[1] = newVal;
				permutation[2] = v1;
				permutation[n] = v2;

				if (v1 != n)
					permutation[v1] = n;

				if (v2 != 2)
					permutation[v2] = newVal == 2 ? 1 : 2;

				if (newVal != 1 && newVal != 2)
					permutation[newVal] = 1;
			}

			return permutation;
		}

		public static int[][] GetSolution2Vals(int n)
		{
			var solution = new int[n][];
			for (int i = 0; i < solution.Length; i++)
				solution[i] = new int[n];

			for (int i = 2; i < n - 1; i++)
				for (int j = 0; j < n; j++)
					solution[j][i] = (j - i + n) % n + 1;

			solution[0][0] = solution[1][1] = 2;
			solution[1][0] = solution[0][1] = 1;
			solution[0][n - 1] = n;
			solution[1][n - 1] = 3;
			solution[2][n - 1] = 2;
			solution[n - 1][n - 1] = 1;

			for (int i = 3; i < n - 1; i++)
				solution[i][n - 1] = i + 1;

			var takenFirstColumn = new bool[n + 1];
			takenFirstColumn[1] = true;
			takenFirstColumn[2] = true;

			var takenSecondColumn = new bool[n + 1];
			takenSecondColumn[1] = true;
			takenSecondColumn[2] = true;

			var takenByRows = new bool[n, n + 1];
			for (int i = 0; i < solution.Length; i++)
				for (int j = 0; j < n; j++)
					takenByRows[i, solution[i][j]] = true;

			for (int i = 2; i < n; i++)
			{
				for (int k = 1; k <= n; k++)
					if (!takenFirstColumn[k] && !takenByRows[i, k])
					{
						takenFirstColumn[k] = true;
						takenByRows[i, k] = true;
						solution[i][0] = k;
						break;
					}

				for (int k = 1; k <= n; k++)
					if (!takenSecondColumn[k] && !takenByRows[i, k])
					{
						takenSecondColumn[k] = true;
						takenByRows[i, k] = true;
						solution[i][1] = k;
						break;
					}
			}

			return solution;
		}

		public static int[][] GetBaseSolution(int n)
		{
			var solution = new int[n][];
			for (int i = 0; i < solution.Length; i++)
			{
				solution[i] = new int[n];
				for (int j = 0; j < n; j++)
					solution[i][j] = ((i - j + n) % n) + 1;
			}

			return solution;
		}
	}
}
