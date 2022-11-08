using System;
using System.Collections;

namespace System.Security.Policy
{
	// Token: 0x020004B2 RID: 1202
	internal sealed class CodeGroupStack
	{
		// Token: 0x06002FD0 RID: 12240 RVA: 0x000A3E7D File Offset: 0x000A2E7D
		internal CodeGroupStack()
		{
			this.m_array = new ArrayList();
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000A3E90 File Offset: 0x000A2E90
		internal void Push(CodeGroupStackFrame element)
		{
			this.m_array.Add(element);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000A3EA0 File Offset: 0x000A2EA0
		internal CodeGroupStackFrame Pop()
		{
			if (this.IsEmpty())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyStack"));
			}
			int count = this.m_array.Count;
			CodeGroupStackFrame result = (CodeGroupStackFrame)this.m_array[count - 1];
			this.m_array.RemoveAt(count - 1);
			return result;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000A3EF4 File Offset: 0x000A2EF4
		internal bool IsEmpty()
		{
			return this.m_array.Count == 0;
		}

		// Token: 0x0400184F RID: 6223
		private ArrayList m_array;
	}
}
