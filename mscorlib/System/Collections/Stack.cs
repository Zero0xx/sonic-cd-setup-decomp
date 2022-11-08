using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000280 RID: 640
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(Stack.StackDebugView))]
	[ComVisible(true)]
	[Serializable]
	public class Stack : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x06001942 RID: 6466 RVA: 0x000419B4 File Offset: 0x000409B4
		public Stack()
		{
			this._array = new object[10];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x000419D8 File Offset: 0x000409D8
		public Stack(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (initialCapacity < 10)
			{
				initialCapacity = 10;
			}
			this._array = new object[initialCapacity];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00041A28 File Offset: 0x00040A28
		public Stack(ICollection col) : this((col == null) ? 32 : col.Count)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			foreach (object obj in col)
			{
				this.Push(obj);
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x00041A73 File Offset: 0x00040A73
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001946 RID: 6470 RVA: 0x00041A7B File Offset: 0x00040A7B
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x00041A7E File Offset: 0x00040A7E
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

		// Token: 0x06001948 RID: 6472 RVA: 0x00041AA0 File Offset: 0x00040AA0
		public virtual void Clear()
		{
			Array.Clear(this._array, 0, this._size);
			this._size = 0;
			this._version++;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00041ACC File Offset: 0x00040ACC
		public virtual object Clone()
		{
			Stack stack = new Stack(this._size);
			stack._size = this._size;
			Array.Copy(this._array, 0, stack._array, 0, this._size);
			stack._version = this._version;
			return stack;
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x00041B18 File Offset: 0x00040B18
		public virtual bool Contains(object obj)
		{
			int size = this._size;
			while (size-- > 0)
			{
				if (obj == null)
				{
					if (this._array[size] == null)
					{
						return true;
					}
				}
				else if (this._array[size] != null && this._array[size].Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00041B64 File Offset: 0x00040B64
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
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int i = 0;
			if (array is object[])
			{
				object[] array2 = (object[])array;
				while (i < this._size)
				{
					array2[i + index] = this._array[this._size - i - 1];
					i++;
				}
				return;
			}
			while (i < this._size)
			{
				array.SetValue(this._array[this._size - i - 1], i + index);
				i++;
			}
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00041C2F File Offset: 0x00040C2F
		public virtual IEnumerator GetEnumerator()
		{
			return new Stack.StackEnumerator(this);
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00041C37 File Offset: 0x00040C37
		public virtual object Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyStack"));
			}
			return this._array[this._size - 1];
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00041C60 File Offset: 0x00040C60
		public virtual object Pop()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyStack"));
			}
			this._version++;
			object result = this._array[--this._size];
			this._array[this._size] = null;
			return result;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00041CBC File Offset: 0x00040CBC
		public virtual void Push(object obj)
		{
			if (this._size == this._array.Length)
			{
				object[] array = new object[2 * this._array.Length];
				Array.Copy(this._array, 0, array, 0, this._size);
				this._array = array;
			}
			this._array[this._size++] = obj;
			this._version++;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00041D2B File Offset: 0x00040D2B
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Stack Synchronized(Stack stack)
		{
			if (stack == null)
			{
				throw new ArgumentNullException("stack");
			}
			return new Stack.SyncStack(stack);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00041D44 File Offset: 0x00040D44
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			for (int i = 0; i < this._size; i++)
			{
				array[i] = this._array[this._size - i - 1];
			}
			return array;
		}

		// Token: 0x040009F9 RID: 2553
		private const int _defaultCapacity = 10;

		// Token: 0x040009FA RID: 2554
		private object[] _array;

		// Token: 0x040009FB RID: 2555
		private int _size;

		// Token: 0x040009FC RID: 2556
		private int _version;

		// Token: 0x040009FD RID: 2557
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x02000281 RID: 641
		[Serializable]
		private class SyncStack : Stack
		{
			// Token: 0x06001952 RID: 6482 RVA: 0x00041D83 File Offset: 0x00040D83
			internal SyncStack(Stack stack)
			{
				this._s = stack;
				this._root = stack.SyncRoot;
			}

			// Token: 0x170003C7 RID: 967
			// (get) Token: 0x06001953 RID: 6483 RVA: 0x00041D9E File Offset: 0x00040D9E
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003C8 RID: 968
			// (get) Token: 0x06001954 RID: 6484 RVA: 0x00041DA1 File Offset: 0x00040DA1
			public override object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x170003C9 RID: 969
			// (get) Token: 0x06001955 RID: 6485 RVA: 0x00041DAC File Offset: 0x00040DAC
			public override int Count
			{
				get
				{
					int count;
					lock (this._root)
					{
						count = this._s.Count;
					}
					return count;
				}
			}

			// Token: 0x06001956 RID: 6486 RVA: 0x00041DEC File Offset: 0x00040DEC
			public override bool Contains(object obj)
			{
				bool result;
				lock (this._root)
				{
					result = this._s.Contains(obj);
				}
				return result;
			}

			// Token: 0x06001957 RID: 6487 RVA: 0x00041E30 File Offset: 0x00040E30
			public override object Clone()
			{
				object result;
				lock (this._root)
				{
					result = new Stack.SyncStack((Stack)this._s.Clone());
				}
				return result;
			}

			// Token: 0x06001958 RID: 6488 RVA: 0x00041E7C File Offset: 0x00040E7C
			public override void Clear()
			{
				lock (this._root)
				{
					this._s.Clear();
				}
			}

			// Token: 0x06001959 RID: 6489 RVA: 0x00041EBC File Offset: 0x00040EBC
			public override void CopyTo(Array array, int arrayIndex)
			{
				lock (this._root)
				{
					this._s.CopyTo(array, arrayIndex);
				}
			}

			// Token: 0x0600195A RID: 6490 RVA: 0x00041EFC File Offset: 0x00040EFC
			public override void Push(object value)
			{
				lock (this._root)
				{
					this._s.Push(value);
				}
			}

			// Token: 0x0600195B RID: 6491 RVA: 0x00041F3C File Offset: 0x00040F3C
			public override object Pop()
			{
				object result;
				lock (this._root)
				{
					result = this._s.Pop();
				}
				return result;
			}

			// Token: 0x0600195C RID: 6492 RVA: 0x00041F7C File Offset: 0x00040F7C
			public override IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._s.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x0600195D RID: 6493 RVA: 0x00041FBC File Offset: 0x00040FBC
			public override object Peek()
			{
				object result;
				lock (this._root)
				{
					result = this._s.Peek();
				}
				return result;
			}

			// Token: 0x0600195E RID: 6494 RVA: 0x00041FFC File Offset: 0x00040FFC
			public override object[] ToArray()
			{
				object[] result;
				lock (this._root)
				{
					result = this._s.ToArray();
				}
				return result;
			}

			// Token: 0x040009FE RID: 2558
			private Stack _s;

			// Token: 0x040009FF RID: 2559
			private object _root;
		}

		// Token: 0x02000282 RID: 642
		[Serializable]
		private class StackEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x0600195F RID: 6495 RVA: 0x0004203C File Offset: 0x0004103C
			internal StackEnumerator(Stack stack)
			{
				this._stack = stack;
				this._version = this._stack._version;
				this._index = -2;
				this.currentElement = null;
			}

			// Token: 0x06001960 RID: 6496 RVA: 0x0004206B File Offset: 0x0004106B
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x06001961 RID: 6497 RVA: 0x00042074 File Offset: 0x00041074
			public virtual bool MoveNext()
			{
				if (this._version != this._stack._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				bool flag;
				if (this._index == -2)
				{
					this._index = this._stack._size - 1;
					flag = (this._index >= 0);
					if (flag)
					{
						this.currentElement = this._stack._array[this._index];
					}
					return flag;
				}
				if (this._index == -1)
				{
					return false;
				}
				flag = (--this._index >= 0);
				if (flag)
				{
					this.currentElement = this._stack._array[this._index];
				}
				else
				{
					this.currentElement = null;
				}
				return flag;
			}

			// Token: 0x170003CA RID: 970
			// (get) Token: 0x06001962 RID: 6498 RVA: 0x00042133 File Offset: 0x00041133
			public virtual object Current
			{
				get
				{
					if (this._index == -2)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					return this.currentElement;
				}
			}

			// Token: 0x06001963 RID: 6499 RVA: 0x0004216E File Offset: 0x0004116E
			public virtual void Reset()
			{
				if (this._version != this._stack._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this._index = -2;
				this.currentElement = null;
			}

			// Token: 0x04000A00 RID: 2560
			private Stack _stack;

			// Token: 0x04000A01 RID: 2561
			private int _index;

			// Token: 0x04000A02 RID: 2562
			private int _version;

			// Token: 0x04000A03 RID: 2563
			private object currentElement;
		}

		// Token: 0x02000283 RID: 643
		internal class StackDebugView
		{
			// Token: 0x06001964 RID: 6500 RVA: 0x000421A2 File Offset: 0x000411A2
			public StackDebugView(Stack stack)
			{
				if (stack == null)
				{
					throw new ArgumentNullException("stack");
				}
				this.stack = stack;
			}

			// Token: 0x170003CB RID: 971
			// (get) Token: 0x06001965 RID: 6501 RVA: 0x000421BF File Offset: 0x000411BF
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					return this.stack.ToArray();
				}
			}

			// Token: 0x04000A04 RID: 2564
			private Stack stack;
		}
	}
}
