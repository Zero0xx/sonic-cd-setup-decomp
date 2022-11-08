using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006FC RID: 1788
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class LocalFileSettingsProvider : SettingsProvider, IApplicationSettingsProvider
	{
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06003710 RID: 14096 RVA: 0x000EA014 File Offset: 0x000E9014
		// (set) Token: 0x06003711 RID: 14097 RVA: 0x000EA01C File Offset: 0x000E901C
		public override string ApplicationName
		{
			get
			{
				return this._appName;
			}
			set
			{
				this._appName = value;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06003712 RID: 14098 RVA: 0x000EA025 File Offset: 0x000E9025
		private LocalFileSettingsProvider.XmlEscaper Escaper
		{
			get
			{
				if (this._escaper == null)
				{
					this._escaper = new LocalFileSettingsProvider.XmlEscaper();
				}
				return this._escaper;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06003713 RID: 14099 RVA: 0x000EA040 File Offset: 0x000E9040
		private ClientSettingsStore Store
		{
			get
			{
				if (this._store == null)
				{
					this._store = new ClientSettingsStore();
				}
				return this._store;
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000EA05B File Offset: 0x000E905B
		public override void Initialize(string name, NameValueCollection values)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "LocalFileSettingsProvider";
			}
			base.Initialize(name, values);
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x000EA074 File Offset: 0x000E9074
		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
		{
			SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
			string sectionName = this.GetSectionName(context);
			IDictionary dictionary = this.Store.ReadSettings(sectionName, false);
			IDictionary dictionary2 = this.Store.ReadSettings(sectionName, true);
			ConnectionStringSettingsCollection connectionStringSettingsCollection = this.Store.ReadConnectionStrings();
			foreach (object obj in properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				string name = settingsProperty.Name;
				SettingsPropertyValue settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
				SpecialSettingAttribute specialSettingAttribute = settingsProperty.Attributes[typeof(SpecialSettingAttribute)] as SpecialSettingAttribute;
				bool flag = specialSettingAttribute != null && specialSettingAttribute.SpecialSetting == SpecialSetting.ConnectionString;
				if (flag)
				{
					string name2 = sectionName + "." + name;
					if (connectionStringSettingsCollection != null && connectionStringSettingsCollection[name2] != null)
					{
						settingsPropertyValue.PropertyValue = connectionStringSettingsCollection[name2].ConnectionString;
					}
					else if (settingsProperty.DefaultValue != null && settingsProperty.DefaultValue is string)
					{
						settingsPropertyValue.PropertyValue = settingsProperty.DefaultValue;
					}
					else
					{
						settingsPropertyValue.PropertyValue = string.Empty;
					}
					settingsPropertyValue.IsDirty = false;
					settingsPropertyValueCollection.Add(settingsPropertyValue);
				}
				else
				{
					bool flag2 = this.IsUserSetting(settingsProperty);
					if (flag2 && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
					{
						throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
					}
					IDictionary dictionary3 = flag2 ? dictionary2 : dictionary;
					if (dictionary3.Contains(name))
					{
						StoredSetting storedSetting = (StoredSetting)dictionary3[name];
						string text = storedSetting.Value.InnerXml;
						if (storedSetting.SerializeAs == SettingsSerializeAs.String)
						{
							text = this.Escaper.Unescape(text);
						}
						settingsPropertyValue.SerializedValue = text;
					}
					else if (settingsProperty.DefaultValue != null)
					{
						settingsPropertyValue.SerializedValue = settingsProperty.DefaultValue;
					}
					else
					{
						settingsPropertyValue.PropertyValue = null;
					}
					settingsPropertyValue.IsDirty = false;
					settingsPropertyValueCollection.Add(settingsPropertyValue);
				}
			}
			return settingsPropertyValueCollection;
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000EA288 File Offset: 0x000E9288
		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
		{
			string sectionName = this.GetSectionName(context);
			IDictionary dictionary = new Hashtable();
			IDictionary dictionary2 = new Hashtable();
			foreach (object obj in values)
			{
				SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj;
				SettingsProperty property = settingsPropertyValue.Property;
				bool flag = this.IsUserSetting(property);
				if (settingsPropertyValue.IsDirty && flag)
				{
					bool flag2 = LocalFileSettingsProvider.IsRoamingSetting(property);
					StoredSetting storedSetting = new StoredSetting(property.SerializeAs, this.SerializeToXmlElement(property, settingsPropertyValue));
					if (flag2)
					{
						dictionary[property.Name] = storedSetting;
					}
					else
					{
						dictionary2[property.Name] = storedSetting;
					}
					settingsPropertyValue.IsDirty = false;
				}
			}
			if (dictionary.Count > 0)
			{
				this.Store.WriteSettings(sectionName, true, dictionary);
			}
			if (dictionary2.Count > 0)
			{
				this.Store.WriteSettings(sectionName, false, dictionary2);
			}
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x000EA398 File Offset: 0x000E9398
		public void Reset(SettingsContext context)
		{
			string sectionName = this.GetSectionName(context);
			this.Store.RevertToParent(sectionName, true);
			this.Store.RevertToParent(sectionName, false);
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000EA3C8 File Offset: 0x000E93C8
		public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
		{
			SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
			SettingsPropertyCollection settingsPropertyCollection2 = new SettingsPropertyCollection();
			foreach (object obj in properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				bool flag = LocalFileSettingsProvider.IsRoamingSetting(settingsProperty);
				if (flag)
				{
					settingsPropertyCollection2.Add(settingsProperty);
				}
				else
				{
					settingsPropertyCollection.Add(settingsProperty);
				}
			}
			if (settingsPropertyCollection2.Count > 0)
			{
				this.Upgrade(context, settingsPropertyCollection2, true);
			}
			if (settingsPropertyCollection.Count > 0)
			{
				this.Upgrade(context, settingsPropertyCollection, false);
			}
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000EA468 File Offset: 0x000E9468
		private Version CreateVersion(string name)
		{
			Version result = null;
			try
			{
				result = new Version(name);
			}
			catch (ArgumentException)
			{
				result = null;
			}
			catch (OverflowException)
			{
				result = null;
			}
			catch (FormatException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000EA4B8 File Offset: 0x000E94B8
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[FileIOPermission(SecurityAction.Assert, AllFiles = (FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery))]
		public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
		{
			bool isRoaming = LocalFileSettingsProvider.IsRoamingSetting(property);
			string previousConfigFileName = this.GetPreviousConfigFileName(isRoaming);
			if (!string.IsNullOrEmpty(previousConfigFileName))
			{
				SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
				settingsPropertyCollection.Add(property);
				SettingsPropertyValueCollection settingValuesFromFile = this.GetSettingValuesFromFile(previousConfigFileName, this.GetSectionName(context), true, settingsPropertyCollection);
				return settingValuesFromFile[property.Name];
			}
			return new SettingsPropertyValue(property)
			{
				PropertyValue = null
			};
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000EA51C File Offset: 0x000E951C
		private string GetPreviousConfigFileName(bool isRoaming)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			string text = isRoaming ? this._prevRoamingConfigFileName : this._prevLocalConfigFileName;
			if (string.IsNullOrEmpty(text))
			{
				string path = isRoaming ? ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigDirectory : ConfigurationManagerInternalFactory.Instance.ExeLocalConfigDirectory;
				Version version = this.CreateVersion(ConfigurationManagerInternalFactory.Instance.ExeProductVersion);
				Version version2 = null;
				DirectoryInfo directoryInfo = null;
				string text2 = null;
				if (version == null)
				{
					return null;
				}
				DirectoryInfo parent = Directory.GetParent(path);
				if (parent.Exists)
				{
					foreach (DirectoryInfo directoryInfo2 in parent.GetDirectories())
					{
						Version version3 = this.CreateVersion(directoryInfo2.Name);
						if (version3 != null && version3 < version)
						{
							if (version2 == null)
							{
								version2 = version3;
								directoryInfo = directoryInfo2;
							}
							else if (version3 > version2)
							{
								version2 = version3;
								directoryInfo = directoryInfo2;
							}
						}
					}
					if (directoryInfo != null)
					{
						text2 = Path.Combine(directoryInfo.FullName, ConfigurationManagerInternalFactory.Instance.UserConfigFilename);
					}
					if (File.Exists(text2))
					{
						text = text2;
					}
				}
				if (isRoaming)
				{
					this._prevRoamingConfigFileName = text;
				}
				else
				{
					this._prevLocalConfigFileName = text;
				}
			}
			return text;
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000EA65C File Offset: 0x000E965C
		private string GetSectionName(SettingsContext context)
		{
			string text = (string)context["GroupName"];
			string text2 = (string)context["SettingsKey"];
			string text3 = text;
			if (!string.IsNullOrEmpty(text2))
			{
				text3 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
				{
					text3,
					text2
				});
			}
			return XmlConvert.EncodeLocalName(text3);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000EA6BC File Offset: 0x000E96BC
		private SettingsPropertyValueCollection GetSettingValuesFromFile(string configFileName, string sectionName, bool userScoped, SettingsPropertyCollection properties)
		{
			SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
			IDictionary dictionary = ClientSettingsStore.ReadSettingsFromFile(configFileName, sectionName, userScoped);
			foreach (object obj in properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				string name = settingsProperty.Name;
				SettingsPropertyValue settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
				if (dictionary.Contains(name))
				{
					StoredSetting storedSetting = (StoredSetting)dictionary[name];
					string text = storedSetting.Value.InnerXml;
					if (storedSetting.SerializeAs == SettingsSerializeAs.String)
					{
						text = this.Escaper.Unescape(text);
					}
					settingsPropertyValue.SerializedValue = text;
					settingsPropertyValue.IsDirty = true;
					settingsPropertyValueCollection.Add(settingsPropertyValue);
				}
			}
			return settingsPropertyValueCollection;
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000EA788 File Offset: 0x000E9788
		private static bool IsRoamingSetting(SettingsProperty setting)
		{
			bool flag = !ApplicationSettingsBase.IsClickOnceDeployed(AppDomain.CurrentDomain);
			bool result = false;
			if (flag)
			{
				SettingsManageabilityAttribute settingsManageabilityAttribute = setting.Attributes[typeof(SettingsManageabilityAttribute)] as SettingsManageabilityAttribute;
				bool flag2;
				if (settingsManageabilityAttribute != null)
				{
					SettingsManageability manageability = settingsManageabilityAttribute.Manageability;
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000EA7D4 File Offset: 0x000E97D4
		private bool IsUserSetting(SettingsProperty setting)
		{
			bool flag = setting.Attributes[typeof(UserScopedSettingAttribute)] is UserScopedSettingAttribute;
			bool flag2 = setting.Attributes[typeof(ApplicationScopedSettingAttribute)] is ApplicationScopedSettingAttribute;
			if (flag && flag2)
			{
				throw new ConfigurationErrorsException(SR.GetString("BothScopeAttributes"));
			}
			if (!flag && !flag2)
			{
				throw new ConfigurationErrorsException(SR.GetString("NoScopeAttributes"));
			}
			return flag;
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000EA84C File Offset: 0x000E984C
		private XmlNode SerializeToXmlElement(SettingsProperty setting, SettingsPropertyValue value)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("value");
			string text = value.SerializedValue as string;
			if (text == null && setting.SerializeAs == SettingsSerializeAs.Binary)
			{
				byte[] array = value.SerializedValue as byte[];
				if (array != null)
				{
					text = Convert.ToBase64String(array);
				}
			}
			if (text == null)
			{
				text = string.Empty;
			}
			if (setting.SerializeAs == SettingsSerializeAs.String)
			{
				text = this.Escaper.Escape(text);
			}
			xmlElement.InnerXml = text;
			XmlNode xmlNode = null;
			foreach (object obj in xmlElement.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.NodeType == XmlNodeType.XmlDeclaration)
				{
					xmlNode = xmlNode2;
					break;
				}
			}
			if (xmlNode != null)
			{
				xmlElement.RemoveChild(xmlNode);
			}
			return xmlElement;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000EA930 File Offset: 0x000E9930
		[FileIOPermission(SecurityAction.Assert, AllFiles = (FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery))]
		private void Upgrade(SettingsContext context, SettingsPropertyCollection properties, bool isRoaming)
		{
			string previousConfigFileName = this.GetPreviousConfigFileName(isRoaming);
			if (!string.IsNullOrEmpty(previousConfigFileName))
			{
				SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
				foreach (object obj in properties)
				{
					SettingsProperty settingsProperty = (SettingsProperty)obj;
					if (!(settingsProperty.Attributes[typeof(NoSettingsVersionUpgradeAttribute)] is NoSettingsVersionUpgradeAttribute))
					{
						settingsPropertyCollection.Add(settingsProperty);
					}
				}
				SettingsPropertyValueCollection settingValuesFromFile = this.GetSettingValuesFromFile(previousConfigFileName, this.GetSectionName(context), true, settingsPropertyCollection);
				this.SetPropertyValues(context, settingValuesFromFile);
			}
		}

		// Token: 0x040031A3 RID: 12707
		private string _appName = string.Empty;

		// Token: 0x040031A4 RID: 12708
		private ClientSettingsStore _store;

		// Token: 0x040031A5 RID: 12709
		private string _prevLocalConfigFileName;

		// Token: 0x040031A6 RID: 12710
		private string _prevRoamingConfigFileName;

		// Token: 0x040031A7 RID: 12711
		private LocalFileSettingsProvider.XmlEscaper _escaper;

		// Token: 0x020006FD RID: 1789
		private class XmlEscaper
		{
			// Token: 0x06003723 RID: 14115 RVA: 0x000EA9EB File Offset: 0x000E99EB
			internal XmlEscaper()
			{
				this.doc = new XmlDocument();
				this.temp = this.doc.CreateElement("temp");
			}

			// Token: 0x06003724 RID: 14116 RVA: 0x000EAA14 File Offset: 0x000E9A14
			internal string Escape(string xmlString)
			{
				if (string.IsNullOrEmpty(xmlString))
				{
					return xmlString;
				}
				this.temp.InnerText = xmlString;
				return this.temp.InnerXml;
			}

			// Token: 0x06003725 RID: 14117 RVA: 0x000EAA37 File Offset: 0x000E9A37
			internal string Unescape(string escapedString)
			{
				if (string.IsNullOrEmpty(escapedString))
				{
					return escapedString;
				}
				this.temp.InnerXml = escapedString;
				return this.temp.InnerText;
			}

			// Token: 0x040031A8 RID: 12712
			private XmlDocument doc;

			// Token: 0x040031A9 RID: 12713
			private XmlElement temp;
		}
	}
}
