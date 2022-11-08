using System;
using System.Configuration.Internal;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006EF RID: 1775
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ConfigXmlDocument : XmlDocument, IConfigErrorInfo
	{
		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x060036C9 RID: 14025 RVA: 0x000E9840 File Offset: 0x000E8840
		int IConfigErrorInfo.LineNumber
		{
			get
			{
				if (this._reader == null)
				{
					return 0;
				}
				if (this._lineOffset > 0)
				{
					return this._reader.LineNumber + this._lineOffset - 1;
				}
				return this._reader.LineNumber;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060036CA RID: 14026 RVA: 0x000E9875 File Offset: 0x000E8875
		public int LineNumber
		{
			get
			{
				return ((IConfigErrorInfo)this).LineNumber;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060036CB RID: 14027 RVA: 0x000E987D File Offset: 0x000E887D
		public string Filename
		{
			get
			{
				return ConfigurationException.SafeFilename(this._filename);
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060036CC RID: 14028 RVA: 0x000E988A File Offset: 0x000E888A
		string IConfigErrorInfo.Filename
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000E9894 File Offset: 0x000E8894
		public override void Load(string filename)
		{
			this._filename = filename;
			try
			{
				this._reader = new XmlTextReader(filename);
				this._reader.XmlResolver = null;
				base.Load(this._reader);
			}
			finally
			{
				if (this._reader != null)
				{
					this._reader.Close();
					this._reader = null;
				}
			}
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x000E98FC File Offset: 0x000E88FC
		public void LoadSingleElement(string filename, XmlTextReader sourceReader)
		{
			this._filename = filename;
			this._lineOffset = sourceReader.LineNumber;
			string s = sourceReader.ReadOuterXml();
			try
			{
				this._reader = new XmlTextReader(new StringReader(s), sourceReader.NameTable);
				base.Load(this._reader);
			}
			finally
			{
				if (this._reader != null)
				{
					this._reader.Close();
					this._reader = null;
				}
			}
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x000E9974 File Offset: 0x000E8974
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlAttribute(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x000E998B File Offset: 0x000E898B
		public override XmlElement CreateElement(string prefix, string localName, string namespaceUri)
		{
			return new ConfigXmlElement(this._filename, this.LineNumber, prefix, localName, namespaceUri, this);
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x000E99A2 File Offset: 0x000E89A2
		public override XmlText CreateTextNode(string text)
		{
			return new ConfigXmlText(this._filename, this.LineNumber, text, this);
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x000E99B7 File Offset: 0x000E89B7
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new ConfigXmlCDataSection(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x000E99CC File Offset: 0x000E89CC
		public override XmlComment CreateComment(string data)
		{
			return new ConfigXmlComment(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x000E99E1 File Offset: 0x000E89E1
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string data)
		{
			return new ConfigXmlSignificantWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x000E99F6 File Offset: 0x000E89F6
		public override XmlWhitespace CreateWhitespace(string data)
		{
			return new ConfigXmlWhitespace(this._filename, this.LineNumber, data, this);
		}

		// Token: 0x04003198 RID: 12696
		private XmlTextReader _reader;

		// Token: 0x04003199 RID: 12697
		private int _lineOffset;

		// Token: 0x0400319A RID: 12698
		private string _filename;
	}
}
