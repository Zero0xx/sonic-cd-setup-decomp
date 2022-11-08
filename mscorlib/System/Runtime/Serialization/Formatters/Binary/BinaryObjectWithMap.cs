using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E2 RID: 2018
	internal sealed class BinaryObjectWithMap : IStreamable
	{
		// Token: 0x06004769 RID: 18281 RVA: 0x000F4ABA File Offset: 0x000F3ABA
		internal BinaryObjectWithMap()
		{
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x000F4AC2 File Offset: 0x000F3AC2
		internal BinaryObjectWithMap(BinaryHeaderEnum binaryHeaderEnum)
		{
			this.binaryHeaderEnum = binaryHeaderEnum;
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x000F4AD1 File Offset: 0x000F3AD1
		internal void Set(int objectId, string name, int numMembers, string[] memberNames, int assemId)
		{
			this.objectId = objectId;
			this.name = name;
			this.numMembers = numMembers;
			this.memberNames = memberNames;
			this.assemId = assemId;
			if (assemId > 0)
			{
				this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMapAssemId;
				return;
			}
			this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMap;
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x000F4B0C File Offset: 0x000F3B0C
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte((byte)this.binaryHeaderEnum);
			sout.WriteInt32(this.objectId);
			sout.WriteString(this.name);
			sout.WriteInt32(this.numMembers);
			for (int i = 0; i < this.numMembers; i++)
			{
				sout.WriteString(this.memberNames[i]);
			}
			if (this.assemId > 0)
			{
				sout.WriteInt32(this.assemId);
			}
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x000F4B80 File Offset: 0x000F3B80
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.name = input.ReadString();
			this.numMembers = input.ReadInt32();
			this.memberNames = new string[this.numMembers];
			for (int i = 0; i < this.numMembers; i++)
			{
				this.memberNames[i] = input.ReadString();
			}
			if (this.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapAssemId)
			{
				this.assemId = input.ReadInt32();
			}
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x000F4BF6 File Offset: 0x000F3BF6
		public void Dump()
		{
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x000F4BF8 File Offset: 0x000F3BF8
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				for (int i = 0; i < this.numMembers; i++)
				{
				}
				BinaryHeaderEnum binaryHeaderEnum = this.binaryHeaderEnum;
			}
		}

		// Token: 0x0400241D RID: 9245
		internal BinaryHeaderEnum binaryHeaderEnum;

		// Token: 0x0400241E RID: 9246
		internal int objectId;

		// Token: 0x0400241F RID: 9247
		internal string name;

		// Token: 0x04002420 RID: 9248
		internal int numMembers;

		// Token: 0x04002421 RID: 9249
		internal string[] memberNames;

		// Token: 0x04002422 RID: 9250
		internal int assemId;
	}
}
