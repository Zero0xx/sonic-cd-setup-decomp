using System;
using System.Text;

namespace System.Net
{
	// Token: 0x020004A5 RID: 1189
	internal class HostHeaderString
	{
		// Token: 0x06002464 RID: 9316 RVA: 0x0008F2FE File Offset: 0x0008E2FE
		internal HostHeaderString()
		{
			this.Init(null);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0008F30D File Offset: 0x0008E30D
		internal HostHeaderString(string s)
		{
			this.Init(s);
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x0008F31C File Offset: 0x0008E31C
		private void Init(string s)
		{
			this.m_String = s;
			this.m_Converted = false;
			this.m_Bytes = null;
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0008F334 File Offset: 0x0008E334
		private void Convert()
		{
			if (this.m_String != null && !this.m_Converted)
			{
				this.m_Bytes = Encoding.Default.GetBytes(this.m_String);
				string @string = Encoding.Default.GetString(this.m_Bytes);
				if (string.Compare(this.m_String, @string, StringComparison.Ordinal) != 0)
				{
					this.m_Bytes = Encoding.UTF8.GetBytes(this.m_String);
				}
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x0008F39D File Offset: 0x0008E39D
		// (set) Token: 0x06002469 RID: 9321 RVA: 0x0008F3A5 File Offset: 0x0008E3A5
		internal string String
		{
			get
			{
				return this.m_String;
			}
			set
			{
				this.Init(value);
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x0008F3AE File Offset: 0x0008E3AE
		internal int ByteCount
		{
			get
			{
				this.Convert();
				return this.m_Bytes.Length;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x0008F3BE File Offset: 0x0008E3BE
		internal byte[] Bytes
		{
			get
			{
				this.Convert();
				return this.m_Bytes;
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0008F3CC File Offset: 0x0008E3CC
		internal void Copy(byte[] destBytes, int destByteIndex)
		{
			this.Convert();
			Array.Copy(this.m_Bytes, 0, destBytes, destByteIndex, this.m_Bytes.Length);
		}

		// Token: 0x040024B4 RID: 9396
		private bool m_Converted;

		// Token: 0x040024B5 RID: 9397
		private string m_String;

		// Token: 0x040024B6 RID: 9398
		private byte[] m_Bytes;
	}
}
