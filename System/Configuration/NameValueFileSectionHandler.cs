using System;
using System.Configuration.Internal;
using System.IO;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006FE RID: 1790
	public class NameValueFileSectionHandler : IConfigurationSectionHandler
	{
		// Token: 0x06003726 RID: 14118 RVA: 0x000EAA5C File Offset: 0x000E9A5C
		public object Create(object parent, object configContext, XmlNode section)
		{
			object obj = parent;
			XmlNode xmlNode = section.Attributes.RemoveNamedItem("file");
			obj = NameValueSectionHandler.CreateStatic(obj, section);
			if (xmlNode != null && xmlNode.Value.Length != 0)
			{
				string value = xmlNode.Value;
				IConfigErrorInfo configErrorInfo = xmlNode as IConfigErrorInfo;
				if (configErrorInfo == null)
				{
					return null;
				}
				string filename = configErrorInfo.Filename;
				string directoryName = Path.GetDirectoryName(filename);
				string text = Path.Combine(directoryName, value);
				if (File.Exists(text))
				{
					ConfigXmlDocument configXmlDocument = new ConfigXmlDocument();
					try
					{
						configXmlDocument.Load(text);
					}
					catch (XmlException ex)
					{
						throw new ConfigurationErrorsException(ex.Message, ex, text, ex.LineNumber);
					}
					if (section.Name != configXmlDocument.DocumentElement.Name)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_name_value_file_section_file_invalid_root", new object[]
						{
							section.Name
						}), configXmlDocument.DocumentElement);
					}
					obj = NameValueSectionHandler.CreateStatic(obj, configXmlDocument.DocumentElement);
				}
			}
			return obj;
		}
	}
}
