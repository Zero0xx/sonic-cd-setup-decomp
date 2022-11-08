using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200003A RID: 58
	[ComVisible(true)]
	[Serializable]
	public sealed class DataMisalignedException : SystemException
	{
		// Token: 0x0600037F RID: 895 RVA: 0x0000E38E File Offset: 0x0000D38E
		public DataMisalignedException() : base(Environment.GetResourceString("Arg_DataMisalignedException"))
		{
			base.SetErrorCode(-2146233023);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000E3AB File Offset: 0x0000D3AB
		public DataMisalignedException(string message) : base(message)
		{
			base.SetErrorCode(-2146233023);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000E3BF File Offset: 0x0000D3BF
		public DataMisalignedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233023);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000E3D4 File Offset: 0x0000D3D4
		internal DataMisalignedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
