using System;
using System.ComponentModel;

namespace System.Net.Security
{
	// Token: 0x02000535 RID: 1333
	internal class ProtocolToken
	{
		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x000A95F4 File Offset: 0x000A85F4
		internal bool Failed
		{
			get
			{
				return this.Status != SecurityStatus.OK && this.Status != SecurityStatus.ContinueNeeded;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x000A9610 File Offset: 0x000A8610
		internal bool Done
		{
			get
			{
				return this.Status == SecurityStatus.OK;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x000A961B File Offset: 0x000A861B
		internal bool Renegotiate
		{
			get
			{
				return this.Status == SecurityStatus.Renegotiate;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060028C7 RID: 10439 RVA: 0x000A962A File Offset: 0x000A862A
		internal bool CloseConnection
		{
			get
			{
				return this.Status == SecurityStatus.ContextExpired;
			}
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000A9639 File Offset: 0x000A8639
		internal ProtocolToken(byte[] data, SecurityStatus errorCode)
		{
			this.Status = errorCode;
			this.Payload = data;
			this.Size = ((data != null) ? data.Length : 0);
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000A965E File Offset: 0x000A865E
		internal Win32Exception GetException()
		{
			if (!this.Done)
			{
				return new Win32Exception((int)this.Status);
			}
			return null;
		}

		// Token: 0x040027B4 RID: 10164
		internal SecurityStatus Status;

		// Token: 0x040027B5 RID: 10165
		internal byte[] Payload;

		// Token: 0x040027B6 RID: 10166
		internal int Size;
	}
}
