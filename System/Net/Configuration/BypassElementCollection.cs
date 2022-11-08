using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000644 RID: 1604
	[ConfigurationCollection(typeof(BypassElement))]
	public sealed class BypassElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000B5C RID: 2908
		public BypassElement this[int index]
		{
			get
			{
				return (BypassElement)base.BaseGet(index);
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

		// Token: 0x17000B5D RID: 2909
		public BypassElement this[string name]
		{
			get
			{
				return (BypassElement)base.BaseGet(name);
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

		// Token: 0x060031AC RID: 12716 RVA: 0x000D4834 File Offset: 0x000D3834
		public void Add(BypassElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000D483D File Offset: 0x000D383D
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000D4845 File Offset: 0x000D3845
		protected override ConfigurationElement CreateNewElement()
		{
			return new BypassElement();
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000D484C File Offset: 0x000D384C
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((BypassElement)element).Key;
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000D4867 File Offset: 0x000D3867
		public int IndexOf(BypassElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000D4870 File Offset: 0x000D3870
		public void Remove(BypassElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000D488C File Offset: 0x000D388C
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000D4895 File Offset: 0x000D3895
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x060031B4 RID: 12724 RVA: 0x000D489E File Offset: 0x000D389E
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return false;
			}
		}
	}
}
