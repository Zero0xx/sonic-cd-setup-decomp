using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FA RID: 506
	internal class StoreAssemblyFileEnumeration : IEnumerator
	{
		// Token: 0x0600153D RID: 5437 RVA: 0x00036FCD File Offset: 0x00035FCD
		public StoreAssemblyFileEnumeration(IEnumSTORE_ASSEMBLY_FILE pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00036FDC File Offset: 0x00035FDC
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00036FDF File Offset: 0x00035FDF
		private STORE_ASSEMBLY_FILE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x00036FF5 File Offset: 0x00035FF5
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x00037002 File Offset: 0x00036002
		public STORE_ASSEMBLY_FILE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0003700C File Offset: 0x0003600C
		public bool MoveNext()
		{
			STORE_ASSEMBLY_FILE[] array = new STORE_ASSEMBLY_FILE[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00037051 File Offset: 0x00036051
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000869 RID: 2153
		private IEnumSTORE_ASSEMBLY_FILE _enum;

		// Token: 0x0400086A RID: 2154
		private bool _fValid;

		// Token: 0x0400086B RID: 2155
		private STORE_ASSEMBLY_FILE _current;
	}
}
