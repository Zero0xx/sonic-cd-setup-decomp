using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F0 RID: 1776
	internal sealed class ConfigXmlElement : XmlElement, IConfigErrorInfo
	{
		// Token: 0x060036D7 RID: 14039 RVA: 0x000E9A13 File Offset: 0x000E8A13
		public ConfigXmlElement(string filename, int line, string prefix, string localName, string namespaceUri, XmlDocument doc) : base(prefix, localName, namespaceUri, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060036D8 RID: 14040 RVA: 0x000E9A30 File Offset: 0x000E8A30
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060036D9 RID: 14041 RVA: 0x000E9A38 File Offset: 0x000E8A38
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x000E9A40 File Offset: 0x000E8A40
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlElement configXmlElement = xmlNode as ConfigXmlElement;
			if (configXmlElement != null)
			{
				configXmlElement._line = this._line;
				configXmlElement._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x0400319B RID: 12699
		private int _line;

		// Token: 0x0400319C RID: 12700
		private string _filename;
	}
}
