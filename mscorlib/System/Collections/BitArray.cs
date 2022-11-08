using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000259 RID: 601
	[ComVisible(true)]
	[Serializable]
	public sealed class BitArray : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x06001776 RID: 6006 RVA: 0x0003C017 File Offset: 0x0003B017
		private BitArray()
		{
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0003C01F File Offset: 0x0003B01F
		public BitArray(int length) : this(length, false)
		{
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0003C02C File Offset: 0x0003B02C
		public BitArray(int length, bool defaultValue)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_array = new int[(length + 31) / 32];
			this.m_length = length;
			int num = defaultValue ? -1 : 0;
			for (int i = 0; i < this.m_array.Length; i++)
			{
				this.m_array[i] = num;
			}
			this._version = 0;
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x0003C09C File Offset: 0x0003B09C
		public BitArray(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.m_array = new int[(bytes.Length + 3) / 4];
			this.m_length = bytes.Length * 8;
			int num = 0;
			int num2 = 0;
			while (bytes.Length - num2 >= 4)
			{
				this.m_array[num++] = ((int)(bytes[num2] & byte.MaxValue) | (int)(bytes[num2 + 1] & byte.MaxValue) << 8 | (int)(bytes[num2 + 2] & byte.MaxValue) << 16 | (int)(bytes[num2 + 3] & byte.MaxValue) << 24);
				num2 += 4;
			}
			switch (bytes.Length - num2)
			{
			case 1:
				goto IL_DB;
			case 2:
				break;
			case 3:
				this.m_array[num] = (int)(bytes[num2 + 2] & byte.MaxValue) << 16;
				break;
			default:
				goto IL_FC;
			}
			this.m_array[num] |= (int)(bytes[num2 + 1] & byte.MaxValue) << 8;
			IL_DB:
			this.m_array[num] |= (int)(bytes[num2] & byte.MaxValue);
			IL_FC:
			this._version = 0;
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x0003C1AC File Offset: 0x0003B1AC
		public BitArray(bool[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.m_array = new int[(values.Length + 31) / 32];
			this.m_length = values.Length;
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i])
				{
					this.m_array[i / 32] |= 1 << i % 32;
				}
			}
			this._version = 0;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x0003C228 File Offset: 0x0003B228
		public BitArray(int[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.m_array = new int[values.Length];
			this.m_length = values.Length * 32;
			Array.Copy(values, this.m_array, values.Length);
			this._version = 0;
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x0003C27C File Offset: 0x0003B27C
		public BitArray(BitArray bits)
		{
			if (bits == null)
			{
				throw new ArgumentNullException("bits");
			}
			this.m_array = new int[(bits.m_length + 31) / 32];
			this.m_length = bits.m_length;
			Array.Copy(bits.m_array, this.m_array, (bits.m_length + 31) / 32);
			this._version = bits._version;
		}

		// Token: 0x17000331 RID: 817
		public bool this[int index]
		{
			get
			{
				return this.Get(index);
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0003C2FC File Offset: 0x0003B2FC
		public bool Get(int index)
		{
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return (this.m_array[index / 32] & 1 << index % 32) != 0;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x0003C33C File Offset: 0x0003B33C
		public void Set(int index, bool value)
		{
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (value)
			{
				this.m_array[index / 32] |= 1 << index % 32;
			}
			else
			{
				this.m_array[index / 32] &= ~(1 << index % 32);
			}
			this._version++;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0003C3C8 File Offset: 0x0003B3C8
		public void SetAll(bool value)
		{
			int num = value ? -1 : 0;
			int num2 = (this.m_length + 31) / 32;
			for (int i = 0; i < num2; i++)
			{
				this.m_array[i] = num;
			}
			this._version++;
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0003C410 File Offset: 0x0003B410
		public BitArray And(BitArray value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.m_length != value.m_length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"));
			}
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] &= value.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x0003C494 File Offset: 0x0003B494
		public BitArray Or(BitArray value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.m_length != value.m_length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"));
			}
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] |= value.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0003C518 File Offset: 0x0003B518
		public BitArray Xor(BitArray value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.m_length != value.m_length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayLengthsDiffer"));
			}
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] ^= value.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0003C59C File Offset: 0x0003B59C
		public BitArray Not()
		{
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] = ~this.m_array[i];
			}
			this._version++;
			return this;
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x0003C5E2 File Offset: 0x0003B5E2
		// (set) Token: 0x06001787 RID: 6023 RVA: 0x0003C5EC File Offset: 0x0003B5EC
		public int Length
		{
			get
			{
				return this.m_length;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				int num = (value + 31) / 32;
				if (num > this.m_array.Length || num + 256 < this.m_array.Length)
				{
					int[] array = new int[num];
					Array.Copy(this.m_array, array, (num > this.m_array.Length) ? this.m_array.Length : num);
					this.m_array = array;
				}
				if (value > this.m_length)
				{
					int num2 = (this.m_length + 31) / 32 - 1;
					int num3 = this.m_length % 32;
					if (num3 > 0)
					{
						this.m_array[num2] &= (1 << num3) - 1;
					}
					Array.Clear(this.m_array, num2 + 1, num - num2 - 1);
				}
				this.m_length = value;
				this._version++;
			}
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0003C6D4 File Offset: 0x0003B6D4
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (array is int[])
			{
				Array.Copy(this.m_array, 0, array, index, (this.m_length + 31) / 32);
				return;
			}
			if (array is byte[])
			{
				if (array.Length - index < (this.m_length + 7) / 8)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				byte[] array2 = (byte[])array;
				for (int i = 0; i < (this.m_length + 7) / 8; i++)
				{
					array2[index + i] = (byte)(this.m_array[i / 4] >> i % 4 * 8 & 255);
				}
				return;
			}
			else
			{
				if (!(array is bool[]))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_BitArrayTypeUnsupported"));
				}
				if (array.Length - index < this.m_length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				bool[] array3 = (bool[])array;
				for (int j = 0; j < this.m_length; j++)
				{
					array3[index + j] = ((this.m_array[j / 32] >> j % 32 & 1) != 0);
				}
				return;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0003C81C File Offset: 0x0003B81C
		public int Count
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0003C824 File Offset: 0x0003B824
		public object Clone()
		{
			return new BitArray(this.m_array)
			{
				_version = this._version,
				m_length = this.m_length
			};
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x0003C856 File Offset: 0x0003B856
		public object SyncRoot
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

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x0003C878 File Offset: 0x0003B878
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x0003C87B File Offset: 0x0003B87B
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0003C87E File Offset: 0x0003B87E
		public IEnumerator GetEnumerator()
		{
			return new BitArray.BitArrayEnumeratorSimple(this);
		}

		// Token: 0x0400097C RID: 2428
		private const int _ShrinkThreshold = 256;

		// Token: 0x0400097D RID: 2429
		private int[] m_array;

		// Token: 0x0400097E RID: 2430
		private int m_length;

		// Token: 0x0400097F RID: 2431
		private int _version;

		// Token: 0x04000980 RID: 2432
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x0200025A RID: 602
		[Serializable]
		private class BitArrayEnumeratorSimple : IEnumerator, ICloneable
		{
			// Token: 0x0600178F RID: 6031 RVA: 0x0003C886 File Offset: 0x0003B886
			internal BitArrayEnumeratorSimple(BitArray bitarray)
			{
				this.bitarray = bitarray;
				this.index = -1;
				this.version = bitarray._version;
			}

			// Token: 0x06001790 RID: 6032 RVA: 0x0003C8A8 File Offset: 0x0003B8A8
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x06001791 RID: 6033 RVA: 0x0003C8B0 File Offset: 0x0003B8B0
			public virtual bool MoveNext()
			{
				if (this.version != this.bitarray._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.index < this.bitarray.Count - 1)
				{
					this.index++;
					this.currentElement = this.bitarray.Get(this.index);
					return true;
				}
				this.index = this.bitarray.Count;
				return false;
			}

			// Token: 0x17000337 RID: 823
			// (get) Token: 0x06001792 RID: 6034 RVA: 0x0003C930 File Offset: 0x0003B930
			public virtual object Current
			{
				get
				{
					if (this.index == -1)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this.index >= this.bitarray.Count)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					return this.currentElement;
				}
			}

			// Token: 0x06001793 RID: 6035 RVA: 0x0003C984 File Offset: 0x0003B984
			public void Reset()
			{
				if (this.version != this.bitarray._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.index = -1;
			}

			// Token: 0x04000981 RID: 2433
			private BitArray bitarray;

			// Token: 0x04000982 RID: 2434
			private int index;

			// Token: 0x04000983 RID: 2435
			private int version;

			// Token: 0x04000984 RID: 2436
			private bool currentElement;
		}
	}
}
