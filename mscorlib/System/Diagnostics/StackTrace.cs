using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020002C6 RID: 710
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[Serializable]
	public class StackTrace
	{
		// Token: 0x06001B7D RID: 7037 RVA: 0x00047941 File Offset: 0x00046941
		public StackTrace()
		{
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, false, null, null);
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00047961 File Offset: 0x00046961
		public StackTrace(bool fNeedFileInfo)
		{
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, fNeedFileInfo, null, null);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x00047981 File Offset: 0x00046981
		public StackTrace(int skipFrames)
		{
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, false, null, null);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000479BA File Offset: 0x000469BA
		public StackTrace(int skipFrames, bool fNeedFileInfo)
		{
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, fNeedFileInfo, null, null);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x000479F3 File Offset: 0x000469F3
		public StackTrace(Exception e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, false, null, e);
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x00047A21 File Offset: 0x00046A21
		public StackTrace(Exception e, bool fNeedFileInfo)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, fNeedFileInfo, null, e);
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x00047A50 File Offset: 0x00046A50
		public StackTrace(Exception e, int skipFrames)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, false, null, e);
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x00047AA4 File Offset: 0x00046AA4
		public StackTrace(Exception e, int skipFrames, bool fNeedFileInfo)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (skipFrames < 0)
			{
				throw new ArgumentOutOfRangeException("skipFrames", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(skipFrames, fNeedFileInfo, null, e);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x00047AF6 File Offset: 0x00046AF6
		public StackTrace(StackFrame frame)
		{
			this.frames = new StackFrame[1];
			this.frames[0] = frame;
			this.m_iMethodsToSkip = 0;
			this.m_iNumOfFrames = 1;
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x00047B21 File Offset: 0x00046B21
		public StackTrace(Thread targetThread, bool needFileInfo)
		{
			this.m_iNumOfFrames = 0;
			this.m_iMethodsToSkip = 0;
			this.CaptureStackTrace(0, needFileInfo, targetThread, null);
		}

		// Token: 0x06001B87 RID: 7047
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetStackFramesInternal(StackFrameHelper sfh, int iSkip, Exception e);

		// Token: 0x06001B88 RID: 7048 RVA: 0x00047B44 File Offset: 0x00046B44
		internal static int CalculateFramesToSkip(StackFrameHelper StackF, int iNumFrames)
		{
			int num = 0;
			string strB = "System.Diagnostics";
			for (int i = 0; i < iNumFrames; i++)
			{
				MethodBase methodBase = StackF.GetMethodBase(i);
				if (methodBase != null)
				{
					Type declaringType = methodBase.DeclaringType;
					if (declaringType == null)
					{
						break;
					}
					string @namespace = declaringType.Namespace;
					if (@namespace == null || string.Compare(@namespace, strB, StringComparison.Ordinal) != 0)
					{
						break;
					}
				}
				num++;
			}
			return num;
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x00047B9C File Offset: 0x00046B9C
		private void CaptureStackTrace(int iSkip, bool fNeedFileInfo, Thread targetThread, Exception e)
		{
			this.m_iMethodsToSkip += iSkip;
			StackFrameHelper stackFrameHelper = new StackFrameHelper(fNeedFileInfo, targetThread);
			StackTrace.GetStackFramesInternal(stackFrameHelper, 0, e);
			this.m_iNumOfFrames = stackFrameHelper.GetNumberOfFrames();
			if (this.m_iMethodsToSkip > this.m_iNumOfFrames)
			{
				this.m_iMethodsToSkip = this.m_iNumOfFrames;
			}
			if (this.m_iNumOfFrames != 0)
			{
				this.frames = new StackFrame[this.m_iNumOfFrames];
				for (int i = 0; i < this.m_iNumOfFrames; i++)
				{
					bool dummyFlag = true;
					bool dummyFlag2 = true;
					StackFrame stackFrame = new StackFrame(dummyFlag, dummyFlag2);
					stackFrame.SetMethodBase(stackFrameHelper.GetMethodBase(i));
					stackFrame.SetOffset(stackFrameHelper.GetOffset(i));
					stackFrame.SetILOffset(stackFrameHelper.GetILOffset(i));
					if (fNeedFileInfo)
					{
						stackFrame.SetFileName(stackFrameHelper.GetFilename(i));
						stackFrame.SetLineNumber(stackFrameHelper.GetLineNumber(i));
						stackFrame.SetColumnNumber(stackFrameHelper.GetColumnNumber(i));
					}
					this.frames[i] = stackFrame;
				}
				if (e == null)
				{
					this.m_iMethodsToSkip += StackTrace.CalculateFramesToSkip(stackFrameHelper, this.m_iNumOfFrames);
				}
				this.m_iNumOfFrames -= this.m_iMethodsToSkip;
				if (this.m_iNumOfFrames < 0)
				{
					this.m_iNumOfFrames = 0;
					return;
				}
			}
			else
			{
				this.frames = null;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x00047CD1 File Offset: 0x00046CD1
		public virtual int FrameCount
		{
			get
			{
				return this.m_iNumOfFrames;
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x00047CD9 File Offset: 0x00046CD9
		public virtual StackFrame GetFrame(int index)
		{
			if (this.frames != null && index < this.m_iNumOfFrames && index >= 0)
			{
				return this.frames[index + this.m_iMethodsToSkip];
			}
			return null;
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00047D04 File Offset: 0x00046D04
		[ComVisible(false)]
		public virtual StackFrame[] GetFrames()
		{
			if (this.frames == null || this.m_iNumOfFrames <= 0)
			{
				return null;
			}
			StackFrame[] array = new StackFrame[this.m_iNumOfFrames];
			Array.Copy(this.frames, this.m_iMethodsToSkip, array, 0, this.m_iNumOfFrames);
			return array;
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x00047D4A File Offset: 0x00046D4A
		public override string ToString()
		{
			return this.ToString(StackTrace.TraceFormat.TrailingNewLine);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x00047D54 File Offset: 0x00046D54
		internal string ToString(StackTrace.TraceFormat traceFormat)
		{
			string text = "at";
			string format = "in {0}:line {1}";
			if (traceFormat != StackTrace.TraceFormat.NoResourceLookup)
			{
				text = Environment.GetResourceString("Word_At");
				format = Environment.GetResourceString("StackTrace_InFileLineNumber");
			}
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder(255);
			for (int i = 0; i < this.m_iNumOfFrames; i++)
			{
				StackFrame frame = this.GetFrame(i);
				MethodBase method = frame.GetMethod();
				if (method != null)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(Environment.NewLine);
					}
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "   {0} ", new object[]
					{
						text
					});
					Type declaringType = method.DeclaringType;
					if (declaringType != null)
					{
						stringBuilder.Append(declaringType.FullName.Replace('+', '.'));
						stringBuilder.Append(".");
					}
					stringBuilder.Append(method.Name);
					if (method is MethodInfo && ((MethodInfo)method).IsGenericMethod)
					{
						Type[] genericArguments = ((MethodInfo)method).GetGenericArguments();
						stringBuilder.Append("[");
						int j = 0;
						bool flag2 = true;
						while (j < genericArguments.Length)
						{
							if (!flag2)
							{
								stringBuilder.Append(",");
							}
							else
							{
								flag2 = false;
							}
							stringBuilder.Append(genericArguments[j].Name);
							j++;
						}
						stringBuilder.Append("]");
					}
					stringBuilder.Append("(");
					ParameterInfo[] parameters = method.GetParameters();
					bool flag3 = true;
					for (int k = 0; k < parameters.Length; k++)
					{
						if (!flag3)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							flag3 = false;
						}
						string str = "<UnknownType>";
						if (parameters[k].ParameterType != null)
						{
							str = parameters[k].ParameterType.Name;
						}
						stringBuilder.Append(str + " " + parameters[k].Name);
					}
					stringBuilder.Append(")");
					if (frame.GetILOffset() != -1)
					{
						string text2 = null;
						try
						{
							text2 = frame.GetFileName();
						}
						catch (SecurityException)
						{
						}
						if (text2 != null)
						{
							stringBuilder.Append(' ');
							stringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, new object[]
							{
								text2,
								frame.GetFileLineNumber()
							});
						}
					}
				}
			}
			if (traceFormat == StackTrace.TraceFormat.TrailingNewLine)
			{
				stringBuilder.Append(Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x00047FBC File Offset: 0x00046FBC
		private static string GetManagedStackTraceStringHelper(bool fNeedFileInfo)
		{
			StackTrace stackTrace = new StackTrace(0, fNeedFileInfo);
			return stackTrace.ToString();
		}

		// Token: 0x04000A97 RID: 2711
		public const int METHODS_TO_SKIP = 0;

		// Token: 0x04000A98 RID: 2712
		private StackFrame[] frames;

		// Token: 0x04000A99 RID: 2713
		private int m_iNumOfFrames;

		// Token: 0x04000A9A RID: 2714
		private int m_iMethodsToSkip;

		// Token: 0x020002C7 RID: 711
		internal enum TraceFormat
		{
			// Token: 0x04000A9C RID: 2716
			Normal,
			// Token: 0x04000A9D RID: 2717
			TrailingNewLine,
			// Token: 0x04000A9E RID: 2718
			NoResourceLookup
		}
	}
}
