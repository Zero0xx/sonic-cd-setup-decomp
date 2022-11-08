﻿using System;
using System.IO;

namespace System.Net
{
	// Token: 0x0200040E RID: 1038
	internal class CoreResponseData
	{
		// Token: 0x060020BB RID: 8379 RVA: 0x00080F20 File Offset: 0x0007FF20
		internal CoreResponseData Clone()
		{
			return new CoreResponseData
			{
				m_StatusCode = this.m_StatusCode,
				m_StatusDescription = this.m_StatusDescription,
				m_IsVersionHttp11 = this.m_IsVersionHttp11,
				m_ContentLength = this.m_ContentLength,
				m_ResponseHeaders = this.m_ResponseHeaders,
				m_ConnectStream = this.m_ConnectStream
			};
		}

		// Token: 0x040020C5 RID: 8389
		public HttpStatusCode m_StatusCode;

		// Token: 0x040020C6 RID: 8390
		public string m_StatusDescription;

		// Token: 0x040020C7 RID: 8391
		public bool m_IsVersionHttp11;

		// Token: 0x040020C8 RID: 8392
		public long m_ContentLength;

		// Token: 0x040020C9 RID: 8393
		public WebHeaderCollection m_ResponseHeaders;

		// Token: 0x040020CA RID: 8394
		public Stream m_ConnectStream;
	}
}
