using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam
{
	public class BacterialTactics
	{
		public static int[,,,] grundyValues;

		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new int[cases];

			for (int i = 0; i < cases; i++)
			{
				var split = Console.ReadLine().Split(' ');

				var R = int.Parse(split[0]);
				var C = int.Parse(split[1]);

				var grid = new bool[C, R];

				for (int y = 0; y < R; y++)
				{
					var s = Console.ReadLine();
					for (int x = 0; x < C; x++)
						grid[x, y] = s[x] == '#';
				}

				solutions[i] = Solve(grid);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
		}

		public static int Solve(bool[,] grid)
		{
			grundyValues = new int[grid.GetLength(1), grid.GetLength(0), grid.GetLength(1), grid.GetLength(0)];
			Helpers.Memset(grundyValues, -1);

			var maxX = grid.GetLength(0) - 1;
			var maxY = grid.GetLength(1) - 1;
			int anyAnswer = GetGrundyValue(0, 0, maxY, maxX, grid);
			if (anyAnswer == 0)
				return 0;

			int answer = 0;

			for (int x = 0; x <= maxX; ++x)
			{
				bool accept = true;
				for (int y = 0; y <= maxY; ++y)
					if (grid[x, y])
					{
						accept = false;
						break;
					}

				if (!accept)
					continue;

				var left = GetGrundyValue(0, 0, maxY, x - 1, grid);
				var right = GetGrundyValue(0, x + 1, maxY, maxX, grid);

				var result = left ^ right;
				if (result == 0)
					answer += maxY + 1;
			}

			for (int y = 0; y <= maxY; ++y)
			{
				bool accept = true;
				for (int x = 0; x <= maxX; ++x)
					if (grid[x, y])
					{
						accept = false;
						break;
					}

				if (!accept)
					continue;

				var left = GetGrundyValue(0, 0, y - 1, maxX, grid);
				var right = GetGrundyValue(y + 1, 0, maxY, maxX, grid);

				var result = left ^ right;
				if (result == 0)
					answer += maxX + 1;
			}

			return answer;
		}

		public static int GetGrundyValue(int minY, int minX, int maxY, int maxX, bool[,] grid)
		{
			if (minY > maxY || minX > maxX)
				return 0;

			var currentlyComputed = grundyValues[minY, minX, maxY, maxX];
			if (currentlyComputed >= 0)
				return currentlyComputed;

			SortedSet<int> moves = new SortedSet<int>();

			for (int x = minX; x <= maxX; ++x)
			{
				bool legal = true;
				for (int y = minY; y <= maxY; ++y)
					if (grid[x, y])
					{
						legal = false;
						break;
					}

				if (!legal)
					continue;

				var left = GetGrundyValue(minY, minX, maxY, x - 1, grid);
				var right = GetGrundyValue(minY, x + 1, maxY, maxX, grid);
				var solution = left ^ right;
				moves.Add(solution);
			}

			for (int y = minY; y <= maxY; ++y)
			{
				bool legal = true;
				for (int x = minX; x <= maxX; ++x)
					if (grid[x, y])
					{
						legal = false;
						break;
					}
				if (!legal)
					continue;

				var left = GetGrundyValue(minY, minX, y - 1, maxX, grid);
				var right = GetGrundyValue(y + 1, minX, maxY, maxX, grid);
				var solution = left ^ right;
				moves.Add(solution);
			}

			int myGrundy = 0;
			foreach (var x in moves)
			{
				if (myGrundy < x)
					break;

				++myGrundy;
			}

			grundyValues[minY, minX, maxY, maxX] = myGrundy;
			return myGrundy;
		}
	}

	public class BacterialTacticsBasic
	{
		public static void Run()
		{
			int cases = int.Parse(Console.ReadLine());
			var solutions = new int[cases];

			for (int i = 0; i < cases; i++)
			{
				var split = Console.ReadLine().Split(' ');

				var R = int.Parse(split[0]);
				var C = int.Parse(split[1]);

				var grid = new CellState[C + 2, R + 2];
				for (int j = 0; j < R + 2; j++)
					grid[0, j] = grid[C + 1, j] = CellState.Infected;
				for (int j = 0; j < C + 2; j++)
					grid[j, 0] = grid[j, R + 1] = CellState.Infected;

				for (int y = 1; y <= R; y++)
				{
					var s = Console.ReadLine();
					for (int x = 1; x <= C; x++)
						grid[x, y] = s[x - 1] == '.' ? CellState.Clear : CellState.Radioactive;
				}

				solutions[i] = Solve(grid);
			}

			for (int i = 0; i < cases; i++)
				Console.WriteLine($"Case #{i + 1}: {solutions[i]}");
		}

		public static int Solve(CellState[,] grid)
		{
			var solutions = new List<BacterialTacticsAction>();

			var state = new BacterialTacticsState(grid);
			var actions = state.AvailableActions();
			foreach (var action in actions)
			{
				state.ApplyMove(action);
				if (GameSearch<BacterialTacticsState, BacterialTacticsAction>.CanAWin(state, false))
					solutions.Add(action);
				state.ReverseLastMove();
			}

			if (solutions.Count == 0)
				return 0;

			var stringsGood = new HashSet<string>(solutions.Select(x => x.ToString()));
			var solutionsAll = state.AvailableActionsNonReduced().Where(x => stringsGood.Contains(x.ToString())).ToList();
			return solutionsAll.Count;
		}

		public enum CellState : byte
		{
			Clear,
			Infected,
			Radioactive
		}

		public struct BacterialTacticsAction
		{
			public override string ToString()
			{
				if (minDim == maxDim)
					return $"X = {(isVertical ? minDim : otherDim)}, Y = {(isVertical ? otherDim : minDim)}";
				if (isVertical)
					return $"X = {otherDim}, Y from {minDim} to {maxDim}";
				else
					return $"Y = {otherDim}, X from {minDim} to {maxDim}";
			}

			public bool isVertical;
			public int minDim;
			public int maxDim;
			public int otherDim;
			public bool playerLoses;

			public List<Point> ChangedPoints()
			{
				var points = new List<Point>();

				if (isVertical)
					for (int i = minDim; i <= maxDim; i++)
						points.Add(new Point(otherDim, i));
				else
					for (int i = minDim; i <= maxDim; i++)
						points.Add(new Point(i, otherDim));

				return points;
			}

			public BacterialTacticsAction(int x, int y, bool isVertical, CellState[,] grid)
			{
				this.isVertical = isVertical;
				playerLoses = false;
				minDim = 0;
				maxDim = 0;

				if (isVertical)
				{
					otherDim = x;
					minDim = y;
					maxDim = y;
					while (true)
					{
						var cellState = grid[x, minDim - 1];
						if (cellState == CellState.Radioactive)
						{
							playerLoses = true;
							return;
						}
						if (cellState == CellState.Infected)
							break;
						minDim--;
					}
					while (true)
					{
						var cellState = grid[x, maxDim + 1];
						if (cellState == CellState.Radioactive)
						{
							playerLoses = true;
							return;
						}
						if (cellState == CellState.Infected)
							break;
						maxDim++;
					}
				}
				else
				{
					otherDim = y;
					minDim = x;
					maxDim = x;
					while (true)
					{
						var cellState = grid[minDim - 1, y];
						if (cellState == CellState.Radioactive)
						{
							playerLoses = true;
							return;
						}
						if (cellState == CellState.Infected)
							break;
						minDim--;
					}
					while (true)
					{
						var cellState = grid[maxDim + 1, y];
						if (cellState == CellState.Radioactive)
						{
							playerLoses = true;
							return;
						}
						if (cellState == CellState.Infected)
							break;
						maxDim++;
					}
				}
			}
		}

		public class BacterialTacticsState : IState<BacterialTacticsAction>
		{
			private readonly CellState[,] grid;
			private readonly List<BacterialTacticsAction> AppliedActions = new List<BacterialTacticsAction>();

			public BacterialTacticsState(CellState[,] grid)
			{
				this.grid = grid;
			}

			public List<BacterialTacticsAction> AvailableActions()
			{
				var maxX = grid.GetLength(0) - 1;
				var maxY = grid.GetLength(1) - 1;
				var result = new List<BacterialTacticsAction>();
				for (int x = 1; x < maxX; x++)
				{
					for (int y = 1; y < maxY; y++)
					{
						bool singleX = false;
						if (grid[x - 1, y] == CellState.Infected && grid[x, y] == CellState.Clear)
						{
							var action = new BacterialTacticsAction(x, y, false, grid);
							if (!action.playerLoses)
								result.Add(action);
							if (!action.playerLoses && action.minDim == action.maxDim)
								singleX = true;
						}

						if (grid[x, y - 1] == CellState.Infected && grid[x, y] == CellState.Clear)
						{
							var action = new BacterialTacticsAction(x, y, true, grid);
							if (!action.playerLoses && (action.minDim != action.maxDim || !singleX))
								result.Add(action);
						}
					}
				}

				return result;
			}

			public List<BacterialTacticsAction> AvailableActionsNonReduced()
			{
				var maxX = grid.GetLength(0) - 1;
				var maxY = grid.GetLength(1) - 1;
				var result = new List<BacterialTacticsAction>();
				for (int x = 1; x < maxX; x++)
				{
					for (int y = 1; y < maxY; y++)
					{
						if (grid[x, y] == CellState.Clear)
						{
							var actionH = new BacterialTacticsAction(x, y, false, grid);
							if (!actionH.playerLoses)
								result.Add(actionH);

							var actionV = new BacterialTacticsAction(x, y, true, grid);
							if (!actionV.playerLoses)
								result.Add(actionV);
						}
					}
				}

				return result;
			}

			public void ApplyMove(BacterialTacticsAction action)
			{
				if (action.isVertical)
					for (int i = action.minDim; i <= action.maxDim; i++)
						grid[action.otherDim, i] = CellState.Infected;
				else
					for (int i = action.minDim; i <= action.maxDim; i++)
						grid[i, action.otherDim] = CellState.Infected;

				AppliedActions.Add(action);
			}

			public void ReverseLastMove()
			{
				var action = AppliedActions[AppliedActions.Count - 1];

				if (action.isVertical)
					for (int i = action.minDim; i <= action.maxDim; i++)
						grid[action.otherDim, i] = CellState.Clear;
				else
					for (int i = action.minDim; i <= action.maxDim; i++)
						grid[i, action.otherDim] = CellState.Clear;

				AppliedActions.RemoveAt(AppliedActions.Count - 1);
			}
		}

		public interface IState<TAction>
		{
			void ApplyMove(TAction action);
			void ReverseLastMove();
			List<TAction> AvailableActions();
		}

		public class GameSearch<TState, TAction> where TState : IState<TAction>
		{
			public static bool CanAWin(TState state, bool playerAStarts)
			{
				return playerAStarts ? CanAWin(state) : WillBLose(state);
			}

			static bool CanAWin(TState state)
			{
				var actions = state.AvailableActions();

				foreach (var action in actions)
				{
					state.ApplyMove(action);
					if (WillBLose(state))
					{
						state.ReverseLastMove();
						return true;
					}

					state.ReverseLastMove();
				}

				return false;
			}

			static bool WillBLose(TState state)
			{
				var actions = state.AvailableActions();
				foreach (var action in actions)
				{
					state.ApplyMove(action);
					if (!CanAWin(state))
					{
						state.ReverseLastMove();
						return false;
					}

					state.ReverseLastMove();
				}

				return true;
			}
		}
	}
}
