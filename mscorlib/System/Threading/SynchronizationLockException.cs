using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000168 RID: 360
	[ComVisible(true)]
	[Serializable]
	public class SynchronizationLockException : SystemException
	{
		// Token: 0x06001301 RID: 4865 RVA: 0x0003477F File Offset: 0x0003377F
		public SynchronizationLockException() : base(Environment.GetResourceString("Arg_SynchronizationLockException"))
		{
			base.SetErrorCode(-2146233064);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0003479C File Offset: 0x0003379C
		public SynchronizationLockException(string message) : base(message)
		{
			base.SetErrorCode(-2146233064);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000347B0 File Offset: 0x000337B0
		public SynchronizationLockException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233064);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000347C5 File Offset: 0x000337C5
		protected SynchronizationLockException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
