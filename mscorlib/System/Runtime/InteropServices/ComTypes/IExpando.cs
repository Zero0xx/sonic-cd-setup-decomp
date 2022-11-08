using System;
using System.Reflection;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000576 RID: 1398
	[Guid("AFBF15E6-C37C-11d2-B88E-00A0C9B471B8")]
	internal interface IExpando : IReflect
	{
		// Token: 0x060033F0 RID: 13296
		FieldInfo AddField(string name);

		// Token: 0x060033F1 RID: 13297
		PropertyInfo AddProperty(string name);

		// Token: 0x060033F2 RID: 13298
		MethodInfo AddMethod(string name, Delegate method);

		// Token: 0x060033F3 RID: 13299
		void RemoveMember(MemberInfo m);
	}
}
