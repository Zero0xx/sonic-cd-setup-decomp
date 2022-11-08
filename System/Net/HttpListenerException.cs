using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x020003CF RID: 975
	[Serializable]
	public class HttpListenerException : Win32Exception
	{
		// Token: 0x06001ECF RID: 7887 RVA: 0x000775E8 File Offset: 0x000765E8
		public HttpListenerException() : base(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000775F5 File Offset: 0x000765F5
		public HttpListenerException(int errorCode) : base(errorCode)
		{
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000775FE File Offset: 0x000765FE
		public HttpListenerException(int errorCode, string message) : base(errorCode, message)
		{
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x00077608 File Offset: 0x00076608
		protected HttpListenerException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x00077612 File Offset: 0x00076612
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}
	}
}
