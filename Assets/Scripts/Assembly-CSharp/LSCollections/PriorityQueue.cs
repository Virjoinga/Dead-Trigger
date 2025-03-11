using System;
using System.Collections;
using System.Diagnostics;

namespace LSCollections
{
	public class PriorityQueue : ICollection, IEnumerable
	{
		private class SynchronizedPriorityQueue : PriorityQueue
		{
			private PriorityQueue queue;

			private object root;

			public override int Count
			{
				get
				{
					lock (root)
					{
						return queue.Count;
					}
				}
			}

			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			public override object SyncRoot
			{
				get
				{
					return root;
				}
			}

			public SynchronizedPriorityQueue(PriorityQueue queue)
			{
				if (queue == null)
				{
					throw new ArgumentNullException("queue");
				}
				this.queue = queue;
				root = queue.SyncRoot;
			}

			public override void Enqueue(object element)
			{
				lock (root)
				{
					queue.Enqueue(element);
				}
			}

			public override object Dequeue()
			{
				lock (root)
				{
					return queue.Dequeue();
				}
			}

			public override void Remove(object element)
			{
				lock (root)
				{
					queue.Remove(element);
				}
			}

			public override void Clear()
			{
				lock (root)
				{
					queue.Clear();
				}
			}

			public override bool Contains(object element)
			{
				lock (root)
				{
					return queue.Contains(element);
				}
			}

			public override object Peek()
			{
				lock (root)
				{
					return queue.Peek();
				}
			}

			public override void CopyTo(Array array, int index)
			{
				lock (root)
				{
					queue.CopyTo(array, index);
				}
			}

			public override IEnumerator GetEnumerator()
			{
				lock (root)
				{
					return queue.GetEnumerator();
				}
			}
		}

		private class DefaultComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				if (!(y is IComparable))
				{
					throw new ArgumentException("Item does not implement IComparable.");
				}
				IComparable comparable = x as IComparable;
				return comparable.CompareTo(y);
			}
		}

		private class Node
		{
			private Node[] forward;

			private object element;

			public Node this[int index]
			{
				get
				{
					return forward[index];
				}
				set
				{
					forward[index] = value;
				}
			}

			public object Element
			{
				get
				{
					return element;
				}
			}

			public Node(object element, int level)
			{
				forward = new Node[level];
				this.element = element;
			}
		}

		private class PriorityQueueEnumerator : IEnumerator
		{
			private PriorityQueue owner;

			private Node head;

			private Node currentNode;

			private bool moveResult;

			private long version;

			public object Current
			{
				get
				{
					if (currentNode == head || currentNode == null)
					{
						throw new InvalidOperationException("The enumerator is positioned before the first element of the collection or after the last element.");
					}
					return currentNode.Element;
				}
			}

			public PriorityQueueEnumerator(PriorityQueue owner)
			{
				this.owner = owner;
				version = owner.version;
				head = owner.header;
				Reset();
			}

			public void Reset()
			{
				if (version != owner.version)
				{
					throw new InvalidOperationException("The PriorityQueue was modified after the enumerator was created.");
				}
				currentNode = head;
				moveResult = true;
			}

			public bool MoveNext()
			{
				if (version != owner.version)
				{
					throw new InvalidOperationException("The PriorityQueue was modified after the enumerator was created.");
				}
				if (moveResult)
				{
					currentNode = currentNode[0];
				}
				if (currentNode == null)
				{
					moveResult = false;
				}
				return moveResult;
			}
		}

		private const int LevelMaxValue = 16;

		private const double Probability = 0.5;

		private int currentLevel = 1;

		private Node header = new Node(null, 16);

		private Random rand = new Random();

		private int count;

		private long version;

		private IComparer comparer;

		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public virtual int Count
		{
			get
			{
				return count;
			}
		}

		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		public PriorityQueue()
		{
			comparer = new DefaultComparer();
		}

		public PriorityQueue(IComparer comparer)
		{
			if (comparer == null)
			{
				this.comparer = new DefaultComparer();
			}
			else
			{
				this.comparer = comparer;
			}
		}

		public virtual void Enqueue(object element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			Node node = header;
			Node[] array = new Node[16];
			int num = NextLevel();
			for (int num2 = currentLevel - 1; num2 >= 0; num2--)
			{
				while (node[num2] != null && comparer.Compare(node[num2].Element, element) > 0)
				{
					node = node[num2];
				}
				array[num2] = node;
			}
			if (num > currentLevel)
			{
				for (int i = currentLevel; i < num; i++)
				{
					array[i] = header;
				}
				currentLevel = num;
			}
			Node node2 = new Node(element, num);
			for (int j = 0; j < num; j++)
			{
				node2[j] = array[j][j];
				array[j][j] = node2;
			}
			count++;
			version++;
		}

		public virtual object Dequeue()
		{
			if (Count == 0)
			{
				throw new InvalidOperationException("Cannot dequeue into an empty PriorityQueue.");
			}
			object element = header[0].Element;
			Node node = header[0];
			for (int i = 0; i < currentLevel && header[i] == node; i++)
			{
				header[i] = node[i];
			}
			while (currentLevel > 1 && header[currentLevel - 1] == null)
			{
				currentLevel--;
			}
			count--;
			version++;
			return element;
		}

		public virtual void Remove(object element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			Node node = header;
			Node[] array = new Node[16];
			NextLevel();
			for (int num = currentLevel - 1; num >= 0; num--)
			{
				while (node[num] != null && comparer.Compare(node[num].Element, element) > 0)
				{
					node = node[num];
				}
				array[num] = node;
			}
			node = node[0];
			if (node != null && comparer.Compare(node.Element, element) == 0)
			{
				for (int i = 0; i < currentLevel && array[i][i] == node; i++)
				{
					array[i][i] = node[i];
				}
				while (currentLevel > 1 && header[currentLevel - 1] == null)
				{
					currentLevel--;
				}
				count--;
				version++;
			}
		}

		public virtual bool Contains(object element)
		{
			if (element == null)
			{
				return false;
			}
			Node node = header;
			for (int num = currentLevel - 1; num >= 0; num--)
			{
				while (node[num] != null && comparer.Compare(node[num].Element, element) > 0)
				{
					node = node[num];
				}
			}
			node = node[0];
			if (node != null && comparer.Compare(node.Element, element) == 0)
			{
				return true;
			}
			return false;
		}

		public virtual object Peek()
		{
			if (Count == 0)
			{
				throw new InvalidOperationException("Cannot peek into an empty PriorityQueue.");
			}
			return header[0].Element;
		}

		public virtual void Clear()
		{
			header = new Node(null, 16);
			currentLevel = 1;
			count = 0;
			version++;
		}

		public static PriorityQueue Synchronized(PriorityQueue queue)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			return new SynchronizedPriorityQueue(queue);
		}

		private int NextLevel()
		{
			int num = 1;
			while (rand.NextDouble() < 0.5 && num < 16 && num <= currentLevel)
			{
				num++;
			}
			return num;
		}

		[Conditional("DEBUG")]
		private void AssertValid()
		{
			int num = 0;
			Node node = header[0];
			while (node != null)
			{
				if (node[0] != null)
				{
				}
				node = node[0];
				num++;
			}
			for (int i = 1; i < currentLevel; i++)
			{
			}
			for (int j = currentLevel; j < 16; j++)
			{
			}
		}

		[Conditional("DEBUG")]
		public static void Test()
		{
			Random random = new Random();
			PriorityQueue priorityQueue = new PriorityQueue();
			int num = 1000;
			for (int i = 0; i < num; i++)
			{
				int num2 = random.Next();
				priorityQueue.Enqueue(num2);
			}
			int num3 = (int)priorityQueue.Peek();
			for (int j = 0; j < num; j++)
			{
				int num4 = (int)priorityQueue.Peek();
				int num2 = (int)priorityQueue.Dequeue();
				num3 = num2;
			}
		}

		public virtual void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", index, "Array index out of range.");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException("Array has more than one dimension.", "array");
			}
			if (index >= array.Length)
			{
				throw new ArgumentException("index is equal to or greater than the length of array.", "index");
			}
			if (Count > array.Length - index)
			{
				throw new ArgumentException("The number of elements in the PriorityQueue is greater than the available space from index to the end of the destination array.", "index");
			}
			int num = index;
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					array.SetValue(current, num);
					num++;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public virtual IEnumerator GetEnumerator()
		{
			return new PriorityQueueEnumerator(this);
		}
	}
}
