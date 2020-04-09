using System;

namespace CodeJam
{
	public static class Helpers
	{
		public static int GCD(int a, int b)
		{
			int remainder;

			while (b != 0)
			{
				remainder = a % b;
				a = b;
				b = remainder;
			}

			return a;
		}
		public static long GCDL(long a, long b)
		{
			long remainder;

			while (b != 0)
			{
				remainder = a % b;
				a = b;
				b = remainder;
			}

			return a;
		}
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		public static void Memset(int[,,,] array, int value)
		{
			for (int k = 0; k < array.GetLength(2); k++)
				for (int l = 0; l < array.GetLength(3); l++)
					array[0, 0, k, l] = value;

			var size = array.GetLength(2) * array.GetLength(3);
			for (int i = 0; i < array.GetLength(0); i++)
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (i == 0 && j == 0)
						continue;
					var offset = (i * array.GetLength(1) + j) * size;
					Array.Copy(array, 0, array, offset, size);
				}
		}

		public static void Memset(int[,] array, int value)
		{
			for (int i = 0; i < array.GetLength(1); i++)
				array[0, i] = value;

			var size = array.GetLength(1);
			for (int i = 1; i < array.GetLength(0); i++)
			{
				var offset = i * size;
				Array.Copy(array, 0, array, offset, size);
			}
		}
	}
}
