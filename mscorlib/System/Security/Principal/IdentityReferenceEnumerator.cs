using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x02000911 RID: 2321
	[ComVisible(false)]
	internal class IdentityReferenceEnumerator : IEnumerator<IdentityReference>, IDisposable, IEnumerator
	{
		// Token: 0x06005423 RID: 21539 RVA: 0x0012F470 File Offset: 0x0012E470
		internal IdentityReferenceEnumerator(IdentityReferenceCollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this._Collection = collection;
			this._Current = -1;
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005424 RID: 21540 RVA: 0x0012F494 File Offset: 0x0012E494
		object IEnumerator.Current
		{
			get
			{
				return this._Collection.Identities[this._Current];
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005425 RID: 21541 RVA: 0x0012F4AC File Offset: 0x0012E4AC
		public IdentityReference Current
		{
			get
			{
				return ((IEnumerator)this).Current as IdentityReference;
			}
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x0012F4B9 File Offset: 0x0012E4B9
		public bool MoveNext()
		{
			this._Current++;
			return this._Current < this._Collection.Count;
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x0012F4DC File Offset: 0x0012E4DC
		public void Reset()
		{
			this._Current = -1;
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x0012F4E5 File Offset: 0x0012E4E5
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		// Token: 0x06005429 RID: 21545 RVA: 0x0012F4ED File Offset: 0x0012E4ED
		protected void Dispose()
		{
		}

		// Token: 0x04002B8A RID: 11146
		private int _Current;

		// Token: 0x04002B8B RID: 11147
		private readonly IdentityReferenceCollection _Collection;
	}
}
