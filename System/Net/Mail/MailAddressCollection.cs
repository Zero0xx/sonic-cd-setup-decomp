using System;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000698 RID: 1688
	public class MailAddressCollection : Collection<MailAddress>
	{
		// Token: 0x0600341A RID: 13338 RVA: 0x000DBC80 File Offset: 0x000DAC80
		public void Add(string addresses)
		{
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (addresses == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"addresses"
				}), "addresses");
			}
			this.ParseValue(addresses);
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000DBCD4 File Offset: 0x000DACD4
		protected override void SetItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000DBCEC File Offset: 0x000DACEC
		protected override void InsertItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000DBD04 File Offset: 0x000DAD04
		internal void ParseValue(string addresses)
		{
			for (int i = 0; i < addresses.Length; i++)
			{
				MailAddress mailAddress = MailBnfHelper.ReadMailAddress(addresses, ref i);
				if (mailAddress == null)
				{
					return;
				}
				base.Add(mailAddress);
				if (!MailBnfHelper.SkipCFWS(addresses, ref i))
				{
					break;
				}
				if (addresses[i] != ',')
				{
					return;
				}
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000DBD4C File Offset: 0x000DAD4C
		internal string ToEncodedString()
		{
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MailAddress mailAddress in this)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(mailAddress.ToEncodedString());
				flag = false;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000DBDBC File Offset: 0x000DADBC
		public override string ToString()
		{
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MailAddress mailAddress in this)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(mailAddress.ToString());
				flag = false;
			}
			return stringBuilder.ToString();
		}
	}
}
