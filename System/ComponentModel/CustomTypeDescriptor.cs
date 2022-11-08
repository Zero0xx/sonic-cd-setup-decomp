using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000C3 RID: 195
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class CustomTypeDescriptor : ICustomTypeDescriptor
	{
		// Token: 0x060006B4 RID: 1716 RVA: 0x000196E0 File Offset: 0x000186E0
		protected CustomTypeDescriptor()
		{
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000196E8 File Offset: 0x000186E8
		protected CustomTypeDescriptor(ICustomTypeDescriptor parent)
		{
			this._parent = parent;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000196F7 File Offset: 0x000186F7
		public virtual AttributeCollection GetAttributes()
		{
			if (this._parent != null)
			{
				return this._parent.GetAttributes();
			}
			return AttributeCollection.Empty;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00019712 File Offset: 0x00018712
		public virtual string GetClassName()
		{
			if (this._parent != null)
			{
				return this._parent.GetClassName();
			}
			return null;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00019729 File Offset: 0x00018729
		public virtual string GetComponentName()
		{
			if (this._parent != null)
			{
				return this._parent.GetComponentName();
			}
			return null;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00019740 File Offset: 0x00018740
		public virtual TypeConverter GetConverter()
		{
			if (this._parent != null)
			{
				return this._parent.GetConverter();
			}
			return new TypeConverter();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001975B File Offset: 0x0001875B
		public virtual EventDescriptor GetDefaultEvent()
		{
			if (this._parent != null)
			{
				return this._parent.GetDefaultEvent();
			}
			return null;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00019772 File Offset: 0x00018772
		public virtual PropertyDescriptor GetDefaultProperty()
		{
			if (this._parent != null)
			{
				return this._parent.GetDefaultProperty();
			}
			return null;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00019789 File Offset: 0x00018789
		public virtual object GetEditor(Type editorBaseType)
		{
			if (this._parent != null)
			{
				return this._parent.GetEditor(editorBaseType);
			}
			return null;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000197A1 File Offset: 0x000187A1
		public virtual EventDescriptorCollection GetEvents()
		{
			if (this._parent != null)
			{
				return this._parent.GetEvents();
			}
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000197BC File Offset: 0x000187BC
		public virtual EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			if (this._parent != null)
			{
				return this._parent.GetEvents(attributes);
			}
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000197D8 File Offset: 0x000187D8
		public virtual PropertyDescriptorCollection GetProperties()
		{
			if (this._parent != null)
			{
				return this._parent.GetProperties();
			}
			return PropertyDescriptorCollection.Empty;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000197F3 File Offset: 0x000187F3
		public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			if (this._parent != null)
			{
				return this._parent.GetProperties(attributes);
			}
			return PropertyDescriptorCollection.Empty;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001980F File Offset: 0x0001880F
		public virtual object GetPropertyOwner(PropertyDescriptor pd)
		{
			if (this._parent != null)
			{
				return this._parent.GetPropertyOwner(pd);
			}
			return null;
		}

		// Token: 0x04000926 RID: 2342
		private ICustomTypeDescriptor _parent;
	}
}
