using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography
{
	// Token: 0x02000896 RID: 2198
	[ComVisible(true)]
	public class Rfc2898DeriveBytes : DeriveBytes
	{
		// Token: 0x06004FF6 RID: 20470 RVA: 0x001162E6 File Offset: 0x001152E6
		public Rfc2898DeriveBytes(string password, int saltSize) : this(password, saltSize, 1000)
		{
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x001162F8 File Offset: 0x001152F8
		public Rfc2898DeriveBytes(string password, int saltSize, int iterations)
		{
			if (saltSize < 0)
			{
				throw new ArgumentOutOfRangeException("saltSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[saltSize];
			Utils.StaticRandomNumberGenerator.GetBytes(array);
			this.Salt = array;
			this.IterationCount = iterations;
			this.m_hmacsha1 = new HMACSHA1(new UTF8Encoding(false).GetBytes(password));
			this.Initialize();
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x00116361 File Offset: 0x00115361
		public Rfc2898DeriveBytes(string password, byte[] salt) : this(password, salt, 1000)
		{
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x00116370 File Offset: 0x00115370
		public Rfc2898DeriveBytes(string password, byte[] salt, int iterations) : this(new UTF8Encoding(false).GetBytes(password), salt, iterations)
		{
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x00116386 File Offset: 0x00115386
		public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations)
		{
			this.Salt = salt;
			this.IterationCount = iterations;
			this.m_hmacsha1 = new HMACSHA1(password);
			this.Initialize();
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06004FFB RID: 20475 RVA: 0x001163AE File Offset: 0x001153AE
		// (set) Token: 0x06004FFC RID: 20476 RVA: 0x001163B6 File Offset: 0x001153B6
		public int IterationCount
		{
			get
			{
				return (int)this.m_iterations;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				this.m_iterations = (uint)value;
				this.Initialize();
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004FFD RID: 20477 RVA: 0x001163DE File Offset: 0x001153DE
		// (set) Token: 0x06004FFE RID: 20478 RVA: 0x001163F0 File Offset: 0x001153F0
		public byte[] Salt
		{
			get
			{
				return (byte[])this.m_salt.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length < 8)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_PasswordDerivedBytes_FewBytesSalt"), new object[0]));
				}
				this.m_salt = (byte[])value.Clone();
				this.Initialize();
			}
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x00116448 File Offset: 0x00115448
		public override byte[] GetBytes(int cb)
		{
			if (cb <= 0)
			{
				throw new ArgumentOutOfRangeException("cb", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			byte[] array = new byte[cb];
			int i = 0;
			int num = this.m_endIndex - this.m_startIndex;
			if (num > 0)
			{
				if (cb < num)
				{
					Buffer.InternalBlockCopy(this.m_buffer, this.m_startIndex, array, 0, cb);
					this.m_startIndex += cb;
					return array;
				}
				Buffer.InternalBlockCopy(this.m_buffer, this.m_startIndex, array, 0, num);
				this.m_startIndex = (this.m_endIndex = 0);
				i += num;
			}
			while (i < cb)
			{
				byte[] src = this.Func();
				int num2 = cb - i;
				if (num2 <= 20)
				{
					Buffer.InternalBlockCopy(src, 0, array, i, num2);
					i += num2;
					Buffer.InternalBlockCopy(src, num2, this.m_buffer, this.m_startIndex, 20 - num2);
					this.m_endIndex += 20 - num2;
					return array;
				}
				Buffer.InternalBlockCopy(src, 0, array, i, 20);
				i += 20;
			}
			return array;
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x00116549 File Offset: 0x00115549
		public override void Reset()
		{
			this.Initialize();
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x00116554 File Offset: 0x00115554
		private void Initialize()
		{
			if (this.m_buffer != null)
			{
				Array.Clear(this.m_buffer, 0, this.m_buffer.Length);
			}
			this.m_buffer = new byte[20];
			this.m_block = 1U;
			this.m_startIndex = (this.m_endIndex = 0);
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x001165A4 File Offset: 0x001155A4
		private byte[] Func()
		{
			byte[] array = Utils.Int(this.m_block);
			this.m_hmacsha1.TransformBlock(this.m_salt, 0, this.m_salt.Length, this.m_salt, 0);
			this.m_hmacsha1.TransformFinalBlock(array, 0, array.Length);
			byte[] array2 = this.m_hmacsha1.Hash;
			this.m_hmacsha1.Initialize();
			byte[] array3 = array2;
			int num = 2;
			while ((long)num <= (long)((ulong)this.m_iterations))
			{
				array2 = this.m_hmacsha1.ComputeHash(array2);
				for (int i = 0; i < 20; i++)
				{
					byte[] array4 = array3;
					int num2 = i;
					array4[num2] ^= array2[i];
				}
				num++;
			}
			this.m_block += 1U;
			return array3;
		}

		// Token: 0x04002929 RID: 10537
		private const int BlockSize = 20;

		// Token: 0x0400292A RID: 10538
		private byte[] m_buffer;

		// Token: 0x0400292B RID: 10539
		private byte[] m_salt;

		// Token: 0x0400292C RID: 10540
		private HMACSHA1 m_hmacsha1;

		// Token: 0x0400292D RID: 10541
		private uint m_iterations;

		// Token: 0x0400292E RID: 10542
		private uint m_block;

		// Token: 0x0400292F RID: 10543
		private int m_startIndex;

		// Token: 0x04002930 RID: 10544
		private int m_endIndex;
	}
}
