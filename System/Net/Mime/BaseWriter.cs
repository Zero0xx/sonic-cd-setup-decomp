using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x0200067F RID: 1663
	internal abstract class BaseWriter
	{
		// Token: 0x0600337D RID: 13181
		internal abstract IAsyncResult BeginGetContentStream(AsyncCallback callback, object state);

		// Token: 0x0600337E RID: 13182
		internal abstract Stream EndGetContentStream(IAsyncResult result);

		// Token: 0x0600337F RID: 13183
		internal abstract Stream GetContentStream();

		// Token: 0x06003380 RID: 13184
		internal abstract void WriteHeader(string name, string value);

		// Token: 0x06003381 RID: 13185
		internal abstract void WriteHeaders(NameValueCollection headers);

		// Token: 0x06003382 RID: 13186
		internal abstract void Close();
	}
}
