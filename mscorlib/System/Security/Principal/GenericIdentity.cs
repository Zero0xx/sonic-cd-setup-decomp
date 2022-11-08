using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004C5 RID: 1221
	[ComVisible(true)]
	[Serializable]
	public class GenericIdentity : IIdentity
	{
		// Token: 0x060030E4 RID: 12516 RVA: 0x000A7B20 File Offset: 0x000A6B20
		public GenericIdentity(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_name = name;
			this.m_type = "";
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000A7B48 File Offset: 0x000A6B48
		public GenericIdentity(string name, string type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.m_name = name;
			this.m_type = type;
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x000A7B7A File Offset: 0x000A6B7A
		public virtual string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x000A7B82 File Offset: 0x000A6B82
		public virtual string AuthenticationType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060030E8 RID: 12520 RVA: 0x000A7B8A File Offset: 0x000A6B8A
		public virtual bool IsAuthenticated
		{
			get
			{
				return !this.m_name.Equals("");
			}
		}

		// Token: 0x0400187D RID: 6269
		private string m_name;

		// Token: 0x0400187E RID: 6270
		private string m_type;
	}
}
