using System;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace System.Security.Util
{
	// Token: 0x02000484 RID: 1156
	internal static class Config
	{
		// Token: 0x06002DC6 RID: 11718 RVA: 0x00099108 File Offset: 0x00098108
		private static void GetFileLocales()
		{
			if (Config.m_machineConfig == null)
			{
				Config.m_machineConfig = Config._GetMachineDirectory();
			}
			if (Config.m_userConfig == null)
			{
				Config.m_userConfig = Config._GetUserDirectory();
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x0009912C File Offset: 0x0009812C
		internal static string MachineDirectory
		{
			get
			{
				Config.GetFileLocales();
				return Config.m_machineConfig;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002DC8 RID: 11720 RVA: 0x00099138 File Offset: 0x00098138
		internal static string UserDirectory
		{
			get
			{
				Config.GetFileLocales();
				return Config.m_userConfig;
			}
		}

		// Token: 0x06002DC9 RID: 11721
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SaveDataByte(string path, byte[] data, int offset, int length);

		// Token: 0x06002DCA RID: 11722
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool RecoverData(ConfigId id);

		// Token: 0x06002DCB RID: 11723
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetQuickCache(ConfigId id, QuickCacheEntryType quickCacheFlags);

		// Token: 0x06002DCC RID: 11724
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetCacheEntry(ConfigId id, int numKey, char[] key, out byte[] data);

		// Token: 0x06002DCD RID: 11725
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddCacheEntry(ConfigId id, int numKey, char[] key, byte[] data);

		// Token: 0x06002DCE RID: 11726
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ResetCacheData(ConfigId id);

		// Token: 0x06002DCF RID: 11727
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string _GetMachineDirectory();

		// Token: 0x06002DD0 RID: 11728
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string _GetUserDirectory();

		// Token: 0x06002DD1 RID: 11729
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool WriteToEventLog(string message);

		// Token: 0x040017A2 RID: 6050
		private static string m_machineConfig;

		// Token: 0x040017A3 RID: 6051
		private static string m_userConfig;
	}
}
