using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x0200077D RID: 1917
	internal static class ProcessManager
	{
		// Token: 0x06003B2C RID: 15148 RVA: 0x000FBCD8 File Offset: 0x000FACD8
		static ProcessManager()
		{
			NativeMethods.LUID luid = default(NativeMethods.LUID);
			if (!NativeMethods.LookupPrivilegeValue(null, "SeDebugPrivilege", out luid))
			{
				return;
			}
			IntPtr zero = IntPtr.Zero;
			try
			{
				if (NativeMethods.OpenProcessToken(new HandleRef(null, NativeMethods.GetCurrentProcess()), 32, out zero))
				{
					NativeMethods.TokenPrivileges tokenPrivileges = new NativeMethods.TokenPrivileges();
					tokenPrivileges.PrivilegeCount = 1;
					tokenPrivileges.Luid = luid;
					tokenPrivileges.Attributes = 2;
					NativeMethods.AdjustTokenPrivileges(new HandleRef(null, zero), false, tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					SafeNativeMethods.CloseHandle(new HandleRef(null, zero));
				}
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06003B2D RID: 15149 RVA: 0x000FBD80 File Offset: 0x000FAD80
		public static bool IsNt
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06003B2E RID: 15150 RVA: 0x000FBD8F File Offset: 0x000FAD8F
		public static bool IsOSOlderThanXP
		{
			get
			{
				return Environment.OSVersion.Version.Major < 5 || (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 0);
			}
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x000FBDCC File Offset: 0x000FADCC
		public static ProcessInfo[] GetProcessInfos(string machineName)
		{
			bool flag = ProcessManager.IsRemoteMachine(machineName);
			if (ProcessManager.IsNt)
			{
				if (!flag && Environment.OSVersion.Version.Major >= 5)
				{
					return NtProcessInfoHelper.GetProcessInfos();
				}
				return NtProcessManager.GetProcessInfos(machineName, flag);
			}
			else
			{
				if (flag)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinNTRequiredForRemote"));
				}
				return WinProcessManager.GetProcessInfos();
			}
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x000FBE22 File Offset: 0x000FAE22
		public static int[] GetProcessIds()
		{
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetProcessIds();
			}
			return WinProcessManager.GetProcessIds();
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x000FBE36 File Offset: 0x000FAE36
		public static int[] GetProcessIds(string machineName)
		{
			if (!ProcessManager.IsRemoteMachine(machineName))
			{
				return ProcessManager.GetProcessIds();
			}
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetProcessIds(machineName, true);
			}
			throw new PlatformNotSupportedException(SR.GetString("WinNTRequiredForRemote"));
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000FBE64 File Offset: 0x000FAE64
		public static bool IsProcessRunning(int processId, string machineName)
		{
			return ProcessManager.IsProcessRunning(processId, ProcessManager.GetProcessIds(machineName));
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000FBE72 File Offset: 0x000FAE72
		public static bool IsProcessRunning(int processId)
		{
			return ProcessManager.IsProcessRunning(processId, ProcessManager.GetProcessIds());
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000FBE80 File Offset: 0x000FAE80
		private static bool IsProcessRunning(int processId, int[] processIds)
		{
			for (int i = 0; i < processIds.Length; i++)
			{
				if (processIds[i] == processId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000FBEA4 File Offset: 0x000FAEA4
		public static int GetProcessIdFromHandle(SafeProcessHandle processHandle)
		{
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetProcessIdFromHandle(processHandle);
			}
			throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000FBEC4 File Offset: 0x000FAEC4
		public static IntPtr GetMainWindowHandle(ProcessInfo processInfo)
		{
			MainWindowFinder mainWindowFinder = new MainWindowFinder();
			return mainWindowFinder.FindMainWindow(processInfo.processId);
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000FBEE3 File Offset: 0x000FAEE3
		public static ModuleInfo[] GetModuleInfos(int processId)
		{
			if (ProcessManager.IsNt)
			{
				return NtProcessManager.GetModuleInfos(processId);
			}
			return WinProcessManager.GetModuleInfos(processId);
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x000FBEFC File Offset: 0x000FAEFC
		public static SafeProcessHandle OpenProcess(int processId, int access, bool throwIfExited)
		{
			SafeProcessHandle safeProcessHandle = NativeMethods.OpenProcess(access, false, processId);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (!safeProcessHandle.IsInvalid)
			{
				return safeProcessHandle;
			}
			if (processId == 0)
			{
				throw new Win32Exception(5);
			}
			if (ProcessManager.IsProcessRunning(processId))
			{
				throw new Win32Exception(lastWin32Error);
			}
			if (throwIfExited)
			{
				throw new InvalidOperationException(SR.GetString("ProcessHasExited", new object[]
				{
					processId.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return SafeProcessHandle.InvalidHandle;
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x000FBF6C File Offset: 0x000FAF6C
		public static SafeThreadHandle OpenThread(int threadId, int access)
		{
			SafeThreadHandle result;
			try
			{
				SafeThreadHandle safeThreadHandle = NativeMethods.OpenThread(access, false, threadId);
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (safeThreadHandle.IsInvalid)
				{
					if (lastWin32Error == 87)
					{
						throw new InvalidOperationException(SR.GetString("ThreadExited", new object[]
						{
							threadId.ToString(CultureInfo.CurrentCulture)
						}));
					}
					throw new Win32Exception(lastWin32Error);
				}
				else
				{
					result = safeThreadHandle;
				}
			}
			catch (EntryPointNotFoundException inner)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"), inner);
			}
			return result;
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x000FBFF0 File Offset: 0x000FAFF0
		public static bool IsRemoteMachine(string machineName)
		{
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			if (machineName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
				{
					"machineName",
					machineName
				}));
			}
			string text;
			if (machineName.StartsWith("\\", StringComparison.Ordinal))
			{
				text = machineName.Substring(2);
			}
			else
			{
				text = machineName;
			}
			if (text.Equals("."))
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			SafeNativeMethods.GetComputerName(stringBuilder, new int[]
			{
				stringBuilder.Capacity
			});
			string strA = stringBuilder.ToString();
			return string.Compare(strA, text, StringComparison.OrdinalIgnoreCase) != 0;
		}
	}
}
