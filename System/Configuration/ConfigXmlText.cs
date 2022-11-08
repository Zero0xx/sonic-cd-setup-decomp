using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F2 RID: 1778
	internal sealed class ConfigXmlText : XmlText, IConfigErrorInfo
	{
		// Token: 0x060036DF RID: 14047 RVA: 0x000E9ADC File Offset: 0x000E8ADC
		public ConfigXmlText(string filename, int line, string strData, XmlDocument doc) : base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060036E0 RID: 14048 RVA: 0x000E9AF5 File Offset: 0x000E8AF5
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x060036E1 RID: 14049 RVA: 0x000E9AFD File Offset: 0x000E8AFD
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x000E9B08 File Offset: 0x000E8B08
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlText configXmlText = xmlNode as ConfigXmlText;
			if (configXmlText != null)
			{
				configXmlText._line = this._line;
				configXmlText._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x0400319F RID: 12703
		private int _line;

		// Token: 0x040031A0 RID: 12704
		private string _filename;
	}
}
