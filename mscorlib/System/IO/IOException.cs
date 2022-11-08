using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x020005AE RID: 1454
	[ComVisible(true)]
	[Serializable]
	public class IOException : SystemException
	{
		// Token: 0x06003590 RID: 13712 RVA: 0x000B2A1A File Offset: 0x000B1A1A
		public IOException() : base(Environment.GetResourceString("Arg_IOException"))
		{
			base.SetErrorCode(-2146232800);
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000B2A37 File Offset: 0x000B1A37
		public IOException(string message) : base(message)
		{
			base.SetErrorCode(-2146232800);
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000B2A4B File Offset: 0x000B1A4B
		public IOException(string message, int hresult) : base(message)
		{
			base.SetErrorCode(hresult);
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000B2A5B File Offset: 0x000B1A5B
		internal IOException(string message, int hresult, string maybeFullPath) : base(message)
		{
			base.SetErrorCode(hresult);
			this._maybeFullPath = maybeFullPath;
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000B2A72 File Offset: 0x000B1A72
		public IOException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146232800);
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000B2A87 File Offset: 0x000B1A87
		protected IOException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04001C24 RID: 7204
		[NonSerialized]
		private string _maybeFullPath;
	}
}
