using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000275 RID: 629
	[DebuggerTypeProxy(typeof(Queue.QueueDebugView))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(true)]
	[Serializable]
	public class Queue : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x060018A1 RID: 6305 RVA: 0x0003FAC0 File Offset: 0x0003EAC0
		public Queue() : this(32, 2f)
		{
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0003FACF File Offset: 0x0003EACF
		public Queue(int capacity) : this(capacity, 2f)
		{
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0003FAE0 File Offset: 0x0003EAE0
		public Queue(int capacity, float growFactor)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if ((double)growFactor < 1.0 || (double)growFactor > 10.0)
			{
				throw new ArgumentOutOfRangeException("growFactor", Environment.GetResourceString("ArgumentOutOfRange_QueueGrowFactor", new object[]
				{
					1,
					10
				}));
			}
			this._array = new object[capacity];
			this._head = 0;
			this._tail = 0;
			this._size = 0;
			this._growFactor = (int)(growFactor * 100f);
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0003FB88 File Offset: 0x0003EB88
		public Queue(ICollection col) : this((col == null) ? 32 : col.Count)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			foreach (object obj in col)
			{
				this.Enqueue(obj);
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0003FBD3 File Offset: 0x0003EBD3
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0003FBDC File Offset: 0x0003EBDC
		public virtual object Clone()
		{
			Queue queue = new Queue(this._size);
			queue._size = this._size;
			int num = this._size;
			int num2 = (this._array.Length - this._head < num) ? (this._array.Length - this._head) : num;
			Array.Copy(this._array, this._head, queue._array, 0, num2);
			num -= num2;
			if (num > 0)
			{
				Array.Copy(this._array, 0, queue._array, this._array.Length - this._head, num);
			}
			queue._version = this._version;
			return queue;
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0003FC7D File Offset: 0x0003EC7D
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x0003FC80 File Offset: 0x0003EC80
		public virtual object SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0003FCA4 File Offset: 0x0003ECA4
		public virtual void Clear()
		{
			if (this._head < this._tail)
			{
				Array.Clear(this._array, this._head, this._size);
			}
			else
			{
				Array.Clear(this._array, this._head, this._array.Length - this._head);
				Array.Clear(this._array, 0, this._tail);
			}
			this._head = 0;
			this._tail = 0;
			this._size = 0;
			this._version++;
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0003FD30 File Offset: 0x0003ED30
		public virtual void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			int length = array.Length;
			if (length - index < this._size)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int num = this._size;
			if (num == 0)
			{
				return;
			}
			int num2 = (this._array.Length - this._head < num) ? (this._array.Length - this._head) : num;
			Array.Copy(this._array, this._head, array, index, num2);
			num -= num2;
			if (num > 0)
			{
				Array.Copy(this._array, 0, array, index + this._array.Length - this._head, num);
			}
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0003FE0C File Offset: 0x0003EE0C
		public virtual void Enqueue(object obj)
		{
			if (this._size == this._array.Length)
			{
				int num = (int)((long)this._array.Length * (long)this._growFactor / 100L);
				if (num < this._array.Length + 4)
				{
					num = this._array.Length + 4;
				}
				this.SetCapacity(num);
			}
			this._array[this._tail] = obj;
			this._tail = (this._tail + 1) % this._array.Length;
			this._size++;
			this._version++;
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0003FEA0 File Offset: 0x0003EEA0
		public virtual IEnumerator GetEnumerator()
		{
			return new Queue.QueueEnumerator(this);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0003FEA8 File Offset: 0x0003EEA8
		public virtual object Dequeue()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyQueue"));
			}
			object result = this._array[this._head];
			this._array[this._head] = null;
			this._head = (this._head + 1) % this._array.Length;
			this._size--;
			this._version++;
			return result;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0003FF1D File Offset: 0x0003EF1D
		public virtual object Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyQueue"));
			}
			return this._array[this._head];
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0003FF44 File Offset: 0x0003EF44
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Queue Synchronized(Queue queue)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			return new Queue.SynchronizedQueue(queue);
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0003FF5C File Offset: 0x0003EF5C
		public virtual bool Contains(object obj)
		{
			int num = this._head;
			int size = this._size;
			while (size-- > 0)
			{
				if (obj == null)
				{
					if (this._array[num] == null)
					{
						return true;
					}
				}
				else if (this._array[num] != null && this._array[num].Equals(obj))
				{
					return true;
				}
				num = (num + 1) % this._array.Length;
			}
			return false;
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0003FFBA File Offset: 0x0003EFBA
		internal object GetElement(int i)
		{
			return this._array[(this._head + i) % this._array.Length];
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0003FFD4 File Offset: 0x0003EFD4
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			if (this._size == 0)
			{
				return array;
			}
			if (this._head < this._tail)
			{
				Array.Copy(this._array, this._head, array, 0, this._size);
			}
			else
			{
				Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
				Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
			}
			return array;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00040068 File Offset: 0x0003F068
		private void SetCapacity(int capacity)
		{
			object[] array = new object[capacity];
			if (this._size > 0)
			{
				if (this._head < this._tail)
				{
					Array.Copy(this._array, this._head, array, 0, this._size);
				}
				else
				{
					Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
					Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
				}
			}
			this._array = array;
			this._head = 0;
			this._tail = ((this._size == capacity) ? 0 : this._size);
			this._version++;
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00040126 File Offset: 0x0003F126
		public virtual void TrimToSize()
		{
			this.SetCapacity(this._size);
		}

		// Token: 0x040009CD RID: 2509
		private const int _MinimumGrow = 4;

		// Token: 0x040009CE RID: 2510
		private const int _ShrinkThreshold = 32;

		// Token: 0x040009CF RID: 2511
		private object[] _array;

		// Token: 0x040009D0 RID: 2512
		private int _head;

		// Token: 0x040009D1 RID: 2513
		private int _tail;

		// Token: 0x040009D2 RID: 2514
		private int _size;

		// Token: 0x040009D3 RID: 2515
		private int _growFactor;

		// Token: 0x040009D4 RID: 2516
		private int _version;

		// Token: 0x040009D5 RID: 2517
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x02000276 RID: 630
		[Serializable]
		private class SynchronizedQueue : Queue
		{
			// Token: 0x060018B5 RID: 6325 RVA: 0x00040134 File Offset: 0x0003F134
			internal SynchronizedQueue(Queue q)
			{
				this._q = q;
				this.root = this._q.SyncRoot;
			}

			// Token: 0x1700039A RID: 922
			// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00040154 File Offset: 0x0003F154
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700039B RID: 923
			// (get) Token: 0x060018B7 RID: 6327 RVA: 0x00040157 File Offset: 0x0003F157
			public override object SyncRoot
			{
				get
				{
					return this.root;
				}
			}

			// Token: 0x1700039C RID: 924
			// (get) Token: 0x060018B8 RID: 6328 RVA: 0x00040160 File Offset: 0x0003F160
			public override int Count
			{
				get
				{
					int count;
					lock (this.root)
					{
						count = this._q.Count;
					}
					return count;
				}
			}

			// Token: 0x060018B9 RID: 6329 RVA: 0x000401A0 File Offset: 0x0003F1A0
			public override void Clear()
			{
				lock (this.root)
				{
					this._q.Clear();
				}
			}

			// Token: 0x060018BA RID: 6330 RVA: 0x000401E0 File Offset: 0x0003F1E0
			public override object Clone()
			{
				object result;
				lock (this.root)
				{
					result = new Queue.SynchronizedQueue((Queue)this._q.Clone());
				}
				return result;
			}

			// Token: 0x060018BB RID: 6331 RVA: 0x0004022C File Offset: 0x0003F22C
			public override bool Contains(object obj)
			{
				bool result;
				lock (this.root)
				{
					result = this._q.Contains(obj);
				}
				return result;
			}

			// Token: 0x060018BC RID: 6332 RVA: 0x00040270 File Offset: 0x0003F270
			public override void CopyTo(Array array, int arrayIndex)
			{
				lock (this.root)
				{
					this._q.CopyTo(array, arrayIndex);
				}
			}

			// Token: 0x060018BD RID: 6333 RVA: 0x000402B0 File Offset: 0x0003F2B0
			public override void Enqueue(object value)
			{
				lock (this.root)
				{
					this._q.Enqueue(value);
				}
			}

			// Token: 0x060018BE RID: 6334 RVA: 0x000402F0 File Offset: 0x0003F2F0
			public override object Dequeue()
			{
				object result;
				lock (this.root)
				{
					result = this._q.Dequeue();
				}
				return result;
			}

			// Token: 0x060018BF RID: 6335 RVA: 0x00040330 File Offset: 0x0003F330
			public override IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this.root)
				{
					enumerator = this._q.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x060018C0 RID: 6336 RVA: 0x00040370 File Offset: 0x0003F370
			public override object Peek()
			{
				object result;
				lock (this.root)
				{
					result = this._q.Peek();
				}
				return result;
			}

			// Token: 0x060018C1 RID: 6337 RVA: 0x000403B0 File Offset: 0x0003F3B0
			public override object[] ToArray()
			{
				object[] result;
				lock (this.root)
				{
					result = this._q.ToArray();
				}
				return result;
			}

			// Token: 0x060018C2 RID: 6338 RVA: 0x000403F0 File Offset: 0x0003F3F0
			public override void TrimToSize()
			{
				lock (this.root)
				{
					this._q.TrimToSize();
				}
			}

			// Token: 0x040009D6 RID: 2518
			private Queue _q;

			// Token: 0x040009D7 RID: 2519
			private object root;
		}

		// Token: 0x02000277 RID: 631
		[Serializable]
		private class QueueEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x060018C3 RID: 6339 RVA: 0x00040430 File Offset: 0x0003F430
			internal QueueEnumerator(Queue q)
			{
				this._q = q;
				this._version = this._q._version;
				this._index = 0;
				this.currentElement = this._q._array;
				if (this._q._size == 0)
				{
					this._index = -1;
				}
			}

			// Token: 0x060018C4 RID: 6340 RVA: 0x00040487 File Offset: 0x0003F487
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x060018C5 RID: 6341 RVA: 0x00040490 File Offset: 0x0003F490
			public virtual bool MoveNext()
			{
				if (this._version != this._q._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this._index < 0)
				{
					this.currentElement = this._q._array;
					return false;
				}
				this.currentElement = this._q.GetElement(this._index);
				this._index++;
				if (this._index == this._q._size)
				{
					this._index = -1;
				}
				return true;
			}

			// Token: 0x1700039D RID: 925
			// (get) Token: 0x060018C6 RID: 6342 RVA: 0x0004051C File Offset: 0x0003F51C
			public virtual object Current
			{
				get
				{
					if (this.currentElement != this._q._array)
					{
						return this.currentElement;
					}
					if (this._index == 0)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
			}

			// Token: 0x060018C7 RID: 6343 RVA: 0x0004056C File Offset: 0x0003F56C
			public virtual void Reset()
			{
				if (this._version != this._q._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this._q._size == 0)
				{
					this._index = -1;
				}
				else
				{
					this._index = 0;
				}
				this.currentElement = this._q._array;
			}

			// Token: 0x040009D8 RID: 2520
			private Queue _q;

			// Token: 0x040009D9 RID: 2521
			private int _index;

			// Token: 0x040009DA RID: 2522
			private int _version;

			// Token: 0x040009DB RID: 2523
			private object currentElement;
		}

		// Token: 0x02000278 RID: 632
		internal class QueueDebugView
		{
			// Token: 0x060018C8 RID: 6344 RVA: 0x000405CA File Offset: 0x0003F5CA
			public QueueDebugView(Queue queue)
			{
				if (queue == null)
				{
					throw new ArgumentNullException("queue");
				}
				this.queue = queue;
			}

			// Token: 0x1700039E RID: 926
			// (get) Token: 0x060018C9 RID: 6345 RVA: 0x000405E7 File Offset: 0x0003F5E7
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					return this.queue.ToArray();
				}
			}

			// Token: 0x040009DC RID: 2524
			private Queue queue;
		}
	}
}
