using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000752 RID: 1874
	internal abstract class Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x060063AB RID: 25515 RVA: 0x0016BE47 File Offset: 0x0016AE47
		public virtual bool AllowExpand
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x060063AC RID: 25516
		public abstract Type ManagedType { get; }

		// Token: 0x060063AD RID: 25517
		public abstract object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd);

		// Token: 0x060063AE RID: 25518
		public abstract object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet);
	}
}
