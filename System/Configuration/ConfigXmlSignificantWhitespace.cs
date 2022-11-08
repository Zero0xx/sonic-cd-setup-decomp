using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F1 RID: 1777
	internal sealed class ConfigXmlSignificantWhitespace : XmlSignificantWhitespace, IConfigErrorInfo
	{
		// Token: 0x060036DB RID: 14043 RVA: 0x000E9A78 File Offset: 0x000E8A78
		public ConfigXmlSignificantWhitespace(string filename, int line, string strData, XmlDocument doc) : base(strData, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x060036DC RID: 14044 RVA: 0x000E9A91 File Offset: 0x000E8A91
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060036DD RID: 14045 RVA: 0x000E9A99 File Offset: 0x000E8A99
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000E9AA4 File Offset: 0x000E8AA4
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlSignificantWhitespace configXmlSignificantWhitespace = xmlNode as ConfigXmlSignificantWhitespace;
			if (configXmlSignificantWhitespace != null)
			{
				configXmlSignificantWhitespace._line = this._line;
				configXmlSignificantWhitespace._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x0400319D RID: 12701
		private int _line;

		// Token: 0x0400319E RID: 12702
		private string _filename;
	}
}
