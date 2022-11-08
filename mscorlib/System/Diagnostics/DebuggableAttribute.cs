using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002B9 RID: 697
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
	public sealed class DebuggableAttribute : Attribute
	{
		// Token: 0x06001B1E RID: 6942 RVA: 0x00046E8F File Offset: 0x00045E8F
		public DebuggableAttribute(bool isJITTrackingEnabled, bool isJITOptimizerDisabled)
		{
			this.m_debuggingModes = DebuggableAttribute.DebuggingModes.None;
			if (isJITTrackingEnabled)
			{
				this.m_debuggingModes |= DebuggableAttribute.DebuggingModes.Default;
			}
			if (isJITOptimizerDisabled)
			{
				this.m_debuggingModes |= DebuggableAttribute.DebuggingModes.DisableOptimizations;
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x00046EC4 File Offset: 0x00045EC4
		public DebuggableAttribute(DebuggableAttribute.DebuggingModes modes)
		{
			this.m_debuggingModes = modes;
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x00046ED3 File Offset: 0x00045ED3
		public bool IsJITTrackingEnabled
		{
			get
			{
				return (this.m_debuggingModes & DebuggableAttribute.DebuggingModes.Default) != DebuggableAttribute.DebuggingModes.None;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x00046EE3 File Offset: 0x00045EE3
		public bool IsJITOptimizerDisabled
		{
			get
			{
				return (this.m_debuggingModes & DebuggableAttribute.DebuggingModes.DisableOptimizations) != DebuggableAttribute.DebuggingModes.None;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x00046EF7 File Offset: 0x00045EF7
		public DebuggableAttribute.DebuggingModes DebuggingFlags
		{
			get
			{
				return this.m_debuggingModes;
			}
		}

		// Token: 0x04000A53 RID: 2643
		private DebuggableAttribute.DebuggingModes m_debuggingModes;

		// Token: 0x020002BA RID: 698
		[Flags]
		[ComVisible(true)]
		public enum DebuggingModes
		{
			// Token: 0x04000A55 RID: 2645
			None = 0,
			// Token: 0x04000A56 RID: 2646
			Default = 1,
			// Token: 0x04000A57 RID: 2647
			DisableOptimizations = 256,
			// Token: 0x04000A58 RID: 2648
			IgnoreSymbolStoreSequencePoints = 2,
			// Token: 0x04000A59 RID: 2649
			EnableEditAndContinue = 4
		}
	}
}
