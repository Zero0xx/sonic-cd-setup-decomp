using System;
using System.Collections;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x0200081C RID: 2076
	internal class DynamicScope
	{
		// Token: 0x060049C1 RID: 18881 RVA: 0x00100CAD File Offset: 0x000FFCAD
		internal DynamicScope()
		{
			this.m_tokens = new ArrayList();
			this.m_tokens.Add(null);
		}

		// Token: 0x17000CA7 RID: 3239
		internal object this[int token]
		{
			get
			{
				token &= 16777215;
				if (token < 0 || token > this.m_tokens.Count)
				{
					return null;
				}
				return this.m_tokens[token];
			}
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x00100CF8 File Offset: 0x000FFCF8
		internal int GetTokenFor(VarArgMethod varArgMethod)
		{
			return this.m_tokens.Add(varArgMethod) | 167772160;
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x00100D0C File Offset: 0x000FFD0C
		internal string GetString(int token)
		{
			return this[token] as string;
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x00100D1C File Offset: 0x000FFD1C
		internal byte[] ResolveSignature(int token, int fromMethod)
		{
			if (fromMethod == 0)
			{
				return (byte[])this[token];
			}
			VarArgMethod varArgMethod = this[token] as VarArgMethod;
			if (varArgMethod == null)
			{
				return null;
			}
			return varArgMethod.m_signature.GetSignature(true);
		}

		// Token: 0x060049C6 RID: 18886 RVA: 0x00100D58 File Offset: 0x000FFD58
		public int GetTokenFor(RuntimeMethodHandle method)
		{
			MethodBase methodBase = RuntimeType.GetMethodBase(method);
			if (methodBase.DeclaringType != null && methodBase.DeclaringType.IsGenericType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_MethodDeclaringTypeGenericLcg"), new object[]
				{
					methodBase,
					methodBase.DeclaringType.GetGenericTypeDefinition()
				}));
			}
			return this.m_tokens.Add(method) | 100663296;
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x00100DCC File Offset: 0x000FFDCC
		public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle typeContext)
		{
			return this.m_tokens.Add(new GenericMethodInfo(method, typeContext)) | 100663296;
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x00100DE6 File Offset: 0x000FFDE6
		public int GetTokenFor(DynamicMethod method)
		{
			return this.m_tokens.Add(method) | 100663296;
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x00100DFA File Offset: 0x000FFDFA
		public int GetTokenFor(RuntimeFieldHandle field)
		{
			return this.m_tokens.Add(field) | 67108864;
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x00100E13 File Offset: 0x000FFE13
		public int GetTokenFor(RuntimeFieldHandle field, RuntimeTypeHandle typeContext)
		{
			return this.m_tokens.Add(new GenericFieldInfo(field, typeContext)) | 67108864;
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x00100E2D File Offset: 0x000FFE2D
		public int GetTokenFor(RuntimeTypeHandle type)
		{
			return this.m_tokens.Add(type) | 33554432;
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x00100E46 File Offset: 0x000FFE46
		public int GetTokenFor(string literal)
		{
			return this.m_tokens.Add(literal) | 1879048192;
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x00100E5A File Offset: 0x000FFE5A
		public int GetTokenFor(byte[] signature)
		{
			return this.m_tokens.Add(signature) | 285212672;
		}

		// Token: 0x040025C4 RID: 9668
		internal ArrayList m_tokens;
	}
}
