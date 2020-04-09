using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public static class Contransmutation
	{
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new int[cases];

			for (int i = 0; i < cases; i++)
			{
				var m = int.Parse(Console.ReadLine());

				var points = Enumerable.Range(0, m).Select(x =>
				{
					var s = Console.ReadLine().Split(' ');
					var g1 = int.Parse(s[0]);
					var g2 = int.Parse(s[1]);
					return new Point(g1 - 1, g2 - 1);
				}).ToArray();

				var amounts = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
				solutions[i] = Solve(points, amounts);
			}

			for (int i = 0; i < cases; i++)
				if (solutions[i] == int.MaxValue)
					Console.WriteLine($"Case #{i + 1}: UNBOUNDED");
				else
					Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
		}

		public static int Solve(Point[] edges, int[] amounts)
		{
			List<int>[] reverseGraph = GetReverseGraph(edges);
			bool[] reachable = GetReachable(reverseGraph);
			bool[] reachableFromAmount = GetReachableFromAmount(edges, amounts);

			if (CheckForInfiniteResult(reachable, reachableFromAmount, edges))
				return int.MaxValue;

			var counts = new int[edges.Length];
			counts[0] = 1;

			for (int i = 0; i < reachable.Length; i++)
			{
				if (!reachable[i] || !reachableFromAmount[i])
					continue;

				SolveRec(i, edges, counts, reachable);
			}

			var solution = 0;
			for (int i = 0; i < edges.Length; i++)
			{
				solution += (int)((long)amounts[i] * counts[i] % 1000000007);
				solution %= 1000000007;
			}
			return solution;
		}

		public static bool CheckForInfiniteResult(bool[] reachable, bool[] reachableFromAmount, Point[] edges)
		{
			for (int v = 0; v < reachableFromAmount.Length; v++)
			{
				if (!reachableFromAmount[v])
					continue;

				if (edges[v].X == v && reachable[edges[v].Y])
					return true;

				if (edges[v].Y == v && reachable[edges[v].X])
					return true;
			}

			int[] sccs = FindSccs(edges);

			for (int v = 0; v < reachableFromAmount.Length; v++)
			{
				if (!reachableFromAmount[v] || !reachable[v])
					continue;

				if (sccs[edges[v].Y] == sccs[v] && sccs[edges[v].X] == sccs[v])
					return true;
				if (sccs[edges[v].X] == sccs[v] && reachable[edges[v].Y])
					return true;
				if (sccs[edges[v].Y] == sccs[v] && reachable[edges[v].X])
					return true;
			}

			return false;
		}

		private static List<int>[] GetReverseGraph(Point[] edges)
		{
			var reverseGraph = new List<int>[edges.Length];
			for (int i = 0; i < reverseGraph.Length; i++)
				reverseGraph[i] = new List<int>();

			for (int i = 0; i < edges.Length; i++)
			{
				reverseGraph[edges[i].X].Add(i);
				reverseGraph[edges[i].Y].Add(i);
			}

			return reverseGraph;
		}

		private static bool[] GetReachable(List<int>[] reverseGraph)
		{
			bool[] reachable = new bool[reverseGraph.Length];
			reachable[0] = true;
			var queue = new Queue<int>();
			queue.Enqueue(0);
			while (queue.Count != 0)
			{
				var vertex = queue.Dequeue();
				foreach (var neighbour in reverseGraph[vertex])
					if (!reachable[neighbour])
					{
						reachable[neighbour] = true;
						queue.Enqueue(neighbour);
					}
			}

			return reachable;
		}

		private static bool[] GetReachableFromAmount(Point[] edges, int[] amounts)
		{
			Queue<int> queue = new Queue<int>();
			bool[] reachableFromAmount = new bool[edges.Length];

			for (int i = 0; i < amounts.Length; i++)
			{
				if (amounts[i] == 0)
					continue;
				if (reachableFromAmount[i])
					continue;

				reachableFromAmount[i] = true;
				queue.Enqueue(i);
				while (queue.Count != 0)
				{
					var vertex = queue.Dequeue();
					var left = edges[vertex].X;
					var right = edges[vertex].Y;
					if (!reachableFromAmount[left])
					{
						reachableFromAmount[left] = true;
						queue.Enqueue(left);
					}
					if (!reachableFromAmount[right])
					{
						reachableFromAmount[right] = true;
						queue.Enqueue(right);
					}
				}
			}

			return reachableFromAmount;
		}

		public static int SolveRec(int i, Point[] edges, int[] counts, bool[] reachable)
		{
			if (!reachable[i])
				return 0;

			if (counts[i] != 0)
				return counts[i];

			var left = SolveRec(edges[i].X, edges, counts, reachable);
			var right = SolveRec(edges[i].Y, edges, counts, reachable);

			int result = (left + right) % 1000000007;

			counts[i] = result;
			return result;
		}

		internal struct VertexNode
		{
			public int LowNr;       //the smallest index of any node known to be reachable from v, including v itself
			public int DfsIndex;    //depth first search first visit index, not index in the array
			public int Scc;         //nr of the strongly connected component vertice is in
		}

		public static int[] FindSccs(Point[] edges)
		{
			int curInd = 0;
			int sccnr = 0;

			Stack<int> componentsStack = new Stack<int>();
			Stack<int> searchStack = new Stack<int>();

			VertexNode[] array = new VertexNode[edges.Length];
			for (int i = 0; i < array.Length; ++i)
				array[i].DfsIndex = array[i].Scc = -1;

			for (int i = 0; i < array.Length; ++i)
			{
				if (array[i].Scc != -1)
					continue;
				searchStack.Push(i);
				while (searchStack.Count != 0)
				{
					int v = searchStack.Peek();
					if (array[v].Scc != -1)
					{
						searchStack.Pop();
						continue;
					}

					var out1 = edges[v].X;
					var out2 = edges[v].Y;
					if (array[v].DfsIndex == -1)
					{
						componentsStack.Push(v);
						array[v].DfsIndex = array[v].LowNr = curInd++;
						if (array[out1].DfsIndex == -1)
							searchStack.Push(out1);
						if (array[out2].DfsIndex == -1)
							searchStack.Push(out2);
					}
					else
					{
						if (array[out1].Scc == -1 && array[out1].LowNr < array[v].LowNr)
							array[v].LowNr = array[out1].LowNr;
						if (array[out2].Scc == -1 && array[out2].LowNr < array[v].LowNr)
							array[v].LowNr = array[out2].LowNr;

						if (array[v].LowNr == array[v].DfsIndex)
						{
							int k;
							do
							{
								k = componentsStack.Pop();
								array[k].Scc = sccnr;
							}
							while (k != v);
							sccnr++;
						}
						searchStack.Pop();
						continue;
					}
				}
			}

			var scc = new int[edges.Length];
			for (int i = 0; i < scc.Length; i++)
				scc[i] = array[i].Scc;

			return scc;
		}
	}
}
