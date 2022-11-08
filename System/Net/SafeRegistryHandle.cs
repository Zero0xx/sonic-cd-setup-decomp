using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200052F RID: 1327
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002895 RID: 10389 RVA: 0x000A7D54 File Offset: 0x000A6D54
		private SafeRegistryHandle() : base(true)
		{
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000A7D5D File Offset: 0x000A6D5D
		internal static uint RegOpenKeyEx(IntPtr key, string subKey, uint ulOptions, uint samDesired, out SafeRegistryHandle resultSubKey)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(key, subKey, ulOptions, samDesired, out resultSubKey);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000A7D6A File Offset: 0x000A6D6A
		internal uint RegOpenKeyEx(string subKey, uint ulOptions, uint samDesired, out SafeRegistryHandle resultSubKey)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(this, subKey, ulOptions, samDesired, out resultSubKey);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000A7D77 File Offset: 0x000A6D77
		internal uint RegCloseKey()
		{
			base.Close();
			return this.resClose;
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000A7D88 File Offset: 0x000A6D88
		internal uint QueryValue(string name, out object data)
		{
			data = null;
			byte[] array = null;
			uint num = 0U;
			uint num3;
			uint num2;
			for (;;)
			{
				num2 = UnsafeNclNativeMethods.RegistryHelper.RegQueryValueEx(this, name, IntPtr.Zero, out num3, array, ref num);
				if (num2 != 234U && (array != null || num2 != 0U))
				{
					break;
				}
				array = new byte[num];
			}
			if (num2 != 0U)
			{
				return num2;
			}
			uint num4 = num3;
			if (num4 == 3U)
			{
				if ((ulong)num != (ulong)((long)array.Length))
				{
					byte[] src = array;
					array = new byte[num];
					Buffer.BlockCopy(src, 0, array, 0, (int)num);
				}
				data = array;
				return 0U;
			}
			return 50U;
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000A7DF8 File Offset: 0x000A6DF8
		internal uint RegNotifyChangeKeyValue(bool watchSubTree, uint notifyFilter, SafeWaitHandle regEvent, bool async)
		{
			return UnsafeNclNativeMethods.RegistryHelper.RegNotifyChangeKeyValue(this, watchSubTree, notifyFilter, regEvent, async);
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000A7E05 File Offset: 0x000A6E05
		internal static uint RegOpenCurrentUser(uint samDesired, out SafeRegistryHandle resultKey)
		{
			if (ComNetOS.IsWin9x)
			{
				return UnsafeNclNativeMethods.RegistryHelper.RegOpenKeyEx(UnsafeNclNativeMethods.RegistryHelper.HKEY_CURRENT_USER, null, 0U, samDesired, out resultKey);
			}
			return UnsafeNclNativeMethods.RegistryHelper.RegOpenCurrentUser(samDesired, out resultKey);
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000A7E24 File Offset: 0x000A6E24
		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				this.resClose = UnsafeNclNativeMethods.RegistryHelper.RegCloseKey(this.handle);
			}
			base.SetHandleAsInvalid();
			return true;
		}

		// Token: 0x04002788 RID: 10120
		private uint resClose;
	}
}
