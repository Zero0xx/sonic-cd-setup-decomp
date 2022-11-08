using System;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000697 RID: 1687
	public class MailAddress
	{
		// Token: 0x06003409 RID: 13321 RVA: 0x000DB721 File Offset: 0x000DA721
		internal MailAddress(string address, string encodedDisplayName, uint bogusParam)
		{
			this.encodedDisplayName = encodedDisplayName;
			this.GetParts(address);
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x000DB737 File Offset: 0x000DA737
		public MailAddress(string address) : this(address, null, null)
		{
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x000DB742 File Offset: 0x000DA742
		public MailAddress(string address, string displayName) : this(address, displayName, null)
		{
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000DB750 File Offset: 0x000DA750
		public MailAddress(string address, string displayName, Encoding displayNameEncoding)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"address"
				}), "address");
			}
			this.displayNameEncoding = displayNameEncoding;
			this.displayName = displayName;
			this.ParseValue(address);
			if (this.displayName != null && this.displayName != string.Empty)
			{
				if (this.displayName[0] == '"' && this.displayName[this.displayName.Length - 1] == '"')
				{
					this.displayName = this.displayName.Substring(1, this.displayName.Length - 2);
				}
				this.displayName = this.displayName.Trim();
			}
			if (this.displayName != null && this.displayName.Length > 0)
			{
				if (!MimeBasePart.IsAscii(this.displayName, false) || this.displayNameEncoding != null)
				{
					if (this.displayNameEncoding == null)
					{
						this.displayNameEncoding = Encoding.GetEncoding("utf-8");
					}
					this.encodedDisplayName = MimeBasePart.EncodeHeaderValue(this.displayName, this.displayNameEncoding, MimeBasePart.ShouldUseBase64Encoding(displayNameEncoding));
					StringBuilder stringBuilder = new StringBuilder();
					int num = 0;
					MailBnfHelper.ReadUnQuotedString(this.encodedDisplayName, ref num, stringBuilder);
					this.encodedDisplayName = stringBuilder.ToString();
					return;
				}
				this.encodedDisplayName = this.displayName;
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x000DB8C4 File Offset: 0x000DA8C4
		public string DisplayName
		{
			get
			{
				if (this.displayName == null)
				{
					if (this.encodedDisplayName != null && this.encodedDisplayName.Length > 0)
					{
						this.displayName = MimeBasePart.DecodeHeaderValue(this.encodedDisplayName);
					}
					else
					{
						this.displayName = string.Empty;
					}
				}
				return this.displayName;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x0600340E RID: 13326 RVA: 0x000DB913 File Offset: 0x000DA913
		public string User
		{
			get
			{
				return this.userName;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x000DB91B File Offset: 0x000DA91B
		public string Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06003410 RID: 13328 RVA: 0x000DB923 File Offset: 0x000DA923
		public string Address
		{
			get
			{
				if (this.address == null)
				{
					this.CombineParts();
				}
				return this.address;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06003411 RID: 13329 RVA: 0x000DB93C File Offset: 0x000DA93C
		internal string SmtpAddress
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('<');
				stringBuilder.Append(this.Address);
				stringBuilder.Append('>');
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000DB974 File Offset: 0x000DA974
		internal string ToEncodedString()
		{
			if (this.fullAddress == null)
			{
				if (this.encodedDisplayName != null && this.encodedDisplayName != string.Empty)
				{
					StringBuilder stringBuilder = new StringBuilder();
					MailBnfHelper.GetDotAtomOrQuotedString(this.encodedDisplayName, stringBuilder);
					stringBuilder.Append(" <");
					stringBuilder.Append(this.Address);
					stringBuilder.Append('>');
					this.fullAddress = stringBuilder.ToString();
				}
				else
				{
					this.fullAddress = this.Address;
				}
			}
			return this.fullAddress;
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000DB9F8 File Offset: 0x000DA9F8
		public override string ToString()
		{
			if (this.fullAddress == null)
			{
				if (this.encodedDisplayName != null && this.encodedDisplayName != string.Empty)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (this.DisplayName.StartsWith("\"") && this.DisplayName.EndsWith("\""))
					{
						stringBuilder.Append(this.DisplayName);
					}
					else
					{
						stringBuilder.Append('"');
						stringBuilder.Append(this.DisplayName);
						stringBuilder.Append('"');
					}
					stringBuilder.Append(" <");
					stringBuilder.Append(this.Address);
					stringBuilder.Append('>');
					this.fullAddress = stringBuilder.ToString();
				}
				else
				{
					this.fullAddress = this.Address;
				}
			}
			return this.fullAddress;
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x000DBACA File Offset: 0x000DAACA
		public override bool Equals(object value)
		{
			return value != null && this.ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x000DBAE3 File Offset: 0x000DAAE3
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x000DBAF0 File Offset: 0x000DAAF0
		private void GetParts(string address)
		{
			if (address == null)
			{
				return;
			}
			int num = address.IndexOf('@');
			if (num < 0)
			{
				throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
			}
			this.userName = address.Substring(0, num);
			this.host = address.Substring(num + 1);
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x000DBB3C File Offset: 0x000DAB3C
		private void ParseValue(string address)
		{
			string text = null;
			int num = 0;
			MailBnfHelper.SkipFWS(address, ref num);
			int num2 = address.IndexOf('"', num);
			if (num2 == num)
			{
				num2 = address.IndexOf('"', num2 + 1);
				if (num2 > num)
				{
					int num3 = num2 + 1;
					MailBnfHelper.SkipFWS(address, ref num3);
					if (address.Length > num3 && address[num3] != '@')
					{
						text = address.Substring(num, num2 + 1 - num);
						address = address.Substring(num3);
					}
				}
			}
			if (text == null)
			{
				int num4 = address.IndexOf('<', num);
				if (num4 >= num)
				{
					text = address.Substring(num, num4 - num);
					address = address.Substring(num4);
				}
			}
			if (text == null)
			{
				num2 = address.IndexOf('"', num);
				if (num2 > num)
				{
					text = address.Substring(num, num2 - num);
					address = address.Substring(num2);
				}
			}
			if (this.displayName == null)
			{
				this.displayName = text;
			}
			int num5 = 0;
			address = MailBnfHelper.ReadMailAddress(address, ref num5, out this.encodedDisplayName);
			this.GetParts(address);
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000DBC24 File Offset: 0x000DAC24
		private void CombineParts()
		{
			if (this.userName == null || this.host == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			MailBnfHelper.GetDotAtomOrQuotedString(this.User, stringBuilder);
			stringBuilder.Append('@');
			MailBnfHelper.GetDotAtomOrDomainLiteral(this.Host, stringBuilder);
			this.address = stringBuilder.ToString();
		}

		// Token: 0x04002FF0 RID: 12272
		private string displayName;

		// Token: 0x04002FF1 RID: 12273
		private Encoding displayNameEncoding;

		// Token: 0x04002FF2 RID: 12274
		private string encodedDisplayName;

		// Token: 0x04002FF3 RID: 12275
		private string address;

		// Token: 0x04002FF4 RID: 12276
		private string fullAddress;

		// Token: 0x04002FF5 RID: 12277
		private string userName;

		// Token: 0x04002FF6 RID: 12278
		private string host;
	}
}
