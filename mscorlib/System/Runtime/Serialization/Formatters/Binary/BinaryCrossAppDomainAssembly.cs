using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DA RID: 2010
	internal sealed class BinaryCrossAppDomainAssembly : IStreamable
	{
		// Token: 0x06004739 RID: 18233 RVA: 0x000F3C80 File Offset: 0x000F2C80
		internal BinaryCrossAppDomainAssembly()
		{
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x000F3C88 File Offset: 0x000F2C88
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(20);
			sout.WriteInt32(this.assemId);
			sout.WriteInt32(this.assemblyIndex);
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x000F3CAA File Offset: 0x000F2CAA
		public void Read(__BinaryParser input)
		{
			this.assemId = input.ReadInt32();
			this.assemblyIndex = input.ReadInt32();
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x000F3CC4 File Offset: 0x000F2CC4
		public void Dump()
		{
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x000F3CC6 File Offset: 0x000F2CC6
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023F9 RID: 9209
		internal int assemId;

		// Token: 0x040023FA RID: 9210
		internal int assemblyIndex;
	}
}
