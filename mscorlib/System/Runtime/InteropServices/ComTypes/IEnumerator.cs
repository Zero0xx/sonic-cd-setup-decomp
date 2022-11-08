using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056E RID: 1390
	[Guid("496B0ABF-CDEE-11d3-88E8-00902754C43A")]
	internal interface IEnumerator
	{
		// Token: 0x060033CD RID: 13261
		bool MoveNext();

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060033CE RID: 13262
		object Current { get; }

		// Token: 0x060033CF RID: 13263
		void Reset();
	}
}
