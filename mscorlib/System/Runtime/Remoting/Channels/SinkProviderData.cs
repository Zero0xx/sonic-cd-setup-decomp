using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006F0 RID: 1776
	[ComVisible(true)]
	public class SinkProviderData
	{
		// Token: 0x06003F60 RID: 16224 RVA: 0x000D864B File Offset: 0x000D764B
		public SinkProviderData(string name)
		{
			this._name = name;
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003F61 RID: 16225 RVA: 0x000D8675 File Offset: 0x000D7675
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003F62 RID: 16226 RVA: 0x000D867D File Offset: 0x000D767D
		public IDictionary Properties
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x000D8685 File Offset: 0x000D7685
		public IList Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x0400201B RID: 8219
		private string _name;

		// Token: 0x0400201C RID: 8220
		private Hashtable _properties = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

		// Token: 0x0400201D RID: 8221
		private ArrayList _children = new ArrayList();
	}
}
