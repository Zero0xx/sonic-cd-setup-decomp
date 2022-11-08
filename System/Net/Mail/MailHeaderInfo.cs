using System;
using System.Collections.Generic;

namespace System.Net.Mail
{
	// Token: 0x0200069B RID: 1691
	internal static class MailHeaderInfo
	{
		// Token: 0x0600343B RID: 13371 RVA: 0x000DD9CC File Offset: 0x000DC9CC
		static MailHeaderInfo()
		{
			for (int i = 0; i < MailHeaderInfo.m_HeaderInfo.Length; i++)
			{
				MailHeaderInfo.m_HeaderDictionary.Add(MailHeaderInfo.m_HeaderInfo[i].NormalizedName, i);
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000DDD74 File Offset: 0x000DCD74
		internal static string GetString(MailHeaderID id)
		{
			if (id == MailHeaderID.Unknown || id == (MailHeaderID)33)
			{
				return null;
			}
			return MailHeaderInfo.m_HeaderInfo[(int)id].NormalizedName;
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000DDDA4 File Offset: 0x000DCDA4
		internal static MailHeaderID GetID(string name)
		{
			int result;
			if (MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out result))
			{
				return (MailHeaderID)result;
			}
			return MailHeaderID.Unknown;
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000DDDC4 File Offset: 0x000DCDC4
		internal static bool IsWellKnown(string name)
		{
			int num;
			return MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num);
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000DDDE0 File Offset: 0x000DCDE0
		internal static bool IsSingleton(string name)
		{
			int num;
			return MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num) && MailHeaderInfo.m_HeaderInfo[num].IsSingleton;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000DDE14 File Offset: 0x000DCE14
		internal static string NormalizeCase(string name)
		{
			int num;
			if (MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num))
			{
				return MailHeaderInfo.m_HeaderInfo[num].NormalizedName;
			}
			return name;
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000DDE48 File Offset: 0x000DCE48
		internal static bool IsMatch(string name, MailHeaderID header)
		{
			int num;
			return MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num) && num == (int)header;
		}

		// Token: 0x04003025 RID: 12325
		private static readonly MailHeaderInfo.HeaderInfo[] m_HeaderInfo = new MailHeaderInfo.HeaderInfo[]
		{
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Bcc, "Bcc", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Cc, "Cc", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Comments, "Comments", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentDescription, "Content-Description", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentDisposition, "Content-Disposition", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentID, "Content-ID", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentLocation, "Content-Location", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentTransferEncoding, "Content-Transfer-Encoding", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentType, "Content-Type", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Date, "Date", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.From, "From", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Importance, "Importance", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.InReplyTo, "In-Reply-To", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Keywords, "Keywords", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Max, "Max", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.MessageID, "Message-ID", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.MimeVersion, "MIME-Version", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Priority, "Priority", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.References, "References", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ReplyTo, "Reply-To", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentBcc, "Resent-Bcc", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentCc, "Resent-Cc", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentDate, "Resent-Date", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentFrom, "Resent-From", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentMessageID, "Resent-Message-ID", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentSender, "Resent-Sender", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentTo, "Resent-To", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Sender, "Sender", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Subject, "Subject", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.To, "To", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XPriority, "X-Priority", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XReceiver, "X-Receiver", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XSender, "X-Sender", true)
		};

		// Token: 0x04003026 RID: 12326
		private static readonly Dictionary<string, int> m_HeaderDictionary = new Dictionary<string, int>(33, StringComparer.OrdinalIgnoreCase);

		// Token: 0x0200069C RID: 1692
		private struct HeaderInfo
		{
			// Token: 0x06003442 RID: 13378 RVA: 0x000DDE6B File Offset: 0x000DCE6B
			public HeaderInfo(MailHeaderID id, string name, bool isSingleton)
			{
				this.ID = id;
				this.NormalizedName = name;
				this.IsSingleton = isSingleton;
			}

			// Token: 0x04003027 RID: 12327
			public readonly string NormalizedName;

			// Token: 0x04003028 RID: 12328
			public readonly bool IsSingleton;

			// Token: 0x04003029 RID: 12329
			public readonly MailHeaderID ID;
		}
	}
}
