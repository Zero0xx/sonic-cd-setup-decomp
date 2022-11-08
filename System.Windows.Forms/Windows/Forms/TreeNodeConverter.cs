using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x020006FC RID: 1788
	public class TreeNodeConverter : TypeConverter
	{
		// Token: 0x06005F5A RID: 24410 RVA: 0x0015AD02 File Offset: 0x00159D02
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06005F5B RID: 24411 RVA: 0x0015AD1C File Offset: 0x00159D1C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is TreeNode)
			{
				TreeNode treeNode = (TreeNode)value;
				MemberInfo constructor;
				object[] arguments;
				if (treeNode.ImageIndex == -1 || treeNode.SelectedImageIndex == -1)
				{
					if (treeNode.Nodes.Count == 0)
					{
						constructor = typeof(TreeNode).GetConstructor(new Type[]
						{
							typeof(string)
						});
						arguments = new object[]
						{
							treeNode.Text
						};
					}
					else
					{
						constructor = typeof(TreeNode).GetConstructor(new Type[]
						{
							typeof(string),
							typeof(TreeNode[])
						});
						TreeNode[] array = new TreeNode[treeNode.Nodes.Count];
						treeNode.Nodes.CopyTo(array, 0);
						arguments = new object[]
						{
							treeNode.Text,
							array
						};
					}
				}
				else if (treeNode.Nodes.Count == 0)
				{
					constructor = typeof(TreeNode).GetConstructor(new Type[]
					{
						typeof(string),
						typeof(int),
						typeof(int)
					});
					arguments = new object[]
					{
						treeNode.Text,
						treeNode.ImageIndex,
						treeNode.SelectedImageIndex
					};
				}
				else
				{
					constructor = typeof(TreeNode).GetConstructor(new Type[]
					{
						typeof(string),
						typeof(int),
						typeof(int),
						typeof(TreeNode[])
					});
					TreeNode[] array2 = new TreeNode[treeNode.Nodes.Count];
					treeNode.Nodes.CopyTo(array2, 0);
					arguments = new object[]
					{
						treeNode.Text,
						treeNode.ImageIndex,
						treeNode.SelectedImageIndex,
						array2
					};
				}
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, arguments, false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
