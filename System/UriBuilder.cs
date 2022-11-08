using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

namespace System
{
	// Token: 0x0200035C RID: 860
	public class UriBuilder
	{
		// Token: 0x06001B61 RID: 7009 RVA: 0x00066BA8 File Offset: 0x00065BA8
		public UriBuilder()
		{
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x00066C24 File Offset: 0x00065C24
		public UriBuilder(string uri)
		{
			Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
			if (uri2.IsAbsoluteUri)
			{
				this.Init(uri2);
				return;
			}
			uri = Uri.UriSchemeHttp + Uri.SchemeDelimiter + uri;
			this.Init(new Uri(uri));
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x00066CD4 File Offset: 0x00065CD4
		public UriBuilder(Uri uri)
		{
			this.Init(uri);
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x00066D54 File Offset: 0x00065D54
		private void Init(Uri uri)
		{
			this.m_fragment = uri.Fragment;
			this.m_query = uri.Query;
			this.m_host = uri.Host;
			this.m_path = uri.AbsolutePath;
			this.m_port = uri.Port;
			this.m_scheme = uri.Scheme;
			this.m_schemeDelimiter = (uri.HasAuthority ? Uri.SchemeDelimiter : ":");
			string userInfo = uri.UserInfo;
			if (!ValidationHelper.IsBlankString(userInfo))
			{
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					this.m_password = userInfo.Substring(num + 1);
					this.m_username = userInfo.Substring(0, num);
				}
				else
				{
					this.m_username = userInfo;
				}
			}
			this.SetFieldsFromUri(uri);
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x00066E0C File Offset: 0x00065E0C
		public UriBuilder(string schemeName, string hostName)
		{
			this.Scheme = schemeName;
			this.Host = hostName;
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x00066E93 File Offset: 0x00065E93
		public UriBuilder(string scheme, string host, int portNumber) : this(scheme, host)
		{
			this.Port = portNumber;
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x00066EA4 File Offset: 0x00065EA4
		public UriBuilder(string scheme, string host, int port, string pathValue) : this(scheme, host, port)
		{
			this.Path = pathValue;
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x00066EB8 File Offset: 0x00065EB8
		public UriBuilder(string scheme, string host, int port, string path, string extraValue) : this(scheme, host, port, path)
		{
			try
			{
				this.Extra = extraValue;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException("extraValue");
			}
		}

		// Token: 0x17000548 RID: 1352
		// (set) Token: 0x06001B69 RID: 7017 RVA: 0x00066F10 File Offset: 0x00065F10
		private string Extra
		{
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length <= 0)
				{
					this.Fragment = string.Empty;
					this.Query = string.Empty;
					return;
				}
				if (value[0] == '#')
				{
					this.Fragment = value.Substring(1);
					return;
				}
				if (value[0] == '?')
				{
					int num = value.IndexOf('#');
					if (num == -1)
					{
						num = value.Length;
					}
					else
					{
						this.Fragment = value.Substring(num + 1);
					}
					this.Query = value.Substring(1, num - 1);
					return;
				}
				throw new ArgumentException("value");
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001B6A RID: 7018 RVA: 0x00066FAB File Offset: 0x00065FAB
		// (set) Token: 0x06001B6B RID: 7019 RVA: 0x00066FB3 File Offset: 0x00065FB3
		public string Fragment
		{
			get
			{
				return this.m_fragment;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length > 0)
				{
					value = '#' + value;
				}
				this.m_fragment = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x00066FE5 File Offset: 0x00065FE5
		// (set) Token: 0x06001B6D RID: 7021 RVA: 0x00066FF0 File Offset: 0x00065FF0
		public string Host
		{
			get
			{
				return this.m_host;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_host = value;
				if (this.m_host.IndexOf(':') >= 0 && this.m_host[0] != '[')
				{
					this.m_host = "[" + this.m_host + "]";
				}
				this.m_changed = true;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001B6E RID: 7022 RVA: 0x00067050 File Offset: 0x00066050
		// (set) Token: 0x06001B6F RID: 7023 RVA: 0x00067058 File Offset: 0x00066058
		public string Password
		{
			get
			{
				return this.m_password;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_password = value;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001B70 RID: 7024 RVA: 0x0006706B File Offset: 0x0006606B
		// (set) Token: 0x06001B71 RID: 7025 RVA: 0x00067073 File Offset: 0x00066073
		public string Path
		{
			get
			{
				return this.m_path;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					value = "/";
				}
				this.m_path = Uri.InternalEscapeString(this.ConvertSlashes(value));
				this.m_changed = true;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001B72 RID: 7026 RVA: 0x000670A0 File Offset: 0x000660A0
		// (set) Token: 0x06001B73 RID: 7027 RVA: 0x000670A8 File Offset: 0x000660A8
		public int Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				if (value < -1 || value > 65535)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_port = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x000670CF File Offset: 0x000660CF
		// (set) Token: 0x06001B75 RID: 7029 RVA: 0x000670D7 File Offset: 0x000660D7
		public string Query
		{
			get
			{
				return this.m_query;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length > 0)
				{
					value = '?' + value;
				}
				this.m_query = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x00067109 File Offset: 0x00066109
		// (set) Token: 0x06001B77 RID: 7031 RVA: 0x00067114 File Offset: 0x00066114
		public string Scheme
		{
			get
			{
				return this.m_scheme;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				int num = value.IndexOf(':');
				if (num != -1)
				{
					value = value.Substring(0, num);
				}
				if (value.Length != 0)
				{
					if (!Uri.CheckSchemeName(value))
					{
						throw new ArgumentException("value");
					}
					value = value.ToLower(CultureInfo.InvariantCulture);
				}
				this.m_scheme = value;
				this.m_changed = true;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00067178 File Offset: 0x00066178
		public Uri Uri
		{
			get
			{
				if (this.m_changed)
				{
					this.m_uri = new Uri(this.ToString());
					this.SetFieldsFromUri(this.m_uri);
					this.m_changed = false;
				}
				return this.m_uri;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x000671AC File Offset: 0x000661AC
		// (set) Token: 0x06001B7A RID: 7034 RVA: 0x000671B4 File Offset: 0x000661B4
		public string UserName
		{
			get
			{
				return this.m_username;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_username = value;
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000671C8 File Offset: 0x000661C8
		private string ConvertSlashes(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(path.Length);
			foreach (char c in path)
			{
				if (c == '\\')
				{
					c = '/';
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x00067210 File Offset: 0x00066210
		public override bool Equals(object rparam)
		{
			return rparam != null && this.Uri.Equals(rparam.ToString());
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00067228 File Offset: 0x00066228
		public override int GetHashCode()
		{
			return this.Uri.GetHashCode();
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00067238 File Offset: 0x00066238
		private void SetFieldsFromUri(Uri uri)
		{
			this.m_fragment = uri.Fragment;
			this.m_query = uri.Query;
			this.m_host = uri.Host;
			this.m_path = uri.AbsolutePath;
			this.m_port = uri.Port;
			this.m_scheme = uri.Scheme;
			this.m_schemeDelimiter = (uri.HasAuthority ? Uri.SchemeDelimiter : ":");
			string userInfo = uri.UserInfo;
			if (userInfo.Length > 0)
			{
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					this.m_password = userInfo.Substring(num + 1);
					this.m_username = userInfo.Substring(0, num);
					return;
				}
				this.m_username = userInfo;
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000672EC File Offset: 0x000662EC
		public override string ToString()
		{
			if (this.m_username.Length == 0 && this.m_password.Length > 0)
			{
				throw new UriFormatException(SR.GetString("net_uri_BadUserPassword"));
			}
			if (this.m_scheme.Length != 0)
			{
				UriParser syntax = UriParser.GetSyntax(this.m_scheme);
				if (syntax != null)
				{
					this.m_schemeDelimiter = ((syntax.InFact(UriSyntaxFlags.MustHaveAuthority) || (this.m_host.Length != 0 && syntax.NotAny(UriSyntaxFlags.MailToLikeUri) && syntax.InFact(UriSyntaxFlags.OptionalAuthority))) ? Uri.SchemeDelimiter : ":");
				}
				else
				{
					this.m_schemeDelimiter = ((this.m_host.Length != 0) ? Uri.SchemeDelimiter : ":");
				}
			}
			string text = (this.m_scheme.Length != 0) ? (this.m_scheme + this.m_schemeDelimiter) : string.Empty;
			return string.Concat(new string[]
			{
				text,
				this.m_username,
				(this.m_password.Length > 0) ? (":" + this.m_password) : string.Empty,
				(this.m_username.Length > 0) ? "@" : string.Empty,
				this.m_host,
				(this.m_port != -1 && this.m_host.Length > 0) ? (":" + this.m_port) : string.Empty,
				(this.m_host.Length > 0 && this.m_path.Length != 0 && this.m_path[0] != '/') ? "/" : string.Empty,
				this.m_path,
				this.m_query,
				this.m_fragment
			});
		}

		// Token: 0x04001C12 RID: 7186
		private bool m_changed = true;

		// Token: 0x04001C13 RID: 7187
		private string m_fragment = string.Empty;

		// Token: 0x04001C14 RID: 7188
		private string m_host = "localhost";

		// Token: 0x04001C15 RID: 7189
		private string m_password = string.Empty;

		// Token: 0x04001C16 RID: 7190
		private string m_path = "/";

		// Token: 0x04001C17 RID: 7191
		private int m_port = -1;

		// Token: 0x04001C18 RID: 7192
		private string m_query = string.Empty;

		// Token: 0x04001C19 RID: 7193
		private string m_scheme = "http";

		// Token: 0x04001C1A RID: 7194
		private string m_schemeDelimiter = Uri.SchemeDelimiter;

		// Token: 0x04001C1B RID: 7195
		private Uri m_uri;

		// Token: 0x04001C1C RID: 7196
		private string m_username = string.Empty;
	}
}
