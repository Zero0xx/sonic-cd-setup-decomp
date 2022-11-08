using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x02000509 RID: 1289
	internal class RegBlobWebProxyDataBuilder : WebProxyDataBuilder
	{
		// Token: 0x06002801 RID: 10241 RVA: 0x000A50DE File Offset: 0x000A40DE
		public RegBlobWebProxyDataBuilder(string connectoid, SafeRegistryHandle registry)
		{
			this.m_Registry = registry;
			this.m_Connectoid = connectoid;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000A50F4 File Offset: 0x000A40F4
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings")]
		private bool ReadRegSettings()
		{
			SafeRegistryHandle safeRegistryHandle = null;
			RegistryKey registryKey = null;
			try
			{
				bool flag = true;
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
				if (registryKey != null)
				{
					object value = registryKey.GetValue("ProxySettingsPerUser");
					if (value != null && value.GetType() == typeof(int) && (int)value == 0)
					{
						flag = false;
					}
				}
				uint num;
				if (flag)
				{
					if (this.m_Registry != null)
					{
						num = this.m_Registry.RegOpenKeyEx("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections", 0U, 131097U, out safeRegistryHandle);
					}
					else
					{
						num = 1168U;
					}
				}
				else
				{
					num = SafeRegistryHandle.RegOpenKeyEx(UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections", 0U, 131097U, out safeRegistryHandle);
				}
				if (num != 0U)
				{
					safeRegistryHandle = null;
				}
				object obj;
				if (safeRegistryHandle != null && safeRegistryHandle.QueryValue((this.m_Connectoid != null) ? this.m_Connectoid : "DefaultConnectionSettings", out obj) == 0U)
				{
					this.m_RegistryBytes = (byte[])obj;
				}
			}
			catch (Exception exception)
			{
				if (NclUtilities.IsFatal(exception))
				{
					throw;
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (safeRegistryHandle != null)
				{
					safeRegistryHandle.RegCloseKey();
				}
			}
			return this.m_RegistryBytes != null;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000A5218 File Offset: 0x000A4218
		public string ReadString()
		{
			string result = null;
			int num = this.ReadInt32();
			if (num > 0)
			{
				int num2 = this.m_RegistryBytes.Length - this.m_ByteOffset;
				if (num >= num2)
				{
					num = num2;
				}
				result = Encoding.UTF8.GetString(this.m_RegistryBytes, this.m_ByteOffset, num);
				this.m_ByteOffset += num;
			}
			return result;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000A5270 File Offset: 0x000A4270
		internal unsafe int ReadInt32()
		{
			int result = 0;
			int num = this.m_RegistryBytes.Length - this.m_ByteOffset;
			if (num >= 4)
			{
				fixed (byte* registryBytes = this.m_RegistryBytes)
				{
					if (sizeof(IntPtr) == 4)
					{
						result = ((int*)registryBytes)[this.m_ByteOffset / 4];
					}
					else
					{
						result = Marshal.ReadInt32((IntPtr)((void*)registryBytes), this.m_ByteOffset);
					}
				}
				this.m_ByteOffset += 4;
			}
			return result;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000A52EC File Offset: 0x000A42EC
		protected override void BuildInternal()
		{
			bool flag = this.ReadRegSettings();
			if (flag)
			{
				flag = (this.ReadInt32() >= 60);
			}
			if (!flag)
			{
				base.SetAutoDetectSettings(true);
				return;
			}
			this.ReadInt32();
			RegBlobWebProxyDataBuilder.ProxyTypeFlags proxyTypeFlags = (RegBlobWebProxyDataBuilder.ProxyTypeFlags)this.ReadInt32();
			string addressString = this.ReadString();
			string bypassListString = this.ReadString();
			if ((proxyTypeFlags & RegBlobWebProxyDataBuilder.ProxyTypeFlags.PROXY_TYPE_PROXY) != (RegBlobWebProxyDataBuilder.ProxyTypeFlags)0)
			{
				base.SetProxyAndBypassList(addressString, bypassListString);
			}
			base.SetAutoDetectSettings((proxyTypeFlags & RegBlobWebProxyDataBuilder.ProxyTypeFlags.PROXY_TYPE_AUTO_DETECT) != (RegBlobWebProxyDataBuilder.ProxyTypeFlags)0);
			string autoProxyUrl = this.ReadString();
			if ((proxyTypeFlags & RegBlobWebProxyDataBuilder.ProxyTypeFlags.PROXY_TYPE_AUTO_PROXY_URL) != (RegBlobWebProxyDataBuilder.ProxyTypeFlags)0)
			{
				base.SetAutoProxyUrl(autoProxyUrl);
			}
		}

		// Token: 0x04002749 RID: 10057
		internal const string PolicyKey = "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";

		// Token: 0x0400274A RID: 10058
		internal const string ProxyKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections";

		// Token: 0x0400274B RID: 10059
		private const string DefaultConnectionSettings = "DefaultConnectionSettings";

		// Token: 0x0400274C RID: 10060
		private const string ProxySettingsPerUser = "ProxySettingsPerUser";

		// Token: 0x0400274D RID: 10061
		private const int IE50StrucSize = 60;

		// Token: 0x0400274E RID: 10062
		private byte[] m_RegistryBytes;

		// Token: 0x0400274F RID: 10063
		private int m_ByteOffset;

		// Token: 0x04002750 RID: 10064
		private string m_Connectoid;

		// Token: 0x04002751 RID: 10065
		private SafeRegistryHandle m_Registry;

		// Token: 0x0200050A RID: 1290
		[Flags]
		private enum ProxyTypeFlags
		{
			// Token: 0x04002753 RID: 10067
			PROXY_TYPE_DIRECT = 1,
			// Token: 0x04002754 RID: 10068
			PROXY_TYPE_PROXY = 2,
			// Token: 0x04002755 RID: 10069
			PROXY_TYPE_AUTO_PROXY_URL = 4,
			// Token: 0x04002756 RID: 10070
			PROXY_TYPE_AUTO_DETECT = 8
		}
	}
}
