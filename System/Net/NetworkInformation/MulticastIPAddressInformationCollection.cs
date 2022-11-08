using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E4 RID: 1508
	public class MulticastIPAddressInformationCollection : ICollection<MulticastIPAddressInformation>, IEnumerable<MulticastIPAddressInformation>, IEnumerable
	{
		// Token: 0x06002F8D RID: 12173 RVA: 0x000CED1E File Offset: 0x000CDD1E
		protected internal MulticastIPAddressInformationCollection()
		{
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000CED31 File Offset: 0x000CDD31
		public virtual void CopyTo(MulticastIPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x000CED40 File Offset: 0x000CDD40
		public virtual int Count
		{
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x000CED4D File Offset: 0x000CDD4D
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000CED50 File Offset: 0x000CDD50
		public virtual void Add(MulticastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000CED61 File Offset: 0x000CDD61
		internal void InternalAdd(MulticastIPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000CED6F File Offset: 0x000CDD6F
		public virtual bool Contains(MulticastIPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000CED7D File Offset: 0x000CDD7D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000CED80 File Offset: 0x000CDD80
		public virtual IEnumerator<MulticastIPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x17000A59 RID: 2649
		public virtual MulticastIPAddressInformation this[int index]
		{
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000CED9B File Offset: 0x000CDD9B
		public virtual bool Remove(MulticastIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000CEDAC File Offset: 0x000CDDAC
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04002CB1 RID: 11441
		private Collection<MulticastIPAddressInformation> addresses = new Collection<MulticastIPAddressInformation>();
	}
}
