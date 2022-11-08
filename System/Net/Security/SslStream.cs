using System;
using System.IO;
using System.Security.Authentication;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Net.Security
{
	// Token: 0x02000596 RID: 1430
	public class SslStream : AuthenticatedStream
	{
		// Token: 0x06002BE1 RID: 11233 RVA: 0x000BD790 File Offset: 0x000BC790
		public SslStream(Stream innerStream) : this(innerStream, false, null, null)
		{
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000BD79C File Offset: 0x000BC79C
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen) : this(innerStream, leaveInnerStreamOpen, null, null)
		{
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000BD7A8 File Offset: 0x000BC7A8
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback) : this(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, null)
		{
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000BD7B4 File Offset: 0x000BC7B4
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback) : base(innerStream, leaveInnerStreamOpen)
		{
			this._userCertificateValidationCallback = userCertificateValidationCallback;
			this._userCertificateSelectionCallback = userCertificateSelectionCallback;
			RemoteCertValidationCallback certValidationCallback = new RemoteCertValidationCallback(this.userCertValidationCallbackWrapper);
			LocalCertSelectionCallback certSelectionCallback = (userCertificateSelectionCallback == null) ? null : new LocalCertSelectionCallback(this.userCertSelectionCallbackWrapper);
			this._SslState = new SslState(innerStream, certValidationCallback, certSelectionCallback);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000BD808 File Offset: 0x000BC808
		private bool userCertValidationCallbackWrapper(string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			this.m_RemoteCertificateOrBytes = ((certificate == null) ? null : certificate.GetRawCertData());
			if (this._userCertificateValidationCallback == null)
			{
				if (!this._SslState.RemoteCertRequired)
				{
					sslPolicyErrors &= ~SslPolicyErrors.RemoteCertificateNotAvailable;
				}
				return sslPolicyErrors == SslPolicyErrors.None;
			}
			return this._userCertificateValidationCallback(this, certificate, chain, sslPolicyErrors);
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000BD859 File Offset: 0x000BC859
		private X509Certificate userCertSelectionCallbackWrapper(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
		{
			return this._userCertificateSelectionCallback(this, targetHost, localCertificates, remoteCertificate, acceptableIssuers);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000BD86C File Offset: 0x000BC86C
		public virtual void AuthenticateAsClient(string targetHost)
		{
			this.AuthenticateAsClient(targetHost, new X509CertificateCollection(), ServicePointManager.DefaultSslProtocols, false);
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000BD880 File Offset: 0x000BC880
		public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			this._SslState.ValidateCreateContext(false, targetHost, enabledSslProtocols, null, clientCertificates, true, checkCertificateRevocation);
			this._SslState.ProcessAuthentication(null);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000BD8A1 File Offset: 0x000BC8A1
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(targetHost, new X509CertificateCollection(), ServicePointManager.DefaultSslProtocols, false, asyncCallback, asyncState);
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000BD8B8 File Offset: 0x000BC8B8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			this._SslState.ValidateCreateContext(false, targetHost, enabledSslProtocols, null, clientCertificates, true, checkCertificateRevocation);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._SslState, asyncState, asyncCallback);
			this._SslState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000BD8F5 File Offset: 0x000BC8F5
		public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
		{
			this._SslState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000BD903 File Offset: 0x000BC903
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate)
		{
			this.AuthenticateAsServer(serverCertificate, false, ServicePointManager.DefaultSslProtocols, false);
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000BD913 File Offset: 0x000BC913
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
		{
			if (!ComNetOS.IsWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
			this._SslState.ValidateCreateContext(true, string.Empty, enabledSslProtocols, serverCertificate, null, clientCertificateRequired, checkCertificateRevocation);
			this._SslState.ProcessAuthentication(null);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000BD94F File Offset: 0x000BC94F
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsServer(serverCertificate, false, ServicePointManager.DefaultSslProtocols, false, asyncCallback, asyncState);
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000BD964 File Offset: 0x000BC964
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			if (!ComNetOS.IsWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
			}
			this._SslState.ValidateCreateContext(true, string.Empty, enabledSslProtocols, serverCertificate, null, clientCertificateRequired, checkCertificateRevocation);
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this._SslState, asyncState, asyncCallback);
			this._SslState.ProcessAuthentication(lazyAsyncResult);
			return lazyAsyncResult;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000BD9BC File Offset: 0x000BC9BC
		public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
		{
			this._SslState.EndProcessAuthentication(asyncResult);
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x000BD9CA File Offset: 0x000BC9CA
		public TransportContext TransportContext
		{
			get
			{
				return new SslStreamContext(this);
			}
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000BD9D2 File Offset: 0x000BC9D2
		internal ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this._SslState.GetChannelBinding(kind);
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000BD9E0 File Offset: 0x000BC9E0
		public override bool IsAuthenticated
		{
			get
			{
				return this._SslState.IsAuthenticated;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x000BD9ED File Offset: 0x000BC9ED
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				return this._SslState.IsMutuallyAuthenticated;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x000BD9FA File Offset: 0x000BC9FA
		public override bool IsEncrypted
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06002BF6 RID: 11254 RVA: 0x000BDA02 File Offset: 0x000BCA02
		public override bool IsSigned
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x000BDA0A File Offset: 0x000BCA0A
		public override bool IsServer
		{
			get
			{
				return this._SslState.IsServer;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x000BDA17 File Offset: 0x000BCA17
		public virtual SslProtocols SslProtocol
		{
			get
			{
				return this._SslState.SslProtocol;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x000BDA24 File Offset: 0x000BCA24
		public virtual bool CheckCertRevocationStatus
		{
			get
			{
				return this._SslState.CheckCertRevocationStatus;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06002BFA RID: 11258 RVA: 0x000BDA31 File Offset: 0x000BCA31
		public virtual X509Certificate LocalCertificate
		{
			get
			{
				return this._SslState.LocalCertificate;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06002BFB RID: 11259 RVA: 0x000BDA40 File Offset: 0x000BCA40
		public virtual X509Certificate RemoteCertificate
		{
			get
			{
				this._SslState.CheckThrow(true);
				object remoteCertificateOrBytes = this.m_RemoteCertificateOrBytes;
				if (remoteCertificateOrBytes != null && remoteCertificateOrBytes.GetType() == typeof(byte[]))
				{
					return (X509Certificate)(this.m_RemoteCertificateOrBytes = new X509Certificate((byte[])remoteCertificateOrBytes));
				}
				return remoteCertificateOrBytes as X509Certificate;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06002BFC RID: 11260 RVA: 0x000BDA95 File Offset: 0x000BCA95
		public virtual CipherAlgorithmType CipherAlgorithm
		{
			get
			{
				return this._SslState.CipherAlgorithm;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06002BFD RID: 11261 RVA: 0x000BDAA2 File Offset: 0x000BCAA2
		public virtual int CipherStrength
		{
			get
			{
				return this._SslState.CipherStrength;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06002BFE RID: 11262 RVA: 0x000BDAAF File Offset: 0x000BCAAF
		public virtual HashAlgorithmType HashAlgorithm
		{
			get
			{
				return this._SslState.HashAlgorithm;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06002BFF RID: 11263 RVA: 0x000BDABC File Offset: 0x000BCABC
		public virtual int HashStrength
		{
			get
			{
				return this._SslState.HashStrength;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06002C00 RID: 11264 RVA: 0x000BDAC9 File Offset: 0x000BCAC9
		public virtual ExchangeAlgorithmType KeyExchangeAlgorithm
		{
			get
			{
				return this._SslState.KeyExchangeAlgorithm;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06002C01 RID: 11265 RVA: 0x000BDAD6 File Offset: 0x000BCAD6
		public virtual int KeyExchangeStrength
		{
			get
			{
				return this._SslState.KeyExchangeStrength;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x000BDAE3 File Offset: 0x000BCAE3
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06002C03 RID: 11267 RVA: 0x000BDAE6 File Offset: 0x000BCAE6
		public override bool CanRead
		{
			get
			{
				return this._SslState.IsAuthenticated && base.InnerStream.CanRead;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06002C04 RID: 11268 RVA: 0x000BDB02 File Offset: 0x000BCB02
		public override bool CanTimeout
		{
			get
			{
				return base.InnerStream.CanTimeout;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06002C05 RID: 11269 RVA: 0x000BDB0F File Offset: 0x000BCB0F
		public override bool CanWrite
		{
			get
			{
				return this._SslState.IsAuthenticated && base.InnerStream.CanWrite;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000BDB2B File Offset: 0x000BCB2B
		// (set) Token: 0x06002C07 RID: 11271 RVA: 0x000BDB38 File Offset: 0x000BCB38
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

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000BDB46 File Offset: 0x000BCB46
		// (set) Token: 0x06002C09 RID: 11273 RVA: 0x000BDB53 File Offset: 0x000BCB53
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

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06002C0A RID: 11274 RVA: 0x000BDB61 File Offset: 0x000BCB61
		public override long Length
		{
			get
			{
				return base.InnerStream.Length;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000BDB6E File Offset: 0x000BCB6E
		// (set) Token: 0x06002C0C RID: 11276 RVA: 0x000BDB7B File Offset: 0x000BCB7B
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

		// Token: 0x06002C0D RID: 11277 RVA: 0x000BDB8C File Offset: 0x000BCB8C
		public override void SetLength(long value)
		{
			base.InnerStream.SetLength(value);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000BDB9A File Offset: 0x000BCB9A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000BDBAB File Offset: 0x000BCBAB
		public override void Flush()
		{
			this._SslState.Flush();
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000BDBB8 File Offset: 0x000BCBB8
		protected override void Dispose(bool disposing)
		{
			try
			{
				this._SslState.Close();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000BDBEC File Offset: 0x000BCBEC
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._SslState.SecureStream.Read(buffer, offset, count);
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000BDC01 File Offset: 0x000BCC01
		public void Write(byte[] buffer)
		{
			this._SslState.SecureStream.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000BDC18 File Offset: 0x000BCC18
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._SslState.SecureStream.Write(buffer, offset, count);
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000BDC2D File Offset: 0x000BCC2D
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			return this._SslState.SecureStream.BeginRead(buffer, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000BDC46 File Offset: 0x000BCC46
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._SslState.SecureStream.EndRead(asyncResult);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000BDC59 File Offset: 0x000BCC59
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			return this._SslState.SecureStream.BeginWrite(buffer, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000BDC72 File Offset: 0x000BCC72
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._SslState.SecureStream.EndWrite(asyncResult);
		}

		// Token: 0x040029F3 RID: 10739
		private SslState _SslState;

		// Token: 0x040029F4 RID: 10740
		private RemoteCertificateValidationCallback _userCertificateValidationCallback;

		// Token: 0x040029F5 RID: 10741
		private LocalCertificateSelectionCallback _userCertificateSelectionCallback;

		// Token: 0x040029F6 RID: 10742
		private object m_RemoteCertificateOrBytes;
	}
}
