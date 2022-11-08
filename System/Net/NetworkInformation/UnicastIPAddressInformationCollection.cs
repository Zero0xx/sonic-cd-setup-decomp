using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E2 RID: 1506
	public class UnicastIPAddressInformationCollection : ICollection<UnicastIPAddressInformation>, IEnumerable<UnicastIPAddressInformation>, IEnumerable
	{
		// Token: 0x06002F7A RID: 12154 RVA: 0x000CEC72 File Offset: 0x000CDC72
		protected internal UnicastIPAddressInformationCollection()
		{
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000CEC85 File Offset: 0x000CDC85
		public virtual void CopyTo(UnicastIPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x000CEC94 File Offset: 0x000CDC94
		public virtual int Count
		{
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002F7D RID: 12157 RVA: 0x000CECA1 File Offset: 0x000CDCA1
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000CECA4 File Offset: 0x000CDCA4
		public virtual void Add(UnicastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000CECB5 File Offset: 0x000CDCB5
		internal void InternalAdd(UnicastIPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x000CECC3 File Offset: 0x000CDCC3
		public virtual bool Contains(UnicastIPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000CECD1 File Offset: 0x000CDCD1
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000CECD9 File Offset: 0x000CDCD9
		public virtual IEnumerator<UnicastIPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x17000A50 RID: 2640
		public virtual UnicastIPAddressInformation this[int index]
		{
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000CECF4 File Offset: 0x000CDCF4
		public virtual bool Remove(UnicastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000CED05 File Offset: 0x000CDD05
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04002CB0 RID: 11440
		private Collection<UnicastIPAddressInformation> addresses = new Collection<UnicastIPAddressInformation>();
	}
}
