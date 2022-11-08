using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200005C RID: 92
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class EnvelopedCms
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000040EB File Offset: 0x000030EB
		public EnvelopedCms() : this(SubjectIdentifierType.IssuerAndSerialNumber, new ContentInfo("1.2.840.113549.1.7.1", new byte[0]), new AlgorithmIdentifier("1.2.840.113549.3.7"))
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000410E File Offset: 0x0000310E
		public EnvelopedCms(ContentInfo contentInfo) : this(SubjectIdentifierType.IssuerAndSerialNumber, contentInfo, new AlgorithmIdentifier("1.2.840.113549.3.7"))
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004122 File Offset: 0x00003122
		public EnvelopedCms(SubjectIdentifierType recipientIdentifierType, ContentInfo contentInfo) : this(recipientIdentifierType, contentInfo, new AlgorithmIdentifier("1.2.840.113549.3.7"))
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004136 File Offset: 0x00003136
		public EnvelopedCms(ContentInfo contentInfo, AlgorithmIdentifier encryptionAlgorithm) : this(SubjectIdentifierType.IssuerAndSerialNumber, contentInfo, encryptionAlgorithm)
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004144 File Offset: 0x00003144
		public EnvelopedCms(SubjectIdentifierType recipientIdentifierType, ContentInfo contentInfo, AlgorithmIdentifier encryptionAlgorithm)
		{
			if (contentInfo == null)
			{
				throw new ArgumentNullException("contentInfo");
			}
			if (contentInfo.Content == null)
			{
				throw new ArgumentNullException("contentInfo.Content");
			}
			if (encryptionAlgorithm == null)
			{
				throw new ArgumentNullException("encryptionAlgorithm");
			}
			this.m_safeCryptMsgHandle = SafeCryptMsgHandle.InvalidHandle;
			this.m_version = ((recipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier) ? 2 : 0);
			this.m_recipientIdentifierType = recipientIdentifierType;
			this.m_contentInfo = contentInfo;
			this.m_encryptionAlgorithm = encryptionAlgorithm;
			this.m_encryptionAlgorithm.Parameters = new byte[0];
			this.m_certificates = new X509Certificate2Collection();
			this.m_unprotectedAttributes = new CryptographicAttributeObjectCollection();
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000041DB File Offset: 0x000031DB
		public int Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000041E3 File Offset: 0x000031E3
		public ContentInfo ContentInfo
		{
			get
			{
				return this.m_contentInfo;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000041EB File Offset: 0x000031EB
		public AlgorithmIdentifier ContentEncryptionAlgorithm
		{
			get
			{
				return this.m_encryptionAlgorithm;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000041F3 File Offset: 0x000031F3
		public X509Certificate2Collection Certificates
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000041FB File Offset: 0x000031FB
		public CryptographicAttributeObjectCollection UnprotectedAttributes
		{
			get
			{
				return this.m_unprotectedAttributes;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004203 File Offset: 0x00003203
		public RecipientInfoCollection RecipientInfos
		{
			get
			{
				if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
				{
					return new RecipientInfoCollection();
				}
				return new RecipientInfoCollection(this.m_safeCryptMsgHandle);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000422B File Offset: 0x0000322B
		public byte[] Encode()
		{
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_MessageNotEncrypted"));
			}
			return PkcsUtils.GetContent(this.m_safeCryptMsgHandle);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004260 File Offset: 0x00003260
		public void Decode(byte[] encodedMessage)
		{
			if (encodedMessage == null)
			{
				throw new ArgumentNullException("encodedMessage");
			}
			if (this.m_safeCryptMsgHandle != null && !this.m_safeCryptMsgHandle.IsInvalid)
			{
				this.m_safeCryptMsgHandle.Dispose();
			}
			this.m_safeCryptMsgHandle = EnvelopedCms.OpenToDecode(encodedMessage);
			this.m_version = (int)PkcsUtils.GetVersion(this.m_safeCryptMsgHandle);
			Oid contentType = PkcsUtils.GetContentType(this.m_safeCryptMsgHandle);
			byte[] content = PkcsUtils.GetContent(this.m_safeCryptMsgHandle);
			this.m_contentInfo = new ContentInfo(contentType, content);
			this.m_encryptionAlgorithm = PkcsUtils.GetAlgorithmIdentifier(this.m_safeCryptMsgHandle);
			this.m_certificates = PkcsUtils.GetCertificates(this.m_safeCryptMsgHandle);
			this.m_unprotectedAttributes = PkcsUtils.GetUnprotectedAttributes(this.m_safeCryptMsgHandle);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004310 File Offset: 0x00003310
		public void Encrypt()
		{
			this.Encrypt(new CmsRecipientCollection());
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000431D File Offset: 0x0000331D
		public void Encrypt(CmsRecipient recipient)
		{
			if (recipient == null)
			{
				throw new ArgumentNullException("recipient");
			}
			this.Encrypt(new CmsRecipientCollection(recipient));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000433C File Offset: 0x0000333C
		public void Encrypt(CmsRecipientCollection recipients)
		{
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			if (this.ContentInfo.Content.Length == 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Envelope_Empty_Content"));
			}
			if (recipients.Count == 0)
			{
				recipients = PkcsUtils.SelectRecipients(this.m_recipientIdentifierType);
			}
			this.EncryptContent(recipients);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004392 File Offset: 0x00003392
		public void Decrypt()
		{
			this.DecryptContent(this.RecipientInfos, null);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000043A1 File Offset: 0x000033A1
		public void Decrypt(RecipientInfo recipientInfo)
		{
			if (recipientInfo == null)
			{
				throw new ArgumentNullException("recipientInfo");
			}
			this.DecryptContent(new RecipientInfoCollection(recipientInfo), null);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000043BE File Offset: 0x000033BE
		public void Decrypt(X509Certificate2Collection extraStore)
		{
			if (extraStore == null)
			{
				throw new ArgumentNullException("extraStore");
			}
			this.DecryptContent(this.RecipientInfos, extraStore);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000043DB File Offset: 0x000033DB
		public void Decrypt(RecipientInfo recipientInfo, X509Certificate2Collection extraStore)
		{
			if (recipientInfo == null)
			{
				throw new ArgumentNullException("recipientInfo");
			}
			if (extraStore == null)
			{
				throw new ArgumentNullException("extraStore");
			}
			this.DecryptContent(new RecipientInfoCollection(recipientInfo), extraStore);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004408 File Offset: 0x00003408
		private unsafe void DecryptContent(RecipientInfoCollection recipientInfos, X509Certificate2Collection extraStore)
		{
			int num = -2146889717;
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_NoEncryptedMessageToEncode"));
			}
			for (int i = 0; i < recipientInfos.Count; i++)
			{
				RecipientInfo recipientInfo = recipientInfos[i];
				EnvelopedCms.CMSG_DECRYPT_PARAM cmsg_DECRYPT_PARAM = default(EnvelopedCms.CMSG_DECRYPT_PARAM);
				int num2 = EnvelopedCms.GetCspParams(recipientInfo, extraStore, ref cmsg_DECRYPT_PARAM);
				if (num2 == 0)
				{
					CspParameters parameters = new CspParameters();
					if (!X509Utils.GetPrivateKeyInfo(cmsg_DECRYPT_PARAM.safeCertContextHandle, ref parameters))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
					KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Decrypt);
					keyContainerPermission.AccessEntries.Add(accessEntry);
					keyContainerPermission.Demand();
					switch (recipientInfo.Type)
					{
					case RecipientInfoType.KeyTransport:
					{
						CAPIBase.CMSG_CTRL_DECRYPT_PARA cmsg_CTRL_DECRYPT_PARA = new CAPIBase.CMSG_CTRL_DECRYPT_PARA(Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_DECRYPT_PARA)));
						cmsg_CTRL_DECRYPT_PARA.hCryptProv = cmsg_DECRYPT_PARAM.safeCryptProvHandle.DangerousGetHandle();
						cmsg_CTRL_DECRYPT_PARA.dwKeySpec = cmsg_DECRYPT_PARAM.keySpec;
						cmsg_CTRL_DECRYPT_PARA.dwRecipientIndex = recipientInfo.Index;
						if (!CAPI.CryptMsgControl(this.m_safeCryptMsgHandle, 0U, 2U, new IntPtr((void*)(&cmsg_CTRL_DECRYPT_PARA))))
						{
							num2 = Marshal.GetHRForLastWin32Error();
						}
						GC.KeepAlive(cmsg_CTRL_DECRYPT_PARA);
						break;
					}
					case RecipientInfoType.KeyAgreement:
					{
						SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
						KeyAgreeRecipientInfo keyAgreeRecipientInfo = (KeyAgreeRecipientInfo)recipientInfo;
						CAPIBase.CMSG_CMS_RECIPIENT_INFO cmsg_CMS_RECIPIENT_INFO = (CAPIBase.CMSG_CMS_RECIPIENT_INFO)Marshal.PtrToStructure(keyAgreeRecipientInfo.pCmsgRecipientInfo.DangerousGetHandle(), typeof(CAPIBase.CMSG_CMS_RECIPIENT_INFO));
						CAPIBase.CMSG_CTRL_KEY_AGREE_DECRYPT_PARA cmsg_CTRL_KEY_AGREE_DECRYPT_PARA = new CAPIBase.CMSG_CTRL_KEY_AGREE_DECRYPT_PARA(Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_KEY_AGREE_DECRYPT_PARA)));
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.hCryptProv = cmsg_DECRYPT_PARAM.safeCryptProvHandle.DangerousGetHandle();
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.dwKeySpec = cmsg_DECRYPT_PARAM.keySpec;
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.pKeyAgree = cmsg_CMS_RECIPIENT_INFO.pRecipientInfo;
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.dwRecipientIndex = keyAgreeRecipientInfo.Index;
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.dwRecipientEncryptedKeyIndex = keyAgreeRecipientInfo.SubIndex;
						if (keyAgreeRecipientInfo.SubType == RecipientSubType.CertIdKeyAgreement)
						{
							CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO cmsg_KEY_AGREE_CERT_ID_RECIPIENT_INFO = (CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO)keyAgreeRecipientInfo.CmsgRecipientInfo;
							SafeCertStoreHandle hCertStore = EnvelopedCms.BuildOriginatorStore(this.Certificates, extraStore);
							safeCertContextHandle = CAPI.CertFindCertificateInStore(hCertStore, 65537U, 0U, 1048576U, new IntPtr((void*)(&cmsg_KEY_AGREE_CERT_ID_RECIPIENT_INFO.OriginatorCertId)), SafeCertContextHandle.InvalidHandle);
							if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
							{
								num2 = -2146885628;
								break;
							}
							cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.OriginatorPublicKey = ((CAPIBase.CERT_INFO)Marshal.PtrToStructure(((CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(safeCertContextHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT))).pCertInfo, typeof(CAPIBase.CERT_INFO))).SubjectPublicKeyInfo.PublicKey;
						}
						else
						{
							cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.OriginatorPublicKey = ((CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO)keyAgreeRecipientInfo.CmsgRecipientInfo).OriginatorPublicKeyInfo.PublicKey;
						}
						if (!CAPI.CryptMsgControl(this.m_safeCryptMsgHandle, 0U, 17U, new IntPtr((void*)(&cmsg_CTRL_KEY_AGREE_DECRYPT_PARA))))
						{
							num2 = Marshal.GetHRForLastWin32Error();
						}
						GC.KeepAlive(cmsg_CTRL_KEY_AGREE_DECRYPT_PARA);
						GC.KeepAlive(safeCertContextHandle);
						break;
					}
					default:
						throw new CryptographicException(-2147483647);
					}
					GC.KeepAlive(cmsg_DECRYPT_PARAM);
				}
				if (num2 == 0)
				{
					uint num3 = 0U;
					SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle;
					PkcsUtils.GetParam(this.m_safeCryptMsgHandle, 2U, 0U, out invalidHandle, out num3);
					if (num3 > 0U)
					{
						Oid contentType = PkcsUtils.GetContentType(this.m_safeCryptMsgHandle);
						byte[] array = new byte[num3];
						Marshal.Copy(invalidHandle.DangerousGetHandle(), array, 0, (int)num3);
						this.m_contentInfo = new ContentInfo(contentType, array);
					}
					invalidHandle.Dispose();
					num = 0;
					break;
				}
				num = num2;
			}
			if (num != 0)
			{
				throw new CryptographicException(num);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000478C File Offset: 0x0000378C
		private unsafe void EncryptContent(CmsRecipientCollection recipients)
		{
			EnvelopedCms.CMSG_ENCRYPT_PARAM cmsg_ENCRYPT_PARAM = default(EnvelopedCms.CMSG_ENCRYPT_PARAM);
			if (recipients.Count < 1)
			{
				throw new CryptographicException(-2146889717);
			}
			foreach (CmsRecipient cmsRecipient in recipients)
			{
				if (cmsRecipient.Certificate == null)
				{
					throw new ArgumentNullException(SecurityResources.GetResourceString("Cryptography_Cms_RecipientCertificateNotFound"));
				}
				if (PkcsUtils.GetRecipientInfoType(cmsRecipient.Certificate) == RecipientInfoType.KeyAgreement || cmsRecipient.RecipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
				{
					cmsg_ENCRYPT_PARAM.useCms = true;
				}
			}
			if (!cmsg_ENCRYPT_PARAM.useCms && (this.Certificates.Count > 0 || this.UnprotectedAttributes.Count > 0))
			{
				cmsg_ENCRYPT_PARAM.useCms = true;
			}
			if (cmsg_ENCRYPT_PARAM.useCms && !PkcsUtils.CmsSupported())
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Not_Supported"));
			}
			CAPIBase.CMSG_ENVELOPED_ENCODE_INFO cmsg_ENVELOPED_ENCODE_INFO = new CAPIBase.CMSG_ENVELOPED_ENCODE_INFO(Marshal.SizeOf(typeof(CAPIBase.CMSG_ENVELOPED_ENCODE_INFO)));
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_ENVELOPED_ENCODE_INFO))));
			EnvelopedCms.SetCspParams(this.ContentEncryptionAlgorithm, ref cmsg_ENCRYPT_PARAM);
			cmsg_ENVELOPED_ENCODE_INFO.ContentEncryptionAlgorithm.pszObjId = this.ContentEncryptionAlgorithm.Oid.Value;
			if (cmsg_ENCRYPT_PARAM.pvEncryptionAuxInfo != null && !cmsg_ENCRYPT_PARAM.pvEncryptionAuxInfo.IsInvalid)
			{
				cmsg_ENVELOPED_ENCODE_INFO.pvEncryptionAuxInfo = cmsg_ENCRYPT_PARAM.pvEncryptionAuxInfo.DangerousGetHandle();
			}
			cmsg_ENVELOPED_ENCODE_INFO.cRecipients = (uint)recipients.Count;
			List<SafeCertContextHandle> obj = null;
			if (cmsg_ENCRYPT_PARAM.useCms)
			{
				EnvelopedCms.SetCmsRecipientParams(recipients, this.Certificates, this.UnprotectedAttributes, this.ContentEncryptionAlgorithm, ref cmsg_ENCRYPT_PARAM);
				cmsg_ENVELOPED_ENCODE_INFO.rgCmsRecipients = cmsg_ENCRYPT_PARAM.rgpRecipients.DangerousGetHandle();
				if (cmsg_ENCRYPT_PARAM.rgCertEncoded != null && !cmsg_ENCRYPT_PARAM.rgCertEncoded.IsInvalid)
				{
					cmsg_ENVELOPED_ENCODE_INFO.cCertEncoded = (uint)this.Certificates.Count;
					cmsg_ENVELOPED_ENCODE_INFO.rgCertEncoded = cmsg_ENCRYPT_PARAM.rgCertEncoded.DangerousGetHandle();
				}
				if (cmsg_ENCRYPT_PARAM.rgUnprotectedAttr != null && !cmsg_ENCRYPT_PARAM.rgUnprotectedAttr.IsInvalid)
				{
					cmsg_ENVELOPED_ENCODE_INFO.cUnprotectedAttr = (uint)this.UnprotectedAttributes.Count;
					cmsg_ENVELOPED_ENCODE_INFO.rgUnprotectedAttr = cmsg_ENCRYPT_PARAM.rgUnprotectedAttr.DangerousGetHandle();
				}
			}
			else
			{
				EnvelopedCms.SetPkcs7RecipientParams(recipients, ref cmsg_ENCRYPT_PARAM, out obj);
				cmsg_ENVELOPED_ENCODE_INFO.rgpRecipients = cmsg_ENCRYPT_PARAM.rgpRecipients.DangerousGetHandle();
			}
			Marshal.StructureToPtr(cmsg_ENVELOPED_ENCODE_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
			try
			{
				SafeCryptMsgHandle safeCryptMsgHandle = CAPI.CryptMsgOpenToEncode(65537U, 0U, 3U, safeLocalAllocHandle.DangerousGetHandle(), this.ContentInfo.ContentType.Value, IntPtr.Zero);
				if (safeCryptMsgHandle == null || safeCryptMsgHandle.IsInvalid)
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				if (this.m_safeCryptMsgHandle != null && !this.m_safeCryptMsgHandle.IsInvalid)
				{
					this.m_safeCryptMsgHandle.Dispose();
				}
				this.m_safeCryptMsgHandle = safeCryptMsgHandle;
			}
			finally
			{
				Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_ENVELOPED_ENCODE_INFO));
				safeLocalAllocHandle.Dispose();
			}
			byte[] array = new byte[0];
			if (string.Compare(this.ContentInfo.ContentType.Value, "1.2.840.113549.1.7.1", StringComparison.OrdinalIgnoreCase) == 0)
			{
				byte[] content = this.ContentInfo.Content;
				fixed (byte* ptr = content)
				{
					CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
					cryptoapi_BLOB.cbData = (uint)content.Length;
					cryptoapi_BLOB.pbData = new IntPtr((void*)ptr);
					if (!CAPI.EncodeObject(new IntPtr(25L), new IntPtr((void*)(&cryptoapi_BLOB)), out array))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
				}
			}
			else
			{
				array = this.ContentInfo.Content;
			}
			if (array.Length > 0 && !CAPISafe.CryptMsgUpdate(this.m_safeCryptMsgHandle, array, (uint)array.Length, true))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			GC.KeepAlive(cmsg_ENCRYPT_PARAM);
			GC.KeepAlive(recipients);
			GC.KeepAlive(obj);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004B48 File Offset: 0x00003B48
		private static SafeCryptMsgHandle OpenToDecode(byte[] encodedMessage)
		{
			SafeCryptMsgHandle safeCryptMsgHandle = CAPISafe.CryptMsgOpenToDecode(65537U, 0U, 0U, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			if (safeCryptMsgHandle == null || safeCryptMsgHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (!CAPISafe.CryptMsgUpdate(safeCryptMsgHandle, encodedMessage, (uint)encodedMessage.Length, true))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (3U != PkcsUtils.GetMessageType(safeCryptMsgHandle))
			{
				throw new CryptographicException(-2146889724);
			}
			return safeCryptMsgHandle;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004BB8 File Offset: 0x00003BB8
		private unsafe static int GetCspParams(RecipientInfo recipientInfo, X509Certificate2Collection extraStore, ref EnvelopedCms.CMSG_DECRYPT_PARAM cmsgDecryptParam)
		{
			int result = -2146889717;
			SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
			SafeCertStoreHandle hCertStore = EnvelopedCms.BuildDecryptorStore(extraStore);
			switch (recipientInfo.Type)
			{
			case RecipientInfoType.KeyTransport:
				if (recipientInfo.SubType == RecipientSubType.Pkcs7KeyTransport)
				{
					safeCertContextHandle = CAPI.CertFindCertificateInStore(hCertStore, 65537U, 0U, 720896U, recipientInfo.pCmsgRecipientInfo.DangerousGetHandle(), SafeCertContextHandle.InvalidHandle);
				}
				else
				{
					safeCertContextHandle = CAPI.CertFindCertificateInStore(hCertStore, 65537U, 0U, 1048576U, new IntPtr((void*)(&((CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO)recipientInfo.CmsgRecipientInfo).RecipientId)), SafeCertContextHandle.InvalidHandle);
				}
				break;
			case RecipientInfoType.KeyAgreement:
			{
				KeyAgreeRecipientInfo keyAgreeRecipientInfo = (KeyAgreeRecipientInfo)recipientInfo;
				CAPIBase.CERT_ID recipientId = keyAgreeRecipientInfo.RecipientId;
				safeCertContextHandle = CAPI.CertFindCertificateInStore(hCertStore, 65537U, 0U, 1048576U, new IntPtr((void*)(&recipientId)), SafeCertContextHandle.InvalidHandle);
				break;
			}
			default:
				result = -2147483647;
				break;
			}
			if (safeCertContextHandle != null && !safeCertContextHandle.IsInvalid)
			{
				SafeCryptProvHandle invalidHandle = SafeCryptProvHandle.InvalidHandle;
				uint num = 0U;
				bool flag = false;
				CspParameters cspParameters = new CspParameters();
				if (!X509Utils.GetPrivateKeyInfo(safeCertContextHandle, ref cspParameters))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				if (string.Compare(cspParameters.ProviderName, "Microsoft Base Cryptographic Provider v1.0", StringComparison.OrdinalIgnoreCase) == 0 && (CAPI.CryptAcquireContext(ref invalidHandle, cspParameters.KeyContainerName, "Microsoft Enhanced Cryptographic Provider v1.0", 1U, 0U) || CAPI.CryptAcquireContext(ref invalidHandle, cspParameters.KeyContainerName, "Microsoft Strong Cryptographic Provider", 1U, 0U)))
				{
					cmsgDecryptParam.safeCryptProvHandle = invalidHandle;
				}
				cmsgDecryptParam.safeCertContextHandle = safeCertContextHandle;
				cmsgDecryptParam.keySpec = (uint)cspParameters.KeyNumber;
				result = 0;
				if (invalidHandle == null || invalidHandle.IsInvalid)
				{
					if (CAPISafe.CryptAcquireCertificatePrivateKey(safeCertContextHandle, 6U, IntPtr.Zero, ref invalidHandle, ref num, ref flag))
					{
						if (!flag)
						{
							GC.SuppressFinalize(invalidHandle);
						}
						cmsgDecryptParam.safeCryptProvHandle = invalidHandle;
					}
					else
					{
						result = Marshal.GetHRForLastWin32Error();
					}
				}
			}
			return result;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004D64 File Offset: 0x00003D64
		private static void SetCspParams(AlgorithmIdentifier contentEncryptionAlgorithm, ref EnvelopedCms.CMSG_ENCRYPT_PARAM encryptParam)
		{
			encryptParam.safeCryptProvHandle = SafeCryptProvHandle.InvalidHandle;
			encryptParam.pvEncryptionAuxInfo = SafeLocalAllocHandle.InvalidHandle;
			SafeCryptProvHandle invalidHandle = SafeCryptProvHandle.InvalidHandle;
			if (!CAPI.CryptAcquireContext(ref invalidHandle, IntPtr.Zero, IntPtr.Zero, 1U, 4026531840U) && !CAPI.CryptAcquireContext(ref invalidHandle, IntPtr.Zero, IntPtr.Zero, 1U, 0U))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			uint num = X509Utils.OidToAlgId(contentEncryptionAlgorithm.Oid.Value);
			if (num == 26114U || num == 26625U)
			{
				CAPIBase.CMSG_RC2_AUX_INFO cmsg_RC2_AUX_INFO = new CAPIBase.CMSG_RC2_AUX_INFO(Marshal.SizeOf(typeof(CAPIBase.CMSG_RC2_AUX_INFO)));
				uint num2 = (uint)contentEncryptionAlgorithm.KeyLength;
				if (num2 == 0U)
				{
					num2 = (uint)PkcsUtils.GetMaxKeyLength(invalidHandle, num);
				}
				cmsg_RC2_AUX_INFO.dwBitLen = num2;
				SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_RC2_AUX_INFO))));
				Marshal.StructureToPtr(cmsg_RC2_AUX_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
				encryptParam.pvEncryptionAuxInfo = safeLocalAllocHandle;
			}
			encryptParam.safeCryptProvHandle = invalidHandle;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004E58 File Offset: 0x00003E58
		private unsafe static void SetCmsRecipientParams(CmsRecipientCollection recipients, X509Certificate2Collection certificates, CryptographicAttributeObjectCollection unprotectedAttributes, AlgorithmIdentifier contentEncryptionAlgorithm, ref EnvelopedCms.CMSG_ENCRYPT_PARAM encryptParam)
		{
			uint[] array = new uint[recipients.Count];
			int num = 0;
			int num2 = recipients.Count * Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCODE_INFO));
			int num3 = num2;
			for (int i = 0; i < recipients.Count; i++)
			{
				array[i] = (uint)PkcsUtils.GetRecipientInfoType(recipients[i].Certificate);
				if (array[i] == 1U)
				{
					num3 += Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO));
				}
				else
				{
					if (array[i] != 2U)
					{
						throw new CryptographicException(-2146889726);
					}
					num++;
					num3 += Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO));
				}
			}
			encryptParam.rgpRecipients = CAPI.LocalAlloc(64U, new IntPtr(num3));
			encryptParam.rgCertEncoded = SafeLocalAllocHandle.InvalidHandle;
			encryptParam.rgUnprotectedAttr = SafeLocalAllocHandle.InvalidHandle;
			encryptParam.rgSubjectKeyIdentifier = new SafeLocalAllocHandle[recipients.Count];
			encryptParam.rgszObjId = new SafeLocalAllocHandle[recipients.Count];
			if (num > 0)
			{
				encryptParam.rgszKeyWrapObjId = new SafeLocalAllocHandle[num];
				encryptParam.rgKeyWrapAuxInfo = new SafeLocalAllocHandle[num];
				encryptParam.rgEphemeralIdentifier = new SafeLocalAllocHandle[num];
				encryptParam.rgszEphemeralObjId = new SafeLocalAllocHandle[num];
				encryptParam.rgUserKeyingMaterial = new SafeLocalAllocHandle[num];
				encryptParam.prgpEncryptedKey = new SafeLocalAllocHandle[num];
				encryptParam.rgpEncryptedKey = new SafeLocalAllocHandle[num];
			}
			if (certificates.Count > 0)
			{
				encryptParam.rgCertEncoded = CAPI.LocalAlloc(64U, new IntPtr(certificates.Count * Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB))));
				for (int i = 0; i < certificates.Count; i++)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = (CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(X509Utils.GetCertContext(certificates[i]).DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT));
					CAPIBase.CRYPTOAPI_BLOB* ptr = (CAPIBase.CRYPTOAPI_BLOB*)((void*)new IntPtr((long)encryptParam.rgCertEncoded.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB)))));
					ptr->cbData = cert_CONTEXT.cbCertEncoded;
					ptr->pbData = cert_CONTEXT.pbCertEncoded;
				}
			}
			if (unprotectedAttributes.Count > 0)
			{
				encryptParam.rgUnprotectedAttr = new SafeLocalAllocHandle(PkcsUtils.CreateCryptAttributes(unprotectedAttributes));
			}
			num = 0;
			IntPtr intPtr = new IntPtr((long)encryptParam.rgpRecipients.DangerousGetHandle() + (long)num2);
			for (int i = 0; i < recipients.Count; i++)
			{
				CmsRecipient cmsRecipient = recipients[i];
				X509Certificate2 certificate = cmsRecipient.Certificate;
				CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(((CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(X509Utils.GetCertContext(certificate).DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT))).pCertInfo, typeof(CAPIBase.CERT_INFO));
				CAPIBase.CMSG_RECIPIENT_ENCODE_INFO* ptr2 = (CAPIBase.CMSG_RECIPIENT_ENCODE_INFO*)((void*)new IntPtr((long)encryptParam.rgpRecipients.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCODE_INFO)))));
				ptr2->dwRecipientChoice = array[i];
				ptr2->pRecipientInfo = intPtr;
				if (array[i] == 1U)
				{
					IntPtr ptr3 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "cbSize"));
					Marshal.WriteInt32(ptr3, Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO)));
					IntPtr value = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "KeyEncryptionAlgorithm"));
					byte[] bytes = Encoding.ASCII.GetBytes(cert_INFO.SubjectPublicKeyInfo.Algorithm.pszObjId);
					encryptParam.rgszObjId[i] = CAPI.LocalAlloc(64U, new IntPtr(bytes.Length + 1));
					Marshal.Copy(bytes, 0, encryptParam.rgszObjId[i].DangerousGetHandle(), bytes.Length);
					IntPtr ptr4 = new IntPtr((long)value + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(ptr4, encryptParam.rgszObjId[i].DangerousGetHandle());
					IntPtr value2 = new IntPtr((long)value + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "Parameters"));
					IntPtr ptr5 = new IntPtr((long)value2 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
					Marshal.WriteInt32(ptr5, (int)cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.cbData);
					IntPtr ptr6 = new IntPtr((long)value2 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
					Marshal.WriteIntPtr(ptr6, cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.pbData);
					IntPtr value3 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "RecipientPublicKey"));
					ptr5 = new IntPtr((long)value3 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cbData"));
					Marshal.WriteInt32(ptr5, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cbData);
					ptr6 = new IntPtr((long)value3 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "pbData"));
					Marshal.WriteIntPtr(ptr6, cert_INFO.SubjectPublicKeyInfo.PublicKey.pbData);
					IntPtr ptr7 = new IntPtr((long)value3 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cUnusedBits"));
					Marshal.WriteInt32(ptr7, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cUnusedBits);
					IntPtr value4 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "RecipientId"));
					if (cmsRecipient.RecipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
					{
						uint num4 = 0U;
						SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle, ref num4))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num4)));
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle, ref num4))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						encryptParam.rgSubjectKeyIdentifier[i] = safeLocalAllocHandle;
						IntPtr ptr8 = new IntPtr((long)value4 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "dwIdChoice"));
						Marshal.WriteInt32(ptr8, 2);
						IntPtr value5 = new IntPtr((long)value4 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						ptr5 = new IntPtr((long)value5 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(ptr5, (int)num4);
						ptr6 = new IntPtr((long)value5 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(ptr6, safeLocalAllocHandle.DangerousGetHandle());
					}
					else
					{
						IntPtr ptr9 = new IntPtr((long)value4 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "dwIdChoice"));
						Marshal.WriteInt32(ptr9, 1);
						IntPtr value6 = new IntPtr((long)value4 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						IntPtr value7 = new IntPtr((long)value6 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "Issuer"));
						ptr5 = new IntPtr((long)value7 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(ptr5, (int)cert_INFO.Issuer.cbData);
						ptr6 = new IntPtr((long)value7 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(ptr6, cert_INFO.Issuer.pbData);
						IntPtr value8 = new IntPtr((long)value6 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "SerialNumber"));
						ptr5 = new IntPtr((long)value8 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(ptr5, (int)cert_INFO.SerialNumber.cbData);
						ptr6 = new IntPtr((long)value8 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(ptr6, cert_INFO.SerialNumber.pbData);
					}
					intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO)));
				}
				else if (array[i] == 2U)
				{
					IntPtr ptr10 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "cbSize"));
					Marshal.WriteInt32(ptr10, Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO)));
					IntPtr value9 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "KeyEncryptionAlgorithm"));
					byte[] bytes2 = Encoding.ASCII.GetBytes("1.2.840.113549.1.9.16.3.5");
					encryptParam.rgszObjId[i] = CAPI.LocalAlloc(64U, new IntPtr(bytes2.Length + 1));
					Marshal.Copy(bytes2, 0, encryptParam.rgszObjId[i].DangerousGetHandle(), bytes2.Length);
					IntPtr ptr11 = new IntPtr((long)value9 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(ptr11, encryptParam.rgszObjId[i].DangerousGetHandle());
					IntPtr value10 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "KeyWrapAlgorithm"));
					uint num5 = X509Utils.OidToAlgId(contentEncryptionAlgorithm.Oid.Value);
					if (num5 == 26114U)
					{
						bytes2 = Encoding.ASCII.GetBytes("1.2.840.113549.1.9.16.3.7");
					}
					else
					{
						bytes2 = Encoding.ASCII.GetBytes("1.2.840.113549.1.9.16.3.6");
					}
					encryptParam.rgszKeyWrapObjId[num] = CAPI.LocalAlloc(64U, new IntPtr(bytes2.Length + 1));
					Marshal.Copy(bytes2, 0, encryptParam.rgszKeyWrapObjId[num].DangerousGetHandle(), bytes2.Length);
					ptr11 = new IntPtr((long)value10 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(ptr11, encryptParam.rgszKeyWrapObjId[num].DangerousGetHandle());
					if (num5 == 26114U)
					{
						IntPtr ptr12 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "pvKeyWrapAuxInfo"));
						Marshal.WriteIntPtr(ptr12, encryptParam.pvEncryptionAuxInfo.DangerousGetHandle());
					}
					IntPtr ptr13 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "dwKeyChoice"));
					Marshal.WriteInt32(ptr13, 1);
					IntPtr ptr14 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "pEphemeralAlgorithmOrSenderId"));
					encryptParam.rgEphemeralIdentifier[num] = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER))));
					Marshal.WriteIntPtr(ptr14, encryptParam.rgEphemeralIdentifier[num].DangerousGetHandle());
					bytes2 = Encoding.ASCII.GetBytes(cert_INFO.SubjectPublicKeyInfo.Algorithm.pszObjId);
					encryptParam.rgszEphemeralObjId[num] = CAPI.LocalAlloc(64U, new IntPtr(bytes2.Length + 1));
					Marshal.Copy(bytes2, 0, encryptParam.rgszEphemeralObjId[num].DangerousGetHandle(), bytes2.Length);
					ptr11 = new IntPtr((long)encryptParam.rgEphemeralIdentifier[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(ptr11, encryptParam.rgszEphemeralObjId[num].DangerousGetHandle());
					IntPtr value11 = new IntPtr((long)encryptParam.rgEphemeralIdentifier[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "Parameters"));
					IntPtr ptr15 = new IntPtr((long)value11 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
					Marshal.WriteInt32(ptr15, (int)cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.cbData);
					IntPtr ptr16 = new IntPtr((long)value11 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
					Marshal.WriteIntPtr(ptr16, cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.pbData);
					IntPtr ptr17 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "cRecipientEncryptedKeys"));
					Marshal.WriteInt32(ptr17, 1);
					encryptParam.prgpEncryptedKey[num] = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(IntPtr))));
					IntPtr ptr18 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "rgpRecipientEncryptedKeys"));
					Marshal.WriteIntPtr(ptr18, encryptParam.prgpEncryptedKey[num].DangerousGetHandle());
					encryptParam.rgpEncryptedKey[num] = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO))));
					Marshal.WriteIntPtr(encryptParam.prgpEncryptedKey[num].DangerousGetHandle(), encryptParam.rgpEncryptedKey[num].DangerousGetHandle());
					ptr10 = new IntPtr((long)encryptParam.rgpEncryptedKey[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO), "cbSize"));
					Marshal.WriteInt32(ptr10, Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO)));
					IntPtr value12 = new IntPtr((long)encryptParam.rgpEncryptedKey[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO), "RecipientPublicKey"));
					ptr15 = new IntPtr((long)value12 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cbData"));
					Marshal.WriteInt32(ptr15, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cbData);
					ptr16 = new IntPtr((long)value12 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "pbData"));
					Marshal.WriteIntPtr(ptr16, cert_INFO.SubjectPublicKeyInfo.PublicKey.pbData);
					IntPtr ptr19 = new IntPtr((long)value12 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cUnusedBits"));
					Marshal.WriteInt32(ptr19, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cUnusedBits);
					IntPtr value13 = new IntPtr((long)encryptParam.rgpEncryptedKey[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO), "RecipientId"));
					IntPtr ptr20 = new IntPtr((long)value13 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "dwIdChoice"));
					if (cmsRecipient.RecipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
					{
						Marshal.WriteInt32(ptr20, 2);
						IntPtr value14 = new IntPtr((long)value13 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						uint num6 = 0U;
						SafeLocalAllocHandle safeLocalAllocHandle2 = SafeLocalAllocHandle.InvalidHandle;
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle2, ref num6))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						safeLocalAllocHandle2 = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num6)));
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle2, ref num6))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						encryptParam.rgSubjectKeyIdentifier[num] = safeLocalAllocHandle2;
						ptr15 = new IntPtr((long)value14 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(ptr15, (int)num6);
						ptr16 = new IntPtr((long)value14 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(ptr16, safeLocalAllocHandle2.DangerousGetHandle());
					}
					else
					{
						Marshal.WriteInt32(ptr20, 1);
						IntPtr value15 = new IntPtr((long)value13 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						IntPtr value16 = new IntPtr((long)value15 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "Issuer"));
						ptr15 = new IntPtr((long)value16 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(ptr15, (int)cert_INFO.Issuer.cbData);
						ptr16 = new IntPtr((long)value16 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(ptr16, cert_INFO.Issuer.pbData);
						IntPtr value17 = new IntPtr((long)value15 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "SerialNumber"));
						ptr15 = new IntPtr((long)value17 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(ptr15, (int)cert_INFO.SerialNumber.cbData);
						ptr16 = new IntPtr((long)value17 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(ptr16, cert_INFO.SerialNumber.pbData);
					}
					num++;
					intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO)));
				}
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005FFC File Offset: 0x00004FFC
		private static void SetPkcs7RecipientParams(CmsRecipientCollection recipients, ref EnvelopedCms.CMSG_ENCRYPT_PARAM encryptParam, out List<SafeCertContextHandle> certContexts)
		{
			int count = recipients.Count;
			certContexts = new List<SafeCertContextHandle>();
			uint num = (uint)(count * Marshal.SizeOf(typeof(IntPtr)));
			encryptParam.rgpRecipients = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num)));
			IntPtr intPtr = encryptParam.rgpRecipients.DangerousGetHandle();
			for (int i = 0; i < count; i++)
			{
				SafeCertContextHandle certContext = X509Utils.GetCertContext(recipients[i].Certificate);
				certContexts.Add(certContext);
				IntPtr ptr = certContext.DangerousGetHandle();
				CAPIBase.CERT_CONTEXT cert_CONTEXT = (CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(ptr, typeof(CAPIBase.CERT_CONTEXT));
				Marshal.WriteIntPtr(intPtr, cert_CONTEXT.pCertInfo);
				intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(IntPtr)));
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000060C0 File Offset: 0x000050C0
		private static SafeCertStoreHandle BuildDecryptorStore(X509Certificate2Collection extraStore)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			try
			{
				X509Store x509Store = new X509Store("MY", StoreLocation.CurrentUser);
				x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store.Certificates);
			}
			catch (SecurityException)
			{
			}
			try
			{
				X509Store x509Store2 = new X509Store("MY", StoreLocation.LocalMachine);
				x509Store2.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store2.Certificates);
			}
			catch (SecurityException)
			{
			}
			if (extraStore != null)
			{
				x509Certificate2Collection.AddRange(extraStore);
			}
			if (x509Certificate2Collection.Count == 0)
			{
				throw new CryptographicException(-2146889717);
			}
			return X509Utils.ExportToMemoryStore(x509Certificate2Collection);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000615C File Offset: 0x0000515C
		private static SafeCertStoreHandle BuildOriginatorStore(X509Certificate2Collection bagOfCerts, X509Certificate2Collection extraStore)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			try
			{
				X509Store x509Store = new X509Store("AddressBook", StoreLocation.CurrentUser);
				x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store.Certificates);
			}
			catch (SecurityException)
			{
			}
			try
			{
				X509Store x509Store2 = new X509Store("AddressBook", StoreLocation.LocalMachine);
				x509Store2.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store2.Certificates);
			}
			catch (SecurityException)
			{
			}
			if (bagOfCerts != null)
			{
				x509Certificate2Collection.AddRange(bagOfCerts);
			}
			if (extraStore != null)
			{
				x509Certificate2Collection.AddRange(extraStore);
			}
			if (x509Certificate2Collection.Count == 0)
			{
				throw new CryptographicException(-2146885628);
			}
			return X509Utils.ExportToMemoryStore(x509Certificate2Collection);
		}

		// Token: 0x04000439 RID: 1081
		private SafeCryptMsgHandle m_safeCryptMsgHandle;

		// Token: 0x0400043A RID: 1082
		private int m_version;

		// Token: 0x0400043B RID: 1083
		private SubjectIdentifierType m_recipientIdentifierType;

		// Token: 0x0400043C RID: 1084
		private ContentInfo m_contentInfo;

		// Token: 0x0400043D RID: 1085
		private AlgorithmIdentifier m_encryptionAlgorithm;

		// Token: 0x0400043E RID: 1086
		private X509Certificate2Collection m_certificates;

		// Token: 0x0400043F RID: 1087
		private CryptographicAttributeObjectCollection m_unprotectedAttributes;

		// Token: 0x0200005D RID: 93
		private struct CMSG_DECRYPT_PARAM
		{
			// Token: 0x04000440 RID: 1088
			internal SafeCertContextHandle safeCertContextHandle;

			// Token: 0x04000441 RID: 1089
			internal SafeCryptProvHandle safeCryptProvHandle;

			// Token: 0x04000442 RID: 1090
			internal uint keySpec;
		}

		// Token: 0x0200005E RID: 94
		private struct CMSG_ENCRYPT_PARAM
		{
			// Token: 0x04000443 RID: 1091
			internal bool useCms;

			// Token: 0x04000444 RID: 1092
			internal SafeCryptProvHandle safeCryptProvHandle;

			// Token: 0x04000445 RID: 1093
			internal SafeLocalAllocHandle pvEncryptionAuxInfo;

			// Token: 0x04000446 RID: 1094
			internal SafeLocalAllocHandle rgpRecipients;

			// Token: 0x04000447 RID: 1095
			internal SafeLocalAllocHandle rgCertEncoded;

			// Token: 0x04000448 RID: 1096
			internal SafeLocalAllocHandle rgUnprotectedAttr;

			// Token: 0x04000449 RID: 1097
			internal SafeLocalAllocHandle[] rgSubjectKeyIdentifier;

			// Token: 0x0400044A RID: 1098
			internal SafeLocalAllocHandle[] rgszObjId;

			// Token: 0x0400044B RID: 1099
			internal SafeLocalAllocHandle[] rgszKeyWrapObjId;

			// Token: 0x0400044C RID: 1100
			internal SafeLocalAllocHandle[] rgKeyWrapAuxInfo;

			// Token: 0x0400044D RID: 1101
			internal SafeLocalAllocHandle[] rgEphemeralIdentifier;

			// Token: 0x0400044E RID: 1102
			internal SafeLocalAllocHandle[] rgszEphemeralObjId;

			// Token: 0x0400044F RID: 1103
			internal SafeLocalAllocHandle[] rgUserKeyingMaterial;

			// Token: 0x04000450 RID: 1104
			internal SafeLocalAllocHandle[] prgpEncryptedKey;

			// Token: 0x04000451 RID: 1105
			internal SafeLocalAllocHandle[] rgpEncryptedKey;
		}
	}
}
