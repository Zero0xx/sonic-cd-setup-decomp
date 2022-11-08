using System;
using System.Globalization;
using System.Security;

namespace System.Net
{
	// Token: 0x02000440 RID: 1088
	[Serializable]
	public class EndpointPermission
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002233 RID: 8755 RVA: 0x00087B64 File Offset: 0x00086B64
		public string Hostname
		{
			get
			{
				return this.hostname;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x00087B6C File Offset: 0x00086B6C
		public TransportType Transport
		{
			get
			{
				return this.transport;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x00087B74 File Offset: 0x00086B74
		public int Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x00087B7C File Offset: 0x00086B7C
		internal EndpointPermission(string epname, int port, TransportType trtype)
		{
			if (EndpointPermission.CheckEndPointName(epname) == EndpointPermission.EndPointType.Invalid)
			{
				throw new ArgumentException(SR.GetString("net_perm_epname", new object[]
				{
					epname
				}), "epname");
			}
			if (!ValidationHelper.ValidateTcpPort(port) && port != -1)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_perm_invalid_val", new object[]
				{
					"Port",
					port.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			this.hostname = epname;
			this.port = port;
			this.transport = trtype;
			this.wildcard = false;
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00087C10 File Offset: 0x00086C10
		public override bool Equals(object obj)
		{
			EndpointPermission endpointPermission = (EndpointPermission)obj;
			return string.Compare(this.hostname, endpointPermission.hostname, StringComparison.OrdinalIgnoreCase) == 0 && this.port == endpointPermission.port && this.transport == endpointPermission.transport;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00087C5B File Offset: 0x00086C5B
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x00087C68 File Offset: 0x00086C68
		internal bool IsDns
		{
			get
			{
				return !this.IsValidWildcard && EndpointPermission.CheckEndPointName(this.hostname) == EndpointPermission.EndPointType.DnsOrWildcard;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x00087C84 File Offset: 0x00086C84
		private bool IsValidWildcard
		{
			get
			{
				int length = this.hostname.Length;
				if (length < 3)
				{
					return false;
				}
				if (this.hostname[0] == '.' || this.hostname[length - 1] == '.')
				{
					return false;
				}
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.hostname.Length; i++)
				{
					if (this.hostname[i] == '.')
					{
						num++;
					}
					else if (this.hostname[i] == '*')
					{
						num2++;
					}
					else if (!char.IsDigit(this.hostname[i]))
					{
						return false;
					}
				}
				return num == 3 && num2 > 0;
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x00087D2C File Offset: 0x00086D2C
		internal bool MatchAddress(EndpointPermission e)
		{
			if (this.Hostname.Length == 0 || e.Hostname.Length == 0)
			{
				return false;
			}
			if (this.Hostname.Equals("0.0.0.0"))
			{
				return e.Hostname.Equals("*.*.*.*") || e.Hostname.Equals("0.0.0.0");
			}
			if (this.IsDns && e.IsDns)
			{
				return string.Compare(this.hostname, e.hostname, StringComparison.OrdinalIgnoreCase) == 0;
			}
			this.Resolve();
			e.Resolve();
			if ((this.address == null && !this.wildcard) || (e.address == null && !e.wildcard))
			{
				return false;
			}
			if (this.wildcard && !e.wildcard)
			{
				return false;
			}
			if (e.wildcard)
			{
				if (this.wildcard)
				{
					if (this.MatchWildcard(e.hostname))
					{
						return true;
					}
				}
				else
				{
					for (int i = 0; i < this.address.Length; i++)
					{
						if (e.MatchWildcard(this.address[i].ToString()))
						{
							return true;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < this.address.Length; j++)
				{
					for (int k = 0; k < e.address.Length; k++)
					{
						if (this.address[j].Equals(e.address[k]))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x00087E84 File Offset: 0x00086E84
		internal bool MatchWildcard(string str)
		{
			string[] array = this.hostname.Split(EndpointPermission.DotSeparator);
			string[] array2 = str.Split(EndpointPermission.DotSeparator);
			if (array2.Length != 4 || array.Length != 4)
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				if (array2[i] != array[i] && array[i] != "*")
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00087EE8 File Offset: 0x00086EE8
		internal void Resolve()
		{
			if (this.cached)
			{
				return;
			}
			if (this.wildcard)
			{
				return;
			}
			if (this.IsValidWildcard)
			{
				this.wildcard = true;
				this.cached = true;
				return;
			}
			IPAddress ipaddress;
			if (IPAddress.TryParse(this.hostname, out ipaddress))
			{
				this.address = new IPAddress[1];
				this.address[0] = ipaddress;
				this.cached = true;
				return;
			}
			try
			{
				bool flag;
				IPHostEntry iphostEntry = Dns.InternalResolveFast(this.hostname, -1, out flag);
				if (iphostEntry != null)
				{
					this.address = iphostEntry.AddressList;
				}
			}
			catch (SecurityException)
			{
				throw;
			}
			catch
			{
			}
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00087F8C File Offset: 0x00086F8C
		internal bool SubsetMatch(EndpointPermission e)
		{
			return (this.transport == e.transport || e.transport == TransportType.All) && (this.port == e.port || e.port == -1 || this.port == 0) && this.MatchAddress(e);
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00087FD8 File Offset: 0x00086FD8
		public override string ToString()
		{
			object[] array = new object[5];
			array[0] = this.hostname;
			array[1] = "#";
			array[2] = this.port;
			array[3] = "#";
			object[] array2 = array;
			int num = 4;
			int num2 = (int)this.transport;
			array2[num] = num2.ToString(NumberFormatInfo.InvariantInfo);
			return string.Concat(array);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00088030 File Offset: 0x00087030
		internal EndpointPermission Intersect(EndpointPermission E)
		{
			string text = null;
			TransportType trtype;
			if (this.transport == E.transport)
			{
				trtype = this.transport;
			}
			else if (this.transport == TransportType.All)
			{
				trtype = E.transport;
			}
			else
			{
				if (E.transport != TransportType.All)
				{
					return null;
				}
				trtype = this.transport;
			}
			int num;
			if (this.port == E.port)
			{
				num = this.port;
			}
			else if (this.port == -1)
			{
				num = E.port;
			}
			else
			{
				if (E.port != -1)
				{
					return null;
				}
				num = this.port;
			}
			if (this.Hostname.Equals("0.0.0.0"))
			{
				if (!E.Hostname.Equals("*.*.*.*") && !E.Hostname.Equals("0.0.0.0"))
				{
					return null;
				}
				text = this.Hostname;
			}
			else if (E.Hostname.Equals("0.0.0.0"))
			{
				if (!this.Hostname.Equals("*.*.*.*") && !this.Hostname.Equals("0.0.0.0"))
				{
					return null;
				}
				text = E.Hostname;
			}
			else if (this.IsDns && E.IsDns)
			{
				if (string.Compare(this.hostname, E.hostname, StringComparison.OrdinalIgnoreCase) != 0)
				{
					return null;
				}
				text = this.hostname;
			}
			else
			{
				this.Resolve();
				E.Resolve();
				if ((this.address == null && !this.wildcard) || (E.address == null && !E.wildcard))
				{
					return null;
				}
				if (this.wildcard && E.wildcard)
				{
					string[] array = this.hostname.Split(EndpointPermission.DotSeparator);
					string[] array2 = E.hostname.Split(EndpointPermission.DotSeparator);
					string text2 = "";
					if (array2.Length != 4 || array.Length != 4)
					{
						return null;
					}
					for (int i = 0; i < 4; i++)
					{
						if (i != 0)
						{
							text2 += ".";
						}
						if (array2[i] == array[i])
						{
							text2 += array2[i];
						}
						else if (array2[i] == "*")
						{
							text2 += array[i];
						}
						else
						{
							if (!(array[i] == "*"))
							{
								return null;
							}
							text2 += array2[i];
						}
					}
					text = text2;
				}
				else if (this.wildcard)
				{
					for (int j = 0; j < E.address.Length; j++)
					{
						if (this.MatchWildcard(E.address[j].ToString()))
						{
							text = E.hostname;
							break;
						}
					}
				}
				else if (E.wildcard)
				{
					for (int k = 0; k < this.address.Length; k++)
					{
						if (E.MatchWildcard(this.address[k].ToString()))
						{
							text = this.hostname;
							break;
						}
					}
				}
				else
				{
					if (this.address == E.address)
					{
						text = this.hostname;
					}
					int num2 = 0;
					while (text == null && num2 < this.address.Length)
					{
						for (int l = 0; l < E.address.Length; l++)
						{
							if (this.address[num2].Equals(E.address[l]))
							{
								text = this.hostname;
								break;
							}
						}
						num2++;
					}
				}
				if (text == null)
				{
					return null;
				}
			}
			return new EndpointPermission(text, num, trtype);
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00088378 File Offset: 0x00087378
		private static EndpointPermission.EndPointType CheckEndPointName(string name)
		{
			if (name == null)
			{
				return EndpointPermission.EndPointType.Invalid;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			int i = 0;
			while (i < name.Length)
			{
				char c = name[i];
				char c2 = c;
				if (c2 <= '.')
				{
					if (c2 == '%')
					{
						goto IL_5B;
					}
					switch (c2)
					{
					case '*':
					case '-':
						goto IL_57;
					case '+':
					case ',':
						goto IL_5F;
					case '.':
						break;
					default:
						goto IL_5F;
					}
				}
				else
				{
					if (c2 == ':')
					{
						goto IL_5B;
					}
					if (c2 == '_')
					{
						goto IL_57;
					}
					goto IL_5F;
				}
				IL_A5:
				i++;
				continue;
				IL_57:
				flag2 = true;
				goto IL_A5;
				IL_5B:
				flag = true;
				goto IL_A5;
				IL_5F:
				if ((c > 'f' && c <= 'z') || (c > 'F' && c <= 'Z'))
				{
					flag2 = true;
					goto IL_A5;
				}
				if ((c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
				{
					flag3 = true;
					goto IL_A5;
				}
				if (c < '0' || c > '9')
				{
					return EndpointPermission.EndPointType.Invalid;
				}
				goto IL_A5;
			}
			if (!flag)
			{
				if (flag2)
				{
					return EndpointPermission.EndPointType.DnsOrWildcard;
				}
				if (!flag3)
				{
					return EndpointPermission.EndPointType.IPv4;
				}
				return EndpointPermission.EndPointType.DnsOrWildcard;
			}
			else
			{
				if (!flag2)
				{
					return EndpointPermission.EndPointType.IPv6;
				}
				return EndpointPermission.EndPointType.Invalid;
			}
		}

		// Token: 0x04002214 RID: 8724
		private const string encSeperator = "#";

		// Token: 0x04002215 RID: 8725
		internal string hostname;

		// Token: 0x04002216 RID: 8726
		internal int port;

		// Token: 0x04002217 RID: 8727
		internal TransportType transport;

		// Token: 0x04002218 RID: 8728
		internal bool wildcard;

		// Token: 0x04002219 RID: 8729
		internal IPAddress[] address;

		// Token: 0x0400221A RID: 8730
		internal bool cached;

		// Token: 0x0400221B RID: 8731
		private static char[] DotSeparator = new char[]
		{
			'.'
		};

		// Token: 0x02000441 RID: 1089
		private enum EndPointType
		{
			// Token: 0x0400221D RID: 8733
			Invalid,
			// Token: 0x0400221E RID: 8734
			IPv6,
			// Token: 0x0400221F RID: 8735
			DnsOrWildcard,
			// Token: 0x04002220 RID: 8736
			IPv4
		}
	}
}
