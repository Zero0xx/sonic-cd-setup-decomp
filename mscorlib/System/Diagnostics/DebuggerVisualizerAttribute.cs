using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002BF RID: 703
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public sealed class DebuggerVisualizerAttribute : Attribute
	{
		// Token: 0x06001B36 RID: 6966 RVA: 0x00047039 File Offset: 0x00046039
		public DebuggerVisualizerAttribute(string visualizerTypeName)
		{
			this.visualizerName = visualizerTypeName;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00047048 File Offset: 0x00046048
		public DebuggerVisualizerAttribute(string visualizerTypeName, string visualizerObjectSourceTypeName)
		{
			this.visualizerName = visualizerTypeName;
			this.visualizerObjectSourceName = visualizerObjectSourceTypeName;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x0004705E File Offset: 0x0004605E
		public DebuggerVisualizerAttribute(string visualizerTypeName, Type visualizerObjectSource)
		{
			if (visualizerObjectSource == null)
			{
				throw new ArgumentNullException("visualizerObjectSource");
			}
			this.visualizerName = visualizerTypeName;
			this.visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00047087 File Offset: 0x00046087
		public DebuggerVisualizerAttribute(Type visualizer)
		{
			if (visualizer == null)
			{
				throw new ArgumentNullException("visualizer");
			}
			this.visualizerName = visualizer.AssemblyQualifiedName;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x000470A9 File Offset: 0x000460A9
		public DebuggerVisualizerAttribute(Type visualizer, Type visualizerObjectSource)
		{
			if (visualizer == null)
			{
				throw new ArgumentNullException("visualizer");
			}
			if (visualizerObjectSource == null)
			{
				throw new ArgumentNullException("visualizerObjectSource");
			}
			this.visualizerName = visualizer.AssemblyQualifiedName;
			this.visualizerObjectSourceName = visualizerObjectSource.AssemblyQualifiedName;
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000470E5 File Offset: 0x000460E5
		public DebuggerVisualizerAttribute(Type visualizer, string visualizerObjectSourceTypeName)
		{
			if (visualizer == null)
			{
				throw new ArgumentNullException("visualizer");
			}
			this.visualizerName = visualizer.AssemblyQualifiedName;
			this.visualizerObjectSourceName = visualizerObjectSourceTypeName;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001B3C RID: 6972 RVA: 0x0004710E File Offset: 0x0004610E
		public string VisualizerObjectSourceTypeName
		{
			get
			{
				return this.visualizerObjectSourceName;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x00047116 File Offset: 0x00046116
		public string VisualizerTypeName
		{
			get
			{
				return this.visualizerName;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x0004711E File Offset: 0x0004611E
		// (set) Token: 0x06001B3F RID: 6975 RVA: 0x00047126 File Offset: 0x00046126
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x00047152 File Offset: 0x00046152
		// (set) Token: 0x06001B40 RID: 6976 RVA: 0x0004712F File Offset: 0x0004612F
		public Type Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.targetName = value.AssemblyQualifiedName;
				this.target = value;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x00047163 File Offset: 0x00046163
		// (set) Token: 0x06001B42 RID: 6978 RVA: 0x0004715A File Offset: 0x0004615A
		public string TargetTypeName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x04000A67 RID: 2663
		private string visualizerObjectSourceName;

		// Token: 0x04000A68 RID: 2664
		private string visualizerName;

		// Token: 0x04000A69 RID: 2665
		private string description;

		// Token: 0x04000A6A RID: 2666
		private string targetName;

		// Token: 0x04000A6B RID: 2667
		private Type target;
	}
}
