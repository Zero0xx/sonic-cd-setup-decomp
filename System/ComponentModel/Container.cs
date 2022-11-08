using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000BC RID: 188
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Container : IContainer, IDisposable
	{
		// Token: 0x06000682 RID: 1666 RVA: 0x00018E8C File Offset: 0x00017E8C
		~Container()
		{
			this.Dispose(false);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00018EBC File Offset: 0x00017EBC
		public virtual void Add(IComponent component)
		{
			this.Add(component, null);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00018EC8 File Offset: 0x00017EC8
		public virtual void Add(IComponent component, string name)
		{
			lock (this.syncObj)
			{
				if (component != null)
				{
					ISite site = component.Site;
					if (site == null || site.Container != this)
					{
						if (this.sites == null)
						{
							this.sites = new ISite[4];
						}
						else
						{
							this.ValidateName(component, name);
							if (this.sites.Length == this.siteCount)
							{
								ISite[] destinationArray = new ISite[this.siteCount * 2];
								Array.Copy(this.sites, 0, destinationArray, 0, this.siteCount);
								this.sites = destinationArray;
							}
						}
						if (site != null)
						{
							site.Container.Remove(component);
						}
						ISite site2 = this.CreateSite(component, name);
						this.sites[this.siteCount++] = site2;
						component.Site = site2;
						this.components = null;
					}
				}
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00018FB4 File Offset: 0x00017FB4
		protected virtual ISite CreateSite(IComponent component, string name)
		{
			return new Container.Site(component, this, name);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00018FBE File Offset: 0x00017FBE
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00018FD0 File Offset: 0x00017FD0
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this.syncObj)
				{
					while (this.siteCount > 0)
					{
						ISite site = this.sites[--this.siteCount];
						site.Component.Site = null;
						site.Component.Dispose();
					}
					this.sites = null;
					this.components = null;
				}
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00019050 File Offset: 0x00018050
		protected virtual object GetService(Type service)
		{
			if (service != typeof(IContainer))
			{
				return null;
			}
			return this;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00019064 File Offset: 0x00018064
		public virtual ComponentCollection Components
		{
			get
			{
				ComponentCollection result;
				lock (this.syncObj)
				{
					if (this.components == null)
					{
						IComponent[] array = new IComponent[this.siteCount];
						for (int i = 0; i < this.siteCount; i++)
						{
							array[i] = this.sites[i].Component;
						}
						this.components = new ComponentCollection(array);
						if (this.filter == null && this.checkedFilter)
						{
							this.checkedFilter = false;
						}
					}
					if (!this.checkedFilter)
					{
						this.filter = (this.GetService(typeof(ContainerFilterService)) as ContainerFilterService);
						this.checkedFilter = true;
					}
					if (this.filter != null)
					{
						ComponentCollection componentCollection = this.filter.FilterComponents(this.components);
						if (componentCollection != null)
						{
							this.components = componentCollection;
						}
					}
					result = this.components;
				}
				return result;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00019148 File Offset: 0x00018148
		public virtual void Remove(IComponent component)
		{
			this.Remove(component, false);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00019154 File Offset: 0x00018154
		private void Remove(IComponent component, bool preserveSite)
		{
			lock (this.syncObj)
			{
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null && site.Container == this)
					{
						if (!preserveSite)
						{
							component.Site = null;
						}
						for (int i = 0; i < this.siteCount; i++)
						{
							if (this.sites[i] == site)
							{
								this.siteCount--;
								Array.Copy(this.sites, i + 1, this.sites, i, this.siteCount - i);
								this.sites[this.siteCount] = null;
								this.components = null;
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001920C File Offset: 0x0001820C
		protected void RemoveWithoutUnsiting(IComponent component)
		{
			this.Remove(component, true);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00019218 File Offset: 0x00018218
		protected virtual void ValidateName(IComponent component, string name)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (name != null)
			{
				for (int i = 0; i < Math.Min(this.siteCount, this.sites.Length); i++)
				{
					ISite site = this.sites[i];
					if (site != null && site.Name != null && string.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase) && site.Component != component)
					{
						InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(site.Component)[typeof(InheritanceAttribute)];
						if (inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
						{
							throw new ArgumentException(SR.GetString("DuplicateComponentName", new object[]
							{
								name
							}));
						}
					}
				}
			}
		}

		// Token: 0x0400091C RID: 2332
		private ISite[] sites;

		// Token: 0x0400091D RID: 2333
		private int siteCount;

		// Token: 0x0400091E RID: 2334
		private ComponentCollection components;

		// Token: 0x0400091F RID: 2335
		private ContainerFilterService filter;

		// Token: 0x04000920 RID: 2336
		private bool checkedFilter;

		// Token: 0x04000921 RID: 2337
		private object syncObj = new object();

		// Token: 0x020000BE RID: 190
		private class Site : ISite, IServiceProvider
		{
			// Token: 0x06000694 RID: 1684 RVA: 0x000192DE File Offset: 0x000182DE
			internal Site(IComponent component, Container container, string name)
			{
				this.component = component;
				this.container = container;
				this.name = name;
			}

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x06000695 RID: 1685 RVA: 0x000192FB File Offset: 0x000182FB
			public IComponent Component
			{
				get
				{
					return this.component;
				}
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000696 RID: 1686 RVA: 0x00019303 File Offset: 0x00018303
			public IContainer Container
			{
				get
				{
					return this.container;
				}
			}

			// Token: 0x06000697 RID: 1687 RVA: 0x0001930B File Offset: 0x0001830B
			public object GetService(Type service)
			{
				if (service != typeof(ISite))
				{
					return this.container.GetService(service);
				}
				return this;
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x06000698 RID: 1688 RVA: 0x00019328 File Offset: 0x00018328
			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001932B File Offset: 0x0001832B
			// (set) Token: 0x0600069A RID: 1690 RVA: 0x00019333 File Offset: 0x00018333
			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					if (value == null || this.name == null || !value.Equals(this.name))
					{
						this.container.ValidateName(this.component, value);
						this.name = value;
					}
				}
			}

			// Token: 0x04000922 RID: 2338
			private IComponent component;

			// Token: 0x04000923 RID: 2339
			private Container container;

			// Token: 0x04000924 RID: 2340
			private string name;
		}
	}
}
