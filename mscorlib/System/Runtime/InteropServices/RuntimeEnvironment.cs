using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000519 RID: 1305
	[ComVisible(true)]
	public class RuntimeEnvironment
	{
		// Token: 0x060032B0 RID: 12976
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetModuleFileName();

		// Token: 0x060032B1 RID: 12977
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetDeveloperPath();

		// Token: 0x060032B2 RID: 12978
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetHostBindingFile();

		// Token: 0x060032B3 RID: 12979
		[DllImport("mscoree.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern int GetCORVersion(StringBuilder sb, int BufferLength, ref int retLength);

		// Token: 0x060032B4 RID: 12980
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool FromGlobalAccessCache(Assembly a);

		// Token: 0x060032B5 RID: 12981 RVA: 0x000AB4CC File Offset: 0x000AA4CC
		public static string GetSystemVersion()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			int num = 0;
			if (RuntimeEnvironment.GetCORVersion(stringBuilder, 256, ref num) == 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000AB500 File Offset: 0x000AA500
		public static string GetRuntimeDirectory()
		{
			string runtimeDirectoryImpl = RuntimeEnvironment.GetRuntimeDirectoryImpl();
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, runtimeDirectoryImpl).Demand();
			return runtimeDirectoryImpl;
		}

		// Token: 0x060032B7 RID: 12983
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetRuntimeDirectoryImpl();

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x060032B8 RID: 12984 RVA: 0x000AB520 File Offset: 0x000AA520
		public static string SystemConfigurationFile
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				stringBuilder.Append(RuntimeEnvironment.GetRuntimeDirectory());
				stringBuilder.Append(AppDomainSetup.RuntimeConfigurationFile);
				string text = stringBuilder.ToString();
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				return text;
			}
		}
	}
}
