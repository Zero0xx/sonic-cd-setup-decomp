using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x0200015B RID: 347
	[ComVisible(true)]
	public struct LockCookie
	{
		// Token: 0x0600128D RID: 4749 RVA: 0x000335CE File Offset: 0x000325CE
		public override int GetHashCode()
		{
			return this._dwFlags + this._dwWriterSeqNum + this._wReaderAndWriterLevel + this._dwThreadID;
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x000335EB File Offset: 0x000325EB
		public override bool Equals(object obj)
		{
			return obj is LockCookie && this.Equals((LockCookie)obj);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00033603 File Offset: 0x00032603
		public bool Equals(LockCookie obj)
		{
			return obj._dwFlags == this._dwFlags && obj._dwWriterSeqNum == this._dwWriterSeqNum && obj._wReaderAndWriterLevel == this._wReaderAndWriterLevel && obj._dwThreadID == this._dwThreadID;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00033643 File Offset: 0x00032643
		public static bool operator ==(LockCookie a, LockCookie b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0003364D File Offset: 0x0003264D
		public static bool operator !=(LockCookie a, LockCookie b)
		{
			return !(a == b);
		}

		// Token: 0x0400065E RID: 1630
		private int _dwFlags;

		// Token: 0x0400065F RID: 1631
		private int _dwWriterSeqNum;

		// Token: 0x04000660 RID: 1632
		private int _wReaderAndWriterLevel;

		// Token: 0x04000661 RID: 1633
		private int _dwThreadID;
	}
}
