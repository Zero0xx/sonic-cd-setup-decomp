﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Net
{
	// Token: 0x020004AB RID: 1195
	internal sealed class WinHttpWebProxyFinder : BaseWebProxyFinder
	{
		// Token: 0x060024C8 RID: 9416 RVA: 0x00091874 File Offset: 0x00090874
		public WinHttpWebProxyFinder(AutoWebProxyScriptEngine engine) : base(engine)
		{
			this.session = UnsafeNclNativeMethods.WinHttp.WinHttpOpen(null, UnsafeNclNativeMethods.WinHttp.AccessType.NoProxy, null, null, 0);
			if (this.session == null || this.session.IsInvalid)
			{
				int lastWin32Error = WinHttpWebProxyFinder.GetLastWin32Error();
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, string.Format(CultureInfo.InvariantCulture, "Can't open WinHttp session. Error code: {0}.", new object[]
					{
						lastWin32Error
					}));
					return;
				}
			}
			else
			{
				int downloadTimeout = SettingsSectionInternal.Section.DownloadTimeout;
				if (!UnsafeNclNativeMethods.WinHttp.WinHttpSetTimeouts(this.session, downloadTimeout, downloadTimeout, downloadTimeout, downloadTimeout))
				{
					int lastWin32Error2 = WinHttpWebProxyFinder.GetLastWin32Error();
					if (Logging.On)
					{
						Logging.PrintError(Logging.Web, string.Format(CultureInfo.InvariantCulture, "Can't specify proxy discovery timeout. Error code: {0}.", new object[]
						{
							lastWin32Error2
						}));
					}
				}
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00091940 File Offset: 0x00090940
		public override bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			proxyList = null;
			if (this.session == null || this.session.IsInvalid)
			{
				return false;
			}
			if (base.State == BaseWebProxyFinder.AutoWebProxyState.UnrecognizedScheme)
			{
				return false;
			}
			string text = null;
			int num = 12180;
			if (base.Engine.AutomaticallyDetectSettings && !this.autoDetectFailed)
			{
				num = this.GetProxies(destination, null, out text);
				this.autoDetectFailed = WinHttpWebProxyFinder.IsErrorFatalForAutoDetect(num);
				if (num == 12006)
				{
					base.State = BaseWebProxyFinder.AutoWebProxyState.UnrecognizedScheme;
					return false;
				}
			}
			if (base.Engine.AutomaticConfigurationScript != null && WinHttpWebProxyFinder.IsRecoverableAutoProxyError(num))
			{
				num = this.GetProxies(destination, base.Engine.AutomaticConfigurationScript, out text);
			}
			base.State = WinHttpWebProxyFinder.GetStateFromErrorCode(num);
			if (base.State == BaseWebProxyFinder.AutoWebProxyState.Completed)
			{
				if (string.IsNullOrEmpty(text))
				{
					string[] array = new string[1];
					proxyList = array;
				}
				else
				{
					text = WinHttpWebProxyFinder.RemoveWhitespaces(text);
					proxyList = text.Split(new char[]
					{
						';'
					});
				}
				return true;
			}
			return false;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00091A2F File Offset: 0x00090A2F
		public override void Abort()
		{
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00091A31 File Offset: 0x00090A31
		public override void Reset()
		{
			base.Reset();
			this.autoDetectFailed = false;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00091A40 File Offset: 0x00090A40
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.session != null && !this.session.IsInvalid)
			{
				this.session.Close();
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00091A68 File Offset: 0x00090A68
		private int GetProxies(Uri destination, Uri scriptLocation, out string proxyListString)
		{
			int num = 0;
			proxyListString = null;
			UnsafeNclNativeMethods.WinHttp.WINHTTP_AUTOPROXY_OPTIONS winhttp_AUTOPROXY_OPTIONS = default(UnsafeNclNativeMethods.WinHttp.WINHTTP_AUTOPROXY_OPTIONS);
			winhttp_AUTOPROXY_OPTIONS.AutoLogonIfChallenged = false;
			if (scriptLocation == null)
			{
				winhttp_AUTOPROXY_OPTIONS.Flags = UnsafeNclNativeMethods.WinHttp.AutoProxyFlags.AutoDetect;
				winhttp_AUTOPROXY_OPTIONS.AutoConfigUrl = null;
				winhttp_AUTOPROXY_OPTIONS.AutoDetectFlags = (UnsafeNclNativeMethods.WinHttp.AutoDetectType.Dhcp | UnsafeNclNativeMethods.WinHttp.AutoDetectType.DnsA);
			}
			else
			{
				winhttp_AUTOPROXY_OPTIONS.Flags = UnsafeNclNativeMethods.WinHttp.AutoProxyFlags.AutoProxyConfigUrl;
				winhttp_AUTOPROXY_OPTIONS.AutoConfigUrl = scriptLocation.ToString();
				winhttp_AUTOPROXY_OPTIONS.AutoDetectFlags = UnsafeNclNativeMethods.WinHttp.AutoDetectType.None;
			}
			if (!this.WinHttpGetProxyForUrl(destination.ToString(), ref winhttp_AUTOPROXY_OPTIONS, out proxyListString))
			{
				num = WinHttpWebProxyFinder.GetLastWin32Error();
				if (num == 12015 && base.Engine.Credentials != null)
				{
					winhttp_AUTOPROXY_OPTIONS.AutoLogonIfChallenged = true;
					if (!this.WinHttpGetProxyForUrl(destination.ToString(), ref winhttp_AUTOPROXY_OPTIONS, out proxyListString))
					{
						num = WinHttpWebProxyFinder.GetLastWin32Error();
					}
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, string.Format(CultureInfo.InvariantCulture, "Can't retrieve proxy settings for Uri '{0}'. Error code: {1}.", new object[]
					{
						destination,
						num
					}));
				}
			}
			return num;
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00091B4C File Offset: 0x00090B4C
		private bool WinHttpGetProxyForUrl(string destination, ref UnsafeNclNativeMethods.WinHttp.WINHTTP_AUTOPROXY_OPTIONS autoProxyOptions, out string proxyListString)
		{
			proxyListString = null;
			bool flag = false;
			UnsafeNclNativeMethods.WinHttp.WINHTTP_PROXY_INFO winhttp_PROXY_INFO = default(UnsafeNclNativeMethods.WinHttp.WINHTTP_PROXY_INFO);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				flag = UnsafeNclNativeMethods.WinHttp.WinHttpGetProxyForUrl(this.session, destination, ref autoProxyOptions, out winhttp_PROXY_INFO);
				if (flag)
				{
					proxyListString = Marshal.PtrToStringUni(winhttp_PROXY_INFO.Proxy);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(winhttp_PROXY_INFO.Proxy);
				Marshal.FreeHGlobal(winhttp_PROXY_INFO.ProxyBypass);
			}
			return flag;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x00091BB8 File Offset: 0x00090BB8
		private static int GetLastWin32Error()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 8)
			{
				throw new OutOfMemoryException();
			}
			return lastWin32Error;
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x00091BD8 File Offset: 0x00090BD8
		private static bool IsRecoverableAutoProxyError(int errorCode)
		{
			if (errorCode <= 12006)
			{
				if (errorCode != 12002 && errorCode != 12006)
				{
					return false;
				}
			}
			else
			{
				switch (errorCode)
				{
				case 12015:
				case 12017:
					break;
				case 12016:
					return false;
				default:
					switch (errorCode)
					{
					case 12166:
					case 12167:
						break;
					default:
						switch (errorCode)
						{
						case 12178:
						case 12180:
							break;
						case 12179:
							return false;
						default:
							return false;
						}
						break;
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00091C4C File Offset: 0x00090C4C
		private static BaseWebProxyFinder.AutoWebProxyState GetStateFromErrorCode(int errorCode)
		{
			if ((long)errorCode == 0L)
			{
				return BaseWebProxyFinder.AutoWebProxyState.Completed;
			}
			switch (errorCode)
			{
			case 12005:
				break;
			case 12006:
				return BaseWebProxyFinder.AutoWebProxyState.UnrecognizedScheme;
			default:
				switch (errorCode)
				{
				case 12166:
					break;
				case 12167:
					return BaseWebProxyFinder.AutoWebProxyState.DownloadFailure;
				default:
					switch (errorCode)
					{
					case 12178:
						return BaseWebProxyFinder.AutoWebProxyState.Completed;
					case 12180:
						return BaseWebProxyFinder.AutoWebProxyState.DiscoveryFailure;
					}
					return BaseWebProxyFinder.AutoWebProxyState.CompilationFailure;
				}
				break;
			}
			return BaseWebProxyFinder.AutoWebProxyState.Completed;
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x00091CB0 File Offset: 0x00090CB0
		private static string RemoveWhitespaces(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in value)
			{
				if (!char.IsWhiteSpace(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00091CF4 File Offset: 0x00090CF4
		private static bool IsErrorFatalForAutoDetect(int errorCode)
		{
			if (errorCode <= 12005)
			{
				if (errorCode != 0 && errorCode != 12005)
				{
					return true;
				}
			}
			else if (errorCode != 12166 && errorCode != 12178)
			{
				return true;
			}
			return false;
		}

		// Token: 0x040024D0 RID: 9424
		private SafeInternetHandle session;

		// Token: 0x040024D1 RID: 9425
		private bool autoDetectFailed;
	}
}
