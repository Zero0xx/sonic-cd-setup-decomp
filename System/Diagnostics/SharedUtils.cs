using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000796 RID: 1942
	internal static class SharedUtils
	{
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x001002C4 File Offset: 0x000FF2C4
		private static object InternalSyncObject
		{
			get
			{
				if (SharedUtils.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref SharedUtils.s_InternalSyncObject, value, null);
				}
				return SharedUtils.s_InternalSyncObject;
			}
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x001002F0 File Offset: 0x000FF2F0
		internal static Win32Exception CreateSafeWin32Exception()
		{
			return SharedUtils.CreateSafeWin32Exception(0);
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x001002F8 File Offset: 0x000FF2F8
		internal static Win32Exception CreateSafeWin32Exception(int error)
		{
			Win32Exception result = null;
			SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
			securityPermission.Assert();
			try
			{
				if (error == 0)
				{
					result = new Win32Exception();
				}
				else
				{
					result = new Win32Exception(error);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003BE5 RID: 15333 RVA: 0x00100340 File Offset: 0x000FF340
		internal static int CurrentEnvironment
		{
			get
			{
				if (SharedUtils.environment == 0)
				{
					lock (SharedUtils.InternalSyncObject)
					{
						if (SharedUtils.environment == 0)
						{
							if (Environment.OSVersion.Platform == PlatformID.Win32NT)
							{
								if (Environment.OSVersion.Version.Major >= 5)
								{
									SharedUtils.environment = 1;
								}
								else
								{
									SharedUtils.environment = 2;
								}
							}
							else
							{
								SharedUtils.environment = 3;
							}
						}
					}
				}
				return SharedUtils.environment;
			}
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x001003BC File Offset: 0x000FF3BC
		internal static void CheckEnvironment()
		{
			if (SharedUtils.CurrentEnvironment == 3)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x001003D6 File Offset: 0x000FF3D6
		internal static void CheckNtEnvironment()
		{
			if (SharedUtils.CurrentEnvironment == 2)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x001003F0 File Offset: 0x000FF3F0
		internal static void EnterMutex(string name, ref Mutex mutex)
		{
			string mutexName;
			if (SharedUtils.CurrentEnvironment == 1)
			{
				mutexName = "Global\\" + name;
			}
			else
			{
				mutexName = name;
			}
			SharedUtils.EnterMutexWithoutGlobal(mutexName, ref mutex);
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x00100420 File Offset: 0x000FF420
		internal static void EnterMutexWithoutGlobal(string mutexName, ref Mutex mutex)
		{
			MutexSecurity mutexSecurity = new MutexSecurity();
			SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
			mutexSecurity.AddAccessRule(new MutexAccessRule(identity, MutexRights.Modify | MutexRights.Synchronize, AccessControlType.Allow));
			bool flag;
			Mutex mutexIn = new Mutex(false, mutexName, ref flag, mutexSecurity);
			SharedUtils.SafeWaitForMutex(mutexIn, ref mutex);
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x00100461 File Offset: 0x000FF461
		private static bool SafeWaitForMutex(Mutex mutexIn, ref Mutex mutexOut)
		{
			while (SharedUtils.SafeWaitForMutexOnce(mutexIn, ref mutexOut))
			{
				if (mutexOut != null)
				{
					return true;
				}
				Thread.Sleep(0);
			}
			return false;
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x0010047C File Offset: 0x000FF47C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool SafeWaitForMutexOnce(Mutex mutexIn, ref Mutex mutexOut)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			bool result;
			try
			{
			}
			finally
			{
				Thread.BeginCriticalRegion();
				Thread.BeginThreadAffinity();
				int num = SharedUtils.WaitForSingleObjectDontCallThis(mutexIn.SafeWaitHandle, 500);
				int num2 = num;
				if (num2 != 0 && num2 != 128)
				{
					result = (num2 == 258);
				}
				else
				{
					mutexOut = mutexIn;
					result = true;
				}
				if (mutexOut == null)
				{
					Thread.EndThreadAffinity();
					Thread.EndCriticalRegion();
				}
			}
			return result;
		}

		// Token: 0x06003BEC RID: 15340
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject", ExactSpelling = true, SetLastError = true)]
		private static extern int WaitForSingleObjectDontCallThis(SafeWaitHandle handle, int timeout);

		// Token: 0x06003BED RID: 15341 RVA: 0x001004F0 File Offset: 0x000FF4F0
		internal static string GetLatestBuildDllDirectory(string machineName)
		{
			string result = "";
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Assert();
			try
			{
				if (machineName.Equals("."))
				{
					return SharedUtils.GetLocalBuildDirectory();
				}
				registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName);
				if (registryKey == null)
				{
					throw new InvalidOperationException(SR.GetString("RegKeyMissingShort", new object[]
					{
						"HKEY_LOCAL_MACHINE",
						machineName
					}));
				}
				registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework");
				if (registryKey2 != null)
				{
					string text = (string)registryKey2.GetValue("InstallRoot");
					if (text != null && text != string.Empty)
					{
						Version version = Environment.Version;
						string text2 = "v" + version.ToString(2);
						string text3 = null;
						RegistryKey registryKey3 = registryKey2.OpenSubKey("policy\\" + text2);
						if (registryKey3 != null)
						{
							try
							{
								text3 = (string)registryKey3.GetValue("Version");
								if (text3 == null)
								{
									string[] valueNames = registryKey3.GetValueNames();
									for (int i = 0; i < valueNames.Length; i++)
									{
										string text4 = text2 + "." + valueNames[i].Replace('-', '.');
										if (string.Compare(text4, text3, StringComparison.Ordinal) > 0)
										{
											text3 = text4;
										}
									}
								}
							}
							finally
							{
								registryKey3.Close();
							}
							if (text3 != null && text3 != string.Empty)
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.Append(text);
								if (!text.EndsWith("\\", StringComparison.Ordinal))
								{
									stringBuilder.Append("\\");
								}
								stringBuilder.Append(text3);
								stringBuilder.Append("\\");
								result = stringBuilder.ToString();
							}
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x00100714 File Offset: 0x000FF714
		private static string GetLocalBuildDirectory()
		{
			int num = 264;
			int num2 = 25;
			StringBuilder stringBuilder = new StringBuilder(num);
			StringBuilder stringBuilder2 = new StringBuilder(num2);
			uint num3;
			uint num4;
			uint requestedRuntimeInfo;
			for (requestedRuntimeInfo = NativeMethods.GetRequestedRuntimeInfo(null, null, null, 0U, 65U, stringBuilder, num, out num3, stringBuilder2, num2, out num4); requestedRuntimeInfo == 122U; requestedRuntimeInfo = NativeMethods.GetRequestedRuntimeInfo(null, null, null, 0U, 0U, stringBuilder, num, out num3, stringBuilder2, num2, out num4))
			{
				num *= 2;
				num2 *= 2;
				stringBuilder = new StringBuilder(num);
				stringBuilder2 = new StringBuilder(num2);
			}
			if (requestedRuntimeInfo != 0U)
			{
				throw SharedUtils.CreateSafeWin32Exception();
			}
			stringBuilder.Append(stringBuilder2);
			return stringBuilder.ToString();
		}

		// Token: 0x04003486 RID: 13446
		internal const int UnknownEnvironment = 0;

		// Token: 0x04003487 RID: 13447
		internal const int W2kEnvironment = 1;

		// Token: 0x04003488 RID: 13448
		internal const int NtEnvironment = 2;

		// Token: 0x04003489 RID: 13449
		internal const int NonNtEnvironment = 3;

		// Token: 0x0400348A RID: 13450
		private static int environment;

		// Token: 0x0400348B RID: 13451
		private static object s_InternalSyncObject;
	}
}
