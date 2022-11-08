using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000184 RID: 388
	[ComVisible(false)]
	[Serializable]
	public class WaitHandleCannotBeOpenedException : ApplicationException
	{
		// Token: 0x06001423 RID: 5155 RVA: 0x0003653B File Offset: 0x0003553B
		public WaitHandleCannotBeOpenedException() : base(Environment.GetResourceString("Threading.WaitHandleCannotBeOpenedException"))
		{
			base.SetErrorCode(-2146233044);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00036558 File Offset: 0x00035558
		public WaitHandleCannotBeOpenedException(string message) : base(message)
		{
			base.SetErrorCode(-2146233044);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0003656C File Offset: 0x0003556C
		public WaitHandleCannotBeOpenedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233044);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00036581 File Offset: 0x00035581
		protected WaitHandleCannotBeOpenedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
