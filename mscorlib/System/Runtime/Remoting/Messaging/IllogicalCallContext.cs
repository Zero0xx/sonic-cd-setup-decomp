using System;
using System.Collections;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006A9 RID: 1705
	internal class IllogicalCallContext : ICloneable
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x000D272B File Offset: 0x000D172B
		private Hashtable Datastore
		{
			get
			{
				if (this.m_Datastore == null)
				{
					this.m_Datastore = new Hashtable();
				}
				return this.m_Datastore;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06003D92 RID: 15762 RVA: 0x000D2746 File Offset: 0x000D1746
		// (set) Token: 0x06003D93 RID: 15763 RVA: 0x000D274E File Offset: 0x000D174E
		internal object HostContext
		{
			get
			{
				return this.m_HostContext;
			}
			set
			{
				this.m_HostContext = value;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x000D2757 File Offset: 0x000D1757
		internal bool HasUserData
		{
			get
			{
				return this.m_Datastore != null && this.m_Datastore.Count > 0;
			}
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x000D2771 File Offset: 0x000D1771
		public void FreeNamedDataSlot(string name)
		{
			this.Datastore.Remove(name);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x000D277F File Offset: 0x000D177F
		public object GetData(string name)
		{
			return this.Datastore[name];
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x000D278D File Offset: 0x000D178D
		public void SetData(string name, object data)
		{
			this.Datastore[name] = data;
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x000D279C File Offset: 0x000D179C
		public object Clone()
		{
			IllogicalCallContext illogicalCallContext = new IllogicalCallContext();
			if (this.HasUserData)
			{
				IDictionaryEnumerator enumerator = this.m_Datastore.GetEnumerator();
				while (enumerator.MoveNext())
				{
					illogicalCallContext.Datastore[(string)enumerator.Key] = enumerator.Value;
				}
			}
			return illogicalCallContext;
		}

		// Token: 0x04001F77 RID: 8055
		private Hashtable m_Datastore;

		// Token: 0x04001F78 RID: 8056
		private object m_HostContext;
	}
}
