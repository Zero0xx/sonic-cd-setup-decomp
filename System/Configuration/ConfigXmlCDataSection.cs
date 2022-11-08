using System;
using System.Configuration.Internal;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006ED RID: 1773
	internal sealed class ConfigXmlCDataSection : XmlCDataSection, IConfigErrorInfo
	{
		// Token: 0x060036C1 RID: 14017 RVA: 0x000E9778 File Offset: 0x000E8778
		public ConfigXmlCDataSection(string filename, int line, string data, XmlDocument doc) : base(data, doc)
		{
			this._line = line;
			this._filename = filename;
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x060036C2 RID: 14018 RVA: 0x000E9791 File Offset: 0x000E8791
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x060036C3 RID: 14019 RVA: 0x000E9799 File Offset: 0x000E8799
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000E97A4 File Offset: 0x000E87A4
		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlCDataSection configXmlCDataSection = xmlNode as ConfigXmlCDataSection;
			if (configXmlCDataSection != null)
			{
				configXmlCDataSection._line = this._line;
				configXmlCDataSection._filename = this._filename;
			}
			return xmlNode;
		}

		// Token: 0x04003194 RID: 12692
		private int _line;

		// Token: 0x04003195 RID: 12693
		private string _filename;
	}
}
