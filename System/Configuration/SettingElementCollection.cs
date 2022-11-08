using System;

namespace System.Configuration
{
	// Token: 0x02000720 RID: 1824
	public sealed class SettingElementCollection : ConfigurationElementCollection
	{
		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x060037A8 RID: 14248 RVA: 0x000EBC68 File Offset: 0x000EAC68
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x060037A9 RID: 14249 RVA: 0x000EBC6B File Offset: 0x000EAC6B
		protected override string ElementName
		{
			get
			{
				return "setting";
			}
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000EBC72 File Offset: 0x000EAC72
		protected override ConfigurationElement CreateNewElement()
		{
			return new SettingElement();
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000EBC79 File Offset: 0x000EAC79
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SettingElement)element).Key;
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x000EBC86 File Offset: 0x000EAC86
		public SettingElement Get(string elementKey)
		{
			return (SettingElement)base.BaseGet(elementKey);
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x000EBC94 File Offset: 0x000EAC94
		public void Add(SettingElement element)
		{
			this.BaseAdd(element);
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x000EBC9D File Offset: 0x000EAC9D
		public void Remove(SettingElement element)
		{
			base.BaseRemove(this.GetElementKey(element));
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x000EBCAC File Offset: 0x000EACAC
		public void Clear()
		{
			base.BaseClear();
		}
	}
}
