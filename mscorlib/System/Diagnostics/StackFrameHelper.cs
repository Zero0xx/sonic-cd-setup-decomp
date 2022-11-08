using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020002C5 RID: 709
	[Serializable]
	internal class StackFrameHelper
	{
		// Token: 0x06001B71 RID: 7025 RVA: 0x0004776C File Offset: 0x0004676C
		public StackFrameHelper(bool fNeedFileLineColInfo, Thread target)
		{
			this.targetThread = target;
			this.rgMethodBase = null;
			this.rgMethodHandle = null;
			this.rgiOffset = null;
			this.rgiILOffset = null;
			this.rgFilename = null;
			this.rgiLineNumber = null;
			this.rgiColumnNumber = null;
			this.dynamicMethods = null;
			this.iFrameCount = 512;
			this.fNeedFileInfo = fNeedFileLineColInfo;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000477D0 File Offset: 0x000467D0
		public virtual MethodBase GetMethodBase(int i)
		{
			RuntimeMethodHandle methodHandle = this.rgMethodHandle[i];
			if (methodHandle.IsNullHandle())
			{
				return null;
			}
			methodHandle = methodHandle.GetTypicalMethodDefinition();
			return RuntimeType.GetMethodBase(methodHandle);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x00047808 File Offset: 0x00046808
		public virtual int GetOffset(int i)
		{
			return this.rgiOffset[i];
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x00047812 File Offset: 0x00046812
		public virtual int GetILOffset(int i)
		{
			return this.rgiILOffset[i];
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0004781C File Offset: 0x0004681C
		public virtual string GetFilename(int i)
		{
			return this.rgFilename[i];
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x00047826 File Offset: 0x00046826
		public virtual int GetLineNumber(int i)
		{
			return this.rgiLineNumber[i];
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x00047830 File Offset: 0x00046830
		public virtual int GetColumnNumber(int i)
		{
			return this.rgiColumnNumber[i];
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0004783A File Offset: 0x0004683A
		public virtual int GetNumberOfFrames()
		{
			return this.iFrameCount;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x00047842 File Offset: 0x00046842
		public virtual void SetNumberOfFrames(int i)
		{
			this.iFrameCount = i;
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0004784C File Offset: 0x0004684C
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			this.rgMethodBase = ((this.rgMethodHandle == null) ? null : new MethodBase[this.rgMethodHandle.Length]);
			if (this.rgMethodHandle != null)
			{
				for (int i = 0; i < this.rgMethodHandle.Length; i++)
				{
					if (!this.rgMethodHandle[i].IsNullHandle())
					{
						this.rgMethodBase[i] = RuntimeType.GetMethodBase(this.rgMethodHandle[i]);
					}
				}
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000478C3 File Offset: 0x000468C3
		[OnSerialized]
		private void OnSerialized(StreamingContext context)
		{
			this.rgMethodBase = null;
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x000478CC File Offset: 0x000468CC
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			this.rgMethodHandle = ((this.rgMethodBase == null) ? null : new RuntimeMethodHandle[this.rgMethodBase.Length]);
			if (this.rgMethodBase != null)
			{
				for (int i = 0; i < this.rgMethodBase.Length; i++)
				{
					if (this.rgMethodBase[i] != null)
					{
						this.rgMethodHandle[i] = this.rgMethodBase[i].MethodHandle;
					}
				}
			}
			this.rgMethodBase = null;
		}

		// Token: 0x04000A8C RID: 2700
		[NonSerialized]
		private Thread targetThread;

		// Token: 0x04000A8D RID: 2701
		private int[] rgiOffset;

		// Token: 0x04000A8E RID: 2702
		private int[] rgiILOffset;

		// Token: 0x04000A8F RID: 2703
		private MethodBase[] rgMethodBase;

		// Token: 0x04000A90 RID: 2704
		private object dynamicMethods;

		// Token: 0x04000A91 RID: 2705
		[NonSerialized]
		private RuntimeMethodHandle[] rgMethodHandle;

		// Token: 0x04000A92 RID: 2706
		private string[] rgFilename;

		// Token: 0x04000A93 RID: 2707
		private int[] rgiLineNumber;

		// Token: 0x04000A94 RID: 2708
		private int[] rgiColumnNumber;

		// Token: 0x04000A95 RID: 2709
		private int iFrameCount;

		// Token: 0x04000A96 RID: 2710
		private bool fNeedFileInfo;
	}
}
