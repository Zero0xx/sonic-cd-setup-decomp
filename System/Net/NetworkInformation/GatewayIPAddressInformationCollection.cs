using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E8 RID: 1512
	public class GatewayIPAddressInformationCollection : ICollection<GatewayIPAddressInformation>, IEnumerable<GatewayIPAddressInformation>, IEnumerable
	{
		// Token: 0x06002FA9 RID: 12201 RVA: 0x000CEE7B File Offset: 0x000CDE7B
		protected internal GatewayIPAddressInformationCollection()
		{
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000CEE8E File Offset: 0x000CDE8E
		public virtual void CopyTo(GatewayIPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002FAB RID: 12203 RVA: 0x000CEE9D File Offset: 0x000CDE9D
		public virtual int Count
		{
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002FAC RID: 12204 RVA: 0x000CEEAA File Offset: 0x000CDEAA
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A61 RID: 2657
		public virtual GatewayIPAddressInformation this[int index]
		{
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000CEEBB File Offset: 0x000CDEBB
		public virtual void Add(GatewayIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000CEECC File Offset: 0x000CDECC
		internal void InternalAdd(GatewayIPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000CEEDA File Offset: 0x000CDEDA
		public virtual bool Contains(GatewayIPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000CEEE8 File Offset: 0x000CDEE8
		public virtual IEnumerator<GatewayIPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000CEEF5 File Offset: 0x000CDEF5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x000CEEF8 File Offset: 0x000CDEF8
		public virtual bool Remove(GatewayIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000CEF09 File Offset: 0x000CDF09
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04002CB4 RID: 11444
		private Collection<GatewayIPAddressInformation> addresses = new Collection<GatewayIPAddressInformation>();
	}
}
