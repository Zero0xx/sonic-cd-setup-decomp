using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000686 RID: 1670
	public class ContentType
	{
		// Token: 0x060033B7 RID: 13239 RVA: 0x000DA654 File Offset: 0x000D9654
		public ContentType() : this(ContentType.Default)
		{
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x000DA664 File Offset: 0x000D9664
		public ContentType(string contentType)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			if (contentType == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"contentType"
				}), "contentType");
			}
			this.isChanged = true;
			this.type = contentType;
			this.ParseValue();
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x060033B9 RID: 13241 RVA: 0x000DA6CB File Offset: 0x000D96CB
		// (set) Token: 0x060033BA RID: 13242 RVA: 0x000DA6DD File Offset: 0x000D96DD
		public string Boundary
		{
			get
			{
				return this.Parameters["boundary"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("boundary");
					return;
				}
				this.Parameters["boundary"] = value;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x060033BB RID: 13243 RVA: 0x000DA711 File Offset: 0x000D9711
		// (set) Token: 0x060033BC RID: 13244 RVA: 0x000DA723 File Offset: 0x000D9723
		public string CharSet
		{
			get
			{
				return this.Parameters["charset"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("charset");
					return;
				}
				this.Parameters["charset"] = value;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x000DA757 File Offset: 0x000D9757
		// (set) Token: 0x060033BE RID: 13246 RVA: 0x000DA770 File Offset: 0x000D9770
		public string MediaType
		{
			get
			{
				return this.mediaType + "/" + this.subType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(SR.GetString("net_emptystringset"), "value");
				}
				int num = 0;
				this.mediaType = MailBnfHelper.ReadToken(value, ref num, null);
				if (this.mediaType.Length == 0 || num >= value.Length || value[num++] != '/')
				{
					throw new FormatException(SR.GetString("MediaTypeInvalid"));
				}
				this.subType = MailBnfHelper.ReadToken(value, ref num, null);
				if (this.subType.Length == 0 || num < value.Length)
				{
					throw new FormatException(SR.GetString("MediaTypeInvalid"));
				}
				this.isChanged = true;
				this.isPersisted = false;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x060033BF RID: 13247 RVA: 0x000DA838 File Offset: 0x000D9838
		// (set) Token: 0x060033C0 RID: 13248 RVA: 0x000DA868 File Offset: 0x000D9868
		public string Name
		{
			get
			{
				string text = this.Parameters["name"];
				Encoding encoding = MimeBasePart.DecodeEncoding(text);
				if (encoding != null)
				{
					text = MimeBasePart.DecodeHeaderValue(text);
				}
				return text;
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("name");
					return;
				}
				if (MimeBasePart.IsAscii(value, false))
				{
					this.Parameters["name"] = value;
					return;
				}
				Encoding encoding = Encoding.GetEncoding("utf-8");
				this.Parameters["name"] = MimeBasePart.EncodeHeaderValue(value, encoding, MimeBasePart.ShouldUseBase64Encoding(encoding));
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060033C1 RID: 13249 RVA: 0x000DA8D9 File Offset: 0x000D98D9
		public StringDictionary Parameters
		{
			get
			{
				if (this.parameters == null && this.type == null)
				{
					this.parameters = new TrackingStringDictionary();
				}
				return this.parameters;
			}
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000DA8FC File Offset: 0x000D98FC
		internal void Set(string contentType, HeaderCollection headers)
		{
			this.type = contentType;
			this.ParseValue();
			headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentType), this.ToString());
			this.isPersisted = true;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000DA924 File Offset: 0x000D9924
		internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
		{
			if (this.IsChanged || !this.isPersisted || forcePersist)
			{
				headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentType), this.ToString());
				this.isPersisted = true;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x000DA952 File Offset: 0x000D9952
		internal bool IsChanged
		{
			get
			{
				return this.isChanged || (this.parameters != null && this.parameters.IsChanged);
			}
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x000DA974 File Offset: 0x000D9974
		public override string ToString()
		{
			if (this.type == null || this.IsChanged)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.mediaType);
				stringBuilder.Append('/');
				stringBuilder.Append(this.subType);
				foreach (object obj in this.Parameters.Keys)
				{
					string text = (string)obj;
					stringBuilder.Append("; ");
					stringBuilder.Append(text);
					stringBuilder.Append('=');
					MailBnfHelper.GetTokenOrQuotedString(this.parameters[text], stringBuilder);
				}
				this.type = stringBuilder.ToString();
				this.isChanged = false;
				this.parameters.IsChanged = false;
				this.isPersisted = false;
			}
			return this.type;
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000DAA64 File Offset: 0x000D9A64
		public override bool Equals(object rparam)
		{
			return rparam != null && string.Compare(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000DAA80 File Offset: 0x000D9A80
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x000DAA90 File Offset: 0x000D9A90
		private void ParseValue()
		{
			int num = 0;
			Exception ex = null;
			this.parameters = new TrackingStringDictionary();
			try
			{
				this.mediaType = MailBnfHelper.ReadToken(this.type, ref num, null);
				if (this.mediaType == null || this.mediaType.Length == 0 || num >= this.type.Length || this.type[num++] != '/')
				{
					ex = new FormatException(SR.GetString("ContentTypeInvalid"));
				}
				if (ex == null)
				{
					this.subType = MailBnfHelper.ReadToken(this.type, ref num, null);
					if (this.subType == null || this.subType.Length == 0)
					{
						ex = new FormatException(SR.GetString("ContentTypeInvalid"));
					}
				}
				if (ex == null)
				{
					while (MailBnfHelper.SkipCFWS(this.type, ref num))
					{
						if (this.type[num++] != ';')
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						if (!MailBnfHelper.SkipCFWS(this.type, ref num))
						{
							break;
						}
						string text = MailBnfHelper.ReadParameterAttribute(this.type, ref num, null);
						if (text == null || text.Length == 0)
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						if (num >= this.type.Length || this.type[num++] != '=')
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						if (!MailBnfHelper.SkipCFWS(this.type, ref num))
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						string text2;
						if (this.type[num] == '"')
						{
							text2 = MailBnfHelper.ReadQuotedString(this.type, ref num, null);
						}
						else
						{
							text2 = MailBnfHelper.ReadToken(this.type, ref num, null);
						}
						if (text2 == null)
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						this.parameters.Add(text, text2);
					}
				}
				this.parameters.IsChanged = false;
			}
			catch (FormatException)
			{
				throw new FormatException(SR.GetString("ContentTypeInvalid"));
			}
			if (ex != null)
			{
				throw new FormatException(SR.GetString("ContentTypeInvalid"));
			}
		}

		// Token: 0x04002FB1 RID: 12209
		private string mediaType;

		// Token: 0x04002FB2 RID: 12210
		private string subType;

		// Token: 0x04002FB3 RID: 12211
		private bool isChanged;

		// Token: 0x04002FB4 RID: 12212
		private string type;

		// Token: 0x04002FB5 RID: 12213
		private bool isPersisted;

		// Token: 0x04002FB6 RID: 12214
		private TrackingStringDictionary parameters;

		// Token: 0x04002FB7 RID: 12215
		internal static readonly string Default = "application/octet-stream";
	}
}
