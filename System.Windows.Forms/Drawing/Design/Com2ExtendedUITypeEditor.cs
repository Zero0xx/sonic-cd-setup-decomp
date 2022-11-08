using System;
using System.ComponentModel;

namespace System.Drawing.Design
{
	// Token: 0x02000756 RID: 1878
	internal class Com2ExtendedUITypeEditor : UITypeEditor
	{
		// Token: 0x060063BB RID: 25531 RVA: 0x0016C183 File Offset: 0x0016B183
		public Com2ExtendedUITypeEditor(UITypeEditor baseTypeEditor)
		{
			this.innerEditor = baseTypeEditor;
		}

		// Token: 0x060063BC RID: 25532 RVA: 0x0016C192 File Offset: 0x0016B192
		public Com2ExtendedUITypeEditor(Type baseType)
		{
			this.innerEditor = (UITypeEditor)TypeDescriptor.GetEditor(baseType, typeof(UITypeEditor));
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x060063BD RID: 25533 RVA: 0x0016C1B5 File Offset: 0x0016B1B5
		public UITypeEditor InnerEditor
		{
			get
			{
				return this.innerEditor;
			}
		}

		// Token: 0x060063BE RID: 25534 RVA: 0x0016C1BD File Offset: 0x0016B1BD
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (this.innerEditor != null)
			{
				return this.innerEditor.EditValue(context, provider, value);
			}
			return base.EditValue(context, provider, value);
		}

		// Token: 0x060063BF RID: 25535 RVA: 0x0016C1DF File Offset: 0x0016B1DF
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			if (this.innerEditor != null)
			{
				return this.innerEditor.GetPaintValueSupported(context);
			}
			return base.GetPaintValueSupported(context);
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x0016C1FD File Offset: 0x0016B1FD
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (this.innerEditor != null)
			{
				return this.innerEditor.GetEditStyle(context);
			}
			return base.GetEditStyle(context);
		}

		// Token: 0x060063C1 RID: 25537 RVA: 0x0016C21B File Offset: 0x0016B21B
		public override void PaintValue(PaintValueEventArgs e)
		{
			if (this.innerEditor != null)
			{
				this.innerEditor.PaintValue(e);
			}
			base.PaintValue(e);
		}

		// Token: 0x04003B81 RID: 15233
		private UITypeEditor innerEditor;
	}
}
