using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x02000570 RID: 1392
	internal class _WinInetCache
	{
		// Token: 0x06002A98 RID: 10904 RVA: 0x000B4FFE File Offset: 0x000B3FFE
		private _WinInetCache()
		{
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000B5008 File Offset: 0x000B4008
		internal unsafe static _WinInetCache.Status LookupInfo(_WinInetCache.Entry entry)
		{
			byte[] array = new byte[2048];
			int num = array.Length;
			byte[] array2 = array;
			for (int i = 0; i < 64; i++)
			{
				try
				{
					fixed (byte* ptr = array2)
					{
						bool urlCacheEntryInfoW = UnsafeNclNativeMethods.UnsafeWinInetCache.GetUrlCacheEntryInfoW(entry.Key, ptr, ref num);
						if (urlCacheEntryInfoW)
						{
							array = array2;
							entry.MaxBufferBytes = num;
							_WinInetCache.EntryFixup(entry, (_WinInetCache.EntryBuffer*)ptr, array2);
							entry.Error = _WinInetCache.Status.Success;
							return entry.Error;
						}
						entry.Error = (_WinInetCache.Status)Marshal.GetLastWin32Error();
						if (entry.Error != _WinInetCache.Status.InsufficientBuffer || array2 != array || num > entry.MaxBufferBytes)
						{
							break;
						}
						array2 = new byte[num];
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			return entry.Error;
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000B50DC File Offset: 0x000B40DC
		internal unsafe static SafeUnlockUrlCacheEntryFile LookupFile(_WinInetCache.Entry entry)
		{
			byte[] array = new byte[2048];
			int num = array.Length;
			SafeUnlockUrlCacheEntryFile safeUnlockUrlCacheEntryFile = null;
			try
			{
				for (;;)
				{
					try
					{
						fixed (byte* ptr = array)
						{
							entry.Error = SafeUnlockUrlCacheEntryFile.GetAndLockFile(entry.Key, ptr, ref num, out safeUnlockUrlCacheEntryFile);
							if (entry.Error == _WinInetCache.Status.Success)
							{
								entry.MaxBufferBytes = num;
								_WinInetCache.EntryFixup(entry, (_WinInetCache.EntryBuffer*)ptr, array);
								return safeUnlockUrlCacheEntryFile;
							}
							if (entry.Error == _WinInetCache.Status.InsufficientBuffer && num <= entry.MaxBufferBytes)
							{
								array = new byte[num];
								continue;
							}
						}
					}
					finally
					{
						byte* ptr = null;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				if (safeUnlockUrlCacheEntryFile != null)
				{
					safeUnlockUrlCacheEntryFile.Close();
				}
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (entry.Error == _WinInetCache.Status.Success)
				{
					entry.Error = _WinInetCache.Status.InternalError;
				}
			}
			catch
			{
				if (safeUnlockUrlCacheEntryFile != null)
				{
					safeUnlockUrlCacheEntryFile.Close();
				}
				if (entry.Error == _WinInetCache.Status.Success)
				{
					entry.Error = _WinInetCache.Status.InternalError;
				}
			}
			return null;
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000B51FC File Offset: 0x000B41FC
		private unsafe static _WinInetCache.Status EntryFixup(_WinInetCache.Entry entry, _WinInetCache.EntryBuffer* bufferPtr, byte[] buffer)
		{
			bufferPtr->_OffsetExtension = ((bufferPtr->_OffsetExtension == IntPtr.Zero) ? IntPtr.Zero : ((IntPtr)((long)((byte*)((void*)bufferPtr->_OffsetExtension) - (byte*)bufferPtr))));
			bufferPtr->_OffsetFileName = ((bufferPtr->_OffsetFileName == IntPtr.Zero) ? IntPtr.Zero : ((IntPtr)((long)((byte*)((void*)bufferPtr->_OffsetFileName) - (byte*)bufferPtr))));
			bufferPtr->_OffsetHeaderInfo = ((bufferPtr->_OffsetHeaderInfo == IntPtr.Zero) ? IntPtr.Zero : ((IntPtr)((long)((byte*)((void*)bufferPtr->_OffsetHeaderInfo) - (byte*)bufferPtr))));
			bufferPtr->_OffsetSourceUrlName = ((bufferPtr->_OffsetSourceUrlName == IntPtr.Zero) ? IntPtr.Zero : ((IntPtr)((long)((byte*)((void*)bufferPtr->_OffsetSourceUrlName) - (byte*)bufferPtr))));
			entry.Info = *bufferPtr;
			entry.OriginalUrl = _WinInetCache.GetEntryBufferString((void*)bufferPtr, (int)bufferPtr->_OffsetSourceUrlName);
			entry.Filename = _WinInetCache.GetEntryBufferString((void*)bufferPtr, (int)bufferPtr->_OffsetFileName);
			entry.FileExt = _WinInetCache.GetEntryBufferString((void*)bufferPtr, (int)bufferPtr->_OffsetExtension);
			return _WinInetCache.GetEntryHeaders(entry, bufferPtr, buffer);
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000B5334 File Offset: 0x000B4334
		internal static _WinInetCache.Status CreateFileName(_WinInetCache.Entry entry)
		{
			entry.Error = _WinInetCache.Status.Success;
			StringBuilder stringBuilder = new StringBuilder(260);
			if (UnsafeNclNativeMethods.UnsafeWinInetCache.CreateUrlCacheEntryW(entry.Key, entry.OptionalLength, entry.FileExt, stringBuilder, 0))
			{
				entry.Filename = stringBuilder.ToString();
				return _WinInetCache.Status.Success;
			}
			entry.Error = (_WinInetCache.Status)Marshal.GetLastWin32Error();
			return entry.Error;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000B5390 File Offset: 0x000B4390
		internal unsafe static _WinInetCache.Status Commit(_WinInetCache.Entry entry)
		{
			string text = entry.MetaInfo;
			if (text == null)
			{
				text = string.Empty;
			}
			if (text.Length + entry.Key.Length + entry.Filename.Length + ((entry.OriginalUrl == null) ? 0 : entry.OriginalUrl.Length) > entry.MaxBufferBytes / 2)
			{
				entry.Error = _WinInetCache.Status.InsufficientBuffer;
				return entry.Error;
			}
			entry.Error = _WinInetCache.Status.Success;
			fixed (char* ptr = text)
			{
				byte* headerInfo = (byte*)((text.Length == 0) ? null : ptr);
				if (!UnsafeNclNativeMethods.UnsafeWinInetCache.CommitUrlCacheEntryW(entry.Key, entry.Filename, entry.Info.ExpireTime, entry.Info.LastModifiedTime, entry.Info.EntryType, headerInfo, text.Length, null, entry.OriginalUrl))
				{
					entry.Error = (_WinInetCache.Status)Marshal.GetLastWin32Error();
				}
			}
			return entry.Error;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000B5474 File Offset: 0x000B4474
		internal unsafe static _WinInetCache.Status Update(_WinInetCache.Entry newEntry, _WinInetCache.Entry_FC attributes)
		{
			byte[] array = new byte[_WinInetCache.EntryBuffer.MarshalSize];
			newEntry.Error = _WinInetCache.Status.Success;
			fixed (byte* ptr = array)
			{
				_WinInetCache.EntryBuffer* ptr2 = (_WinInetCache.EntryBuffer*)ptr;
				*ptr2 = newEntry.Info;
				ptr2->StructSize = _WinInetCache.EntryBuffer.MarshalSize;
				if ((attributes & _WinInetCache.Entry_FC.Headerinfo) != _WinInetCache.Entry_FC.None)
				{
					_WinInetCache.Entry entry = new _WinInetCache.Entry(newEntry.Key, newEntry.MaxBufferBytes);
					SafeUnlockUrlCacheEntryFile safeUnlockUrlCacheEntryFile = null;
					bool flag = false;
					try
					{
						safeUnlockUrlCacheEntryFile = _WinInetCache.LookupFile(entry);
						if (safeUnlockUrlCacheEntryFile == null)
						{
							newEntry.Error = entry.Error;
							return newEntry.Error;
						}
						newEntry.Filename = entry.Filename;
						newEntry.OriginalUrl = entry.OriginalUrl;
						newEntry.FileExt = entry.FileExt;
						attributes &= ~_WinInetCache.Entry_FC.Headerinfo;
						if ((attributes & _WinInetCache.Entry_FC.Exptime) == _WinInetCache.Entry_FC.None)
						{
							newEntry.Info.ExpireTime = entry.Info.ExpireTime;
						}
						if ((attributes & _WinInetCache.Entry_FC.Modtime) == _WinInetCache.Entry_FC.None)
						{
							newEntry.Info.LastModifiedTime = entry.Info.LastModifiedTime;
						}
						if ((attributes & _WinInetCache.Entry_FC.Attribute) == _WinInetCache.Entry_FC.None)
						{
							newEntry.Info.EntryType = entry.Info.EntryType;
							newEntry.Info.U.ExemptDelta = entry.Info.U.ExemptDelta;
							if ((entry.Info.EntryType & _WinInetCache.EntryType.StickyEntry) == _WinInetCache.EntryType.StickyEntry)
							{
								attributes |= (_WinInetCache.Entry_FC.Attribute | _WinInetCache.Entry_FC.ExemptDelta);
							}
						}
						attributes &= ~(_WinInetCache.Entry_FC.Modtime | _WinInetCache.Entry_FC.Exptime);
						flag = ((entry.Info.EntryType & _WinInetCache.EntryType.Edited) != (_WinInetCache.EntryType)0);
						if (!flag)
						{
							_WinInetCache.Entry entry2 = entry;
							entry2.Info.EntryType = (entry2.Info.EntryType | _WinInetCache.EntryType.Edited);
							if (_WinInetCache.Update(entry, _WinInetCache.Entry_FC.Attribute) != _WinInetCache.Status.Success)
							{
								newEntry.Error = entry.Error;
								return newEntry.Error;
							}
						}
					}
					finally
					{
						if (safeUnlockUrlCacheEntryFile != null)
						{
							safeUnlockUrlCacheEntryFile.Close();
						}
					}
					_WinInetCache.Remove(entry);
					_WinInetCache.Status error;
					if (_WinInetCache.Commit(newEntry) != _WinInetCache.Status.Success)
					{
						if (!flag)
						{
							_WinInetCache.Entry entry3 = entry;
							entry3.Info.EntryType = (entry3.Info.EntryType & ~_WinInetCache.EntryType.Edited);
							_WinInetCache.Update(entry, _WinInetCache.Entry_FC.Attribute);
						}
						error = newEntry.Error;
					}
					else
					{
						if (attributes != _WinInetCache.Entry_FC.None)
						{
							_WinInetCache.Update(newEntry, attributes);
							goto IL_213;
						}
						goto IL_213;
					}
					return error;
				}
				if (!UnsafeNclNativeMethods.UnsafeWinInetCache.SetUrlCacheEntryInfoW(newEntry.Key, ptr, attributes))
				{
					newEntry.Error = (_WinInetCache.Status)Marshal.GetLastWin32Error();
				}
				IL_213:;
			}
			return newEntry.Error;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000B56BC File Offset: 0x000B46BC
		internal static _WinInetCache.Status Remove(_WinInetCache.Entry entry)
		{
			entry.Error = _WinInetCache.Status.Success;
			if (!UnsafeNclNativeMethods.UnsafeWinInetCache.DeleteUrlCacheEntryW(entry.Key))
			{
				entry.Error = (_WinInetCache.Status)Marshal.GetLastWin32Error();
			}
			return entry.Error;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000B56E4 File Offset: 0x000B46E4
		private unsafe static string GetEntryBufferString(void* bufferPtr, int offset)
		{
			if (offset == 0)
			{
				return null;
			}
			IntPtr ptr = new IntPtr((void*)((byte*)bufferPtr + offset));
			return Marshal.PtrToStringUni(ptr);
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000B5708 File Offset: 0x000B4708
		private unsafe static _WinInetCache.Status GetEntryHeaders(_WinInetCache.Entry entry, _WinInetCache.EntryBuffer* bufferPtr, byte[] buffer)
		{
			entry.Error = _WinInetCache.Status.Success;
			entry.MetaInfo = null;
			if (bufferPtr->_OffsetHeaderInfo == IntPtr.Zero || bufferPtr->HeaderInfoChars == 0 || (bufferPtr->EntryType & _WinInetCache.EntryType.UrlHistory) != (_WinInetCache.EntryType)0)
			{
				return _WinInetCache.Status.Success;
			}
			int num = bufferPtr->HeaderInfoChars + (int)bufferPtr->_OffsetHeaderInfo / 2;
			if (num * 2 > entry.MaxBufferBytes)
			{
				num = entry.MaxBufferBytes / 2;
			}
			while (*(ushort*)(bufferPtr + (IntPtr)(num - 1) * 2 / (IntPtr)sizeof(_WinInetCache.EntryBuffer)) == 0)
			{
				num--;
			}
			entry.MetaInfo = Encoding.Unicode.GetString(buffer, (int)bufferPtr->_OffsetHeaderInfo, (num - (int)bufferPtr->_OffsetHeaderInfo / 2) * 2);
			return entry.Error;
		}

		// Token: 0x04002937 RID: 10551
		private const int c_CharSz = 2;

		// Token: 0x02000571 RID: 1393
		[Flags]
		internal enum EntryType
		{
			// Token: 0x04002939 RID: 10553
			NormalEntry = 65,
			// Token: 0x0400293A RID: 10554
			StickyEntry = 68,
			// Token: 0x0400293B RID: 10555
			Edited = 8,
			// Token: 0x0400293C RID: 10556
			TrackOffline = 16,
			// Token: 0x0400293D RID: 10557
			TrackOnline = 32,
			// Token: 0x0400293E RID: 10558
			Sparse = 65536,
			// Token: 0x0400293F RID: 10559
			Cookie = 1048576,
			// Token: 0x04002940 RID: 10560
			UrlHistory = 2097152
		}

		// Token: 0x02000572 RID: 1394
		[Flags]
		internal enum Entry_FC
		{
			// Token: 0x04002942 RID: 10562
			None = 0,
			// Token: 0x04002943 RID: 10563
			Attribute = 4,
			// Token: 0x04002944 RID: 10564
			Hitrate = 16,
			// Token: 0x04002945 RID: 10565
			Modtime = 64,
			// Token: 0x04002946 RID: 10566
			Exptime = 128,
			// Token: 0x04002947 RID: 10567
			Acctime = 256,
			// Token: 0x04002948 RID: 10568
			Synctime = 512,
			// Token: 0x04002949 RID: 10569
			Headerinfo = 1024,
			// Token: 0x0400294A RID: 10570
			ExemptDelta = 2048
		}

		// Token: 0x02000573 RID: 1395
		internal enum Status
		{
			// Token: 0x0400294C RID: 10572
			Success,
			// Token: 0x0400294D RID: 10573
			InsufficientBuffer = 122,
			// Token: 0x0400294E RID: 10574
			FileNotFound = 2,
			// Token: 0x0400294F RID: 10575
			NoMoreItems = 259,
			// Token: 0x04002950 RID: 10576
			NotEnoughStorage = 8,
			// Token: 0x04002951 RID: 10577
			SharingViolation = 32,
			// Token: 0x04002952 RID: 10578
			InvalidParameter = 87,
			// Token: 0x04002953 RID: 10579
			Warnings = 16777216,
			// Token: 0x04002954 RID: 10580
			FatalErrors = 16781312,
			// Token: 0x04002955 RID: 10581
			CorruptedHeaders,
			// Token: 0x04002956 RID: 10582
			InternalError
		}

		// Token: 0x02000574 RID: 1396
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct FILETIME
		{
			// Token: 0x06002AA2 RID: 10914 RVA: 0x000B57BA File Offset: 0x000B47BA
			public FILETIME(long time)
			{
				this.Low = (uint)time;
				this.High = (uint)(time >> 32);
			}

			// Token: 0x06002AA3 RID: 10915 RVA: 0x000B57CF File Offset: 0x000B47CF
			public long ToLong()
			{
				return (long)((ulong)this.High << 32 | (ulong)this.Low);
			}

			// Token: 0x170008D1 RID: 2257
			// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x000B57E3 File Offset: 0x000B47E3
			public bool IsNull
			{
				get
				{
					return this.Low == 0U && this.High == 0U;
				}
			}

			// Token: 0x04002957 RID: 10583
			public uint Low;

			// Token: 0x04002958 RID: 10584
			public uint High;

			// Token: 0x04002959 RID: 10585
			public static readonly _WinInetCache.FILETIME Zero = new _WinInetCache.FILETIME(0L);
		}

		// Token: 0x02000575 RID: 1397
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct EntryBuffer
		{
			// Token: 0x0400295A RID: 10586
			public static int MarshalSize = Marshal.SizeOf(typeof(_WinInetCache.EntryBuffer));

			// Token: 0x0400295B RID: 10587
			public int StructSize;

			// Token: 0x0400295C RID: 10588
			public IntPtr _OffsetSourceUrlName;

			// Token: 0x0400295D RID: 10589
			public IntPtr _OffsetFileName;

			// Token: 0x0400295E RID: 10590
			public _WinInetCache.EntryType EntryType;

			// Token: 0x0400295F RID: 10591
			public int UseCount;

			// Token: 0x04002960 RID: 10592
			public int HitRate;

			// Token: 0x04002961 RID: 10593
			public int SizeLow;

			// Token: 0x04002962 RID: 10594
			public int SizeHigh;

			// Token: 0x04002963 RID: 10595
			public _WinInetCache.FILETIME LastModifiedTime;

			// Token: 0x04002964 RID: 10596
			public _WinInetCache.FILETIME ExpireTime;

			// Token: 0x04002965 RID: 10597
			public _WinInetCache.FILETIME LastAccessTime;

			// Token: 0x04002966 RID: 10598
			public _WinInetCache.FILETIME LastSyncTime;

			// Token: 0x04002967 RID: 10599
			public IntPtr _OffsetHeaderInfo;

			// Token: 0x04002968 RID: 10600
			public int HeaderInfoChars;

			// Token: 0x04002969 RID: 10601
			public IntPtr _OffsetExtension;

			// Token: 0x0400296A RID: 10602
			public _WinInetCache.EntryBuffer.Rsv U;

			// Token: 0x02000576 RID: 1398
			[StructLayout(LayoutKind.Explicit)]
			public struct Rsv
			{
				// Token: 0x0400296B RID: 10603
				[FieldOffset(0)]
				public int ExemptDelta;

				// Token: 0x0400296C RID: 10604
				[FieldOffset(0)]
				public int Reserved;
			}
		}

		// Token: 0x02000577 RID: 1399
		internal class Entry
		{
			// Token: 0x06002AA7 RID: 10919 RVA: 0x000B581C File Offset: 0x000B481C
			public Entry(string key, int maxHeadersSize)
			{
				this.Key = key;
				this.MaxBufferBytes = maxHeadersSize;
				if (maxHeadersSize != 2147483647 && 2147483647 - (key.Length + _WinInetCache.EntryBuffer.MarshalSize + 1024) * 2 > maxHeadersSize)
				{
					this.MaxBufferBytes += (key.Length + _WinInetCache.EntryBuffer.MarshalSize + 1024) * 2;
				}
				this.Info.EntryType = _WinInetCache.EntryType.NormalEntry;
			}

			// Token: 0x0400296D RID: 10605
			public const int DefaultBufferSize = 2048;

			// Token: 0x0400296E RID: 10606
			public _WinInetCache.Status Error;

			// Token: 0x0400296F RID: 10607
			public string Key;

			// Token: 0x04002970 RID: 10608
			public string Filename;

			// Token: 0x04002971 RID: 10609
			public string FileExt;

			// Token: 0x04002972 RID: 10610
			public int OptionalLength;

			// Token: 0x04002973 RID: 10611
			public string OriginalUrl;

			// Token: 0x04002974 RID: 10612
			public string MetaInfo;

			// Token: 0x04002975 RID: 10613
			public int MaxBufferBytes;

			// Token: 0x04002976 RID: 10614
			public _WinInetCache.EntryBuffer Info;
		}
	}
}
