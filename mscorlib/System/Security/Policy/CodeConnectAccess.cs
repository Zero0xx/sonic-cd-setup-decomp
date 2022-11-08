using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x020004AA RID: 1194
	[ComVisible(true)]
	[Serializable]
	public class CodeConnectAccess
	{
		// Token: 0x06002F44 RID: 12100 RVA: 0x0009FFAC File Offset: 0x0009EFAC
		public CodeConnectAccess(string allowScheme, int allowPort)
		{
			if (!CodeConnectAccess.IsValidScheme(allowScheme))
			{
				throw new ArgumentOutOfRangeException("allowScheme");
			}
			this.SetCodeConnectAccess(allowScheme.ToLower(CultureInfo.InvariantCulture), allowPort);
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x0009FFDC File Offset: 0x0009EFDC
		public static CodeConnectAccess CreateOriginSchemeAccess(int allowPort)
		{
			CodeConnectAccess codeConnectAccess = new CodeConnectAccess();
			codeConnectAccess.SetCodeConnectAccess(CodeConnectAccess.OriginScheme, allowPort);
			return codeConnectAccess;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x0009FFFC File Offset: 0x0009EFFC
		public static CodeConnectAccess CreateAnySchemeAccess(int allowPort)
		{
			CodeConnectAccess codeConnectAccess = new CodeConnectAccess();
			codeConnectAccess.SetCodeConnectAccess(CodeConnectAccess.AnyScheme, allowPort);
			return codeConnectAccess;
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000A001C File Offset: 0x0009F01C
		private CodeConnectAccess()
		{
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000A0024 File Offset: 0x0009F024
		private void SetCodeConnectAccess(string lowerCaseScheme, int allowPort)
		{
			this._LowerCaseScheme = lowerCaseScheme;
			if (allowPort == CodeConnectAccess.DefaultPort)
			{
				this._LowerCasePort = "$default";
			}
			else if (allowPort == CodeConnectAccess.OriginPort)
			{
				this._LowerCasePort = "$origin";
			}
			else
			{
				if (allowPort < 0 || allowPort > 65535)
				{
					throw new ArgumentOutOfRangeException("allowPort");
				}
				this._LowerCasePort = allowPort.ToString(CultureInfo.InvariantCulture);
			}
			this._IntPort = allowPort;
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002F49 RID: 12105 RVA: 0x000A0092 File Offset: 0x0009F092
		public string Scheme
		{
			get
			{
				return this._LowerCaseScheme;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002F4A RID: 12106 RVA: 0x000A009A File Offset: 0x0009F09A
		public int Port
		{
			get
			{
				return this._IntPort;
			}
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000A00A4 File Offset: 0x0009F0A4
		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			CodeConnectAccess codeConnectAccess = o as CodeConnectAccess;
			return codeConnectAccess != null && this.Scheme == codeConnectAccess.Scheme && this.Port == codeConnectAccess.Port;
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000A00E8 File Offset: 0x0009F0E8
		public override int GetHashCode()
		{
			return this.Scheme.GetHashCode() + this.Port.GetHashCode();
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000A0110 File Offset: 0x0009F110
		internal CodeConnectAccess(string allowScheme, string allowPort)
		{
			if (allowScheme == null || allowScheme.Length == 0)
			{
				throw new ArgumentNullException("allowScheme");
			}
			if (allowPort == null || allowPort.Length == 0)
			{
				throw new ArgumentNullException("allowPort");
			}
			this._LowerCaseScheme = allowScheme.ToLower(CultureInfo.InvariantCulture);
			if (this._LowerCaseScheme == CodeConnectAccess.OriginScheme)
			{
				this._LowerCaseScheme = CodeConnectAccess.OriginScheme;
			}
			else if (this._LowerCaseScheme == CodeConnectAccess.AnyScheme)
			{
				this._LowerCaseScheme = CodeConnectAccess.AnyScheme;
			}
			else if (!CodeConnectAccess.IsValidScheme(this._LowerCaseScheme))
			{
				throw new ArgumentOutOfRangeException("allowScheme");
			}
			this._LowerCasePort = allowPort.ToLower(CultureInfo.InvariantCulture);
			if (this._LowerCasePort == "$default")
			{
				this._IntPort = CodeConnectAccess.DefaultPort;
				return;
			}
			if (this._LowerCasePort == "$origin")
			{
				this._IntPort = CodeConnectAccess.OriginPort;
				return;
			}
			this._IntPort = int.Parse(allowPort, CultureInfo.InvariantCulture);
			if (this._IntPort < 0 || this._IntPort > 65535)
			{
				throw new ArgumentOutOfRangeException("allowPort");
			}
			this._LowerCasePort = this._IntPort.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002F4E RID: 12110 RVA: 0x000A024B File Offset: 0x0009F24B
		internal bool IsOriginScheme
		{
			get
			{
				return this._LowerCaseScheme == CodeConnectAccess.OriginScheme;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002F4F RID: 12111 RVA: 0x000A025A File Offset: 0x0009F25A
		internal bool IsAnyScheme
		{
			get
			{
				return this._LowerCaseScheme == CodeConnectAccess.AnyScheme;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002F50 RID: 12112 RVA: 0x000A0269 File Offset: 0x0009F269
		internal bool IsDefaultPort
		{
			get
			{
				return this.Port == CodeConnectAccess.DefaultPort;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002F51 RID: 12113 RVA: 0x000A0278 File Offset: 0x0009F278
		internal bool IsOriginPort
		{
			get
			{
				return this.Port == CodeConnectAccess.OriginPort;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002F52 RID: 12114 RVA: 0x000A0287 File Offset: 0x0009F287
		internal string StrPort
		{
			get
			{
				return this._LowerCasePort;
			}
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000A0290 File Offset: 0x0009F290
		internal static bool IsValidScheme(string scheme)
		{
			if (scheme == null || scheme.Length == 0 || !CodeConnectAccess.IsAsciiLetter(scheme[0]))
			{
				return false;
			}
			for (int i = scheme.Length - 1; i > 0; i--)
			{
				if (!CodeConnectAccess.IsAsciiLetterOrDigit(scheme[i]) && scheme[i] != '+' && scheme[i] != '-' && scheme[i] != '.')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000A02FD File Offset: 0x0009F2FD
		private static bool IsAsciiLetterOrDigit(char character)
		{
			return CodeConnectAccess.IsAsciiLetter(character) || (character >= '0' && character <= '9');
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000A0318 File Offset: 0x0009F318
		private static bool IsAsciiLetter(char character)
		{
			return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
		}

		// Token: 0x04001810 RID: 6160
		private const string DefaultStr = "$default";

		// Token: 0x04001811 RID: 6161
		private const string OriginStr = "$origin";

		// Token: 0x04001812 RID: 6162
		internal const int NoPort = -1;

		// Token: 0x04001813 RID: 6163
		internal const int AnyPort = -2;

		// Token: 0x04001814 RID: 6164
		private string _LowerCaseScheme;

		// Token: 0x04001815 RID: 6165
		private string _LowerCasePort;

		// Token: 0x04001816 RID: 6166
		private int _IntPort;

		// Token: 0x04001817 RID: 6167
		public static readonly int DefaultPort = -3;

		// Token: 0x04001818 RID: 6168
		public static readonly int OriginPort = -4;

		// Token: 0x04001819 RID: 6169
		public static readonly string OriginScheme = "$origin";

		// Token: 0x0400181A RID: 6170
		public static readonly string AnyScheme = "*";
	}
}
