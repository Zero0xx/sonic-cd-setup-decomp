using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200033A RID: 826
	[ComVisible(true)]
	[Serializable]
	public struct ParameterModifier
	{
		// Token: 0x06001FB0 RID: 8112 RVA: 0x0004FA17 File Offset: 0x0004EA17
		public ParameterModifier(int parameterCount)
		{
			if (parameterCount <= 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ParmArraySize"));
			}
			this._byRef = new bool[parameterCount];
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x0004FA39 File Offset: 0x0004EA39
		internal bool[] IsByRefArray
		{
			get
			{
				return this._byRef;
			}
		}

		// Token: 0x17000541 RID: 1345
		public bool this[int index]
		{
			get
			{
				return this._byRef[index];
			}
			set
			{
				this._byRef[index] = value;
			}
		}

		// Token: 0x04000DAD RID: 3501
		private bool[] _byRef;
	}
}
