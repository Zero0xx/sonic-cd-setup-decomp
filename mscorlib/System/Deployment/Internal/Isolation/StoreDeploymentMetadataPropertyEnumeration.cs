using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F6 RID: 502
	internal class StoreDeploymentMetadataPropertyEnumeration : IEnumerator
	{
		// Token: 0x06001527 RID: 5415 RVA: 0x00036E9C File Offset: 0x00035E9C
		public StoreDeploymentMetadataPropertyEnumeration(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x00036EAB File Offset: 0x00035EAB
		private StoreOperationMetadataProperty GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x00036EC1 File Offset: 0x00035EC1
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x00036ECE File Offset: 0x00035ECE
		public StoreOperationMetadataProperty Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x00036ED6 File Offset: 0x00035ED6
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00036EDC File Offset: 0x00035EDC
		public bool MoveNext()
		{
			StoreOperationMetadataProperty[] array = new StoreOperationMetadataProperty[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00036F21 File Offset: 0x00035F21
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000863 RID: 2147
		private IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY _enum;

		// Token: 0x04000864 RID: 2148
		private bool _fValid;

		// Token: 0x04000865 RID: 2149
		private StoreOperationMetadataProperty _current;
	}
}
