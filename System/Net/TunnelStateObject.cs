using System;

namespace System.Net
{
	// Token: 0x020004C4 RID: 1220
	internal struct TunnelStateObject
	{
		// Token: 0x060025A9 RID: 9641 RVA: 0x0009608C File Offset: 0x0009508C
		internal TunnelStateObject(HttpWebRequest r, Connection c)
		{
			this.Connection = c;
			this.OriginalRequest = r;
		}

		// Token: 0x0400256B RID: 9579
		internal Connection Connection;

		// Token: 0x0400256C RID: 9580
		internal HttpWebRequest OriginalRequest;
	}
}
