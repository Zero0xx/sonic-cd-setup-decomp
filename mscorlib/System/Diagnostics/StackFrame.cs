using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x020002C8 RID: 712
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[Serializable]
	public class StackFrame
	{
		// Token: 0x06001B90 RID: 7056 RVA: 0x00047FD7 File Offset: 0x00046FD7
		internal void InitMembers()
		{
			this.method = null;
			this.offset = -1;
			this.ILOffset = -1;
			this.strFileName = null;
			this.iLineNumber = 0;
			this.iColumnNumber = 0;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x00048003 File Offset: 0x00047003
		public StackFrame()
		{
			this.InitMembers();
			this.BuildStackFrame(0, false);
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x00048019 File Offset: 0x00047019
		public StackFrame(bool fNeedFileInfo)
		{
			this.InitMembers();
			this.BuildStackFrame(0, fNeedFileInfo);
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0004802F File Offset: 0x0004702F
		public StackFrame(int skipFrames)
		{
			this.InitMembers();
			this.BuildStackFrame(skipFrames, false);
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00048045 File Offset: 0x00047045
		public StackFrame(int skipFrames, bool fNeedFileInfo)
		{
			this.InitMembers();
			this.BuildStackFrame(skipFrames, fNeedFileInfo);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0004805B File Offset: 0x0004705B
		internal StackFrame(bool DummyFlag1, bool DummyFlag2)
		{
			this.InitMembers();
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x00048069 File Offset: 0x00047069
		public StackFrame(string fileName, int lineNumber)
		{
			this.InitMembers();
			this.BuildStackFrame(0, false);
			this.strFileName = fileName;
			this.iLineNumber = lineNumber;
			this.iColumnNumber = 0;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x00048094 File Offset: 0x00047094
		public StackFrame(string fileName, int lineNumber, int colNumber)
		{
			this.InitMembers();
			this.BuildStackFrame(0, false);
			this.strFileName = fileName;
			this.iLineNumber = lineNumber;
			this.iColumnNumber = colNumber;
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000480BF File Offset: 0x000470BF
		internal virtual void SetMethodBase(MethodBase mb)
		{
			this.method = mb;
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000480C8 File Offset: 0x000470C8
		internal virtual void SetOffset(int iOffset)
		{
			this.offset = iOffset;
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000480D1 File Offset: 0x000470D1
		internal virtual void SetILOffset(int iOffset)
		{
			this.ILOffset = iOffset;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000480DA File Offset: 0x000470DA
		internal virtual void SetFileName(string strFName)
		{
			this.strFileName = strFName;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000480E3 File Offset: 0x000470E3
		internal virtual void SetLineNumber(int iLine)
		{
			this.iLineNumber = iLine;
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000480EC File Offset: 0x000470EC
		internal virtual void SetColumnNumber(int iCol)
		{
			this.iColumnNumber = iCol;
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000480F5 File Offset: 0x000470F5
		public virtual MethodBase GetMethod()
		{
			return this.method;
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000480FD File Offset: 0x000470FD
		public virtual int GetNativeOffset()
		{
			return this.offset;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00048105 File Offset: 0x00047105
		public virtual int GetILOffset()
		{
			return this.ILOffset;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00048110 File Offset: 0x00047110
		public virtual string GetFileName()
		{
			if (this.strFileName != null)
			{
				new FileIOPermission(PermissionState.None)
				{
					AllFiles = FileIOPermissionAccess.PathDiscovery
				}.Demand();
			}
			return this.strFileName;
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0004813F File Offset: 0x0004713F
		public virtual int GetFileLineNumber()
		{
			return this.iLineNumber;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x00048147 File Offset: 0x00047147
		public virtual int GetFileColumnNumber()
		{
			return this.iColumnNumber;
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00048150 File Offset: 0x00047150
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			if (this.method != null)
			{
				stringBuilder.Append(this.method.Name);
				if (this.method is MethodInfo && ((MethodInfo)this.method).IsGenericMethod)
				{
					Type[] genericArguments = ((MethodInfo)this.method).GetGenericArguments();
					stringBuilder.Append("<");
					int i = 0;
					bool flag = true;
					while (i < genericArguments.Length)
					{
						if (!flag)
						{
							stringBuilder.Append(",");
						}
						else
						{
							flag = false;
						}
						stringBuilder.Append(genericArguments[i].Name);
						i++;
					}
					stringBuilder.Append(">");
				}
				stringBuilder.Append(" at offset ");
				if (this.offset == -1)
				{
					stringBuilder.Append("<offset unknown>");
				}
				else
				{
					stringBuilder.Append(this.offset);
				}
				stringBuilder.Append(" in file:line:column ");
				bool flag2 = this.strFileName != null;
				if (flag2)
				{
					try
					{
						new FileIOPermission(PermissionState.None)
						{
							AllFiles = FileIOPermissionAccess.PathDiscovery
						}.Demand();
					}
					catch (SecurityException)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					stringBuilder.Append("<filename unknown>");
				}
				else
				{
					stringBuilder.Append(this.strFileName);
				}
				stringBuilder.Append(":");
				stringBuilder.Append(this.iLineNumber);
				stringBuilder.Append(":");
				stringBuilder.Append(this.iColumnNumber);
			}
			else
			{
				stringBuilder.Append("<null>");
			}
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x000482F0 File Offset: 0x000472F0
		private void BuildStackFrame(int skipFrames, bool fNeedFileInfo)
		{
			StackFrameHelper stackFrameHelper = new StackFrameHelper(fNeedFileInfo, null);
			StackTrace.GetStackFramesInternal(stackFrameHelper, 0, null);
			int numberOfFrames = stackFrameHelper.GetNumberOfFrames();
			skipFrames += StackTrace.CalculateFramesToSkip(stackFrameHelper, numberOfFrames);
			if (numberOfFrames - skipFrames > 0)
			{
				this.method = stackFrameHelper.GetMethodBase(skipFrames);
				this.offset = stackFrameHelper.GetOffset(skipFrames);
				this.ILOffset = stackFrameHelper.GetILOffset(skipFrames);
				if (fNeedFileInfo)
				{
					this.strFileName = stackFrameHelper.GetFilename(skipFrames);
					this.iLineNumber = stackFrameHelper.GetLineNumber(skipFrames);
					this.iColumnNumber = stackFrameHelper.GetColumnNumber(skipFrames);
				}
			}
		}

		// Token: 0x04000A9F RID: 2719
		public const int OFFSET_UNKNOWN = -1;

		// Token: 0x04000AA0 RID: 2720
		private MethodBase method;

		// Token: 0x04000AA1 RID: 2721
		private int offset;

		// Token: 0x04000AA2 RID: 2722
		private int ILOffset;

		// Token: 0x04000AA3 RID: 2723
		private string strFileName;

		// Token: 0x04000AA4 RID: 2724
		private int iLineNumber;

		// Token: 0x04000AA5 RID: 2725
		private int iColumnNumber;
	}
}
