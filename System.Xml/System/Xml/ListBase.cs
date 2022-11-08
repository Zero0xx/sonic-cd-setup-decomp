using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Xml
{
	// Token: 0x02000020 RID: 32
	internal abstract class ListBase<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000084 RID: 132
		public abstract int Count { get; }

		// Token: 0x17000020 RID: 32
		public abstract T this[int index]
		{
			get;
			set;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003C59 File Offset: 0x00002C59
		public virtual bool Contains(T value)
		{
			return this.IndexOf(value) != -1;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003C68 File Offset: 0x00002C68
		public virtual int IndexOf(T value)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (value.Equals(this[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003CA4 File Offset: 0x00002CA4
		public virtual void CopyTo(T[] array, int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[index + i] = this[i];
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003CD2 File Offset: 0x00002CD2
		public virtual IListEnumerator<T> GetEnumerator()
		{
			return new IListEnumerator<T>(this);
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003CDA File Offset: 0x00002CDA
		public virtual bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003CDD File Offset: 0x00002CDD
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003CE0 File Offset: 0x00002CE0
		public virtual void Add(T value)
		{
			this.Insert(this.Count, value);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003CEF File Offset: 0x00002CEF
		public virtual void Insert(int index, T value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003CF8 File Offset: 0x00002CF8
		public virtual bool Remove(T value)
		{
			int num = this.IndexOf(value);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003D1B File Offset: 0x00002D1B
		public virtual void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003D24 File Offset: 0x00002D24
		public virtual void Clear()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.RemoveAt(i);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003D4B File Offset: 0x00002D4B
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return new IListEnumerator<T>(this);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003D58 File Offset: 0x00002D58
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new IListEnumerator<T>(this);
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003D65 File Offset: 0x00002D65
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003D6D File Offset: 0x00002D6D
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003D70 File Offset: 0x00002D70
		void ICollection.CopyTo(Array array, int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
			}
		}

		// Token: 0x17000025 RID: 37
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (!ListBase<T>.IsCompatibleType(value.GetType()))
				{
					throw new ArgumentException();
				}
				this[index] = (T)((object)value);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003DD1 File Offset: 0x00002DD1
		int IList.Add(object value)
		{
			if (!ListBase<T>.IsCompatibleType(value.GetType()))
			{
				throw new ArgumentException();
			}
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003DFA File Offset: 0x00002DFA
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003E02 File Offset: 0x00002E02
		bool IList.Contains(object value)
		{
			return ListBase<T>.IsCompatibleType(value.GetType()) && this.Contains((T)((object)value));
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003E1F File Offset: 0x00002E1F
		int IList.IndexOf(object value)
		{
			if (!ListBase<T>.IsCompatibleType(value.GetType()))
			{
				return -1;
			}
			return this.IndexOf((T)((object)value));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003E3C File Offset: 0x00002E3C
		void IList.Insert(int index, object value)
		{
			if (!ListBase<T>.IsCompatibleType(value.GetType()))
			{
				throw new ArgumentException();
			}
			this.Insert(index, (T)((object)value));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003E5E File Offset: 0x00002E5E
		void IList.Remove(object value)
		{
			if (!ListBase<T>.IsCompatibleType(value.GetType()))
			{
				throw new ArgumentException();
			}
			this.Remove((T)((object)value));
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003E80 File Offset: 0x00002E80
		private static bool IsCompatibleType(object value)
		{
			return (value == null && !typeof(T).IsValueType) || value is T;
		}
	}
}
