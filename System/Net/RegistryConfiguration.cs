using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x02000426 RID: 1062
	internal static class RegistryConfiguration
	{
		// Token: 0x06002141 RID: 8513 RVA: 0x00083414 File Offset: 0x00082414
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static int GlobalConfigReadInt(string configVariable, int defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetNetFrameworkVersionedPath(), configVariable, RegistryValueKind.DWord);
			if (obj != null)
			{
				return (int)obj;
			}
			return defaultValue;
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x0008343C File Offset: 0x0008243C
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static string GlobalConfigReadString(string configVariable, string defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetNetFrameworkVersionedPath(), configVariable, RegistryValueKind.String);
			if (obj != null)
			{
				return (string)obj;
			}
			return defaultValue;
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00083464 File Offset: 0x00082464
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static int AppConfigReadInt(string configVariable, int defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetAppConfigPath(configVariable), RegistryConfiguration.GetAppConfigValueName(), RegistryValueKind.DWord);
			if (obj != null)
			{
				return (int)obj;
			}
			return defaultValue;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00083490 File Offset: 0x00082490
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework")]
		public static string AppConfigReadString(string configVariable, string defaultValue)
		{
			object obj = RegistryConfiguration.ReadConfig(RegistryConfiguration.GetAppConfigPath(configVariable), RegistryConfiguration.GetAppConfigValueName(), RegistryValueKind.String);
			if (obj != null)
			{
				return (string)obj;
			}
			return defaultValue;
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000834BC File Offset: 0x000824BC
		private static object ReadConfig(string path, string valueName, RegistryValueKind kind)
		{
			object result = null;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path))
				{
					if (registryKey == null)
					{
						return result;
					}
					try
					{
						object value = registryKey.GetValue(valueName, null);
						if (value != null && registryKey.GetValueKind(valueName) == kind)
						{
							result = value;
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
			return result;
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x00083558 File Offset: 0x00082558
		private static string GetNetFrameworkVersionedPath()
		{
			return string.Format(CultureInfo.InvariantCulture, "SOFTWARE\\Microsoft\\.NETFramework\\v{0}", new object[]
			{
				Environment.Version.ToString(3)
			});
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x0008358C File Offset: 0x0008258C
		private static string GetAppConfigPath(string valueName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
			{
				RegistryConfiguration.GetNetFrameworkVersionedPath(),
				valueName
			});
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000835BC File Offset: 0x000825BC
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		private static string GetAppConfigValueName()
		{
			string text = "Unknown";
			Process currentProcess = Process.GetCurrentProcess();
			try
			{
				ProcessModule mainModule = currentProcess.MainModule;
				text = mainModule.FileName;
			}
			catch (NotSupportedException)
			{
			}
			catch (Win32Exception)
			{
			}
			catch (InvalidOperationException)
			{
			}
			try
			{
				text = Path.GetFullPath(text);
			}
			catch (ArgumentException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (PathTooLongException)
			{
			}
			return text;
		}

		// Token: 0x0400216F RID: 8559
		private const string netFrameworkPath = "SOFTWARE\\Microsoft\\.NETFramework";

		// Token: 0x04002170 RID: 8560
		private const string netFrameworkVersionedPath = "SOFTWARE\\Microsoft\\.NETFramework\\v{0}";

		// Token: 0x04002171 RID: 8561
		private const string netFrameworkFullPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\.NETFramework";
	}
}
