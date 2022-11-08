using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000200 RID: 512
	internal class StoreCategoryInstanceEnumeration : IEnumerator
	{
		// Token: 0x0600155E RID: 5470 RVA: 0x00037195 File Offset: 0x00036195
		public StoreCategoryInstanceEnumeration(IEnumSTORE_CATEGORY_INSTANCE pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x000371A4 File Offset: 0x000361A4
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x000371A7 File Offset: 0x000361A7
		private STORE_CATEGORY_INSTANCE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x000371BD File Offset: 0x000361BD
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x000371CA File Offset: 0x000361CA
		public STORE_CATEGORY_INSTANCE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x000371D4 File Offset: 0x000361D4
		public bool MoveNext()
		{
			STORE_CATEGORY_INSTANCE[] array = new STORE_CATEGORY_INSTANCE[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00037219 File Offset: 0x00036219
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000872 RID: 2162
		private IEnumSTORE_CATEGORY_INSTANCE _enum;

		// Token: 0x04000873 RID: 2163
		private bool _fValid;

		// Token: 0x04000874 RID: 2164
		private STORE_CATEGORY_INSTANCE _current;
	}
}
