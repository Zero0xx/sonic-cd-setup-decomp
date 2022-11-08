using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000115 RID: 277
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ListSortDescription
	{
		// Token: 0x0600088B RID: 2187 RVA: 0x0001CD68 File Offset: 0x0001BD68
		public ListSortDescription(PropertyDescriptor property, ListSortDirection direction)
		{
			this.property = property;
			this.sortDirection = direction;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001CD7E File Offset: 0x0001BD7E
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x0001CD86 File Offset: 0x0001BD86
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.property;
			}
			set
			{
				this.property = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0001CD8F File Offset: 0x0001BD8F
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x0001CD97 File Offset: 0x0001BD97
		public ListSortDirection SortDirection
		{
			get
			{
				return this.sortDirection;
			}
			set
			{
				this.sortDirection = value;
			}
		}

		// Token: 0x040009B0 RID: 2480
		private PropertyDescriptor property;

		// Token: 0x040009B1 RID: 2481
		private ListSortDirection sortDirection;
	}
}
