using System;
using System.Diagnostics;
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E8 RID: 2024
	internal sealed class MessageEnd : IStreamable
	{
		// Token: 0x0600478E RID: 18318 RVA: 0x000F542D File Offset: 0x000F442D
		internal MessageEnd()
		{
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x000F5435 File Offset: 0x000F4435
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(11);
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x000F543F File Offset: 0x000F443F
		public void Read(__BinaryParser input)
		{
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x000F5441 File Offset: 0x000F4441
		public void Dump()
		{
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x000F5443 File Offset: 0x000F4443
		public void Dump(Stream sout)
		{
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x000F5445 File Offset: 0x000F4445
		[Conditional("_LOGGING")]
		private void DumpInternal(Stream sout)
		{
			if (BCLDebug.CheckEnabled("BINARY") && sout != null && sout.CanSeek)
			{
				long length = sout.Length;
			}
		}
	}
}
