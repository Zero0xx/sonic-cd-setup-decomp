using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000374 RID: 884
	[ComVisible(true)]
	[Serializable]
	public abstract class SerializationBinder
	{
		// Token: 0x06002292 RID: 8850
		public abstract Type BindToType(string assemblyName, string typeName);
	}
}
