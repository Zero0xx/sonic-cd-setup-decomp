using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000B6 RID: 182
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class ComponentCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000666 RID: 1638 RVA: 0x00018721 File Offset: 0x00017721
		public ComponentCollection(IComponent[] components)
		{
			base.InnerList.AddRange(components);
		}

		// Token: 0x17000153 RID: 339
		public virtual IComponent this[string name]
		{
			get
			{
				if (name != null)
				{
					IList innerList = base.InnerList;
					foreach (object obj in innerList)
					{
						IComponent component = (IComponent)obj;
						if (component != null && component.Site != null && component.Site.Name != null && string.Equals(component.Site.Name, name, StringComparison.OrdinalIgnoreCase))
						{
							return component;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000154 RID: 340
		public virtual IComponent this[int index]
		{
			get
			{
				return (IComponent)base.InnerList[index];
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x000187DB File Offset: 0x000177DB
		public void CopyTo(IComponent[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
