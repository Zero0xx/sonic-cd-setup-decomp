using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DF RID: 2015
	internal sealed class BinaryCrossAppDomainString : IStreamable
	{
		// Token: 0x06004759 RID: 18265 RVA: 0x000F49BD File Offset: 0x000F39BD
		internal BinaryCrossAppDomainString()
		{
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x000F49C5 File Offset: 0x000F39C5
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(19);
			sout.WriteInt32(this.objectId);
			sout.WriteInt32(this.value);
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x000F49E7 File Offset: 0x000F39E7
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.value = input.ReadInt32();
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x000F4A01 File Offset: 0x000F3A01
		public void Dump()
		{
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x000F4A03 File Offset: 0x000F3A03
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x04002418 RID: 9240
		internal int objectId;

		// Token: 0x04002419 RID: 9241
		internal int value;
	}
}
