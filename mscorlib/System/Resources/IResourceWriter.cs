using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x02000431 RID: 1073
	[ComVisible(true)]
	public interface IResourceWriter : IDisposable
	{
		// Token: 0x06002BCB RID: 11211
		void AddResource(string name, string value);

		// Token: 0x06002BCC RID: 11212
		void AddResource(string name, object value);

		// Token: 0x06002BCD RID: 11213
		void AddResource(string name, byte[] value);

		// Token: 0x06002BCE RID: 11214
		void Close();

		// Token: 0x06002BCF RID: 11215
		void Generate();
	}
}
