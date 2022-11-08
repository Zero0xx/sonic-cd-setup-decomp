using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006BE RID: 1726
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ClientSponsor : MarshalByRefObject, ISponsor
	{
		// Token: 0x06003E11 RID: 15889 RVA: 0x000D430C File Offset: 0x000D330C
		public ClientSponsor()
		{
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x000D4335 File Offset: 0x000D3335
		public ClientSponsor(TimeSpan renewalTime)
		{
			this.m_renewalTime = renewalTime;
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x000D4365 File Offset: 0x000D3365
		// (set) Token: 0x06003E14 RID: 15892 RVA: 0x000D436D File Offset: 0x000D336D
		public TimeSpan RenewalTime
		{
			get
			{
				return this.m_renewalTime;
			}
			set
			{
				this.m_renewalTime = value;
			}
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x000D4378 File Offset: 0x000D3378
		public bool Register(MarshalByRefObject obj)
		{
			ILease lease = (ILease)obj.GetLifetimeService();
			if (lease == null)
			{
				return false;
			}
			lease.Register(this);
			lock (this.sponsorTable)
			{
				this.sponsorTable[obj] = lease;
			}
			return true;
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x000D43D4 File Offset: 0x000D33D4
		public void Unregister(MarshalByRefObject obj)
		{
			ILease lease = null;
			lock (this.sponsorTable)
			{
				lease = (ILease)this.sponsorTable[obj];
			}
			if (lease != null)
			{
				lease.Unregister(this);
			}
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x000D4428 File Offset: 0x000D3428
		public TimeSpan Renewal(ILease lease)
		{
			return this.m_renewalTime;
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x000D4430 File Offset: 0x000D3430
		public void Close()
		{
			lock (this.sponsorTable)
			{
				IDictionaryEnumerator enumerator = this.sponsorTable.GetEnumerator();
				while (enumerator.MoveNext())
				{
					((ILease)enumerator.Value).Unregister(this);
				}
				this.sponsorTable.Clear();
			}
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x000D4498 File Offset: 0x000D3498
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x000D449C File Offset: 0x000D349C
		~ClientSponsor()
		{
		}

		// Token: 0x04001FA7 RID: 8103
		private Hashtable sponsorTable = new Hashtable(10);

		// Token: 0x04001FA8 RID: 8104
		private TimeSpan m_renewalTime = TimeSpan.FromMinutes(2.0);
	}
}
