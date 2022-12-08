using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBush
{
	public partial class RBush<T> : ISpatialDatabase<T>, ISpatialIndex<T> where T : ISpatialData
	{
		private const int DefaultMaxEntries = 9;
		private const int MinimumMaxEntries = 4;
		private const int MinimumMinEntries = 2;
		private const double DefaultFillFactor = 0.4;

		private int maxEntries;
		private int minEntries;
		internal Node root;

		public RBush() : this(DefaultMaxEntries) { }
		public RBush(int maxEntries)
			: this(maxEntries, EqualityComparer<T>.Default) { }
		public RBush(int maxEntries, EqualityComparer<T> comparer)
		{
			this.maxEntries = Math.Max(MinimumMaxEntries, maxEntries);
			this.minEntries = Math.Max(MinimumMinEntries, (int)Math.Ceiling(this.maxEntries * DefaultFillFactor));

			this.Clear();
		}

		public int Count { get; private set; }

		public void Clear()
		{
			this.root = new Node(new List<ISpatialData>(), 1);
			this.Count = 0;
		}

		public IReadOnlyList<T> Search() => GetAllChildren(this.root).ToList();

		public IReadOnlyList<T> Search(in Envelope boundingBox)
		{
			return DoSearch(boundingBox).Select(x => (T)x.Peek()).ToList();
		}

		public List<T> Overlap(in Envelope boundingBox)
		{
			return DoOverlap(boundingBox).Select(x => (T)x.Peek()).ToList();
		}

		public List<T> Rangequery(in Envelope boundingBox)
		{
			return Search(boundingBox).ToList();
		}

		public List<T> Knn(double x, double y, int k)
		{
			var kMax = k + 20;
			bool terminate = true;
			if (k <= 0)
				return new List<T>();

			var result = new List<T>();
			var refPoint = new myPoint(x, y);


			var node = this.root;
			var queue = new PriorityQueue<WrapperNode<T>>();

			var wrapper = new WrapperNode<T>(node, refPoint);
			queue.Enqueue(wrapper);
			while (terminate)
			{
				var current = queue.Dequeue();
				queue.Enqueue(wrapper);
				foreach (var child in current.Node.Children)
				{
					if (child is T)
					{
						var temp = new WrapperNode<T>(new Node(new List<ISpatialData>() { (T)child }, 1), refPoint, true);
						temp.item = (T)child;
						queue.Enqueue(temp);
					}
					else
					{
						var temp = new WrapperNode<T>((Node)child, refPoint, false);
						queue.Enqueue(temp);
					}
				}
				while (queue.Count != 0 && queue.Peek().IsItem)
				{
					result.Add(queue.Dequeue().item);
					terminate =  kMax > result.Count;
				}


			}


			///Mientras resuelvo el problema de que no me da bien siempre los mas cercanos
			var queue2 = new PriorityQueue<WrapperNode<T>>();
			foreach (var item in result)
			{
				var temp = new WrapperNode<T>(new Node(new List<ISpatialData>() { (T)item }, 1), refPoint, true);
				temp.item = (T)item;
				queue2.Enqueue(temp);
			}
			var result2 = new List<T>();
			for (int i = 0; i < k; i++)
			{
				result2.Add(queue2.Dequeue().item);
			}

			return result2;
		}



		public void Insert(T item)
		{
			Insert(item, this.root.Height);
			this.Count++;
		}

		public void BulkLoad(IEnumerable<T> items)
		{
			var data = items.Cast<ISpatialData>().ToList();
			if (data.Count == 0) return;

			if (this.root.IsLeaf &&
				this.root.Children.Count + data.Count < maxEntries)
			{
				foreach (var i in data)
					Insert((T)i);
				return;
			}

			if (data.Count < this.minEntries)
			{
				foreach (var i in data)
					Insert((T)i);
				return;
			}

			var dataRoot = BuildTree(data);
			this.Count += data.Count;

			if (this.root.Children.Count == 0)
				this.root = dataRoot;
			else if (this.root.Height == dataRoot.Height)
			{
				if (this.root.Children.Count + dataRoot.Children.Count <= this.maxEntries)
				{
					foreach (var isd in dataRoot.Children)
						this.root.Add(dataRoot);
				}
				else
					SplitRoot(dataRoot);
			}
			else
			{
				if (this.root.Height < dataRoot.Height)
				{
					var tmp = this.root;
					this.root = dataRoot;
					dataRoot = tmp;
				}

				this.Insert(dataRoot, this.root.Height - dataRoot.Height);
			}
		}

		public void Delete(T item)
		{
			var candidates = DoSearch(item.Envelope);

			foreach (var c in candidates
				.Where(c => object.Equals(item, c.Peek())))
			{
				var path = c.Pop();
				(path.Peek() as Node).Children.Remove(item);
				while (!path.IsEmpty)
				{
					(path.Peek() as Node).ResetEnvelope();
					path = path.Pop();
				}
			}
		}
	}



	///<summary>
	/// Priority Queue data structure
	/// </summary>
	public class PriorityQueue<T>
		where T : IComparable
	{
		protected List<T> storedValues;

		public PriorityQueue()
		{
			//Initialize the array that will hold the values
			storedValues = new List<T>();

			//Fill the first cell in the array with an empty value
			storedValues.Add(default(T));
		}

		/// <summary>
		/// Gets the number of values stored within the Priority Queue
		/// </summary>
		public virtual int Count
		{
			get { return storedValues.Count - 1; }
		}

		/// <summary>
		/// Returns the value at the head of the Priority Queue without removing it.
		/// </summary>
		public virtual T Peek()
		{
			if (this.Count == 0)
				return default(T); //Priority Queue empty
			else
				return storedValues[1]; //head of the queue
		}

		/// <summary>
		/// Adds a value to the Priority Queue
		/// </summary>
		public virtual void Enqueue(T value)
		{
			//Add the value to the internal array
			storedValues.Add(value);

			//Bubble up to preserve the heap property,
			//starting at the inserted value
			this.BubbleUp(storedValues.Count - 1);
		}

		/// <summary>
		/// Returns the minimum value inside the Priority Queue
		/// </summary>
		public virtual T Dequeue()
		{
			if (this.Count == 0)
				return default(T); //queue is empty
			else
			{
				//The smallest value in the Priority Queue is the first item in the array
				T minValue = this.storedValues[1];

				//If there's more than one item, replace the first item in the array with the last one
				if (this.storedValues.Count > 2)
				{
					T lastValue = this.storedValues[storedValues.Count - 1];

					//Move last node to the head
					this.storedValues.RemoveAt(storedValues.Count - 1);
					this.storedValues[1] = lastValue;

					//Bubble down
					this.BubbleDown(1);
				}
				else
				{
					//Remove the only value stored in the queue
					storedValues.RemoveAt(1);
				}

				return minValue;
			}
		}

		/// <summary>
		/// Restores the heap-order property between child and parent values going up towards the head
		/// </summary>
		protected virtual void BubbleUp(int startCell)
		{
			int cell = startCell;

			//Bubble up as long as the parent is greater
			while (this.IsParentBigger(cell))
			{
				//Get values of parent and child
				T parentValue = this.storedValues[cell / 2];
				T childValue = this.storedValues[cell];

				//Swap the values
				this.storedValues[cell / 2] = childValue;
				this.storedValues[cell] = parentValue;

				cell /= 2; //go up parents
			}
		}

		/// <summary>
		/// Restores the heap-order property between child and parent values going down towards the bottom
		/// </summary>
		protected virtual void BubbleDown(int startCell)
		{
			int cell = startCell;

			//Bubble down as long as either child is smaller
			while (this.IsLeftChildSmaller(cell) || this.IsRightChildSmaller(cell))
			{
				int child = this.CompareChild(cell);

				if (child == -1) //Left Child
				{
					//Swap values
					T parentValue = storedValues[cell];
					T leftChildValue = storedValues[2 * cell];

					storedValues[cell] = leftChildValue;
					storedValues[2 * cell] = parentValue;

					cell = 2 * cell; //move down to left child
				}
				else if (child == 1) //Right Child
				{
					//Swap values
					T parentValue = storedValues[cell];
					T rightChildValue = storedValues[2 * cell + 1];

					storedValues[cell] = rightChildValue;
					storedValues[2 * cell + 1] = parentValue;

					cell = 2 * cell + 1; //move down to right child
				}
			}
		}

		/// <summary>
		/// Returns if the value of a parent is greater than its child
		/// </summary>
		protected virtual bool IsParentBigger(int childCell)
		{
			if (childCell == 1)
				return false; //top of heap, no parent
			else
				return storedValues[childCell / 2].CompareTo(storedValues[childCell]) > 0;
			//return storedNodes[childCell / 2].Key > storedNodes[childCell].Key;
		}

		/// <summary>
		/// Returns whether the left child cell is smaller than the parent cell.
		/// Returns false if a left child does not exist.
		/// </summary>
		protected virtual bool IsLeftChildSmaller(int parentCell)
		{
			if (2 * parentCell >= storedValues.Count)
				return false; //out of bounds
			else
				return storedValues[2 * parentCell].CompareTo(storedValues[parentCell]) < 0;
			//return storedNodes[2 * parentCell].Key < storedNodes[parentCell].Key;
		}

		/// <summary>
		/// Returns whether the right child cell is smaller than the parent cell.
		/// Returns false if a right child does not exist.
		/// </summary>
		protected virtual bool IsRightChildSmaller(int parentCell)
		{
			if (2 * parentCell + 1 >= storedValues.Count)
				return false; //out of bounds
			else
				return storedValues[2 * parentCell + 1].CompareTo(storedValues[parentCell]) < 0;
			//return storedNodes[2 * parentCell + 1].Key < storedNodes[parentCell].Key;
		}

		/// <summary>
		/// Compares the children cells of a parent cell. -1 indicates the left child is the smaller of the two,
		/// 1 indicates the right child is the smaller of the two, 0 inidicates that neither child is smaller than the parent.
		/// </summary>
		protected virtual int CompareChild(int parentCell)
		{
			bool leftChildSmaller = this.IsLeftChildSmaller(parentCell);
			bool rightChildSmaller = this.IsRightChildSmaller(parentCell);

			if (leftChildSmaller || rightChildSmaller)
			{
				if (leftChildSmaller && rightChildSmaller)
				{
					//Figure out which of the two is smaller
					int leftChild = 2 * parentCell;
					int rightChild = 2 * parentCell + 1;

					T leftValue = this.storedValues[leftChild];
					T rightValue = this.storedValues[rightChild];

					//Compare the values of the children
					if (leftValue.CompareTo(rightValue) <= 0)
						return -1; //left child is smaller
					else
						return 1; //right child is smaller
				}
				else if (leftChildSmaller)
					return -1; //left child is smaller
				else
					return 1; //right child smaller
			}
			else
				return 0; //both children are bigger or don't exist
		}

	}

	public struct myPoint
	{
		public double X { get; set; }
		public double Y { get; set; }

		public myPoint(double x, double y)
		{
			X = x;
			Y = y;
		}
	}

	public class WrapperNode<T> : IComparable where T : ISpatialData
	{
		public RBush<T>.Node Node { get; set; }

		public T item { get; set; }

		public bool IsItem { get; set; }

		public myPoint RefPoint { get; set; }

		public WrapperNode(RBush<T>.Node node, myPoint point, bool isItem = false)
		{
			Node = node;
			RefPoint = point;
			IsItem = isItem;
		}

		public int CompareTo(object obj)
		{
			return CompareEnvelope(this.Node.Envelope, (obj as WrapperNode<T>).Node.Envelope);
		}

		private List<myPoint> EnvelopeToPoints(Envelope boundingbox)
		{
			var result = new List<myPoint>();
			result.Add(new myPoint(boundingbox.MinX, boundingbox.MinY));
			result.Add(new myPoint(boundingbox.MinX, boundingbox.MaxY));
			result.Add(new myPoint(boundingbox.MaxX, boundingbox.MinY));
			result.Add(new myPoint(boundingbox.MaxX, boundingbox.MaxY));
			return result;
		}

		private int CompareEnvelope(Envelope rectangle1, Envelope rectangle2)
		{
			var d1 = MinDistanceToPoinst(EnvelopeToPoints(rectangle1));
			var d2 = MinDistanceToPoinst(EnvelopeToPoints(rectangle2));

			if (d1 < d2)
				return -1;
			if (d1 > d2)
				return 1;
			return 0;

		}

		private double MinDistanceToPoinst(List<myPoint> listPoint)
		{
			double min = double.MaxValue;
			var p1 = new myPoint();
			var p2 = new myPoint();
			var p3 = new myPoint();
			var p4 = new myPoint();
			FindDistanceToSegment(RefPoint, listPoint[0], listPoint[1], out p1);
			FindDistanceToSegment(RefPoint, listPoint[1], listPoint[2], out p2);
			FindDistanceToSegment(RefPoint, listPoint[2], listPoint[3], out p3);
			FindDistanceToSegment(RefPoint, listPoint[3], listPoint[0], out p4);

			var listtemp = new List<myPoint> {p1,p2,p3,p4 };

			foreach (var point in listtemp)
			{
				var tempdistance = Distance(RefPoint, point);
				if (min > tempdistance)
					min = tempdistance;
			}
			return min;
		}



		private double FindDistanceToSegment(
			myPoint pt, myPoint p1, myPoint p2, out myPoint closest)
		{
			double dx = p2.X - p1.X;
			double dy = p2.Y - p1.Y;
			if ((dx == 0) && (dy == 0))
			{
				// It's a point not a line segment.
				closest = p1;
				dx = pt.X - p1.X;
				dy = pt.Y - p1.Y;
				return Math.Sqrt(dx * dx + dy * dy);
			}

			// Calculate the t that minimizes the distance.
			double t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
					  (dx * dx + dy * dy);

			// See if this represents one of the segment's
			// end points or a point in the middle.
			if (t < 0)
			{
				closest = new myPoint(p1.X, p1.Y);
				dx = pt.X - p1.X;
				dy = pt.Y - p1.Y;
			}
			else if (t > 1)
			{
				closest = new myPoint(p2.X, p2.Y);
				dx = pt.X - p2.X;
				dy = pt.Y - p2.Y;
			}
			else
			{
				closest = new myPoint(p1.X + t * dx, p1.Y + t * dy);
				dx = pt.X - closest.X;
				dy = pt.Y - closest.Y;
			}

			return (dx * dx + dy * dy);
		}
		private double Distance(myPoint point1, myPoint point2)
		{
			return Math.Pow((point2.X - point1.X), 2) + Math.Pow((point2.Y - point1.Y), 2);
		}
	}

}
