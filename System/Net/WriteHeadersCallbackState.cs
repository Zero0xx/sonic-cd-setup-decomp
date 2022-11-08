using System;

namespace System.Net
{
	// Token: 0x020004CC RID: 1228
	internal struct WriteHeadersCallbackState
	{
		// Token: 0x06002601 RID: 9729 RVA: 0x00099898 File Offset: 0x00098898
		internal WriteHeadersCallbackState(HttpWebRequest request, ConnectStream stream)
		{
			this.request = request;
			this.stream = stream;
		}

		// Token: 0x040025B8 RID: 9656
		internal HttpWebRequest request;

		// Token: 0x040025B9 RID: 9657
		internal ConnectStream stream;
	}
}
