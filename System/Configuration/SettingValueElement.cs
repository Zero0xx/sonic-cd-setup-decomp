using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000722 RID: 1826
	public sealed class SettingValueElement : ConfigurationElement
	{
		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x000EBE3F File Offset: 0x000EAE3F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (SettingValueElement._properties == null)
				{
					SettingValueElement._properties = new ConfigurationPropertyCollection();
				}
				return SettingValueElement._properties;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x060037BF RID: 14271 RVA: 0x000EBE57 File Offset: 0x000EAE57
		// (set) Token: 0x060037C0 RID: 14272 RVA: 0x000EBE5F File Offset: 0x000EAE5F
		public XmlNode ValueXml
		{
			get
			{
				return this._valueXml;
			}
			set
			{
				this._valueXml = value;
				this.isModified = true;
			}
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x000EBE6F File Offset: 0x000EAE6F
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.ValueXml = SettingValueElement.doc.ReadNode(reader);
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000EBE84 File Offset: 0x000EAE84
		public override bool Equals(object settingValue)
		{
			SettingValueElement settingValueElement = settingValue as SettingValueElement;
			return settingValueElement != null && object.Equals(settingValueElement.ValueXml, this.ValueXml);
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x000EBEAE File Offset: 0x000EAEAE
		public override int GetHashCode()
		{
			return this.ValueXml.GetHashCode();
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000EBEBB File Offset: 0x000EAEBB
		protected override bool IsModified()
		{
			return this.isModified;
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000EBEC3 File Offset: 0x000EAEC3
		protected override void ResetModified()
		{
			this.isModified = false;
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x000EBECC File Offset: 0x000EAECC
		protected override bool SerializeToXmlElement(XmlWriter writer, string elementName)
		{
			if (this.ValueXml != null)
			{
				if (writer != null)
				{
					this.ValueXml.WriteTo(writer);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000EBEE8 File Offset: 0x000EAEE8
		protected override void Reset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
			this.ValueXml = ((SettingValueElement)parentElement).ValueXml;
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000EBF02 File Offset: 0x000EAF02
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			this.ValueXml = ((SettingValueElement)sourceElement).ValueXml;
		}

		// Token: 0x040031DA RID: 12762
		private static ConfigurationPropertyCollection _properties;

		// Token: 0x040031DB RID: 12763
		private static XmlDocument doc = new XmlDocument();

		// Token: 0x040031DC RID: 12764
		private XmlNode _valueXml;

		// Token: 0x040031DD RID: 12765
		private bool isModified;
	}
}
