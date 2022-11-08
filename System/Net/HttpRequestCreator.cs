using System;

namespace System.Net
{
	// Token: 0x0200040D RID: 1037
	internal class HttpRequestCreator : IWebRequestCreate
	{
		// Token: 0x060020B9 RID: 8377 RVA: 0x00080F0E File Offset: 0x0007FF0E
		public WebRequest Create(Uri Uri)
		{
			return new HttpWebRequest(Uri, null);
		}
	}
}
