using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200032A RID: 810
	// (Invoke) Token: 0x06001F09 RID: 7945
	[ComVisible(true)]
	[Serializable]
	public delegate bool MemberFilter(MemberInfo m, object filterCriteria);
}
