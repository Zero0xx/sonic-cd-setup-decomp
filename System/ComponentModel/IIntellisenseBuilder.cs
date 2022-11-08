using System;

namespace System.ComponentModel
{
	// Token: 0x020000F0 RID: 240
	public interface IIntellisenseBuilder
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007F1 RID: 2033
		string Name { get; }

		// Token: 0x060007F2 RID: 2034
		bool Show(string language, string value, ref string newValue);
	}
}
