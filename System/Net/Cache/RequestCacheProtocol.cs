using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200057E RID: 1406
	internal class RequestCacheProtocol
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x000B7173 File Offset: 0x000B6173
		internal CacheValidationStatus ProtocolStatus
		{
			get
			{
				return this._ProtocolStatus;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x000B717B File Offset: 0x000B617B
		internal Exception ProtocolException
		{
			get
			{
				return this._ProtocolException;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002B10 RID: 11024 RVA: 0x000B7183 File Offset: 0x000B6183
		internal Stream ResponseStream
		{
			get
			{
				return this._ResponseStream;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000B718B File Offset: 0x000B618B
		internal long ResponseStreamLength
		{
			get
			{
				return this._ResponseStreamLength;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002B12 RID: 11026 RVA: 0x000B7193 File Offset: 0x000B6193
		internal RequestCacheValidator Validator
		{
			get
			{
				return this._Validator;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002B13 RID: 11027 RVA: 0x000B719B File Offset: 0x000B619B
		internal bool IsCacheFresh
		{
			get
			{
				return this._Validator != null && this._Validator.CacheFreshnessStatus == CacheFreshnessStatus.Fresh;
			}
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x000B71B5 File Offset: 0x000B61B5
		internal RequestCacheProtocol(RequestCache cache, RequestCacheValidator defaultValidator)
		{
			this._RequestCache = cache;
			this._Validator = defaultValidator;
			this._CanTakeNewRequest = true;
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x000B71D4 File Offset: 0x000B61D4
		internal CacheValidationStatus GetRetrieveStatus(Uri cacheUri, WebRequest request)
		{
			if (cacheUri == null)
			{
				throw new ArgumentNullException("cacheUri");
			}
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (!this._CanTakeNewRequest || this._ProtocolStatus == CacheValidationStatus.RetryResponseFromServer)
			{
				return CacheValidationStatus.Continue;
			}
			this._CanTakeNewRequest = false;
			this._ResponseStream = null;
			this._ResponseStreamLength = 0L;
			this._ProtocolStatus = CacheValidationStatus.Continue;
			this._ProtocolException = null;
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, this, "GetRetrieveStatus", request);
			}
			try
			{
				if (request.CachePolicy == null || request.CachePolicy.Level == RequestCacheLevel.BypassCache)
				{
					this._ProtocolStatus = CacheValidationStatus.DoNotUseCache;
					return this._ProtocolStatus;
				}
				if (this._RequestCache == null || this._Validator == null)
				{
					this._ProtocolStatus = CacheValidationStatus.DoNotUseCache;
					return this._ProtocolStatus;
				}
				this._Validator.FetchRequest(cacheUri, request);
				CacheValidationStatus cacheValidationStatus = this._ProtocolStatus = this.ValidateRequest();
				switch (cacheValidationStatus)
				{
				case CacheValidationStatus.DoNotUseCache:
				case CacheValidationStatus.DoNotTakeFromCache:
					break;
				case CacheValidationStatus.Fail:
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[]
					{
						"ValidateRequest"
					}));
					break;
				default:
					if (cacheValidationStatus != CacheValidationStatus.Continue)
					{
						this._ProtocolStatus = CacheValidationStatus.Fail;
						this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
						{
							"ValidateRequest",
							this._Validator.ValidationStatus.ToString()
						}));
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
							{
								"ValidateRequest()",
								this._Validator.ValidationStatus.ToString()
							}));
						}
					}
					break;
				}
				if (this._ProtocolStatus != CacheValidationStatus.Continue)
				{
					return this._ProtocolStatus;
				}
				this.CheckRetrieveBeforeSubmit();
			}
			catch (Exception ex)
			{
				this._ProtocolException = ex;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, this, "GetRetrieveStatus", "result = " + this._ProtocolStatus.ToString());
				}
			}
			return this._ProtocolStatus;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000B74B4 File Offset: 0x000B64B4
		internal CacheValidationStatus GetRevalidateStatus(WebResponse response, Stream responseStream)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			if (this._ProtocolStatus == CacheValidationStatus.DoNotUseCache)
			{
				return CacheValidationStatus.DoNotUseCache;
			}
			if (this._ProtocolStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				this._ProtocolStatus = CacheValidationStatus.DoNotUseCache;
				return this._ProtocolStatus;
			}
			try
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.RequestCache, this, "GetRevalidateStatus", (this._Validator == null) ? null : this._Validator.Request);
				}
				this._Validator.FetchResponse(response);
				if (this._ProtocolStatus != CacheValidationStatus.Continue && this._ProtocolStatus != CacheValidationStatus.RetryResponseFromServer)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_revalidation_not_needed", new object[]
						{
							"GetRevalidateStatus()"
						}));
					}
					return this._ProtocolStatus;
				}
				this.CheckRetrieveOnResponse(responseStream);
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, this, "GetRevalidateStatus", "result = " + this._ProtocolStatus.ToString());
				}
			}
			return this._ProtocolStatus;
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000B75C4 File Offset: 0x000B65C4
		internal CacheValidationStatus GetUpdateStatus(WebResponse response, Stream responseStream)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			if (this._ProtocolStatus == CacheValidationStatus.DoNotUseCache)
			{
				return CacheValidationStatus.DoNotUseCache;
			}
			try
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.RequestCache, this, "GetUpdateStatus", null);
				}
				if (this._Validator.Response == null)
				{
					this._Validator.FetchResponse(response);
				}
				if (this._ProtocolStatus == CacheValidationStatus.RemoveFromCache)
				{
					this.EnsureCacheRemoval(this._Validator.CacheKey);
					return this._ProtocolStatus;
				}
				if (this._ProtocolStatus != CacheValidationStatus.DoNotTakeFromCache && this._ProtocolStatus != CacheValidationStatus.ReturnCachedResponse && this._ProtocolStatus != CacheValidationStatus.CombineCachedAndServerResponse)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_based_on_cache_protocol_status", new object[]
						{
							"GetUpdateStatus()",
							this._ProtocolStatus.ToString()
						}));
					}
					return this._ProtocolStatus;
				}
				this.CheckUpdateOnResponse(responseStream);
			}
			catch (Exception ex)
			{
				this._ProtocolException = ex;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, this, "GetUpdateStatus", "result = " + this._ProtocolStatus.ToString());
				}
			}
			return this._ProtocolStatus;
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000B77AC File Offset: 0x000B67AC
		internal void Reset()
		{
			this._CanTakeNewRequest = true;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000B77B8 File Offset: 0x000B67B8
		internal void Abort()
		{
			if (this._CanTakeNewRequest)
			{
				return;
			}
			Stream responseStream = this._ResponseStream;
			if (responseStream != null)
			{
				try
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_closing_cache_stream", new object[]
						{
							"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
							"Abort()",
							responseStream.GetType().FullName,
							this._Validator.CacheKey
						}));
					}
					ICloseEx closeEx = responseStream as ICloseEx;
					if (closeEx != null)
					{
						closeEx.CloseEx(CloseExState.Abort | CloseExState.Silent);
					}
					else
					{
						responseStream.Close();
					}
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_exception_ignored", new object[]
						{
							"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
							"stream.Close()",
							ex.ToString()
						}));
					}
				}
			}
			this.Reset();
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000B78DC File Offset: 0x000B68DC
		private void CheckRetrieveBeforeSubmit()
		{
			try
			{
				CacheValidationStatus protocolStatus;
				for (;;)
				{
					if (this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
					{
						this._Validator.CacheStream.Close();
						this._Validator.CacheStream = Stream.Null;
					}
					RequestCacheEntry requestCacheEntry;
					if (this._Validator.StrictCacheErrors)
					{
						this._Validator.CacheStream = this._RequestCache.Retrieve(this._Validator.CacheKey, out requestCacheEntry);
					}
					else
					{
						Stream cacheStream;
						this._RequestCache.TryRetrieve(this._Validator.CacheKey, out requestCacheEntry, out cacheStream);
						this._Validator.CacheStream = cacheStream;
					}
					if (requestCacheEntry == null)
					{
						requestCacheEntry = new RequestCacheEntry();
						requestCacheEntry.IsPrivateEntry = this._RequestCache.IsPrivateCache;
						this._Validator.FetchCacheEntry(requestCacheEntry);
					}
					if (this._Validator.CacheStream == null)
					{
						this._Validator.CacheStream = Stream.Null;
					}
					this.ValidateFreshness(requestCacheEntry);
					this._ProtocolStatus = this.ValidateCache();
					protocolStatus = this._ProtocolStatus;
					switch (protocolStatus)
					{
					case CacheValidationStatus.DoNotUseCache:
					case CacheValidationStatus.DoNotTakeFromCache:
						goto IL_35A;
					case CacheValidationStatus.Fail:
						goto IL_29E;
					case CacheValidationStatus.RetryResponseFromCache:
						continue;
					case CacheValidationStatus.RetryResponseFromServer:
						goto IL_2CB;
					case CacheValidationStatus.ReturnCachedResponse:
						goto IL_123;
					}
					break;
				}
				if (protocolStatus != CacheValidationStatus.Continue)
				{
					goto IL_2CB;
				}
				this._ResponseStream = this._Validator.CacheStream;
				goto IL_35A;
				IL_123:
				if (this._Validator.CacheStream == null || this._Validator.CacheStream == Stream.Null)
				{
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_no_cache_entry", new object[]
						{
							"ValidateCache()"
						}));
					}
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_no_stream", new object[]
					{
						this._Validator.CacheKey
					}));
					goto IL_35A;
				}
				Stream stream = this._Validator.CacheStream;
				this._RequestCache.UnlockEntry(this._Validator.CacheStream);
				if (this._Validator.CacheStreamOffset != 0L || this._Validator.CacheStreamLength != this._Validator.CacheEntry.StreamSize)
				{
					stream = new RangeStream(stream, this._Validator.CacheStreamOffset, this._Validator.CacheStreamLength);
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_returned_range_cache", new object[]
						{
							"ValidateCache()",
							this._Validator.CacheStreamOffset,
							this._Validator.CacheStreamLength
						}));
					}
				}
				this._ResponseStream = stream;
				this._ResponseStreamLength = this._Validator.CacheStreamLength;
				goto IL_35A;
				IL_29E:
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[]
				{
					"ValidateCache"
				}));
				goto IL_35A;
				IL_2CB:
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"ValidateCache",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"ValidateCache()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_35A:;
			}
			catch (Exception ex)
			{
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = ex;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (this._ResponseStream == null && this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
				{
					this._Validator.CacheStream.Close();
					this._Validator.CacheStream = Stream.Null;
				}
			}
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000B7D54 File Offset: 0x000B6D54
		private void CheckRetrieveOnResponse(Stream responseStream)
		{
			bool flag = true;
			try
			{
				CacheValidationStatus cacheValidationStatus = this._ProtocolStatus = this.ValidateResponse();
				switch (cacheValidationStatus)
				{
				case CacheValidationStatus.DoNotUseCache:
					goto IL_107;
				case CacheValidationStatus.Fail:
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[]
					{
						"ValidateResponse"
					}));
					goto IL_107;
				case CacheValidationStatus.DoNotTakeFromCache:
				case CacheValidationStatus.RetryResponseFromCache:
					break;
				case CacheValidationStatus.RetryResponseFromServer:
					flag = false;
					goto IL_107;
				default:
					if (cacheValidationStatus == CacheValidationStatus.Continue)
					{
						flag = false;
						goto IL_107;
					}
					break;
				}
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"ValidateResponse",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"ValidateResponse()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_107:;
			}
			catch (Exception ex)
			{
				flag = true;
				this._ProtocolException = ex;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (flag && this._ResponseStream != null)
				{
					this._ResponseStream.Close();
					this._ResponseStream = null;
					this._Validator.CacheStream = Stream.Null;
				}
			}
			if (this._ProtocolStatus != CacheValidationStatus.Continue)
			{
				return;
			}
			try
			{
				switch (this._ProtocolStatus = this.RevalidateCache())
				{
				case CacheValidationStatus.DoNotUseCache:
				case CacheValidationStatus.DoNotTakeFromCache:
				case CacheValidationStatus.RemoveFromCache:
					flag = true;
					goto IL_4FF;
				case CacheValidationStatus.Fail:
					flag = true;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[]
					{
						"RevalidateCache"
					}));
					goto IL_4FF;
				case CacheValidationStatus.ReturnCachedResponse:
					if (this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
					{
						Stream stream = this._Validator.CacheStream;
						if (this._Validator.CacheStreamOffset != 0L || this._Validator.CacheStreamLength != this._Validator.CacheEntry.StreamSize)
						{
							stream = new RangeStream(stream, this._Validator.CacheStreamOffset, this._Validator.CacheStreamLength);
							if (Logging.On)
							{
								Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_returned_range_cache", new object[]
								{
									"RevalidateCache()",
									this._Validator.CacheStreamOffset,
									this._Validator.CacheStreamLength
								}));
							}
						}
						this._ResponseStream = stream;
						this._ResponseStreamLength = this._Validator.CacheStreamLength;
						goto IL_4FF;
					}
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_no_stream", new object[]
					{
						this._Validator.CacheKey
					}));
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_null_cached_stream", new object[]
						{
							"RevalidateCache()"
						}));
						goto IL_4FF;
					}
					goto IL_4FF;
				case CacheValidationStatus.CombineCachedAndServerResponse:
					if (this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
					{
						Stream stream;
						if (responseStream != null)
						{
							stream = new CombinedReadStream(this._Validator.CacheStream, responseStream);
						}
						else
						{
							stream = this._Validator.CacheStream;
						}
						this._ResponseStream = stream;
						this._ResponseStreamLength = this._Validator.CacheStreamLength;
						goto IL_4FF;
					}
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_no_stream", new object[]
					{
						this._Validator.CacheKey
					}));
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_requested_combined_but_null_cached_stream", new object[]
						{
							"RevalidateCache()"
						}));
						goto IL_4FF;
					}
					goto IL_4FF;
				}
				flag = true;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"RevalidateCache",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"RevalidateCache()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_4FF:;
			}
			catch (Exception ex2)
			{
				flag = true;
				this._ProtocolException = ex2;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex2 is WebException) ? ex2.Message : ex2.ToString()
					}));
				}
			}
			finally
			{
				if (flag && this._ResponseStream != null)
				{
					this._ResponseStream.Close();
					this._ResponseStream = null;
					this._Validator.CacheStream = Stream.Null;
				}
			}
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000B8388 File Offset: 0x000B7388
		private void CheckUpdateOnResponse(Stream responseStream)
		{
			if (this._Validator.CacheEntry == null)
			{
				RequestCacheEntry requestCacheEntry = new RequestCacheEntry();
				requestCacheEntry.IsPrivateEntry = this._RequestCache.IsPrivateCache;
				this._Validator.FetchCacheEntry(requestCacheEntry);
			}
			string cacheKey = this._Validator.CacheKey;
			bool flag = true;
			try
			{
				switch (this._ProtocolStatus = this.UpdateCache())
				{
				case CacheValidationStatus.DoNotUseCache:
				case CacheValidationStatus.DoNotUpdateCache:
					goto IL_327;
				case CacheValidationStatus.Fail:
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[]
					{
						"UpdateCache"
					}));
					goto IL_327;
				case CacheValidationStatus.CacheResponse:
				{
					Stream stream;
					if (this._Validator.StrictCacheErrors)
					{
						stream = this._RequestCache.Store(this._Validator.CacheKey, this._Validator.CacheEntry.StreamSize, this._Validator.CacheEntry.ExpiresUtc, this._Validator.CacheEntry.LastModifiedUtc, this._Validator.CacheEntry.MaxStale, this._Validator.CacheEntry.EntryMetadata, this._Validator.CacheEntry.SystemMetadata);
					}
					else
					{
						this._RequestCache.TryStore(this._Validator.CacheKey, this._Validator.CacheEntry.StreamSize, this._Validator.CacheEntry.ExpiresUtc, this._Validator.CacheEntry.LastModifiedUtc, this._Validator.CacheEntry.MaxStale, this._Validator.CacheEntry.EntryMetadata, this._Validator.CacheEntry.SystemMetadata, out stream);
					}
					if (stream == null)
					{
						this._ProtocolStatus = CacheValidationStatus.DoNotUpdateCache;
						goto IL_327;
					}
					this._ResponseStream = new ForwardingReadStream(responseStream, stream, this._Validator.CacheStreamOffset, this._Validator.StrictCacheErrors);
					this._ProtocolStatus = CacheValidationStatus.UpdateResponseInformation;
					goto IL_327;
				}
				case CacheValidationStatus.UpdateResponseInformation:
					this._ResponseStream = new MetadataUpdateStream(responseStream, this._RequestCache, this._Validator.CacheKey, this._Validator.CacheEntry.ExpiresUtc, this._Validator.CacheEntry.LastModifiedUtc, this._Validator.CacheEntry.LastSynchronizedUtc, this._Validator.CacheEntry.MaxStale, this._Validator.CacheEntry.EntryMetadata, this._Validator.CacheEntry.SystemMetadata, this._Validator.StrictCacheErrors);
					flag = false;
					this._ProtocolStatus = CacheValidationStatus.UpdateResponseInformation;
					goto IL_327;
				case CacheValidationStatus.RemoveFromCache:
					this.EnsureCacheRemoval(cacheKey);
					flag = false;
					goto IL_327;
				}
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"UpdateCache",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"UpdateCache()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_327:;
			}
			finally
			{
				if (flag)
				{
					this._RequestCache.UnlockEntry(this._Validator.CacheStream);
				}
			}
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000B86F4 File Offset: 0x000B76F4
		private CacheValidationStatus ValidateRequest()
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, string.Concat(new object[]
				{
					"Request#",
					this._Validator.Request.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
					", Policy = ",
					this._Validator.Request.CachePolicy.ToString(),
					", Cache Uri = ",
					this._Validator.Uri
				}));
			}
			CacheValidationStatus cacheValidationStatus = this._Validator.ValidateRequest();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, "Selected cache Key = " + this._Validator.CacheKey);
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000B87BC File Offset: 0x000B77BC
		private void ValidateFreshness(RequestCacheEntry fetchEntry)
		{
			this._Validator.FetchCacheEntry(fetchEntry);
			if (this._Validator.CacheStream == null || this._Validator.CacheStream == Stream.Null)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_entry_not_found_freshness_undefined", new object[]
					{
						"ValidateFreshness()"
					}));
				}
				this._Validator.SetFreshnessStatus(CacheFreshnessStatus.Undefined);
				return;
			}
			if (Logging.On && Logging.IsVerbose(Logging.RequestCache))
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping_cache_context"));
				if (fetchEntry == null)
				{
					Logging.PrintInfo(Logging.RequestCache, "<null>");
				}
				else
				{
					string[] array = fetchEntry.ToString(Logging.IsVerbose(Logging.RequestCache)).Split(RequestCache.LineSplits);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].Length != 0)
						{
							Logging.PrintInfo(Logging.RequestCache, array[i]);
						}
					}
				}
			}
			CacheFreshnessStatus cacheFreshnessStatus = this._Validator.ValidateFreshness();
			this._Validator.SetFreshnessStatus(cacheFreshnessStatus);
			this._IsCacheFresh = (cacheFreshnessStatus == CacheFreshnessStatus.Fresh);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"ValidateFreshness()",
					cacheFreshnessStatus.ToString()
				}));
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000B8908 File Offset: 0x000B7908
		private CacheValidationStatus ValidateCache()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.ValidateCache();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"ValidateCache()",
					cacheValidationStatus.ToString()
				}));
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000B8968 File Offset: 0x000B7968
		private CacheValidationStatus RevalidateCache()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.RevalidateCache();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"RevalidateCache()",
					cacheValidationStatus.ToString()
				}));
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000B89C8 File Offset: 0x000B79C8
		private CacheValidationStatus ValidateResponse()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.ValidateResponse();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"ValidateResponse()",
					cacheValidationStatus.ToString()
				}));
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000B8A28 File Offset: 0x000B7A28
		private CacheValidationStatus UpdateCache()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.UpdateCache();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			return cacheValidationStatus;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000B8A50 File Offset: 0x000B7A50
		private void EnsureCacheRemoval(string retrieveKey)
		{
			this._RequestCache.UnlockEntry(this._Validator.CacheStream);
			if (this._Validator.StrictCacheErrors)
			{
				this._RequestCache.Remove(retrieveKey);
			}
			else
			{
				this._RequestCache.TryRemove(retrieveKey);
			}
			if (retrieveKey != this._Validator.CacheKey)
			{
				if (this._Validator.StrictCacheErrors)
				{
					this._RequestCache.Remove(this._Validator.CacheKey);
					return;
				}
				this._RequestCache.TryRemove(this._Validator.CacheKey);
			}
		}

		// Token: 0x0400299D RID: 10653
		private CacheValidationStatus _ProtocolStatus;

		// Token: 0x0400299E RID: 10654
		private Exception _ProtocolException;

		// Token: 0x0400299F RID: 10655
		private Stream _ResponseStream;

		// Token: 0x040029A0 RID: 10656
		private long _ResponseStreamLength;

		// Token: 0x040029A1 RID: 10657
		private RequestCacheValidator _Validator;

		// Token: 0x040029A2 RID: 10658
		private RequestCache _RequestCache;

		// Token: 0x040029A3 RID: 10659
		private bool _IsCacheFresh;

		// Token: 0x040029A4 RID: 10660
		private bool _CanTakeNewRequest;
	}
}
