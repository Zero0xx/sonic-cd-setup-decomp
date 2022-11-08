using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D9 RID: 2009
	internal sealed class BinaryAssembly : IStreamable
	{
		// Token: 0x06004733 RID: 18227 RVA: 0x000F3C1D File Offset: 0x000F2C1D
		internal BinaryAssembly()
		{
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x000F3C25 File Offset: 0x000F2C25
		internal void Set(int assemId, string assemblyString)
		{
			this.assemId = assemId;
			this.assemblyString = assemblyString;
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x000F3C35 File Offset: 0x000F2C35
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(12);
			sout.WriteInt32(this.assemId);
			sout.WriteString(this.assemblyString);
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x000F3C57 File Offset: 0x000F2C57
		public void Read(__BinaryParser input)
		{
			this.assemId = input.ReadInt32();
			this.assemblyString = input.ReadString();
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x000F3C71 File Offset: 0x000F2C71
		public void Dump()
		{
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x000F3C73 File Offset: 0x000F2C73
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023F7 RID: 9207
		internal int assemId;

		// Token: 0x040023F8 RID: 9208
		internal string assemblyString;
	}
}
