using System;

namespace System.Collections.Generic
{
	// Token: 0x02000297 RID: 663
	[Serializable]
	internal class ByteEqualityComparer : EqualityComparer<byte>
	{
		// Token: 0x06001A16 RID: 6678 RVA: 0x00044281 File Offset: 0x00043281
		public override bool Equals(byte x, byte y)
		{
			return x == y;
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x00044287 File Offset: 0x00043287
		public override int GetHashCode(byte b)
		{
			return b.GetHashCode();
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x00044290 File Offset: 0x00043290
		internal unsafe override int IndexOf(byte[] array, byte value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			if (count > array.Length - startIndex)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (count == 0)
			{
				return -1;
			}
			fixed (byte* ptr = array)
			{
				return Buffer.IndexOfByte(ptr, value, startIndex, count);
			}
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00044324 File Offset: 0x00043324
		internal override int LastIndexOf(byte[] array, byte value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			for (int i = startIndex; i >= num; i--)
			{
				if (array[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x00044350 File Offset: 0x00043350
		public override bool Equals(object obj)
		{
			ByteEqualityComparer byteEqualityComparer = obj as ByteEqualityComparer;
			return byteEqualityComparer != null;
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0004436B File Offset: 0x0004336B
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
