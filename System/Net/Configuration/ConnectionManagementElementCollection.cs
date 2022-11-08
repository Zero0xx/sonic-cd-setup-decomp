using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000648 RID: 1608
	[ConfigurationCollection(typeof(ConnectionManagementElement))]
	public sealed class ConnectionManagementElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000B6B RID: 2923
		public ConnectionManagementElement this[int index]
		{
			get
			{
				return (ConnectionManagementElement)base.BaseGet(index);
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

		// Token: 0x17000B6C RID: 2924
		public ConnectionManagementElement this[string name]
		{
			get
			{
				return (ConnectionManagementElement)base.BaseGet(name);
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

		// Token: 0x060031CE RID: 12750 RVA: 0x000D4B07 File Offset: 0x000D3B07
		public void Add(ConnectionManagementElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000D4B10 File Offset: 0x000D3B10
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000D4B18 File Offset: 0x000D3B18
		protected override ConfigurationElement CreateNewElement()
		{
			return new ConnectionManagementElement();
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x000D4B1F File Offset: 0x000D3B1F
		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ((ConnectionManagementElement)element).Key;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x000D4B3A File Offset: 0x000D3B3A
		public int IndexOf(ConnectionManagementElement element)
		{
			return base.BaseIndexOf(element);
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000D4B43 File Offset: 0x000D3B43
		public void Remove(ConnectionManagementElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			base.BaseRemove(element.Key);
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000D4B5F File Offset: 0x000D3B5F
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000D4B68 File Offset: 0x000D3B68
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}
	}
}
