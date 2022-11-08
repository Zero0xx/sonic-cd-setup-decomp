using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000755 RID: 1877
	internal abstract class Com2ExtendedBrowsingHandler
	{
		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x060063B7 RID: 25527
		public abstract Type Interface { get; }

		// Token: 0x060063B8 RID: 25528 RVA: 0x0016C15C File Offset: 0x0016B15C
		public virtual void SetupPropertyHandlers(Com2PropertyDescriptor propDesc)
		{
			this.SetupPropertyHandlers(new Com2PropertyDescriptor[]
			{
				propDesc
			});
		}

		// Token: 0x060063B9 RID: 25529
		public abstract void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc);
	}
}
