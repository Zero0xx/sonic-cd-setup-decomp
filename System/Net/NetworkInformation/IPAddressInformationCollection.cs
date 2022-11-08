using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005D9 RID: 1497
	public class IPAddressInformationCollection : ICollection<IPAddressInformation>, IEnumerable<IPAddressInformation>, IEnumerable
	{
		// Token: 0x06002F22 RID: 12066 RVA: 0x000CEB92 File Offset: 0x000CDB92
		internal IPAddressInformationCollection()
		{
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000CEBA5 File Offset: 0x000CDBA5
		public virtual void CopyTo(IPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06002F24 RID: 12068 RVA: 0x000CEBB4 File Offset: 0x000CDBB4
		public virtual int Count
		{
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06002F25 RID: 12069 RVA: 0x000CEBC1 File Offset: 0x000CDBC1
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000CEBC4 File Offset: 0x000CDBC4
		public virtual void Add(IPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000CEBD5 File Offset: 0x000CDBD5
		internal void InternalAdd(IPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000CEBE3 File Offset: 0x000CDBE3
		public virtual bool Contains(IPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000CEBF1 File Offset: 0x000CDBF1
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000CEBF4 File Offset: 0x000CDBF4
		public virtual IEnumerator<IPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x17000A15 RID: 2581
		public virtual IPAddressInformation this[int index]
		{
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000CEC0F File Offset: 0x000CDC0F
		public virtual bool Remove(IPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000CEC20 File Offset: 0x000CDC20
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04002C79 RID: 11385
		private Collection<IPAddressInformation> addresses = new Collection<IPAddressInformation>();
	}
}
