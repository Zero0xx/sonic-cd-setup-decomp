using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FC RID: 508
	internal class StoreCategoryEnumeration : IEnumerator
	{
		// Token: 0x06001548 RID: 5448 RVA: 0x00037065 File Offset: 0x00036065
		public StoreCategoryEnumeration(IEnumSTORE_CATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00037074 File Offset: 0x00036074
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00037077 File Offset: 0x00036077
		private STORE_CATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x0003708D File Offset: 0x0003608D
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x0003709A File Offset: 0x0003609A
		public STORE_CATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x000370A4 File Offset: 0x000360A4
		public bool MoveNext()
		{
			STORE_CATEGORY[] array = new STORE_CATEGORY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x000370E9 File Offset: 0x000360E9
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x0400086C RID: 2156
		private IEnumSTORE_CATEGORY _enum;

		// Token: 0x0400086D RID: 2157
		private bool _fValid;

		// Token: 0x0400086E RID: 2158
		private STORE_CATEGORY _current;
	}
}
