using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FE RID: 510
	internal class StoreSubcategoryEnumeration : IEnumerator
	{
		// Token: 0x06001553 RID: 5459 RVA: 0x000370FD File Offset: 0x000360FD
		public StoreSubcategoryEnumeration(IEnumSTORE_CATEGORY_SUBCATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0003710C File Offset: 0x0003610C
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0003710F File Offset: 0x0003610F
		private STORE_CATEGORY_SUBCATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x00037125 File Offset: 0x00036125
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001557 RID: 5463 RVA: 0x00037132 File Offset: 0x00036132
		public STORE_CATEGORY_SUBCATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0003713C File Offset: 0x0003613C
		public bool MoveNext()
		{
			STORE_CATEGORY_SUBCATEGORY[] array = new STORE_CATEGORY_SUBCATEGORY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00037181 File Offset: 0x00036181
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x0400086F RID: 2159
		private IEnumSTORE_CATEGORY_SUBCATEGORY _enum;

		// Token: 0x04000870 RID: 2160
		private bool _fValid;

		// Token: 0x04000871 RID: 2161
		private STORE_CATEGORY_SUBCATEGORY _current;
	}
}
