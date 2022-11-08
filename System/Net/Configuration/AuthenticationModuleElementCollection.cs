using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000640 RID: 1600
	[ConfigurationCollection(typeof(AuthenticationModuleElement))]
	public sealed class AuthenticationModuleElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000B53 RID: 2899
		public AuthenticationModuleElement this[int index]
		{
			get
			{
				return (AuthenticationModuleElement)base.BaseGet(index);
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

		// Token: 0x17000B54 RID: 2900
		public AuthenticationModuleElement this[string name]
		{
			get
			{
				return (AuthenticationModuleElement)base.BaseGet(name);
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

		// Token: 0x06003190 RID: 12688 RVA: 0x000D43B8 File Offset: 0x000D33B8
		public void Add(AuthenticationModuleElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000D43C1 File Offset: 0x000D33C1
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000D43C9 File Offset: 0x000D33C9
		protected override ConfigurationElement CreateNewElement()
		{
			return new AuthenticationModuleElement();
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000D43D0 File Offset: 0x000D33D0
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((AuthenticationModuleElement)element).Key;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000D43EB File Offset: 0x000D33EB
		public int IndexOf(AuthenticationModuleElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000D43F4 File Offset: 0x000D33F4
		public void Remove(AuthenticationModuleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000D4410 File Offset: 0x000D3410
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000D4419 File Offset: 0x000D3419
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
