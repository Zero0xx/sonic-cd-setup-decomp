using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x0200009F RID: 159
	[DesignerCategory("Component")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	public class Component : MarshalByRefObject, IComponent, IDisposable
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x000174A0 File Offset: 0x000164A0
		~Component()
		{
			this.Dispose(false);
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x000174D0 File Offset: 0x000164D0
		protected virtual bool CanRaiseEvents
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x000174D3 File Offset: 0x000164D3
		internal bool CanRaiseEventsInternal
		{
			get
			{
				return this.CanRaiseEvents;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060005A5 RID: 1445 RVA: 0x000174DB File Offset: 0x000164DB
		// (remove) Token: 0x060005A6 RID: 1446 RVA: 0x000174EE File Offset: 0x000164EE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Disposed
		{
			add
			{
				this.Events.AddHandler(Component.EventDisposed, value);
			}
			remove
			{
				this.Events.RemoveHandler(Component.EventDisposed, value);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00017501 File Offset: 0x00016501
		protected EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList(this);
				}
				return this.events;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001751D File Offset: 0x0001651D
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00017525 File Offset: 0x00016525
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual ISite Site
		{
			get
			{
				return this.site;
			}
			set
			{
				this.site = value;
			}
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001752E File Offset: 0x0001652E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00017540 File Offset: 0x00016540
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this)
				{
					if (this.site != null && this.site.Container != null)
					{
						this.site.Container.Remove(this);
					}
					if (this.events != null)
					{
						EventHandler eventHandler = (EventHandler)this.events[Component.EventDisposed];
						if (eventHandler != null)
						{
							eventHandler(this, EventArgs.Empty);
						}
					}
				}
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x000175C4 File Offset: 0x000165C4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public IContainer Container
		{
			get
			{
				ISite site = this.site;
				if (site != null)
				{
					return site.Container;
				}
				return null;
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000175E4 File Offset: 0x000165E4
		protected virtual object GetService(Type service)
		{
			ISite site = this.site;
			if (site != null)
			{
				return site.GetService(service);
			}
			return null;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00017604 File Offset: 0x00016604
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected bool DesignMode
		{
			get
			{
				ISite site = this.site;
				return site != null && site.DesignMode;
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00017624 File Offset: 0x00016624
		public override string ToString()
		{
			ISite site = this.site;
			if (site != null)
			{
				return site.Name + " [" + base.GetType().FullName + "]";
			}
			return base.GetType().FullName;
		}

		// Token: 0x040008DF RID: 2271
		private static readonly object EventDisposed = new object();

		// Token: 0x040008E0 RID: 2272
		private ISite site;

		// Token: 0x040008E1 RID: 2273
		private EventHandlerList events;
	}
}
