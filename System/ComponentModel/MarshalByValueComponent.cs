using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x0200011A RID: 282
	[TypeConverter(typeof(ComponentConverter))]
	[ComVisible(true)]
	[DesignerCategory("Component")]
	[Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	public class MarshalByValueComponent : IComponent, IDisposable, IServiceProvider
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x0001D040 File Offset: 0x0001C040
		~MarshalByValueComponent()
		{
			this.Dispose(false);
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060008B5 RID: 2229 RVA: 0x0001D070 File Offset: 0x0001C070
		// (remove) Token: 0x060008B6 RID: 2230 RVA: 0x0001D083 File Offset: 0x0001C083
		public event EventHandler Disposed
		{
			add
			{
				this.Events.AddHandler(MarshalByValueComponent.EventDisposed, value);
			}
			remove
			{
				this.Events.RemoveHandler(MarshalByValueComponent.EventDisposed, value);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0001D096 File Offset: 0x0001C096
		protected EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0001D0B1 File Offset: 0x0001C0B1
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x0001D0B9 File Offset: 0x0001C0B9
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

		// Token: 0x060008BA RID: 2234 RVA: 0x0001D0C2 File Offset: 0x0001C0C2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001D0D4 File Offset: 0x0001C0D4
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
						EventHandler eventHandler = (EventHandler)this.events[MarshalByValueComponent.EventDisposed];
						if (eventHandler != null)
						{
							eventHandler(this, EventArgs.Empty);
						}
					}
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0001D158 File Offset: 0x0001C158
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IContainer Container
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

		// Token: 0x060008BD RID: 2237 RVA: 0x0001D177 File Offset: 0x0001C177
		public virtual object GetService(Type service)
		{
			if (this.site != null)
			{
				return this.site.GetService(service);
			}
			return null;
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0001D190 File Offset: 0x0001C190
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool DesignMode
		{
			get
			{
				ISite site = this.site;
				return site != null && site.DesignMode;
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001D1B0 File Offset: 0x0001C1B0
		public override string ToString()
		{
			ISite site = this.site;
			if (site != null)
			{
				return site.Name + " [" + base.GetType().FullName + "]";
			}
			return base.GetType().FullName;
		}

		// Token: 0x040009BF RID: 2495
		private static readonly object EventDisposed = new object();

		// Token: 0x040009C0 RID: 2496
		private ISite site;

		// Token: 0x040009C1 RID: 2497
		private EventHandlerList events;
	}
}
