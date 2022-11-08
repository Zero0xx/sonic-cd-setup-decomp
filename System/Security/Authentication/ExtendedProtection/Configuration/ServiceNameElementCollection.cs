using System;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x0200034E RID: 846
	public sealed class ServiceNameElementCollection : ConfigurationElementCollection
	{
		// Token: 0x1700051C RID: 1308
		public ServiceNameElement this[int index]
		{
			get
			{
				return (ServiceNameElement)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x1700051D RID: 1309
		public ServiceNameElement this[string name]
		{
			get
			{
				return (ServiceNameElement)base.BaseGet(name);
			}
			set
			{
				if (base.BaseGet(name) != null)
				{
					base.BaseRemove(name);
				}
				this.BaseAdd(value);
			}
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0005CA8A File Offset: 0x0005BA8A
		public void Add(ServiceNameElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0005CA93 File Offset: 0x0005BA93
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x0005CA9B File Offset: 0x0005BA9B
		protected override ConfigurationElement CreateNewElement()
		{
			return new ServiceNameElement();
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0005CAA2 File Offset: 0x0005BAA2
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((ServiceNameElement)element).Key;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0005CABD File Offset: 0x0005BABD
		public int IndexOf(ServiceNameElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0005CAC6 File Offset: 0x0005BAC6
		public void Remove(ServiceNameElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x0005CAE2 File Offset: 0x0005BAE2
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x0005CAEB File Offset: 0x0005BAEB
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
