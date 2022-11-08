using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E5 RID: 2021
	internal sealed class MemberPrimitiveUnTyped : IStreamable
	{
		// Token: 0x0600477A RID: 18298 RVA: 0x000F5297 File Offset: 0x000F4297
		internal MemberPrimitiveUnTyped()
		{
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x000F529F File Offset: 0x000F429F
		internal void Set(InternalPrimitiveTypeE typeInformation, object value)
		{
			this.typeInformation = typeInformation;
			this.value = value;
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x000F52AF File Offset: 0x000F42AF
		internal void Set(InternalPrimitiveTypeE typeInformation)
		{
			this.typeInformation = typeInformation;
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x000F52B8 File Offset: 0x000F42B8
		public void Write(__BinaryWriter sout)
		{
			sout.WriteValue(this.typeInformation, this.value);
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x000F52CC File Offset: 0x000F42CC
		public void Read(__BinaryParser input)
		{
			this.value = input.ReadValue(this.typeInformation);
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x000F52E0 File Offset: 0x000F42E0
		public void Dump()
		{
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x000F52E2 File Offset: 0x000F42E2
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				Converter.ToComType(this.typeInformation);
			}
		}

		// Token: 0x04002435 RID: 9269
		internal InternalPrimitiveTypeE typeInformation;

		// Token: 0x04002436 RID: 9270
		internal object value;
	}
}
