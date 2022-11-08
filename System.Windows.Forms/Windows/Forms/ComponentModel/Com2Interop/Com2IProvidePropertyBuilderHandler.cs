using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200075E RID: 1886
	[SuppressUnmanagedCodeSecurity]
	internal class Com2IProvidePropertyBuilderHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x060063ED RID: 25581 RVA: 0x0016D016 File Offset: 0x0016C016
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.IProvidePropertyBuilder);
			}
		}

		// Token: 0x060063EE RID: 25582 RVA: 0x0016D024 File Offset: 0x0016C024
		private bool GetBuilderGuidString(NativeMethods.IProvidePropertyBuilder target, int dispid, ref string strGuidBldr, int[] bldrType)
		{
			bool flag = false;
			string[] array = new string[1];
			if (NativeMethods.Failed(target.MapPropertyToBuilder(dispid, bldrType, array, ref flag)))
			{
				flag = false;
			}
			if (flag && (bldrType[0] & 2) == 0)
			{
				flag = false;
			}
			if (!flag)
			{
				return false;
			}
			if (array[0] == null)
			{
				strGuidBldr = Guid.Empty.ToString();
			}
			else
			{
				strGuidBldr = array[0];
			}
			return true;
		}

		// Token: 0x060063EF RID: 25583 RVA: 0x0016D084 File Offset: 0x0016C084
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetBaseAttributes += this.OnGetBaseAttributes;
				propDesc[i].QueryGetTypeConverterAndTypeEditor += this.OnGetTypeConverterAndTypeEditor;
			}
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x0016D0CC File Offset: 0x0016C0CC
		private void OnGetBaseAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			NativeMethods.IProvidePropertyBuilder providePropertyBuilder = sender.TargetObject as NativeMethods.IProvidePropertyBuilder;
			if (providePropertyBuilder != null)
			{
				string text = null;
				bool builderGuidString = this.GetBuilderGuidString(providePropertyBuilder, sender.DISPID, ref text, new int[1]);
				if (sender.CanShow && builderGuidString && typeof(UnsafeNativeMethods.IDispatch).IsAssignableFrom(sender.PropertyType))
				{
					attrEvent.Add(BrowsableAttribute.Yes);
				}
			}
		}

		// Token: 0x060063F1 RID: 25585 RVA: 0x0016D130 File Offset: 0x0016C130
		private void OnGetTypeConverterAndTypeEditor(Com2PropertyDescriptor sender, GetTypeConverterAndTypeEditorEvent gveevent)
		{
			object targetObject = sender.TargetObject;
			if (targetObject is NativeMethods.IProvidePropertyBuilder)
			{
				NativeMethods.IProvidePropertyBuilder target = (NativeMethods.IProvidePropertyBuilder)targetObject;
				int[] array = new int[1];
				string guidString = null;
				if (this.GetBuilderGuidString(target, sender.DISPID, ref guidString, array))
				{
					gveevent.TypeEditor = new Com2PropertyBuilderUITypeEditor(sender, guidString, array[0], (UITypeEditor)gveevent.TypeEditor);
				}
			}
		}
	}
}
