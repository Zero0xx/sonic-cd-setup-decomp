using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051A RID: 1306
	[ComVisible(true)]
	[Serializable]
	public class SEHException : ExternalException
	{
		// Token: 0x060032BA RID: 12986 RVA: 0x000AB56C File Offset: 0x000AA56C
		public SEHException()
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000AB57F File Offset: 0x000AA57F
		public SEHException(string message) : base(message)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x000AB593 File Offset: 0x000AA593
		public SEHException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x000AB5A8 File Offset: 0x000AA5A8
		protected SEHException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x000AB5B2 File Offset: 0x000AA5B2
		public virtual bool CanResume()
		{
			return false;
		}
	}
}
