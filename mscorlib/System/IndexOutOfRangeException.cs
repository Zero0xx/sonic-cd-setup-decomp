using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C3 RID: 195
	[ComVisible(true)]
	[Serializable]
	public sealed class IndexOutOfRangeException : SystemException
	{
		// Token: 0x06000B01 RID: 2817 RVA: 0x0002254F File Offset: 0x0002154F
		public IndexOutOfRangeException() : base(Environment.GetResourceString("Arg_IndexOutOfRangeException"))
		{
			base.SetErrorCode(-2146233080);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002256C File Offset: 0x0002156C
		public IndexOutOfRangeException(string message) : base(message)
		{
			base.SetErrorCode(-2146233080);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00022580 File Offset: 0x00021580
		public IndexOutOfRangeException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233080);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00022595 File Offset: 0x00021595
		internal IndexOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
