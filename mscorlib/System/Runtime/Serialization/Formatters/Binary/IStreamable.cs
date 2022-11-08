using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D6 RID: 2006
	internal interface IStreamable
	{
		// Token: 0x06004727 RID: 18215
		void Read(__BinaryParser input);

		// Token: 0x06004728 RID: 18216
		void Write(__BinaryWriter sout);
	}
}
