using System;
using System.Linq;

namespace CodeJam
{
	public static class AlienRhyme
	{
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new int[cases];

			for (int i = 0; i < cases; i++)
			{
				int wordCount = int.Parse(Console.ReadLine());
				string[] words = Enumerable.Range(0, wordCount).Select(x => Console.ReadLine()).ToArray();
				solutions[i] = Solve(words);
			}

			for (int i = 0; i < cases; i++)
			{
				Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
			}
		}

		public static int Solve(string[] words)
		{
			var wordsR = words.Select(x => new string(x.Reverse().ToArray())).ToArray();
			var root = new TrieNode();
			for (int i = 0; i < root.Children.Length; i++)
				root.Children[i] = new TrieNode();

			foreach (var s in wordsR)
			{
				var node = root;

				foreach (var c in s)
				{
					var i = c - 'A';
					if (node.Children[i] == null)
						node.Children[i] = new TrieNode();
					node.Children[i].Count += 1;
					node = node.Children[i];
				}
			}

			return root.Children.Sum(n => n.GetSumWithChildren()) * 2;
		}

		public class TrieNode
		{
			public int Count;
			public TrieNode[] Children;
			public TrieNode() { Children = new TrieNode['Z' - 'A' + 1]; }
			public int GetSumWithChildren()
			{
				var sum = 0;
				foreach (var child in Children)
					sum += child?.GetSumWithChildren() ?? 0;
				if (Count >= sum * 2 + 2)
					return sum + 1;
				return sum;
			}
		}
	}
}
