using System;
using System.Globalization;
using System.IO;

namespace System.Reflection.Emit
{
	// Token: 0x0200083B RID: 2107
	[Serializable]
	internal class ModuleBuilderData
	{
		// Token: 0x06004BCC RID: 19404 RVA: 0x0010830F File Offset: 0x0010730F
		internal ModuleBuilderData(ModuleBuilder module, string strModuleName, string strFileName)
		{
			this.Init(module, strModuleName, strFileName);
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x00108320 File Offset: 0x00107320
		internal virtual void Init(ModuleBuilder module, string strModuleName, string strFileName)
		{
			this.m_fGlobalBeenCreated = false;
			this.m_fHasGlobal = false;
			this.m_globalTypeBuilder = new TypeBuilder(module);
			this.m_module = module;
			this.m_strModuleName = strModuleName;
			this.m_tkFile = 0;
			this.m_isSaved = false;
			this.m_embeddedRes = null;
			this.m_strResourceFileName = null;
			this.m_resourceBytes = null;
			if (strFileName == null)
			{
				this.m_strFileName = strModuleName;
				this.m_isTransient = true;
			}
			else
			{
				string extension = Path.GetExtension(strFileName);
				if (extension == null || extension == string.Empty)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NoModuleFileExtension"), new object[]
					{
						strFileName
					}));
				}
				this.m_strFileName = strFileName;
				this.m_isTransient = false;
			}
			this.m_module.InternalSetModuleProps(this.m_strModuleName);
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x001083E7 File Offset: 0x001073E7
		internal virtual bool IsTransient()
		{
			return this.m_isTransient;
		}

		// Token: 0x04002689 RID: 9865
		internal const string MULTI_BYTE_VALUE_CLASS = "$ArrayType$";

		// Token: 0x0400268A RID: 9866
		internal string m_strModuleName;

		// Token: 0x0400268B RID: 9867
		internal string m_strFileName;

		// Token: 0x0400268C RID: 9868
		internal bool m_fGlobalBeenCreated;

		// Token: 0x0400268D RID: 9869
		internal bool m_fHasGlobal;

		// Token: 0x0400268E RID: 9870
		[NonSerialized]
		internal TypeBuilder m_globalTypeBuilder;

		// Token: 0x0400268F RID: 9871
		[NonSerialized]
		internal ModuleBuilder m_module;

		// Token: 0x04002690 RID: 9872
		internal int m_tkFile;

		// Token: 0x04002691 RID: 9873
		internal bool m_isSaved;

		// Token: 0x04002692 RID: 9874
		[NonSerialized]
		internal ResWriterData m_embeddedRes;

		// Token: 0x04002693 RID: 9875
		internal bool m_isTransient;

		// Token: 0x04002694 RID: 9876
		internal string m_strResourceFileName;

		// Token: 0x04002695 RID: 9877
		internal byte[] m_resourceBytes;
	}
}
