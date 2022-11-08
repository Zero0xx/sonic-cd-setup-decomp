using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D8 RID: 2008
	internal sealed class SerializationHeaderRecord : IStreamable
	{
		// Token: 0x0600472C RID: 18220 RVA: 0x000F3AAB File Offset: 0x000F2AAB
		internal SerializationHeaderRecord()
		{
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x000F3ABA File Offset: 0x000F2ABA
		internal SerializationHeaderRecord(BinaryHeaderEnum binaryHeaderEnum, int topId, int headerId, int majorVersion, int minorVersion)
		{
			this.binaryHeaderEnum = binaryHeaderEnum;
			this.topId = topId;
			this.headerId = headerId;
			this.majorVersion = majorVersion;
			this.minorVersion = minorVersion;
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x000F3AF0 File Offset: 0x000F2AF0
		public void Write(__BinaryWriter sout)
		{
			this.majorVersion = this.binaryFormatterMajorVersion;
			this.minorVersion = this.binaryFormatterMinorVersion;
			sout.WriteByte((byte)this.binaryHeaderEnum);
			sout.WriteInt32(this.topId);
			sout.WriteInt32(this.headerId);
			sout.WriteInt32(this.binaryFormatterMajorVersion);
			sout.WriteInt32(this.binaryFormatterMinorVersion);
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x000F3B52 File Offset: 0x000F2B52
		private static int GetInt32(byte[] buffer, int index)
		{
			return (int)buffer[index] | (int)buffer[index + 1] << 8 | (int)buffer[index + 2] << 16 | (int)buffer[index + 3] << 24;
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x000F3B74 File Offset: 0x000F2B74
		public void Read(__BinaryParser input)
		{
			byte[] array = input.ReadBytes(17);
			if (array.Length < 17)
			{
				__Error.EndOfFile();
			}
			this.majorVersion = SerializationHeaderRecord.GetInt32(array, 9);
			if (this.majorVersion > this.binaryFormatterMajorVersion)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidFormat"), new object[]
				{
					BitConverter.ToString(array)
				}));
			}
			this.binaryHeaderEnum = (BinaryHeaderEnum)array[0];
			this.topId = SerializationHeaderRecord.GetInt32(array, 1);
			this.headerId = SerializationHeaderRecord.GetInt32(array, 5);
			this.minorVersion = SerializationHeaderRecord.GetInt32(array, 13);
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x000F3C0E File Offset: 0x000F2C0E
		public void Dump()
		{
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x000F3C10 File Offset: 0x000F2C10
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023F0 RID: 9200
		internal int binaryFormatterMajorVersion = 1;

		// Token: 0x040023F1 RID: 9201
		internal int binaryFormatterMinorVersion;

		// Token: 0x040023F2 RID: 9202
		internal BinaryHeaderEnum binaryHeaderEnum;

		// Token: 0x040023F3 RID: 9203
		internal int topId;

		// Token: 0x040023F4 RID: 9204
		internal int headerId;

		// Token: 0x040023F5 RID: 9205
		internal int majorVersion;

		// Token: 0x040023F6 RID: 9206
		internal int minorVersion;
	}
}
