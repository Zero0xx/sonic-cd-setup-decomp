using System;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200052E RID: 1326
	internal sealed class SafeUnlockUrlCacheEntryFile : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002891 RID: 10385 RVA: 0x000A7C70 File Offset: 0x000A6C70
		private SafeUnlockUrlCacheEntryFile(string keyString) : base(true)
		{
			this.m_KeyString = keyString;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000A7C80 File Offset: 0x000A6C80
		protected unsafe override bool ReleaseHandle()
		{
			fixed (char* keyString = this.m_KeyString)
			{
				UnsafeNclNativeMethods.SafeNetHandles.UnlockUrlCacheEntryFileW(keyString, 0);
			}
			base.SetHandle(IntPtr.Zero);
			this.m_KeyString = null;
			return true;
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000A7CC0 File Offset: 0x000A6CC0
		internal unsafe static _WinInetCache.Status GetAndLockFile(string key, byte* entryPtr, ref int entryBufSize, out SafeUnlockUrlCacheEntryFile handle)
		{
			if (ValidationHelper.IsBlankString(key))
			{
				throw new ArgumentNullException("key");
			}
			handle = new SafeUnlockUrlCacheEntryFile(key);
			IntPtr intPtr2;
			IntPtr intPtr = intPtr2 = key;
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* key2 = intPtr2;
			return SafeUnlockUrlCacheEntryFile.MustRunGetAndLockFile(key2, entryPtr, ref entryBufSize, handle);
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000A7D04 File Offset: 0x000A6D04
		private unsafe static _WinInetCache.Status MustRunGetAndLockFile(char* key, byte* entryPtr, ref int entryBufSize, SafeUnlockUrlCacheEntryFile handle)
		{
			_WinInetCache.Status result = _WinInetCache.Status.Success;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (!UnsafeNclNativeMethods.SafeNetHandles.RetrieveUrlCacheEntryFileW(key, entryPtr, ref entryBufSize, 0))
				{
					result = (_WinInetCache.Status)Marshal.GetLastWin32Error();
					handle.SetHandleAsInvalid();
				}
				else
				{
					handle.SetHandle((IntPtr)1);
				}
			}
			return result;
		}

		// Token: 0x04002787 RID: 10119
		private string m_KeyString;
	}
}
