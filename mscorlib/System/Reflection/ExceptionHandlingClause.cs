using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000336 RID: 822
	[ComVisible(true)]
	public sealed class ExceptionHandlingClause
	{
		// Token: 0x06001F9B RID: 8091 RVA: 0x0004F70E File Offset: 0x0004E70E
		private ExceptionHandlingClause()
		{
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x0004F716 File Offset: 0x0004E716
		public ExceptionHandlingClauseOptions Flags
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x0004F71E File Offset: 0x0004E71E
		public int TryOffset
		{
			get
			{
				return this.m_tryOffset;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x0004F726 File Offset: 0x0004E726
		public int TryLength
		{
			get
			{
				return this.m_tryLength;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x0004F72E File Offset: 0x0004E72E
		public int HandlerOffset
		{
			get
			{
				return this.m_handlerOffset;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x0004F736 File Offset: 0x0004E736
		public int HandlerLength
		{
			get
			{
				return this.m_handlerLength;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x0004F73E File Offset: 0x0004E73E
		public int FilterOffset
		{
			get
			{
				if (this.m_flags != ExceptionHandlingClauseOptions.Filter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_EHClauseNotFilter"));
				}
				return this.m_filterOffset;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x0004F760 File Offset: 0x0004E760
		public Type CatchType
		{
			get
			{
				if (this.m_flags != ExceptionHandlingClauseOptions.Clause)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_EHClauseNotClause"));
				}
				Type result = null;
				if (!MetadataToken.IsNullToken(this.m_catchMetadataToken))
				{
					Type declaringType = this.m_methodBody.m_methodBase.DeclaringType;
					Module module = (declaringType == null) ? this.m_methodBody.m_methodBase.Module : declaringType.Module;
					result = module.ResolveType(this.m_catchMetadataToken, (declaringType == null) ? null : declaringType.GetGenericArguments(), (this.m_methodBody.m_methodBase is MethodInfo) ? this.m_methodBody.m_methodBase.GetGenericArguments() : null);
				}
				return result;
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0004F800 File Offset: 0x0004E800
		public override string ToString()
		{
			if (this.Flags == ExceptionHandlingClauseOptions.Clause)
			{
				return string.Format(CultureInfo.CurrentUICulture, "Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}, CatchType={5}", new object[]
				{
					this.Flags,
					this.TryOffset,
					this.TryLength,
					this.HandlerOffset,
					this.HandlerLength,
					this.CatchType
				});
			}
			if (this.Flags == ExceptionHandlingClauseOptions.Filter)
			{
				return string.Format(CultureInfo.CurrentUICulture, "Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}, FilterOffset={5}", new object[]
				{
					this.Flags,
					this.TryOffset,
					this.TryLength,
					this.HandlerOffset,
					this.HandlerLength,
					this.FilterOffset
				});
			}
			return string.Format(CultureInfo.CurrentUICulture, "Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}", new object[]
			{
				this.Flags,
				this.TryOffset,
				this.TryLength,
				this.HandlerOffset,
				this.HandlerLength
			});
		}

		// Token: 0x04000D8F RID: 3471
		private MethodBody m_methodBody;

		// Token: 0x04000D90 RID: 3472
		private ExceptionHandlingClauseOptions m_flags;

		// Token: 0x04000D91 RID: 3473
		private int m_tryOffset;

		// Token: 0x04000D92 RID: 3474
		private int m_tryLength;

		// Token: 0x04000D93 RID: 3475
		private int m_handlerOffset;

		// Token: 0x04000D94 RID: 3476
		private int m_handlerLength;

		// Token: 0x04000D95 RID: 3477
		private int m_catchMetadataToken;

		// Token: 0x04000D96 RID: 3478
		private int m_filterOffset;
	}
}
