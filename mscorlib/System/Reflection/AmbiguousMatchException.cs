using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020002DB RID: 731
	[ComVisible(true)]
	[Serializable]
	public sealed class AmbiguousMatchException : SystemException
	{
		// Token: 0x06001C0B RID: 7179 RVA: 0x00048825 File Offset: 0x00047825
		public AmbiguousMatchException() : base(Environment.GetResourceString("Arg_AmbiguousMatchException"))
		{
			base.SetErrorCode(-2147475171);
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x00048842 File Offset: 0x00047842
		public AmbiguousMatchException(string message) : base(message)
		{
			base.SetErrorCode(-2147475171);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x00048856 File Offset: 0x00047856
		public AmbiguousMatchException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2147475171);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x0004886B File Offset: 0x0004786B
		internal AmbiguousMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
