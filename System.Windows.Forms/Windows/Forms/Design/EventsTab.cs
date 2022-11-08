using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000780 RID: 1920
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class EventsTab : PropertyTab
	{
		// Token: 0x060064F4 RID: 25844 RVA: 0x001715C2 File Offset: 0x001705C2
		public EventsTab(IServiceProvider sp)
		{
			this.sp = sp;
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x060064F5 RID: 25845 RVA: 0x001715D1 File Offset: 0x001705D1
		public override string TabName
		{
			get
			{
				return SR.GetString("PBRSToolTipEvents");
			}
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x060064F6 RID: 25846 RVA: 0x001715DD File Offset: 0x001705DD
		public override string HelpKeyword
		{
			get
			{
				return "Events";
			}
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x001715E4 File Offset: 0x001705E4
		public override bool CanExtend(object extendee)
		{
			return !Marshal.IsComObject(extendee);
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x001715EF File Offset: 0x001705EF
		private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs adevent)
		{
			this.currentHost = adevent.NewDesigner;
		}

		// Token: 0x060064F9 RID: 25849 RVA: 0x00171600 File Offset: 0x00170600
		public override PropertyDescriptor GetDefaultProperty(object obj)
		{
			IEventBindingService eventPropertyService = this.GetEventPropertyService(obj, null);
			if (eventPropertyService == null)
			{
				return null;
			}
			EventDescriptor defaultEvent = TypeDescriptor.GetDefaultEvent(obj);
			if (defaultEvent != null)
			{
				return eventPropertyService.GetEventProperty(defaultEvent);
			}
			return null;
		}

		// Token: 0x060064FA RID: 25850 RVA: 0x00171630 File Offset: 0x00170630
		private IEventBindingService GetEventPropertyService(object obj, ITypeDescriptorContext context)
		{
			IEventBindingService eventBindingService = null;
			if (!this.sunkEvent)
			{
				IDesignerEventService designerEventService = (IDesignerEventService)this.sp.GetService(typeof(IDesignerEventService));
				if (designerEventService != null)
				{
					designerEventService.ActiveDesignerChanged += this.OnActiveDesignerChanged;
				}
				this.sunkEvent = true;
			}
			if (eventBindingService == null && this.currentHost != null)
			{
				eventBindingService = (IEventBindingService)this.currentHost.GetService(typeof(IEventBindingService));
			}
			if (eventBindingService == null && obj is IComponent)
			{
				ISite site = ((IComponent)obj).Site;
				if (site != null)
				{
					eventBindingService = (IEventBindingService)site.GetService(typeof(IEventBindingService));
				}
			}
			if (eventBindingService == null && context != null)
			{
				eventBindingService = (IEventBindingService)context.GetService(typeof(IEventBindingService));
			}
			return eventBindingService;
		}

		// Token: 0x060064FB RID: 25851 RVA: 0x001716F1 File Offset: 0x001706F1
		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			return this.GetProperties(null, component, attributes);
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x001716FC File Offset: 0x001706FC
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
		{
			IEventBindingService eventPropertyService = this.GetEventPropertyService(component, context);
			if (eventPropertyService == null)
			{
				return new PropertyDescriptorCollection(null);
			}
			EventDescriptorCollection events = TypeDescriptor.GetEvents(component, attributes);
			PropertyDescriptorCollection propertyDescriptorCollection = eventPropertyService.GetEventProperties(events);
			Attribute[] array = new Attribute[attributes.Length + 1];
			Array.Copy(attributes, 0, array, 0, attributes.Length);
			array[attributes.Length] = DesignerSerializationVisibilityAttribute.Content;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component, array);
			if (properties.Count > 0)
			{
				ArrayList arrayList = null;
				for (int i = 0; i < properties.Count; i++)
				{
					PropertyDescriptor propertyDescriptor = properties[i];
					TypeConverter converter = propertyDescriptor.Converter;
					if (converter.GetPropertiesSupported())
					{
						object value = propertyDescriptor.GetValue(component);
						EventDescriptorCollection events2 = TypeDescriptor.GetEvents(value, attributes);
						if (events2.Count > 0)
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							propertyDescriptor = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[]
							{
								MergablePropertyAttribute.No
							});
							arrayList.Add(propertyDescriptor);
						}
					}
				}
				if (arrayList != null)
				{
					PropertyDescriptor[] array2 = new PropertyDescriptor[arrayList.Count];
					arrayList.CopyTo(array2, 0);
					PropertyDescriptor[] array3 = new PropertyDescriptor[propertyDescriptorCollection.Count + array2.Length];
					propertyDescriptorCollection.CopyTo(array3, 0);
					Array.Copy(array2, 0, array3, propertyDescriptorCollection.Count, array2.Length);
					propertyDescriptorCollection = new PropertyDescriptorCollection(array3);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x04003C06 RID: 15366
		private IServiceProvider sp;

		// Token: 0x04003C07 RID: 15367
		private IDesignerHost currentHost;

		// Token: 0x04003C08 RID: 15368
		private bool sunkEvent;
	}
}
