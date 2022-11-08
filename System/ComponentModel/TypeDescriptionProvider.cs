using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000CF RID: 207
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class TypeDescriptionProvider
	{
		// Token: 0x0600070C RID: 1804 RVA: 0x0001A4C7 File Offset: 0x000194C7
		protected TypeDescriptionProvider()
		{
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001A4CF File Offset: 0x000194CF
		protected TypeDescriptionProvider(TypeDescriptionProvider parent)
		{
			this._parent = parent;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001A4DE File Offset: 0x000194DE
		public virtual object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		{
			if (this._parent != null)
			{
				return this._parent.CreateInstance(provider, objectType, argTypes, args);
			}
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			return SecurityUtils.SecureCreateInstance(objectType, args);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001A50F File Offset: 0x0001950F
		public virtual IDictionary GetCache(object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetCache(instance);
			}
			return null;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001A527 File Offset: 0x00019527
		public virtual ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetExtendedTypeDescriptor(instance);
			}
			if (this._emptyDescriptor == null)
			{
				this._emptyDescriptor = new TypeDescriptionProvider.EmptyCustomTypeDescriptor();
			}
			return this._emptyDescriptor;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001A557 File Offset: 0x00019557
		public virtual string GetFullComponentName(object component)
		{
			if (this._parent != null)
			{
				return this._parent.GetFullComponentName(component);
			}
			return this.GetTypeDescriptor(component).GetComponentName();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001A57A File Offset: 0x0001957A
		public Type GetReflectionType(Type objectType)
		{
			return this.GetReflectionType(objectType, null);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001A584 File Offset: 0x00019584
		public Type GetReflectionType(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return this.GetReflectionType(instance.GetType(), instance);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001A5A1 File Offset: 0x000195A1
		public virtual Type GetReflectionType(Type objectType, object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetReflectionType(objectType, instance);
			}
			return objectType;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001A5BA File Offset: 0x000195BA
		public ICustomTypeDescriptor GetTypeDescriptor(Type objectType)
		{
			return this.GetTypeDescriptor(objectType, null);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001A5C4 File Offset: 0x000195C4
		public ICustomTypeDescriptor GetTypeDescriptor(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return this.GetTypeDescriptor(instance.GetType(), instance);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001A5E1 File Offset: 0x000195E1
		public virtual ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			if (this._parent != null)
			{
				return this._parent.GetTypeDescriptor(objectType, instance);
			}
			if (this._emptyDescriptor == null)
			{
				this._emptyDescriptor = new TypeDescriptionProvider.EmptyCustomTypeDescriptor();
			}
			return this._emptyDescriptor;
		}

		// Token: 0x0400093E RID: 2366
		private TypeDescriptionProvider _parent;

		// Token: 0x0400093F RID: 2367
		private TypeDescriptionProvider.EmptyCustomTypeDescriptor _emptyDescriptor;

		// Token: 0x020000D0 RID: 208
		private sealed class EmptyCustomTypeDescriptor : CustomTypeDescriptor
		{
		}
	}
}
