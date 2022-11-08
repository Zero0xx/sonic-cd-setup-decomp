using System;
using System.ComponentModel.Design;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x020000D2 RID: 210
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public sealed class DesignerAttribute : Attribute
	{
		// Token: 0x06000721 RID: 1825 RVA: 0x0001A690 File Offset: 0x00019690
		public DesignerAttribute(string designerTypeName)
		{
			designerTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.designerTypeName = designerTypeName;
			this.designerBaseTypeName = typeof(IDesigner).FullName;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001A6C0 File Offset: 0x000196C0
		public DesignerAttribute(Type designerType)
		{
			this.designerTypeName = designerType.AssemblyQualifiedName;
			this.designerBaseTypeName = typeof(IDesigner).FullName;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001A6E9 File Offset: 0x000196E9
		public DesignerAttribute(string designerTypeName, string designerBaseTypeName)
		{
			designerTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.designerTypeName = designerTypeName;
			this.designerBaseTypeName = designerBaseTypeName;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001A70B File Offset: 0x0001970B
		public DesignerAttribute(string designerTypeName, Type designerBaseType)
		{
			designerTypeName.ToUpper(CultureInfo.InvariantCulture);
			this.designerTypeName = designerTypeName;
			this.designerBaseTypeName = designerBaseType.AssemblyQualifiedName;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001A732 File Offset: 0x00019732
		public DesignerAttribute(Type designerType, Type designerBaseType)
		{
			this.designerTypeName = designerType.AssemblyQualifiedName;
			this.designerBaseTypeName = designerBaseType.AssemblyQualifiedName;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001A752 File Offset: 0x00019752
		public string DesignerBaseTypeName
		{
			get
			{
				return this.designerBaseTypeName;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0001A75A File Offset: 0x0001975A
		public string DesignerTypeName
		{
			get
			{
				return this.designerTypeName;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001A764 File Offset: 0x00019764
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					string text = this.designerBaseTypeName;
					int num = text.IndexOf(',');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					this.typeId = base.GetType().FullName + text;
				}
				return this.typeId;
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001A7B4 File Offset: 0x000197B4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignerAttribute designerAttribute = obj as DesignerAttribute;
			return designerAttribute != null && designerAttribute.designerBaseTypeName == this.designerBaseTypeName && designerAttribute.designerTypeName == this.designerTypeName;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001A7F7 File Offset: 0x000197F7
		public override int GetHashCode()
		{
			return this.designerTypeName.GetHashCode() ^ this.designerBaseTypeName.GetHashCode();
		}

		// Token: 0x04000941 RID: 2369
		private readonly string designerTypeName;

		// Token: 0x04000942 RID: 2370
		private readonly string designerBaseTypeName;

		// Token: 0x04000943 RID: 2371
		private string typeId;
	}
}
