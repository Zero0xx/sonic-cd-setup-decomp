using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	// Token: 0x0200075D RID: 1885
	[ComVisible(true)]
	public class TypeEntry
	{
		// Token: 0x06004319 RID: 17177 RVA: 0x000E5612 File Offset: 0x000E4612
		protected TypeEntry()
		{
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600431A RID: 17178 RVA: 0x000E561A File Offset: 0x000E461A
		// (set) Token: 0x0600431B RID: 17179 RVA: 0x000E5622 File Offset: 0x000E4622
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
			set
			{
				this._typeName = value;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600431C RID: 17180 RVA: 0x000E562B File Offset: 0x000E462B
		// (set) Token: 0x0600431D RID: 17181 RVA: 0x000E5633 File Offset: 0x000E4633
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				this._assemblyName = value;
			}
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x000E563C File Offset: 0x000E463C
		internal void CacheRemoteAppEntry(RemoteAppEntry entry)
		{
			this._cachedRemoteAppEntry = entry;
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x000E5645 File Offset: 0x000E4645
		internal RemoteAppEntry GetRemoteAppEntry()
		{
			return this._cachedRemoteAppEntry;
		}

		// Token: 0x040021C3 RID: 8643
		private string _typeName;

		// Token: 0x040021C4 RID: 8644
		private string _assemblyName;

		// Token: 0x040021C5 RID: 8645
		private RemoteAppEntry _cachedRemoteAppEntry;
	}
}
