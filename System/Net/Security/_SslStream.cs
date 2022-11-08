using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x020005A0 RID: 1440
	internal class _SslStream
	{
		// Token: 0x06002C98 RID: 11416 RVA: 0x000C0697 File Offset: 0x000BF697
		internal _SslStream(SslState sslState)
		{
			this._SslState = sslState;
			this._Reader = new FixedSizeReader(this._SslState.InnerStream);
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000C06BC File Offset: 0x000BF6BC
		internal int Read(byte[] buffer, int offset, int count)
		{
			return this.ProcessRead(buffer, offset, count, null);
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000C06C8 File Offset: 0x000BF6C8
		internal void Write(byte[] buffer, int offset, int count)
		{
			this.ProcessWrite(buffer, offset, count, null);
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000C06D4 File Offset: 0x000BF6D4
		internal void Write(BufferOffsetSize[] buffers)
		{
			this.ProcessWrite(buffers, null);
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000C06E0 File Offset: 0x000BF6E0
		internal IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			BufferAsyncResult bufferAsyncResult = new BufferAsyncResult(this, buffer, offset, count, asyncState, asyncCallback);
			AsyncProtocolRequest asyncRequest = new AsyncProtocolRequest(bufferAsyncResult);
			this.ProcessRead(buffer, offset, count, asyncRequest);
			return bufferAsyncResult;
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000C0710 File Offset: 0x000BF710
		internal int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			BufferAsyncResult bufferAsyncResult = asyncResult as BufferAsyncResult;
			if (bufferAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[]
				{
					asyncResult.GetType().FullName
				}), "asyncResult");
			}
			if (Interlocked.Exchange(ref this._NestedRead, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndRead"
				}));
			}
			bufferAsyncResult.InternalWaitForCompletion();
			if (!(bufferAsyncResult.Result is Exception))
			{
				return (int)bufferAsyncResult.Result;
			}
			if (bufferAsyncResult.Result is IOException)
			{
				throw (Exception)bufferAsyncResult.Result;
			}
			throw new IOException(SR.GetString("net_io_write"), (Exception)bufferAsyncResult.Result);
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x000C07E4 File Offset: 0x000BF7E4
		internal IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this, asyncState, asyncCallback);
			AsyncProtocolRequest asyncRequest = new AsyncProtocolRequest(lazyAsyncResult);
			this.ProcessWrite(buffer, offset, count, asyncRequest);
			return lazyAsyncResult;
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x000C0810 File Offset: 0x000BF810
		internal IAsyncResult BeginWrite(BufferOffsetSize[] buffers, AsyncCallback asyncCallback, object asyncState)
		{
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this, asyncState, asyncCallback);
			_SslStream.SplitWriteAsyncProtocolRequest asyncRequest = new _SslStream.SplitWriteAsyncProtocolRequest(lazyAsyncResult);
			this.ProcessWrite(buffers, asyncRequest);
			return lazyAsyncResult;
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000C0838 File Offset: 0x000BF838
		internal void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[]
				{
					asyncResult.GetType().FullName
				}), "asyncResult");
			}
			if (Interlocked.Exchange(ref this._NestedWrite, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndWrite"
				}));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			if (!(lazyAsyncResult.Result is Exception))
			{
				return;
			}
			if (lazyAsyncResult.Result is IOException)
			{
				throw (Exception)lazyAsyncResult.Result;
			}
			throw new IOException(SR.GetString("net_io_write"), (Exception)lazyAsyncResult.Result);
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x000C08FE File Offset: 0x000BF8FE
		internal bool DataAvailable
		{
			get
			{
				return this.InternalBufferCount != 0;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000C090C File Offset: 0x000BF90C
		private byte[] InternalBuffer
		{
			get
			{
				return this._InternalBuffer;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000C0914 File Offset: 0x000BF914
		private int InternalOffset
		{
			get
			{
				return this._InternalOffset;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000C091C File Offset: 0x000BF91C
		private int InternalBufferCount
		{
			get
			{
				return this._InternalBufferCount;
			}
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000C0924 File Offset: 0x000BF924
		private void DecrementInternalBufferCount(int decrCount)
		{
			this._InternalOffset += decrCount;
			this._InternalBufferCount -= decrCount;
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000C0944 File Offset: 0x000BF944
		private void EnsureInternalBufferSize(int curOffset, int addSize)
		{
			if (this._InternalBuffer == null || this._InternalBuffer.Length < addSize + curOffset)
			{
				byte[] internalBuffer = this._InternalBuffer;
				this._InternalBuffer = new byte[addSize + curOffset];
				if (internalBuffer != null && curOffset != 0)
				{
					Buffer.BlockCopy(internalBuffer, 0, this._InternalBuffer, 0, curOffset);
				}
			}
			this._InternalOffset = curOffset;
			this._InternalBufferCount = curOffset + addSize;
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000C09A0 File Offset: 0x000BF9A0
		private void ValidateParameters(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_offset_plus_count"));
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000C09F4 File Offset: 0x000BF9F4
		private void ProcessWrite(BufferOffsetSize[] buffers, _SslStream.SplitWriteAsyncProtocolRequest asyncRequest)
		{
			foreach (BufferOffsetSize bufferOffsetSize in buffers)
			{
				this.ValidateParameters(bufferOffsetSize.Buffer, bufferOffsetSize.Offset, bufferOffsetSize.Size);
			}
			if (Interlocked.Exchange(ref this._NestedWrite, 1) == 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					(asyncRequest != null) ? "BeginWrite" : "Write",
					"write"
				}));
			}
			bool flag = false;
			try
			{
				SplitWritesState splitWritesState = new SplitWritesState(buffers);
				if (asyncRequest != null)
				{
					asyncRequest.SetNextRequest(splitWritesState, _SslStream._ResumeAsyncWriteCallback);
				}
				this.StartWriting(splitWritesState, asyncRequest);
			}
			catch (Exception ex)
			{
				this._SslState.FinishWrite();
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_write"), ex);
			}
			catch
			{
				this._SslState.FinishWrite();
				flag = true;
				throw new IOException(SR.GetString("net_io_write"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			finally
			{
				if (asyncRequest == null || flag)
				{
					this._NestedWrite = 0;
				}
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000C0B2C File Offset: 0x000BFB2C
		private void ProcessWrite(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (this._SslState.LastPayload != null)
			{
				BufferOffsetSize[] buffers = new BufferOffsetSize[]
				{
					new BufferOffsetSize(buffer, offset, count, false)
				};
				if (asyncRequest != null)
				{
					this.ProcessWrite(buffers, new _SslStream.SplitWriteAsyncProtocolRequest(asyncRequest.UserAsyncResult));
					return;
				}
				this.ProcessWrite(buffers, null);
				return;
			}
			else
			{
				this.ValidateParameters(buffer, offset, count);
				if (Interlocked.Exchange(ref this._NestedWrite, 1) == 1)
				{
					throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
					{
						(asyncRequest != null) ? "BeginWrite" : "Write",
						"write"
					}));
				}
				bool flag = false;
				try
				{
					this.StartWriting(buffer, offset, count, asyncRequest);
				}
				catch (Exception ex)
				{
					this._SslState.FinishWrite();
					flag = true;
					if (ex is IOException)
					{
						throw;
					}
					throw new IOException(SR.GetString("net_io_write"), ex);
				}
				catch
				{
					this._SslState.FinishWrite();
					flag = true;
					throw new IOException(SR.GetString("net_io_write"), new Exception(SR.GetString("net_nonClsCompliantException")));
				}
				finally
				{
					if (asyncRequest == null || flag)
					{
						this._NestedWrite = 0;
					}
				}
				return;
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x000C0C64 File Offset: 0x000BFC64
		private void StartWriting(SplitWritesState splitWrite, _SslStream.SplitWriteAsyncProtocolRequest asyncRequest)
		{
			while (!splitWrite.IsDone)
			{
				if (this._SslState.CheckEnqueueWrite(asyncRequest))
				{
					return;
				}
				byte[] lastHandshakePayload = null;
				if (this._SslState.LastPayload != null)
				{
					lastHandshakePayload = this._SslState.LastPayload;
					this._SslState.LastPayloadConsumed();
				}
				BufferOffsetSize[] buffers = splitWrite.GetNextBuffers();
				buffers = this.EncryptBuffers(buffers, lastHandshakePayload);
				if (asyncRequest != null)
				{
					IAsyncResult asyncResult = ((NetworkStream)this._SslState.InnerStream).BeginMultipleWrite(buffers, _SslStream._MulitpleWriteCallback, asyncRequest);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					((NetworkStream)this._SslState.InnerStream).EndMultipleWrite(asyncResult);
				}
				else
				{
					((NetworkStream)this._SslState.InnerStream).MultipleWrite(buffers);
				}
				this._SslState.FinishWrite();
			}
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser();
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000C0D34 File Offset: 0x000BFD34
		private BufferOffsetSize[] EncryptBuffers(BufferOffsetSize[] buffers, byte[] lastHandshakePayload)
		{
			List<BufferOffsetSize> list = null;
			SecurityStatus securityStatus = SecurityStatus.OK;
			foreach (BufferOffsetSize bufferOffsetSize in buffers)
			{
				int num = Math.Min(bufferOffsetSize.Size, this._SslState.MaxDataSize);
				byte[] buffer = null;
				int size;
				securityStatus = this._SslState.EncryptData(bufferOffsetSize.Buffer, bufferOffsetSize.Offset, num, ref buffer, out size);
				if (securityStatus != SecurityStatus.OK)
				{
					break;
				}
				if (num != bufferOffsetSize.Size || list != null)
				{
					if (list == null)
					{
						list = new List<BufferOffsetSize>(buffers.Length * (bufferOffsetSize.Size / num + 1));
						if (lastHandshakePayload != null)
						{
							list.Add(new BufferOffsetSize(lastHandshakePayload, false));
						}
						foreach (BufferOffsetSize bufferOffsetSize2 in buffers)
						{
							if (bufferOffsetSize2 == bufferOffsetSize)
							{
								break;
							}
							list.Add(bufferOffsetSize2);
						}
					}
					list.Add(new BufferOffsetSize(buffer, 0, size, false));
					while ((bufferOffsetSize.Size -= num) != 0)
					{
						bufferOffsetSize.Offset += num;
						num = Math.Min(bufferOffsetSize.Size, this._SslState.MaxDataSize);
						securityStatus = this._SslState.EncryptData(bufferOffsetSize.Buffer, bufferOffsetSize.Offset, num, ref buffer, out size);
						if (securityStatus != SecurityStatus.OK)
						{
							break;
						}
						list.Add(new BufferOffsetSize(buffer, 0, size, false));
					}
				}
				else
				{
					bufferOffsetSize.Buffer = buffer;
					bufferOffsetSize.Offset = 0;
					bufferOffsetSize.Size = size;
				}
				if (securityStatus != SecurityStatus.OK)
				{
					break;
				}
			}
			if (securityStatus != SecurityStatus.OK)
			{
				ProtocolToken protocolToken = new ProtocolToken(null, securityStatus);
				throw new IOException(SR.GetString("net_io_encrypt"), protocolToken.GetException());
			}
			if (list != null)
			{
				buffers = list.ToArray();
			}
			else if (lastHandshakePayload != null)
			{
				BufferOffsetSize[] array3 = new BufferOffsetSize[buffers.Length + 1];
				Array.Copy(buffers, 0, array3, 1, buffers.Length);
				array3[0] = new BufferOffsetSize(lastHandshakePayload, false);
				buffers = array3;
			}
			return buffers;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000C0EFC File Offset: 0x000BFEFC
		private void StartWriting(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(buffer, offset, count, _SslStream._ResumeAsyncWriteCallback);
			}
			if (count >= 0)
			{
				byte[] buffer2 = null;
				while (!this._SslState.CheckEnqueueWrite(asyncRequest))
				{
					int num = Math.Min(count, this._SslState.MaxDataSize);
					int count2;
					SecurityStatus securityStatus = this._SslState.EncryptData(buffer, offset, num, ref buffer2, out count2);
					if (securityStatus != SecurityStatus.OK)
					{
						ProtocolToken protocolToken = new ProtocolToken(null, securityStatus);
						throw new IOException(SR.GetString("net_io_encrypt"), protocolToken.GetException());
					}
					if (asyncRequest != null)
					{
						asyncRequest.SetNextRequest(buffer, offset + num, count - num, _SslStream._ResumeAsyncWriteCallback);
						IAsyncResult asyncResult = this._SslState.InnerStream.BeginWrite(buffer2, 0, count2, _SslStream._WriteCallback, asyncRequest);
						if (!asyncResult.CompletedSynchronously)
						{
							return;
						}
						this._SslState.InnerStream.EndWrite(asyncResult);
					}
					else
					{
						this._SslState.InnerStream.Write(buffer2, 0, count2);
					}
					offset += num;
					count -= num;
					this._SslState.FinishWrite();
					if (count == 0)
					{
						goto IL_F3;
					}
				}
				return;
			}
			IL_F3:
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser();
			}
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000C1008 File Offset: 0x000C0008
		private int ProcessRead(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			this.ValidateParameters(buffer, offset, count);
			if (Interlocked.Exchange(ref this._NestedRead, 1) == 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					(asyncRequest != null) ? "BeginRead" : "Read",
					"read"
				}));
			}
			bool flag = false;
			int result;
			try
			{
				if (this.InternalBufferCount != 0)
				{
					int num = (this.InternalBufferCount > count) ? count : this.InternalBufferCount;
					if (num != 0)
					{
						Buffer.BlockCopy(this.InternalBuffer, this.InternalOffset, buffer, offset, num);
						this.DecrementInternalBufferCount(num);
					}
					if (asyncRequest != null)
					{
						asyncRequest.CompleteUser(num);
					}
					result = num;
				}
				else
				{
					result = this.StartReading(buffer, offset, count, asyncRequest);
				}
			}
			catch (Exception ex)
			{
				this._SslState.FinishRead(null);
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_read"), ex);
			}
			catch
			{
				this._SslState.FinishRead(null);
				flag = true;
				throw new IOException(SR.GetString("net_io_read"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			finally
			{
				if (asyncRequest == null || flag)
				{
					this._NestedRead = 0;
				}
			}
			return result;
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000C1154 File Offset: 0x000C0154
		private int StartReading(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			int num;
			for (;;)
			{
				if (asyncRequest != null)
				{
					asyncRequest.SetNextRequest(buffer, offset, count, _SslStream._ResumeAsyncReadCallback);
				}
				num = this._SslState.CheckEnqueueRead(buffer, offset, count, asyncRequest);
				if (num == 0)
				{
					break;
				}
				if (num != -1)
				{
					goto Block_2;
				}
				int result;
				if ((result = this.StartFrameHeader(buffer, offset, count, asyncRequest)) != -1)
				{
					return result;
				}
			}
			return 0;
			Block_2:
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser(num);
			}
			return num;
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000C11B4 File Offset: 0x000C01B4
		private int StartFrameHeader(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			this.EnsureInternalBufferSize(0, 5);
			int readBytes;
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this.InternalBuffer, 0, 5, _SslStream._ReadHeaderCallback);
				this._Reader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				readBytes = asyncRequest.Result;
			}
			else
			{
				readBytes = this._Reader.ReadPacket(this.InternalBuffer, 0, 5);
			}
			return this.StartFrameBody(readBytes, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000C1228 File Offset: 0x000C0228
		private int StartFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				this.DecrementInternalBufferCount(this.InternalBufferCount);
				if (asyncRequest != null)
				{
					asyncRequest.CompleteUser(0);
				}
				return 0;
			}
			readBytes = this._SslState.GetRemainingFrameSize(this.InternalBuffer, readBytes);
			if (readBytes < 0)
			{
				throw new IOException(SR.GetString("net_frame_read_size"));
			}
			this.EnsureInternalBufferSize(5, readBytes);
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this.InternalBuffer, 5, readBytes, _SslStream._ReadFrameCallback);
				this._Reader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				readBytes = asyncRequest.Result;
			}
			else
			{
				readBytes = this._Reader.ReadPacket(this.InternalBuffer, 5, readBytes);
			}
			return this.ProcessFrameBody(readBytes, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000C12E4 File Offset: 0x000C02E4
		private int ProcessFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				throw new IOException(SR.GetString("net_io_eof"));
			}
			readBytes += 5;
			int num = 0;
			SecurityStatus securityStatus = this._SslState.DecryptData(this.InternalBuffer, ref num, ref readBytes);
			if (securityStatus != SecurityStatus.OK)
			{
				byte[] array = null;
				if (readBytes != 0)
				{
					array = new byte[readBytes];
					Buffer.BlockCopy(this.InternalBuffer, num, array, 0, readBytes);
				}
				this.DecrementInternalBufferCount(this.InternalBufferCount);
				return this.ProcessReadErrorCode(securityStatus, buffer, offset, count, asyncRequest, array);
			}
			if (readBytes == 0 && count != 0)
			{
				this.DecrementInternalBufferCount(this.InternalBufferCount);
				return -1;
			}
			this.EnsureInternalBufferSize(0, num + readBytes);
			this.DecrementInternalBufferCount(num);
			if (readBytes > count)
			{
				readBytes = count;
			}
			Buffer.BlockCopy(this.InternalBuffer, this.InternalOffset, buffer, offset, readBytes);
			this.DecrementInternalBufferCount(readBytes);
			this._SslState.FinishRead(null);
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser(readBytes);
			}
			return readBytes;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000C13C4 File Offset: 0x000C03C4
		private int ProcessReadErrorCode(SecurityStatus errorCode, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest, byte[] extraBuffer)
		{
			ProtocolToken protocolToken = new ProtocolToken(null, errorCode);
			if (protocolToken.Renegotiate)
			{
				this._SslState.ReplyOnReAuthentication(extraBuffer);
				return -1;
			}
			if (protocolToken.CloseConnection)
			{
				this._SslState.FinishRead(null);
				if (asyncRequest != null)
				{
					asyncRequest.CompleteUser(0);
				}
				return 0;
			}
			throw new IOException(SR.GetString("net_io_decrypt"), protocolToken.GetException());
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000C142C File Offset: 0x000C042C
		private static void WriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			AsyncProtocolRequest asyncProtocolRequest = (AsyncProtocolRequest)transportResult.AsyncState;
			_SslStream sslStream = (_SslStream)asyncProtocolRequest.AsyncObject;
			try
			{
				sslStream._SslState.InnerStream.EndWrite(transportResult);
				sslStream._SslState.FinishWrite();
				if (asyncProtocolRequest.Count == 0)
				{
					asyncProtocolRequest.Count = -1;
				}
				sslStream.StartWriting(asyncProtocolRequest.Buffer, asyncProtocolRequest.Offset, asyncProtocolRequest.Count, asyncProtocolRequest);
			}
			catch (Exception e)
			{
				if (asyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				sslStream._SslState.FinishWrite();
				asyncProtocolRequest.CompleteWithError(e);
			}
			catch
			{
				if (asyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				sslStream._SslState.FinishWrite();
				asyncProtocolRequest.CompleteWithError(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000C1508 File Offset: 0x000C0508
		private static void MulitpleWriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			_SslStream.SplitWriteAsyncProtocolRequest splitWriteAsyncProtocolRequest = (_SslStream.SplitWriteAsyncProtocolRequest)transportResult.AsyncState;
			_SslStream sslStream = (_SslStream)splitWriteAsyncProtocolRequest.AsyncObject;
			try
			{
				((NetworkStream)sslStream._SslState.InnerStream).EndMultipleWrite(transportResult);
				sslStream._SslState.FinishWrite();
				sslStream.StartWriting(splitWriteAsyncProtocolRequest.SplitWritesState, splitWriteAsyncProtocolRequest);
			}
			catch (Exception e)
			{
				if (splitWriteAsyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				sslStream._SslState.FinishWrite();
				splitWriteAsyncProtocolRequest.CompleteWithError(e);
			}
			catch
			{
				if (splitWriteAsyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				sslStream._SslState.FinishWrite();
				splitWriteAsyncProtocolRequest.CompleteWithError(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000C15D0 File Offset: 0x000C05D0
		private static void ResumeAsyncReadCallback(AsyncProtocolRequest request)
		{
			try
			{
				((_SslStream)request.AsyncObject).StartReading(request.Buffer, request.Offset, request.Count, request);
			}
			catch (Exception e)
			{
				if (request.IsUserCompleted)
				{
					throw;
				}
				((_SslStream)request.AsyncObject)._SslState.FinishRead(null);
				request.CompleteWithError(e);
			}
			catch
			{
				if (request.IsUserCompleted)
				{
					throw;
				}
				((_SslStream)request.AsyncObject)._SslState.FinishRead(null);
				request.CompleteWithError(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000C1684 File Offset: 0x000C0684
		private static void ResumeAsyncWriteCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				_SslStream.SplitWriteAsyncProtocolRequest splitWriteAsyncProtocolRequest = asyncRequest as _SslStream.SplitWriteAsyncProtocolRequest;
				if (splitWriteAsyncProtocolRequest != null)
				{
					((_SslStream)asyncRequest.AsyncObject).StartWriting(splitWriteAsyncProtocolRequest.SplitWritesState, splitWriteAsyncProtocolRequest);
				}
				else
				{
					((_SslStream)asyncRequest.AsyncObject).StartWriting(asyncRequest.Buffer, asyncRequest.Offset, asyncRequest.Count, asyncRequest);
				}
			}
			catch (Exception e)
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				((_SslStream)asyncRequest.AsyncObject)._SslState.FinishWrite();
				asyncRequest.CompleteWithError(e);
			}
			catch
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				((_SslStream)asyncRequest.AsyncObject)._SslState.FinishWrite();
				asyncRequest.CompleteWithError(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000C1758 File Offset: 0x000C0758
		private static void ReadHeaderCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				_SslStream sslStream = (_SslStream)asyncRequest.AsyncObject;
				BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)asyncRequest.UserAsyncResult;
				if (-1 == sslStream.StartFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest))
				{
					sslStream.StartReading(bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
				}
			}
			catch (Exception e)
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				asyncRequest.CompleteWithError(e);
			}
			catch
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				asyncRequest.CompleteWithError(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000C1810 File Offset: 0x000C0810
		private static void ReadFrameCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				_SslStream sslStream = (_SslStream)asyncRequest.AsyncObject;
				BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)asyncRequest.UserAsyncResult;
				if (-1 == sslStream.ProcessFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest))
				{
					sslStream.StartReading(bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
				}
			}
			catch (Exception e)
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				asyncRequest.CompleteWithError(e);
			}
			catch
			{
				if (asyncRequest.IsUserCompleted)
				{
					throw;
				}
				asyncRequest.CompleteWithError(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x04002A50 RID: 10832
		private static AsyncCallback _WriteCallback = new AsyncCallback(_SslStream.WriteCallback);

		// Token: 0x04002A51 RID: 10833
		private static AsyncCallback _MulitpleWriteCallback = new AsyncCallback(_SslStream.MulitpleWriteCallback);

		// Token: 0x04002A52 RID: 10834
		private static AsyncProtocolCallback _ResumeAsyncWriteCallback = new AsyncProtocolCallback(_SslStream.ResumeAsyncWriteCallback);

		// Token: 0x04002A53 RID: 10835
		private static AsyncProtocolCallback _ResumeAsyncReadCallback = new AsyncProtocolCallback(_SslStream.ResumeAsyncReadCallback);

		// Token: 0x04002A54 RID: 10836
		private static AsyncProtocolCallback _ReadHeaderCallback = new AsyncProtocolCallback(_SslStream.ReadHeaderCallback);

		// Token: 0x04002A55 RID: 10837
		private static AsyncProtocolCallback _ReadFrameCallback = new AsyncProtocolCallback(_SslStream.ReadFrameCallback);

		// Token: 0x04002A56 RID: 10838
		private SslState _SslState;

		// Token: 0x04002A57 RID: 10839
		private int _NestedWrite;

		// Token: 0x04002A58 RID: 10840
		private int _NestedRead;

		// Token: 0x04002A59 RID: 10841
		private byte[] _InternalBuffer;

		// Token: 0x04002A5A RID: 10842
		private int _InternalOffset;

		// Token: 0x04002A5B RID: 10843
		private int _InternalBufferCount;

		// Token: 0x04002A5C RID: 10844
		private FixedSizeReader _Reader;

		// Token: 0x020005A1 RID: 1441
		private class SplitWriteAsyncProtocolRequest : AsyncProtocolRequest
		{
			// Token: 0x06002CBA RID: 11450 RVA: 0x000C193B File Offset: 0x000C093B
			internal SplitWriteAsyncProtocolRequest(LazyAsyncResult userAsyncResult) : base(userAsyncResult)
			{
			}

			// Token: 0x06002CBB RID: 11451 RVA: 0x000C1944 File Offset: 0x000C0944
			internal void SetNextRequest(SplitWritesState splitWritesState, AsyncProtocolCallback callback)
			{
				this.SplitWritesState = splitWritesState;
				base.SetNextRequest(null, 0, 0, callback);
			}

			// Token: 0x04002A5D RID: 10845
			internal SplitWritesState SplitWritesState;
		}
	}
}
