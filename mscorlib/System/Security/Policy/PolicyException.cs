using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Policy
{
	// Token: 0x020004AD RID: 1197
	[ComVisible(true)]
	[Serializable]
	public class PolicyException : SystemException
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x000A163B File Offset: 0x000A063B
		public PolicyException() : base(Environment.GetResourceString("Policy_Default"))
		{
			base.HResult = -2146233322;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000A1658 File Offset: 0x000A0658
		public PolicyException(string message) : base(message)
		{
			base.HResult = -2146233322;
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000A166C File Offset: 0x000A066C
		public PolicyException(string message, Exception exception) : base(message, exception)
		{
			base.HResult = -2146233322;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000A1681 File Offset: 0x000A0681
		protected PolicyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000A168B File Offset: 0x000A068B
		internal PolicyException(string message, int hresult) : base(message)
		{
			base.HResult = hresult;
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000A169B File Offset: 0x000A069B
		internal PolicyException(string message, int hresult, Exception exception) : base(message, exception)
		{
			base.HResult = hresult;
		}
	}
}
