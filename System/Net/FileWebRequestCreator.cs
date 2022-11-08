using System;

namespace System.Net
{
	// Token: 0x020003B1 RID: 945
	internal class FileWebRequestCreator : IWebRequestCreate
	{
		// Token: 0x06001DB9 RID: 7609 RVA: 0x00071035 File Offset: 0x00070035
		internal FileWebRequestCreator()
		{
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0007103D File Offset: 0x0007003D
		public WebRequest Create(Uri uri)
		{
			return new FileWebRequest(uri);
		}
	}
}
