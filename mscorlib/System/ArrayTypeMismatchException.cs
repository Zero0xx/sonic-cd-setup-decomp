using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000072 RID: 114
	[ComVisible(true)]
	[Serializable]
	public class ArrayTypeMismatchException : SystemException
	{
		// Token: 0x06000672 RID: 1650 RVA: 0x00015D36 File Offset: 0x00014D36
		public ArrayTypeMismatchException() : base(Environment.GetResourceString("Arg_ArrayTypeMismatchException"))
		{
			base.SetErrorCode(-2146233085);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00015D53 File Offset: 0x00014D53
		public ArrayTypeMismatchException(string message) : base(message)
		{
			base.SetErrorCode(-2146233085);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00015D67 File Offset: 0x00014D67
		public ArrayTypeMismatchException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233085);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00015D7C File Offset: 0x00014D7C
		protected ArrayTypeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
