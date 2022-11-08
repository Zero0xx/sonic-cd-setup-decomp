using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E6 RID: 2022
	internal sealed class MemberReference : IStreamable
	{
		// Token: 0x06004781 RID: 18305 RVA: 0x000F52FC File Offset: 0x000F42FC
		internal MemberReference()
		{
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x000F5304 File Offset: 0x000F4304
		internal void Set(int idRef)
		{
			this.idRef = idRef;
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x000F530D File Offset: 0x000F430D
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(9);
			sout.WriteInt32(this.idRef);
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x000F5323 File Offset: 0x000F4323
		public void Read(__BinaryParser input)
		{
			this.idRef = input.ReadInt32();
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x000F5331 File Offset: 0x000F4331
		public void Dump()
		{
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x000F5333 File Offset: 0x000F4333
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x04002437 RID: 9271
		internal int idRef;
	}
}
