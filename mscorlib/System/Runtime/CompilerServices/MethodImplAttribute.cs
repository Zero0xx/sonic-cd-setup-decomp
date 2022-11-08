using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005ED RID: 1517
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class MethodImplAttribute : Attribute
	{
		// Token: 0x060037EC RID: 14316 RVA: 0x000BBC3C File Offset: 0x000BAC3C
		internal MethodImplAttribute(MethodImplAttributes methodImplAttributes)
		{
			MethodImplOptions methodImplOptions = MethodImplOptions.Unmanaged | MethodImplOptions.ForwardRef | MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall | MethodImplOptions.Synchronized | MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization;
			this._val = (MethodImplOptions)(methodImplAttributes & (MethodImplAttributes)methodImplOptions);
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x000BBC5E File Offset: 0x000BAC5E
		public MethodImplAttribute(MethodImplOptions methodImplOptions)
		{
			this._val = methodImplOptions;
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x000BBC6D File Offset: 0x000BAC6D
		public MethodImplAttribute(short value)
		{
			this._val = (MethodImplOptions)value;
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x000BBC7C File Offset: 0x000BAC7C
		public MethodImplAttribute()
		{
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060037F0 RID: 14320 RVA: 0x000BBC84 File Offset: 0x000BAC84
		public MethodImplOptions Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001CFC RID: 7420
		internal MethodImplOptions _val;

		// Token: 0x04001CFD RID: 7421
		public MethodCodeType MethodCodeType;
	}
}
