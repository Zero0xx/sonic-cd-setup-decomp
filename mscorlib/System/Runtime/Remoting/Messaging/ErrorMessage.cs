using System;
using System.Collections;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000724 RID: 1828
	internal class ErrorMessage : IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004187 RID: 16775 RVA: 0x000DF729 File Offset: 0x000DE729
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004188 RID: 16776 RVA: 0x000DF72C File Offset: 0x000DE72C
		public string Uri
		{
			get
			{
				return this.m_URI;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x000DF734 File Offset: 0x000DE734
		public string MethodName
		{
			get
			{
				return this.m_MethodName;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x000DF73C File Offset: 0x000DE73C
		public string TypeName
		{
			get
			{
				return this.m_TypeName;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x000DF744 File Offset: 0x000DE744
		public object MethodSignature
		{
			get
			{
				return this.m_MethodSignature;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x000DF74C File Offset: 0x000DE74C
		public MethodBase MethodBase
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x000DF74F File Offset: 0x000DE74F
		public int ArgCount
		{
			get
			{
				return this.m_ArgCount;
			}
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x000DF757 File Offset: 0x000DE757
		public string GetArgName(int index)
		{
			return this.m_ArgName;
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x000DF75F File Offset: 0x000DE75F
		public object GetArg(int argNum)
		{
			return null;
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004190 RID: 16784 RVA: 0x000DF762 File Offset: 0x000DE762
		public object[] Args
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06004191 RID: 16785 RVA: 0x000DF765 File Offset: 0x000DE765
		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x000DF768 File Offset: 0x000DE768
		public int InArgCount
		{
			get
			{
				return this.m_ArgCount;
			}
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x000DF770 File Offset: 0x000DE770
		public string GetInArgName(int index)
		{
			return null;
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x000DF773 File Offset: 0x000DE773
		public object GetInArg(int argNum)
		{
			return null;
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x000DF776 File Offset: 0x000DE776
		public object[] InArgs
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004196 RID: 16790 RVA: 0x000DF779 File Offset: 0x000DE779
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040020EF RID: 8431
		private string m_URI = "Exception";

		// Token: 0x040020F0 RID: 8432
		private string m_MethodName = "Unknown";

		// Token: 0x040020F1 RID: 8433
		private string m_TypeName = "Unknown";

		// Token: 0x040020F2 RID: 8434
		private object m_MethodSignature;

		// Token: 0x040020F3 RID: 8435
		private int m_ArgCount;

		// Token: 0x040020F4 RID: 8436
		private string m_ArgName = "Unknown";
	}
}
