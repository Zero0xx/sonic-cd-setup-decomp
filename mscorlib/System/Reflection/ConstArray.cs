using System;

namespace System.Reflection
{
	// Token: 0x02000321 RID: 801
	[Serializable]
	internal struct ConstArray
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x0004D607 File Offset: 0x0004C607
		public IntPtr Signature
		{
			get
			{
				return this.m_constArray;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x0004D60F File Offset: 0x0004C60F
		public int Length
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x17000507 RID: 1287
		public unsafe byte this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_length)
				{
					throw new IndexOutOfRangeException();
				}
				return ((byte*)this.m_constArray.ToPointer())[index];
			}
		}

		// Token: 0x04000D0A RID: 3338
		internal int m_length;

		// Token: 0x04000D0B RID: 3339
		internal IntPtr m_constArray;
	}
}
