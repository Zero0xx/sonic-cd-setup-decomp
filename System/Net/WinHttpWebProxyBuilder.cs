using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200050B RID: 1291
	internal sealed class WinHttpWebProxyBuilder : WebProxyDataBuilder
	{
		// Token: 0x06002806 RID: 10246 RVA: 0x000A536C File Offset: 0x000A436C
		protected override void BuildInternal()
		{
			UnsafeNclNativeMethods.WinHttp.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG winhttp_CURRENT_USER_IE_PROXY_CONFIG = default(UnsafeNclNativeMethods.WinHttp.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (UnsafeNclNativeMethods.WinHttp.WinHttpGetIEProxyConfigForCurrentUser(ref winhttp_CURRENT_USER_IE_PROXY_CONFIG))
				{
					string addressString = Marshal.PtrToStringUni(winhttp_CURRENT_USER_IE_PROXY_CONFIG.Proxy);
					string bypassListString = Marshal.PtrToStringUni(winhttp_CURRENT_USER_IE_PROXY_CONFIG.ProxyBypass);
					string autoProxyUrl = Marshal.PtrToStringUni(winhttp_CURRENT_USER_IE_PROXY_CONFIG.AutoConfigUrl);
					base.SetProxyAndBypassList(addressString, bypassListString);
					base.SetAutoDetectSettings(winhttp_CURRENT_USER_IE_PROXY_CONFIG.AutoDetect);
					base.SetAutoProxyUrl(autoProxyUrl);
				}
				else
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 8)
					{
						throw new OutOfMemoryException();
					}
					base.SetAutoDetectSettings(true);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(winhttp_CURRENT_USER_IE_PROXY_CONFIG.Proxy);
				Marshal.FreeHGlobal(winhttp_CURRENT_USER_IE_PROXY_CONFIG.ProxyBypass);
				Marshal.FreeHGlobal(winhttp_CURRENT_USER_IE_PROXY_CONFIG.AutoConfigUrl);
			}
		}
	}
}
