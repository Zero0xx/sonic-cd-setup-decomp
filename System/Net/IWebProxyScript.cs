using System;

namespace System.Net
{
	// Token: 0x020004B2 RID: 1202
	public interface IWebProxyScript
	{
		// Token: 0x06002530 RID: 9520
		bool Load(Uri scriptLocation, string script, Type helperType);

		// Token: 0x06002531 RID: 9521
		string Run(string url, string host);

		// Token: 0x06002532 RID: 9522
		void Close();
	}
}
