using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006EC RID: 1772
	internal sealed class ConfigXmlAttribute : XmlAttribute, IConfigErrorInfo
	{
		// Token: 0x060036BD RID: 14013 RVA: 0x000E9710 File Offset: 0x000E8710
		public ConfigXmlAttribute(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc) : base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x000E972D File Offset: 0x000E872D
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x060036BF RID: 14015 RVA: 0x000E9735 File Offset: 0x000E8735
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000E9740 File Offset: 0x000E8740
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlAttribute configXmlAttribute = xmlNode as ConfigXmlAttribute;
			if (configXmlAttribute != null)
			{
				configXmlAttribute._line = this._line;
				configXmlAttribute._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04003192 RID: 12690
		private int _line;

		// Token: 0x04003193 RID: 12691
		private string _filename;
	}
}
