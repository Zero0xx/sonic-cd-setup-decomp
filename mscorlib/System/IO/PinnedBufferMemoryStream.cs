using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005C3 RID: 1475
	internal sealed class PinnedBufferMemoryStream : UnmanagedMemoryStream
	{
		// Token: 0x060036C5 RID: 14021 RVA: 0x000B950C File Offset: 0x000B850C
		internal unsafe PinnedBufferMemoryStream(byte[] array)
		{
			int num = array.Length;
			if (num == 0)
			{
				array = new byte[1];
				num = 0;
			}
			this._array = array;
			this._pinningHandle = new GCHandle(array, GCHandleType.Pinned);
			fixed (byte* array2 = this._array)
			{
				base.Initialize(array2, (long)num, (long)num, FileAccess.Read, true);
			}
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000B9574 File Offset: 0x000B8574
		~PinnedBufferMemoryStream()
		{
			this.Dispose(false);
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x000B95A4 File Offset: 0x000B85A4
		protected override void Dispose(bool disposing)
		{
			if (this._isOpen)
			{
				this._pinningHandle.Free();
				this._isOpen = false;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04001CA6 RID: 7334
		private byte[] _array;

		// Token: 0x04001CA7 RID: 7335
		private GCHandle _pinningHandle;
	}
}
