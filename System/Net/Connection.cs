using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004C8 RID: 1224
	internal class Connection : PooledStream
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000962B5 File Offset: 0x000952B5
		internal override ServicePoint ServicePoint
		{
			get
			{
				return this.ConnectionGroup.ServicePoint;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x000962C2 File Offset: 0x000952C2
		private ConnectionGroup ConnectionGroup
		{
			get
			{
				return this.m_ConnectionGroup;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000962CA File Offset: 0x000952CA
		// (set) Token: 0x060025B9 RID: 9657 RVA: 0x000962D4 File Offset: 0x000952D4
		internal HttpWebRequest LockedRequest
		{
			get
			{
				return this.m_LockedRequest;
			}
			set
			{
				HttpWebRequest lockedRequest = this.m_LockedRequest;
				if (value == lockedRequest)
				{
					if (value != null && value.UnlockConnectionDelegate != this.m_ConnectionUnlock)
					{
						throw new InternalException();
					}
					return;
				}
				else
				{
					object obj = (lockedRequest == null) ? null : lockedRequest.UnlockConnectionDelegate;
					if (obj != null && (value != null || this.m_ConnectionUnlock != obj))
					{
						throw new InternalException();
					}
					if (value == null)
					{
						this.m_LockedRequest = null;
						lockedRequest.UnlockConnectionDelegate = null;
						return;
					}
					UnlockConnectionDelegate unlockConnectionDelegate = value.UnlockConnectionDelegate;
					if (unlockConnectionDelegate != null)
					{
						if (unlockConnectionDelegate == this.m_ConnectionUnlock)
						{
							throw new InternalException();
						}
						unlockConnectionDelegate();
					}
					value.UnlockConnectionDelegate = this.m_ConnectionUnlock;
					this.m_LockedRequest = value;
					if (value.Aborted)
					{
						this.UnlockRequest();
					}
					return;
				}
			}
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x00096378 File Offset: 0x00095378
		private void UnlockRequest()
		{
			this.LockedRequest = null;
			if (this.ConnectionGroup != null)
			{
				this.ConnectionGroup.ConnectionGoneIdle();
			}
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x00096394 File Offset: 0x00095394
		internal Connection(ConnectionGroup connectionGroup) : base(null)
		{
			this.m_MaximumUnauthorizedUploadLength = (long)SettingsSectionInternal.Section.MaximumUnauthorizedUploadLength;
			if (this.m_MaximumUnauthorizedUploadLength > 0L)
			{
				this.m_MaximumUnauthorizedUploadLength *= 1024L;
			}
			this.m_ResponseData = new CoreResponseData();
			this.m_ConnectionGroup = connectionGroup;
			this.m_ReadBuffer = new byte[4096];
			this.m_ReadState = ReadState.Start;
			this.m_WaitList = new ArrayList();
			this.m_WriteList = new ArrayList();
			this.m_AbortDelegate = new HttpAbortDelegate(this.AbortOrDisassociate);
			this.m_ConnectionUnlock = new UnlockConnectionDelegate(this.UnlockRequest);
			this.m_StatusLineValues = new Connection.StatusLineValues();
			this.m_RecycleTimer = this.ConnectionGroup.ServicePoint.ConnectionLeaseTimerQueue.CreateTimer();
			this.ConnectionGroup.Associate(this);
			this.m_ReadDone = true;
			this.m_WriteDone = true;
			this.m_Error = WebExceptionStatus.Success;
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x0009649C File Offset: 0x0009549C
		internal int BusyCount
		{
			get
			{
				return (this.m_ReadDone ? 0 : 1) + 2 * (this.m_WaitList.Count + this.m_WriteList.Count) + this.m_ReservedCount;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x000964CB File Offset: 0x000954CB
		internal int IISVersion
		{
			get
			{
				return this.m_IISVersion;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000964D3 File Offset: 0x000954D3
		internal bool AtLeastOneResponseReceived
		{
			get
			{
				return this.m_AtLeastOneResponseReceived;
			}
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x000964DC File Offset: 0x000954DC
		internal bool SubmitRequest(HttpWebRequest request)
		{
			TriState triState = TriState.Unspecified;
			ConnectionReturnResult responses = null;
			bool flag = false;
			lock (this)
			{
				request.AbortDelegate = this.m_AbortDelegate;
				if (request.Aborted)
				{
					return true;
				}
				if (!base.CanBePooled)
				{
					return false;
				}
				if (this.m_RecycleTimer.Duration != this.ServicePoint.ConnectionLeaseTimerQueue.Duration)
				{
					this.m_RecycleTimer.Cancel();
					this.m_RecycleTimer = this.ServicePoint.ConnectionLeaseTimerQueue.CreateTimer();
				}
				if (this.m_RecycleTimer.HasExpired)
				{
					request.KeepAlive = false;
				}
				if (this.LockedRequest != null && this.LockedRequest != request)
				{
					return false;
				}
				if (this.m_Free && this.m_WriteDone && (this.m_WriteList.Count == 0 || (request.Pipelined && !request.RequireBody && this.m_CanPipeline && this.m_Pipelining && !this.m_IsPipelinePaused)))
				{
					this.m_Free = false;
					triState = this.StartRequest(request);
					if (triState == TriState.Unspecified)
					{
						flag = true;
						this.PrepareCloseConnectionSocket(ref responses);
						base.Close(0);
					}
				}
				else
				{
					this.m_WaitList.Add(request);
					this.CheckNonIdle();
				}
			}
			if (flag)
			{
				ConnectionReturnResult.SetResponses(responses);
				return false;
			}
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, request);
			}
			if (triState != TriState.Unspecified)
			{
				this.CompleteStartRequest(true, request, triState);
			}
			if (!request.Async)
			{
				object obj = request.ConnectionAsyncResult.InternalWaitForCompletion();
				ConnectStream connectStream = obj as ConnectStream;
				Connection.AsyncTriState asyncTriState = null;
				if (connectStream == null)
				{
					asyncTriState = (obj as Connection.AsyncTriState);
				}
				if (triState == TriState.Unspecified && asyncTriState != null)
				{
					this.CompleteStartRequest(true, request, asyncTriState.Value);
				}
				else if (connectStream != null)
				{
					request.SetRequestSubmitDone(connectStream);
				}
			}
			return true;
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000966B4 File Offset: 0x000956B4
		private TriState StartRequest(HttpWebRequest request)
		{
			if (this.m_WriteList.Count == 0)
			{
				if (this.ServicePoint.MaxIdleTime != -1 && this.m_IdleSinceUtc != DateTime.MinValue && this.m_IdleSinceUtc + TimeSpan.FromMilliseconds((double)this.ServicePoint.MaxIdleTime) < DateTime.UtcNow)
				{
					return TriState.Unspecified;
				}
				bool flag = base.PollRead();
				if (flag)
				{
					return TriState.Unspecified;
				}
			}
			TriState result = TriState.False;
			this.m_IdleSinceUtc = DateTime.MinValue;
			if (!this.m_IsPipelinePaused)
			{
				this.m_IsPipelinePaused = (this.m_WriteList.Count >= Connection.s_MaxPipelinedCount);
			}
			this.m_Pipelining = (this.m_CanPipeline && request.Pipelined && !request.RequireBody);
			this.m_KeepAlive &= (request.KeepAlive || request.NtlmKeepAlive);
			this.m_WriteDone = false;
			this.m_WriteList.Add(request);
			this.CheckNonIdle();
			if (base.IsInitalizing)
			{
				result = TriState.True;
			}
			return result;
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x000967BC File Offset: 0x000957BC
		private void CompleteStartRequest(bool onSubmitThread, HttpWebRequest request, TriState needReConnect)
		{
			if (needReConnect == TriState.True)
			{
				try
				{
					if (request.Async)
					{
						this.CompleteStartConnection(true, request);
					}
					else if (onSubmitThread)
					{
						this.CompleteStartConnection(false, request);
					}
				}
				catch (Exception exception)
				{
					if (NclUtilities.IsFatal(exception))
					{
						throw;
					}
				}
				if (!request.Async)
				{
					request.ConnectionAsyncResult.InvokeCallback(new Connection.AsyncTriState(needReConnect));
				}
				return;
			}
			if (request.Async)
			{
				request.OpenWriteSideResponseWindow();
			}
			ConnectStream connectStream = new ConnectStream(this, request);
			if (request.Async || onSubmitThread)
			{
				request.SetRequestSubmitDone(connectStream);
				return;
			}
			request.ConnectionAsyncResult.InvokeCallback(connectStream);
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x00096858 File Offset: 0x00095858
		private HttpWebRequest CheckNextRequest()
		{
			if (this.m_WaitList.Count == 0)
			{
				this.m_Free = this.m_KeepAlive;
				return null;
			}
			if (!base.CanBePooled)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = this.m_WaitList[0] as HttpWebRequest;
			if (this.m_IsPipelinePaused)
			{
				this.m_IsPipelinePaused = (this.m_WriteList.Count > Connection.s_MinPipelinedCount);
			}
			if ((!httpWebRequest.Pipelined || httpWebRequest.RequireBody || !this.m_CanPipeline || !this.m_Pipelining || this.m_IsPipelinePaused) && this.m_WriteList.Count != 0)
			{
				httpWebRequest = null;
			}
			if (httpWebRequest != null)
			{
				this.m_WaitList.RemoveAt(0);
				this.CheckIdle();
			}
			return httpWebRequest;
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x0009690C File Offset: 0x0009590C
		private void CompleteStartConnection(bool async, HttpWebRequest httpWebRequest)
		{
			WebExceptionStatus webExceptionStatus = WebExceptionStatus.ConnectFailure;
			this.m_InnerException = null;
			bool flag = true;
			try
			{
				if (httpWebRequest.Address.Scheme == Uri.UriSchemeHttps && this.ServicePoint.InternalProxyServicePoint)
				{
					if (!this.TunnelThroughProxy(this.ServicePoint.InternalAddress, httpWebRequest, async))
					{
						webExceptionStatus = WebExceptionStatus.ConnectFailure;
						flag = false;
					}
					if (async)
					{
						return;
					}
				}
				else
				{
					TimerThread.Timer requestTimer = httpWebRequest.RequestTimer;
					if (!base.Activate(httpWebRequest, async, (requestTimer != null) ? requestTimer.TimeRemaining : 0, new GeneralAsyncDelegate(this.CompleteConnectionWrapper)))
					{
						return;
					}
				}
			}
			catch (Exception ex)
			{
				if (this.m_InnerException == null)
				{
					this.m_InnerException = ex;
				}
				if (ex is WebException)
				{
					webExceptionStatus = ((WebException)ex).Status;
				}
				flag = false;
			}
			if (flag)
			{
				this.CompleteConnection(async, httpWebRequest);
				return;
			}
			ConnectionReturnResult responses = null;
			this.HandleError(false, false, webExceptionStatus, ref responses);
			ConnectionReturnResult.SetResponses(responses);
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000969EC File Offset: 0x000959EC
		private void CompleteConnectionWrapper(object request, object state)
		{
			Exception ex = state as Exception;
			if (ex != null)
			{
				ConnectionReturnResult responses = null;
				if (this.m_InnerException == null)
				{
					this.m_InnerException = ex;
				}
				this.HandleError(false, false, WebExceptionStatus.ConnectFailure, ref responses);
				ConnectionReturnResult.SetResponses(responses);
			}
			this.CompleteConnection(true, (HttpWebRequest)request);
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00096A34 File Offset: 0x00095A34
		private void CompleteConnection(bool async, HttpWebRequest request)
		{
			WebExceptionStatus webExceptionStatus = WebExceptionStatus.ConnectFailure;
			if (request.Async)
			{
				request.OpenWriteSideResponseWindow();
			}
			try
			{
				try
				{
					if (request.Address.Scheme == Uri.UriSchemeHttps)
					{
						TlsStream networkStream = new TlsStream(request.Address.Host, base.NetworkStream, request.ClientCertificates, this.ServicePoint, request, request.Async ? request.GetConnectingContext().ContextCopy : null);
						base.NetworkStream = networkStream;
					}
				}
				finally
				{
					this.m_ReadState = ReadState.Start;
					this.ClearReaderState();
					request.SetRequestSubmitDone(new ConnectStream(this, request));
					webExceptionStatus = WebExceptionStatus.Success;
				}
			}
			catch (Exception ex)
			{
				if (this.m_InnerException == null)
				{
					this.m_InnerException = ex;
				}
				WebException ex2 = ex as WebException;
				if (ex2 != null)
				{
					webExceptionStatus = ex2.Status;
				}
			}
			if (webExceptionStatus != WebExceptionStatus.Success)
			{
				ConnectionReturnResult responses = null;
				this.HandleError(false, false, webExceptionStatus, ref responses);
				ConnectionReturnResult.SetResponses(responses);
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x00096B24 File Offset: 0x00095B24
		private void InternalWriteStartNextRequest(HttpWebRequest request, ref bool calledCloseConnection, ref TriState startRequestResult, ref HttpWebRequest nextRequest, ref ConnectionReturnResult returnResult)
		{
			lock (this)
			{
				this.m_WriteDone = true;
				if (!this.m_KeepAlive || this.m_Error != WebExceptionStatus.Success || !base.CanBePooled)
				{
					if (this.m_ReadDone)
					{
						if (this.m_Error == WebExceptionStatus.Success)
						{
							this.m_Error = WebExceptionStatus.KeepAliveFailure;
						}
						this.PrepareCloseConnectionSocket(ref returnResult);
						calledCloseConnection = true;
						this.Close();
					}
					else if (this.m_Error != WebExceptionStatus.Success)
					{
					}
				}
				else
				{
					if (this.m_Pipelining || this.m_ReadDone)
					{
						nextRequest = this.CheckNextRequest();
					}
					if (nextRequest != null)
					{
						startRequestResult = this.StartRequest(nextRequest);
					}
				}
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x00096BD0 File Offset: 0x00095BD0
		internal void WriteStartNextRequest(HttpWebRequest request, ref ConnectionReturnResult returnResult)
		{
			TriState triState = TriState.Unspecified;
			HttpWebRequest request2 = null;
			bool flag = false;
			this.InternalWriteStartNextRequest(request, ref flag, ref triState, ref request2, ref returnResult);
			if (!flag && triState != TriState.Unspecified)
			{
				this.CompleteStartRequest(false, request2, triState);
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x00096C04 File Offset: 0x00095C04
		internal void ReadStartNextRequest(WebRequest currentRequest, ref ConnectionReturnResult returnResult)
		{
			HttpWebRequest httpWebRequest = null;
			TriState triState = TriState.Unspecified;
			bool flag = false;
			bool flag2 = false;
			Interlocked.Decrement(ref this.m_ReservedCount);
			try
			{
				lock (this)
				{
					if (this.m_WriteList.Count > 0 && currentRequest == this.m_WriteList[0])
					{
						this.m_ReadState = ReadState.Start;
						this.m_WriteList.RemoveAt(0);
						this.m_ResponseData.m_ConnectStream = null;
					}
					else
					{
						flag2 = true;
					}
					if (!flag2)
					{
						if (this.m_ReadDone)
						{
							throw new InternalException();
						}
						if (!this.m_KeepAlive || this.m_Error != WebExceptionStatus.Success || !base.CanBePooled)
						{
							this.m_ReadDone = true;
							if (this.m_WriteDone)
							{
								if (this.m_Error == WebExceptionStatus.Success)
								{
									this.m_Error = WebExceptionStatus.KeepAliveFailure;
								}
								this.PrepareCloseConnectionSocket(ref returnResult);
								flag = true;
								this.Close();
							}
						}
						else
						{
							this.m_AtLeastOneResponseReceived = true;
							if (this.m_WriteList.Count != 0)
							{
								httpWebRequest = (this.m_WriteList[0] as HttpWebRequest);
								if (!httpWebRequest.HeadersCompleted)
								{
									httpWebRequest = null;
									this.m_ReadDone = true;
								}
							}
							else
							{
								this.m_ReadDone = true;
								if (this.m_WriteDone)
								{
									httpWebRequest = this.CheckNextRequest();
									if (httpWebRequest != null)
									{
										if (httpWebRequest.HeadersCompleted)
										{
											throw new InternalException();
										}
										triState = this.StartRequest(httpWebRequest);
									}
									else
									{
										this.m_Free = true;
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				this.CheckIdle();
				if (returnResult != null)
				{
					ConnectionReturnResult.SetResponses(returnResult);
				}
			}
			if (!flag2 && !flag)
			{
				if (triState != TriState.Unspecified)
				{
					this.CompleteStartRequest(false, httpWebRequest, triState);
					return;
				}
				if (httpWebRequest != null)
				{
					if (!httpWebRequest.Async)
					{
						httpWebRequest.ConnectionReaderAsyncResult.InvokeCallback();
						return;
					}
					if (this.m_BytesScanned < this.m_BytesRead)
					{
						this.ReadComplete(0, WebExceptionStatus.Success);
						return;
					}
					if (Thread.CurrentThread.IsThreadPoolThread)
					{
						this.PostReceive();
						return;
					}
					ThreadPool.UnsafeQueueUserWorkItem(Connection.m_PostReceiveDelegate, this);
				}
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00096DF4 File Offset: 0x00095DF4
		internal void MarkAsReserved()
		{
			Interlocked.Increment(ref this.m_ReservedCount);
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x00096E04 File Offset: 0x00095E04
		internal void CheckStartReceive(HttpWebRequest request)
		{
			lock (this)
			{
				request.HeadersCompleted = true;
				if (this.m_WriteList.Count == 0)
				{
					return;
				}
				if (!this.m_ReadDone || this.m_WriteList[0] != request)
				{
					return;
				}
				this.m_ReadDone = false;
				this.m_CurrentRequest = (HttpWebRequest)this.m_WriteList[0];
			}
			if (!request.Async)
			{
				request.ConnectionReaderAsyncResult.InvokeCallback();
				return;
			}
			if (this.m_BytesScanned < this.m_BytesRead)
			{
				this.ReadComplete(0, WebExceptionStatus.Success);
				return;
			}
			if (Thread.CurrentThread.IsThreadPoolThread)
			{
				this.PostReceive();
				return;
			}
			ThreadPool.UnsafeQueueUserWorkItem(Connection.m_PostReceiveDelegate, this);
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00096ED0 File Offset: 0x00095ED0
		private void InitializeParseStatusLine()
		{
			this.m_StatusState = 0;
			this.m_StatusLineValues.MajorVersion = 0;
			this.m_StatusLineValues.MinorVersion = 0;
			this.m_StatusLineValues.StatusCode = 0;
			this.m_StatusLineValues.StatusDescription = null;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x00096F0C File Offset: 0x00095F0C
		private DataParseStatus ParseStatusLine(byte[] statusLine, int statusLineLength, ref int bytesParsed, ref int[] statusLineInts, ref string statusDescription, ref int statusState, ref WebParseError parseError)
		{
			DataParseStatus dataParseStatus = DataParseStatus.Done;
			int num = -1;
			int num2 = 0;
			while (bytesParsed < statusLineLength && statusLine[bytesParsed] != 13 && statusLine[bytesParsed] != 10)
			{
				switch (statusState)
				{
				case 0:
					if (statusLine[bytesParsed] == 47)
					{
						statusState++;
					}
					else if (statusLine[bytesParsed] == 32)
					{
						statusState = 3;
					}
					break;
				case 1:
					if (statusLine[bytesParsed] != 46)
					{
						goto IL_69;
					}
					statusState++;
					break;
				case 2:
					goto IL_69;
				case 3:
					goto IL_7A;
				case 4:
					if (statusLine[bytesParsed] != 32)
					{
						num2 = bytesParsed;
						if (num == -1)
						{
							num = bytesParsed;
						}
					}
					break;
				}
				IL_DA:
				bytesParsed++;
				if (this.m_MaximumResponseHeadersLength >= 0 && ++this.m_TotalResponseHeadersLength >= this.m_MaximumResponseHeadersLength)
				{
					dataParseStatus = DataParseStatus.DataTooBig;
					IL_1CA:
					if (dataParseStatus == DataParseStatus.Done && statusState != 4 && (statusState != 3 || statusLineInts[3] <= 0))
					{
						dataParseStatus = DataParseStatus.Invalid;
					}
					if (dataParseStatus == DataParseStatus.Invalid)
					{
						parseError.Section = WebParseErrorSection.ResponseStatusLine;
						parseError.Code = WebParseErrorCode.Generic;
					}
					return dataParseStatus;
				}
				continue;
				IL_69:
				if (statusLine[bytesParsed] == 32)
				{
					statusState++;
					goto IL_DA;
				}
				IL_7A:
				if (char.IsDigit((char)statusLine[bytesParsed]))
				{
					int num3 = (int)(statusLine[bytesParsed] - 48);
					statusLineInts[statusState] = statusLineInts[statusState] * 10 + num3;
					goto IL_DA;
				}
				if (statusLineInts[3] > 0)
				{
					statusState++;
					goto IL_DA;
				}
				if (!char.IsWhiteSpace((char)statusLine[bytesParsed]))
				{
					statusLineInts[statusState] = -1;
					goto IL_DA;
				}
				goto IL_DA;
			}
			if (num != -1)
			{
				statusDescription += WebHeaderCollection.HeaderEncoding.GetString(statusLine, num, num2 - num + 1);
			}
			if (bytesParsed == statusLineLength)
			{
				return DataParseStatus.NeedMoreData;
			}
			while (bytesParsed < statusLineLength && (statusLine[bytesParsed] == 13 || statusLine[bytesParsed] == 32))
			{
				bytesParsed++;
				if (this.m_MaximumResponseHeadersLength >= 0 && ++this.m_TotalResponseHeadersLength >= this.m_MaximumResponseHeadersLength)
				{
					dataParseStatus = DataParseStatus.DataTooBig;
					goto IL_1CA;
				}
			}
			if (bytesParsed == statusLineLength)
			{
				dataParseStatus = DataParseStatus.NeedMoreData;
				goto IL_1CA;
			}
			if (statusLine[bytesParsed] != 10)
			{
				goto IL_1CA;
			}
			bytesParsed++;
			if (this.m_MaximumResponseHeadersLength >= 0 && ++this.m_TotalResponseHeadersLength >= this.m_MaximumResponseHeadersLength)
			{
				dataParseStatus = DataParseStatus.DataTooBig;
				goto IL_1CA;
			}
			dataParseStatus = DataParseStatus.Done;
			goto IL_1CA;
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00097114 File Offset: 0x00096114
		private unsafe static DataParseStatus ParseStatusLineStrict(byte[] statusLine, int statusLineLength, ref int bytesParsed, ref int statusState, Connection.StatusLineValues statusLineValues, int maximumHeaderLength, ref int totalBytesParsed, ref WebParseError parseError)
		{
			int num = bytesParsed;
			DataParseStatus dataParseStatus = DataParseStatus.DataTooBig;
			int num2 = (maximumHeaderLength <= 0) ? int.MaxValue : (maximumHeaderLength - totalBytesParsed + bytesParsed);
			if (statusLineLength < num2)
			{
				dataParseStatus = DataParseStatus.NeedMoreData;
				num2 = statusLineLength;
			}
			if (bytesParsed < num2)
			{
				try
				{
					fixed (byte* ptr = statusLine)
					{
						switch (statusState)
						{
						case 0:
							while (totalBytesParsed - num + bytesParsed < "HTTP/".Length)
							{
								if ((byte)"HTTP/"[totalBytesParsed - num + bytesParsed] != ptr[bytesParsed])
								{
									dataParseStatus = DataParseStatus.Invalid;
									goto IL_43F;
								}
								if (++bytesParsed == num2)
								{
									goto IL_43F;
								}
							}
							if (ptr[bytesParsed] == 46)
							{
								dataParseStatus = DataParseStatus.Invalid;
								goto IL_43F;
							}
							statusState = 1;
							break;
						case 1:
							break;
						case 2:
							goto IL_190;
						case 3:
							goto IL_1FB;
						case 4:
							goto IL_29B;
						case 5:
							goto IL_423;
						default:
							goto IL_439;
						}
						while (ptr[bytesParsed] != 46)
						{
							if (ptr[bytesParsed] < 48 || ptr[bytesParsed] > 57)
							{
								dataParseStatus = DataParseStatus.Invalid;
								goto IL_43F;
							}
							statusLineValues.MajorVersion = statusLineValues.MajorVersion * 10 + (int)ptr[bytesParsed] - 48;
							if (++bytesParsed == num2)
							{
								goto IL_43F;
							}
						}
						if (bytesParsed + 1 == num2)
						{
							goto IL_43F;
						}
						bytesParsed++;
						if (ptr[bytesParsed] == 32)
						{
							dataParseStatus = DataParseStatus.Invalid;
							goto IL_43F;
						}
						statusState = 2;
						IL_190:
						while (ptr[bytesParsed] != 32)
						{
							if (ptr[bytesParsed] < 48 || ptr[bytesParsed] > 57)
							{
								dataParseStatus = DataParseStatus.Invalid;
								goto IL_43F;
							}
							statusLineValues.MinorVersion = statusLineValues.MinorVersion * 10 + (int)ptr[bytesParsed] - 48;
							if (++bytesParsed == num2)
							{
								goto IL_43F;
							}
						}
						statusState = 3;
						statusLineValues.StatusCode = 1;
						if (++bytesParsed == num2)
						{
							goto IL_43F;
						}
						IL_1FB:
						while (ptr[bytesParsed] >= 48 && ptr[bytesParsed] <= 57)
						{
							if (statusLineValues.StatusCode >= 1000)
							{
								dataParseStatus = DataParseStatus.Invalid;
								goto IL_43F;
							}
							statusLineValues.StatusCode = statusLineValues.StatusCode * 10 + (int)ptr[bytesParsed] - 48;
							if (++bytesParsed == num2)
							{
								goto IL_43F;
							}
						}
						if (ptr[bytesParsed] != 32 || statusLineValues.StatusCode < 1000)
						{
							if (ptr[bytesParsed] != 13 || statusLineValues.StatusCode < 1000)
							{
								dataParseStatus = DataParseStatus.Invalid;
								goto IL_43F;
							}
							statusLineValues.StatusCode -= 1000;
							statusState = 5;
							if (++bytesParsed == num2)
							{
								goto IL_43F;
							}
							goto IL_423;
						}
						else
						{
							statusLineValues.StatusCode -= 1000;
							statusState = 4;
							if (++bytesParsed == num2)
							{
								goto IL_43F;
							}
						}
						IL_29B:
						if (statusLineValues.StatusDescription == null)
						{
							string[] array = Connection.s_ShortcutStatusDescriptions;
							int i = 0;
							while (i < array.Length)
							{
								string text = array[i];
								if (bytesParsed < num2 - text.Length && ptr[bytesParsed] == (byte)text[0])
								{
									byte* ptr2 = ptr + bytesParsed + 1;
									int num3 = 1;
									while (num3 < text.Length && *(ptr2++) == (byte)text[num3])
									{
										num3++;
									}
									if (num3 == text.Length)
									{
										statusLineValues.StatusDescription = text;
										bytesParsed += text.Length;
										break;
									}
									break;
								}
								else
								{
									i++;
								}
							}
						}
						int num4 = bytesParsed;
						while (ptr[bytesParsed] != 13)
						{
							if (ptr[bytesParsed] < 32 || ptr[bytesParsed] == 127)
							{
								dataParseStatus = DataParseStatus.Invalid;
								goto IL_43F;
							}
							if (++bytesParsed == num2)
							{
								string @string = WebHeaderCollection.HeaderEncoding.GetString(ptr + num4, bytesParsed - num4);
								if (statusLineValues.StatusDescription == null)
								{
									statusLineValues.StatusDescription = @string;
								}
								else
								{
									statusLineValues.StatusDescription += @string;
								}
								goto IL_43F;
							}
						}
						if (bytesParsed > num4)
						{
							string string2 = WebHeaderCollection.HeaderEncoding.GetString(ptr + num4, bytesParsed - num4);
							if (statusLineValues.StatusDescription == null)
							{
								statusLineValues.StatusDescription = string2;
							}
							else
							{
								statusLineValues.StatusDescription += string2;
							}
						}
						else if (statusLineValues.StatusDescription == null)
						{
							statusLineValues.StatusDescription = "";
						}
						statusState = 5;
						if (++bytesParsed == num2)
						{
							goto IL_43F;
						}
						IL_423:
						if (ptr[bytesParsed] != 10)
						{
							dataParseStatus = DataParseStatus.Invalid;
						}
						else
						{
							dataParseStatus = DataParseStatus.Done;
							bytesParsed++;
						}
						IL_439:;
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			IL_43F:
			totalBytesParsed += bytesParsed - num;
			if (dataParseStatus == DataParseStatus.Invalid)
			{
				parseError.Section = WebParseErrorSection.ResponseStatusLine;
				parseError.Code = WebParseErrorCode.Generic;
			}
			return dataParseStatus;
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x0009759C File Offset: 0x0009659C
		private void SetStatusLineParsed()
		{
			this.m_ResponseData.m_StatusCode = (HttpStatusCode)this.m_StatusLineValues.StatusCode;
			this.m_ResponseData.m_StatusDescription = this.m_StatusLineValues.StatusDescription;
			this.m_ResponseData.m_IsVersionHttp11 = (this.m_StatusLineValues.MajorVersion >= 1 && this.m_StatusLineValues.MinorVersion >= 1);
			if (this.ServicePoint.HttpBehaviour == HttpBehaviour.Unknown || (this.ServicePoint.HttpBehaviour == HttpBehaviour.HTTP11 && !this.m_ResponseData.m_IsVersionHttp11))
			{
				this.ServicePoint.HttpBehaviour = (this.m_ResponseData.m_IsVersionHttp11 ? HttpBehaviour.HTTP11 : HttpBehaviour.HTTP10);
			}
			if (ServicePointManager.UseHttpPipeliningAndBufferPooling)
			{
				this.m_CanPipeline = this.ServicePoint.SupportsPipelining;
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00097660 File Offset: 0x00096660
		private long ProcessHeaderData(ref bool fHaveChunked, HttpWebRequest request, out bool dummyResponseStream)
		{
			long num = -1L;
			fHaveChunked = false;
			string text = this.m_ResponseData.m_ResponseHeaders["Transfer-Encoding"];
			if (text != null)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
				fHaveChunked = (text.IndexOf("chunked") != -1);
			}
			if (!fHaveChunked)
			{
				string text2 = this.m_ResponseData.m_ResponseHeaders.ContentLength;
				if (text2 != null)
				{
					int num2 = text2.IndexOf(':');
					if (num2 != -1)
					{
						text2 = text2.Substring(num2 + 1);
					}
					if (!long.TryParse(text2, NumberStyles.None, CultureInfo.InvariantCulture.NumberFormat, out num))
					{
						num = -1L;
						num2 = text2.LastIndexOf(',');
						if (num2 != -1)
						{
							text2 = text2.Substring(num2 + 1);
							if (!long.TryParse(text2, NumberStyles.None, CultureInfo.InvariantCulture.NumberFormat, out num))
							{
								num = -1L;
							}
						}
					}
					if (num < 0L)
					{
						num = -2L;
					}
				}
			}
			dummyResponseStream = (!request.CanGetResponseStream || this.m_ResponseData.m_StatusCode < HttpStatusCode.OK || this.m_ResponseData.m_StatusCode == HttpStatusCode.NoContent || (this.m_ResponseData.m_StatusCode == HttpStatusCode.NotModified && num < 0L));
			if (this.m_KeepAlive)
			{
				bool flag = false;
				if (!dummyResponseStream && num < 0L && !fHaveChunked)
				{
					flag = true;
				}
				else if (this.m_ResponseData.m_StatusCode == HttpStatusCode.Forbidden && base.NetworkStream is TlsStream)
				{
					flag = true;
				}
				else if (this.m_ResponseData.m_StatusCode > (HttpStatusCode)299 && (request.CurrentMethod == KnownHttpVerb.Post || request.CurrentMethod == KnownHttpVerb.Put) && this.m_MaximumUnauthorizedUploadLength >= 0L && request.ContentLength > this.m_MaximumUnauthorizedUploadLength && (request.CurrentAuthenticationState == null || request.CurrentAuthenticationState.Module == null))
				{
					flag = true;
				}
				else
				{
					bool flag2 = false;
					bool flag3 = false;
					string text3 = this.m_ResponseData.m_ResponseHeaders["Connection"];
					if (text3 == null && (this.ServicePoint.InternalProxyServicePoint || request.IsTunnelRequest))
					{
						text3 = this.m_ResponseData.m_ResponseHeaders["Proxy-Connection"];
					}
					if (text3 != null)
					{
						text3 = text3.ToLower(CultureInfo.InvariantCulture);
						if (text3.IndexOf("keep-alive") != -1)
						{
							flag3 = true;
						}
						else if (text3.IndexOf("close") != -1)
						{
							flag2 = true;
						}
					}
					if ((flag2 && this.ServicePoint.HttpBehaviour == HttpBehaviour.HTTP11) || (!flag3 && this.ServicePoint.HttpBehaviour <= HttpBehaviour.HTTP10))
					{
						flag = true;
					}
				}
				if (flag)
				{
					lock (this)
					{
						this.m_KeepAlive = false;
						this.m_Free = false;
					}
				}
			}
			return num;
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060025D0 RID: 9680 RVA: 0x00097910 File Offset: 0x00096910
		internal bool KeepAlive
		{
			get
			{
				return this.m_KeepAlive;
			}
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00097918 File Offset: 0x00096918
		private DataParseStatus ParseStreamData(ref ConnectionReturnResult returnResult)
		{
			if (this.m_CurrentRequest == null)
			{
				this.m_ParseError.Section = WebParseErrorSection.Generic;
				this.m_ParseError.Code = WebParseErrorCode.UnexpectedServerResponse;
				return DataParseStatus.Invalid;
			}
			bool flag = false;
			bool flag2;
			long num = this.ProcessHeaderData(ref flag, this.m_CurrentRequest, out flag2);
			if (num == -2L)
			{
				this.m_ParseError.Section = WebParseErrorSection.ResponseHeader;
				this.m_ParseError.Code = WebParseErrorCode.InvalidContentLength;
				return DataParseStatus.Invalid;
			}
			int num2 = this.m_BytesRead - this.m_BytesScanned;
			if (this.m_ResponseData.m_StatusCode > (HttpStatusCode)299)
			{
				this.m_CurrentRequest.ErrorStatusCodeNotify(this, this.m_KeepAlive, false);
			}
			int num3;
			if (flag2)
			{
				num3 = 0;
				flag = false;
			}
			else if (flag)
			{
				num3 = Connection.FindChunkEntitySize(this.m_ReadBuffer, this.m_BytesScanned, num2);
				if (num3 == 0)
				{
					this.m_ParseError.Section = WebParseErrorSection.ResponseBody;
					this.m_ParseError.Code = WebParseErrorCode.InvalidChunkFormat;
					return DataParseStatus.Invalid;
				}
			}
			else if (num > 2147483647L)
			{
				num3 = -1;
			}
			else
			{
				num3 = (int)num;
			}
			DataParseStatus result;
			if (num3 != -1 && num3 <= num2)
			{
				this.m_ResponseData.m_ConnectStream = new ConnectStream(this, this.m_ReadBuffer, this.m_BytesScanned, num3, flag2 ? 0L : num, flag, this.m_CurrentRequest);
				result = DataParseStatus.ContinueParsing;
				this.m_BytesScanned += num3;
			}
			else
			{
				this.m_ResponseData.m_ConnectStream = new ConnectStream(this, this.m_ReadBuffer, this.m_BytesScanned, num2, flag2 ? 0L : num, flag, this.m_CurrentRequest);
				result = DataParseStatus.Done;
				this.ClearReaderState();
			}
			this.m_ResponseData.m_ContentLength = num;
			ConnectionReturnResult.Add(ref returnResult, this.m_CurrentRequest, this.m_ResponseData.Clone());
			return result;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00097AA5 File Offset: 0x00096AA5
		private void ClearReaderState()
		{
			this.m_BytesRead = 0;
			this.m_BytesScanned = 0;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00097AB8 File Offset: 0x00096AB8
		private DataParseStatus ParseResponseData(ref ConnectionReturnResult returnResult, out bool requestDone, out CoreResponseData continueResponseData)
		{
			DataParseStatus result = DataParseStatus.NeedMoreData;
			requestDone = false;
			continueResponseData = null;
			switch (this.m_ReadState)
			{
			case ReadState.Start:
				break;
			case ReadState.StatusLine:
				goto IL_C7;
			case ReadState.Headers:
				goto IL_26A;
			case ReadState.Data:
				goto IL_51B;
			default:
				goto IL_526;
			}
			IL_2C:
			if (this.m_CurrentRequest == null)
			{
				lock (this)
				{
					if (this.m_WriteList.Count == 0 || (this.m_CurrentRequest = (this.m_WriteList[0] as HttpWebRequest)) == null)
					{
						this.m_ParseError.Section = WebParseErrorSection.Generic;
						this.m_ParseError.Code = WebParseErrorCode.Generic;
						result = DataParseStatus.Invalid;
						goto IL_526;
					}
				}
			}
			this.m_MaximumResponseHeadersLength = this.m_CurrentRequest.MaximumResponseHeadersLength * 1024;
			this.m_ResponseData = new CoreResponseData();
			this.m_ReadState = ReadState.StatusLine;
			this.m_TotalResponseHeadersLength = 0;
			this.InitializeParseStatusLine();
			IL_C7:
			DataParseStatus dataParseStatus;
			if (SettingsSectionInternal.Section.UseUnsafeHeaderParsing)
			{
				int[] array = new int[]
				{
					0,
					this.m_StatusLineValues.MajorVersion,
					this.m_StatusLineValues.MinorVersion,
					this.m_StatusLineValues.StatusCode
				};
				if (this.m_StatusLineValues.StatusDescription == null)
				{
					this.m_StatusLineValues.StatusDescription = "";
				}
				dataParseStatus = this.ParseStatusLine(this.m_ReadBuffer, this.m_BytesRead, ref this.m_BytesScanned, ref array, ref this.m_StatusLineValues.StatusDescription, ref this.m_StatusState, ref this.m_ParseError);
				this.m_StatusLineValues.MajorVersion = array[1];
				this.m_StatusLineValues.MinorVersion = array[2];
				this.m_StatusLineValues.StatusCode = array[3];
			}
			else
			{
				dataParseStatus = Connection.ParseStatusLineStrict(this.m_ReadBuffer, this.m_BytesRead, ref this.m_BytesScanned, ref this.m_StatusState, this.m_StatusLineValues, this.m_MaximumResponseHeadersLength, ref this.m_TotalResponseHeadersLength, ref this.m_ParseError);
			}
			if (dataParseStatus == DataParseStatus.Done)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_received_status_line", new object[]
					{
						this.m_StatusLineValues.MajorVersion + "." + this.m_StatusLineValues.MinorVersion,
						this.m_StatusLineValues.StatusCode,
						this.m_StatusLineValues.StatusDescription
					}));
				}
				this.SetStatusLineParsed();
				this.m_ReadState = ReadState.Headers;
				this.m_ResponseData.m_ResponseHeaders = new WebHeaderCollection(WebHeaderCollectionType.HttpWebResponse);
			}
			else
			{
				if (dataParseStatus != DataParseStatus.NeedMoreData)
				{
					result = dataParseStatus;
					goto IL_526;
				}
				goto IL_526;
			}
			IL_26A:
			if (this.m_BytesScanned >= this.m_BytesRead)
			{
				goto IL_526;
			}
			if (SettingsSectionInternal.Section.UseUnsafeHeaderParsing)
			{
				dataParseStatus = this.m_ResponseData.m_ResponseHeaders.ParseHeaders(this.m_ReadBuffer, this.m_BytesRead, ref this.m_BytesScanned, ref this.m_TotalResponseHeadersLength, this.m_MaximumResponseHeadersLength, ref this.m_ParseError);
			}
			else
			{
				dataParseStatus = this.m_ResponseData.m_ResponseHeaders.ParseHeadersStrict(this.m_ReadBuffer, this.m_BytesRead, ref this.m_BytesScanned, ref this.m_TotalResponseHeadersLength, this.m_MaximumResponseHeadersLength, ref this.m_ParseError);
			}
			if (dataParseStatus == DataParseStatus.Invalid || dataParseStatus == DataParseStatus.DataTooBig)
			{
				result = dataParseStatus;
				goto IL_526;
			}
			if (dataParseStatus != DataParseStatus.Done)
			{
				goto IL_526;
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_received_headers", new object[]
				{
					this.m_ResponseData.m_ResponseHeaders.ToString(true)
				}));
			}
			if (this.m_IISVersion == -1)
			{
				string server = this.m_ResponseData.m_ResponseHeaders.Server;
				if (server != null && server.ToLower(CultureInfo.InvariantCulture).Contains("microsoft-iis"))
				{
					int num = server.IndexOf("/");
					if (num++ > 0 && num < server.Length)
					{
						this.m_IISVersion = (int)(server[num++] - '0');
						while (num < server.Length && char.IsDigit(server[num]))
						{
							this.m_IISVersion = this.m_IISVersion * 10 + (int)server[num++] - 48;
						}
					}
				}
				if (this.m_IISVersion == -1 && this.m_ResponseData.m_StatusCode != HttpStatusCode.Continue)
				{
					this.m_IISVersion = 0;
				}
			}
			bool flag = ServicePointManager.UseStrictRfcInterimResponseHandling && this.m_ResponseData.m_StatusCode > HttpStatusCode.SwitchingProtocols && this.m_ResponseData.m_StatusCode < HttpStatusCode.OK;
			if (this.m_ResponseData.m_StatusCode == HttpStatusCode.Continue || this.m_ResponseData.m_StatusCode == HttpStatusCode.BadRequest || flag)
			{
				if (this.m_ResponseData.m_StatusCode == HttpStatusCode.BadRequest)
				{
					if (this.ServicePoint.HttpBehaviour == HttpBehaviour.HTTP11 && this.m_CurrentRequest.HttpWriteMode == HttpWriteMode.Chunked && this.m_ResponseData.m_ResponseHeaders.Via != null && string.Compare(this.m_ResponseData.m_StatusDescription, "Bad Request ( The HTTP request includes a non-supported header. Contact the Server administrator.  )", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.ServicePoint.HttpBehaviour = HttpBehaviour.HTTP11PartiallyCompliant;
					}
				}
				else
				{
					if (this.m_ResponseData.m_StatusCode == HttpStatusCode.Continue)
					{
						this.m_CurrentRequest.Saw100Continue = true;
						if (!this.ServicePoint.Understands100Continue)
						{
							this.ServicePoint.Understands100Continue = true;
						}
						continueResponseData = this.m_ResponseData;
						goto IL_2C;
					}
					goto IL_2C;
				}
			}
			this.m_ReadState = ReadState.Data;
			IL_51B:
			requestDone = true;
			result = this.ParseStreamData(ref returnResult);
			IL_526:
			if (this.m_BytesScanned == this.m_BytesRead)
			{
				this.ClearReaderState();
			}
			return result;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00098010 File Offset: 0x00097010
		internal void CloseOnIdle()
		{
			lock (this)
			{
				this.m_KeepAlive = false;
				this.m_RemovedFromConnectionList = true;
				if (!this.m_Idle)
				{
					this.CheckIdle();
				}
				if (this.m_Idle)
				{
					this.AbortSocket(false);
					GC.SuppressFinalize(this);
				}
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00098070 File Offset: 0x00097070
		internal bool AbortOrDisassociate(HttpWebRequest request, WebException webException)
		{
			ConnectionReturnResult responses = null;
			lock (this)
			{
				int num = this.m_WriteList.IndexOf(request);
				if (num == -1)
				{
					num = this.m_WaitList.IndexOf(request);
					if (num != -1)
					{
						this.m_WaitList.RemoveAt(num);
					}
					return true;
				}
				if (num != 0)
				{
					this.m_WriteList.RemoveAt(num);
					this.m_KeepAlive = false;
					return true;
				}
				this.m_KeepAlive = false;
				if (webException != null && this.m_InnerException == null)
				{
					this.m_InnerException = webException;
					this.m_Error = webException.Status;
				}
				else
				{
					this.m_Error = WebExceptionStatus.RequestCanceled;
				}
				this.PrepareCloseConnectionSocket(ref responses);
				base.Close(0);
			}
			ConnectionReturnResult.SetResponses(responses);
			return false;
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00098134 File Offset: 0x00097134
		internal void AbortSocket(bool isAbortState)
		{
			if (isAbortState)
			{
				this.UnlockRequest();
				this.CheckIdle();
			}
			else
			{
				this.m_Error = WebExceptionStatus.KeepAliveFailure;
			}
			lock (this)
			{
				base.Close(0);
			}
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00098184 File Offset: 0x00097184
		private void PrepareCloseConnectionSocket(ref ConnectionReturnResult returnResult)
		{
			this.m_IdleSinceUtc = DateTime.MinValue;
			base.CanBePooled = false;
			if (this.m_WriteList.Count != 0 || this.m_WaitList.Count != 0)
			{
				HttpWebRequest lockedRequest = this.LockedRequest;
				if (lockedRequest != null)
				{
					bool flag = false;
					foreach (object obj in this.m_WriteList)
					{
						HttpWebRequest httpWebRequest = (HttpWebRequest)obj;
						if (httpWebRequest == lockedRequest)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						foreach (object obj2 in this.m_WaitList)
						{
							HttpWebRequest httpWebRequest2 = (HttpWebRequest)obj2;
							if (httpWebRequest2 == lockedRequest)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						this.UnlockRequest();
					}
				}
				if (this.m_WaitList.Count != 0)
				{
					HttpWebRequest[] array = new HttpWebRequest[this.m_WaitList.Count];
					this.m_WaitList.CopyTo(array, 0);
					ConnectionReturnResult.AddExceptionRange(ref returnResult, array, ExceptionHelper.IsolatedException);
				}
				if (this.m_WriteList.Count != 0)
				{
					Exception ex = this.m_InnerException;
					if (!(ex is WebException) && !(ex is SecurityException))
					{
						if (this.m_Error == WebExceptionStatus.ServerProtocolViolation)
						{
							string text = NetRes.GetWebStatusString(this.m_Error);
							string text2 = "";
							if (this.m_ParseError.Section != WebParseErrorSection.Generic)
							{
								text2 = text2 + " Section=" + this.m_ParseError.Section.ToString();
							}
							if (this.m_ParseError.Code != WebParseErrorCode.Generic)
							{
								text2 = text2 + " Detail=" + SR.GetString("net_WebResponseParseError_" + this.m_ParseError.Code.ToString());
							}
							if (text2.Length != 0)
							{
								text = text + "." + text2;
							}
							ex = new WebException(text, ex, this.m_Error, null, WebExceptionInternalStatus.RequestFatal);
						}
						else if (this.m_Error == WebExceptionStatus.SecureChannelFailure)
						{
							ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.SecureChannelFailure), WebExceptionStatus.SecureChannelFailure);
						}
						else if (this.m_Error == WebExceptionStatus.Timeout)
						{
							ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
						}
						else if (this.m_Error == WebExceptionStatus.RequestCanceled)
						{
							ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled, WebExceptionInternalStatus.RequestFatal, ex);
						}
						else if (this.m_Error == WebExceptionStatus.MessageLengthLimitExceeded || this.m_Error == WebExceptionStatus.TrustFailure)
						{
							ex = new WebException(NetRes.GetWebStatusString("net_connclosed", this.m_Error), this.m_Error, WebExceptionInternalStatus.RequestFatal, ex);
						}
						else
						{
							if (this.m_Error == WebExceptionStatus.Success)
							{
								throw new InternalException();
							}
							bool flag2 = false;
							bool flag3 = false;
							if (this.m_WriteList.Count != 1)
							{
								flag2 = true;
							}
							else if (this.m_Error == WebExceptionStatus.KeepAliveFailure)
							{
								HttpWebRequest httpWebRequest3 = (HttpWebRequest)this.m_WriteList[0];
								if (!httpWebRequest3.BodyStarted)
								{
									flag3 = true;
								}
							}
							else
							{
								flag2 = (!this.AtLeastOneResponseReceived && !((HttpWebRequest)this.m_WriteList[0]).BodyStarted);
							}
							ex = new WebException(NetRes.GetWebStatusString("net_connclosed", this.m_Error), this.m_Error, flag3 ? WebExceptionInternalStatus.Isolated : (flag2 ? WebExceptionInternalStatus.Recoverable : WebExceptionInternalStatus.RequestFatal), ex);
						}
					}
					WebException exception = new WebException(NetRes.GetWebStatusString("net_connclosed", WebExceptionStatus.PipelineFailure), WebExceptionStatus.PipelineFailure, WebExceptionInternalStatus.Recoverable, ex);
					HttpWebRequest[] array = new HttpWebRequest[this.m_WriteList.Count];
					this.m_WriteList.CopyTo(array, 0);
					ConnectionReturnResult.AddExceptionRange(ref returnResult, array, exception, ex);
				}
				this.m_WriteList.Clear();
				this.m_WaitList.Clear();
			}
			this.CheckIdle();
			if (this.m_Idle)
			{
				GC.SuppressFinalize(this);
			}
			if (!this.m_RemovedFromConnectionList && this.ConnectionGroup != null)
			{
				this.m_RemovedFromConnectionList = true;
				this.ConnectionGroup.Disassociate(this);
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00098588 File Offset: 0x00097588
		internal void HandleConnectStreamException(bool writeDone, bool readDone, WebExceptionStatus webExceptionStatus, ref ConnectionReturnResult returnResult, Exception e)
		{
			if (this.m_InnerException == null)
			{
				this.m_InnerException = e;
				if (!(e is WebException) && base.NetworkStream is TlsStream)
				{
					webExceptionStatus = ((TlsStream)base.NetworkStream).ExceptionStatus;
				}
				else if (e is ObjectDisposedException)
				{
					webExceptionStatus = WebExceptionStatus.RequestCanceled;
				}
			}
			this.HandleError(writeDone, readDone, webExceptionStatus, ref returnResult);
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x000985E6 File Offset: 0x000975E6
		private void HandleErrorWithReadDone(WebExceptionStatus webExceptionStatus, ref ConnectionReturnResult returnResult)
		{
			this.HandleError(false, true, webExceptionStatus, ref returnResult);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000985F4 File Offset: 0x000975F4
		private void HandleError(bool writeDone, bool readDone, WebExceptionStatus webExceptionStatus, ref ConnectionReturnResult returnResult)
		{
			lock (this)
			{
				if (writeDone)
				{
					this.m_WriteDone = true;
				}
				if (readDone)
				{
					this.m_ReadDone = true;
				}
				if (webExceptionStatus == WebExceptionStatus.Success)
				{
					throw new InternalException();
				}
				this.m_Error = webExceptionStatus;
				this.PrepareCloseConnectionSocket(ref returnResult);
				base.Close(0);
			}
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00098658 File Offset: 0x00097658
		private static void ReadCallbackWrapper(IAsyncResult asyncResult)
		{
			if (asyncResult.CompletedSynchronously)
			{
				return;
			}
			((Connection)asyncResult.AsyncState).ReadCallback(asyncResult);
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00098674 File Offset: 0x00097674
		private void ReadCallback(IAsyncResult asyncResult)
		{
			int num = -1;
			WebExceptionStatus errorStatus = WebExceptionStatus.ReceiveFailure;
			try
			{
				num = this.EndRead(asyncResult);
				if (num == 0)
				{
					num = -1;
				}
				errorStatus = WebExceptionStatus.Success;
			}
			catch (Exception ex)
			{
				HttpWebRequest currentRequest = this.m_CurrentRequest;
				if (currentRequest != null)
				{
					currentRequest.ErrorStatusCodeNotify(this, false, true);
				}
				if (this.m_InnerException == null)
				{
					this.m_InnerException = ex;
				}
				if (ex.GetType() == typeof(ObjectDisposedException))
				{
					errorStatus = WebExceptionStatus.RequestCanceled;
				}
				if (base.NetworkStream is TlsStream)
				{
					errorStatus = ((TlsStream)base.NetworkStream).ExceptionStatus;
				}
				else
				{
					errorStatus = WebExceptionStatus.ReceiveFailure;
				}
			}
			this.ReadComplete(num, errorStatus);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x0009870C File Offset: 0x0009770C
		internal void PollAndRead(HttpWebRequest request, bool userRetrievedStream)
		{
			request.SawInitialResponse = false;
			if (request.ConnectionReaderAsyncResult.InternalPeekCompleted && request.ConnectionReaderAsyncResult.Result == null && base.CanBePooled)
			{
				this.SyncRead(request, userRetrievedStream, true);
			}
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00098740 File Offset: 0x00097740
		internal void SyncRead(HttpWebRequest request, bool userRetrievedStream, bool probeRead)
		{
			if (Connection.t_SyncReadNesting > 0)
			{
				return;
			}
			bool flag = !probeRead;
			try
			{
				Connection.t_SyncReadNesting++;
				int num = probeRead ? request.RequestContinueCount : 0;
				int num2 = -1;
				WebExceptionStatus errorStatus = WebExceptionStatus.ReceiveFailure;
				if (this.m_BytesScanned < this.m_BytesRead)
				{
					flag = true;
					num2 = 0;
					errorStatus = WebExceptionStatus.Success;
				}
				bool flag2;
				do
				{
					flag2 = true;
					try
					{
						if (num2 != 0)
						{
							errorStatus = WebExceptionStatus.ReceiveFailure;
							if (!flag)
							{
								flag = base.Poll(350000, SelectMode.SelectRead);
							}
							if (flag)
							{
								this.ReadTimeout = request.Timeout;
								num2 = this.Read(this.m_ReadBuffer, this.m_BytesRead, this.m_ReadBuffer.Length - this.m_BytesRead);
								errorStatus = WebExceptionStatus.Success;
								if (num2 == 0)
								{
									num2 = -1;
								}
							}
						}
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						if (this.m_InnerException == null)
						{
							this.m_InnerException = ex;
						}
						if (ex.GetType() == typeof(ObjectDisposedException))
						{
							errorStatus = WebExceptionStatus.RequestCanceled;
						}
						else if (base.NetworkStream is TlsStream)
						{
							errorStatus = ((TlsStream)base.NetworkStream).ExceptionStatus;
						}
						else
						{
							SocketException ex2 = ex.InnerException as SocketException;
							if (ex2 != null)
							{
								if (ex2.ErrorCode == 10060)
								{
									errorStatus = WebExceptionStatus.Timeout;
								}
								else
								{
									errorStatus = WebExceptionStatus.ReceiveFailure;
								}
							}
						}
					}
					if (flag)
					{
						flag2 = this.ReadComplete(num2, errorStatus);
					}
					num2 = -1;
				}
				while (!flag2 && (userRetrievedStream || num == request.RequestContinueCount));
			}
			finally
			{
				Connection.t_SyncReadNesting--;
			}
			if (probeRead)
			{
				if (flag)
				{
					if (!request.Saw100Continue && !userRetrievedStream)
					{
						request.SawInitialResponse = true;
						return;
					}
				}
				else
				{
					request.SetRequestContinue();
				}
			}
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x000988F0 File Offset: 0x000978F0
		private bool ReadComplete(int bytesRead, WebExceptionStatus errorStatus)
		{
			bool result = true;
			CoreResponseData coreResponseData = null;
			ConnectionReturnResult connectionReturnResult = null;
			HttpWebRequest httpWebRequest = null;
			try
			{
				if (bytesRead < 0)
				{
					if (this.m_ReadState == ReadState.Start && this.m_AtLeastOneResponseReceived)
					{
						if (errorStatus == WebExceptionStatus.Success || errorStatus == WebExceptionStatus.ReceiveFailure)
						{
							errorStatus = WebExceptionStatus.KeepAliveFailure;
						}
					}
					else if (errorStatus == WebExceptionStatus.Success)
					{
						errorStatus = WebExceptionStatus.ConnectionClosed;
					}
					HttpWebRequest currentRequest = this.m_CurrentRequest;
					if (currentRequest != null)
					{
						currentRequest.ErrorStatusCodeNotify(this, false, true);
					}
					this.HandleErrorWithReadDone(errorStatus, ref connectionReturnResult);
				}
				else
				{
					bytesRead += this.m_BytesRead;
					if (bytesRead > this.m_ReadBuffer.Length)
					{
						throw new InternalException();
					}
					this.m_BytesRead = bytesRead;
					DataParseStatus dataParseStatus = this.ParseResponseData(ref connectionReturnResult, out result, out coreResponseData);
					httpWebRequest = this.m_CurrentRequest;
					if (dataParseStatus != DataParseStatus.NeedMoreData)
					{
						this.m_CurrentRequest = null;
					}
					if (dataParseStatus == DataParseStatus.Invalid || dataParseStatus == DataParseStatus.DataTooBig)
					{
						if (httpWebRequest != null)
						{
							httpWebRequest.ErrorStatusCodeNotify(this, false, false);
						}
						if (dataParseStatus == DataParseStatus.Invalid)
						{
							this.HandleErrorWithReadDone(WebExceptionStatus.ServerProtocolViolation, ref connectionReturnResult);
						}
						else
						{
							this.HandleErrorWithReadDone(WebExceptionStatus.MessageLengthLimitExceeded, ref connectionReturnResult);
						}
					}
					else if (dataParseStatus != DataParseStatus.Done)
					{
						if (dataParseStatus == DataParseStatus.NeedMoreData)
						{
							int num = this.m_BytesRead - this.m_BytesScanned;
							if (num != 0)
							{
								if (this.m_BytesScanned == 0 && this.m_BytesRead == this.m_ReadBuffer.Length)
								{
									byte[] array = new byte[this.m_ReadBuffer.Length * 2];
									Buffer.BlockCopy(this.m_ReadBuffer, 0, array, 0, this.m_BytesRead);
									this.m_ReadBuffer = array;
								}
								else
								{
									Buffer.BlockCopy(this.m_ReadBuffer, this.m_BytesScanned, this.m_ReadBuffer, 0, num);
								}
							}
							this.m_BytesRead = num;
							this.m_BytesScanned = 0;
							if (httpWebRequest != null && httpWebRequest.Async)
							{
								if (Thread.CurrentThread.IsThreadPoolThread)
								{
									this.PostReceive();
								}
								else
								{
									ThreadPool.UnsafeQueueUserWorkItem(Connection.m_PostReceiveDelegate, this);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				result = true;
				if (this.m_InnerException == null)
				{
					this.m_InnerException = ex;
				}
				HttpWebRequest currentRequest2 = this.m_CurrentRequest;
				if (currentRequest2 != null)
				{
					currentRequest2.ErrorStatusCodeNotify(this, false, true);
				}
				this.HandleErrorWithReadDone(WebExceptionStatus.ReceiveFailure, ref connectionReturnResult);
			}
			try
			{
				if ((coreResponseData != null || (connectionReturnResult != null && connectionReturnResult.IsNotEmpty)) && httpWebRequest != null)
				{
					httpWebRequest.SetRequestContinue(coreResponseData);
				}
			}
			finally
			{
				ConnectionReturnResult.SetResponses(connectionReturnResult);
			}
			return result;
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00098B20 File Offset: 0x00097B20
		internal void Write(ScatterGatherBuffers writeBuffer)
		{
			BufferOffsetSize[] buffers = writeBuffer.GetBuffers();
			if (buffers != null)
			{
				base.MultipleWrite(buffers);
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x00098B40 File Offset: 0x00097B40
		private static int FindChunkEntitySize(byte[] buffer, int offset, int size)
		{
			BufferChunkBytes bufferChunkBytes = default(BufferChunkBytes);
			int num = offset;
			int num2 = offset + size;
			bufferChunkBytes.Buffer = buffer;
			while (offset < num2)
			{
				bufferChunkBytes.Offset = offset;
				bufferChunkBytes.Count = size;
				int num4;
				int num3 = ChunkParse.GetChunkSize(bufferChunkBytes, out num4);
				if (num3 == -1)
				{
					return -1;
				}
				if (num3 == 0)
				{
					return 0;
				}
				offset += num3;
				size -= num3;
				if (num4 != 0)
				{
					bufferChunkBytes.Offset = offset;
					bufferChunkBytes.Count = size;
					num3 = ChunkParse.SkipPastCRLF(bufferChunkBytes);
					if (num3 <= 0)
					{
						return num3;
					}
					offset += num3;
					size -= num3;
					offset += num4 + 2;
					size -= num4 + 2;
				}
				else
				{
					if (size < 2)
					{
						return -1;
					}
					offset += 2;
					size -= 2;
					while (size >= 2 && buffer[offset] != 13 && buffer[offset + 1] != 10)
					{
						bufferChunkBytes.Offset = offset;
						bufferChunkBytes.Count = size;
						num3 = ChunkParse.SkipPastCRLF(bufferChunkBytes);
						if (num3 <= 0)
						{
							return num3;
						}
						offset += num3;
						size -= num3;
					}
					if (size >= 2)
					{
						return offset + 2 - num;
					}
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00098C44 File Offset: 0x00097C44
		private static void PostReceiveWrapper(object state)
		{
			Connection connection = state as Connection;
			connection.PostReceive();
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00098C60 File Offset: 0x00097C60
		private void PostReceive()
		{
			try
			{
				if (this.m_LastAsyncResult != null && !this.m_LastAsyncResult.IsCompleted)
				{
					throw new InternalException();
				}
				this.m_LastAsyncResult = this.UnsafeBeginRead(this.m_ReadBuffer, this.m_BytesRead, this.m_ReadBuffer.Length - this.m_BytesRead, Connection.m_ReadCallback, this);
				if (this.m_LastAsyncResult.CompletedSynchronously)
				{
					this.ReadCallback(this.m_LastAsyncResult);
				}
			}
			catch (Exception)
			{
				HttpWebRequest currentRequest = this.m_CurrentRequest;
				if (currentRequest != null)
				{
					currentRequest.ErrorStatusCodeNotify(this, false, true);
				}
				ConnectionReturnResult responses = null;
				this.HandleErrorWithReadDone(WebExceptionStatus.ReceiveFailure, ref responses);
				ConnectionReturnResult.SetResponses(responses);
			}
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00098D08 File Offset: 0x00097D08
		private static void TunnelThroughProxyWrapper(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			bool flag = false;
			WebExceptionStatus webExceptionStatus = WebExceptionStatus.ConnectFailure;
			HttpWebRequest httpWebRequest = (HttpWebRequest)((LazyAsyncResult)result).AsyncObject;
			Connection connection = ((TunnelStateObject)result.AsyncState).Connection;
			HttpWebRequest originalRequest = ((TunnelStateObject)result.AsyncState).OriginalRequest;
			try
			{
				httpWebRequest.EndGetResponse(result);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				ConnectStream connectStream = (ConnectStream)httpWebResponse.GetResponseStream();
				connection.NetworkStream = new NetworkStream(connectStream.Connection.NetworkStream, true);
				connectStream.Connection.NetworkStream.ConvertToNotSocketOwner();
				flag = true;
			}
			catch (Exception ex)
			{
				if (connection.m_InnerException == null)
				{
					connection.m_InnerException = ex;
				}
				if (ex is WebException)
				{
					webExceptionStatus = ((WebException)ex).Status;
				}
			}
			if (!flag)
			{
				ConnectionReturnResult responses = null;
				connection.HandleError(false, false, webExceptionStatus, ref responses);
				ConnectionReturnResult.SetResponses(responses);
				return;
			}
			connection.CompleteConnection(true, originalRequest);
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00098E04 File Offset: 0x00097E04
		private bool TunnelThroughProxy(Uri proxy, HttpWebRequest originalRequest, bool async)
		{
			bool result = false;
			HttpWebRequest httpWebRequest = null;
			try
			{
				new WebPermission(NetworkAccess.Connect, proxy).Assert();
				try
				{
					httpWebRequest = new HttpWebRequest(proxy, originalRequest.Address, originalRequest);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				httpWebRequest.Credentials = ((originalRequest.InternalProxy == null) ? null : originalRequest.InternalProxy.Credentials);
				httpWebRequest.InternalProxy = null;
				httpWebRequest.PreAuthenticate = true;
				HttpWebResponse httpWebResponse;
				if (async)
				{
					TunnelStateObject tunnelStateObject = new TunnelStateObject(originalRequest, this);
					IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(Connection.m_TunnelCallback, tunnelStateObject);
					if (!asyncResult.CompletedSynchronously)
					{
						return true;
					}
					httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
				}
				else
				{
					httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				}
				ConnectStream connectStream = (ConnectStream)httpWebResponse.GetResponseStream();
				base.NetworkStream = new NetworkStream(connectStream.Connection.NetworkStream, true);
				connectStream.Connection.NetworkStream.ConvertToNotSocketOwner();
				result = true;
			}
			catch (Exception innerException)
			{
				if (this.m_InnerException == null)
				{
					this.m_InnerException = innerException;
				}
			}
			return result;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00098F1C File Offset: 0x00097F1C
		private void CheckNonIdle()
		{
			if (this.m_Idle && this.BusyCount != 0)
			{
				this.m_Idle = false;
				this.ServicePoint.IncrementConnection();
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x00098F40 File Offset: 0x00097F40
		private void CheckIdle()
		{
			if (!this.m_Idle && this.BusyCount == 0)
			{
				this.m_Idle = true;
				this.ServicePoint.DecrementConnection();
				if (this.ConnectionGroup != null)
				{
					this.ConnectionGroup.ConnectionGoneIdle();
				}
				this.m_IdleSinceUtc = DateTime.UtcNow;
			}
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x00098F90 File Offset: 0x00097F90
		[Conditional("TRAVE")]
		private void DebugDumpArrayListEntries(ArrayList list, string listType)
		{
			for (int i = 0; i < list.Count; i++)
			{
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00098FAE File Offset: 0x00097FAE
		[Conditional("DEBUG")]
		internal void Debug(int requestHash)
		{
		}

		// Token: 0x04002574 RID: 9588
		private const int CRLFSize = 2;

		// Token: 0x04002575 RID: 9589
		private const long c_InvalidContentLength = -2L;

		// Token: 0x04002576 RID: 9590
		private const int BeforeVersionNumbers = 0;

		// Token: 0x04002577 RID: 9591
		private const int MajorVersionNumber = 1;

		// Token: 0x04002578 RID: 9592
		private const int MinorVersionNumber = 2;

		// Token: 0x04002579 RID: 9593
		private const int StatusCodeNumber = 3;

		// Token: 0x0400257A RID: 9594
		private const int AfterStatusCode = 4;

		// Token: 0x0400257B RID: 9595
		private const int AfterCarriageReturn = 5;

		// Token: 0x0400257C RID: 9596
		private const string BeforeVersionNumberBytes = "HTTP/";

		// Token: 0x0400257D RID: 9597
		[ThreadStatic]
		private static int t_SyncReadNesting;

		// Token: 0x0400257E RID: 9598
		private WebExceptionStatus m_Error;

		// Token: 0x0400257F RID: 9599
		internal Exception m_InnerException;

		// Token: 0x04002580 RID: 9600
		internal int m_IISVersion = -1;

		// Token: 0x04002581 RID: 9601
		private byte[] m_ReadBuffer;

		// Token: 0x04002582 RID: 9602
		private int m_BytesRead;

		// Token: 0x04002583 RID: 9603
		private int m_BytesScanned;

		// Token: 0x04002584 RID: 9604
		private int m_TotalResponseHeadersLength;

		// Token: 0x04002585 RID: 9605
		private int m_MaximumResponseHeadersLength;

		// Token: 0x04002586 RID: 9606
		private long m_MaximumUnauthorizedUploadLength;

		// Token: 0x04002587 RID: 9607
		private CoreResponseData m_ResponseData;

		// Token: 0x04002588 RID: 9608
		private ReadState m_ReadState;

		// Token: 0x04002589 RID: 9609
		private Connection.StatusLineValues m_StatusLineValues;

		// Token: 0x0400258A RID: 9610
		private int m_StatusState;

		// Token: 0x0400258B RID: 9611
		private ArrayList m_WaitList;

		// Token: 0x0400258C RID: 9612
		private ArrayList m_WriteList;

		// Token: 0x0400258D RID: 9613
		private IAsyncResult m_LastAsyncResult;

		// Token: 0x0400258E RID: 9614
		private TimerThread.Timer m_RecycleTimer;

		// Token: 0x0400258F RID: 9615
		private WebParseError m_ParseError;

		// Token: 0x04002590 RID: 9616
		private bool m_AtLeastOneResponseReceived;

		// Token: 0x04002591 RID: 9617
		private static readonly WaitCallback m_PostReceiveDelegate = new WaitCallback(Connection.PostReceiveWrapper);

		// Token: 0x04002592 RID: 9618
		private static readonly AsyncCallback m_ReadCallback = new AsyncCallback(Connection.ReadCallbackWrapper);

		// Token: 0x04002593 RID: 9619
		private static readonly AsyncCallback m_TunnelCallback = new AsyncCallback(Connection.TunnelThroughProxyWrapper);

		// Token: 0x04002594 RID: 9620
		private static byte[] s_NullBuffer = new byte[0];

		// Token: 0x04002595 RID: 9621
		private HttpAbortDelegate m_AbortDelegate;

		// Token: 0x04002596 RID: 9622
		private ConnectionGroup m_ConnectionGroup;

		// Token: 0x04002597 RID: 9623
		private UnlockConnectionDelegate m_ConnectionUnlock;

		// Token: 0x04002598 RID: 9624
		private DateTime m_IdleSinceUtc;

		// Token: 0x04002599 RID: 9625
		private HttpWebRequest m_LockedRequest;

		// Token: 0x0400259A RID: 9626
		private HttpWebRequest m_CurrentRequest;

		// Token: 0x0400259B RID: 9627
		private bool m_CanPipeline;

		// Token: 0x0400259C RID: 9628
		private bool m_Free = true;

		// Token: 0x0400259D RID: 9629
		private bool m_Idle = true;

		// Token: 0x0400259E RID: 9630
		private bool m_KeepAlive = true;

		// Token: 0x0400259F RID: 9631
		private bool m_Pipelining;

		// Token: 0x040025A0 RID: 9632
		private int m_ReservedCount;

		// Token: 0x040025A1 RID: 9633
		private bool m_ReadDone;

		// Token: 0x040025A2 RID: 9634
		private bool m_WriteDone;

		// Token: 0x040025A3 RID: 9635
		private bool m_RemovedFromConnectionList;

		// Token: 0x040025A4 RID: 9636
		private bool m_IsPipelinePaused;

		// Token: 0x040025A5 RID: 9637
		private static int s_MaxPipelinedCount = 10;

		// Token: 0x040025A6 RID: 9638
		private static int s_MinPipelinedCount = 5;

		// Token: 0x040025A7 RID: 9639
		private static readonly string[] s_ShortcutStatusDescriptions = new string[]
		{
			"OK",
			"Continue",
			"Unauthorized"
		};

		// Token: 0x020004C9 RID: 1225
		private class StatusLineValues
		{
			// Token: 0x040025A8 RID: 9640
			internal int MajorVersion;

			// Token: 0x040025A9 RID: 9641
			internal int MinorVersion;

			// Token: 0x040025AA RID: 9642
			internal int StatusCode;

			// Token: 0x040025AB RID: 9643
			internal string StatusDescription;
		}

		// Token: 0x020004CA RID: 1226
		private class AsyncTriState
		{
			// Token: 0x060025EC RID: 9708 RVA: 0x00099035 File Offset: 0x00098035
			public AsyncTriState(TriState newValue)
			{
				this.Value = newValue;
			}

			// Token: 0x040025AC RID: 9644
			public TriState Value;
		}
	}
}
