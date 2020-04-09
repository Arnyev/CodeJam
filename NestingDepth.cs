using System;
using System.Linq;
using System.Text;

namespace CodeJam
{
	public static class NestingDepth
	{
		public static void Run()
		{
			var cases = int.Parse(Console.ReadLine());
			var solutions = new string[cases];

			for (int i = 0; i < cases; i++)
			{
				var vals = Console.ReadLine().Select(x => int.Parse(x.ToString())).ToArray();

				solutions[i] = Solve(vals);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i] }");
		}

		public class TreeNode
		{
			public TreeNode left;
			public TreeNode right;
			public int parentheses;
			public int minStart;
			public int minEnd;

			public TreeNode(int[] vals, int start, int end, int currentDepth)
			{
				if (start == end)
				{
					minStart = minEnd = start;
					parentheses = vals[start] - currentDepth;
					return;
				}

				parentheses = int.MaxValue;
				minStart = -1;
				for (int i = start; i <= end; i++)
					if (vals[i] - currentDepth < parentheses)
					{
						parentheses = vals[i] - currentDepth;
						minStart = i;
					}

				minEnd = minStart;
				while (minEnd + 1 <= end && vals[minEnd + 1] == vals[minStart])
					++minEnd;

				if (minStart > start)
					left = new TreeNode(vals, start, minStart - 1, currentDepth + parentheses);

				if (end > minEnd)
					right = new TreeNode(vals, minEnd + 1, end, currentDepth + parentheses);
			}

			public void Write(StringBuilder sb, int nestingDepth)
			{
				var cLeft = new char[parentheses];
				for (int i = 0; i < cLeft.Length; i++)
					cLeft[i] = '(';

				var cRight = new char[parentheses];
				for (int i = 0; i < cRight.Length; i++)
					cRight[i] = ')';

				var cMid = new char[minEnd - minStart + 1];
				var cD = (char)('0' + nestingDepth + parentheses);
				for (int i = 0; i < cMid.Length; i++)
					cMid[i] = cD;

				sb.Append(cLeft);
				left?.Write(sb, nestingDepth + parentheses);

				sb.Append(cMid);
				right?.Write(sb, nestingDepth + parentheses);

				sb.Append(cRight);
			}
		}

		public static string Solve(int[] vals)
		{
			var root = new TreeNode(vals, 0, vals.Length - 1, 0);
			var sb = new StringBuilder();
			root.Write(sb, 0);
			return sb.ToString();
		}
	}
}
