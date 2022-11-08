using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200072C RID: 1836
	internal class SmuggledObjRef
	{
		// Token: 0x060041D4 RID: 16852 RVA: 0x000E0123 File Offset: 0x000DF123
		public SmuggledObjRef(ObjRef objRef)
		{
			this._objRef = objRef;
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060041D5 RID: 16853 RVA: 0x000E0132 File Offset: 0x000DF132
		public ObjRef ObjRef
		{
			get
			{
				return this._objRef;
			}
		}

		// Token: 0x04002105 RID: 8453
		private ObjRef _objRef;
	}
}
