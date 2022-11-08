using System;

namespace System.Net
{
	// Token: 0x020003C0 RID: 960
	internal class FtpWebRequestCreator : IWebRequestCreate
	{
		// Token: 0x06001E3E RID: 7742 RVA: 0x00074106 File Offset: 0x00073106
		internal FtpWebRequestCreator()
		{
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0007410E File Offset: 0x0007310E
		public WebRequest Create(Uri uri)
		{
			return new FtpWebRequest(uri);
		}
	}
}
