using System;
using System.Reflection;

namespace System.Runtime.InteropServices.Expando
{
	// Token: 0x0200059D RID: 1437
	[Guid("AFBF15E6-C37C-11d2-B88E-00A0C9B471B8")]
	[ComVisible(true)]
	public interface IExpando : IReflect
	{
		// Token: 0x0600346F RID: 13423
		FieldInfo AddField(string name);

		// Token: 0x06003470 RID: 13424
		PropertyInfo AddProperty(string name);

		// Token: 0x06003471 RID: 13425
		MethodInfo AddMethod(string name, Delegate method);

		// Token: 0x06003472 RID: 13426
		void RemoveMember(MemberInfo m);
	}
}
