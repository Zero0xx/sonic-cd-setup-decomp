using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000536 RID: 1334
	internal class ServiceNameStore
	{
		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x000A9675 File Offset: 0x000A8675
		public ServiceNameCollection ServiceNames
		{
			get
			{
				if (this.serviceNameCollection == null)
				{
					this.serviceNameCollection = new ServiceNameCollection(this.serviceNames);
				}
				return this.serviceNameCollection;
			}
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000A9696 File Offset: 0x000A8696
		public ServiceNameStore()
		{
			this.serviceNames = new List<string>();
			this.serviceNameCollection = null;
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000A96B0 File Offset: 0x000A86B0
		private bool AddSingleServiceName(string spn)
		{
			if (this.Contains(spn))
			{
				return false;
			}
			this.serviceNames.Add(spn);
			return true;
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000A96CC File Offset: 0x000A86CC
		public bool Add(string uriPrefix)
		{
			string[] array = this.BuildServiceNames(uriPrefix);
			bool flag = false;
			foreach (string text in array)
			{
				if (this.AddSingleServiceName(text))
				{
					flag = true;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
						{
							"ServiceNameStore#",
							ValidationHelper.HashString(this),
							"::Add() adding default SPNs '",
							text,
							"' from prefix '",
							uriPrefix,
							"'"
						}));
					}
				}
			}
			if (flag)
			{
				this.serviceNameCollection = null;
			}
			else if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
				{
					"ServiceNameStore#",
					ValidationHelper.HashString(this),
					"::Add() no default SPN added for prefix '",
					uriPrefix,
					"'"
				}));
			}
			return flag;
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x000A97B8 File Offset: 0x000A87B8
		public bool Remove(string uriPrefix)
		{
			string text = this.BuildSimpleServiceName(uriPrefix);
			bool flag = this.Contains(text);
			if (flag)
			{
				this.serviceNames.Remove(text);
				this.serviceNameCollection = null;
			}
			if (Logging.On)
			{
				if (flag)
				{
					Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
					{
						"ServiceNameStore#",
						ValidationHelper.HashString(this),
						"::Remove() removing default SPN '",
						text,
						"' from prefix '",
						uriPrefix,
						"'"
					}));
				}
				else
				{
					Logging.PrintInfo(Logging.HttpListener, string.Concat(new string[]
					{
						"ServiceNameStore#",
						ValidationHelper.HashString(this),
						"::Remove() no default SPN removed for prefix '",
						uriPrefix,
						"'"
					}));
				}
			}
			return flag;
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000A9880 File Offset: 0x000A8880
		private bool Contains(string newServiceName)
		{
			if (newServiceName == null)
			{
				return false;
			}
			bool result = false;
			foreach (string strA in this.serviceNames)
			{
				if (string.Compare(strA, newServiceName, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x000A98E4 File Offset: 0x000A88E4
		public void Clear()
		{
			this.serviceNames.Clear();
			this.serviceNameCollection = null;
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x000A98F8 File Offset: 0x000A88F8
		private string ExtractHostname(string uriPrefix, bool allowInvalidUriStrings)
		{
			if (Uri.IsWellFormedUriString(uriPrefix, UriKind.Absolute))
			{
				Uri uri = new Uri(uriPrefix);
				return uri.Host;
			}
			if (allowInvalidUriStrings)
			{
				int num = uriPrefix.IndexOf("://") + 3;
				int num2 = num;
				bool flag = false;
				while (num2 < uriPrefix.Length && uriPrefix[num2] != '/' && (uriPrefix[num2] != ':' || flag))
				{
					if (uriPrefix[num2] == '[')
					{
						if (flag)
						{
							num2 = num;
							break;
						}
						flag = true;
					}
					if (flag && uriPrefix[num2] == ']')
					{
						flag = false;
					}
					num2++;
				}
				return uriPrefix.Substring(num, num2 - num);
			}
			return null;
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x000A998C File Offset: 0x000A898C
		public string BuildSimpleServiceName(string uriPrefix)
		{
			string text = this.ExtractHostname(uriPrefix, false);
			if (text != null)
			{
				return "HTTP/" + text;
			}
			return null;
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000A99B4 File Offset: 0x000A89B4
		public string[] BuildServiceNames(string uriPrefix)
		{
			string text = this.ExtractHostname(uriPrefix, true);
			IPAddress ipaddress = null;
			if (string.Compare(text, "*", StringComparison.InvariantCultureIgnoreCase) != 0 && string.Compare(text, "+", StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				if (!IPAddress.TryParse(text, out ipaddress))
				{
					goto IL_7D;
				}
			}
			try
			{
				string hostName = Dns.GetHostEntry(string.Empty).HostName;
				return new string[]
				{
					"HTTP/" + hostName
				};
			}
			catch (SocketException)
			{
				return new string[0];
			}
			catch (SecurityException)
			{
				return new string[0];
			}
			IL_7D:
			if (!text.Contains("."))
			{
				try
				{
					string hostName2 = Dns.GetHostEntry(text).HostName;
					return new string[]
					{
						"HTTP/" + text,
						"HTTP/" + hostName2
					};
				}
				catch (SocketException)
				{
					return new string[]
					{
						"HTTP/" + text
					};
				}
				catch (SecurityException)
				{
					return new string[]
					{
						"HTTP/" + text
					};
				}
			}
			return new string[]
			{
				"HTTP/" + text
			};
		}

		// Token: 0x040027B7 RID: 10167
		private List<string> serviceNames;

		// Token: 0x040027B8 RID: 10168
		private ServiceNameCollection serviceNameCollection;
	}
}
