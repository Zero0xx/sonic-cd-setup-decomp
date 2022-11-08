using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x020003E5 RID: 997
	internal sealed class HybridWebProxyFinder : IWebProxyFinder, IDisposable
	{
		// Token: 0x0600205C RID: 8284 RVA: 0x0007F9C8 File Offset: 0x0007E9C8
		static HybridWebProxyFinder()
		{
			HybridWebProxyFinder.InitializeFallbackSettings();
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x0007F9CF File Offset: 0x0007E9CF
		public HybridWebProxyFinder(AutoWebProxyScriptEngine engine)
		{
			this.engine = engine;
			this.winHttpFinder = new WinHttpWebProxyFinder(engine);
			this.currentFinder = this.winHttpFinder;
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x0007F9F6 File Offset: 0x0007E9F6
		public bool IsValid
		{
			get
			{
				return this.currentFinder.IsValid;
			}
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x0007FA04 File Offset: 0x0007EA04
		public bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			if (this.currentFinder.GetProxies(destination, out proxyList))
			{
				return true;
			}
			if (HybridWebProxyFinder.allowFallback && this.currentFinder.IsUnrecognizedScheme && this.currentFinder == this.winHttpFinder)
			{
				if (this.netFinder == null)
				{
					this.netFinder = new NetWebProxyFinder(this.engine);
				}
				this.currentFinder = this.netFinder;
				return this.currentFinder.GetProxies(destination, out proxyList);
			}
			return false;
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x0007FA78 File Offset: 0x0007EA78
		public void Abort()
		{
			this.currentFinder.Abort();
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x0007FA85 File Offset: 0x0007EA85
		public void Reset()
		{
			this.winHttpFinder.Reset();
			if (this.netFinder != null)
			{
				this.netFinder.Reset();
			}
			this.currentFinder = this.winHttpFinder;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0007FAB1 File Offset: 0x0007EAB1
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0007FABA File Offset: 0x0007EABA
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.winHttpFinder.Dispose();
				if (this.netFinder != null)
				{
					this.netFinder.Dispose();
				}
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0007FAE0 File Offset: 0x0007EAE0
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		private static void InitializeFallbackSettings()
		{
			HybridWebProxyFinder.allowFallback = false;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework"))
				{
					try
					{
						if (registryKey.GetValueKind("LegacyWPADSupport") == RegistryValueKind.DWord)
						{
							HybridWebProxyFinder.allowFallback = ((int)registryKey.GetValue("LegacyWPADSupport") == 1);
						}
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (IOException)
					{
					}
				}
			}
			catch (SecurityException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x04001FA4 RID: 8100
		private const string allowFallbackKey = "SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04001FA5 RID: 8101
		private const string allowFallbackKeyPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04001FA6 RID: 8102
		private const string allowFallbackValueName = "LegacyWPADSupport";

		// Token: 0x04001FA7 RID: 8103
		private static bool allowFallback;

		// Token: 0x04001FA8 RID: 8104
		private NetWebProxyFinder netFinder;

		// Token: 0x04001FA9 RID: 8105
		private WinHttpWebProxyFinder winHttpFinder;

		// Token: 0x04001FAA RID: 8106
		private BaseWebProxyFinder currentFinder;

		// Token: 0x04001FAB RID: 8107
		private AutoWebProxyScriptEngine engine;
	}
}
