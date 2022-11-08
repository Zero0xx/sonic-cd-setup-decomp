using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006EE RID: 1774
	internal sealed class ConfigXmlComment : XmlComment, IConfigErrorInfo
	{
		// Token: 0x060036C5 RID: 14021 RVA: 0x000E97DC File Offset: 0x000E87DC
		public ConfigXmlComment(string filename, int line, string comment, XmlDocument doc) : base(comment, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x060036C6 RID: 14022 RVA: 0x000E97F5 File Offset: 0x000E87F5
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x060036C7 RID: 14023 RVA: 0x000E97FD File Offset: 0x000E87FD
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x000E9808 File Offset: 0x000E8808
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlComment configXmlComment = xmlNode as ConfigXmlComment;
			if (configXmlComment != null)
			{
				configXmlComment._line = this._line;
				configXmlComment._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04003196 RID: 12694
		private int _line;

		// Token: 0x04003197 RID: 12695
		private string _filename;
	}
}
