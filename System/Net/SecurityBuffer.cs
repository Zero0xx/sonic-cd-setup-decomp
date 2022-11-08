using System;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000407 RID: 1031
	internal class SecurityBuffer
	{
		// Token: 0x060020AE RID: 8366 RVA: 0x00080D10 File Offset: 0x0007FD10
		public SecurityBuffer(byte[] data, int offset, int size, BufferType tokentype)
		{
			this.offset = ((data == null || offset < 0) ? 0 : Math.Min(offset, data.Length));
			this.size = ((data == null || size < 0) ? 0 : Math.Min(size, data.Length - this.offset));
			this.type = tokentype;
			this.token = ((size == 0) ? null : data);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00080D71 File Offset: 0x0007FD71
		public SecurityBuffer(byte[] data, BufferType tokentype)
		{
			this.size = ((data == null) ? 0 : data.Length);
			this.type = tokentype;
			this.token = ((this.size == 0) ? null : data);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x00080DA1 File Offset: 0x0007FDA1
		public SecurityBuffer(int size, BufferType tokentype)
		{
			this.size = size;
			this.type = tokentype;
			this.token = ((size == 0) ? null : new byte[size]);
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00080DC9 File Offset: 0x0007FDC9
		public SecurityBuffer(ChannelBinding binding)
		{
			this.size = ((binding == null) ? 0 : binding.Size);
			this.type = BufferType.ChannelBindings;
			this.unmanagedToken = binding;
		}

		// Token: 0x0400208B RID: 8331
		public int size;

		// Token: 0x0400208C RID: 8332
		public BufferType type;

		// Token: 0x0400208D RID: 8333
		public byte[] token;

		// Token: 0x0400208E RID: 8334
		public SafeHandle unmanagedToken;

		// Token: 0x0400208F RID: 8335
		public int offset;
	}
}
