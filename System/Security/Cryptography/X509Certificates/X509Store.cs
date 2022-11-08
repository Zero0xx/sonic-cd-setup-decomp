using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000344 RID: 836
	public sealed class X509Store
	{
		// Token: 0x06001A3C RID: 6716 RVA: 0x0005B61F File Offset: 0x0005A61F
		public X509Store() : this("MY", StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0005B62D File Offset: 0x0005A62D
		public X509Store(string storeName) : this(storeName, StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0005B637 File Offset: 0x0005A637
		public X509Store(StoreName storeName) : this(storeName, StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0005B641 File Offset: 0x0005A641
		public X509Store(StoreLocation storeLocation) : this("MY", storeLocation)
		{
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0005B650 File Offset: 0x0005A650
		public X509Store(StoreName storeName, StoreLocation storeLocation)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeLocation != StoreLocation.CurrentUser && storeLocation != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[]
				{
					"storeLocation"
				}));
			}
			switch (storeName)
			{
			case StoreName.AddressBook:
				this.m_storeName = "AddressBook";
				break;
			case StoreName.AuthRoot:
				this.m_storeName = "AuthRoot";
				break;
			case StoreName.CertificateAuthority:
				this.m_storeName = "CA";
				break;
			case StoreName.Disallowed:
				this.m_storeName = "Disallowed";
				break;
			case StoreName.My:
				this.m_storeName = "My";
				break;
			case StoreName.Root:
				this.m_storeName = "Root";
				break;
			case StoreName.TrustedPeople:
				this.m_storeName = "TrustedPeople";
				break;
			case StoreName.TrustedPublisher:
				this.m_storeName = "TrustedPublisher";
				break;
			default:
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[]
				{
					"storeName"
				}));
			}
			this.m_location = storeLocation;
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0005B768 File Offset: 0x0005A768
		public X509Store(string storeName, StoreLocation storeLocation)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeLocation != StoreLocation.CurrentUser && storeLocation != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[]
				{
					"storeLocation"
				}));
			}
			this.m_storeName = storeName;
			this.m_location = storeLocation;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0005B7C8 File Offset: 0x0005A7C8
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Store(IntPtr storeHandle)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeHandle == IntPtr.Zero)
			{
				throw new ArgumentNullException("storeHandle");
			}
			this.m_safeCertStoreHandle = CAPISafe.CertDuplicateStore(storeHandle);
			if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidStoreHandle"), "storeHandle");
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x0005B834 File Offset: 0x0005A834
		public IntPtr StoreHandle
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeCertStoreHandle.DangerousGetHandle();
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x0005B841 File Offset: 0x0005A841
		public StoreLocation Location
		{
			get
			{
				return this.m_location;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x0005B849 File Offset: 0x0005A849
		public string Name
		{
			get
			{
				return this.m_storeName;
			}
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0005B854 File Offset: 0x0005A854
		public void Open(OpenFlags flags)
		{
			if (this.m_location != StoreLocation.CurrentUser && this.m_location != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[]
				{
					"m_location"
				}));
			}
			uint dwFlags = X509Utils.MapX509StoreFlags(this.m_location, flags);
			if (!this.m_safeCertStoreHandle.IsInvalid)
			{
				this.m_safeCertStoreHandle.Dispose();
			}
			this.m_safeCertStoreHandle = CAPI.CertOpenStore(new IntPtr(10L), 65537U, IntPtr.Zero, dwFlags, this.m_storeName);
			if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPISafe.CertControlStore(this.m_safeCertStoreHandle, 0U, 4U, IntPtr.Zero);
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0005B919 File Offset: 0x0005A919
		public void Close()
		{
			if (this.m_safeCertStoreHandle != null && !this.m_safeCertStoreHandle.IsClosed)
			{
				this.m_safeCertStoreHandle.Dispose();
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0005B93B File Offset: 0x0005A93B
		public void Add(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (!CAPI.CertAddCertificateContextToStore(this.m_safeCertStoreHandle, certificate.CertContext, 5U, SafeCertContextHandle.InvalidHandle))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x0005B970 File Offset: 0x0005A970
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

		// Token: 0x06001A4A RID: 6730 RVA: 0x0005B9E0 File Offset: 0x0005A9E0
		public void Remove(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			X509Store.RemoveCertificateFromStore(this.m_safeCertStoreHandle, certificate.CertContext);
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0005BA04 File Offset: 0x0005AA04
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

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x0005BA74 File Offset: 0x0005AA74
		public X509Certificate2Collection Certificates
		{
			get
			{
				if (this.m_safeCertStoreHandle.IsInvalid || this.m_safeCertStoreHandle.IsClosed)
				{
					return new X509Certificate2Collection();
				}
				return X509Utils.GetCertificates(this.m_safeCertStoreHandle);
			}
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x0005BAA4 File Offset: 0x0005AAA4
		private static void RemoveCertificateFromStore(SafeCertStoreHandle safeCertStoreHandle, SafeCertContextHandle safeCertContext)
		{
			if (safeCertContext == null || safeCertContext.IsInvalid)
			{
				return;
			}
			SafeCertContextHandle safeCertContextHandle = CAPI.CertFindCertificateInStore(safeCertStoreHandle, 65537U, 0U, 851968U, safeCertContext.DangerousGetHandle(), SafeCertContextHandle.InvalidHandle);
			if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
			{
				return;
			}
			GC.SuppressFinalize(safeCertContextHandle);
			if (!CAPI.CertDeleteCertificateFromStore(safeCertContextHandle))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x04001B33 RID: 6963
		private string m_storeName;

		// Token: 0x04001B34 RID: 6964
		private StoreLocation m_location;

		// Token: 0x04001B35 RID: 6965
		private SafeCertStoreHandle m_safeCertStoreHandle;
	}
}
