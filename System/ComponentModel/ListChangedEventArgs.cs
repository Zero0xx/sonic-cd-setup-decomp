using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000112 RID: 274
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ListChangedEventArgs : EventArgs
	{
		// Token: 0x0600087F RID: 2175 RVA: 0x0001CCF2 File Offset: 0x0001BCF2
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex) : this(listChangedType, newIndex, -1)
		{
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001CCFD File Offset: 0x0001BCFD
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, PropertyDescriptor propDesc) : this(listChangedType, newIndex)
		{
			this.propDesc = propDesc;
			this.oldIndex = newIndex;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001CD15 File Offset: 0x0001BD15
		public ListChangedEventArgs(ListChangedType listChangedType, PropertyDescriptor propDesc)
		{
			this.listChangedType = listChangedType;
			this.propDesc = propDesc;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001CD2B File Offset: 0x0001BD2B
		public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex)
		{
			this.listChangedType = listChangedType;
			this.newIndex = newIndex;
			this.oldIndex = oldIndex;
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0001CD48 File Offset: 0x0001BD48
		public ListChangedType ListChangedType
		{
			get
			{
				return this.listChangedType;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0001CD50 File Offset: 0x0001BD50
		public int NewIndex
		{
			get
			{
				return this.newIndex;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x0001CD58 File Offset: 0x0001BD58
		public int OldIndex
		{
			get
			{
				return this.oldIndex;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0001CD60 File Offset: 0x0001BD60
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propDesc;
			}
		}

		// Token: 0x040009A3 RID: 2467
		private ListChangedType listChangedType;

		// Token: 0x040009A4 RID: 2468
		private int newIndex;

		// Token: 0x040009A5 RID: 2469
		private int oldIndex;

		// Token: 0x040009A6 RID: 2470
		private PropertyDescriptor propDesc;
	}
}
