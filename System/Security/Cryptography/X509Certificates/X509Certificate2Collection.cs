﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200032B RID: 811
	public class X509Certificate2Collection : X509CertificateCollection
	{
		// Token: 0x06001994 RID: 6548 RVA: 0x000580BB File Offset: 0x000570BB
		public X509Certificate2Collection()
		{
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000580C3 File Offset: 0x000570C3
		public X509Certificate2Collection(X509Certificate2 certificate)
		{
			this.Add(certificate);
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x000580D3 File Offset: 0x000570D3
		public X509Certificate2Collection(X509Certificate2Collection certificates)
		{
			this.AddRange(certificates);
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x000580E2 File Offset: 0x000570E2
		public X509Certificate2Collection(X509Certificate2[] certificates)
		{
			this.AddRange(certificates);
		}

		// Token: 0x170004E6 RID: 1254
		public X509Certificate2 this[int index]
		{
			get
			{
				return (X509Certificate2)base.List[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.List[index] = value;
			}
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00058121 File Offset: 0x00057121
		public int Add(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			return base.List.Add(certificate);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00058140 File Offset: 0x00057140
		public void AddRange(X509Certificate2[] certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int i = 0;
			try
			{
				while (i < certificates.Length)
				{
					this.Add(certificates[i]);
					i++;
				}
			}
			catch
			{
				for (int j = 0; j < i; j++)
				{
					this.Remove(certificates[j]);
				}
				throw;
			}
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x000581A0 File Offset: 0x000571A0
		public void AddRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int num = 0;
			try
			{
				foreach (X509Certificate2 certificate in certificates)
				{
					this.Add(certificate);
					num++;
				}
			}
			catch
			{
				for (int i = 0; i < num; i++)
				{
					this.Remove(certificates[i]);
				}
				throw;
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00058210 File Offset: 0x00057210
		public bool Contains(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			return base.List.Contains(certificate);
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0005822C File Offset: 0x0005722C
		public void Insert(int index, X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			base.List.Insert(index, certificate);
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00058249 File Offset: 0x00057249
		public new X509Certificate2Enumerator GetEnumerator()
		{
			return new X509Certificate2Enumerator(this);
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00058251 File Offset: 0x00057251
		public void Remove(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			base.List.Remove(certificate);
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00058270 File Offset: 0x00057270
		public void RemoveRange(X509Certificate2[] certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int i = 0;
			try
			{
				while (i < certificates.Length)
				{
					this.Remove(certificates[i]);
					i++;
				}
			}
			catch
			{
				for (int j = 0; j < i; j++)
				{
					this.Add(certificates[j]);
				}
				throw;
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x000582D0 File Offset: 0x000572D0
		public void RemoveRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int num = 0;
			try
			{
				foreach (X509Certificate2 certificate in certificates)
				{
					this.Remove(certificate);
					num++;
				}
			}
			catch
			{
				for (int i = 0; i < num; i++)
				{
					this.Add(certificates[i]);
				}
				throw;
			}
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x00058340 File Offset: 0x00057340
		public X509Certificate2Collection Find(X509FindType findType, object findValue, bool validOnly)
		{
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AllFlags);
			storePermission.Assert();
			SafeCertStoreHandle safeCertStoreHandle = X509Utils.ExportToMemoryStore(this);
			SafeCertStoreHandle safeCertStoreHandle2 = X509Certificate2Collection.FindCertInStore(safeCertStoreHandle, findType, findValue, validOnly);
			X509Certificate2Collection certificates = X509Utils.GetCertificates(safeCertStoreHandle2);
			safeCertStoreHandle2.Dispose();
			safeCertStoreHandle.Dispose();
			return certificates;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00058383 File Offset: 0x00057383
		public void Import(byte[] rawData)
		{
			this.Import(rawData, null, X509KeyStorageFlags.DefaultKeySet);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00058390 File Offset: 0x00057390
		public void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			uint dwFlags = X509Utils.MapKeyStorageFlags(keyStorageFlags);
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AllFlags);
			storePermission.Assert();
			safeCertStoreHandle = X509Certificate2Collection.LoadStoreFromBlob(rawData, password, dwFlags, (keyStorageFlags & X509KeyStorageFlags.PersistKeySet) != X509KeyStorageFlags.DefaultKeySet);
			X509Certificate2Collection certificates = X509Utils.GetCertificates(safeCertStoreHandle);
			safeCertStoreHandle.Dispose();
			X509Certificate2[] array = new X509Certificate2[certificates.Count];
			certificates.CopyTo(array, 0);
			this.AddRange(array);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x000583F9 File Offset: 0x000573F9
		public void Import(string fileName)
		{
			this.Import(fileName, null, X509KeyStorageFlags.DefaultKeySet);
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00058404 File Offset: 0x00057404
		public void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			uint dwFlags = X509Utils.MapKeyStorageFlags(keyStorageFlags);
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AllFlags);
			storePermission.Assert();
			safeCertStoreHandle = X509Certificate2Collection.LoadStoreFromFile(fileName, password, dwFlags, (keyStorageFlags & X509KeyStorageFlags.PersistKeySet) != X509KeyStorageFlags.DefaultKeySet);
			X509Certificate2Collection certificates = X509Utils.GetCertificates(safeCertStoreHandle);
			safeCertStoreHandle.Dispose();
			X509Certificate2[] array = new X509Certificate2[certificates.Count];
			certificates.CopyTo(array, 0);
			this.AddRange(array);
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0005846D File Offset: 0x0005746D
		public byte[] Export(X509ContentType contentType)
		{
			return this.Export(contentType, null);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x00058478 File Offset: 0x00057478
		public byte[] Export(X509ContentType contentType, string password)
		{
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AllFlags);
			storePermission.Assert();
			SafeCertStoreHandle safeCertStoreHandle = X509Utils.ExportToMemoryStore(this);
			byte[] result = X509Certificate2Collection.ExportCertificatesToBlob(safeCertStoreHandle, contentType, password);
			safeCertStoreHandle.Dispose();
			return result;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x000584B0 File Offset: 0x000574B0
		private unsafe static byte[] ExportCertificatesToBlob(SafeCertStoreHandle safeCertStoreHandle, X509ContentType contentType, string password)
		{
			SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
			uint dwSaveAs = 2U;
			byte[] array = null;
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			switch (contentType)
			{
			case X509ContentType.Cert:
				safeCertContextHandle = CAPI.CertEnumCertificatesInStore(safeCertStoreHandle, safeCertContextHandle);
				if (safeCertContextHandle != null && !safeCertContextHandle.IsInvalid)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
					array = new byte[cert_CONTEXT.cbCertEncoded];
					Marshal.Copy(cert_CONTEXT.pbCertEncoded, array, 0, array.Length);
				}
				break;
			case X509ContentType.SerializedCert:
			{
				safeCertContextHandle = CAPI.CertEnumCertificatesInStore(safeCertStoreHandle, safeCertContextHandle);
				uint num = 0U;
				if (safeCertContextHandle != null && !safeCertContextHandle.IsInvalid)
				{
					if (!CAPISafe.CertSerializeCertificateStoreElement(safeCertContextHandle, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
					if (!CAPISafe.CertSerializeCertificateStoreElement(safeCertContextHandle, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					array = new byte[num];
					Marshal.Copy(safeLocalAllocHandle.DangerousGetHandle(), array, 0, array.Length);
				}
				break;
			}
			case X509ContentType.Pfx:
				if (!CAPI.PFXExportCertStore(safeCertStoreHandle, new IntPtr((void*)(&cryptoapi_BLOB)), password, 6U))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)cryptoapi_BLOB.cbData)));
				cryptoapi_BLOB.pbData = safeLocalAllocHandle.DangerousGetHandle();
				if (!CAPI.PFXExportCertStore(safeCertStoreHandle, new IntPtr((void*)(&cryptoapi_BLOB)), password, 6U))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				array = new byte[cryptoapi_BLOB.cbData];
				Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, array.Length);
				break;
			case X509ContentType.SerializedStore:
			case X509ContentType.Pkcs7:
				if (contentType == X509ContentType.SerializedStore)
				{
					dwSaveAs = 1U;
				}
				if (!CAPI.CertSaveStore(safeCertStoreHandle, 65537U, dwSaveAs, 2U, new IntPtr((void*)(&cryptoapi_BLOB)), 0U))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)cryptoapi_BLOB.cbData)));
				cryptoapi_BLOB.pbData = safeLocalAllocHandle.DangerousGetHandle();
				if (!CAPI.CertSaveStore(safeCertStoreHandle, 65537U, dwSaveAs, 2U, new IntPtr((void*)(&cryptoapi_BLOB)), 0U))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				array = new byte[cryptoapi_BLOB.cbData];
				Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, array.Length);
				break;
			default:
				throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidContentType"));
			}
			safeLocalAllocHandle.Dispose();
			safeCertContextHandle.Dispose();
			return array;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00058700 File Offset: 0x00057700
		private unsafe static SafeCertStoreHandle FindCertInStore(SafeCertStoreHandle safeSourceStoreHandle, X509FindType findType, object findValue, bool validOnly)
		{
			if (findValue == null)
			{
				throw new ArgumentNullException("findValue");
			}
			IntPtr pvFindPara = IntPtr.Zero;
			object obj = null;
			object pvCallbackData = null;
			X509Certificate2Collection.FindProcDelegate pfnCertCallback = null;
			X509Certificate2Collection.FindProcDelegate pfnCertCallback2 = null;
			uint dwFindType = 0U;
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			System.Runtime.InteropServices.ComTypes.FILETIME filetime = default(System.Runtime.InteropServices.ComTypes.FILETIME);
			switch (findType)
			{
			case X509FindType.FindByThumbprint:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				byte[] array = X509Utils.DecodeHexString((string)findValue);
				safeLocalAllocHandle = X509Utils.ByteToPtr(array);
				cryptoapi_BLOB.pbData = safeLocalAllocHandle.DangerousGetHandle();
				cryptoapi_BLOB.cbData = (uint)array.Length;
				dwFindType = 65536U;
				pvFindPara = new IntPtr((void*)(&cryptoapi_BLOB));
				break;
			}
			case X509FindType.FindBySubjectName:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				string text = (string)findValue;
				dwFindType = 524295U;
				safeLocalAllocHandle = X509Utils.StringToUniPtr(text);
				pvFindPara = safeLocalAllocHandle.DangerousGetHandle();
				break;
			}
			case X509FindType.FindBySubjectDistinguishedName:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				string text = (string)findValue;
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindSubjectDistinguishedNameCallback);
				obj = text;
				break;
			}
			case X509FindType.FindByIssuerName:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				string text2 = (string)findValue;
				dwFindType = 524292U;
				safeLocalAllocHandle = X509Utils.StringToUniPtr(text2);
				pvFindPara = safeLocalAllocHandle.DangerousGetHandle();
				break;
			}
			case X509FindType.FindByIssuerDistinguishedName:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				string text2 = (string)findValue;
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindIssuerDistinguishedNameCallback);
				obj = text2;
				break;
			}
			case X509FindType.FindBySerialNumber:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindSerialNumberCallback);
				pfnCertCallback2 = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindSerialNumberCallback);
				BigInt bigInt = new BigInt();
				bigInt.FromHexadecimal((string)findValue);
				obj = bigInt.ToByteArray();
				bigInt.FromDecimal((string)findValue);
				pvCallbackData = bigInt.ToByteArray();
				break;
			}
			case X509FindType.FindByTimeValid:
				if (findValue.GetType() != typeof(DateTime))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				*(long*)(&filetime) = ((DateTime)findValue).ToFileTime();
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindTimeValidCallback);
				obj = filetime;
				break;
			case X509FindType.FindByTimeNotYetValid:
				if (findValue.GetType() != typeof(DateTime))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				*(long*)(&filetime) = ((DateTime)findValue).ToFileTime();
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindTimeNotBeforeCallback);
				obj = filetime;
				break;
			case X509FindType.FindByTimeExpired:
				if (findValue.GetType() != typeof(DateTime))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				*(long*)(&filetime) = ((DateTime)findValue).ToFileTime();
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindTimeNotAfterCallback);
				obj = filetime;
				break;
			case X509FindType.FindByTemplateName:
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				obj = (string)findValue;
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindTemplateNameCallback);
				break;
			case X509FindType.FindByApplicationPolicy:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				string text3 = X509Utils.FindOidInfo(2U, (string)findValue, OidGroup.Policy);
				if (text3 == null)
				{
					text3 = (string)findValue;
					X509Utils.ValidateOidValue(text3);
				}
				obj = text3;
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindApplicationPolicyCallback);
				break;
			}
			case X509FindType.FindByCertificatePolicy:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				string text3 = X509Utils.FindOidInfo(2U, (string)findValue, OidGroup.Policy);
				if (text3 == null)
				{
					text3 = (string)findValue;
					X509Utils.ValidateOidValue(text3);
				}
				obj = text3;
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindCertificatePolicyCallback);
				break;
			}
			case X509FindType.FindByExtension:
			{
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				string text3 = X509Utils.FindOidInfo(2U, (string)findValue, OidGroup.ExtensionOrAttribute);
				if (text3 == null)
				{
					text3 = (string)findValue;
					X509Utils.ValidateOidValue(text3);
				}
				obj = text3;
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindExtensionCallback);
				break;
			}
			case X509FindType.FindByKeyUsage:
				if (findValue.GetType() == typeof(string))
				{
					CAPIBase.KEY_USAGE_STRUCT[] array2 = new CAPIBase.KEY_USAGE_STRUCT[]
					{
						new CAPIBase.KEY_USAGE_STRUCT("DigitalSignature", 128U),
						new CAPIBase.KEY_USAGE_STRUCT("NonRepudiation", 64U),
						new CAPIBase.KEY_USAGE_STRUCT("KeyEncipherment", 32U),
						new CAPIBase.KEY_USAGE_STRUCT("DataEncipherment", 16U),
						new CAPIBase.KEY_USAGE_STRUCT("KeyAgreement", 8U),
						new CAPIBase.KEY_USAGE_STRUCT("KeyCertSign", 4U),
						new CAPIBase.KEY_USAGE_STRUCT("CrlSign", 2U),
						new CAPIBase.KEY_USAGE_STRUCT("EncipherOnly", 1U),
						new CAPIBase.KEY_USAGE_STRUCT("DecipherOnly", 32768U)
					};
					uint num = 0U;
					while ((ulong)num < (ulong)((long)array2.Length))
					{
						if (string.Compare(array2[(int)((UIntPtr)num)].pwszKeyUsage, (string)findValue, StringComparison.OrdinalIgnoreCase) == 0)
						{
							obj = array2[(int)((UIntPtr)num)].dwKeyUsageBit;
							break;
						}
						num += 1U;
					}
					if (obj == null)
					{
						throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindType"));
					}
				}
				else if (findValue.GetType() == typeof(X509KeyUsageFlags))
				{
					obj = findValue;
				}
				else
				{
					if (findValue.GetType() != typeof(uint) && findValue.GetType() != typeof(int))
					{
						throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindType"));
					}
					obj = findValue;
				}
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindKeyUsageCallback);
				break;
			case X509FindType.FindBySubjectKeyIdentifier:
				if (findValue.GetType() != typeof(string))
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindValue"));
				}
				obj = X509Utils.DecodeHexString((string)findValue);
				pfnCertCallback = new X509Certificate2Collection.FindProcDelegate(X509Certificate2Collection.FindSubjectKeyIdentifierCallback);
				break;
			default:
				throw new CryptographicException(SR.GetString("Cryptography_X509_InvalidFindType"));
			}
			SafeCertStoreHandle safeCertStoreHandle = CAPI.CertOpenStore(new IntPtr(2L), 65537U, IntPtr.Zero, 8704U, null);
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			X509Certificate2Collection.FindByCert(safeSourceStoreHandle, dwFindType, pvFindPara, validOnly, pfnCertCallback, pfnCertCallback2, obj, pvCallbackData, safeCertStoreHandle);
			safeLocalAllocHandle.Dispose();
			return safeCertStoreHandle;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00058E08 File Offset: 0x00057E08
		private static void FindByCert(SafeCertStoreHandle safeSourceStoreHandle, uint dwFindType, IntPtr pvFindPara, bool validOnly, X509Certificate2Collection.FindProcDelegate pfnCertCallback1, X509Certificate2Collection.FindProcDelegate pfnCertCallback2, object pvCallbackData1, object pvCallbackData2, SafeCertStoreHandle safeTargetStoreHandle)
		{
			int num = 0;
			SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
			safeCertContextHandle = CAPI.CertFindCertificateInStore(safeSourceStoreHandle, 65537U, 0U, dwFindType, pvFindPara, safeCertContextHandle);
			while (safeCertContextHandle != null && !safeCertContextHandle.IsInvalid)
			{
				if (pfnCertCallback1 == null)
				{
					goto IL_46;
				}
				num = pfnCertCallback1(safeCertContextHandle, pvCallbackData1);
				if (num == 1)
				{
					if (pfnCertCallback2 != null)
					{
						num = pfnCertCallback2(safeCertContextHandle, pvCallbackData2);
					}
					if (num == 1)
					{
						goto IL_8D;
					}
				}
				if (num == 0)
				{
					goto IL_46;
				}
				break;
				IL_8D:
				GC.SuppressFinalize(safeCertContextHandle);
				safeCertContextHandle = CAPI.CertFindCertificateInStore(safeSourceStoreHandle, 65537U, 0U, dwFindType, pvFindPara, safeCertContextHandle);
				continue;
				IL_46:
				if (validOnly)
				{
					num = X509Utils.VerifyCertificate(safeCertContextHandle, null, null, X509RevocationMode.NoCheck, X509RevocationFlag.ExcludeRoot, DateTime.Now, new TimeSpan(0, 0, 0), null, new IntPtr(1L), IntPtr.Zero);
					if (num == 1)
					{
						goto IL_8D;
					}
					if (num != 0)
					{
						break;
					}
				}
				if (!CAPI.CertAddCertificateLinkToStore(safeTargetStoreHandle, safeCertContextHandle, 4U, SafeCertContextHandle.InvalidHandle))
				{
					num = Marshal.GetHRForLastWin32Error();
					break;
				}
				goto IL_8D;
			}
			if (safeCertContextHandle != null && !safeCertContextHandle.IsInvalid)
			{
				safeCertContextHandle.Dispose();
			}
			if (num != 1 && num != 0)
			{
				throw new CryptographicException(num);
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00058EE8 File Offset: 0x00057EE8
		private static int FindSubjectDistinguishedNameCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			string certNameInfo = CAPI.GetCertNameInfo(safeCertContextHandle, 0U, 2U);
			if (string.Compare(certNameInfo, (string)pvCallbackData, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00058F10 File Offset: 0x00057F10
		private static int FindIssuerDistinguishedNameCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			string certNameInfo = CAPI.GetCertNameInfo(safeCertContextHandle, 1U, 2U);
			if (string.Compare(certNameInfo, (string)pvCallbackData, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00058F38 File Offset: 0x00057F38
		private unsafe static int FindSerialNumberCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			byte[] array = new byte[cert_INFO.SerialNumber.cbData];
			Marshal.Copy(cert_INFO.SerialNumber.pbData, array, 0, array.Length);
			int hexArraySize = X509Utils.GetHexArraySize(array);
			byte[] array2 = (byte[])pvCallbackData;
			if (array2.Length != hexArraySize)
			{
				return 1;
			}
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i] != array[i])
				{
					return 1;
				}
			}
			return 0;
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00058FD4 File Offset: 0x00057FD4
		private unsafe static int FindTimeValidCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			System.Runtime.InteropServices.ComTypes.FILETIME filetime = (System.Runtime.InteropServices.ComTypes.FILETIME)pvCallbackData;
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			if (CAPISafe.CertVerifyTimeValidity(ref filetime, cert_CONTEXT.pCertInfo) == 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0005900C File Offset: 0x0005800C
		private unsafe static int FindTimeNotAfterCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			System.Runtime.InteropServices.ComTypes.FILETIME filetime = (System.Runtime.InteropServices.ComTypes.FILETIME)pvCallbackData;
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			if (CAPISafe.CertVerifyTimeValidity(ref filetime, cert_CONTEXT.pCertInfo) == 1)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00059048 File Offset: 0x00058048
		private unsafe static int FindTimeNotBeforeCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			System.Runtime.InteropServices.ComTypes.FILETIME filetime = (System.Runtime.InteropServices.ComTypes.FILETIME)pvCallbackData;
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			if (CAPISafe.CertVerifyTimeValidity(ref filetime, cert_CONTEXT.pCertInfo) == -1)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00059084 File Offset: 0x00058084
		private unsafe static int FindTemplateNameCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			intPtr = CAPISafe.CertFindExtension("1.3.6.1.4.1.311.20.2", cert_INFO.cExtension, cert_INFO.rgExtension);
			intPtr2 = CAPISafe.CertFindExtension("1.3.6.1.4.1.311.21.7", cert_INFO.cExtension, cert_INFO.rgExtension);
			if (intPtr == IntPtr.Zero && intPtr2 == IntPtr.Zero)
			{
				return 1;
			}
			if (intPtr != IntPtr.Zero)
			{
				CAPIBase.CERT_EXTENSION cert_EXTENSION = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CERT_EXTENSION));
				byte[] array = new byte[cert_EXTENSION.Value.cbData];
				Marshal.Copy(cert_EXTENSION.Value.pbData, array, 0, array.Length);
				uint num = 0U;
				SafeLocalAllocHandle safeLocalAllocHandle = null;
				bool flag = CAPI.DecodeObject(new IntPtr(24L), array, out safeLocalAllocHandle, out num);
				if (flag)
				{
					string strA = Marshal.PtrToStringUni(((CAPIBase.CERT_NAME_VALUE)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_NAME_VALUE))).Value.pbData);
					if (string.Compare(strA, (string)pvCallbackData, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return 0;
					}
				}
			}
			if (intPtr2 != IntPtr.Zero)
			{
				CAPIBase.CERT_EXTENSION cert_EXTENSION2 = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(intPtr2, typeof(CAPIBase.CERT_EXTENSION));
				byte[] array2 = new byte[cert_EXTENSION2.Value.cbData];
				Marshal.Copy(cert_EXTENSION2.Value.pbData, array2, 0, array2.Length);
				uint num2 = 0U;
				SafeLocalAllocHandle safeLocalAllocHandle2 = null;
				bool flag2 = CAPI.DecodeObject(new IntPtr(64L), array2, out safeLocalAllocHandle2, out num2);
				if (flag2)
				{
					CAPIBase.CERT_TEMPLATE_EXT cert_TEMPLATE_EXT = (CAPIBase.CERT_TEMPLATE_EXT)Marshal.PtrToStructure(safeLocalAllocHandle2.DangerousGetHandle(), typeof(CAPIBase.CERT_TEMPLATE_EXT));
					string text = X509Utils.FindOidInfo(2U, (string)pvCallbackData, OidGroup.Template);
					if (text == null)
					{
						text = (string)pvCallbackData;
					}
					if (string.Compare(cert_TEMPLATE_EXT.pszObjId, text, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return 0;
					}
				}
			}
			return 1;
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0005928C File Offset: 0x0005828C
		private unsafe static int FindApplicationPolicyCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			string text = (string)pvCallbackData;
			if (text.Length == 0)
			{
				return 1;
			}
			IntPtr intPtr = safeCertContextHandle.DangerousGetHandle();
			int num = 0;
			uint num2 = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CertGetValidUsages(1U, new IntPtr((void*)(&intPtr)), new IntPtr((void*)(&num)), safeLocalAllocHandle, new IntPtr((void*)(&num2))))
			{
				return 1;
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num2)));
			if (!CAPISafe.CertGetValidUsages(1U, new IntPtr((void*)(&intPtr)), new IntPtr((void*)(&num)), safeLocalAllocHandle, new IntPtr((void*)(&num2))))
			{
				return 1;
			}
			if (num == -1)
			{
				return 0;
			}
			for (int i = 0; i < num; i++)
			{
				IntPtr ptr = Marshal.ReadIntPtr(new IntPtr((long)safeLocalAllocHandle.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(IntPtr)))));
				string strB = Marshal.PtrToStringAnsi(ptr);
				if (string.Compare(text, strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return 0;
				}
			}
			return 1;
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0005936C File Offset: 0x0005836C
		private unsafe static int FindCertificatePolicyCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			string text = (string)pvCallbackData;
			if (text.Length == 0)
			{
				return 1;
			}
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			IntPtr intPtr = CAPISafe.CertFindExtension("2.5.29.32", cert_INFO.cExtension, cert_INFO.rgExtension);
			if (intPtr == IntPtr.Zero)
			{
				return 1;
			}
			CAPIBase.CERT_EXTENSION cert_EXTENSION = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CERT_EXTENSION));
			byte[] array = new byte[cert_EXTENSION.Value.cbData];
			Marshal.Copy(cert_EXTENSION.Value.pbData, array, 0, array.Length);
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			bool flag = CAPI.DecodeObject(new IntPtr(16L), array, out safeLocalAllocHandle, out num);
			if (flag)
			{
				CAPIBase.CERT_POLICIES_INFO cert_POLICIES_INFO = (CAPIBase.CERT_POLICIES_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_POLICIES_INFO));
				int num2 = 0;
				while ((long)num2 < (long)((ulong)cert_POLICIES_INFO.cPolicyInfo))
				{
					IntPtr ptr = new IntPtr((long)cert_POLICIES_INFO.rgPolicyInfo + (long)(num2 * Marshal.SizeOf(typeof(CAPIBase.CERT_POLICY_INFO))));
					if (string.Compare(text, ((CAPIBase.CERT_POLICY_INFO)Marshal.PtrToStructure(ptr, typeof(CAPIBase.CERT_POLICY_INFO))).pszPolicyIdentifier, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return 0;
					}
					num2++;
				}
			}
			return 1;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000594CC File Offset: 0x000584CC
		private unsafe static int FindExtensionCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			IntPtr value = CAPISafe.CertFindExtension((string)pvCallbackData, cert_INFO.cExtension, cert_INFO.rgExtension);
			if (value == IntPtr.Zero)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00059530 File Offset: 0x00058530
		private unsafe static int FindKeyUsageCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)((void*)safeCertContextHandle.DangerousGetHandle());
			uint num = 0U;
			if (!CAPISafe.CertGetIntendedKeyUsage(65537U, cert_CONTEXT.pCertInfo, new IntPtr((void*)(&num)), 4U))
			{
				return 0;
			}
			uint num2 = Convert.ToUInt32(pvCallbackData, null);
			if ((num & num2) == num2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x00059580 File Offset: 0x00058580
		private static int FindSubjectKeyIdentifierCallback(SafeCertContextHandle safeCertContextHandle, object pvCallbackData)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			uint num = 0U;
			if (!CAPISafe.CertGetCertificateContextProperty(safeCertContextHandle, 20U, safeLocalAllocHandle, ref num))
			{
				return 1;
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
			if (!CAPISafe.CertGetCertificateContextProperty(safeCertContextHandle, 20U, safeLocalAllocHandle, ref num))
			{
				return 1;
			}
			byte[] array = (byte[])pvCallbackData;
			if ((long)array.Length != (long)((ulong)num))
			{
				return 1;
			}
			byte[] array2 = new byte[num];
			Marshal.Copy(safeLocalAllocHandle.DangerousGetHandle(), array2, 0, array2.Length);
			safeLocalAllocHandle.Dispose();
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				if (array[(int)((UIntPtr)num2)] != array2[(int)((UIntPtr)num2)])
				{
					return 1;
				}
			}
			return 0;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00059610 File Offset: 0x00058610
		private unsafe static SafeCertStoreHandle LoadStoreFromBlob(byte[] rawData, string password, uint dwFlags, bool persistKeyContainers)
		{
			uint num = 0U;
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			if (!CAPI.CryptQueryObject(2U, rawData, 5938U, 14U, 0U, IntPtr.Zero, new IntPtr((void*)(&num)), IntPtr.Zero, ref safeCertStoreHandle, IntPtr.Zero, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (num == 12U)
			{
				safeCertStoreHandle.Dispose();
				safeCertStoreHandle = CAPI.PFXImportCertStore(2U, rawData, password, dwFlags, persistKeyContainers);
			}
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return safeCertStoreHandle;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0005968C File Offset: 0x0005868C
		private unsafe static SafeCertStoreHandle LoadStoreFromFile(string fileName, string password, uint dwFlags, bool persistKeyContainers)
		{
			uint num = 0U;
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			if (!CAPI.CryptQueryObject(1U, fileName, 5938U, 14U, 0U, IntPtr.Zero, new IntPtr((void*)(&num)), IntPtr.Zero, ref safeCertStoreHandle, IntPtr.Zero, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (num == 12U)
			{
				safeCertStoreHandle.Dispose();
				safeCertStoreHandle = CAPI.PFXImportCertStore(1U, fileName, password, dwFlags, persistKeyContainers);
			}
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return safeCertStoreHandle;
		}

		// Token: 0x04001ABC RID: 6844
		private const uint X509_STORE_CONTENT_FLAGS = 5938U;

		// Token: 0x0200032C RID: 812
		// (Invoke) Token: 0x060019BC RID: 6588
		internal delegate int FindProcDelegate(SafeCertContextHandle safeCertContextHandle, object pvCallbackData);
	}
}
