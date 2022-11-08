using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E3 RID: 2019
	internal sealed class BinaryObjectWithMapTyped : IStreamable
	{
		// Token: 0x06004770 RID: 18288 RVA: 0x000F4C2B File Offset: 0x000F3C2B
		internal BinaryObjectWithMapTyped()
		{
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x000F4C33 File Offset: 0x000F3C33
		internal BinaryObjectWithMapTyped(BinaryHeaderEnum binaryHeaderEnum)
		{
			this.binaryHeaderEnum = binaryHeaderEnum;
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x000F4C44 File Offset: 0x000F3C44
		internal void Set(int objectId, string name, int numMembers, string[] memberNames, BinaryTypeEnum[] binaryTypeEnumA, object[] typeInformationA, int[] memberAssemIds, int assemId)
		{
			this.objectId = objectId;
			this.assemId = assemId;
			this.name = name;
			this.numMembers = numMembers;
			this.memberNames = memberNames;
			this.binaryTypeEnumA = binaryTypeEnumA;
			this.typeInformationA = typeInformationA;
			this.memberAssemIds = memberAssemIds;
			this.assemId = assemId;
			if (assemId > 0)
			{
				this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMapTypedAssemId;
				return;
			}
			this.binaryHeaderEnum = BinaryHeaderEnum.ObjectWithMapTyped;
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x000F4CAC File Offset: 0x000F3CAC
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
			for (int j = 0; j < this.numMembers; j++)
			{
				sout.WriteByte((byte)this.binaryTypeEnumA[j]);
			}
			for (int k = 0; k < this.numMembers; k++)
			{
				BinaryConverter.WriteTypeInfo(this.binaryTypeEnumA[k], this.typeInformationA[k], this.memberAssemIds[k], sout);
			}
			if (this.assemId > 0)
			{
				sout.WriteInt32(this.assemId);
			}
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x000F4D70 File Offset: 0x000F3D70
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.name = input.ReadString();
			this.numMembers = input.ReadInt32();
			this.memberNames = new string[this.numMembers];
			this.binaryTypeEnumA = new BinaryTypeEnum[this.numMembers];
			this.typeInformationA = new object[this.numMembers];
			this.memberAssemIds = new int[this.numMembers];
			for (int i = 0; i < this.numMembers; i++)
			{
				this.memberNames[i] = input.ReadString();
			}
			for (int j = 0; j < this.numMembers; j++)
			{
				this.binaryTypeEnumA[j] = (BinaryTypeEnum)input.ReadByte();
			}
			for (int k = 0; k < this.numMembers; k++)
			{
				if (this.binaryTypeEnumA[k] != BinaryTypeEnum.ObjectUrt && this.binaryTypeEnumA[k] != BinaryTypeEnum.ObjectUser)
				{
					this.typeInformationA[k] = BinaryConverter.ReadTypeInfo(this.binaryTypeEnumA[k], input, out this.memberAssemIds[k]);
				}
				else
				{
					BinaryConverter.ReadTypeInfo(this.binaryTypeEnumA[k], input, out this.memberAssemIds[k]);
				}
			}
			if (this.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapTypedAssemId)
			{
				this.assemId = input.ReadInt32();
			}
		}

		// Token: 0x04002423 RID: 9251
		internal BinaryHeaderEnum binaryHeaderEnum;

		// Token: 0x04002424 RID: 9252
		internal int objectId;

		// Token: 0x04002425 RID: 9253
		internal string name;

		// Token: 0x04002426 RID: 9254
		internal int numMembers;

		// Token: 0x04002427 RID: 9255
		internal string[] memberNames;

		// Token: 0x04002428 RID: 9256
		internal BinaryTypeEnum[] binaryTypeEnumA;

		// Token: 0x04002429 RID: 9257
		internal object[] typeInformationA;

		// Token: 0x0400242A RID: 9258
		internal int[] memberAssemIds;

		// Token: 0x0400242B RID: 9259
		internal int assemId;
	}
}
