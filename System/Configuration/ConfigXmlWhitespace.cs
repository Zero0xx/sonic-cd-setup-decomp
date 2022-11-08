using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F3 RID: 1779
	internal sealed class ConfigXmlWhitespace : XmlWhitespace, IConfigErrorInfo
	{
		// Token: 0x060036E3 RID: 14051 RVA: 0x000E9B40 File Offset: 0x000E8B40
		public ConfigXmlWhitespace(string filename, int line, string comment, XmlDocument doc) : base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x060036E4 RID: 14052 RVA: 0x000E9B59 File Offset: 0x000E8B59
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x060036E5 RID: 14053 RVA: 0x000E9B61 File Offset: 0x000E8B61
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000E9B6C File Offset: 0x000E8B6C
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlWhitespace configXmlWhitespace = xmlNode as ConfigXmlWhitespace;
			if (configXmlWhitespace != null)
			{
				configXmlWhitespace._line = this._line;
				configXmlWhitespace._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x040031A1 RID: 12705
		private int _line;

		// Token: 0x040031A2 RID: 12706
		private string _filename;
	}
}
