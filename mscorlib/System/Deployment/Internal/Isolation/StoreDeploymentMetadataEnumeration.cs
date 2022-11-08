using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F4 RID: 500
	internal class StoreDeploymentMetadataEnumeration : IEnumerator
	{
		// Token: 0x0600151C RID: 5404 RVA: 0x00036E12 File Offset: 0x00035E12
		public StoreDeploymentMetadataEnumeration(IEnumSTORE_DEPLOYMENT_METADATA pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00036E21 File Offset: 0x00035E21
		private IDefinitionAppId GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x00036E37 File Offset: 0x00035E37
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x00036E3F File Offset: 0x00035E3F
		public IDefinitionAppId Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00036E47 File Offset: 0x00035E47
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00036E4C File Offset: 0x00035E4C
		public bool MoveNext()
		{
			IDefinitionAppId[] array = new IDefinitionAppId[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = (num == 1U);
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00036E88 File Offset: 0x00035E88
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000860 RID: 2144
		private IEnumSTORE_DEPLOYMENT_METADATA _enum;

		// Token: 0x04000861 RID: 2145
		private bool _fValid;

		// Token: 0x04000862 RID: 2146
		private IDefinitionAppId _current;
	}
}
