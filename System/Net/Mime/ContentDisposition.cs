using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000684 RID: 1668
	public class ContentDisposition
	{
		// Token: 0x0600339F RID: 13215 RVA: 0x000DA090 File Offset: 0x000D9090
		public ContentDisposition()
		{
			this.isChanged = true;
			this.disposition = "attachment";
			this.ParseValue();
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x000DA0B0 File Offset: 0x000D90B0
		public ContentDisposition(string disposition)
		{
			if (disposition == null)
			{
				throw new ArgumentNullException("disposition");
			}
			this.isChanged = true;
			this.disposition = disposition;
			this.ParseValue();
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x000DA0DA File Offset: 0x000D90DA
		// (set) Token: 0x060033A2 RID: 13218 RVA: 0x000DA0E2 File Offset: 0x000D90E2
		public string DispositionType
		{
			get
			{
				return this.dispositionType;
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
				this.isChanged = true;
				this.dispositionType = value;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x060033A3 RID: 13219 RVA: 0x000DA122 File Offset: 0x000D9122
		public StringDictionary Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					this.parameters = new TrackingStringDictionary();
				}
				return this.parameters;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x060033A4 RID: 13220 RVA: 0x000DA13D File Offset: 0x000D913D
		// (set) Token: 0x060033A5 RID: 13221 RVA: 0x000DA14F File Offset: 0x000D914F
		public string FileName
		{
			get
			{
				return this.Parameters["filename"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("filename");
					return;
				}
				this.Parameters["filename"] = value;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x060033A6 RID: 13222 RVA: 0x000DA184 File Offset: 0x000D9184
		// (set) Token: 0x060033A7 RID: 13223 RVA: 0x000DA1B5 File Offset: 0x000D91B5
		public DateTime CreationDate
		{
			get
			{
				string text = this.Parameters["creation-date"];
				if (text == null)
				{
					return DateTime.MinValue;
				}
				int num = 0;
				return MailBnfHelper.ReadDateTime(text, ref num);
			}
			set
			{
				this.Parameters["creation-date"] = MailBnfHelper.GetDateTimeString(value, null);
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x000DA1D0 File Offset: 0x000D91D0
		// (set) Token: 0x060033A9 RID: 13225 RVA: 0x000DA201 File Offset: 0x000D9201
		public DateTime ModificationDate
		{
			get
			{
				string text = this.Parameters["modification-date"];
				if (text == null)
				{
					return DateTime.MinValue;
				}
				int num = 0;
				return MailBnfHelper.ReadDateTime(text, ref num);
			}
			set
			{
				this.Parameters["modification-date"] = MailBnfHelper.GetDateTimeString(value, null);
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x060033AA RID: 13226 RVA: 0x000DA21A File Offset: 0x000D921A
		// (set) Token: 0x060033AB RID: 13227 RVA: 0x000DA22C File Offset: 0x000D922C
		public bool Inline
		{
			get
			{
				return this.dispositionType == "inline";
			}
			set
			{
				this.isChanged = true;
				if (value)
				{
					this.dispositionType = "inline";
					return;
				}
				this.dispositionType = "attachment";
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x060033AC RID: 13228 RVA: 0x000DA250 File Offset: 0x000D9250
		// (set) Token: 0x060033AD RID: 13229 RVA: 0x000DA281 File Offset: 0x000D9281
		public DateTime ReadDate
		{
			get
			{
				string text = this.Parameters["read-date"];
				if (text == null)
				{
					return DateTime.MinValue;
				}
				int num = 0;
				return MailBnfHelper.ReadDateTime(text, ref num);
			}
			set
			{
				this.Parameters["read-date"] = MailBnfHelper.GetDateTimeString(value, null);
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x060033AE RID: 13230 RVA: 0x000DA29C File Offset: 0x000D929C
		// (set) Token: 0x060033AF RID: 13231 RVA: 0x000DA2CB File Offset: 0x000D92CB
		public long Size
		{
			get
			{
				string text = this.Parameters["size"];
				if (text == null)
				{
					return -1L;
				}
				return long.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				this.Parameters["size"] = value.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000DA2E9 File Offset: 0x000D92E9
		internal void Set(string contentDisposition, HeaderCollection headers)
		{
			this.disposition = contentDisposition;
			this.ParseValue();
			headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
			this.isPersisted = true;
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000DA311 File Offset: 0x000D9311
		internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
		{
			if (this.IsChanged || !this.isPersisted || forcePersist)
			{
				headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
				this.isPersisted = true;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x000DA33F File Offset: 0x000D933F
		internal bool IsChanged
		{
			get
			{
				return this.isChanged || (this.parameters != null && this.parameters.IsChanged);
			}
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x000DA360 File Offset: 0x000D9360
		public override string ToString()
		{
			if (this.disposition == null || this.isChanged || (this.parameters != null && this.parameters.IsChanged))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.dispositionType);
				foreach (object obj in this.Parameters.Keys)
				{
					string text = (string)obj;
					stringBuilder.Append("; ");
					stringBuilder.Append(text);
					stringBuilder.Append('=');
					MailBnfHelper.GetTokenOrQuotedString(this.parameters[text], stringBuilder);
				}
				this.disposition = stringBuilder.ToString();
				this.isChanged = false;
				this.parameters.IsChanged = false;
				this.isPersisted = false;
			}
			return this.disposition;
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x000DA454 File Offset: 0x000D9454
		public override bool Equals(object rparam)
		{
			return rparam != null && string.Compare(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000DA470 File Offset: 0x000D9470
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000DA480 File Offset: 0x000D9480
		private void ParseValue()
		{
			int index = 0;
			this.parameters = new TrackingStringDictionary();
			Exception ex = null;
			try
			{
				this.dispositionType = MailBnfHelper.ReadToken(this.disposition, ref index, null);
				if (this.dispositionType == null || this.dispositionType.Length == 0)
				{
					ex = new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
				}
				if (ex == null)
				{
					while (MailBnfHelper.SkipCFWS(this.disposition, ref index))
					{
						if (this.disposition[index++] != ';')
						{
							ex = new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
						}
						if (!MailBnfHelper.SkipCFWS(this.disposition, ref index))
						{
							break;
						}
						string text = MailBnfHelper.ReadParameterAttribute(this.disposition, ref index, null);
						if (this.disposition[index++] != '=')
						{
							ex = new FormatException(SR.GetString("MailHeaderFieldMalformedHeader"));
							break;
						}
						string text2;
						if (!MailBnfHelper.SkipCFWS(this.disposition, ref index))
						{
							text2 = string.Empty;
						}
						else if (this.disposition[index] == '"')
						{
							text2 = MailBnfHelper.ReadQuotedString(this.disposition, ref index, null);
						}
						else
						{
							text2 = MailBnfHelper.ReadToken(this.disposition, ref index, null);
						}
						if (text == null || text2 == null || text.Length == 0 || text2.Length == 0)
						{
							ex = new FormatException(SR.GetString("ContentDispositionInvalid"));
							break;
						}
						if (string.Compare(text, "creation-date", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "modification-date", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "read-date", StringComparison.OrdinalIgnoreCase) == 0)
						{
							int num = 0;
							MailBnfHelper.ReadDateTime(text2, ref num);
						}
						this.parameters.Add(text, text2);
					}
				}
			}
			catch (FormatException)
			{
				throw new FormatException(SR.GetString("ContentDispositionInvalid"));
			}
			if (ex != null)
			{
				throw ex;
			}
			this.parameters.IsChanged = false;
		}

		// Token: 0x04002FA4 RID: 12196
		private string dispositionType;

		// Token: 0x04002FA5 RID: 12197
		private TrackingStringDictionary parameters;

		// Token: 0x04002FA6 RID: 12198
		private bool isChanged;

		// Token: 0x04002FA7 RID: 12199
		private bool isPersisted;

		// Token: 0x04002FA8 RID: 12200
		private string disposition;
	}
}
