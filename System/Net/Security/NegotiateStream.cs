using System;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x0200058B RID: 1419
	public class NegotiateStream : AuthenticatedStream
	{
		// Token: 0x06002B8D RID: 11149 RVA: 0x000BC916 File Offset: 0x000BB916
		public NegotiateStream(Stream innerStream) : this(innerStream, false)
		{
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000BC920 File Offset: 0x000BB920
		public NegotiateStream(Stream innerStream, bool leaveInnerStreamOpen) : base(innerStream, leaveInnerStreamOpen)
		{
			this._NegoState = new NegoState(innerStream, leaveInnerStreamOpen);
			this._Package = NegoState.DefaultPackage;
			this.InitializeStreamPart();
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000BC948 File Offset: 0x000BB948
		public virtual void AuthenticateAsClient()
		{
			this.AuthenticateAsClient((NetworkCredential)CredentialCache.DefaultCredentials, null, string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000BC962 File Offset: 0x000BB962
		public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName)
		{
			this.AuthenticateAsClient(credential, null, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000BC96F File Offset: 0x000BB96F
		public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName)
		{
			this.AuthenticateAsClient(credential, binding, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000BC97C File Offset: 0x000BB97C
		public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
		{
			this.AuthenticateAsClient(credential, null, targetName, requiredProtectionLevel, allowedImpersonationLevel);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000BC98A File Offset: 0x000BB98A
		public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
		{
			this._NegoState.ValidateCreateContext(this._Package, false, credential, targetName, binding, requiredProtectionLevel, allowedImpersonationLevel);
			this._NegoState.ProcessAuthentication(null);
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000BC9B1 File Offset: 0x000BB9B1
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient((NetworkCredential)CredentialCache.DefaultCredentials, null, string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000BC9CD File Offset: 0x000BB9CD
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(credential, null, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000BC9DD File Offset: 0x000BB9DD
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(credential, binding, targetName, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000BC9EE File Offset: 0x000BB9EE
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(credential, null, targetName, requiredProtectionLevel, allowedImpersonationLevel, asyncCallback, asyncState);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000BCA00 File Offset: 0x000BBA00
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			this._NegoState.ValidateCreateContext(this._Package, false, credential, targetName, binding, requiredProtectionLevel, allowedImpersonationLevel);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._NegoState, asyncState, asyncCallback);
			this._NegoState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000BCA43 File Offset: 0x000BBA43
		public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
		{
			this._NegoState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000BCA51 File Offset: 0x000BBA51
		public virtual void AuthenticateAsServer()
		{
			this.AuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, null, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000BCA66 File Offset: 0x000BBA66
		public virtual void AuthenticateAsServer(ExtendedProtectionPolicy policy)
		{
			this.AuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, policy, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000BCA7B File Offset: 0x000BBA7B
		public virtual void AuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
		{
			this.AuthenticateAsServer(credential, null, requiredProtectionLevel, requiredImpersonationLevel);
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000BCA87 File Offset: 0x000BBA87
		public virtual void AuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
		{
			if (!ComNetOS.IsWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
			this._NegoState.ValidateCreateContext(this._Package, credential, string.Empty, policy, requiredProtectionLevel, requiredImpersonationLevel);
			this._NegoState.ProcessAuthentication(null);
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x000BCAC7 File Offset: 0x000BBAC7
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, null, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000BCADE File Offset: 0x000BBADE
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(ExtendedProtectionPolicy policy, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, policy, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification, asyncCallback, asyncState);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000BCAF5 File Offset: 0x000BBAF5
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer(credential, null, requiredProtectionLevel, requiredImpersonationLevel, asyncCallback, asyncState);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000BCB08 File Offset: 0x000BBB08
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback asyncCallback, object asyncState)
		{
			if (!ComNetOS.IsWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
			this._NegoState.ValidateCreateContext(this._Package, credential, string.Empty, policy, requiredProtectionLevel, requiredImpersonationLevel);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._NegoState, asyncState, asyncCallback);
			this._NegoState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000BCB64 File Offset: 0x000BBB64
		public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
		{
			this._NegoState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x000BCB72 File Offset: 0x000BBB72
		public override bool IsAuthenticated
		{
			get
			{
				return this._NegoState.IsAuthenticated;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06002BA4 RID: 11172 RVA: 0x000BCB7F File Offset: 0x000BBB7F
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				return this._NegoState.IsMutuallyAuthenticated;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x000BCB8C File Offset: 0x000BBB8C
		public override bool IsEncrypted
		{
			get
			{
				return this._NegoState.IsEncrypted;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x000BCB99 File Offset: 0x000BBB99
		public override bool IsSigned
		{
			get
			{
				return this._NegoState.IsSigned;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x000BCBA6 File Offset: 0x000BBBA6
		public override bool IsServer
		{
			get
			{
				return this._NegoState.IsServer;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x000BCBB3 File Offset: 0x000BBBB3
		public virtual TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				return this._NegoState.AllowedImpersonation;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000BCBC0 File Offset: 0x000BBBC0
		public virtual IIdentity RemoteIdentity
		{
			get
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				if (this._RemoteIdentity == null)
				{
					this._RemoteIdentity = this._NegoState.GetIdentity();
				}
				return this._RemoteIdentity;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06002BAA RID: 11178 RVA: 0x000BCBEC File Offset: 0x000BBBEC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06002BAB RID: 11179 RVA: 0x000BCBEF File Offset: 0x000BBBEF
		public override bool CanRead
		{
			get
			{
				return this.IsAuthenticated && base.InnerStream.CanRead;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000BCC06 File Offset: 0x000BBC06
		public override bool CanTimeout
		{
			get
			{
				return base.InnerStream.CanTimeout;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06002BAD RID: 11181 RVA: 0x000BCC13 File Offset: 0x000BBC13
		public override bool CanWrite
		{
			get
			{
				return this.IsAuthenticated && base.InnerStream.CanWrite;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x000BCC2A File Offset: 0x000BBC2A
		// (set) Token: 0x06002BAF RID: 11183 RVA: 0x000BCC37 File Offset: 0x000BBC37
		public override int ReadTimeout
		{
			get
			{
				return base.InnerStream.ReadTimeout;
			}
			set
			{
				base.InnerStream.ReadTimeout = value;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x000BCC45 File Offset: 0x000BBC45
		// (set) Token: 0x06002BB1 RID: 11185 RVA: 0x000BCC52 File Offset: 0x000BBC52
		public override int WriteTimeout
		{
			get
			{
				return base.InnerStream.WriteTimeout;
			}
			set
			{
				base.InnerStream.WriteTimeout = value;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x000BCC60 File Offset: 0x000BBC60
		public override long Length
		{
			get
			{
				return base.InnerStream.Length;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06002BB3 RID: 11187 RVA: 0x000BCC6D File Offset: 0x000BBC6D
		// (set) Token: 0x06002BB4 RID: 11188 RVA: 0x000BCC7A File Offset: 0x000BBC7A
		public override long Position
		{
			get
			{
				return base.InnerStream.Position;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000BCC8B File Offset: 0x000BBC8B
		public override void SetLength(long value)
		{
			base.InnerStream.SetLength(value);
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000BCC99 File Offset: 0x000BBC99
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000BCCAA File Offset: 0x000BBCAA
		public override void Flush()
		{
			base.InnerStream.Flush();
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000BCCB8 File Offset: 0x000BBCB8
		protected override void Dispose(bool disposing)
		{
			try
			{
				this._NegoState.Close();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x000BCCEC File Offset: 0x000BBCEC
		public override int Read(byte[] buffer, int offset, int count)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.Read(buffer, offset, count);
			}
			return this.ProcessRead(buffer, offset, count, null);
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000BCD20 File Offset: 0x000BBD20
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				base.InnerStream.Write(buffer, offset, count);
				return;
			}
			this.ProcessWrite(buffer, offset, count, null);
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000BCD54 File Offset: 0x000BBD54
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.BeginRead(buffer, offset, count, asyncCallback, asyncState);
			}
			BufferAsyncResult bufferAsyncResult = new BufferAsyncResult(this, buffer, offset, count, asyncState, asyncCallback);
			AsyncProtocolRequest asyncRequest = new AsyncProtocolRequest(bufferAsyncResult);
			this.ProcessRead(buffer, offset, count, asyncRequest);
			return bufferAsyncResult;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000BCDB0 File Offset: 0x000BBDB0
		public override int EndRead(IAsyncResult asyncResult)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.EndRead(asyncResult);
			}
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

		// Token: 0x06002BBD RID: 11197 RVA: 0x000BCEA8 File Offset: 0x000BBEA8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				return base.InnerStream.BeginWrite(buffer, offset, count, asyncCallback, asyncState);
			}
			BufferAsyncResult bufferAsyncResult = new BufferAsyncResult(this, buffer, offset, count, true, asyncState, asyncCallback);
			AsyncProtocolRequest asyncRequest = new AsyncProtocolRequest(bufferAsyncResult);
			this.ProcessWrite(buffer, offset, count, asyncRequest);
			return bufferAsyncResult;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000BCF04 File Offset: 0x000BBF04
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._NegoState.CheckThrow(true);
			if (!this._NegoState.CanGetSecureStream)
			{
				base.InnerStream.EndWrite(asyncResult);
				return;
			}
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
			if (Interlocked.Exchange(ref this._NestedWrite, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndWrite"
				}));
			}
			bufferAsyncResult.InternalWaitForCompletion();
			if (!(bufferAsyncResult.Result is Exception))
			{
				return;
			}
			if (bufferAsyncResult.Result is IOException)
			{
				throw (Exception)bufferAsyncResult.Result;
			}
			throw new IOException(SR.GetString("net_io_write"), (Exception)bufferAsyncResult.Result);
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x000BCFF0 File Offset: 0x000BBFF0
		private void InitializeStreamPart()
		{
			this._ReadHeader = new byte[4];
			this._FrameReader = new FixedSizeReader(base.InnerStream);
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x000BD00F File Offset: 0x000BC00F
		private byte[] InternalBuffer
		{
			get
			{
				return this._InternalBuffer;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x000BD017 File Offset: 0x000BC017
		private int InternalOffset
		{
			get
			{
				return this._InternalOffset;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x000BD01F File Offset: 0x000BC01F
		private int InternalBufferCount
		{
			get
			{
				return this._InternalBufferCount;
			}
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000BD027 File Offset: 0x000BC027
		private void DecrementInternalBufferCount(int decrCount)
		{
			this._InternalOffset += decrCount;
			this._InternalBufferCount -= decrCount;
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000BD045 File Offset: 0x000BC045
		private void EnsureInternalBufferSize(int bytes)
		{
			this._InternalBufferCount = bytes;
			this._InternalOffset = 0;
			if (this.InternalBuffer == null || this.InternalBuffer.Length < bytes)
			{
				this._InternalBuffer = new byte[bytes];
			}
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000BD074 File Offset: 0x000BC074
		private void AdjustInternalBufferOffsetSize(int bytes, int offset)
		{
			this._InternalBufferCount = bytes;
			this._InternalOffset = offset;
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000BD084 File Offset: 0x000BC084
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

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000BD0D8 File Offset: 0x000BC0D8
		private void ProcessWrite(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
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
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_write"), ex);
			}
			catch
			{
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

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000BD1B8 File Offset: 0x000BC1B8
		private void StartWriting(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (count >= 0)
			{
				byte[] buffer2 = null;
				for (;;)
				{
					int num = Math.Min(count, 64512);
					int count2;
					try
					{
						count2 = this._NegoState.EncryptData(buffer, offset, num, ref buffer2);
					}
					catch (Exception innerException)
					{
						throw new IOException(SR.GetString("net_io_encrypt"), innerException);
					}
					catch
					{
						throw new IOException(SR.GetString("net_io_encrypt"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
					if (asyncRequest != null)
					{
						asyncRequest.SetNextRequest(buffer, offset + num, count - num, null);
						IAsyncResult asyncResult = base.InnerStream.BeginWrite(buffer2, 0, count2, NegotiateStream._WriteCallback, asyncRequest);
						if (!asyncResult.CompletedSynchronously)
						{
							break;
						}
						base.InnerStream.EndWrite(asyncResult);
					}
					else
					{
						base.InnerStream.Write(buffer2, 0, count2);
					}
					offset += num;
					count -= num;
					if (count == 0)
					{
						goto IL_BB;
					}
				}
				return;
			}
			IL_BB:
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser();
			}
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000BD2A8 File Offset: 0x000BC2A8
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
				flag = true;
				if (ex is IOException)
				{
					throw;
				}
				throw new IOException(SR.GetString("net_io_read"), ex);
			}
			catch
			{
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

		// Token: 0x06002BCA RID: 11210 RVA: 0x000BD3DC File Offset: 0x000BC3DC
		private int StartReading(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			int result;
			while ((result = this.StartFrameHeader(buffer, offset, count, asyncRequest)) == -1)
			{
			}
			return result;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000BD3FC File Offset: 0x000BC3FC
		private int StartFrameHeader(byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			int readBytes;
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this._ReadHeader, 0, this._ReadHeader.Length, NegotiateStream._ReadCallback);
				this._FrameReader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				readBytes = asyncRequest.Result;
			}
			else
			{
				readBytes = this._FrameReader.ReadPacket(this._ReadHeader, 0, this._ReadHeader.Length);
			}
			return this.StartFrameBody(readBytes, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000BD474 File Offset: 0x000BC474
		private int StartFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				if (asyncRequest != null)
				{
					asyncRequest.CompleteUser(0);
				}
				return 0;
			}
			readBytes = (int)this._ReadHeader[3];
			readBytes = (readBytes << 8 | (int)this._ReadHeader[2]);
			readBytes = (readBytes << 8 | (int)this._ReadHeader[1]);
			readBytes = (readBytes << 8 | (int)this._ReadHeader[0]);
			if (readBytes <= 4 || readBytes > 65536)
			{
				throw new IOException(SR.GetString("net_frame_read_size"));
			}
			this.EnsureInternalBufferSize(readBytes);
			if (asyncRequest != null)
			{
				asyncRequest.SetNextRequest(this.InternalBuffer, 0, readBytes, NegotiateStream._ReadCallback);
				this._FrameReader.AsyncReadPacket(asyncRequest);
				if (!asyncRequest.MustCompleteSynchronously)
				{
					return 0;
				}
				readBytes = asyncRequest.Result;
			}
			else
			{
				readBytes = this._FrameReader.ReadPacket(this.InternalBuffer, 0, readBytes);
			}
			return this.ProcessFrameBody(readBytes, buffer, offset, count, asyncRequest);
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000BD54C File Offset: 0x000BC54C
		private int ProcessFrameBody(int readBytes, byte[] buffer, int offset, int count, AsyncProtocolRequest asyncRequest)
		{
			if (readBytes == 0)
			{
				throw new IOException(SR.GetString("net_io_eof"));
			}
			int offset2;
			readBytes = this._NegoState.DecryptData(this.InternalBuffer, 0, readBytes, out offset2);
			this.AdjustInternalBufferOffsetSize(readBytes, offset2);
			if (readBytes == 0 && count != 0)
			{
				return -1;
			}
			if (readBytes > count)
			{
				readBytes = count;
			}
			Buffer.BlockCopy(this.InternalBuffer, this.InternalOffset, buffer, offset, readBytes);
			this.DecrementInternalBufferCount(readBytes);
			if (asyncRequest != null)
			{
				asyncRequest.CompleteUser(readBytes);
			}
			return readBytes;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x000BD5CC File Offset: 0x000BC5CC
		private static void WriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			AsyncProtocolRequest asyncProtocolRequest = (AsyncProtocolRequest)transportResult.AsyncState;
			try
			{
				NegotiateStream negotiateStream = (NegotiateStream)asyncProtocolRequest.AsyncObject;
				negotiateStream.InnerStream.EndWrite(transportResult);
				if (asyncProtocolRequest.Count == 0)
				{
					asyncProtocolRequest.Count = -1;
				}
				negotiateStream.StartWriting(asyncProtocolRequest.Buffer, asyncProtocolRequest.Offset, asyncProtocolRequest.Count, asyncProtocolRequest);
			}
			catch (Exception e)
			{
				if (asyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				asyncProtocolRequest.CompleteWithError(e);
			}
			catch
			{
				if (asyncProtocolRequest.IsUserCompleted)
				{
					throw;
				}
				asyncProtocolRequest.CompleteWithError(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x000BD684 File Offset: 0x000BC684
		private static void ReadCallback(AsyncProtocolRequest asyncRequest)
		{
			try
			{
				NegotiateStream negotiateStream = (NegotiateStream)asyncRequest.AsyncObject;
				BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)asyncRequest.UserAsyncResult;
				if (asyncRequest.Buffer == negotiateStream._ReadHeader)
				{
					negotiateStream.StartFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
				}
				else if (-1 == negotiateStream.ProcessFrameBody(asyncRequest.Result, bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest))
				{
					negotiateStream.StartReading(bufferAsyncResult.Buffer, bufferAsyncResult.Offset, bufferAsyncResult.Count, asyncRequest);
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

		// Token: 0x040029C7 RID: 10695
		private NegoState _NegoState;

		// Token: 0x040029C8 RID: 10696
		private string _Package;

		// Token: 0x040029C9 RID: 10697
		private IIdentity _RemoteIdentity;

		// Token: 0x040029CA RID: 10698
		private static AsyncCallback _WriteCallback = new AsyncCallback(NegotiateStream.WriteCallback);

		// Token: 0x040029CB RID: 10699
		private static AsyncProtocolCallback _ReadCallback = new AsyncProtocolCallback(NegotiateStream.ReadCallback);

		// Token: 0x040029CC RID: 10700
		private int _NestedWrite;

		// Token: 0x040029CD RID: 10701
		private int _NestedRead;

		// Token: 0x040029CE RID: 10702
		private byte[] _ReadHeader;

		// Token: 0x040029CF RID: 10703
		private byte[] _InternalBuffer;

		// Token: 0x040029D0 RID: 10704
		private int _InternalOffset;

		// Token: 0x040029D1 RID: 10705
		private int _InternalBufferCount;

		// Token: 0x040029D2 RID: 10706
		private FixedSizeReader _FrameReader;
	}
}
