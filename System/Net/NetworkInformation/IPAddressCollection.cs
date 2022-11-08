using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E5 RID: 1509
	public class IPAddressCollection : ICollection<IPAddress>, IEnumerable<IPAddress>, IEnumerable
	{
		// Token: 0x06002F99 RID: 12185 RVA: 0x000CEDBD File Offset: 0x000CDDBD
		protected internal IPAddressCollection()
		{
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000CEDD0 File Offset: 0x000CDDD0
		public virtual void CopyTo(IPAddress[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06002F9B RID: 12187 RVA: 0x000CEDDF File Offset: 0x000CDDDF
		public virtual int Count
		{
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06002F9C RID: 12188 RVA: 0x000CEDEC File Offset: 0x000CDDEC
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x000CEDEF File Offset: 0x000CDDEF
		public virtual void Add(IPAddress address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000CEE00 File Offset: 0x000CDE00
		internal void InternalAdd(IPAddress address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000CEE0E File Offset: 0x000CDE0E
		public virtual bool Contains(IPAddress address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000CEE1C File Offset: 0x000CDE1C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000CEE1F File Offset: 0x000CDE1F
		public virtual IEnumerator<IPAddress> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x17000A5C RID: 2652
		public virtual IPAddress this[int index]
		{
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000CEE3A File Offset: 0x000CDE3A
		public virtual bool Remove(IPAddress address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000CEE4B File Offset: 0x000CDE4B
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04002CB2 RID: 11442
		private Collection<IPAddress> addresses = new Collection<IPAddress>();
	}
}
