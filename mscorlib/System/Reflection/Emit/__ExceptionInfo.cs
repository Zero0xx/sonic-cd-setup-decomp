using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000828 RID: 2088
	internal class __ExceptionInfo
	{
		// Token: 0x06004A52 RID: 19026 RVA: 0x00102214 File Offset: 0x00101214
		private __ExceptionInfo()
		{
			this.m_startAddr = 0;
			this.m_filterAddr = null;
			this.m_catchAddr = null;
			this.m_catchEndAddr = null;
			this.m_endAddr = 0;
			this.m_currentCatch = 0;
			this.m_type = null;
			this.m_endFinally = -1;
			this.m_currentState = 0;
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x00102268 File Offset: 0x00101268
		internal __ExceptionInfo(int startAddr, Label endLabel)
		{
			this.m_startAddr = startAddr;
			this.m_endAddr = -1;
			this.m_filterAddr = new int[4];
			this.m_catchAddr = new int[4];
			this.m_catchEndAddr = new int[4];
			this.m_catchClass = new Type[4];
			this.m_currentCatch = 0;
			this.m_endLabel = endLabel;
			this.m_type = new int[4];
			this.m_endFinally = -1;
			this.m_currentState = 0;
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x001022E4 File Offset: 0x001012E4
		private void MarkHelper(int catchorfilterAddr, int catchEndAddr, Type catchClass, int type)
		{
			if (this.m_currentCatch >= this.m_catchAddr.Length)
			{
				this.m_filterAddr = ILGenerator.EnlargeArray(this.m_filterAddr);
				this.m_catchAddr = ILGenerator.EnlargeArray(this.m_catchAddr);
				this.m_catchEndAddr = ILGenerator.EnlargeArray(this.m_catchEndAddr);
				this.m_catchClass = ILGenerator.EnlargeArray(this.m_catchClass);
				this.m_type = ILGenerator.EnlargeArray(this.m_type);
			}
			if (type == 1)
			{
				this.m_type[this.m_currentCatch] = type;
				this.m_filterAddr[this.m_currentCatch] = catchorfilterAddr;
				this.m_catchAddr[this.m_currentCatch] = -1;
				if (this.m_currentCatch > 0)
				{
					this.m_catchEndAddr[this.m_currentCatch - 1] = catchorfilterAddr;
				}
			}
			else
			{
				this.m_catchClass[this.m_currentCatch] = catchClass;
				if (this.m_type[this.m_currentCatch] != 1)
				{
					this.m_type[this.m_currentCatch] = type;
				}
				this.m_catchAddr[this.m_currentCatch] = catchorfilterAddr;
				if (this.m_currentCatch > 0 && this.m_type[this.m_currentCatch] != 1)
				{
					this.m_catchEndAddr[this.m_currentCatch - 1] = catchEndAddr;
				}
				this.m_catchEndAddr[this.m_currentCatch] = -1;
				this.m_currentCatch++;
			}
			if (this.m_endAddr == -1)
			{
				this.m_endAddr = catchorfilterAddr;
			}
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x00102437 File Offset: 0x00101437
		internal virtual void MarkFilterAddr(int filterAddr)
		{
			this.m_currentState = 1;
			this.MarkHelper(filterAddr, filterAddr, null, 1);
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x0010244A File Offset: 0x0010144A
		internal virtual void MarkFaultAddr(int faultAddr)
		{
			this.m_currentState = 4;
			this.MarkHelper(faultAddr, faultAddr, null, 4);
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0010245D File Offset: 0x0010145D
		internal virtual void MarkCatchAddr(int catchAddr, Type catchException)
		{
			this.m_currentState = 2;
			this.MarkHelper(catchAddr, catchAddr, catchException, 0);
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x00102470 File Offset: 0x00101470
		internal virtual void MarkFinallyAddr(int finallyAddr, int endCatchAddr)
		{
			if (this.m_endFinally != -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TooManyFinallyClause"));
			}
			this.m_currentState = 3;
			this.m_endFinally = finallyAddr;
			this.MarkHelper(finallyAddr, endCatchAddr, null, 2);
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x001024A3 File Offset: 0x001014A3
		internal virtual void Done(int endAddr)
		{
			this.m_catchEndAddr[this.m_currentCatch - 1] = endAddr;
			this.m_currentState = 5;
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x001024BC File Offset: 0x001014BC
		internal virtual int GetStartAddress()
		{
			return this.m_startAddr;
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x001024C4 File Offset: 0x001014C4
		internal virtual int GetEndAddress()
		{
			return this.m_endAddr;
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x001024CC File Offset: 0x001014CC
		internal virtual int GetFinallyEndAddress()
		{
			return this.m_endFinally;
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x001024D4 File Offset: 0x001014D4
		internal virtual Label GetEndLabel()
		{
			return this.m_endLabel;
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x001024DC File Offset: 0x001014DC
		internal virtual int[] GetFilterAddresses()
		{
			return this.m_filterAddr;
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x001024E4 File Offset: 0x001014E4
		internal virtual int[] GetCatchAddresses()
		{
			return this.m_catchAddr;
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x001024EC File Offset: 0x001014EC
		internal virtual int[] GetCatchEndAddresses()
		{
			return this.m_catchEndAddr;
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x001024F4 File Offset: 0x001014F4
		internal virtual Type[] GetCatchClass()
		{
			return this.m_catchClass;
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x001024FC File Offset: 0x001014FC
		internal virtual int GetNumberOfCatches()
		{
			return this.m_currentCatch;
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x00102504 File Offset: 0x00101504
		internal virtual int[] GetExceptionTypes()
		{
			return this.m_type;
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x0010250C File Offset: 0x0010150C
		internal virtual void SetFinallyEndLabel(Label lbl)
		{
			this.m_finallyEndLabel = lbl;
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x00102515 File Offset: 0x00101515
		internal virtual Label GetFinallyEndLabel()
		{
			return this.m_finallyEndLabel;
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x00102520 File Offset: 0x00101520
		internal bool IsInner(__ExceptionInfo exc)
		{
			int num = exc.m_currentCatch - 1;
			int num2 = this.m_currentCatch - 1;
			return exc.m_catchEndAddr[num] < this.m_catchEndAddr[num2] || (exc.m_catchEndAddr[num] == this.m_catchEndAddr[num2] && exc.GetEndAddress() > this.GetEndAddress());
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x00102576 File Offset: 0x00101576
		internal virtual int GetCurrentState()
		{
			return this.m_currentState;
		}

		// Token: 0x040025F3 RID: 9715
		internal const int None = 0;

		// Token: 0x040025F4 RID: 9716
		internal const int Filter = 1;

		// Token: 0x040025F5 RID: 9717
		internal const int Finally = 2;

		// Token: 0x040025F6 RID: 9718
		internal const int Fault = 4;

		// Token: 0x040025F7 RID: 9719
		internal const int PreserveStack = 4;

		// Token: 0x040025F8 RID: 9720
		internal const int State_Try = 0;

		// Token: 0x040025F9 RID: 9721
		internal const int State_Filter = 1;

		// Token: 0x040025FA RID: 9722
		internal const int State_Catch = 2;

		// Token: 0x040025FB RID: 9723
		internal const int State_Finally = 3;

		// Token: 0x040025FC RID: 9724
		internal const int State_Fault = 4;

		// Token: 0x040025FD RID: 9725
		internal const int State_Done = 5;

		// Token: 0x040025FE RID: 9726
		internal int m_startAddr;

		// Token: 0x040025FF RID: 9727
		internal int[] m_filterAddr;

		// Token: 0x04002600 RID: 9728
		internal int[] m_catchAddr;

		// Token: 0x04002601 RID: 9729
		internal int[] m_catchEndAddr;

		// Token: 0x04002602 RID: 9730
		internal int[] m_type;

		// Token: 0x04002603 RID: 9731
		internal Type[] m_catchClass;

		// Token: 0x04002604 RID: 9732
		internal Label m_endLabel;

		// Token: 0x04002605 RID: 9733
		internal Label m_finallyEndLabel;

		// Token: 0x04002606 RID: 9734
		internal int m_endAddr;

		// Token: 0x04002607 RID: 9735
		internal int m_endFinally;

		// Token: 0x04002608 RID: 9736
		internal int m_currentCatch;

		// Token: 0x04002609 RID: 9737
		private int m_currentState;
	}
}
