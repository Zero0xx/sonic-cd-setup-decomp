using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007DB RID: 2011
	internal sealed class BinaryObject : IStreamable
	{
		// Token: 0x0600473E RID: 18238 RVA: 0x000F3CD3 File Offset: 0x000F2CD3
		internal BinaryObject()
		{
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x000F3CDB File Offset: 0x000F2CDB
		internal void Set(int objectId, int mapId)
		{
			this.objectId = objectId;
			this.mapId = mapId;
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x000F3CEB File Offset: 0x000F2CEB
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(1);
			sout.WriteInt32(this.objectId);
			sout.WriteInt32(this.mapId);
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x000F3D0C File Offset: 0x000F2D0C
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.mapId = input.ReadInt32();
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x000F3D26 File Offset: 0x000F2D26
		public void Dump()
		{
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x000F3D28 File Offset: 0x000F2D28
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023FB RID: 9211
		internal int objectId;

		// Token: 0x040023FC RID: 9212
		internal int mapId;
	}
}
