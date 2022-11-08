using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000039 RID: 57
	[ComVisible(true)]
	[Serializable]
	public sealed class StackOverflowException : SystemException
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0000E33E File Offset: 0x0000D33E
		public StackOverflowException() : base(Environment.GetResourceString("Arg_StackOverflowException"))
		{
			base.SetErrorCode(-2147023895);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000E35B File Offset: 0x0000D35B
		public StackOverflowException(string message) : base(message)
		{
			base.SetErrorCode(-2147023895);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000E36F File Offset: 0x0000D36F
		public StackOverflowException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147023895);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000E384 File Offset: 0x0000D384
		internal StackOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
