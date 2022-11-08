using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000750 RID: 1872
	internal class Com2AboutBoxPropertyDescriptor : Com2PropertyDescriptor
	{
		// Token: 0x0600639D RID: 25501 RVA: 0x0016BD10 File Offset: 0x0016AD10
		public Com2AboutBoxPropertyDescriptor() : base(-552, "About", new Attribute[]
		{
			new DispIdAttribute(-552),
			DesignerSerializationVisibilityAttribute.Hidden,
			new DescriptionAttribute(SR.GetString("AboutBoxDesc")),
			new ParenthesizePropertyNameAttribute(true)
		}, true, typeof(string), null, false)
		{
		}

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x0600639E RID: 25502 RVA: 0x0016BD72 File Offset: 0x0016AD72
		public override Type ComponentType
		{
			get
			{
				return typeof(UnsafeNativeMethods.IDispatch);
			}
		}

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x0600639F RID: 25503 RVA: 0x0016BD7E File Offset: 0x0016AD7E
		public override TypeConverter Converter
		{
			get
			{
				if (this.converter == null)
				{
					this.converter = new TypeConverter();
				}
				return this.converter;
			}
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x060063A0 RID: 25504 RVA: 0x0016BD99 File Offset: 0x0016AD99
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x060063A1 RID: 25505 RVA: 0x0016BD9C File Offset: 0x0016AD9C
		public override Type PropertyType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x060063A2 RID: 25506 RVA: 0x0016BDA8 File Offset: 0x0016ADA8
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x060063A3 RID: 25507 RVA: 0x0016BDAB File Offset: 0x0016ADAB
		public override object GetEditor(Type editorBaseType)
		{
			if (editorBaseType == typeof(UITypeEditor) && this.editor == null)
			{
				this.editor = new Com2AboutBoxPropertyDescriptor.AboutBoxUITypeEditor();
			}
			return this.editor;
		}

		// Token: 0x060063A4 RID: 25508 RVA: 0x0016BDD3 File Offset: 0x0016ADD3
		public override object GetValue(object component)
		{
			return "";
		}

		// Token: 0x060063A5 RID: 25509 RVA: 0x0016BDDA File Offset: 0x0016ADDA
		public override void ResetValue(object component)
		{
		}

		// Token: 0x060063A6 RID: 25510 RVA: 0x0016BDDC File Offset: 0x0016ADDC
		public override void SetValue(object component, object value)
		{
			throw new ArgumentException();
		}

		// Token: 0x060063A7 RID: 25511 RVA: 0x0016BDE3 File Offset: 0x0016ADE3
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x04003B7F RID: 15231
		private TypeConverter converter;

		// Token: 0x04003B80 RID: 15232
		private UITypeEditor editor;

		// Token: 0x02000751 RID: 1873
		public class AboutBoxUITypeEditor : UITypeEditor
		{
			// Token: 0x060063A8 RID: 25512 RVA: 0x0016BDE8 File Offset: 0x0016ADE8
			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				object instance = context.Instance;
				if (Marshal.IsComObject(instance) && instance is UnsafeNativeMethods.IDispatch)
				{
					UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)instance;
					NativeMethods.tagEXCEPINFO pExcepInfo = new NativeMethods.tagEXCEPINFO();
					Guid empty = Guid.Empty;
					dispatch.Invoke(-552, ref empty, SafeNativeMethods.GetThreadLCID(), 1, new NativeMethods.tagDISPPARAMS(), null, pExcepInfo, null);
				}
				return value;
			}

			// Token: 0x060063A9 RID: 25513 RVA: 0x0016BE3C File Offset: 0x0016AE3C
			public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}
		}
	}
}
