using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;

namespace System.Net.Mime
{
	// Token: 0x02000688 RID: 1672
	internal class HeaderCollection : NameValueCollection
	{
		// Token: 0x060033CA RID: 13258 RVA: 0x000DACD0 File Offset: 0x000D9CD0
		internal HeaderCollection() : base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000DACE0 File Offset: 0x000D9CE0
		public override void Remove(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Remove", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"name"
				}), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType = null;
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition = null;
			}
			base.Remove(name);
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x000DAD90 File Offset: 0x000D9D90
		public override string Get(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Get", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"name"
				}), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.Get(name);
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000DAE4C File Offset: 0x000D9E4C
		public override string[] GetValues(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Get", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"name"
				}), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.GetValues(name);
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x000DAF07 File Offset: 0x000D9F07
		internal void InternalRemove(string name)
		{
			base.Remove(name);
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x000DAF10 File Offset: 0x000D9F10
		internal void InternalSet(string name, string value)
		{
			base.Set(name, value);
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000DAF1C File Offset: 0x000D9F1C
		public override void Set(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Set", name.ToString() + "=" + value.ToString());
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"name"
				}), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"value"
				}), "name");
			}
			if (!MimeBasePart.IsAscii(name, false))
			{
				throw new FormatException(SR.GetString("InvalidHeaderName"));
			}
			if (!MimeBasePart.IsAnsi(value, false))
			{
				throw new FormatException(SR.GetString("InvalidHeaderValue"));
			}
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			base.Set(name, value);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000DB07C File Offset: 0x000DA07C
		public override void Add(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Add", name.ToString() + "=" + value.ToString());
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"name"
				}), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"value"
				}), "name");
			}
			MailBnfHelper.ValidateHeaderName(name);
			if (!MimeBasePart.IsAnsi(value, false))
			{
				throw new FormatException(SR.GetString("InvalidHeaderValue"));
			}
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (MailHeaderInfo.IsSingleton(name))
			{
				base.Set(name, value);
				return;
			}
			base.Add(name, value);
		}

		// Token: 0x04002FBA RID: 12218
		private MimeBasePart part;
	}
}
