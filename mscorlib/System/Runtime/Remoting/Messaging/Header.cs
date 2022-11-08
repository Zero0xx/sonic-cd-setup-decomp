using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006DC RID: 1756
	[ComVisible(true)]
	[Serializable]
	public class Header
	{
		// Token: 0x06003F20 RID: 16160 RVA: 0x000D83AD File Offset: 0x000D73AD
		public Header(string _Name, object _Value) : this(_Name, _Value, true)
		{
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x000D83B8 File Offset: 0x000D73B8
		public Header(string _Name, object _Value, bool _MustUnderstand)
		{
			this.Name = _Name;
			this.Value = _Value;
			this.MustUnderstand = _MustUnderstand;
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x000D83D5 File Offset: 0x000D73D5
		public Header(string _Name, object _Value, bool _MustUnderstand, string _HeaderNamespace)
		{
			this.Name = _Name;
			this.Value = _Value;
			this.MustUnderstand = _MustUnderstand;
			this.HeaderNamespace = _HeaderNamespace;
		}

		// Token: 0x0400200A RID: 8202
		public string Name;

		// Token: 0x0400200B RID: 8203
		public object Value;

		// Token: 0x0400200C RID: 8204
		public bool MustUnderstand;

		// Token: 0x0400200D RID: 8205
		public string HeaderNamespace;
	}
}
