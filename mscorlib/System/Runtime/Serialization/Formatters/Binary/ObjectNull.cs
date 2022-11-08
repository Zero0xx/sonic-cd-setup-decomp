using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E7 RID: 2023
	internal sealed class ObjectNull : IStreamable
	{
		// Token: 0x06004787 RID: 18311 RVA: 0x000F5340 File Offset: 0x000F4340
		internal ObjectNull()
		{
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x000F5348 File Offset: 0x000F4348
		internal void SetNullCount(int nullCount)
		{
			this.nullCount = nullCount;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x000F5354 File Offset: 0x000F4354
		public void Write(__BinaryWriter sout)
		{
			if (this.nullCount == 1)
			{
				sout.WriteByte(10);
				return;
			}
			if (this.nullCount < 256)
			{
				sout.WriteByte(13);
				sout.WriteByte((byte)this.nullCount);
				return;
			}
			sout.WriteByte(14);
			sout.WriteInt32(this.nullCount);
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x000F53AA File Offset: 0x000F43AA
		public void Read(__BinaryParser input)
		{
			this.Read(input, BinaryHeaderEnum.ObjectNull);
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x000F53B8 File Offset: 0x000F43B8
		public void Read(__BinaryParser input, BinaryHeaderEnum binaryHeaderEnum)
		{
			switch (binaryHeaderEnum)
			{
			case BinaryHeaderEnum.ObjectNull:
				this.nullCount = 1;
				return;
			case BinaryHeaderEnum.MessageEnd:
			case BinaryHeaderEnum.Assembly:
				break;
			case BinaryHeaderEnum.ObjectNullMultiple256:
				this.nullCount = (int)input.ReadByte();
				return;
			case BinaryHeaderEnum.ObjectNullMultiple:
				this.nullCount = input.ReadInt32();
				break;
			default:
				return;
			}
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x000F5406 File Offset: 0x000F4406
		public void Dump()
		{
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x000F5408 File Offset: 0x000F4408
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				if (this.nullCount == 1)
				{
					return;
				}
				if (this.nullCount < 256)
				{
				}
			}
		}

		// Token: 0x04002438 RID: 9272
		internal int nullCount;
	}
}
