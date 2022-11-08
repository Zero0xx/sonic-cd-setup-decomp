using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000472 RID: 1138
	[ComVisible(true)]
	public static class Registry
	{
		// Token: 0x06002D4D RID: 11597 RVA: 0x000971FC File Offset: 0x000961FC
		private static RegistryKey GetBaseKeyFromKeyName(string keyName, out string subKeyName)
		{
			if (keyName == null)
			{
				throw new ArgumentNullException("keyName");
			}
			int num = keyName.IndexOf('\\');
			string text;
			if (num != -1)
			{
				text = keyName.Substring(0, num).ToUpper(CultureInfo.InvariantCulture);
			}
			else
			{
				text = keyName.ToUpper(CultureInfo.InvariantCulture);
			}
			string key;
			if ((key = text) != null)
			{
				if (<PrivateImplementationDetails>{DC94E429-CD79-4112-B209-2F5DDA579560}.$$method0x6002cf5-1 == null)
				{
					<PrivateImplementationDetails>{DC94E429-CD79-4112-B209-2F5DDA579560}.$$method0x6002cf5-1 = new Dictionary<string, int>(7)
					{
						{
							"HKEY_CURRENT_USER",
							0
						},
						{
							"HKEY_LOCAL_MACHINE",
							1
						},
						{
							"HKEY_CLASSES_ROOT",
							2
						},
						{
							"HKEY_USERS",
							3
						},
						{
							"HKEY_PERFORMANCE_DATA",
							4
						},
						{
							"HKEY_CURRENT_CONFIG",
							5
						},
						{
							"HKEY_DYN_DATA",
							6
						}
					};
				}
				int num2;
				if (<PrivateImplementationDetails>{DC94E429-CD79-4112-B209-2F5DDA579560}.$$method0x6002cf5-1.TryGetValue(key, out num2))
				{
					RegistryKey result;
					switch (num2)
					{
					case 0:
						result = Registry.CurrentUser;
						break;
					case 1:
						result = Registry.LocalMachine;
						break;
					case 2:
						result = Registry.ClassesRoot;
						break;
					case 3:
						result = Registry.Users;
						break;
					case 4:
						result = Registry.PerformanceData;
						break;
					case 5:
						result = Registry.CurrentConfig;
						break;
					case 6:
						result = Registry.DynData;
						break;
					default:
						goto IL_11E;
					}
					if (num == -1 || num == keyName.Length)
					{
						subKeyName = string.Empty;
					}
					else
					{
						subKeyName = keyName.Substring(num + 1, keyName.Length - num - 1);
					}
					return result;
				}
			}
			IL_11E:
			throw new ArgumentException(Environment.GetResourceString("Arg_RegInvalidKeyName", new object[]
			{
				"keyName"
			}));
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x00097378 File Offset: 0x00096378
		public static object GetValue(string keyName, string valueName, object defaultValue)
		{
			string name;
			RegistryKey baseKeyFromKeyName = Registry.GetBaseKeyFromKeyName(keyName, out name);
			RegistryKey registryKey = baseKeyFromKeyName.OpenSubKey(name);
			if (registryKey == null)
			{
				return null;
			}
			object value;
			try
			{
				value = registryKey.GetValue(valueName, defaultValue);
			}
			finally
			{
				registryKey.Close();
			}
			return value;
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000973C0 File Offset: 0x000963C0
		public static void SetValue(string keyName, string valueName, object value)
		{
			Registry.SetValue(keyName, valueName, value, RegistryValueKind.Unknown);
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000973CC File Offset: 0x000963CC
		public static void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
		{
			string subkey;
			RegistryKey baseKeyFromKeyName = Registry.GetBaseKeyFromKeyName(keyName, out subkey);
			RegistryKey registryKey = baseKeyFromKeyName.CreateSubKey(subkey);
			try
			{
				registryKey.SetValue(valueName, value, valueKind);
			}
			finally
			{
				registryKey.Close();
			}
		}

		// Token: 0x04001765 RID: 5989
		public static readonly RegistryKey CurrentUser = RegistryKey.GetBaseKey(RegistryKey.HKEY_CURRENT_USER);

		// Token: 0x04001766 RID: 5990
		public static readonly RegistryKey LocalMachine = RegistryKey.GetBaseKey(RegistryKey.HKEY_LOCAL_MACHINE);

		// Token: 0x04001767 RID: 5991
		public static readonly RegistryKey ClassesRoot = RegistryKey.GetBaseKey(RegistryKey.HKEY_CLASSES_ROOT);

		// Token: 0x04001768 RID: 5992
		public static readonly RegistryKey Users = RegistryKey.GetBaseKey(RegistryKey.HKEY_USERS);

		// Token: 0x04001769 RID: 5993
		public static readonly RegistryKey PerformanceData = RegistryKey.GetBaseKey(RegistryKey.HKEY_PERFORMANCE_DATA);

		// Token: 0x0400176A RID: 5994
		public static readonly RegistryKey CurrentConfig = RegistryKey.GetBaseKey(RegistryKey.HKEY_CURRENT_CONFIG);

		// Token: 0x0400176B RID: 5995
		public static readonly RegistryKey DynData = RegistryKey.GetBaseKey(RegistryKey.HKEY_DYN_DATA);
	}
}
