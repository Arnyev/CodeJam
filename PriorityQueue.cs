using System;

namespace CodeJam
{
	public class PriorityQueue
	{
		public int[] Values; // pointer to array of elements in heap 
		public int CurrentSize; // Current number of elements in min heap 

		public PriorityQueue(int capacity)
		{
			CurrentSize = 0;
			Values = new int[capacity];
		}

		public void InsertKey(int k)
		{
			if (CurrentSize == Values.Length)
				throw new Exception("Overflow: Could not insert Key");

			CurrentSize++;
			int i = CurrentSize - 1;
			Values[i] = k;

			UpdateHeap(i);
		}

		private void UpdateHeap(int i)
		{
			while (i != 0 && Values[(i - 1) / 2] > Values[i])
			{
				var tmp = Values[i];
				Values[i] = Values[(i - 1) / 2];
				Values[(i - 1) / 2] = tmp;
				i = (i - 1) / 2; ;
			}
		}

		public void DecreaseKey(int i, int new_val)
		{
			Values[i] = new_val;
			UpdateHeap(i);
		}

		public int ExtractMin()
		{
			if (CurrentSize <= 0)
				return int.MaxValue;

			if (CurrentSize == 1)
			{
				CurrentSize--;
				return Values[0];
			}

			int root = Values[0];
			Values[0] = Values[CurrentSize - 1];
			CurrentSize--;
			MinHeapify(0);

			return root;
		}

		public void DeleteKey(int i)
		{
			DecreaseKey(i, int.MinValue);
			ExtractMin();
		}

		// A recursive method to heapify a subtree with the root at given index 
		// This method assumes that the subtrees are already heapified 
		void MinHeapify(int i)
		{
			int l = 2 * i + 1;
			int r = 2 * i + 2;
			int smallest = i;
			if (l < CurrentSize && Values[l] < Values[i])
				smallest = l;
			if (r < CurrentSize && Values[r] < Values[smallest])
				smallest = r;
			if (smallest != i)
			{
				var tmp = Values[i];
				Values[i] = Values[smallest];
				Values[smallest] = tmp;

				MinHeapify(smallest);
			}
		}
	}
}
