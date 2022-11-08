using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000134 RID: 308
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public struct UIntPtr : ISerializable
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x0002F6BA File Offset: 0x0002E6BA
		public UIntPtr(uint value)
		{
			this.m_value = value;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0002F6C4 File Offset: 0x0002E6C4
		public UIntPtr(ulong value)
		{
			this.m_value = value;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0002F6CE File Offset: 0x0002E6CE
		[CLSCompliant(false)]
		public unsafe UIntPtr(void* value)
		{
			this.m_value = value;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0002F6D8 File Offset: 0x0002E6D8
		private UIntPtr(SerializationInfo info, StreamingContext context)
		{
			ulong @uint = info.GetUInt64("value");
			if (UIntPtr.Size == 4 && @uint > (ulong)-1)
			{
				throw new ArgumentException(Environment.GetResourceString("Serialization_InvalidPtrValue"));
			}
			this.m_value = @uint;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0002F716 File Offset: 0x0002E716
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("value", this.m_value);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0002F738 File Offset: 0x0002E738
		public override bool Equals(object obj)
		{
			return obj is UIntPtr && this.m_value == ((UIntPtr)obj).m_value;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0002F765 File Offset: 0x0002E765
		public override int GetHashCode()
		{
			return this.m_value & int.MaxValue;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0002F775 File Offset: 0x0002E775
		public uint ToUInt32()
		{
			return this.m_value;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0002F77E File Offset: 0x0002E77E
		public ulong ToUInt64()
		{
			return this.m_value;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0002F788 File Offset: 0x0002E788
		public override string ToString()
		{
			return this.m_value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0002F7A9 File Offset: 0x0002E7A9
		public static explicit operator UIntPtr(uint value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x0002F7B1 File Offset: 0x0002E7B1
		public static explicit operator UIntPtr(ulong value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0002F7B9 File Offset: 0x0002E7B9
		public static explicit operator uint(UIntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0002F7C3 File Offset: 0x0002E7C3
		public static explicit operator ulong(UIntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0002F7CD File Offset: 0x0002E7CD
		[CLSCompliant(false)]
		public unsafe static explicit operator UIntPtr(void* value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0002F7D5 File Offset: 0x0002E7D5
		[CLSCompliant(false)]
		public unsafe static explicit operator void*(UIntPtr value)
		{
			return value.ToPointer();
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0002F7DE File Offset: 0x0002E7DE
		public static bool operator ==(UIntPtr value1, UIntPtr value2)
		{
			return value1.m_value == value2.m_value;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0002F7F0 File Offset: 0x0002E7F0
		public static bool operator !=(UIntPtr value1, UIntPtr value2)
		{
			return value1.m_value != value2.m_value;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x0002F805 File Offset: 0x0002E805
		public static int Size
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0002F808 File Offset: 0x0002E808
		[CLSCompliant(false)]
		public unsafe void* ToPointer()
		{
			return this.m_value;
		}

		// Token: 0x040005C3 RID: 1475
		private unsafe void* m_value;

		// Token: 0x040005C4 RID: 1476
		public static readonly UIntPtr Zero;
	}
}
