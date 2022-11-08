using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000767 RID: 1895
	internal class PerformanceMonitor
	{
		// Token: 0x06003A67 RID: 14951 RVA: 0x000F8311 File Offset: 0x000F7311
		internal PerformanceMonitor(string machineName)
		{
			this.machineName = machineName;
			this.Init();
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x000F8328 File Offset: 0x000F7328
		private void Init()
		{
			try
			{
				if (this.machineName != "." && string.Compare(this.machineName, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) != 0)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					this.perfDataKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.PerformanceData, this.machineName);
				}
				else
				{
					this.perfDataKey = Registry.PerformanceData;
				}
			}
			catch (UnauthorizedAccessException)
			{
				throw new Win32Exception(5);
			}
			catch (IOException e)
			{
				throw new Win32Exception(Marshal.GetHRForException(e));
			}
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x000F83BC File Offset: 0x000F73BC
		internal void Close()
		{
			if (this.perfDataKey != null)
			{
				this.perfDataKey.Close();
			}
			this.perfDataKey = null;
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x000F83D8 File Offset: 0x000F73D8
		internal byte[] GetData(string item)
		{
			int i = 17;
			int num = 0;
			int num2 = 0;
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			while (i > 0)
			{
				try
				{
					return (byte[])this.perfDataKey.GetValue(item);
				}
				catch (IOException e)
				{
					num2 = Marshal.GetHRForException(e);
					int num3 = num2;
					if (num3 <= 167)
					{
						if (num3 != 6)
						{
							if (num3 != 21 && num3 != 167)
							{
								goto IL_AC;
							}
							goto IL_94;
						}
					}
					else if (num3 <= 258)
					{
						if (num3 != 170 && num3 != 258)
						{
							goto IL_AC;
						}
						goto IL_94;
					}
					else if (num3 != 1722 && num3 != 1726)
					{
						goto IL_AC;
					}
					this.Init();
					IL_94:
					i--;
					if (num == 0)
					{
						num = 10;
					}
					else
					{
						Thread.Sleep(num);
						num *= 2;
					}
					continue;
					IL_AC:
					throw SharedUtils.CreateSafeWin32Exception(num2);
				}
			}
			throw SharedUtils.CreateSafeWin32Exception(num2);
		}

		// Token: 0x04003326 RID: 13094
		private RegistryKey perfDataKey;

		// Token: 0x04003327 RID: 13095
		private string machineName;
	}
}
