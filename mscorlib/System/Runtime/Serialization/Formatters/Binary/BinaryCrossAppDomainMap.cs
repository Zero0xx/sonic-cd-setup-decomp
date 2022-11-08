using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E0 RID: 2016
	internal sealed class BinaryCrossAppDomainMap : IStreamable
	{
		// Token: 0x0600475E RID: 18270 RVA: 0x000F4A10 File Offset: 0x000F3A10
		internal BinaryCrossAppDomainMap()
		{
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x000F4A18 File Offset: 0x000F3A18
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(18);
			sout.WriteInt32(this.crossAppDomainArrayIndex);
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x000F4A2E File Offset: 0x000F3A2E
		public void Read(__BinaryParser input)
		{
			this.crossAppDomainArrayIndex = input.ReadInt32();
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x000F4A3C File Offset: 0x000F3A3C
		public void Dump()
		{
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x000F4A3E File Offset: 0x000F3A3E
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x0400241A RID: 9242
		internal int crossAppDomainArrayIndex;
	}
}
