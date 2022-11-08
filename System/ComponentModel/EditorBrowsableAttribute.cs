using System;

namespace System.ComponentModel
{
	// Token: 0x020000DD RID: 221
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
	public sealed class EditorBrowsableAttribute : Attribute
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x0001ACF7 File Offset: 0x00019CF7
		public EditorBrowsableAttribute(EditorBrowsableState state)
		{
			this.browsableState = state;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001AD06 File Offset: 0x00019D06
		public EditorBrowsableAttribute() : this(EditorBrowsableState.Always)
		{
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0001AD0F File Offset: 0x00019D0F
		public EditorBrowsableState State
		{
			get
			{
				return this.browsableState;
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001AD18 File Offset: 0x00019D18
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			EditorBrowsableAttribute editorBrowsableAttribute = obj as EditorBrowsableAttribute;
			return editorBrowsableAttribute != null && editorBrowsableAttribute.browsableState == this.browsableState;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001AD45 File Offset: 0x00019D45
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000962 RID: 2402
		private EditorBrowsableState browsableState;
	}
}
