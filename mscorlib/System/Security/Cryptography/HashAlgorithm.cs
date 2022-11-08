using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000882 RID: 2178
	[ComVisible(true)]
	public abstract class HashAlgorithm : ICryptoTransform, IDisposable
	{
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06004F5D RID: 20317 RVA: 0x001142F7 File Offset: 0x001132F7
		public virtual int HashSize
		{
			get
			{
				return this.HashSizeValue;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06004F5E RID: 20318 RVA: 0x00114300 File Offset: 0x00113300
		public virtual byte[] Hash
		{
			get
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
				}
				if (this.State != 0)
				{
					throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_HashNotYetFinalized"));
				}
				return (byte[])this.HashValue.Clone();
			}
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x0011434E File Offset: 0x0011334E
		public static HashAlgorithm Create()
		{
			return HashAlgorithm.Create("System.Security.Cryptography.HashAlgorithm");
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x0011435A File Offset: 0x0011335A
		public static HashAlgorithm Create(string hashName)
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName(hashName);
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x00114368 File Offset: 0x00113368
		public byte[] ComputeHash(Stream inputStream)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			byte[] array = new byte[4096];
			int num;
			do
			{
				num = inputStream.Read(array, 0, 4096);
				if (num > 0)
				{
					this.HashCore(array, 0, num);
				}
			}
			while (num > 0);
			this.HashValue = this.HashFinal();
			byte[] result = (byte[])this.HashValue.Clone();
			this.Initialize();
			return result;
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x001143DC File Offset: 0x001133DC
		public byte[] ComputeHash(byte[] buffer)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.HashCore(buffer, 0, buffer.Length);
			this.HashValue = this.HashFinal();
			byte[] result = (byte[])this.HashValue.Clone();
			this.Initialize();
			return result;
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x00114440 File Offset: 0x00113440
		public byte[] ComputeHash(byte[] buffer, int offset, int count)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0 || count > buffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			if (buffer.Length - count < offset)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			this.HashCore(buffer, offset, count);
			this.HashValue = this.HashFinal();
			byte[] result = (byte[])this.HashValue.Clone();
			this.Initialize();
			return result;
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06004F64 RID: 20324 RVA: 0x001144EC File Offset: 0x001134EC
		public virtual int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004F65 RID: 20325 RVA: 0x001144EF File Offset: 0x001134EF
		public virtual int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06004F66 RID: 20326 RVA: 0x001144F2 File Offset: 0x001134F2
		public virtual bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06004F67 RID: 20327 RVA: 0x001144F5 File Offset: 0x001134F5
		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x001144F8 File Offset: 0x001134F8
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
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
			this.State = 1;
			this.HashCore(inputBuffer, inputOffset, inputCount);
			if (outputBuffer != null && (inputBuffer != outputBuffer || inputOffset != outputOffset))
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x001145A4 File Offset: 0x001135A4
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (this.m_bDisposed)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_Generic"));
			}
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
			this.HashCore(inputBuffer, inputOffset, inputCount);
			this.HashValue = this.HashFinal();
			byte[] array = new byte[inputCount];
			if (inputCount != 0)
			{
				Buffer.InternalBlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			}
			this.State = 0;
			return array;
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x00114654 File Offset: 0x00113654
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x00114663 File Offset: 0x00113663
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x0011466B File Offset: 0x0011366B
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.HashValue != null)
				{
					Array.Clear(this.HashValue, 0, this.HashValue.Length);
				}
				this.HashValue = null;
				this.m_bDisposed = true;
			}
		}

		// Token: 0x06004F6D RID: 20333
		public abstract void Initialize();

		// Token: 0x06004F6E RID: 20334
		protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

		// Token: 0x06004F6F RID: 20335
		protected abstract byte[] HashFinal();

		// Token: 0x040028FA RID: 10490
		protected int HashSizeValue;

		// Token: 0x040028FB RID: 10491
		protected internal byte[] HashValue;

		// Token: 0x040028FC RID: 10492
		protected int State;

		// Token: 0x040028FD RID: 10493
		private bool m_bDisposed;
	}
}
