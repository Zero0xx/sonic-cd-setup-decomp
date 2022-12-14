using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Microsoft.Win32
{
	// Token: 0x02000563 RID: 1379
	internal class WinInetCache : RequestCache
	{
		// Token: 0x06002A39 RID: 10809 RVA: 0x000B2448 File Offset: 0x000B1448
		internal WinInetCache(bool isPrivateCache, bool canWrite, bool async) : base(isPrivateCache, canWrite)
		{
			WinInetCache._MaximumResponseHeadersLength = int.MaxValue;
			this.async = async;
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000B2463 File Offset: 0x000B1463
		internal override Stream Retrieve(string key, out RequestCacheEntry cacheEntry)
		{
			return this.Lookup(key, out cacheEntry, true);
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000B246E File Offset: 0x000B146E
		internal override bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream)
		{
			readStream = this.Lookup(key, out cacheEntry, false);
			return readStream != null;
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000B2484 File Offset: 0x000B1484
		internal override Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			return this.GetWriteStream(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, true);
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000B24A4 File Offset: 0x000B14A4
		internal override bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream)
		{
			writeStream = this.GetWriteStream(key, contentLength, expiresUtc, lastModifiedUtc, maxStale, entryMetadata, systemMetadata, false);
			return writeStream != null;
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x000B24D0 File Offset: 0x000B14D0
		internal override void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.Remove()",
						SR.GetString("net_cache_access_denied", new object[]
						{
							"Write"
						})
					}));
				}
				return;
			}
			_WinInetCache.Entry entry = new _WinInetCache.Entry(key, WinInetCache._MaximumResponseHeadersLength);
			if (_WinInetCache.Remove(entry) != _WinInetCache.Status.Success && entry.Error != _WinInetCache.Status.FileNotFound)
			{
				Win32Exception ex = new Win32Exception((int)entry.Error);
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_cannot_remove", new object[]
					{
						"WinInetCache.Remove()",
						key,
						ex.Message
					}));
				}
				throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
				{
					ex.Message
				}), ex);
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
				{
					"WinInetCache.Remove(), ",
					key,
					entry.Error.ToString()
				}));
			}
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000B260E File Offset: 0x000B160E
		internal override bool TryRemove(string key)
		{
			return this.TryRemove(key, false);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000B2618 File Offset: 0x000B1618
		internal bool TryRemove(string key, bool forceRemove)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.TryRemove()",
						SR.GetString("net_cache_access_denied", new object[]
						{
							"Write"
						})
					}));
				}
				return false;
			}
			_WinInetCache.Entry entry = new _WinInetCache.Entry(key, WinInetCache._MaximumResponseHeadersLength);
			if (_WinInetCache.Remove(entry) == _WinInetCache.Status.Success || entry.Error == _WinInetCache.Status.FileNotFound)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
					{
						"WinInetCache.TryRemove()",
						key,
						entry.Error.ToString()
					}));
				}
				return true;
			}
			if (!forceRemove)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_key_remove_failed_status", new object[]
					{
						"WinInetCache.TryRemove()",
						key,
						entry.Error.ToString()
					}));
				}
				return false;
			}
			if (_WinInetCache.LookupInfo(entry) == _WinInetCache.Status.Success)
			{
				while (entry.Info.UseCount != 0)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
						{
							"WinInetCache.TryRemove()",
							key,
							entry.Error.ToString()
						}));
					}
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_usecount_file", new object[]
						{
							"WinInetCache.TryRemove()",
							entry.Info.UseCount,
							entry.Filename
						}));
					}
					if (!UnsafeNclNativeMethods.UnsafeWinInetCache.UnlockUrlCacheEntryFileW(key, 0))
					{
						break;
					}
					_WinInetCache.Status status = _WinInetCache.LookupInfo(entry);
				}
			}
			_WinInetCache.Remove(entry);
			if (entry.Error != _WinInetCache.Status.Success && _WinInetCache.LookupInfo(entry) == _WinInetCache.Status.FileNotFound)
			{
				entry.Error = _WinInetCache.Status.Success;
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key_status", new object[]
				{
					"WinInetCache.TryRemove()",
					key,
					entry.Error.ToString()
				}));
			}
			return entry.Error == _WinInetCache.Status.Success;
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000B286C File Offset: 0x000B186C
		internal override void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			this.UpdateInfo(key, expiresUtc, lastModifiedUtc, lastSynchronizedUtc, maxStale, entryMetadata, systemMetadata, true);
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000B288C File Offset: 0x000B188C
		internal override bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata)
		{
			return this.UpdateInfo(key, expiresUtc, lastModifiedUtc, lastSynchronizedUtc, maxStale, entryMetadata, systemMetadata, false);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000B28AC File Offset: 0x000B18AC
		internal override void UnlockEntry(Stream stream)
		{
			WinInetCache.ReadStream readStream = stream as WinInetCache.ReadStream;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_stream", new object[]
				{
					"WinInetCache.UnlockEntry",
					(stream == null) ? "<null>" : stream.GetType().FullName
				}));
			}
			if (readStream == null)
			{
				return;
			}
			readStream.UnlockEntry();
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000B2910 File Offset: 0x000B1910
		private unsafe Stream Lookup(string key, out RequestCacheEntry cacheEntry, bool isThrow)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, "WinInetCache.Retrieve", "key = " + key);
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			Stream stream = Stream.Null;
			SafeUnlockUrlCacheEntryFile safeUnlockUrlCacheEntryFile = null;
			_WinInetCache.Entry entry = new _WinInetCache.Entry(key, WinInetCache._MaximumResponseHeadersLength);
			try
			{
				safeUnlockUrlCacheEntryFile = _WinInetCache.LookupFile(entry);
				if (entry.Error == _WinInetCache.Status.Success)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_filename", new object[]
						{
							"WinInetCache.Retrieve()",
							entry.Filename,
							entry.Error
						}));
					}
					cacheEntry = new RequestCacheEntry(entry, base.IsPrivateCache);
					if (entry.MetaInfo != null && entry.MetaInfo.Length != 0)
					{
						int num = 0;
						int length = entry.MetaInfo.Length;
						StringCollection stringCollection = new StringCollection();
						try
						{
							fixed (char* metaInfo = entry.MetaInfo)
							{
								for (int i = 0; i < length; i++)
								{
									if (i == num && i + 2 < length && metaInfo[i] == '~' && (metaInfo[i + 1] == 'U' || metaInfo[i + 1] == 'u') && metaInfo[i + 2] == ':')
									{
										while (i < length && metaInfo[(IntPtr)(++i) * 2] != '\n')
										{
										}
										num = i + 1;
									}
									else if (i + 1 == length || metaInfo[i] == '\n')
									{
										string text = entry.MetaInfo.Substring(num, ((metaInfo[i - 1] == '\r') ? (i - 1) : (i + 1)) - num);
										if (text.Length == 0 && cacheEntry.EntryMetadata == null)
										{
											cacheEntry.EntryMetadata = stringCollection;
											stringCollection = new StringCollection();
										}
										else if (cacheEntry.EntryMetadata != null && text.StartsWith("~SPARSE_ENTRY:", StringComparison.Ordinal))
										{
											cacheEntry.IsPartialEntry = true;
										}
										else
										{
											stringCollection.Add(text);
										}
										num = i + 1;
									}
								}
							}
						}
						finally
						{
							string text2 = null;
						}
						if (cacheEntry.EntryMetadata == null)
						{
							cacheEntry.EntryMetadata = stringCollection;
						}
						else
						{
							cacheEntry.SystemMetadata = stringCollection;
						}
					}
					stream = new WinInetCache.ReadStream(entry, safeUnlockUrlCacheEntryFile, this.async);
				}
				else
				{
					if (safeUnlockUrlCacheEntryFile != null)
					{
						safeUnlockUrlCacheEntryFile.Close();
					}
					cacheEntry = new RequestCacheEntry();
					cacheEntry.IsPrivateEntry = base.IsPrivateCache;
					if (entry.Error != _WinInetCache.Status.FileNotFound)
					{
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_lookup_failed", new object[]
							{
								"WinInetCache.Retrieve()",
								new Win32Exception((int)entry.Error).Message
							}));
						}
						if (Logging.On)
						{
							Logging.Exit(Logging.RequestCache, "WinInetCache.Retrieve()");
						}
						if (isThrow)
						{
							Win32Exception ex = new Win32Exception((int)entry.Error);
							throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
							{
								ex.Message
							}), ex);
						}
						return null;
					}
				}
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_exception", new object[]
					{
						"WinInetCache.Retrieve()",
						ex2.ToString()
					}));
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, "WinInetCache.Retrieve()");
				}
				if (safeUnlockUrlCacheEntryFile != null)
				{
					safeUnlockUrlCacheEntryFile.Close();
				}
				stream.Close();
				stream = Stream.Null;
				cacheEntry = new RequestCacheEntry();
				cacheEntry.IsPrivateEntry = base.IsPrivateCache;
				if (isThrow)
				{
					throw;
				}
				return null;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.RequestCache, "WinInetCache.Retrieve()", "Status = " + entry.Error.ToString());
			}
			return stream;
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000B2D28 File Offset: 0x000B1D28
		private string CombineMetaInfo(StringCollection entryMetadata, StringCollection systemMetadata)
		{
			if ((entryMetadata == null || entryMetadata.Count == 0) && (systemMetadata == null || systemMetadata.Count == 0))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(100);
			if (entryMetadata != null && entryMetadata.Count != 0)
			{
				for (int i = 0; i < entryMetadata.Count; i++)
				{
					if (entryMetadata[i] != null && entryMetadata[i].Length != 0)
					{
						stringBuilder.Append(entryMetadata[i]).Append("\r\n");
					}
				}
			}
			if (systemMetadata != null && systemMetadata.Count != 0)
			{
				stringBuilder.Append("\r\n");
				for (int i = 0; i < systemMetadata.Count; i++)
				{
					if (systemMetadata[i] != null && systemMetadata[i].Length != 0)
					{
						stringBuilder.Append(systemMetadata[i]).Append("\r\n");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000B2E04 File Offset: 0x000B1E04
		private Stream GetWriteStream(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, bool isThrow)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, "WinInetCache.Store()", "Key = " + key);
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.Store()",
						SR.GetString("net_cache_access_denied", new object[]
						{
							"Write"
						})
					}));
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, "WinInetCache.Store");
				}
				if (isThrow)
				{
					throw new InvalidOperationException(SR.GetString("net_cache_access_denied", new object[]
					{
						"Write"
					}));
				}
				return null;
			}
			else
			{
				_WinInetCache.Entry entry = new _WinInetCache.Entry(key, WinInetCache._MaximumResponseHeadersLength);
				entry.Key = key;
				entry.OptionalLength = ((contentLength < 0L) ? 0 : ((contentLength > 2147483647L) ? int.MaxValue : ((int)contentLength)));
				entry.Info.ExpireTime = _WinInetCache.FILETIME.Zero;
				if (expiresUtc != DateTime.MinValue && expiresUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry.Info.ExpireTime = new _WinInetCache.FILETIME(expiresUtc.ToFileTimeUtc());
				}
				entry.Info.LastModifiedTime = _WinInetCache.FILETIME.Zero;
				if (lastModifiedUtc != DateTime.MinValue && lastModifiedUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry.Info.LastModifiedTime = new _WinInetCache.FILETIME(lastModifiedUtc.ToFileTimeUtc());
				}
				entry.Info.EntryType = _WinInetCache.EntryType.NormalEntry;
				if (maxStale > TimeSpan.Zero)
				{
					if (maxStale >= WinInetCache.s_MaxTimeSpanForInt32)
					{
						maxStale = WinInetCache.s_MaxTimeSpanForInt32;
					}
					entry.Info.U.ExemptDelta = (int)maxStale.TotalSeconds;
					entry.Info.EntryType = _WinInetCache.EntryType.StickyEntry;
				}
				entry.MetaInfo = this.CombineMetaInfo(entryMetadata, systemMetadata);
				entry.FileExt = "cache";
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_expected_length", new object[]
					{
						entry.OptionalLength
					}));
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_last_modified", new object[]
					{
						entry.Info.LastModifiedTime.IsNull ? "0" : DateTime.FromFileTimeUtc(entry.Info.LastModifiedTime.ToLong()).ToString("r")
					}));
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_expires", new object[]
					{
						entry.Info.ExpireTime.IsNull ? "0" : DateTime.FromFileTimeUtc(entry.Info.ExpireTime.ToLong()).ToString("r")
					}));
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_max_stale", new object[]
					{
						(maxStale > TimeSpan.Zero) ? ((int)maxStale.TotalSeconds).ToString() : "n/a"
					}));
					if (Logging.IsVerbose(Logging.RequestCache))
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping_metadata"));
						if (entry.MetaInfo.Length == 0)
						{
							Logging.PrintInfo(Logging.RequestCache, "<null>");
						}
						else
						{
							if (entryMetadata != null)
							{
								foreach (string text in entryMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text.TrimEnd(RequestCache.LineSplits));
								}
							}
							Logging.PrintInfo(Logging.RequestCache, "------");
							if (systemMetadata != null)
							{
								foreach (string text2 in systemMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text2.TrimEnd(RequestCache.LineSplits));
								}
							}
						}
					}
				}
				_WinInetCache.CreateFileName(entry);
				Stream result = Stream.Null;
				if (entry.Error == _WinInetCache.Status.Success)
				{
					try
					{
						result = new WinInetCache.WriteStream(entry, isThrow, contentLength, this.async);
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_exception", new object[]
							{
								"WinInetCache.Store()",
								ex
							}));
							Logging.Exit(Logging.RequestCache, "WinInetCache.Store");
						}
						if (isThrow)
						{
							throw;
						}
						return null;
					}
					if (Logging.On)
					{
						Logging.Exit(Logging.RequestCache, "WinInetCache.Store", "Filename = " + entry.Filename);
					}
					return result;
				}
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_create_failed", new object[]
					{
						new Win32Exception((int)entry.Error).Message
					}));
					Logging.Exit(Logging.RequestCache, "WinInetCache.Store");
				}
				if (isThrow)
				{
					Win32Exception ex2 = new Win32Exception((int)entry.Error);
					throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
					{
						ex2.Message
					}), ex2);
				}
				return null;
			}
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000B3394 File Offset: 0x000B2394
		private bool UpdateInfo(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, bool isThrow)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, "WinInetCache.Update", "Key = " + key);
			}
			if (!base.CanWrite)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_operation_failed_with_error", new object[]
					{
						"WinInetCache.Update()",
						SR.GetString("net_cache_access_denied", new object[]
						{
							"Write"
						})
					}));
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, "WinInetCache.Update()");
				}
				if (isThrow)
				{
					throw new InvalidOperationException(SR.GetString("net_cache_access_denied", new object[]
					{
						"Write"
					}));
				}
				return false;
			}
			else
			{
				_WinInetCache.Entry entry = new _WinInetCache.Entry(key, WinInetCache._MaximumResponseHeadersLength);
				_WinInetCache.Entry_FC entry_FC = _WinInetCache.Entry_FC.None;
				if (expiresUtc != DateTime.MinValue && expiresUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry_FC |= _WinInetCache.Entry_FC.Exptime;
					entry.Info.ExpireTime = new _WinInetCache.FILETIME(expiresUtc.ToFileTimeUtc());
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_set_expires", new object[]
						{
							expiresUtc.ToString("r")
						}));
					}
				}
				if (lastModifiedUtc != DateTime.MinValue && lastModifiedUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry_FC |= _WinInetCache.Entry_FC.Modtime;
					entry.Info.LastModifiedTime = new _WinInetCache.FILETIME(lastModifiedUtc.ToFileTimeUtc());
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_set_last_modified", new object[]
						{
							lastModifiedUtc.ToString("r")
						}));
					}
				}
				if (lastSynchronizedUtc != DateTime.MinValue && lastSynchronizedUtc > WinInetCache.s_MinDateTimeUtcForFileTimeUtc)
				{
					entry_FC |= _WinInetCache.Entry_FC.Synctime;
					entry.Info.LastSyncTime = new _WinInetCache.FILETIME(lastSynchronizedUtc.ToFileTimeUtc());
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_set_last_synchronized", new object[]
						{
							lastSynchronizedUtc.ToString("r")
						}));
					}
				}
				if (maxStale != TimeSpan.MinValue)
				{
					entry_FC |= (_WinInetCache.Entry_FC.Attribute | _WinInetCache.Entry_FC.ExemptDelta);
					entry.Info.EntryType = _WinInetCache.EntryType.NormalEntry;
					if (maxStale >= TimeSpan.Zero)
					{
						if (maxStale >= WinInetCache.s_MaxTimeSpanForInt32)
						{
							maxStale = WinInetCache.s_MaxTimeSpanForInt32;
						}
						entry.Info.EntryType = _WinInetCache.EntryType.StickyEntry;
						entry.Info.U.ExemptDelta = (int)maxStale.TotalSeconds;
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_enable_max_stale", new object[]
							{
								((int)maxStale.TotalSeconds).ToString()
							}));
						}
					}
					else
					{
						entry.Info.U.ExemptDelta = 0;
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_disable_max_stale"));
						}
					}
				}
				entry.MetaInfo = this.CombineMetaInfo(entryMetadata, systemMetadata);
				if (entry.MetaInfo.Length != 0)
				{
					entry_FC |= _WinInetCache.Entry_FC.Headerinfo;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping"));
						if (Logging.IsVerbose(Logging.RequestCache))
						{
							Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping"));
							if (entryMetadata != null)
							{
								foreach (string text in entryMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text.TrimEnd(RequestCache.LineSplits));
								}
							}
							Logging.PrintInfo(Logging.RequestCache, "------");
							if (systemMetadata != null)
							{
								foreach (string text2 in systemMetadata)
								{
									Logging.PrintInfo(Logging.RequestCache, text2.TrimEnd(RequestCache.LineSplits));
								}
							}
						}
					}
				}
				_WinInetCache.Update(entry, entry_FC);
				if (entry.Error == _WinInetCache.Status.Success)
				{
					if (Logging.On)
					{
						Logging.Exit(Logging.RequestCache, "WinInetCache.Update()", "Status = " + entry.Error.ToString());
					}
					return true;
				}
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_update_failed", new object[]
					{
						"WinInetCache.Update()",
						entry.Key,
						new Win32Exception((int)entry.Error).Message
					}));
					Logging.Exit(Logging.RequestCache, "WinInetCache.Update()");
				}
				if (isThrow)
				{
					Win32Exception ex = new Win32Exception((int)entry.Error);
					throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
					{
						ex.Message
					}), ex);
				}
				return false;
			}
		}

		// Token: 0x040028DB RID: 10459
		internal const string c_SPARSE_ENTRY_HACK = "~SPARSE_ENTRY:";

		// Token: 0x040028DC RID: 10460
		private static int _MaximumResponseHeadersLength;

		// Token: 0x040028DD RID: 10461
		private bool async;

		// Token: 0x040028DE RID: 10462
		private static readonly DateTime s_MinDateTimeUtcForFileTimeUtc = DateTime.FromFileTimeUtc(0L);

		// Token: 0x040028DF RID: 10463
		internal static readonly TimeSpan s_MaxTimeSpanForInt32 = TimeSpan.FromSeconds(2147483647.0);

		// Token: 0x02000564 RID: 1380
		private class ReadStream : FileStream, ICloseEx
		{
			// Token: 0x06002A49 RID: 10825 RVA: 0x000B38C0 File Offset: 0x000B28C0
			[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
			internal ReadStream(_WinInetCache.Entry entry, SafeUnlockUrlCacheEntryFile handle, bool async) : base(entry.Filename, FileMode.Open, FileAccess.Read, ComNetOS.IsWinNt ? (FileShare.Read | FileShare.Delete) : FileShare.Read, 4096, async)
			{
				this.m_Key = entry.Key;
				this.m_Handle = handle;
				this.m_ReadTimeout = (this.m_WriteTimeout = -1);
			}

			// Token: 0x06002A4A RID: 10826 RVA: 0x000B390F File Offset: 0x000B290F
			internal void UnlockEntry()
			{
				this.m_Handle.Close();
			}

			// Token: 0x06002A4B RID: 10827 RVA: 0x000B391C File Offset: 0x000B291C
			public override int Read(byte[] buffer, int offset, int count)
			{
				int result;
				lock (this.m_Handle)
				{
					try
					{
						if (this.m_CallNesting != 0)
						{
							throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
						}
						if (this.m_Aborted)
						{
							throw ExceptionHelper.RequestAbortedException;
						}
						if (this.m_Event != null)
						{
							throw new ObjectDisposedException(base.GetType().FullName);
						}
						this.m_CallNesting = 1;
						result = base.Read(buffer, offset, count);
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							this.m_Event.Set();
						}
					}
				}
				return result;
			}

			// Token: 0x06002A4C RID: 10828 RVA: 0x000B39C8 File Offset: 0x000B29C8
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				IAsyncResult result;
				lock (this.m_Handle)
				{
					if (this.m_CallNesting != 0)
					{
						throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
					}
					if (this.m_Aborted)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					if (this.m_Event != null)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
					this.m_CallNesting = 1;
					try
					{
						result = base.BeginRead(buffer, offset, count, callback, state);
					}
					catch
					{
						this.m_CallNesting = 0;
						throw;
					}
				}
				return result;
			}

			// Token: 0x06002A4D RID: 10829 RVA: 0x000B3A68 File Offset: 0x000B2A68
			public override int EndRead(IAsyncResult asyncResult)
			{
				int result;
				lock (this.m_Handle)
				{
					try
					{
						result = base.EndRead(asyncResult);
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							try
							{
								this.m_Event.Set();
							}
							catch
							{
							}
						}
					}
				}
				return result;
			}

			// Token: 0x06002A4E RID: 10830 RVA: 0x000B3AE0 File Offset: 0x000B2AE0
			public void CloseEx(CloseExState closeState)
			{
				if ((closeState & CloseExState.Abort) != CloseExState.Normal)
				{
					this.m_Aborted = true;
				}
				try
				{
					this.Close();
				}
				catch
				{
					if ((closeState & CloseExState.Silent) == CloseExState.Normal)
					{
						throw;
					}
				}
			}

			// Token: 0x06002A4F RID: 10831 RVA: 0x000B3B1C File Offset: 0x000B2B1C
			protected override void Dispose(bool disposing)
			{
				if (Interlocked.Exchange(ref this.m_Disposed, 1) == 0 && this.m_Key != null)
				{
					try
					{
						lock (this.m_Handle)
						{
							if (this.m_CallNesting == 0)
							{
								base.Dispose(disposing);
							}
							else
							{
								this.m_Event = new ManualResetEvent(false);
							}
						}
						if (disposing && this.m_Event != null)
						{
							using (this.m_Event)
							{
								this.m_Event.WaitOne();
								lock (this.m_Handle)
								{
								}
							}
							base.Dispose(disposing);
						}
					}
					finally
					{
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_key", new object[]
							{
								"WinInetReadStream.Close()",
								this.m_Key
							}));
						}
						this.m_Handle.Close();
					}
				}
			}

			// Token: 0x170008B4 RID: 2228
			// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000B3C38 File Offset: 0x000B2C38
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008B5 RID: 2229
			// (get) Token: 0x06002A51 RID: 10833 RVA: 0x000B3C3B File Offset: 0x000B2C3B
			// (set) Token: 0x06002A52 RID: 10834 RVA: 0x000B3C43 File Offset: 0x000B2C43
			public override int ReadTimeout
			{
				get
				{
					return this.m_ReadTimeout;
				}
				set
				{
					this.m_ReadTimeout = value;
				}
			}

			// Token: 0x170008B6 RID: 2230
			// (get) Token: 0x06002A53 RID: 10835 RVA: 0x000B3C4C File Offset: 0x000B2C4C
			// (set) Token: 0x06002A54 RID: 10836 RVA: 0x000B3C54 File Offset: 0x000B2C54
			public override int WriteTimeout
			{
				get
				{
					return this.m_WriteTimeout;
				}
				set
				{
					this.m_WriteTimeout = value;
				}
			}

			// Token: 0x040028E0 RID: 10464
			private string m_Key;

			// Token: 0x040028E1 RID: 10465
			private int m_ReadTimeout;

			// Token: 0x040028E2 RID: 10466
			private int m_WriteTimeout;

			// Token: 0x040028E3 RID: 10467
			private SafeUnlockUrlCacheEntryFile m_Handle;

			// Token: 0x040028E4 RID: 10468
			private int m_Disposed;

			// Token: 0x040028E5 RID: 10469
			private int m_CallNesting;

			// Token: 0x040028E6 RID: 10470
			private ManualResetEvent m_Event;

			// Token: 0x040028E7 RID: 10471
			private bool m_Aborted;
		}

		// Token: 0x02000565 RID: 1381
		private class WriteStream : FileStream, ICloseEx
		{
			// Token: 0x06002A55 RID: 10837 RVA: 0x000B3C60 File Offset: 0x000B2C60
			[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
			internal WriteStream(_WinInetCache.Entry entry, bool isThrow, long streamSize, bool async) : base(entry.Filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, async)
			{
				this.m_Entry = entry;
				this.m_IsThrow = isThrow;
				this.m_StreamSize = streamSize;
				this.m_OneWriteSucceeded = (streamSize == 0L);
				this.m_ReadTimeout = (this.m_WriteTimeout = -1);
			}

			// Token: 0x170008B7 RID: 2231
			// (get) Token: 0x06002A56 RID: 10838 RVA: 0x000B3CB3 File Offset: 0x000B2CB3
			public override bool CanTimeout
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008B8 RID: 2232
			// (get) Token: 0x06002A57 RID: 10839 RVA: 0x000B3CB6 File Offset: 0x000B2CB6
			// (set) Token: 0x06002A58 RID: 10840 RVA: 0x000B3CBE File Offset: 0x000B2CBE
			public override int ReadTimeout
			{
				get
				{
					return this.m_ReadTimeout;
				}
				set
				{
					this.m_ReadTimeout = value;
				}
			}

			// Token: 0x170008B9 RID: 2233
			// (get) Token: 0x06002A59 RID: 10841 RVA: 0x000B3CC7 File Offset: 0x000B2CC7
			// (set) Token: 0x06002A5A RID: 10842 RVA: 0x000B3CCF File Offset: 0x000B2CCF
			public override int WriteTimeout
			{
				get
				{
					return this.m_WriteTimeout;
				}
				set
				{
					this.m_WriteTimeout = value;
				}
			}

			// Token: 0x06002A5B RID: 10843 RVA: 0x000B3CD8 File Offset: 0x000B2CD8
			public override void Write(byte[] buffer, int offset, int count)
			{
				lock (this.m_Entry)
				{
					if (this.m_Aborted)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					if (this.m_Event != null)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
					this.m_CallNesting = 1;
					try
					{
						base.Write(buffer, offset, count);
						if (this.m_StreamSize > 0L)
						{
							this.m_StreamSize -= (long)count;
						}
						if (!this.m_OneWriteSucceeded && count != 0)
						{
							this.m_OneWriteSucceeded = true;
						}
					}
					catch
					{
						this.m_Aborted = true;
						throw;
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							this.m_Event.Set();
						}
					}
				}
			}

			// Token: 0x06002A5C RID: 10844 RVA: 0x000B3DB0 File Offset: 0x000B2DB0
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				IAsyncResult result;
				lock (this.m_Entry)
				{
					if (this.m_CallNesting != 0)
					{
						throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
					}
					if (this.m_Aborted)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					if (this.m_Event != null)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
					this.m_CallNesting = 1;
					try
					{
						if (this.m_StreamSize > 0L)
						{
							this.m_StreamSize -= (long)count;
						}
						result = base.BeginWrite(buffer, offset, count, callback, state);
					}
					catch
					{
						this.m_Aborted = true;
						this.m_CallNesting = 0;
						throw;
					}
				}
				return result;
			}

			// Token: 0x06002A5D RID: 10845 RVA: 0x000B3E70 File Offset: 0x000B2E70
			public override void EndWrite(IAsyncResult asyncResult)
			{
				lock (this.m_Entry)
				{
					try
					{
						base.EndWrite(asyncResult);
						if (!this.m_OneWriteSucceeded)
						{
							this.m_OneWriteSucceeded = true;
						}
					}
					catch
					{
						this.m_Aborted = true;
						throw;
					}
					finally
					{
						this.m_CallNesting = 0;
						if (this.m_Event != null)
						{
							try
							{
								this.m_Event.Set();
							}
							catch
							{
							}
						}
					}
				}
			}

			// Token: 0x06002A5E RID: 10846 RVA: 0x000B3F0C File Offset: 0x000B2F0C
			public void CloseEx(CloseExState closeState)
			{
				if ((closeState & CloseExState.Abort) != CloseExState.Normal)
				{
					this.m_Aborted = true;
				}
				try
				{
					this.Close();
				}
				catch
				{
					if ((closeState & CloseExState.Silent) == CloseExState.Normal)
					{
						throw;
					}
				}
			}

			// Token: 0x06002A5F RID: 10847 RVA: 0x000B3F48 File Offset: 0x000B2F48
			protected override void Dispose(bool disposing)
			{
				if (Interlocked.Exchange(ref this.m_Disposed, 1) == 0 && this.m_Entry != null)
				{
					lock (this.m_Entry)
					{
						if (this.m_CallNesting == 0)
						{
							base.Dispose(disposing);
						}
						else
						{
							this.m_Event = new ManualResetEvent(false);
						}
					}
					if (disposing && this.m_Event != null)
					{
						using (this.m_Event)
						{
							this.m_Event.WaitOne();
							lock (this.m_Entry)
							{
							}
						}
						base.Dispose(disposing);
					}
					TriState triState;
					if (this.m_StreamSize < 0L)
					{
						if (this.m_Aborted)
						{
							if (this.m_OneWriteSucceeded)
							{
								triState = TriState.Unspecified;
							}
							else
							{
								triState = TriState.False;
							}
						}
						else
						{
							triState = TriState.True;
						}
					}
					else if (!this.m_OneWriteSucceeded)
					{
						triState = TriState.False;
					}
					else if (this.m_StreamSize > 0L)
					{
						triState = TriState.Unspecified;
					}
					else
					{
						triState = TriState.True;
					}
					if (triState == TriState.False)
					{
						try
						{
							if (Logging.On)
							{
								Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_no_commit", new object[]
								{
									"WinInetWriteStream.Close()"
								}));
							}
							File.Delete(this.m_Entry.Filename);
						}
						catch (Exception ex)
						{
							if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
							{
								throw;
							}
							if (Logging.On)
							{
								Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_error_deleting_filename", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Filename
								}));
							}
						}
						finally
						{
							_WinInetCache.Status status = _WinInetCache.Remove(this.m_Entry);
							if (status != _WinInetCache.Status.Success && status != _WinInetCache.Status.FileNotFound && Logging.On)
							{
								Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_delete_failed", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Key,
									new Win32Exception((int)this.m_Entry.Error).Message
								}));
							}
							this.m_Entry = null;
						}
						return;
					}
					this.m_Entry.OriginalUrl = null;
					if (triState == TriState.Unspecified)
					{
						if (this.m_Entry.MetaInfo == null || this.m_Entry.MetaInfo.Length == 0 || (this.m_Entry.MetaInfo != "\r\n" && this.m_Entry.MetaInfo.IndexOf("\r\n\r\n", StringComparison.Ordinal) == -1))
						{
							this.m_Entry.MetaInfo = "\r\n~SPARSE_ENTRY:\r\n";
						}
						else
						{
							_WinInetCache.Entry entry3 = this.m_Entry;
							entry3.MetaInfo += "~SPARSE_ENTRY:\r\n";
						}
					}
					if (_WinInetCache.Commit(this.m_Entry) != _WinInetCache.Status.Success)
					{
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_commit_failed", new object[]
							{
								"WinInetWriteStream.Close()",
								this.m_Entry.Key,
								new Win32Exception((int)this.m_Entry.Error).Message
							}));
						}
						try
						{
							File.Delete(this.m_Entry.Filename);
						}
						catch (Exception ex2)
						{
							if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
							{
								throw;
							}
							if (Logging.On)
							{
								Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_error_deleting_filename", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Filename
								}));
							}
						}
						if (this.m_IsThrow)
						{
							Win32Exception ex3 = new Win32Exception((int)this.m_Entry.Error);
							throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
							{
								ex3.Message
							}), ex3);
						}
						return;
					}
					else
					{
						if (Logging.On)
						{
							if (this.m_StreamSize > 0L || (this.m_StreamSize < 0L && this.m_Aborted))
							{
								Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_committed_as_partial", new object[]
								{
									"WinInetWriteStream.Close()",
									this.m_Entry.Key,
									(this.m_StreamSize > 0L) ? this.m_StreamSize.ToString(CultureInfo.CurrentCulture) : SR.GetString("net_log_unknown")
								}));
							}
							Logging.PrintInfo(Logging.RequestCache, "WinInetWriteStream.Close(), Key = " + this.m_Entry.Key + ", Commit Status = " + this.m_Entry.Error.ToString());
						}
						if ((this.m_Entry.Info.EntryType & _WinInetCache.EntryType.StickyEntry) == _WinInetCache.EntryType.StickyEntry)
						{
							if (_WinInetCache.Update(this.m_Entry, _WinInetCache.Entry_FC.ExemptDelta) != _WinInetCache.Status.Success)
							{
								if (Logging.On)
								{
									Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_update_failed", new object[]
									{
										"WinInetWriteStream.Close(), Key = " + this.m_Entry.Key,
										new Win32Exception((int)this.m_Entry.Error).Message
									}));
								}
								if (this.m_IsThrow)
								{
									Win32Exception ex4 = new Win32Exception((int)this.m_Entry.Error);
									throw new IOException(SR.GetString("net_cache_retrieve_failure", new object[]
									{
										ex4.Message
									}), ex4);
								}
								return;
							}
							else if (Logging.On)
							{
								Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_max_stale_and_update_status", new object[]
								{
									"WinInetWriteFile.Close()",
									this.m_Entry.Info.U.ExemptDelta,
									this.m_Entry.Error.ToString()
								}));
							}
						}
						base.Dispose(disposing);
					}
				}
			}

			// Token: 0x040028E8 RID: 10472
			private _WinInetCache.Entry m_Entry;

			// Token: 0x040028E9 RID: 10473
			private bool m_IsThrow;

			// Token: 0x040028EA RID: 10474
			private long m_StreamSize;

			// Token: 0x040028EB RID: 10475
			private bool m_Aborted;

			// Token: 0x040028EC RID: 10476
			private int m_ReadTimeout;

			// Token: 0x040028ED RID: 10477
			private int m_WriteTimeout;

			// Token: 0x040028EE RID: 10478
			private int m_Disposed;

			// Token: 0x040028EF RID: 10479
			private int m_CallNesting;

			// Token: 0x040028F0 RID: 10480
			private ManualResetEvent m_Event;

			// Token: 0x040028F1 RID: 10481
			private bool m_OneWriteSucceeded;
		}
	}
}
