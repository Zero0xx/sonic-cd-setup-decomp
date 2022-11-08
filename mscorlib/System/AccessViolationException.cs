using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000047 RID: 71
	[ComVisible(true)]
	[Serializable]
	public class AccessViolationException : SystemException
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x00010623 File Offset: 0x0000F623
		public AccessViolationException() : base(Environment.GetResourceString("Arg_AccessViolationException"))
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00010640 File Offset: 0x0000F640
		public AccessViolationException(string message) : base(message)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00010654 File Offset: 0x0000F654
		public AccessViolationException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010669 File Offset: 0x0000F669
		protected AccessViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04000184 RID: 388
		private IntPtr _ip;

		// Token: 0x04000185 RID: 389
		private IntPtr _target;

		// Token: 0x04000186 RID: 390
		private int _accessType;
	}
}
