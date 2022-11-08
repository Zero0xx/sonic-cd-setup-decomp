using System;
using System.Collections;

namespace System.Security
{
	// Token: 0x02000677 RID: 1655
	internal class PermissionSetEnumerator : IEnumerator
	{
		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06003BDD RID: 15325 RVA: 0x000CC3E3 File Offset: 0x000CB3E3
		public object Current
		{
			get
			{
				return this.enm.Current;
			}
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x000CC3F0 File Offset: 0x000CB3F0
		public bool MoveNext()
		{
			return this.enm.MoveNext();
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x000CC3FD File Offset: 0x000CB3FD
		public void Reset()
		{
			this.enm.Reset();
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x000CC40A File Offset: 0x000CB40A
		internal PermissionSetEnumerator(PermissionSet permSet)
		{
			this.enm = new PermissionSetEnumeratorInternal(permSet);
		}

		// Token: 0x04001EDD RID: 7901
		private PermissionSetEnumeratorInternal enm;
	}
}
