using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200069C RID: 1692
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class ContextAttribute : Attribute, IContextAttribute, IContextProperty
	{
		// Token: 0x06003D3B RID: 15675 RVA: 0x000D1BD1 File Offset: 0x000D0BD1
		public ContextAttribute(string name)
		{
			this.AttributeName = name;
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06003D3C RID: 15676 RVA: 0x000D1BE0 File Offset: 0x000D0BE0
		public virtual string Name
		{
			get
			{
				return this.AttributeName;
			}
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x000D1BE8 File Offset: 0x000D0BE8
		public virtual bool IsNewContextOK(Context newCtx)
		{
			return true;
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x000D1BEB File Offset: 0x000D0BEB
		public virtual void Freeze(Context newContext)
		{
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x000D1BF0 File Offset: 0x000D0BF0
		public override bool Equals(object o)
		{
			IContextProperty contextProperty = o as IContextProperty;
			return contextProperty != null && this.AttributeName.Equals(contextProperty.Name);
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x000D1C1A File Offset: 0x000D0C1A
		public override int GetHashCode()
		{
			return this.AttributeName.GetHashCode();
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x000D1C28 File Offset: 0x000D0C28
		public virtual bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			if (!ctorMsg.ActivationType.IsContextful)
			{
				return true;
			}
			object property = ctx.GetProperty(this.AttributeName);
			return property != null && this.Equals(property);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x000D1C7C File Offset: 0x000D0C7C
		public virtual void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			ctorMsg.ContextProperties.Add(this);
		}

		// Token: 0x04001F67 RID: 8039
		protected string AttributeName;
	}
}
