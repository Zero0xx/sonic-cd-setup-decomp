using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000521 RID: 1313
	[ComVisible(true)]
	public sealed class ExtensibleClassFactory
	{
		// Token: 0x060032DF RID: 13023 RVA: 0x000ABBF7 File Offset: 0x000AABF7
		private ExtensibleClassFactory()
		{
		}

		// Token: 0x060032E0 RID: 13024
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RegisterObjectCreationCallback(ObjectCreationDelegate callback);
	}
}
