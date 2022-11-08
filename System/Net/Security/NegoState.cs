using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Authentication;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x0200059B RID: 1435
	internal class NegoState
	{
		// Token: 0x06002C2F RID: 11311 RVA: 0x000BDFA0 File Offset: 0x000BCFA0
		internal NegoState(Stream innerStream, bool leaveStreamOpen)
		{
			if (innerStream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._InnerStream = innerStream;
			this._LeaveStreamOpen = leaveStreamOpen;
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06002C30 RID: 11312 RVA: 0x000BDFC4 File Offset: 0x000BCFC4
		internal static string DefaultPackage
		{
			get
			{
				if (!ComNetOS.IsWin9x)
				{
					return "Negotiate";
				}
				return "NTLM";
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000BDFD8 File Offset: 0x000BCFD8
		internal void ValidateCreateContext(string package, NetworkCredential credential, string servicePrincipalName, ExtendedProtectionPolicy policy, ProtectionLevel protectionLevel, TokenImpersonationLevel impersonationLevel)
		{
			if (policy != null)
			{
				if (!AuthenticationManager.OSSupportsExtendedProtection)
				{
					if (policy.PolicyEnforcement == PolicyEnforcement.Always)
					{
						throw new PlatformNotSupportedException(SR.GetString("security_ExtendedProtection_NoOSSupport"));
					}
				}
				else if (policy.CustomChannelBinding == null && policy.CustomServiceNames == null)
				{
					throw new ArgumentException(SR.GetString("net_auth_must_specify_extended_protection_scheme"), "policy");
				}
				this._ExtendedProtectionPolicy = policy;
			}
			else
			{
				this._ExtendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
			}
			this.ValidateCreateContext(package, true, credential, servicePrincipalName, this._ExtendedProtectionPolicy.CustomChannelBinding, protectionLevel, impersonationLevel);
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x000BE060 File Offset: 0x000BD060
		internal void ValidateCreateContext(string package, bool isServer, NetworkCredential credential, string servicePrincipalName, ChannelBinding channelBinding, ProtectionLevel protectionLevel, TokenImpersonationLevel impersonationLevel)
		{
			if (this._Exception != null && !this._CanRetryAuthentication)
			{
				throw this._Exception;
			}
			if (this._Context != null && this._Context.IsValidContext)
			{
				throw new InvalidOperationException(SR.GetString("net_auth_reauth"));
			}
			if (credential == null)
			{
				throw new ArgumentNullException("credential");
			}
			if (servicePrincipalName == null)
			{
				throw new ArgumentNullException("servicePrincipalName");
			}
			if (ComNetOS.IsWin9x && protectionLevel != ProtectionLevel.None)
			{
				throw new NotSupportedException(SR.GetString("net_auth_no_protection_on_win9x"));
			}
			if (impersonationLevel != TokenImpersonationLevel.Identification && impersonationLevel != TokenImpersonationLevel.Impersonation && impersonationLevel != TokenImpersonationLevel.Delegation)
			{
				throw new ArgumentOutOfRangeException("impersonationLevel", impersonationLevel.ToString(), SR.GetString("net_auth_supported_impl_levels"));
			}
			if (this._Context != null && this.IsServer != isServer)
			{
				throw new InvalidOperationException(SR.GetString("net_auth_client_server"));
			}
			this._Exception = null;
			this._RemoteOk = false;
			this._Framer = new StreamFramer(this._InnerStream);
			this._Framer.WriteHeader.MessageId = 22;
			this._ExpectedProtectionLevel = protectionLevel;
			this._ExpectedImpersonationLevel = (isServer ? impersonationLevel : TokenImpersonationLevel.None);
			this._WriteSequenceNumber = 0U;
			this._ReadSequenceNumber = 0U;
			ContextFlags contextFlags = ContextFlags.Connection;
			if (protectionLevel == ProtectionLevel.None && !isServer)
			{
				package = "NTLM";
			}
			else if (protectionLevel == ProtectionLevel.EncryptAndSign)
			{
				contextFlags |= ContextFlags.Confidentiality;
			}
			else if (protectionLevel == ProtectionLevel.Sign)
			{
				contextFlags |= (ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.AcceptStream);
			}
			if (isServer)
			{
				if (this._ExtendedProtectionPolicy.PolicyEnforcement == PolicyEnforcement.WhenSupported)
				{
					contextFlags |= ContextFlags.AllowMissingBindings;
				}
				if (this._ExtendedProtectionPolicy.PolicyEnforcement != PolicyEnforcement.Never && this._ExtendedProtectionPolicy.ProtectionScenario == ProtectionScenario.TrustedProxy)
				{
					contextFlags |= ContextFlags.ProxyBindings;
				}
			}
			else
			{
				if (protectionLevel != ProtectionLevel.None)
				{
					contextFlags |= ContextFlags.MutualAuth;
				}
				if (impersonationLevel == TokenImpersonationLevel.Identification)
				{
					contextFlags |= ContextFlags.AcceptIntegrity;
				}
				if (impersonationLevel == TokenImpersonationLevel.Delegation)
				{
					contextFlags |= ContextFlags.Delegate;
				}
			}
			this._CanRetryAuthentication = false;
			if (!(credential is SystemNetworkCredential))
			{
				ExceptionHelper.ControlPrincipalPermission.Demand();
			}
			try
			{
				this._Context = new NTAuthentication(isServer, package, credential, servicePrincipalName, contextFlags, channelBinding);
			}
			catch (Win32Exception innerException)
			{
				throw new AuthenticationException(SR.GetString("net_auth_SSPI"), innerException);
			}
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000BE264 File Offset: 0x000BD264
		private Exception SetException(Exception e)
		{
			if (this._Exception == null || !(this._Exception is ObjectDisposedException))
			{
				this._Exception = e;
			}
			if (this._Exception != null && this._Context != null)
			{
				this._Context.CloseContext();
			}
			return this._Exception;
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06002C34 RID: 11316 RVA: 0x000BE2A3 File Offset: 0x000BD2A3
		internal bool IsAuthenticated
		{
			get
			{
				return this._Context != null && this.HandshakeComplete && this._Exception == null && this._RemoteOk;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06002C35 RID: 11317 RVA: 0x000BE2C5 File Offset: 0x000BD2C5
		internal bool IsMutuallyAuthenticated
		{
			get
			{
				return this.IsAuthenticated && !ComNetOS.IsWin9x && !this._Context.IsNTLM && this._Context.IsMutualAuthFlag;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06002C36 RID: 11318 RVA: 0x000BE2F4 File Offset: 0x000BD2F4
		internal bool IsEncrypted
		{
			get
			{
				return this.IsAuthenticated && this._Context.IsConfidentialityFlag;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06002C37 RID: 11319 RVA: 0x000BE30B File Offset: 0x000BD30B
		internal bool IsSigned
		{
			get
			{
				return this.IsAuthenticated && (this._Context.IsIntegrityFlag || this._Context.IsConfidentialityFlag);
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06002C38 RID: 11320 RVA: 0x000BE331 File Offset: 0x000BD331
		internal bool IsServer
		{
			get
			{
				return this._Context != null && this._Context.IsServer;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06002C39 RID: 11321 RVA: 0x000BE348 File Offset: 0x000BD348
		internal bool CanGetSecureStream
		{
			get
			{
				return this._Context.IsConfidentialityFlag || this._Context.IsIntegrityFlag;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x000BE364 File Offset: 0x000BD364
		internal TokenImpersonationLevel AllowedImpersonation
		{
			get
			{
				this.CheckThrow(true);
				return this.PrivateImpersonationLevel;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000BE374 File Offset: 0x000BD374
		private TokenImpersonationLevel PrivateImpersonationLevel
		{
			get
			{
				if (this._Context.IsDelegationFlag && this._Context.ProtocolName != "NTLM")
				{
					return TokenImpersonationLevel.Delegation;
				}
				if (this._Context.IsIdentifyFlag)
				{
					return TokenImpersonationLevel.Identification;
				}
				if (!ComNetOS.IsWin9x || !this._Context.IsServer)
				{
					return TokenImpersonationLevel.Impersonation;
				}
				return TokenImpersonationLevel.Identification;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06002C3C RID: 11324 RVA: 0x000BE3CD File Offset: 0x000BD3CD
		private bool HandshakeComplete
		{
			get
			{
				return this._Context.IsCompleted && this._Context.IsValidContext;
			}
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000BE3EC File Offset: 0x000BD3EC
		internal IIdentity GetIdentity()
		{
			this.CheckThrow(true);
			string name = this._Context.IsServer ? this._Context.AssociatedName : this._Context.Spn;
			string type = "NTLM";
			if (!ComNetOS.IsWin9x)
			{
				type = this._Context.ProtocolName;
			}
			if (this._Context.IsServer && !ComNetOS.IsWin9x)
			{
				SafeCloseHandle safeCloseHandle = null;
				try
				{
					safeCloseHandle = this._Context.GetContextToken();
					string protocolName = this._Context.ProtocolName;
					return new WindowsIdentity(safeCloseHandle.DangerousGetHandle(), protocolName, WindowsAccountType.Normal, true);
				}
				catch (SecurityException)
				{
				}
				finally
				{
					if (safeCloseHandle != null)
					{
						safeCloseHandle.Close();
					}
				}
			}
			return new GenericIdentity(name, type);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x000BE4BC File Offset: 0x000BD4BC
		internal void CheckThrow(bool authSucessCheck)
		{
			if (this._Exception != null)
			{
				throw this._Exception;
			}
			if (authSucessCheck && !this.IsAuthenticated)
			{
				throw new InvalidOperationException(SR.GetString("net_auth_noauth"));
			}
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x000BE4E8 File Offset: 0x000BD4E8
		internal void Close()
		{
			this._Exception = new ObjectDisposedException("NegotiateStream");
			if (this._Context != null)
			{
				this._Context.CloseContext();
			}
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x000BE510 File Offset: 0x000BD510
		internal void ProcessAuthentication(LazyAsyncResult lazyResult)
		{
			this.CheckThrow(false);
			if (Interlocked.Exchange(ref this._NestedAuth, 1) == 1)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					(lazyResult == null) ? "BeginAuthenticate" : "Authenticate",
					"authenticate"
				}));
			}
			try
			{
				if (this._Context.IsServer)
				{
					this.StartReceiveBlob(lazyResult);
				}
				else
				{
					this.StartSendBlob(null, lazyResult);
				}
			}
			catch (Exception ex)
			{
				ex = this.SetException(ex);
				throw ex;
			}
			catch
			{
				Exception ex2 = this.SetException(new Exception(SR.GetString("net_nonClsCompliantException")));
				throw ex2;
			}
			finally
			{
				if (lazyResult == null || this._Exception != null)
				{
					this._NestedAuth = 0;
				}
			}
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000BE5E8 File Offset: 0x000BD5E8
		internal void EndProcessAuthentication(IAsyncResult result)
		{
			if (result == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = result as LazyAsyncResult;
			if (lazyAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[]
				{
					result.GetType().FullName
				}), "asyncResult");
			}
			if (Interlocked.Exchange(ref this._NestedAuth, 0) == 0)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndAuthenticate"
				}));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			Exception ex = lazyAsyncResult.Result as Exception;
			if (ex != null)
			{
				ex = this.SetException(ex);
				throw ex;
			}
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000BE688 File Offset: 0x000BD688
		private bool CheckSpn()
		{
			if (this._Context.IsKerberos)
			{
				return true;
			}
			if (this._ExtendedProtectionPolicy.PolicyEnforcement == PolicyEnforcement.Never || this._ExtendedProtectionPolicy.CustomServiceNames == null)
			{
				return true;
			}
			if (!AuthenticationManager.OSSupportsExtendedProtection)
			{
				return true;
			}
			string clientSpecifiedSpn = this._Context.ClientSpecifiedSpn;
			if (!string.IsNullOrEmpty(clientSpecifiedSpn))
			{
				foreach (object obj in this._ExtendedProtectionPolicy.CustomServiceNames)
				{
					string strB = (string)obj;
					if (string.Compare(clientSpecifiedSpn, strB, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
				}
				return false;
			}
			if (this._ExtendedProtectionPolicy.PolicyEnforcement == PolicyEnforcement.WhenSupported)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x000BE74C File Offset: 0x000BD74C
		private void StartSendBlob(byte[] message, LazyAsyncResult lazyResult)
		{
			Win32Exception ex = null;
			if (message != NegoState._EmptyMessage)
			{
				message = this.GetOutgoingBlob(message, ref ex);
			}
			if (ex != null)
			{
				this.StartSendAuthResetSignal(lazyResult, message, ex);
				return;
			}
			if (this.HandshakeComplete)
			{
				if (this._Context.IsServer && !this.CheckSpn())
				{
					Exception exception = new AuthenticationException(SR.GetString("net_auth_bad_client_creds_or_target_mismatch"));
					int num = 1790;
					message = new byte[8];
					for (int i = message.Length - 1; i >= 0; i--)
					{
						message[i] = (byte)(num & 255);
						num = (int)((uint)num >> 8);
					}
					this.StartSendAuthResetSignal(lazyResult, message, exception);
					return;
				}
				if (this.PrivateImpersonationLevel < this._ExpectedImpersonationLevel)
				{
					Exception exception2 = new AuthenticationException(SR.GetString("net_auth_context_expectation", new object[]
					{
						this._ExpectedImpersonationLevel.ToString(),
						this.PrivateImpersonationLevel.ToString()
					}));
					int num2 = 1790;
					message = new byte[8];
					for (int j = message.Length - 1; j >= 0; j--)
					{
						message[j] = (byte)(num2 & 255);
						num2 = (int)((uint)num2 >> 8);
					}
					this.StartSendAuthResetSignal(lazyResult, message, exception2);
					return;
				}
				ProtectionLevel protectionLevel = this._Context.IsConfidentialityFlag ? ProtectionLevel.EncryptAndSign : (this._Context.IsIntegrityFlag ? ProtectionLevel.Sign : ProtectionLevel.None);
				if (protectionLevel < this._ExpectedProtectionLevel)
				{
					Exception exception3 = new AuthenticationException(SR.GetString("net_auth_context_expectation", new object[]
					{
						protectionLevel.ToString(),
						this._ExpectedProtectionLevel.ToString()
					}));
					int num3 = 1790;
					message = new byte[8];
					for (int k = message.Length - 1; k >= 0; k--)
					{
						message[k] = (byte)(num3 & 255);
						num3 = (int)((uint)num3 >> 8);
					}
					this.StartSendAuthResetSignal(lazyResult, message, exception3);
					return;
				}
				this._Framer.WriteHeader.MessageId = 20;
				if (this._Context.IsServer)
				{
					this._RemoteOk = true;
					if (message == null)
					{
						message = NegoState._EmptyMessage;
					}
				}
			}
			else if (message == null || message == NegoState._EmptyMessage)
			{
				throw new InternalException();
			}
			if (message != null)
			{
				if (lazyResult == null)
				{
					this._Framer.WriteMessage(message);
				}
				else
				{
					IAsyncResult asyncResult = this._Framer.BeginWriteMessage(message, NegoState._WriteCallback, lazyResult);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					this._Framer.EndWriteMessage(asyncResult);
				}
			}
			this.CheckCompletionBeforeNextReceive(lazyResult);
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x000BE9B0 File Offset: 0x000BD9B0
		private void CheckCompletionBeforeNextReceive(LazyAsyncResult lazyResult)
		{
			if (this.HandshakeComplete && this._RemoteOk)
			{
				if (lazyResult != null)
				{
					lazyResult.InvokeCallback();
				}
				return;
			}
			this.StartReceiveBlob(lazyResult);
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000BE9D4 File Offset: 0x000BD9D4
		private void StartReceiveBlob(LazyAsyncResult lazyResult)
		{
			byte[] message;
			if (lazyResult == null)
			{
				message = this._Framer.ReadMessage();
			}
			else
			{
				IAsyncResult asyncResult = this._Framer.BeginReadMessage(NegoState._ReadCallback, lazyResult);
				if (!asyncResult.CompletedSynchronously)
				{
					return;
				}
				message = this._Framer.EndReadMessage(asyncResult);
			}
			this.ProcessReceivedBlob(message, lazyResult);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000BEA24 File Offset: 0x000BDA24
		private void ProcessReceivedBlob(byte[] message, LazyAsyncResult lazyResult)
		{
			if (message == null)
			{
				throw new AuthenticationException(SR.GetString("net_auth_eof"), null);
			}
			if (this._Framer.ReadHeader.MessageId == 21)
			{
				Win32Exception ex = null;
				if (message.Length >= 8)
				{
					long num = 0L;
					for (int i = 0; i < 8; i++)
					{
						num = (num << 8) + (long)((ulong)message[i]);
					}
					ex = new Win32Exception((int)num);
				}
				if (ex != null)
				{
					if (ex.NativeErrorCode == -2146893044)
					{
						throw new InvalidCredentialException(SR.GetString("net_auth_bad_client_creds"), ex);
					}
					if (ex.NativeErrorCode == 1790)
					{
						throw new AuthenticationException(SR.GetString("net_auth_context_expectation_remote"), ex);
					}
				}
				throw new AuthenticationException(SR.GetString("net_auth_alert"), ex);
			}
			if (this._Framer.ReadHeader.MessageId == 20)
			{
				this._RemoteOk = true;
			}
			else if (this._Framer.ReadHeader.MessageId != 22)
			{
				throw new AuthenticationException(SR.GetString("net_io_header_id", new object[]
				{
					"MessageId",
					this._Framer.ReadHeader.MessageId,
					22
				}), null);
			}
			this.CheckCompletionBeforeNextSend(message, lazyResult);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000BEB50 File Offset: 0x000BDB50
		private void CheckCompletionBeforeNextSend(byte[] message, LazyAsyncResult lazyResult)
		{
			if (!this.HandshakeComplete)
			{
				this.StartSendBlob(message, lazyResult);
				return;
			}
			if (!this._RemoteOk)
			{
				throw new AuthenticationException(SR.GetString("net_io_header_id", new object[]
				{
					"MessageId",
					this._Framer.ReadHeader.MessageId,
					20
				}), null);
			}
			if (lazyResult != null)
			{
				lazyResult.InvokeCallback();
			}
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000BEBC4 File Offset: 0x000BDBC4
		private void StartSendAuthResetSignal(LazyAsyncResult lazyResult, byte[] message, Exception exception)
		{
			this._Framer.WriteHeader.MessageId = 21;
			Win32Exception ex = exception as Win32Exception;
			if (ex != null && ex.NativeErrorCode == -2146893044)
			{
				if (this.IsServer)
				{
					exception = new InvalidCredentialException(SR.GetString("net_auth_bad_client_creds"), exception);
				}
				else
				{
					exception = new InvalidCredentialException(SR.GetString("net_auth_bad_client_creds_or_target_mismatch"), exception);
				}
			}
			if (!(exception is AuthenticationException))
			{
				exception = new AuthenticationException(SR.GetString("net_auth_SSPI"), exception);
			}
			if (lazyResult == null)
			{
				this._Framer.WriteMessage(message);
			}
			else
			{
				lazyResult.Result = exception;
				IAsyncResult asyncResult = this._Framer.BeginWriteMessage(message, NegoState._WriteCallback, lazyResult);
				if (!asyncResult.CompletedSynchronously)
				{
					return;
				}
				this._Framer.EndWriteMessage(asyncResult);
			}
			this._CanRetryAuthentication = true;
			throw exception;
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000BEC8C File Offset: 0x000BDC8C
		private static void WriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)transportResult.AsyncState;
			try
			{
				NegoState negoState = (NegoState)lazyAsyncResult.AsyncObject;
				negoState._Framer.EndWriteMessage(transportResult);
				if (lazyAsyncResult.Result is Exception)
				{
					negoState._CanRetryAuthentication = true;
					throw (Exception)lazyAsyncResult.Result;
				}
				negoState.CheckCompletionBeforeNextReceive(lazyAsyncResult);
			}
			catch (Exception result)
			{
				if (lazyAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				lazyAsyncResult.InvokeCallback(result);
			}
			catch
			{
				if (lazyAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				lazyAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000BED44 File Offset: 0x000BDD44
		private static void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)transportResult.AsyncState;
			try
			{
				NegoState negoState = (NegoState)lazyAsyncResult.AsyncObject;
				byte[] message = negoState._Framer.EndReadMessage(transportResult);
				negoState.ProcessReceivedBlob(message, lazyAsyncResult);
			}
			catch (Exception result)
			{
				if (lazyAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				lazyAsyncResult.InvokeCallback(result);
			}
			catch
			{
				if (lazyAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				lazyAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000BEDDC File Offset: 0x000BDDDC
		private byte[] GetOutgoingBlob(byte[] incomingBlob, ref Win32Exception e)
		{
			SecurityStatus securityStatus;
			byte[] array = this._Context.GetOutgoingBlob(incomingBlob, false, out securityStatus);
			if ((securityStatus & (SecurityStatus)(-2147483648)) != SecurityStatus.OK)
			{
				e = new Win32Exception((int)securityStatus);
				array = new byte[8];
				for (int i = array.Length - 1; i >= 0; i--)
				{
					array[i] = (byte)(securityStatus & (SecurityStatus)255);
					securityStatus >>= 8;
				}
			}
			if (array != null && array.Length == 0)
			{
				array = NegoState._EmptyMessage;
			}
			return array;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x000BEE3F File Offset: 0x000BDE3F
		internal int EncryptData(byte[] buffer, int offset, int count, ref byte[] outBuffer)
		{
			this.CheckThrow(true);
			this._WriteSequenceNumber += 1U;
			return this._Context.Encrypt(buffer, offset, count, ref outBuffer, this._WriteSequenceNumber);
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000BEE6C File Offset: 0x000BDE6C
		internal int DecryptData(byte[] buffer, int offset, int count, out int newOffset)
		{
			this.CheckThrow(true);
			this._ReadSequenceNumber += 1U;
			return this._Context.Decrypt(buffer, offset, count, out newOffset, this._ReadSequenceNumber);
		}

		// Token: 0x04002A0B RID: 10763
		private const int ERROR_TRUST_FAILURE = 1790;

		// Token: 0x04002A0C RID: 10764
		internal const int c_MaxReadFrameSize = 65536;

		// Token: 0x04002A0D RID: 10765
		internal const int c_MaxWriteDataSize = 64512;

		// Token: 0x04002A0E RID: 10766
		private static readonly byte[] _EmptyMessage = new byte[0];

		// Token: 0x04002A0F RID: 10767
		private static readonly AsyncCallback _ReadCallback = new AsyncCallback(NegoState.ReadCallback);

		// Token: 0x04002A10 RID: 10768
		private static readonly AsyncCallback _WriteCallback = new AsyncCallback(NegoState.WriteCallback);

		// Token: 0x04002A11 RID: 10769
		private Stream _InnerStream;

		// Token: 0x04002A12 RID: 10770
		private bool _LeaveStreamOpen;

		// Token: 0x04002A13 RID: 10771
		private Exception _Exception;

		// Token: 0x04002A14 RID: 10772
		private StreamFramer _Framer;

		// Token: 0x04002A15 RID: 10773
		private NTAuthentication _Context;

		// Token: 0x04002A16 RID: 10774
		private int _NestedAuth;

		// Token: 0x04002A17 RID: 10775
		private bool _CanRetryAuthentication;

		// Token: 0x04002A18 RID: 10776
		private ProtectionLevel _ExpectedProtectionLevel;

		// Token: 0x04002A19 RID: 10777
		private TokenImpersonationLevel _ExpectedImpersonationLevel;

		// Token: 0x04002A1A RID: 10778
		private uint _WriteSequenceNumber;

		// Token: 0x04002A1B RID: 10779
		private uint _ReadSequenceNumber;

		// Token: 0x04002A1C RID: 10780
		private ExtendedProtectionPolicy _ExtendedProtectionPolicy;

		// Token: 0x04002A1D RID: 10781
		private bool _RemoteOk;
	}
}
