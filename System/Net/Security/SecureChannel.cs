using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Net.Security
{
	// Token: 0x02000532 RID: 1330
	internal class SecureChannel
	{
		// Token: 0x060028A5 RID: 10405 RVA: 0x000A8024 File Offset: 0x000A7024
		internal SecureChannel(string hostname, bool serverMode, SchProtocols protocolFlags, X509Certificate serverCertificate, X509CertificateCollection clientCertificates, bool remoteCertRequired, bool checkCertName, bool checkCertRevocationStatus, LocalCertSelectionCallback certSelectionDelegate)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, ".ctor", "hostname=" + hostname + ", #clientCertificates=" + ((clientCertificates == null) ? "0" : clientCertificates.Count.ToString(NumberFormatInfo.InvariantInfo)));
			}
			SSPIWrapper.GetVerifyPackageInfo(GlobalSSPI.SSPISecureChannel, "Microsoft Unified Security Protocol Provider", true);
			if (ComNetOS.IsWin9x && clientCertificates.Count > 0)
			{
				this.m_Destination = hostname + "+" + clientCertificates.GetHashCode();
			}
			else
			{
				this.m_Destination = hostname;
			}
			this.m_HostName = hostname;
			this.m_ServerMode = serverMode;
			if (serverMode)
			{
				this.m_ProtocolFlags = (protocolFlags & SchProtocols.ServerMask);
			}
			else
			{
				this.m_ProtocolFlags = (protocolFlags & SchProtocols.ClientMask);
			}
			this.m_ServerCertificate = serverCertificate;
			this.m_ClientCertificates = clientCertificates;
			this.m_RemoteCertRequired = remoteCertRequired;
			this.m_SecurityContext = null;
			this.m_CheckCertRevocation = checkCertRevocationStatus;
			this.m_CheckCertName = checkCertName;
			this.m_CertSelectionDelegate = certSelectionDelegate;
			this.m_RefreshCredentialNeeded = true;
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060028A6 RID: 10406 RVA: 0x000A8174 File Offset: 0x000A7174
		internal X509Certificate LocalServerCertificate
		{
			get
			{
				return this.m_ServerCertificate;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000A817C File Offset: 0x000A717C
		internal X509Certificate LocalClientCertificate
		{
			get
			{
				return this.m_SelectedClientCertificate;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x000A8184 File Offset: 0x000A7184
		internal bool IsRemoteCertificateAvailable
		{
			get
			{
				return this.m_IsRemoteCertificateAvailable;
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000A818C File Offset: 0x000A718C
		internal X509Certificate2 GetRemoteCertificate(out X509Certificate2Collection remoteCertificateStore)
		{
			remoteCertificateStore = null;
			if (this.m_SecurityContext == null)
			{
				return null;
			}
			X509Certificate2 x509Certificate = null;
			SafeFreeCertContext safeFreeCertContext = null;
			try
			{
				safeFreeCertContext = (SSPIWrapper.QueryContextAttributes(GlobalSSPI.SSPISecureChannel, this.m_SecurityContext, ContextAttribute.RemoteCertificate) as SafeFreeCertContext);
				if (safeFreeCertContext != null && !safeFreeCertContext.IsInvalid)
				{
					x509Certificate = new X509Certificate2(safeFreeCertContext.DangerousGetHandle());
				}
			}
			finally
			{
				if (safeFreeCertContext != null)
				{
					remoteCertificateStore = SecureChannel.UnmanagedCertificateContext.GetStore(safeFreeCertContext);
					safeFreeCertContext.Close();
				}
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, SR.GetString("net_log_remote_certificate", new object[]
				{
					(x509Certificate == null) ? "null" : x509Certificate.ToString(true)
				}));
			}
			return x509Certificate;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000A8238 File Offset: 0x000A7238
		internal ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			ChannelBinding result = null;
			if (this.m_SecurityContext != null)
			{
				result = SSPIWrapper.QueryContextChannelBinding(GlobalSSPI.SSPISecureChannel, this.m_SecurityContext, (ContextAttribute)kind);
			}
			return result;
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x000A8262 File Offset: 0x000A7262
		internal bool CheckCertRevocationStatus
		{
			get
			{
				return this.m_CheckCertRevocation;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000A826A File Offset: 0x000A726A
		internal X509CertificateCollection ClientCertificates
		{
			get
			{
				return this.m_ClientCertificates;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x000A8272 File Offset: 0x000A7272
		internal int HeaderSize
		{
			get
			{
				return this.m_HeaderSize;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x000A827A File Offset: 0x000A727A
		internal int MaxDataSize
		{
			get
			{
				return this.m_MaxDataSize;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x000A8282 File Offset: 0x000A7282
		internal SslConnectionInfo ConnectionInfo
		{
			get
			{
				return this.m_ConnectionInfo;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x060028B0 RID: 10416 RVA: 0x000A828A File Offset: 0x000A728A
		internal bool IsValidContext
		{
			get
			{
				return this.m_SecurityContext != null && !this.m_SecurityContext.IsInvalid;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x060028B1 RID: 10417 RVA: 0x000A82A4 File Offset: 0x000A72A4
		internal bool IsServer
		{
			get
			{
				return this.m_ServerMode;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x000A82AC File Offset: 0x000A72AC
		internal bool RemoteCertRequired
		{
			get
			{
				return this.m_RemoteCertRequired;
			}
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000A82B4 File Offset: 0x000A72B4
		internal void SetRefreshCredentialNeeded()
		{
			this.m_RefreshCredentialNeeded = true;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000A82BD File Offset: 0x000A72BD
		internal void Close()
		{
			if (this.m_SecurityContext != null)
			{
				this.m_SecurityContext.Close();
			}
			if (this.m_CredentialsHandle != null)
			{
				this.m_CredentialsHandle.Close();
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000A82E8 File Offset: 0x000A72E8
		private X509Certificate2 EnsurePrivateKey(X509Certificate certificate)
		{
			if (certificate == null)
			{
				return null;
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_locating_private_key_for_certificate", new object[]
				{
					certificate.ToString(true)
				}));
			}
			try
			{
				X509Certificate2 x509Certificate = certificate as X509Certificate2;
				Type type = certificate.GetType();
				string findValue = null;
				if (type != typeof(X509Certificate2) && type != typeof(X509Certificate))
				{
					if (certificate.Handle != IntPtr.Zero)
					{
						x509Certificate = new X509Certificate2(certificate);
						findValue = x509Certificate.GetCertHashString();
					}
				}
				else
				{
					findValue = certificate.GetCertHashString();
				}
				if (x509Certificate != null)
				{
					if (x509Certificate.HasPrivateKey)
					{
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_cert_is_of_type_2"));
						}
						return x509Certificate;
					}
					if (certificate != x509Certificate)
					{
						x509Certificate.Reset();
					}
				}
				ExceptionHelper.KeyContainerPermissionOpen.Demand();
				X509Store x509Store = SecureChannel.EnsureStoreOpened(this.m_ServerMode);
				if (x509Store != null)
				{
					X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, findValue, false);
					if (x509Certificate2Collection.Count > 0 && x509Certificate2Collection[0].PrivateKey != null)
					{
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_found_cert_in_store", new object[]
							{
								this.m_ServerMode ? "LocalMachine" : "CurrentUser"
							}));
						}
						return x509Certificate2Collection[0];
					}
				}
				x509Store = SecureChannel.EnsureStoreOpened(!this.m_ServerMode);
				if (x509Store != null)
				{
					X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, findValue, false);
					if (x509Certificate2Collection.Count > 0 && x509Certificate2Collection[0].PrivateKey != null)
					{
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_found_cert_in_store", new object[]
							{
								this.m_ServerMode ? "CurrentUser" : "LocalMachine"
							}));
						}
						return x509Certificate2Collection[0];
					}
				}
			}
			catch (CryptographicException)
			{
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_did_not_find_cert_in_store"));
			}
			return null;
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000A850C File Offset: 0x000A750C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal static X509Store EnsureStoreOpened(bool isMachineStore)
		{
			X509Store x509Store = isMachineStore ? SecureChannel.s_MyMachineCertStoreEx : SecureChannel.s_MyCertStoreEx;
			if (x509Store == null)
			{
				lock (SecureChannel.s_SyncObject)
				{
					x509Store = (isMachineStore ? SecureChannel.s_MyMachineCertStoreEx : SecureChannel.s_MyCertStoreEx);
					if (x509Store == null)
					{
						StoreLocation storeLocation = isMachineStore ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
						x509Store = new X509Store(StoreName.My, storeLocation);
						try
						{
							try
							{
								using (WindowsIdentity.Impersonate(IntPtr.Zero))
								{
									x509Store.Open(OpenFlags.OpenExistingOnly);
								}
							}
							catch
							{
								throw;
							}
							if (isMachineStore)
							{
								SecureChannel.s_MyMachineCertStoreEx = x509Store;
							}
							else
							{
								SecureChannel.s_MyCertStoreEx = x509Store;
							}
							return x509Store;
						}
						catch (Exception ex)
						{
							if (ex is CryptographicException || ex is SecurityException)
							{
								return null;
							}
							if (Logging.On)
							{
								Logging.PrintError(Logging.Web, SR.GetString("net_log_open_store_failed", new object[]
								{
									storeLocation,
									ex
								}));
							}
							throw;
						}
					}
				}
				return x509Store;
			}
			return x509Store;
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000A862C File Offset: 0x000A762C
		private static X509Certificate2 MakeEx(X509Certificate certificate)
		{
			if (certificate.GetType() == typeof(X509Certificate2))
			{
				return (X509Certificate2)certificate;
			}
			X509Certificate2 result = null;
			try
			{
				if (certificate.Handle != IntPtr.Zero)
				{
					result = new X509Certificate2(certificate);
				}
			}
			catch (SecurityException)
			{
			}
			catch (CryptographicException)
			{
			}
			return result;
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000A8694 File Offset: 0x000A7694
		private unsafe string[] GetIssuers()
		{
			string[] array = new string[0];
			if (this.IsValidContext)
			{
				IssuerListInfoEx issuerListInfoEx = (IssuerListInfoEx)SSPIWrapper.QueryContextAttributes(GlobalSSPI.SSPISecureChannel, this.m_SecurityContext, ContextAttribute.IssuerListInfoEx);
				try
				{
					if (issuerListInfoEx.cIssuers > 0U)
					{
						uint cIssuers = issuerListInfoEx.cIssuers;
						array = new string[issuerListInfoEx.cIssuers];
						_CERT_CHAIN_ELEMENT* ptr = (_CERT_CHAIN_ELEMENT*)((void*)issuerListInfoEx.aIssuers.DangerousGetHandle());
						int num = 0;
						while ((long)num < (long)((ulong)cIssuers))
						{
							_CERT_CHAIN_ELEMENT* ptr2 = ptr + num;
							uint cbSize = ptr2->cbSize;
							byte* ptr3 = (byte*)((void*)ptr2->pCertContext);
							byte[] array2 = new byte[cbSize];
							int num2 = 0;
							while ((long)num2 < (long)((ulong)cbSize))
							{
								array2[num2] = ptr3[num2];
								num2++;
							}
							X500DistinguishedName x500DistinguishedName = new X500DistinguishedName(array2);
							array[num] = x500DistinguishedName.Name;
							num++;
						}
					}
				}
				finally
				{
					if (issuerListInfoEx.aIssuers != null)
					{
						issuerListInfoEx.aIssuers.Close();
					}
				}
			}
			return array;
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000A879C File Offset: 0x000A779C
		[StorePermission(SecurityAction.Assert, Unrestricted = true)]
		private bool AcquireClientCredentials(ref byte[] thumbPrint)
		{
			X509Certificate x509Certificate = null;
			ArrayList arrayList = new ArrayList();
			string[] array = null;
			bool flag = false;
			if (this.m_CertSelectionDelegate != null)
			{
				if (array == null)
				{
					array = this.GetIssuers();
				}
				X509Certificate2 x509Certificate2 = null;
				try
				{
					X509Certificate2Collection x509Certificate2Collection;
					x509Certificate2 = this.GetRemoteCertificate(out x509Certificate2Collection);
					x509Certificate = this.m_CertSelectionDelegate(this.m_HostName, this.ClientCertificates, x509Certificate2, array);
				}
				finally
				{
					if (x509Certificate2 != null)
					{
						x509Certificate2.Reset();
					}
				}
				if (x509Certificate != null)
				{
					if (this.m_CredentialsHandle == null)
					{
						flag = true;
					}
					arrayList.Add(x509Certificate);
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_got_certificate_from_delegate"));
					}
				}
				else if (this.ClientCertificates.Count == 0)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_no_delegate_and_have_no_client_cert"));
					}
					flag = true;
				}
				else if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_no_delegate_but_have_client_cert"));
				}
			}
			else if (this.m_CredentialsHandle == null && this.m_ClientCertificates != null && this.m_ClientCertificates.Count > 0)
			{
				x509Certificate = this.ClientCertificates[0];
				flag = true;
				if (x509Certificate != null)
				{
					arrayList.Add(x509Certificate);
				}
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_attempting_restart_using_cert", new object[]
					{
						(x509Certificate == null) ? "null" : x509Certificate.ToString(true)
					}));
				}
			}
			else if (this.m_ClientCertificates != null && this.m_ClientCertificates.Count > 0)
			{
				if (array == null)
				{
					array = this.GetIssuers();
				}
				if (Logging.On)
				{
					if (array == null || array.Length == 0)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_no_issuers_try_all_certs"));
					}
					else
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_server_issuers_look_for_matching_certs", new object[]
						{
							array.Length
						}));
					}
				}
				int i = 0;
				while (i < this.m_ClientCertificates.Count)
				{
					if (array != null && array.Length != 0)
					{
						X509Certificate2 x509Certificate3 = null;
						X509Chain x509Chain = null;
						try
						{
							x509Certificate3 = SecureChannel.MakeEx(this.m_ClientCertificates[i]);
							if (x509Certificate3 == null)
							{
								goto IL_317;
							}
							x509Chain = new X509Chain();
							x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
							x509Chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreInvalidName;
							x509Chain.Build(x509Certificate3);
							bool flag2 = false;
							if (x509Chain.ChainElements.Count > 0)
							{
								for (int j = 0; j < x509Chain.ChainElements.Count; j++)
								{
									string issuer = x509Chain.ChainElements[j].Certificate.Issuer;
									flag2 = (Array.IndexOf<string>(array, issuer) != -1);
									if (flag2)
									{
										break;
									}
								}
							}
							if (!flag2)
							{
								goto IL_317;
							}
						}
						finally
						{
							if (x509Chain != null)
							{
								x509Chain.Reset();
							}
							if (x509Certificate3 != null && x509Certificate3 != this.m_ClientCertificates[i])
							{
								x509Certificate3.Reset();
							}
						}
						goto IL_2C6;
					}
					goto IL_2C6;
					IL_317:
					i++;
					continue;
					IL_2C6:
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_selected_cert", new object[]
						{
							this.m_ClientCertificates[i].ToString(true)
						}));
					}
					arrayList.Add(this.m_ClientCertificates[i]);
					goto IL_317;
				}
			}
			bool result = false;
			X509Certificate2 x509Certificate4 = null;
			x509Certificate = null;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_n_certs_after_filtering", new object[]
				{
					arrayList.Count
				}));
				if (arrayList.Count != 0)
				{
					Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_finding_matching_certs"));
				}
			}
			for (int k = 0; k < arrayList.Count; k++)
			{
				x509Certificate = (arrayList[k] as X509Certificate);
				if ((x509Certificate4 = this.EnsurePrivateKey(x509Certificate)) != null)
				{
					break;
				}
				x509Certificate = null;
				x509Certificate4 = null;
			}
			try
			{
				byte[] array2 = (x509Certificate4 == null) ? null : x509Certificate4.GetCertHash();
				SafeFreeCredentials safeFreeCredentials = SslSessionsCache.TryCachedCredential(array2, this.m_ProtocolFlags);
				if (flag && safeFreeCredentials == null && x509Certificate4 != null)
				{
					if (x509Certificate != x509Certificate4)
					{
						x509Certificate4.Reset();
					}
					array2 = null;
					x509Certificate4 = null;
					x509Certificate = null;
				}
				if (safeFreeCredentials != null)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, SR.GetString("net_log_using_cached_credential"));
					}
					this.m_CredentialsHandle = safeFreeCredentials;
					this.m_SelectedClientCertificate = x509Certificate;
					result = true;
				}
				else
				{
					SecureCredential.Flags flags = SecureCredential.Flags.ValidateManual | SecureCredential.Flags.NoDefaultCred;
					if (!ServicePointManager.DisableSendAuxRecord)
					{
						flags |= SecureCredential.Flags.SendAuxRecord;
					}
					if (!ServicePointManager.DisableStrongCrypto && (this.m_ProtocolFlags & SchProtocols.Tls) != SchProtocols.Zero)
					{
						flags |= SecureCredential.Flags.UseStrongCrypto;
					}
					SecureCredential secureCredential = new SecureCredential(4, x509Certificate4, flags, this.m_ProtocolFlags);
					this.m_CredentialsHandle = this.AcquireCredentialsHandle(CredentialUse.Outbound, ref secureCredential);
					thumbPrint = array2;
					this.m_SelectedClientCertificate = x509Certificate;
				}
			}
			finally
			{
				if (x509Certificate4 != null && x509Certificate != x509Certificate4)
				{
					x509Certificate4.Reset();
				}
			}
			return result;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000A8C7C File Offset: 0x000A7C7C
		[StorePermission(SecurityAction.Assert, Unrestricted = true)]
		private bool AcquireServerCredentials(ref byte[] thumbPrint)
		{
			X509Certificate x509Certificate = null;
			bool result = false;
			if (this.m_CertSelectionDelegate != null)
			{
				X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
				x509CertificateCollection.Add(this.m_ServerCertificate);
				x509Certificate = this.m_CertSelectionDelegate(string.Empty, x509CertificateCollection, null, new string[0]);
			}
			else
			{
				x509Certificate = this.m_ServerCertificate;
			}
			if (x509Certificate == null)
			{
				throw new NotSupportedException(SR.GetString("net_ssl_io_no_server_cert"));
			}
			X509Certificate2 x509Certificate2 = this.EnsurePrivateKey(x509Certificate);
			if (x509Certificate2 == null)
			{
				throw new NotSupportedException(SR.GetString("net_ssl_io_no_server_cert"));
			}
			byte[] certHash = x509Certificate2.GetCertHash();
			try
			{
				SafeFreeCredentials safeFreeCredentials = SslSessionsCache.TryCachedCredential(certHash, this.m_ProtocolFlags);
				if (safeFreeCredentials != null)
				{
					this.m_CredentialsHandle = safeFreeCredentials;
					this.m_ServerCertificate = x509Certificate;
					result = true;
				}
				else
				{
					SecureCredential.Flags flags = SecureCredential.Flags.Zero;
					if (!ServicePointManager.DisableSendAuxRecord)
					{
						flags |= SecureCredential.Flags.SendAuxRecord;
					}
					SecureCredential secureCredential = new SecureCredential(4, x509Certificate2, flags, this.m_ProtocolFlags);
					this.m_CredentialsHandle = this.AcquireCredentialsHandle(CredentialUse.Inbound, ref secureCredential);
					thumbPrint = certHash;
					this.m_ServerCertificate = x509Certificate;
				}
			}
			finally
			{
				if (x509Certificate != x509Certificate2)
				{
					x509Certificate2.Reset();
				}
			}
			return result;
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000A8D88 File Offset: 0x000A7D88
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private SafeFreeCredentials AcquireCredentialsHandle(CredentialUse credUsage, ref SecureCredential secureCredential)
		{
			SafeFreeCredentials result;
			try
			{
				using (WindowsIdentity.Impersonate(IntPtr.Zero))
				{
					result = SSPIWrapper.AcquireCredentialsHandle(GlobalSSPI.SSPISecureChannel, "Microsoft Unified Security Protocol Provider", credUsage, secureCredential);
				}
			}
			catch
			{
				result = SSPIWrapper.AcquireCredentialsHandle(GlobalSSPI.SSPISecureChannel, "Microsoft Unified Security Protocol Provider", credUsage, secureCredential);
			}
			return result;
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x000A8DFC File Offset: 0x000A7DFC
		internal ProtocolToken NextMessage(byte[] incoming, int offset, int count)
		{
			byte[] data = null;
			SecurityStatus securityStatus = this.GenerateToken(incoming, offset, count, ref data);
			if (!this.m_ServerMode && securityStatus == SecurityStatus.CredentialsNeeded)
			{
				this.SetRefreshCredentialNeeded();
				securityStatus = this.GenerateToken(incoming, offset, count, ref data);
			}
			return new ProtocolToken(data, securityStatus);
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000A8E44 File Offset: 0x000A7E44
		private SecurityStatus GenerateToken(byte[] input, int offset, int count, ref byte[] output)
		{
			if (offset < 0 || offset > ((input == null) ? 0 : input.Length))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > ((input == null) ? 0 : (input.Length - offset)))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			SecurityBuffer securityBuffer = null;
			SecurityBuffer[] inputBuffers = null;
			if (input != null)
			{
				securityBuffer = new SecurityBuffer(input, offset, count, BufferType.Token);
				inputBuffers = new SecurityBuffer[]
				{
					securityBuffer,
					new SecurityBuffer(null, 0, 0, BufferType.Empty)
				};
			}
			SecurityBuffer securityBuffer2 = new SecurityBuffer(null, BufferType.Token);
			int result = 0;
			bool flag = false;
			byte[] thumbPrint = null;
			try
			{
				do
				{
					thumbPrint = null;
					if (this.m_RefreshCredentialNeeded)
					{
						flag = (this.m_ServerMode ? this.AcquireServerCredentials(ref thumbPrint) : this.AcquireClientCredentials(ref thumbPrint));
					}
					if (this.m_ServerMode)
					{
						result = SSPIWrapper.AcceptSecurityContext(GlobalSSPI.SSPISecureChannel, ref this.m_CredentialsHandle, ref this.m_SecurityContext, ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.Confidentiality | ContextFlags.AllocateMemory | ContextFlags.AcceptStream | (this.m_RemoteCertRequired ? ContextFlags.MutualAuth : ContextFlags.Zero), Endianness.Native, securityBuffer, securityBuffer2, ref this.m_Attributes);
					}
					else if (securityBuffer == null)
					{
						result = SSPIWrapper.InitializeSecurityContext(GlobalSSPI.SSPISecureChannel, ref this.m_CredentialsHandle, ref this.m_SecurityContext, this.m_Destination, ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.Confidentiality | ContextFlags.AllocateMemory | ContextFlags.InitManualCredValidation, Endianness.Native, securityBuffer, securityBuffer2, ref this.m_Attributes);
					}
					else
					{
						result = SSPIWrapper.InitializeSecurityContext(GlobalSSPI.SSPISecureChannel, this.m_CredentialsHandle, ref this.m_SecurityContext, this.m_Destination, ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.Confidentiality | ContextFlags.AllocateMemory | ContextFlags.InitManualCredValidation, Endianness.Native, inputBuffers, securityBuffer2, ref this.m_Attributes);
					}
				}
				while (flag && this.m_CredentialsHandle == null);
			}
			finally
			{
				if (this.m_RefreshCredentialNeeded)
				{
					this.m_RefreshCredentialNeeded = false;
					if (this.m_CredentialsHandle != null)
					{
						this.m_CredentialsHandle.Close();
					}
					if (!flag && this.m_SecurityContext != null && !this.m_SecurityContext.IsInvalid && !this.m_CredentialsHandle.IsInvalid)
					{
						SslSessionsCache.CacheCredential(this.m_CredentialsHandle, thumbPrint, this.m_ProtocolFlags);
					}
				}
			}
			output = securityBuffer2.token;
			return (SecurityStatus)result;
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000A9010 File Offset: 0x000A8010
		internal void ProcessHandshakeSuccess()
		{
			StreamSizes streamSizes = SSPIWrapper.QueryContextAttributes(GlobalSSPI.SSPISecureChannel, this.m_SecurityContext, ContextAttribute.StreamSizes) as StreamSizes;
			if (streamSizes != null)
			{
				try
				{
					this.m_HeaderSize = streamSizes.header;
					this.m_TrailerSize = streamSizes.trailer;
					this.m_MaxDataSize = checked(streamSizes.maximumMessage - (this.m_HeaderSize + this.m_TrailerSize));
				}
				catch (Exception exception)
				{
					NclUtilities.IsFatal(exception);
					throw;
				}
			}
			this.m_ConnectionInfo = (SSPIWrapper.QueryContextAttributes(GlobalSSPI.SSPISecureChannel, this.m_SecurityContext, ContextAttribute.ConnectionInfo) as SslConnectionInfo);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000A90A4 File Offset: 0x000A80A4
		internal SecurityStatus Encrypt(byte[] buffer, int offset, int size, ref byte[] output, out int resultSize)
		{
			byte[] array;
			try
			{
				if (offset < 0 || offset > ((buffer == null) ? 0 : buffer.Length))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (size < 0 || size > ((buffer == null) ? 0 : (buffer.Length - offset)))
				{
					throw new ArgumentOutOfRangeException("size");
				}
				resultSize = 0;
				array = new byte[checked(size + this.m_HeaderSize + this.m_TrailerSize)];
				Buffer.BlockCopy(buffer, offset, array, this.m_HeaderSize, size);
			}
			catch (Exception exception)
			{
				NclUtilities.IsFatal(exception);
				throw;
			}
			SecurityBuffer[] array2 = new SecurityBuffer[]
			{
				new SecurityBuffer(array, 0, this.m_HeaderSize, BufferType.Header),
				new SecurityBuffer(array, this.m_HeaderSize, size, BufferType.Data),
				new SecurityBuffer(array, this.m_HeaderSize + size, this.m_TrailerSize, BufferType.Trailer),
				new SecurityBuffer(null, BufferType.Empty)
			};
			int num = SSPIWrapper.EncryptMessage(GlobalSSPI.SSPISecureChannel, this.m_SecurityContext, array2, 0U);
			if (num != 0)
			{
				return (SecurityStatus)num;
			}
			output = array;
			resultSize = array2[0].size + array2[1].size + array2[2].size;
			return SecurityStatus.OK;
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000A91B4 File Offset: 0x000A81B4
		internal SecurityStatus Decrypt(byte[] payload, ref int offset, ref int count)
		{
			if (offset < 0 || offset > ((payload == null) ? 0 : payload.Length))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > ((payload == null) ? 0 : (payload.Length - offset)))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			SecurityBuffer[] array = new SecurityBuffer[]
			{
				new SecurityBuffer(payload, offset, count, BufferType.Data),
				new SecurityBuffer(null, BufferType.Empty),
				new SecurityBuffer(null, BufferType.Empty),
				new SecurityBuffer(null, BufferType.Empty)
			};
			SecurityStatus securityStatus = (SecurityStatus)SSPIWrapper.DecryptMessage(GlobalSSPI.SSPISecureChannel, this.m_SecurityContext, array, 0U);
			count = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if ((securityStatus == SecurityStatus.OK && array[i].type == BufferType.Data) || (securityStatus != SecurityStatus.OK && array[i].type == BufferType.Extra))
				{
					offset = array[i].offset;
					count = array[i].size;
					break;
				}
			}
			return securityStatus;
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000A9288 File Offset: 0x000A8288
		[StorePermission(SecurityAction.Assert, Unrestricted = true)]
		internal unsafe bool VerifyRemoteCertificate(RemoteCertValidationCallback remoteCertValidationCallback)
		{
			SslPolicyErrors sslPolicyErrors = SslPolicyErrors.None;
			bool flag = false;
			X509Chain x509Chain = null;
			X509Certificate2 x509Certificate = null;
			try
			{
				X509Certificate2Collection x509Certificate2Collection;
				x509Certificate = this.GetRemoteCertificate(out x509Certificate2Collection);
				this.m_IsRemoteCertificateAvailable = (x509Certificate != null);
				if (x509Certificate == null)
				{
					sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNotAvailable;
				}
				else
				{
					x509Chain = new X509Chain();
					x509Chain.ChainPolicy.RevocationMode = (this.m_CheckCertRevocation ? X509RevocationMode.Online : X509RevocationMode.NoCheck);
					x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
					if (!ServicePointManager.DisableCertificateEKUs)
					{
						x509Chain.ChainPolicy.ApplicationPolicy.Add(this.m_ServerMode ? this.m_ClientAuthOid : this.m_ServerAuthOid);
					}
					if (x509Certificate2Collection != null)
					{
						x509Chain.ChainPolicy.ExtraStore.AddRange(x509Certificate2Collection);
					}
					x509Chain.Build(x509Certificate);
					if (this.m_CheckCertName)
					{
						ChainPolicyParameter chainPolicyParameter = default(ChainPolicyParameter);
						chainPolicyParameter.cbSize = ChainPolicyParameter.StructSize;
						chainPolicyParameter.dwFlags = 0U;
						SSL_EXTRA_CERT_CHAIN_POLICY_PARA ssl_EXTRA_CERT_CHAIN_POLICY_PARA = new SSL_EXTRA_CERT_CHAIN_POLICY_PARA(this.IsServer);
						chainPolicyParameter.pvExtraPolicyPara = &ssl_EXTRA_CERT_CHAIN_POLICY_PARA;
						try
						{
							fixed (char* hostName = this.m_HostName)
							{
								ssl_EXTRA_CERT_CHAIN_POLICY_PARA.pwszServerName = hostName;
								chainPolicyParameter.dwFlags |= 4031U;
								SafeFreeCertChain chainContext = new SafeFreeCertChain(x509Chain.ChainContext);
								uint num = PolicyWrapper.VerifyChainPolicy(chainContext, ref chainPolicyParameter);
								if (num == 2148204815U)
								{
									sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNameMismatch;
								}
							}
						}
						finally
						{
							string text = null;
						}
					}
					X509ChainStatus[] chainStatus = x509Chain.ChainStatus;
					if (chainStatus != null && chainStatus.Length != 0)
					{
						sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
					}
				}
				if (remoteCertValidationCallback != null)
				{
					flag = remoteCertValidationCallback(this.m_HostName, x509Certificate, x509Chain, sslPolicyErrors);
				}
				else
				{
					flag = ((sslPolicyErrors == SslPolicyErrors.RemoteCertificateNotAvailable && !this.m_RemoteCertRequired) || sslPolicyErrors == SslPolicyErrors.None);
				}
				if (Logging.On)
				{
					if (sslPolicyErrors != SslPolicyErrors.None)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_remote_cert_has_errors"));
						if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != SslPolicyErrors.None)
						{
							Logging.PrintInfo(Logging.Web, this, "\t" + SR.GetString("net_log_remote_cert_not_available"));
						}
						if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != SslPolicyErrors.None)
						{
							Logging.PrintInfo(Logging.Web, this, "\t" + SR.GetString("net_log_remote_cert_name_mismatch"));
						}
						if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != SslPolicyErrors.None)
						{
							foreach (X509ChainStatus x509ChainStatus in x509Chain.ChainStatus)
							{
								Logging.PrintInfo(Logging.Web, this, "\t" + x509ChainStatus.StatusInformation);
							}
						}
					}
					if (flag)
					{
						if (remoteCertValidationCallback != null)
						{
							Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_remote_cert_user_declared_valid"));
						}
						else
						{
							Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_remote_cert_has_no_errors"));
						}
					}
					else if (remoteCertValidationCallback != null)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_remote_cert_user_declared_invalid"));
					}
				}
			}
			finally
			{
				if (x509Chain != null)
				{
					x509Chain.Reset();
				}
				if (x509Certificate != null)
				{
					x509Certificate.Reset();
				}
			}
			return flag;
		}

		// Token: 0x04002791 RID: 10129
		internal const string SecurityPackage = "Microsoft Unified Security Protocol Provider";

		// Token: 0x04002792 RID: 10130
		private const ContextFlags RequiredFlags = ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.Confidentiality | ContextFlags.AllocateMemory;

		// Token: 0x04002793 RID: 10131
		private const ContextFlags ServerRequiredFlags = ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.Confidentiality | ContextFlags.AllocateMemory | ContextFlags.AcceptStream;

		// Token: 0x04002794 RID: 10132
		private const int ChainRevocationCheckExcludeRoot = 1073741824;

		// Token: 0x04002795 RID: 10133
		internal const int ReadHeaderSize = 5;

		// Token: 0x04002796 RID: 10134
		private static readonly object s_SyncObject = new object();

		// Token: 0x04002797 RID: 10135
		private static X509Store s_MyCertStoreEx;

		// Token: 0x04002798 RID: 10136
		private static X509Store s_MyMachineCertStoreEx;

		// Token: 0x04002799 RID: 10137
		private SafeFreeCredentials m_CredentialsHandle;

		// Token: 0x0400279A RID: 10138
		private SafeDeleteContext m_SecurityContext;

		// Token: 0x0400279B RID: 10139
		private ContextFlags m_Attributes;

		// Token: 0x0400279C RID: 10140
		private readonly string m_Destination;

		// Token: 0x0400279D RID: 10141
		private readonly string m_HostName;

		// Token: 0x0400279E RID: 10142
		private readonly bool m_ServerMode;

		// Token: 0x0400279F RID: 10143
		private readonly bool m_RemoteCertRequired;

		// Token: 0x040027A0 RID: 10144
		private readonly SchProtocols m_ProtocolFlags;

		// Token: 0x040027A1 RID: 10145
		private SslConnectionInfo m_ConnectionInfo;

		// Token: 0x040027A2 RID: 10146
		private X509Certificate m_ServerCertificate;

		// Token: 0x040027A3 RID: 10147
		private X509Certificate m_SelectedClientCertificate;

		// Token: 0x040027A4 RID: 10148
		private bool m_IsRemoteCertificateAvailable;

		// Token: 0x040027A5 RID: 10149
		private readonly X509CertificateCollection m_ClientCertificates;

		// Token: 0x040027A6 RID: 10150
		private LocalCertSelectionCallback m_CertSelectionDelegate;

		// Token: 0x040027A7 RID: 10151
		private int m_HeaderSize = 5;

		// Token: 0x040027A8 RID: 10152
		private int m_TrailerSize = 16;

		// Token: 0x040027A9 RID: 10153
		private int m_MaxDataSize = 16354;

		// Token: 0x040027AA RID: 10154
		private bool m_CheckCertRevocation;

		// Token: 0x040027AB RID: 10155
		private bool m_CheckCertName;

		// Token: 0x040027AC RID: 10156
		private bool m_RefreshCredentialNeeded;

		// Token: 0x040027AD RID: 10157
		private readonly Oid m_ServerAuthOid = new Oid("1.3.6.1.5.5.7.3.1", "1.3.6.1.5.5.7.3.1");

		// Token: 0x040027AE RID: 10158
		private readonly Oid m_ClientAuthOid = new Oid("1.3.6.1.5.5.7.3.2", "1.3.6.1.5.5.7.3.2");

		// Token: 0x02000533 RID: 1331
		private static class UnmanagedCertificateContext
		{
			// Token: 0x060028C3 RID: 10435 RVA: 0x000A9574 File Offset: 0x000A8574
			internal static X509Certificate2Collection GetStore(SafeFreeCertContext certContext)
			{
				X509Certificate2Collection result = new X509Certificate2Collection();
				if (certContext.IsInvalid)
				{
					return result;
				}
				SecureChannel.UnmanagedCertificateContext._CERT_CONTEXT cert_CONTEXT = (SecureChannel.UnmanagedCertificateContext._CERT_CONTEXT)Marshal.PtrToStructure(certContext.DangerousGetHandle(), typeof(SecureChannel.UnmanagedCertificateContext._CERT_CONTEXT));
				if (cert_CONTEXT.hCertStore != IntPtr.Zero)
				{
					X509Store x509Store = null;
					try
					{
						x509Store = new X509Store(cert_CONTEXT.hCertStore);
						result = x509Store.Certificates;
					}
					finally
					{
						if (x509Store != null)
						{
							x509Store.Close();
						}
					}
				}
				return result;
			}

			// Token: 0x02000534 RID: 1332
			private struct _CERT_CONTEXT
			{
				// Token: 0x040027AF RID: 10159
				internal int dwCertEncodingType;

				// Token: 0x040027B0 RID: 10160
				internal IntPtr pbCertEncoded;

				// Token: 0x040027B1 RID: 10161
				internal int cbCertEncoded;

				// Token: 0x040027B2 RID: 10162
				internal IntPtr pCertInfo;

				// Token: 0x040027B3 RID: 10163
				internal IntPtr hCertStore;
			}
		}
	}
}
