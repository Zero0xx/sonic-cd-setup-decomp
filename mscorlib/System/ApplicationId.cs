using System;
using System.Runtime.InteropServices;
using System.Security.Util;
using System.Text;

namespace System
{
	// Token: 0x0200006C RID: 108
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationId
	{
		// Token: 0x0600063D RID: 1597 RVA: 0x00015685 File Offset: 0x00014685
		internal ApplicationId()
		{
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00015690 File Offset: 0x00014690
		public ApplicationId(byte[] publicKeyToken, string name, Version version, string processorArchitecture, string culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyApplicationName"));
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (publicKeyToken == null)
			{
				throw new ArgumentNullException("publicKeyToken");
			}
			this.m_publicKeyToken = new byte[publicKeyToken.Length];
			Array.Copy(publicKeyToken, 0, this.m_publicKeyToken, 0, publicKeyToken.Length);
			this.m_name = name;
			this.m_version = version;
			this.m_processorArchitecture = processorArchitecture;
			this.m_culture = culture;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00015728 File Offset: 0x00014728
		public byte[] PublicKeyToken
		{
			get
			{
				byte[] array = new byte[this.m_publicKeyToken.Length];
				Array.Copy(this.m_publicKeyToken, 0, array, 0, this.m_publicKeyToken.Length);
				return array;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0001575A File Offset: 0x0001475A
		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00015762 File Offset: 0x00014762
		public Version Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0001576A File Offset: 0x0001476A
		public string ProcessorArchitecture
		{
			get
			{
				return this.m_processorArchitecture;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00015772 File Offset: 0x00014772
		public string Culture
		{
			get
			{
				return this.m_culture;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001577A File Offset: 0x0001477A
		public ApplicationId Copy()
		{
			return new ApplicationId(this.m_publicKeyToken, this.m_name, this.m_version, this.m_processorArchitecture, this.m_culture);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000157A0 File Offset: 0x000147A0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.m_name);
			if (this.m_culture != null)
			{
				stringBuilder.Append(", culture=\"");
				stringBuilder.Append(this.m_culture);
				stringBuilder.Append("\"");
			}
			stringBuilder.Append(", version=\"");
			stringBuilder.Append(this.m_version.ToString());
			stringBuilder.Append("\"");
			if (this.m_publicKeyToken != null)
			{
				stringBuilder.Append(", publicKeyToken=\"");
				stringBuilder.Append(Hex.EncodeHexString(this.m_publicKeyToken));
				stringBuilder.Append("\"");
			}
			if (this.m_processorArchitecture != null)
			{
				stringBuilder.Append(", processorArchitecture =\"");
				stringBuilder.Append(this.m_processorArchitecture);
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001587C File Offset: 0x0001487C
		public override bool Equals(object o)
		{
			ApplicationId applicationId = o as ApplicationId;
			if (applicationId == null)
			{
				return false;
			}
			if (!object.Equals(this.m_name, applicationId.m_name) || !object.Equals(this.m_version, applicationId.m_version) || !object.Equals(this.m_processorArchitecture, applicationId.m_processorArchitecture) || !object.Equals(this.m_culture, applicationId.m_culture))
			{
				return false;
			}
			if (this.m_publicKeyToken.Length != applicationId.m_publicKeyToken.Length)
			{
				return false;
			}
			for (int i = 0; i < this.m_publicKeyToken.Length; i++)
			{
				if (this.m_publicKeyToken[i] != applicationId.m_publicKeyToken[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001591F File Offset: 0x0001491F
		public override int GetHashCode()
		{
			return this.m_name.GetHashCode() ^ this.m_version.GetHashCode();
		}

		// Token: 0x040001F1 RID: 497
		private string m_name;

		// Token: 0x040001F2 RID: 498
		private Version m_version;

		// Token: 0x040001F3 RID: 499
		private string m_processorArchitecture;

		// Token: 0x040001F4 RID: 500
		private string m_culture;

		// Token: 0x040001F5 RID: 501
		internal byte[] m_publicKeyToken;
	}
}
