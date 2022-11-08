using System;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x020006B5 RID: 1717
	internal class SevenBitStream : DelegatedStream
	{
		// Token: 0x06003509 RID: 13577 RVA: 0x000E183D File Offset: 0x000E083D
		internal SevenBitStream(Stream stream) : base(stream)
		{
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000E1848 File Offset: 0x000E0848
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.CheckBytes(buffer, offset, count);
			return base.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000E18A4 File Offset: 0x000E08A4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.CheckBytes(buffer, offset, count);
			base.Write(buffer, offset, count);
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000E18FC File Offset: 0x000E08FC
		private void CheckBytes(byte[] buffer, int offset, int count)
		{
			for (int i = count; i < offset + count; i++)
			{
				if (buffer[i] > 127)
				{
					throw new FormatException(SR.GetString("Mail7BitStreamInvalidCharacter"));
				}
			}
		}
	}
}
