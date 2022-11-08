using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E1 RID: 2017
	internal sealed class MemberPrimitiveTyped : IStreamable
	{
		// Token: 0x06004763 RID: 18275 RVA: 0x000F4A4B File Offset: 0x000F3A4B
		internal MemberPrimitiveTyped()
		{
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x000F4A53 File Offset: 0x000F3A53
		internal void Set(InternalPrimitiveTypeE primitiveTypeEnum, object value)
		{
			this.primitiveTypeEnum = primitiveTypeEnum;
			this.value = value;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x000F4A63 File Offset: 0x000F3A63
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(8);
			sout.WriteByte((byte)this.primitiveTypeEnum);
			sout.WriteValue(this.primitiveTypeEnum, this.value);
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x000F4A8B File Offset: 0x000F3A8B
		public void Read(__BinaryParser input)
		{
			this.primitiveTypeEnum = (InternalPrimitiveTypeE)input.ReadByte();
			this.value = input.ReadValue(this.primitiveTypeEnum);
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x000F4AAB File Offset: 0x000F3AAB
		public void Dump()
		{
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x000F4AAD File Offset: 0x000F3AAD
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x0400241B RID: 9243
		internal InternalPrimitiveTypeE primitiveTypeEnum;

		// Token: 0x0400241C RID: 9244
		internal object value;
	}
}
