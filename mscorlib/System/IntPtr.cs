using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C8 RID: 200
	[ComVisible(true)]
	[Serializable]
	public struct IntPtr : ISerializable
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x00022CA6 File Offset: 0x00021CA6
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal bool IsNull()
		{
			return this.m_value == null;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00022CB2 File Offset: 0x00021CB2
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(int value)
		{
			this.m_value = value;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00022CBD File Offset: 0x00021CBD
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(long value)
		{
			this.m_value = value;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00022CC7 File Offset: 0x00021CC7
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe IntPtr(void* value)
		{
			this.m_value = value;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00022CD0 File Offset: 0x00021CD0
		private IntPtr(SerializationInfo info, StreamingContext context)
		{
			long @int = info.GetInt64("value");
			if (IntPtr.Size == 4 && (@int > 2147483647L || @int < -2147483648L))
			{
				throw new ArgumentException(Environment.GetResourceString("Serialization_InvalidPtrValue"));
			}
			this.m_value = @int;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00022D1B File Offset: 0x00021D1B
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("value", this.m_value);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00022D40 File Offset: 0x00021D40
		public override bool Equals(object obj)
		{
			return obj is IntPtr && this.m_value == ((IntPtr)obj).m_value;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00022D6D File Offset: 0x00021D6D
		public override int GetHashCode()
		{
			return this.m_value;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00022D78 File Offset: 0x00021D78
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int ToInt32()
		{
			long num = this.m_value;
			return checked((int)num);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00022D8F File Offset: 0x00021D8F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public long ToInt64()
		{
			return this.m_value;
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00022D98 File Offset: 0x00021D98
		public override string ToString()
		{
			return this.m_value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00022DBC File Offset: 0x00021DBC
		public string ToString(string format)
		{
			return this.m_value.ToString(format, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00022DDE File Offset: 0x00021DDE
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(int value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00022DE6 File Offset: 0x00021DE6
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(long value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00022DEE File Offset: 0x00021DEE
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		[CLSCompliant(false)]
		public unsafe static explicit operator IntPtr(void* value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00022DF6 File Offset: 0x00021DF6
		[CLSCompliant(false)]
		public unsafe static explicit operator void*(IntPtr value)
		{
			return value.ToPointer();
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00022E00 File Offset: 0x00021E00
		public static explicit operator int(IntPtr value)
		{
			long num = value.m_value;
			return checked((int)num);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00022E18 File Offset: 0x00021E18
		public static explicit operator long(IntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00022E22 File Offset: 0x00021E22
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator ==(IntPtr value1, IntPtr value2)
		{
			return value1.m_value == value2.m_value;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00022E34 File Offset: 0x00021E34
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator !=(IntPtr value1, IntPtr value2)
		{
			return value1.m_value != value2.m_value;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00022E49 File Offset: 0x00021E49
		public static int Size
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return 8;
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00022E4C File Offset: 0x00021E4C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public unsafe void* ToPointer()
		{
			return this.m_value;
		}

		// Token: 0x0400041F RID: 1055
		private unsafe void* m_value;

		// Token: 0x04000420 RID: 1056
		public static readonly IntPtr Zero;
	}
}
