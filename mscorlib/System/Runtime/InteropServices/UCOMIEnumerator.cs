using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053B RID: 1339
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumerator instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("496B0ABF-CDEE-11d3-88E8-00902754C43A")]
	internal interface UCOMIEnumerator
	{
		// Token: 0x06003345 RID: 13125
		bool MoveNext();

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06003346 RID: 13126
		object Current { get; }

		// Token: 0x06003347 RID: 13127
		void Reset();
	}
}
