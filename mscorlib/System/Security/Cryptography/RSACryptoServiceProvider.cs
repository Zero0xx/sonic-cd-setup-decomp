using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x0200089C RID: 2204
	[ComVisible(true)]
	public sealed class RSACryptoServiceProvider : RSA, ICspAsymmetricAlgorithm
	{
		// Token: 0x0600501D RID: 20509 RVA: 0x00118D83 File Offset: 0x00117D83
		public RSACryptoServiceProvider() : this(0, new CspParameters(Utils.DefaultRsaProviderType, null, null, RSACryptoServiceProvider.s_UseMachineKeyStore), true)
		{
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x00118D9E File Offset: 0x00117D9E
		public RSACryptoServiceProvider(int dwKeySize) : this(dwKeySize, new CspParameters(Utils.DefaultRsaProviderType, null, null, RSACryptoServiceProvider.s_UseMachineKeyStore), false)
		{
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x00118DB9 File Offset: 0x00117DB9
		public RSACryptoServiceProvider(CspParameters parameters) : this(0, parameters, true)
		{
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x00118DC4 File Offset: 0x00117DC4
		public RSACryptoServiceProvider(int dwKeySize, CspParameters parameters) : this(dwKeySize, parameters, false)
		{
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x00118DD0 File Offset: 0x00117DD0
		private RSACryptoServiceProvider(int dwKeySize, CspParameters parameters, bool useDefaultKeySize)
		{
			if (dwKeySize < 0)
			{
				throw new ArgumentOutOfRangeException("dwKeySize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			bool flag = (parameters.Flags & (CspProviderFlags)(-2147483648)) != CspProviderFlags.NoFlags;
			parameters.Flags &= (CspProviderFlags)2147483647;
			this._parameters = Utils.SaveCspParameters(CspAlgorithmType.Rsa, parameters, RSACryptoServiceProvider.s_UseMachineKeyStore, ref this._randomKeyContainer);
			if (this._parameters.KeyNumber == 2 || Utils.HasEnhProv == 1)
			{
				this.LegalKeySizesValue = new KeySizes[]
				{
					new KeySizes(384, 16384, 8)
				};
				if (useDefaultKeySize)
				{
					this._dwKeySize = 1024;
				}
			}
			else
			{
				this.LegalKeySizesValue = new KeySizes[]
				{
					new KeySizes(384, 512, 8)
				};
				if (useDefaultKeySize)
				{
					this._dwKeySize = 512;
				}
			}
			if (!useDefaultKeySize)
			{
				this._dwKeySize = dwKeySize;
			}
			if (!this._randomKeyContainer || Environment.GetCompatibilityFlag(CompatibilityFlag.EagerlyGenerateRandomAsymmKeys))
			{
				this.GetKeyPair();
			}
			this._randomKeyContainer = (this._randomKeyContainer || flag);
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x00118EE0 File Offset: 0x00117EE0
		private void GetKeyPair()
		{
			if (this._safeKeyHandle == null)
			{
				lock (this)
				{
					if (this._safeKeyHandle == null)
					{
						Utils.GetKeyPairHelper(CspAlgorithmType.Rsa, this._parameters, this._randomKeyContainer, this._dwKeySize, ref this._safeProvHandle, ref this._safeKeyHandle);
					}
				}
			}
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x00118F44 File Offset: 0x00117F44
		protected override void Dispose(bool disposing)
		{
			if (this._safeKeyHandle != null && !this._safeKeyHandle.IsClosed)
			{
				this._safeKeyHandle.Dispose();
			}
			if (this._safeProvHandle != null && !this._safeProvHandle.IsClosed)
			{
				this._safeProvHandle.Dispose();
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06005024 RID: 20516 RVA: 0x00118F94 File Offset: 0x00117F94
		[ComVisible(false)]
		public bool PublicOnly
		{
			get
			{
				this.GetKeyPair();
				byte[] array = Utils._GetKeyParameter(this._safeKeyHandle, 2U);
				return array[0] == 1;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x00118FBA File Offset: 0x00117FBA
		[ComVisible(false)]
		public CspKeyContainerInfo CspKeyContainerInfo
		{
			get
			{
				this.GetKeyPair();
				return new CspKeyContainerInfo(this._parameters, this._randomKeyContainer);
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06005026 RID: 20518 RVA: 0x00118FD4 File Offset: 0x00117FD4
		public override int KeySize
		{
			get
			{
				this.GetKeyPair();
				byte[] array = Utils._GetKeyParameter(this._safeKeyHandle, 1U);
				this._dwKeySize = ((int)array[0] | (int)array[1] << 8 | (int)array[2] << 16 | (int)array[3] << 24);
				return this._dwKeySize;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06005027 RID: 20519 RVA: 0x00119017 File Offset: 0x00118017
		public override string KeyExchangeAlgorithm
		{
			get
			{
				if (this._parameters.KeyNumber == 1)
				{
					return "RSA-PKCS1-KeyEx";
				}
				return null;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005028 RID: 20520 RVA: 0x0011902E File Offset: 0x0011802E
		public override string SignatureAlgorithm
		{
			get
			{
				return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06005029 RID: 20521 RVA: 0x00119035 File Offset: 0x00118035
		// (set) Token: 0x0600502A RID: 20522 RVA: 0x0011903F File Offset: 0x0011803F
		public static bool UseMachineKeyStore
		{
			get
			{
				return RSACryptoServiceProvider.s_UseMachineKeyStore == CspProviderFlags.UseMachineKeyStore;
			}
			set
			{
				RSACryptoServiceProvider.s_UseMachineKeyStore = (value ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x0600502B RID: 20523 RVA: 0x00119050 File Offset: 0x00118050
		// (set) Token: 0x0600502C RID: 20524 RVA: 0x001190B0 File Offset: 0x001180B0
		public bool PersistKeyInCsp
		{
			get
			{
				if (this._safeProvHandle == null)
				{
					lock (this)
					{
						if (this._safeProvHandle == null)
						{
							this._safeProvHandle = Utils.CreateProvHandle(this._parameters, this._randomKeyContainer);
						}
					}
				}
				return Utils._GetPersistKeyInCsp(this._safeProvHandle);
			}
			set
			{
				bool persistKeyInCsp = this.PersistKeyInCsp;
				if (value == persistKeyInCsp)
				{
					return;
				}
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				if (!value)
				{
					KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Delete);
					keyContainerPermission.AccessEntries.Add(accessEntry);
				}
				else
				{
					KeyContainerPermissionAccessEntry accessEntry2 = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Create);
					keyContainerPermission.AccessEntries.Add(accessEntry2);
				}
				keyContainerPermission.Demand();
				Utils._SetPersistKeyInCsp(this._safeProvHandle, value);
			}
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x0011911C File Offset: 0x0011811C
		public override RSAParameters ExportParameters(bool includePrivateParameters)
		{
			this.GetKeyPair();
			if (includePrivateParameters)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Export);
				keyContainerPermission.AccessEntries.Add(accessEntry);
				keyContainerPermission.Demand();
			}
			RSACspObject rsacspObject = new RSACspObject();
			int blobType = includePrivateParameters ? 7 : 6;
			Utils._ExportKey(this._safeKeyHandle, blobType, rsacspObject);
			return RSACryptoServiceProvider.RSAObjectToStruct(rsacspObject);
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x0011917B File Offset: 0x0011817B
		[ComVisible(false)]
		public byte[] ExportCspBlob(bool includePrivateParameters)
		{
			this.GetKeyPair();
			return Utils.ExportCspBlobHelper(includePrivateParameters, this._parameters, this._safeKeyHandle);
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x00119198 File Offset: 0x00118198
		public override void ImportParameters(RSAParameters parameters)
		{
			RSACspObject cspObject = RSACryptoServiceProvider.RSAStructToObject(parameters);
			if (this._safeKeyHandle != null && !this._safeKeyHandle.IsClosed)
			{
				this._safeKeyHandle.Dispose();
			}
			this._safeKeyHandle = SafeKeyHandle.InvalidHandle;
			if (RSACryptoServiceProvider.IsPublic(parameters))
			{
				Utils._ImportKey(Utils.StaticProvHandle, 41984, CspProviderFlags.NoFlags, cspObject, ref this._safeKeyHandle);
				return;
			}
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Import);
			keyContainerPermission.AccessEntries.Add(accessEntry);
			keyContainerPermission.Demand();
			if (this._safeProvHandle == null)
			{
				this._safeProvHandle = Utils.CreateProvHandle(this._parameters, this._randomKeyContainer);
			}
			Utils._ImportKey(this._safeProvHandle, 41984, this._parameters.Flags, cspObject, ref this._safeKeyHandle);
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x00119260 File Offset: 0x00118260
		[ComVisible(false)]
		public void ImportCspBlob(byte[] keyBlob)
		{
			Utils.ImportCspBlobHelper(CspAlgorithmType.Rsa, keyBlob, RSACryptoServiceProvider.IsPublic(keyBlob), ref this._parameters, this._randomKeyContainer, ref this._safeProvHandle, ref this._safeKeyHandle);
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x00119288 File Offset: 0x00118288
		public byte[] SignData(Stream inputStream, object halg)
		{
			string str = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] rgbHash = hashAlgorithm.ComputeHash(inputStream);
			return this.SignHash(rgbHash, str);
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x001192B4 File Offset: 0x001182B4
		public byte[] SignData(byte[] buffer, object halg)
		{
			string str = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] rgbHash = hashAlgorithm.ComputeHash(buffer);
			return this.SignHash(rgbHash, str);
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x001192E0 File Offset: 0x001182E0
		public byte[] SignData(byte[] buffer, int offset, int count, object halg)
		{
			string str = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] rgbHash = hashAlgorithm.ComputeHash(buffer, offset, count);
			return this.SignHash(rgbHash, str);
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x00119310 File Offset: 0x00118310
		public bool VerifyData(byte[] buffer, object halg, byte[] signature)
		{
			string str = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] rgbHash = hashAlgorithm.ComputeHash(buffer);
			return this.VerifyHash(rgbHash, str, signature);
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x0011933C File Offset: 0x0011833C
		public byte[] SignHash(byte[] rgbHash, string str)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (this.PublicOnly)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NoPrivateKey"));
			}
			int calgHash = X509Utils.OidToAlgId(str);
			this.GetKeyPair();
			if (!this._randomKeyContainer)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Sign);
				keyContainerPermission.AccessEntries.Add(accessEntry);
				keyContainerPermission.Demand();
			}
			return Utils._SignValue(this._safeKeyHandle, this._parameters.KeyNumber, 9216, calgHash, rgbHash, 0);
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x001193D0 File Offset: 0x001183D0
		public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			int calgHash = X509Utils.OidToAlgId(str, OidGroup.HashAlgorithm);
			return this.VerifyHash(rgbHash, calgHash, rgbSignature);
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x0011940A File Offset: 0x0011840A
		internal bool VerifyHash(byte[] rgbHash, int calgHash, byte[] rgbSignature)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			this.GetKeyPair();
			return Utils._VerifySign(this._safeKeyHandle, 9216, calgHash, rgbHash, rgbSignature, 0);
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x00119444 File Offset: 0x00118444
		public byte[] Encrypt(byte[] rgb, bool fOAEP)
		{
			if (rgb == null)
			{
				throw new ArgumentNullException("rgb");
			}
			this.GetKeyPair();
			int num = 0;
			byte[] result;
			if (fOAEP)
			{
				if (Utils.HasEnhProv != 1 || Utils.Win2KCrypto != 1)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_Padding_Win2KEnhOnly"));
				}
				result = Utils._EncryptPKWin2KEnh(this._safeKeyHandle, rgb, true, out num);
				if (num != 0)
				{
					throw new CryptographicException(num);
				}
			}
			else
			{
				result = Utils._EncryptPKWin2KEnh(this._safeKeyHandle, rgb, false, out num);
				if (num != 0)
				{
					result = Utils._EncryptKey(this._safeKeyHandle, rgb);
				}
			}
			return result;
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x001194C8 File Offset: 0x001184C8
		public byte[] Decrypt(byte[] rgb, bool fOAEP)
		{
			if (rgb == null)
			{
				throw new ArgumentNullException("rgb");
			}
			this.GetKeyPair();
			if (rgb.Length > this.KeySize / 8)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_Padding_DecDataTooBig"), new object[]
				{
					this.KeySize / 8
				}));
			}
			if (!this._randomKeyContainer)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry accessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Decrypt);
				keyContainerPermission.AccessEntries.Add(accessEntry);
				keyContainerPermission.Demand();
			}
			int num = 0;
			byte[] result;
			if (fOAEP)
			{
				if (Utils.HasEnhProv != 1 || Utils.Win2KCrypto != 1)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_Padding_Win2KEnhOnly"));
				}
				result = Utils._DecryptPKWin2KEnh(this._safeKeyHandle, rgb, true, out num);
				if (num != 0)
				{
					throw new CryptographicException(num);
				}
			}
			else
			{
				result = Utils._DecryptPKWin2KEnh(this._safeKeyHandle, rgb, false, out num);
				if (num != 0)
				{
					result = Utils._DecryptKey(this._safeKeyHandle, rgb, 0);
				}
			}
			return result;
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x001195C1 File Offset: 0x001185C1
		public override byte[] DecryptValue(byte[] rgb)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x001195D2 File Offset: 0x001185D2
		public override byte[] EncryptValue(byte[] rgb)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x001195E4 File Offset: 0x001185E4
		private static RSAParameters RSAObjectToStruct(RSACspObject rsaCspObject)
		{
			return new RSAParameters
			{
				Exponent = rsaCspObject.Exponent,
				Modulus = rsaCspObject.Modulus,
				P = rsaCspObject.P,
				Q = rsaCspObject.Q,
				DP = rsaCspObject.DP,
				DQ = rsaCspObject.DQ,
				InverseQ = rsaCspObject.InverseQ,
				D = rsaCspObject.D
			};
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x00119664 File Offset: 0x00118664
		private static RSACspObject RSAStructToObject(RSAParameters rsaParams)
		{
			return new RSACspObject
			{
				Exponent = rsaParams.Exponent,
				Modulus = rsaParams.Modulus,
				P = rsaParams.P,
				Q = rsaParams.Q,
				DP = rsaParams.DP,
				DQ = rsaParams.DQ,
				InverseQ = rsaParams.InverseQ,
				D = rsaParams.D
			};
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x001196E0 File Offset: 0x001186E0
		private static bool IsPublic(RSAParameters rsaParams)
		{
			return rsaParams.P == null;
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x001196EC File Offset: 0x001186EC
		private static bool IsPublic(byte[] keyBlob)
		{
			if (keyBlob == null)
			{
				throw new ArgumentNullException("keyBlob");
			}
			return keyBlob[0] == 6 && keyBlob[11] == 49 && keyBlob[10] == 65 && keyBlob[9] == 83 && keyBlob[8] == 82;
		}

		// Token: 0x04002945 RID: 10565
		private const uint RandomKeyContainerFlag = 2147483648U;

		// Token: 0x04002946 RID: 10566
		private int _dwKeySize;

		// Token: 0x04002947 RID: 10567
		private CspParameters _parameters;

		// Token: 0x04002948 RID: 10568
		private bool _randomKeyContainer;

		// Token: 0x04002949 RID: 10569
		private SafeProvHandle _safeProvHandle;

		// Token: 0x0400294A RID: 10570
		private SafeKeyHandle _safeKeyHandle;

		// Token: 0x0400294B RID: 10571
		private static CspProviderFlags s_UseMachineKeyStore;
	}
}
