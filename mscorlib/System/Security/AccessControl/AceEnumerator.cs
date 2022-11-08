using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x020008D4 RID: 2260
	public sealed class AceEnumerator : IEnumerator
	{
		// Token: 0x0600521A RID: 21018 RVA: 0x001277AB File Offset: 0x001267AB
		internal AceEnumerator(GenericAcl collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this._acl = collection;
			this.Reset();
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x0600521B RID: 21019 RVA: 0x001277CE File Offset: 0x001267CE
		object IEnumerator.Current
		{
			get
			{
				if (this._current == -1 || this._current >= this._acl.Count)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_InvalidOperationException"));
				}
				return this._acl[this._current];
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x0600521C RID: 21020 RVA: 0x0012780D File Offset: 0x0012680D
		public GenericAce Current
		{
			get
			{
				return ((IEnumerator)this).Current as GenericAce;
			}
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x0012781A File Offset: 0x0012681A
		public bool MoveNext()
		{
			this._current++;
			return this._current < this._acl.Count;
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x0012783D File Offset: 0x0012683D
		public void Reset()
		{
			this._current = -1;
		}

		// Token: 0x04002A77 RID: 10871
		private int _current;

		// Token: 0x04002A78 RID: 10872
		private readonly GenericAcl _acl;
	}
}
