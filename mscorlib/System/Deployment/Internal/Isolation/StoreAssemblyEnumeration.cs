using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F8 RID: 504
	internal class StoreAssemblyEnumeration : IEnumerator
	{
		// Token: 0x06001532 RID: 5426 RVA: 0x00036F35 File Offset: 0x00035F35
		public StoreAssemblyEnumeration(IEnumSTORE_ASSEMBLY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x00036F44 File Offset: 0x00035F44
		private STORE_ASSEMBLY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x00036F5A File Offset: 0x00035F5A
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x00036F67 File Offset: 0x00035F67
		public STORE_ASSEMBLY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00036F6F File Offset: 0x00035F6F
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00036F74 File Offset: 0x00035F74
		public bool MoveNext()
		{
			STORE_ASSEMBLY[] array = new STORE_ASSEMBLY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x00036FB9 File Offset: 0x00035FB9
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000866 RID: 2150
		private IEnumSTORE_ASSEMBLY _enum;

		// Token: 0x04000867 RID: 2151
		private bool _fValid;

		// Token: 0x04000868 RID: 2152
		private STORE_ASSEMBLY _current;
	}
}
