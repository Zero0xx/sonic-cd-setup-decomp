using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200066C RID: 1644
	[ConfigurationCollection(typeof(WebRequestModuleElement))]
	public sealed class WebRequestModuleElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000BEF RID: 3055
		public WebRequestModuleElement this[int index]
		{
			get
			{
				return (WebRequestModuleElement)base.BaseGet(index);
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

		// Token: 0x17000BF0 RID: 3056
		public WebRequestModuleElement this[string name]
		{
			get
			{
				return (WebRequestModuleElement)base.BaseGet(name);
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

		// Token: 0x060032D2 RID: 13010 RVA: 0x000D751F File Offset: 0x000D651F
		public void Add(WebRequestModuleElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000D7528 File Offset: 0x000D6528
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000D7530 File Offset: 0x000D6530
		protected override ConfigurationElement CreateNewElement()
		{
			return new WebRequestModuleElement();
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000D7537 File Offset: 0x000D6537
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((WebRequestModuleElement)element).Key;
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000D7552 File Offset: 0x000D6552
		public int IndexOf(WebRequestModuleElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000D755B File Offset: 0x000D655B
		public void Remove(WebRequestModuleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000D7577 File Offset: 0x000D6577
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000D7580 File Offset: 0x000D6580
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
