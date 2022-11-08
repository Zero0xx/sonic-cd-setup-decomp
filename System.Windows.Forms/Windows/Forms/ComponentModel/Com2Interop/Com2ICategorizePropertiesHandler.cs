using System;
using System.ComponentModel;
using System.Globalization;
using System.Security;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000758 RID: 1880
	[SuppressUnmanagedCodeSecurity]
	internal class Com2ICategorizePropertiesHandler : Com2ExtendedBrowsingHandler
	{
		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x060063C7 RID: 25543 RVA: 0x0016C383 File Offset: 0x0016B383
		public override Type Interface
		{
			get
			{
				return typeof(NativeMethods.ICategorizeProperties);
			}
		}

		// Token: 0x060063C8 RID: 25544 RVA: 0x0016C390 File Offset: 0x0016B390
		private string GetCategoryFromObject(object obj, int dispid)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is NativeMethods.ICategorizeProperties)
			{
				NativeMethods.ICategorizeProperties categorizeProperties = (NativeMethods.ICategorizeProperties)obj;
				try
				{
					int propcat = 0;
					if (categorizeProperties.MapPropertyToCategory(dispid, ref propcat) == 0)
					{
						string result = null;
						switch (propcat)
						{
						case -11:
							return SR.GetString("PropertyCategoryDDE");
						case -10:
							return SR.GetString("PropertyCategoryScale");
						case -9:
							return SR.GetString("PropertyCategoryText");
						case -8:
							return SR.GetString("PropertyCategoryList");
						case -7:
							return SR.GetString("PropertyCategoryData");
						case -6:
							return SR.GetString("PropertyCategoryBehavior");
						case -5:
							return SR.GetString("PropertyCategoryAppearance");
						case -4:
							return SR.GetString("PropertyCategoryPosition");
						case -3:
							return SR.GetString("PropertyCategoryFont");
						case -2:
							return SR.GetString("PropertyCategoryMisc");
						case -1:
							return "";
						default:
							if (categorizeProperties.GetCategoryName(propcat, CultureInfo.CurrentCulture.LCID, out result) == 0)
							{
								return result;
							}
							break;
						}
					}
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x060063C9 RID: 25545 RVA: 0x0016C4C8 File Offset: 0x0016B4C8
		public override void SetupPropertyHandlers(Com2PropertyDescriptor[] propDesc)
		{
			if (propDesc == null)
			{
				return;
			}
			for (int i = 0; i < propDesc.Length; i++)
			{
				propDesc[i].QueryGetBaseAttributes += this.OnGetAttributes;
			}
		}

		// Token: 0x060063CA RID: 25546 RVA: 0x0016C4FC File Offset: 0x0016B4FC
		private void OnGetAttributes(Com2PropertyDescriptor sender, GetAttributesEvent attrEvent)
		{
			string categoryFromObject = this.GetCategoryFromObject(sender.TargetObject, sender.DISPID);
			if (categoryFromObject != null && categoryFromObject.Length > 0)
			{
				attrEvent.Add(new CategoryAttribute(categoryFromObject));
			}
		}
	}
}
