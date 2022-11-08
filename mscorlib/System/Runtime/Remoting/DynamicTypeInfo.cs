using System;

namespace System.Runtime.Remoting
{
	// Token: 0x02000734 RID: 1844
	[Serializable]
	internal class DynamicTypeInfo : TypeInfo
	{
		// Token: 0x06004206 RID: 16902 RVA: 0x000E08E5 File Offset: 0x000DF8E5
		internal DynamicTypeInfo(Type typeOfObj) : base(typeOfObj)
		{
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x000E08EE File Offset: 0x000DF8EE
		public override bool CanCastTo(Type castType, object o)
		{
			return ((MarshalByRefObject)o).IsInstanceOfType(castType);
		}
	}
}
