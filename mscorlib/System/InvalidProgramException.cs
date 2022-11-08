using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000CC RID: 204
	[ComVisible(true)]
	[Serializable]
	public sealed class InvalidProgramException : SystemException
	{
		// Token: 0x06000B8B RID: 2955 RVA: 0x00023145 File Offset: 0x00022145
		public InvalidProgramException() : base(Environment.GetResourceString("InvalidProgram_Default"))
		{
			base.SetErrorCode(-2146233030);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00023162 File Offset: 0x00022162
		public InvalidProgramException(string message) : base(message)
		{
			base.SetErrorCode(-2146233030);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00023176 File Offset: 0x00022176
		public InvalidProgramException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233030);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002318B File Offset: 0x0002218B
		internal InvalidProgramException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
