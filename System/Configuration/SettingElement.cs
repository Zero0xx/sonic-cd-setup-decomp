using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000721 RID: 1825
	public sealed class SettingElement : ConfigurationElement
	{
		// Token: 0x060037B1 RID: 14257 RVA: 0x000EBCBC File Offset: 0x000EACBC
		static SettingElement()
		{
			SettingElement._properties = new ConfigurationPropertyCollection();
			SettingElement._properties.Add(SettingElement._propName);
			SettingElement._properties.Add(SettingElement._propSerializeAs);
			SettingElement._properties.Add(SettingElement._propValue);
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x000EBD64 File Offset: 0x000EAD64
		public SettingElement()
		{
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x000EBD6C File Offset: 0x000EAD6C
		public SettingElement(string name, SettingsSerializeAs serializeAs) : this()
		{
			this.Name = name;
			this.SerializeAs = serializeAs;
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x000EBD82 File Offset: 0x000EAD82
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x000EBD8C File Offset: 0x000EAD8C
		public override bool Equals(object settings)
		{
			SettingElement settingElement = settings as SettingElement;
			return settingElement != null && base.Equals(settings) && object.Equals(settingElement.Value, this.Value);
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x000EBDBF File Offset: 0x000EADBF
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.Value.GetHashCode();
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x000EBDD3 File Offset: 0x000EADD3
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SettingElement._properties;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x060037B8 RID: 14264 RVA: 0x000EBDDA File Offset: 0x000EADDA
		// (set) Token: 0x060037B9 RID: 14265 RVA: 0x000EBDEC File Offset: 0x000EADEC
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[SettingElement._propName];
			}
			set
			{
				base[SettingElement._propName] = value;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x000EBDFA File Offset: 0x000EADFA
		// (set) Token: 0x060037BB RID: 14267 RVA: 0x000EBE0C File Offset: 0x000EAE0C
		[ConfigurationProperty("serializeAs", IsRequired = true, DefaultValue = SettingsSerializeAs.String)]
		public SettingsSerializeAs SerializeAs
		{
			get
			{
				return (SettingsSerializeAs)base[SettingElement._propSerializeAs];
			}
			set
			{
				base[SettingElement._propSerializeAs] = value;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x060037BC RID: 14268 RVA: 0x000EBE1F File Offset: 0x000EAE1F
		// (set) Token: 0x060037BD RID: 14269 RVA: 0x000EBE31 File Offset: 0x000EAE31
		[ConfigurationProperty("value", IsRequired = true, DefaultValue = null)]
		public SettingValueElement Value
		{
			get
			{
				return (SettingValueElement)base[SettingElement._propValue];
			}
			set
			{
				base[SettingElement._propValue] = value;
			}
		}

		// Token: 0x040031D5 RID: 12757
		private static ConfigurationPropertyCollection _properties;

		// Token: 0x040031D6 RID: 12758
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040031D7 RID: 12759
		private static readonly ConfigurationProperty _propSerializeAs = new ConfigurationProperty("serializeAs", typeof(SettingsSerializeAs), SettingsSerializeAs.String, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040031D8 RID: 12760
		private static readonly ConfigurationProperty _propValue = new ConfigurationProperty("value", typeof(SettingValueElement), null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040031D9 RID: 12761
		private static XmlDocument doc = new XmlDocument();
	}
}
