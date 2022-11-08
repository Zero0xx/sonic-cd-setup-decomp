using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System
{
	// Token: 0x020000B2 RID: 178
	[ComVisible(true)]
	public static class Environment
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0001FC24 File Offset: 0x0001EC24
		private static object InternalSyncObject
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				if (Environment.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref Environment.s_InternalSyncObject, value, null);
				}
				return Environment.s_InternalSyncObject;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0001FC50 File Offset: 0x0001EC50
		public static int TickCount
		{
			get
			{
				return Environment.nativeGetTickCount();
			}
		}

		// Token: 0x06000A6C RID: 2668
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeGetTickCount();

		// Token: 0x06000A6D RID: 2669
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExitNative(int exitCode);

		// Token: 0x06000A6E RID: 2670 RVA: 0x0001FC57 File Offset: 0x0001EC57
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Exit(int exitCode)
		{
			Environment.ExitNative(exitCode);
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0001FC5F File Offset: 0x0001EC5F
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x0001FC66 File Offset: 0x0001EC66
		public static int ExitCode
		{
			get
			{
				return Environment.nativeGetExitCode();
			}
			set
			{
				Environment.nativeSetExitCode(value);
			}
		}

		// Token: 0x06000A71 RID: 2673
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void nativeSetExitCode(int exitCode);

		// Token: 0x06000A72 RID: 2674
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeGetExitCode();

		// Token: 0x06000A73 RID: 2675
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FailFast(string message);

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0001FC6E File Offset: 0x0001EC6E
		public static string CommandLine
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "Path").Demand();
				return Environment.GetCommandLineNative();
			}
		}

		// Token: 0x06000A75 RID: 2677
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetCommandLineNative();

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0001FC85 File Offset: 0x0001EC85
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x0001FC8C File Offset: 0x0001EC8C
		public static string CurrentDirectory
		{
			get
			{
				return Directory.GetCurrentDirectory();
			}
			set
			{
				Directory.SetCurrentDirectory(value);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0001FC94 File Offset: 0x0001EC94
		public static string SystemDirectory
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				if (Win32Native.GetSystemDirectory(stringBuilder, 260) == 0)
				{
					__Error.WinIOError();
				}
				string text = stringBuilder.ToString();
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				return text;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0001FCD4 File Offset: 0x0001ECD4
		internal static string InternalWindowsDirectory
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				if (Win32Native.GetWindowsDirectory(stringBuilder, 260) == 0)
				{
					__Error.WinIOError();
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0001FD08 File Offset: 0x0001ED08
		public static string ExpandEnvironmentVariables(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				return name;
			}
			string[] array = name.Split(new char[]
			{
				'%'
			});
			StringBuilder stringBuilder = new StringBuilder();
			int num = 100;
			StringBuilder stringBuilder2 = new StringBuilder(num);
			int j;
			for (int i = 1; i < array.Length - 1; i++)
			{
				if (array[i].Length != 0)
				{
					stringBuilder2.Length = 0;
					string text = "%" + array[i] + "%";
					j = Win32Native.ExpandEnvironmentStrings(text, stringBuilder2, num);
					if (j == 0)
					{
						Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
					}
					while (j > num)
					{
						num = j;
						stringBuilder2.Capacity = num;
						stringBuilder2.Length = 0;
						j = Win32Native.ExpandEnvironmentStrings(text, stringBuilder2, num);
						if (j == 0)
						{
							Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
						}
					}
					string a = stringBuilder2.ToString();
					if (a != text)
					{
						stringBuilder.Append(array[i]);
						stringBuilder.Append(';');
					}
				}
			}
			new EnvironmentPermission(EnvironmentPermissionAccess.Read, stringBuilder.ToString()).Demand();
			stringBuilder2.Length = 0;
			j = Win32Native.ExpandEnvironmentStrings(name, stringBuilder2, num);
			if (j == 0)
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
			while (j > num)
			{
				num = j;
				stringBuilder2.Capacity = num;
				stringBuilder2.Length = 0;
				j = Win32Native.ExpandEnvironmentStrings(name, stringBuilder2, num);
				if (j == 0)
				{
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
			}
			return stringBuilder2.ToString();
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0001FE70 File Offset: 0x0001EE70
		public static string MachineName
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "COMPUTERNAME").Demand();
				StringBuilder stringBuilder = new StringBuilder(256);
				int num = 256;
				if (Win32Native.GetComputerName(stringBuilder, ref num) == 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ComputerName"));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0001FEC0 File Offset: 0x0001EEC0
		public static int ProcessorCount
		{
			get
			{
				Win32Native.SYSTEM_INFO system_INFO = default(Win32Native.SYSTEM_INFO);
				Win32Native.GetSystemInfo(ref system_INFO);
				return system_INFO.dwNumberOfProcessors;
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001FEE3 File Offset: 0x0001EEE3
		public static string[] GetCommandLineArgs()
		{
			new EnvironmentPermission(EnvironmentPermissionAccess.Read, "Path").Demand();
			return Environment.GetCommandLineArgsNative();
		}

		// Token: 0x06000A7E RID: 2686
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetCommandLineArgsNative();

		// Token: 0x06000A7F RID: 2687
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetEnvironmentVariable(string variable);

		// Token: 0x06000A80 RID: 2688 RVA: 0x0001FEFC File Offset: 0x0001EEFC
		public static string GetEnvironmentVariable(string variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			new EnvironmentPermission(EnvironmentPermissionAccess.Read, variable).Demand();
			StringBuilder stringBuilder = new StringBuilder(128);
			int i = Win32Native.GetEnvironmentVariable(variable, stringBuilder, stringBuilder.Capacity);
			if (i == 0 && Marshal.GetLastWin32Error() == 203)
			{
				return null;
			}
			while (i > stringBuilder.Capacity)
			{
				stringBuilder.Capacity = i;
				stringBuilder.Length = 0;
				i = Win32Native.GetEnvironmentVariable(variable, stringBuilder, stringBuilder.Capacity);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0001FF78 File Offset: 0x0001EF78
		public static string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
		{
			if (target == EnvironmentVariableTarget.Process)
			{
				return Environment.GetEnvironmentVariable(variable);
			}
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (Environment.IsWin9X())
			{
				throw new NotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			if (target == EnvironmentVariableTarget.Machine)
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Session Manager\\Environment", false))
				{
					if (registryKey == null)
					{
						return null;
					}
					return registryKey.GetValue(variable) as string;
				}
			}
			if (target == EnvironmentVariableTarget.User)
			{
				using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Environment", false))
				{
					if (registryKey2 == null)
					{
						return null;
					}
					return registryKey2.GetValue(variable) as string;
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[]
			{
				(int)target
			}));
		}

		// Token: 0x06000A82 RID: 2690
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern char[] nativeGetEnvironmentCharArray();

		// Token: 0x06000A83 RID: 2691 RVA: 0x00020080 File Offset: 0x0001F080
		public static IDictionary GetEnvironmentVariables()
		{
			char[] array = Environment.nativeGetEnvironmentCharArray();
			if (array == null)
			{
				throw new OutOfMemoryException();
			}
			Hashtable hashtable = new Hashtable(20);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			for (int i = 0; i < array.Length; i++)
			{
				int num = i;
				while (array[i] != '=' && array[i] != '\0')
				{
					i++;
				}
				if (array[i] != '\0')
				{
					if (i - num == 0)
					{
						while (array[i] != '\0')
						{
							i++;
						}
					}
					else
					{
						string text = new string(array, num, i - num);
						i++;
						int num2 = i;
						while (array[i] != '\0')
						{
							i++;
						}
						string value = new string(array, num2, i - num2);
						hashtable[text] = value;
						if (flag)
						{
							flag = false;
						}
						else
						{
							stringBuilder.Append(';');
						}
						stringBuilder.Append(text);
					}
				}
			}
			new EnvironmentPermission(EnvironmentPermissionAccess.Read, stringBuilder.ToString()).Demand();
			return hashtable;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00020168 File Offset: 0x0001F168
		internal static IDictionary GetRegistryKeyNameValuePairs(RegistryKey registryKey)
		{
			Hashtable hashtable = new Hashtable(20);
			if (registryKey != null)
			{
				string[] valueNames = registryKey.GetValueNames();
				foreach (string text in valueNames)
				{
					string value = registryKey.GetValue(text, "").ToString();
					hashtable.Add(text, value);
				}
			}
			return hashtable;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000201C0 File Offset: 0x0001F1C0
		public static IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target)
		{
			if (target == EnvironmentVariableTarget.Process)
			{
				return Environment.GetEnvironmentVariables();
			}
			if (Environment.IsWin9X())
			{
				throw new NotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			if (target == EnvironmentVariableTarget.Machine)
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Session Manager\\Environment", false))
				{
					return Environment.GetRegistryKeyNameValuePairs(registryKey);
				}
			}
			if (target == EnvironmentVariableTarget.User)
			{
				using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Environment", false))
				{
					return Environment.GetRegistryKeyNameValuePairs(registryKey2);
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[]
			{
				(int)target
			}));
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00020290 File Offset: 0x0001F290
		public static void SetEnvironmentVariable(string variable, string value)
		{
			Environment.CheckEnvironmentVariableName(variable);
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			if (string.IsNullOrEmpty(value) || value[0] == '\0')
			{
				value = null;
			}
			else if (value.Length >= 32767)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_LongEnvVarValue"));
			}
			if (Win32Native.SetEnvironmentVariable(variable, value))
			{
				return;
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 203)
			{
				return;
			}
			if (lastWin32Error == 206)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_LongEnvVarValue"));
			}
			throw new ArgumentException(Win32Native.GetMessage(lastWin32Error));
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00020320 File Offset: 0x0001F320
		private static void CheckEnvironmentVariableName(string variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (variable.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StringZeroLength"), "variable");
			}
			if (variable[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StringFirstCharIsZero"), "variable");
			}
			if (variable.Length >= 32767)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_LongEnvVarValue"));
			}
			if (variable.IndexOf('=') != -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalEnvVarName"));
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000203B0 File Offset: 0x0001F3B0
		public static void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
		{
			if (target == EnvironmentVariableTarget.Process)
			{
				Environment.SetEnvironmentVariable(variable, value);
				return;
			}
			Environment.CheckEnvironmentVariableName(variable);
			if (variable.Length >= 255)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_LongEnvVarName"));
			}
			if (Environment.IsWin9X())
			{
				throw new NotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			if (string.IsNullOrEmpty(value) || value[0] == '\0')
			{
				value = null;
			}
			if (target == EnvironmentVariableTarget.Machine)
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Session Manager\\Environment", true))
				{
					if (registryKey != null)
					{
						if (value == null)
						{
							registryKey.DeleteValue(variable, false);
						}
						else
						{
							registryKey.SetValue(variable, value);
						}
					}
					goto IL_101;
				}
			}
			if (target == EnvironmentVariableTarget.User)
			{
				using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Environment", true))
				{
					if (registryKey2 != null)
					{
						if (value == null)
						{
							registryKey2.DeleteValue(variable, false);
						}
						else
						{
							registryKey2.SetValue(variable, value);
						}
					}
					goto IL_101;
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[]
			{
				(int)target
			}));
			IL_101:
			IntPtr value2 = Win32Native.SendMessageTimeout(new IntPtr(65535), 26, IntPtr.Zero, "Environment", 0U, 1000U, IntPtr.Zero);
			value2 == IntPtr.Zero;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00020518 File Offset: 0x0001F518
		public static string[] GetLogicalDrives()
		{
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			int logicalDrives = Win32Native.GetLogicalDrives();
			if (logicalDrives == 0)
			{
				__Error.WinIOError();
			}
			uint num = (uint)logicalDrives;
			int num2 = 0;
			while (num != 0U)
			{
				if ((num & 1U) != 0U)
				{
					num2++;
				}
				num >>= 1;
			}
			string[] array = new string[num2];
			char[] array2 = new char[]
			{
				'A',
				':',
				'\\'
			};
			num = (uint)logicalDrives;
			num2 = 0;
			while (num != 0U)
			{
				if ((num & 1U) != 0U)
				{
					array[num2++] = new string(array2);
				}
				num >>= 1;
				char[] array3 = array2;
				int num3 = 0;
				array3[num3] += '\u0001';
			}
			return array;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x000205A5 File Offset: 0x0001F5A5
		public static string NewLine
		{
			get
			{
				return "\r\n";
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x000205AC File Offset: 0x0001F5AC
		public static Version Version
		{
			get
			{
				return new Version("2.0.50727.9164");
			}
		}

		// Token: 0x06000A8C RID: 2700
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long nativeGetWorkingSet();

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000205B8 File Offset: 0x0001F5B8
		public static long WorkingSet
		{
			get
			{
				new EnvironmentPermission(PermissionState.Unrestricted).Demand();
				return Environment.nativeGetWorkingSet();
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000205CC File Offset: 0x0001F5CC
		public static OperatingSystem OSVersion
		{
			get
			{
				if (Environment.m_os == null)
				{
					Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
					if (!Win32Native.GetVersionEx(osversioninfo))
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GetVersion"));
					}
					Win32Native.OSVERSIONINFOEX osversioninfoex = new Win32Native.OSVERSIONINFOEX();
					if (osversioninfo.PlatformId != 1 && !Win32Native.GetVersionEx(osversioninfoex))
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GetVersion"));
					}
					PlatformID platform;
					switch (osversioninfo.PlatformId)
					{
					case 0:
						platform = PlatformID.Win32S;
						break;
					case 1:
						platform = PlatformID.Win32Windows;
						break;
					case 2:
						platform = PlatformID.Win32NT;
						break;
					case 3:
						platform = PlatformID.WinCE;
						break;
					default:
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InvalidPlatformID"));
					}
					Version version = new Version(osversioninfo.MajorVersion, osversioninfo.MinorVersion, osversioninfo.BuildNumber, (int)osversioninfoex.ServicePackMajor << 16 | (int)osversioninfoex.ServicePackMinor);
					Environment.m_os = new OperatingSystem(platform, version, osversioninfo.CSDVersion);
				}
				return Environment.m_os;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x000206A8 File Offset: 0x0001F6A8
		internal static bool IsW2k3
		{
			get
			{
				if (!Environment.s_CheckedOSW2k3)
				{
					OperatingSystem osversion = Environment.OSVersion;
					Environment.s_IsW2k3 = (osversion.Platform == PlatformID.Win32NT && osversion.Version.Major == 5 && osversion.Version.Minor == 2);
					Environment.s_CheckedOSW2k3 = true;
				}
				return Environment.s_IsW2k3;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x000206FE File Offset: 0x0001F6FE
		internal static bool RunningOnWinNT
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00020710 File Offset: 0x0001F710
		internal static Environment.OSName OSInfo
		{
			get
			{
				if (Environment.m_osname == Environment.OSName.Invalid)
				{
					lock (Environment.InternalSyncObject)
					{
						if (Environment.m_osname == Environment.OSName.Invalid)
						{
							Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
							if (!Win32Native.GetVersionEx(osversioninfo))
							{
								throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GetVersion"));
							}
							switch (osversioninfo.PlatformId)
							{
							case 1:
								switch (osversioninfo.MajorVersion)
								{
								case 4:
									if (osversioninfo.MinorVersion == 0)
									{
										Environment.m_osname = Environment.OSName.Win95;
									}
									else
									{
										Environment.m_osname = Environment.OSName.Win98;
									}
									break;
								case 5:
									Environment.m_osname = Environment.OSName.WinMe;
									break;
								default:
									Environment.m_osname = Environment.OSName.Win9x;
									break;
								}
								break;
							case 2:
								switch (osversioninfo.MajorVersion)
								{
								case 4:
									Environment.m_osname = Environment.OSName.Nt4;
									break;
								case 5:
									Environment.m_osname = Environment.OSName.Win2k;
									break;
								default:
									Environment.m_osname = Environment.OSName.WinNT;
									break;
								}
								break;
							default:
								Environment.m_osname = Environment.OSName.Unknown;
								break;
							}
						}
					}
				}
				return Environment.m_osname;
			}
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00020824 File Offset: 0x0001F824
		internal static bool IsWin9X()
		{
			return Environment.OSVersion.Platform == PlatformID.Win32Windows;
		}

		// Token: 0x06000A93 RID: 2707
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeIsWin9x();

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00020833 File Offset: 0x0001F833
		public static string StackTrace
		{
			get
			{
				new EnvironmentPermission(PermissionState.Unrestricted).Demand();
				return Environment.GetStackTrace(null, true);
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00020848 File Offset: 0x0001F848
		internal static string GetStackTrace(Exception e, bool needFileInfo)
		{
			StackTrace stackTrace;
			if (e == null)
			{
				stackTrace = new StackTrace(needFileInfo);
			}
			else
			{
				stackTrace = new StackTrace(e, needFileInfo);
			}
			return stackTrace.ToString(System.Diagnostics.StackTrace.TraceFormat.Normal);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00020870 File Offset: 0x0001F870
		private static void InitResourceHelper()
		{
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					Thread.BeginCriticalRegion();
					flag = true;
					Monitor.Enter(Environment.InternalSyncObject);
					flag2 = true;
				}
				if (Environment.m_resHelper == null)
				{
					Environment.ResourceHelper resHelper = new Environment.ResourceHelper();
					Thread.MemoryBarrier();
					Environment.m_resHelper = resHelper;
				}
			}
			finally
			{
				if (flag2)
				{
					Monitor.Exit(Environment.InternalSyncObject);
				}
				if (flag)
				{
					Thread.EndCriticalRegion();
				}
			}
		}

		// Token: 0x06000A97 RID: 2711
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetResourceFromDefault(string key);

		// Token: 0x06000A98 RID: 2712 RVA: 0x000208F0 File Offset: 0x0001F8F0
		internal static string GetResourceStringLocal(string key)
		{
			if (Environment.m_resHelper == null)
			{
				Environment.InitResourceHelper();
			}
			return Environment.m_resHelper.GetResourceString(key);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00020909 File Offset: 0x0001F909
		internal static string GetResourceString(string key)
		{
			return Environment.GetResourceFromDefault(key);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00020914 File Offset: 0x0001F914
		internal static string GetResourceString(string key, params object[] values)
		{
			string resourceFromDefault = Environment.GetResourceFromDefault(key);
			return string.Format(CultureInfo.CurrentCulture, resourceFromDefault, values);
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x00020934 File Offset: 0x0001F934
		public static bool HasShutdownStarted
		{
			get
			{
				return Environment.nativeHasShutdownStarted();
			}
		}

		// Token: 0x06000A9C RID: 2716
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeHasShutdownStarted();

		// Token: 0x06000A9D RID: 2717
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetCompatibilityFlag(CompatibilityFlag flag);

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0002093C File Offset: 0x0001F93C
		public static string UserName
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "UserName").Demand();
				StringBuilder stringBuilder = new StringBuilder(256);
				int capacity = stringBuilder.Capacity;
				Win32Native.GetUserName(stringBuilder, ref capacity);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0002097C File Offset: 0x0001F97C
		public static bool UserInteractive
		{
			get
			{
				if ((Environment.OSInfo & Environment.OSName.WinNT) == Environment.OSName.WinNT)
				{
					IntPtr processWindowStation = Win32Native.GetProcessWindowStation();
					if (processWindowStation != IntPtr.Zero && Environment.processWinStation != processWindowStation)
					{
						int num = 0;
						Win32Native.USEROBJECTFLAGS userobjectflags = new Win32Native.USEROBJECTFLAGS();
						if (Win32Native.GetUserObjectInformation(processWindowStation, 1, userobjectflags, Marshal.SizeOf(userobjectflags), ref num) && (userobjectflags.dwFlags & 1) == 0)
						{
							Environment.isUserNonInteractive = true;
						}
						Environment.processWinStation = processWindowStation;
					}
				}
				return !Environment.isUserNonInteractive;
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000209F4 File Offset: 0x0001F9F4
		public static string GetFolderPath(Environment.SpecialFolder folder)
		{
			if (!Enum.IsDefined(typeof(Environment.SpecialFolder), folder))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[]
				{
					(int)folder
				}));
			}
			StringBuilder stringBuilder = new StringBuilder(260);
			Win32Native.SHGetFolderPath(IntPtr.Zero, (int)folder, IntPtr.Zero, 0, stringBuilder);
			string text = stringBuilder.ToString();
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
			return text;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00020A78 File Offset: 0x0001FA78
		public static string UserDomainName
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "UserDomain").Demand();
				byte[] array = new byte[1024];
				int num = array.Length;
				StringBuilder stringBuilder = new StringBuilder(1024);
				int capacity = stringBuilder.Capacity;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					byte userNameEx = Win32Native.GetUserNameEx(2, stringBuilder, ref capacity);
					if (userNameEx == 1)
					{
						string text = stringBuilder.ToString();
						int num2 = text.IndexOf('\\');
						if (num2 != -1)
						{
							return text.Substring(0, num2);
						}
					}
					capacity = stringBuilder.Capacity;
				}
				int num3;
				if (Win32Native.LookupAccountName(null, Environment.UserName, array, ref num, stringBuilder, ref capacity, out num3))
				{
					return stringBuilder.ToString();
				}
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 120)
				{
					throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UserDomainName"));
			}
		}

		// Token: 0x040003CB RID: 971
		private const int MaximumLength = 32767;

		// Token: 0x040003CC RID: 972
		private const int MaxMachineNameLength = 256;

		// Token: 0x040003CD RID: 973
		private static Environment.ResourceHelper m_resHelper;

		// Token: 0x040003CE RID: 974
		private static bool s_IsW2k3;

		// Token: 0x040003CF RID: 975
		private static volatile bool s_CheckedOSW2k3;

		// Token: 0x040003D0 RID: 976
		private static object s_InternalSyncObject;

		// Token: 0x040003D1 RID: 977
		private static OperatingSystem m_os;

		// Token: 0x040003D2 RID: 978
		private static Environment.OSName m_osname;

		// Token: 0x040003D3 RID: 979
		private static IntPtr processWinStation;

		// Token: 0x040003D4 RID: 980
		private static bool isUserNonInteractive;

		// Token: 0x020000B3 RID: 179
		internal sealed class ResourceHelper
		{
			// Token: 0x06000AA2 RID: 2722 RVA: 0x00020B4C File Offset: 0x0001FB4C
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			internal string GetResourceString(string key)
			{
				if (key == null || key.Length == 0)
				{
					return "[Resource lookup failed - null or empty resource name]";
				}
				Environment.ResourceHelper.GetResourceStringUserData getResourceStringUserData = new Environment.ResourceHelper.GetResourceStringUserData(this, key);
				RuntimeHelpers.TryCode code = new RuntimeHelpers.TryCode(this.GetResourceStringCode);
				RuntimeHelpers.CleanupCode backoutCode = new RuntimeHelpers.CleanupCode(this.GetResourceStringBackoutCode);
				RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(code, backoutCode, getResourceStringUserData);
				return getResourceStringUserData.m_retVal;
			}

			// Token: 0x06000AA3 RID: 2723 RVA: 0x00020B9C File Offset: 0x0001FB9C
			private void GetResourceStringCode(object userDataIn)
			{
				Environment.ResourceHelper.GetResourceStringUserData getResourceStringUserData = (Environment.ResourceHelper.GetResourceStringUserData)userDataIn;
				Environment.ResourceHelper resourceHelper = getResourceStringUserData.m_resourceHelper;
				string key = getResourceStringUserData.m_key;
				Monitor.ReliableEnter(resourceHelper, ref getResourceStringUserData.m_lockWasTaken);
				if (resourceHelper.currentlyLoading != null && resourceHelper.currentlyLoading.Count > 0 && resourceHelper.currentlyLoading.Contains(key))
				{
					try
					{
						StackTrace stackTrace = new StackTrace(true);
						stackTrace.ToString(System.Diagnostics.StackTrace.TraceFormat.NoResourceLookup);
					}
					catch (StackOverflowException)
					{
					}
					catch (NullReferenceException)
					{
					}
					catch (OutOfMemoryException)
					{
					}
					getResourceStringUserData.m_retVal = "[Resource lookup failed - infinite recursion or critical failure detected.]";
					return;
				}
				if (resourceHelper.currentlyLoading == null)
				{
					resourceHelper.currentlyLoading = new Stack(4);
				}
				if (!resourceHelper.resourceManagerInited)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						RuntimeHelpers.RunClassConstructor(typeof(ResourceManager).TypeHandle);
						RuntimeHelpers.RunClassConstructor(typeof(ResourceReader).TypeHandle);
						RuntimeHelpers.RunClassConstructor(typeof(RuntimeResourceSet).TypeHandle);
						RuntimeHelpers.RunClassConstructor(typeof(BinaryReader).TypeHandle);
						resourceHelper.resourceManagerInited = true;
					}
				}
				resourceHelper.currentlyLoading.Push(key);
				if (resourceHelper.SystemResMgr == null)
				{
					resourceHelper.SystemResMgr = new ResourceManager("mscorlib", typeof(object).Assembly);
				}
				string @string = resourceHelper.SystemResMgr.GetString(key, null);
				resourceHelper.currentlyLoading.Pop();
				getResourceStringUserData.m_retVal = @string;
			}

			// Token: 0x06000AA4 RID: 2724 RVA: 0x00020D1C File Offset: 0x0001FD1C
			[PrePrepareMethod]
			private void GetResourceStringBackoutCode(object userDataIn, bool exceptionThrown)
			{
				Environment.ResourceHelper.GetResourceStringUserData getResourceStringUserData = (Environment.ResourceHelper.GetResourceStringUserData)userDataIn;
				Environment.ResourceHelper resourceHelper = getResourceStringUserData.m_resourceHelper;
				if (exceptionThrown && getResourceStringUserData.m_lockWasTaken)
				{
					resourceHelper.SystemResMgr = null;
					resourceHelper.currentlyLoading = null;
				}
				if (getResourceStringUserData.m_lockWasTaken)
				{
					Monitor.Exit(resourceHelper);
				}
			}

			// Token: 0x040003D5 RID: 981
			private ResourceManager SystemResMgr;

			// Token: 0x040003D6 RID: 982
			private Stack currentlyLoading;

			// Token: 0x040003D7 RID: 983
			internal bool resourceManagerInited;

			// Token: 0x020000B4 RID: 180
			internal class GetResourceStringUserData
			{
				// Token: 0x06000AA6 RID: 2726 RVA: 0x00020D66 File Offset: 0x0001FD66
				public GetResourceStringUserData(Environment.ResourceHelper resourceHelper, string key)
				{
					this.m_resourceHelper = resourceHelper;
					this.m_key = key;
				}

				// Token: 0x040003D8 RID: 984
				public Environment.ResourceHelper m_resourceHelper;

				// Token: 0x040003D9 RID: 985
				public string m_key;

				// Token: 0x040003DA RID: 986
				public string m_retVal;

				// Token: 0x040003DB RID: 987
				public bool m_lockWasTaken;
			}
		}

		// Token: 0x020000B5 RID: 181
		[Serializable]
		internal enum OSName
		{
			// Token: 0x040003DD RID: 989
			Invalid,
			// Token: 0x040003DE RID: 990
			Unknown,
			// Token: 0x040003DF RID: 991
			Win9x = 64,
			// Token: 0x040003E0 RID: 992
			Win95,
			// Token: 0x040003E1 RID: 993
			Win98,
			// Token: 0x040003E2 RID: 994
			WinMe,
			// Token: 0x040003E3 RID: 995
			WinNT = 128,
			// Token: 0x040003E4 RID: 996
			Nt4,
			// Token: 0x040003E5 RID: 997
			Win2k
		}

		// Token: 0x020000B6 RID: 182
		[ComVisible(true)]
		public enum SpecialFolder
		{
			// Token: 0x040003E7 RID: 999
			ApplicationData = 26,
			// Token: 0x040003E8 RID: 1000
			CommonApplicationData = 35,
			// Token: 0x040003E9 RID: 1001
			LocalApplicationData = 28,
			// Token: 0x040003EA RID: 1002
			Cookies = 33,
			// Token: 0x040003EB RID: 1003
			Desktop = 0,
			// Token: 0x040003EC RID: 1004
			Favorites = 6,
			// Token: 0x040003ED RID: 1005
			History = 34,
			// Token: 0x040003EE RID: 1006
			InternetCache = 32,
			// Token: 0x040003EF RID: 1007
			Programs = 2,
			// Token: 0x040003F0 RID: 1008
			MyComputer = 17,
			// Token: 0x040003F1 RID: 1009
			MyMusic = 13,
			// Token: 0x040003F2 RID: 1010
			MyPictures = 39,
			// Token: 0x040003F3 RID: 1011
			Recent = 8,
			// Token: 0x040003F4 RID: 1012
			SendTo,
			// Token: 0x040003F5 RID: 1013
			StartMenu = 11,
			// Token: 0x040003F6 RID: 1014
			Startup = 7,
			// Token: 0x040003F7 RID: 1015
			System = 37,
			// Token: 0x040003F8 RID: 1016
			Templates = 21,
			// Token: 0x040003F9 RID: 1017
			DesktopDirectory = 16,
			// Token: 0x040003FA RID: 1018
			Personal = 5,
			// Token: 0x040003FB RID: 1019
			MyDocuments = 5,
			// Token: 0x040003FC RID: 1020
			ProgramFiles = 38,
			// Token: 0x040003FD RID: 1021
			CommonProgramFiles = 43
		}
	}
}
