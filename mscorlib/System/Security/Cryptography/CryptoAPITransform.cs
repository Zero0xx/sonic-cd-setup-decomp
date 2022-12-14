using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000871 RID: 2161
	[ComVisible(true)]
	public sealed class CryptoAPITransform : ICryptoTransform, IDisposable
	{
		// Token: 0x06004EBD RID: 20157 RVA: 0x00110704 File Offset: 0x0010F704
		private CryptoAPITransform()
		{
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x0011070C File Offset: 0x0010F70C
		internal CryptoAPITransform(int algid, int cArgs, int[] rgArgIds, object[] rgArgValues, byte[] rgbKey, PaddingMode padding, CipherMode cipherChainingMode, int blockSize, int feedbackSize, bool useSalt, CryptoAPITransformMode encDecMode)
		{
			this.BlockSizeValue = blockSize;
			this.ModeValue = cipherChainingMode;
			this.PaddingValue = padding;
			this.encryptOrDecrypt = encDecMode;
			int[] array = new int[rgArgIds.Length];
			Array.Copy(rgArgIds, array, rgArgIds.Length);
			this._rgbKey = new byte[rgbKey.Length];
			Array.Copy(rgbKey, this._rgbKey, rgbKey.Length);
			object[] array2 = new object[rgArgValues.Length];
			for (int i = 0; i < rgArgValues.Length; i++)
			{
				if (rgArgValues[i] is byte[])
				{
					byte[] array3 = (byte[])rgArgValues[i];
					byte[] array4 = new byte[array3.Length];
					Array.Copy(array3, array4, array3.Length);
					array2[i] = array4;
				}
				else if (rgArgValues[i] is int)
				{
					array2[i] = (int)rgArgValues[i];
				}
				else if (rgArgValues[i] is CipherMode)
				{
					array2[i] = (int)rgArgValues[i];
				}
			}
			this._safeProvHandle = Utils.AcquireProvHandle(new CspParameters(Utils.DefaultRsaProviderType));
			SafeKeyHandle invalidHandle = SafeKeyHandle.InvalidHandle;
			Utils._ImportBulkKey(this._safeProvHandle, algid, useSalt, this._rgbKey, ref invalidHandle);
			this._safeKeyHandle = invalidHandle;
			int j = 0;
			while (j < cArgs)
			{
				int num = rgArgIds[j];
				int dwValue;
				switch (num)
				{
				case 1:
				{
					this.IVValue = (byte[])array2[j];
					byte[] ivvalue = this.IVValue;
					Utils._SetKeyParamRgb(this._safeKeyHandle, array[j], ivvalue);
					break;
				}
				case 2:
				case 3:
					goto IL_1C9;
				case 4:
					this.ModeValue = (CipherMode)array2[j];
					dwValue = (int)array2[j];
					goto IL_19F;
				case 5:
					dwValue = (int)array2[j];
					goto IL_19F;
				default:
					if (num != 19)
					{
						goto IL_1C9;
					}
					dwValue = (int)array2[j];
					goto IL_19F;
				}
				IL_1DE:
				j++;
				continue;
				IL_19F:
				Utils._SetKeyParamDw(this._safeKeyHandle, array[j], dwValue);
				goto IL_1DE;
				IL_1C9:
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeyParameter"), "_rgArgIds[i]");
			}
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x00110905 File Offset: 0x0010F905
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x00110914 File Offset: 0x0010F914
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x0011091C File Offset: 0x0010F91C
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._rgbKey != null)
				{
					Array.Clear(this._rgbKey, 0, this._rgbKey.Length);
					this._rgbKey = null;
				}
				if (this.IVValue != null)
				{
					Array.Clear(this.IVValue, 0, this.IVValue.Length);
					this.IVValue = null;
				}
				if (this._depadBuffer != null)
				{
					Array.Clear(this._depadBuffer, 0, this._depadBuffer.Length);
					this._depadBuffer = null;
				}
			}
			if (this._safeKeyHandle != null && !this._safeKeyHandle.IsClosed)
			{
				this._safeKeyHandle.Dispose();
			}
			if (this._safeProvHandle != null && !this._safeProvHandle.IsClosed)
			{
				this._safeProvHandle.Dispose();
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06004EC2 RID: 20162 RVA: 0x001109D5 File Offset: 0x0010F9D5
		public IntPtr KeyHandle
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this._safeKeyHandle.DangerousGetHandle();
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06004EC3 RID: 20163 RVA: 0x001109E2 File Offset: 0x0010F9E2
		public int InputBlockSize
		{
			get
			{
				return this.BlockSizeValue / 8;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06004EC4 RID: 20164 RVA: 0x001109EC File Offset: 0x0010F9EC
		public int OutputBlockSize
		{
			get
			{
				return this.BlockSizeValue / 8;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004EC5 RID: 20165 RVA: 0x001109F6 File Offset: 0x0010F9F6
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x001109F9 File Offset: 0x0010F9F9
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x001109FC File Offset: 0x0010F9FC
		[ComVisible(false)]
		public void Reset()
		{
			this._depadBuffer = null;
			byte[] array = null;
			Utils._EncryptData(this._safeKeyHandle, new byte[0], 0, 0, ref array, 0, this.PaddingValue, true);
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x00110A30 File Offset: 0x0010FA30
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (outputBuffer == null)
			{
				throw new ArgumentNullException("outputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (inputCount <= 0 || inputCount % this.InputBlockSize != 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this.encryptOrDecrypt == CryptoAPITransformMode.Encrypt)
			{
				return Utils._EncryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset, this.PaddingValue, false);
			}
			if (this.PaddingValue == PaddingMode.Zeros || this.PaddingValue == PaddingMode.None)
			{
				return Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset, this.PaddingValue, false);
			}
			if (this._depadBuffer == null)
			{
				this._depadBuffer = new byte[this.InputBlockSize];
				int num = inputCount - this.InputBlockSize;
				Buffer.InternalBlockCopy(inputBuffer, inputOffset + num, this._depadBuffer, 0, this.InputBlockSize);
				return Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, num, ref outputBuffer, outputOffset, this.PaddingValue, false);
			}
			int num2 = Utils._DecryptData(this._safeKeyHandle, this._depadBuffer, 0, this._depadBuffer.Length, ref outputBuffer, outputOffset, this.PaddingValue, false);
			outputOffset += this.OutputBlockSize;
			int num3 = inputCount - this.InputBlockSize;
			Buffer.InternalBlockCopy(inputBuffer, inputOffset + num3, this._depadBuffer, 0, this.InputBlockSize);
			num2 = Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, num3, ref outputBuffer, outputOffset, this.PaddingValue, false);
			return this.OutputBlockSize + num2;
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x00110BC4 File Offset: 0x0010FBC4
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this.encryptOrDecrypt == CryptoAPITransformMode.Encrypt)
			{
				byte[] result = null;
				Utils._EncryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref result, 0, this.PaddingValue, true);
				this.Reset();
				return result;
			}
			if (inputCount % this.InputBlockSize != 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_SSD_InvalidDataSize"));
			}
			if (this._depadBuffer == null)
			{
				byte[] result2 = null;
				Utils._DecryptData(this._safeKeyHandle, inputBuffer, inputOffset, inputCount, ref result2, 0, this.PaddingValue, true);
				this.Reset();
				return result2;
			}
			byte[] array = new byte[this._depadBuffer.Length + inputCount];
			Buffer.InternalBlockCopy(this._depadBuffer, 0, array, 0, this._depadBuffer.Length);
			Buffer.InternalBlockCopy(inputBuffer, inputOffset, array, this._depadBuffer.Length, inputCount);
			byte[] result3 = null;
			Utils._DecryptData(this._safeKeyHandle, array, 0, array.Length, ref result3, 0, this.PaddingValue, true);
			this.Reset();
			return result3;
		}

		// Token: 0x040028A2 RID: 10402
		private int BlockSizeValue;

		// Token: 0x040028A3 RID: 10403
		private byte[] IVValue;

		// Token: 0x040028A4 RID: 10404
		private CipherMode ModeValue;

		// Token: 0x040028A5 RID: 10405
		private PaddingMode PaddingValue;

		// Token: 0x040028A6 RID: 10406
		private CryptoAPITransformMode encryptOrDecrypt;

		// Token: 0x040028A7 RID: 10407
		private byte[] _rgbKey;

		// Token: 0x040028A8 RID: 10408
		private byte[] _depadBuffer;

		// Token: 0x040028A9 RID: 10409
		private SafeKeyHandle _safeKeyHandle;

		// Token: 0x040028AA RID: 10410
		private SafeProvHandle _safeProvHandle;
	}
}
