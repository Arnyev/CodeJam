namespace CodeJam
{
	public struct Point
	{
		public int X;
		public int Y;

		public Point(string s)
		{
			var split = s.Split();
			X = int.Parse(split[0]);
			Y = int.Parse(split[1]);
		}

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static bool operator ==(Point p1, Point p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(Point p1, Point p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public override string ToString()
		{
			return $"{X} {Y}";
		}

		public override bool Equals(object obj)
		{
			var p = obj as Point?;
			return p.HasValue && p.Value.X == X && p.Value.Y == Y;
		}

		public override int GetHashCode()
		{
			uint rol5 = ((uint)X << 5) | ((uint)X >> 27);
			return ((int)rol5 + X) ^ Y;
		}
	}

	public struct PointL
	{
		public long X;
		public long Y;

		public PointL(long x, long y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return $"{X} {Y}";
		}
	}
}
