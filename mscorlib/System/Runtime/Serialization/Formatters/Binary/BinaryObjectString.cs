using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DE RID: 2014
	internal sealed class BinaryObjectString : IStreamable
	{
		// Token: 0x06004753 RID: 18259 RVA: 0x000F495B File Offset: 0x000F395B
		internal BinaryObjectString()
		{
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x000F4963 File Offset: 0x000F3963
		internal void Set(int objectId, string value)
		{
			this.objectId = objectId;
			this.value = value;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x000F4973 File Offset: 0x000F3973
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(6);
			sout.WriteInt32(this.objectId);
			sout.WriteString(this.value);
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x000F4994 File Offset: 0x000F3994
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.value = input.ReadString();
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x000F49AE File Offset: 0x000F39AE
		public void Dump()
		{
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x000F49B0 File Offset: 0x000F39B0
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x04002416 RID: 9238
		internal int objectId;

		// Token: 0x04002417 RID: 9239
		internal string value;
	}
}
