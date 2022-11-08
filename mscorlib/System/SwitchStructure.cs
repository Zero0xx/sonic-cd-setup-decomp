using System;

namespace System
{
	// Token: 0x02000042 RID: 66
	internal struct SwitchStructure
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x0000FAE5 File Offset: 0x0000EAE5
		internal SwitchStructure(string n, int v)
		{
			this.name = n;
			this.value = v;
		}

		// Token: 0x04000177 RID: 375
		internal string name;

		// Token: 0x04000178 RID: 376
		internal int value;
	}
}
