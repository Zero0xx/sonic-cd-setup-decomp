using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200083E RID: 2110
	[ComVisible(true)]
	public struct OpCode
	{
		// Token: 0x06004BD2 RID: 19410 RVA: 0x0010A0E8 File Offset: 0x001090E8
		internal OpCode(string stringname, StackBehaviour pop, StackBehaviour push, OperandType operand, OpCodeType type, int size, byte s1, byte s2, FlowControl ctrl, bool endsjmpblk, int stack)
		{
			this.m_stringname = stringname;
			this.m_pop = pop;
			this.m_push = push;
			this.m_operand = operand;
			this.m_type = type;
			this.m_size = size;
			this.m_s1 = s1;
			this.m_s2 = s2;
			this.m_ctrl = ctrl;
			this.m_endsUncondJmpBlk = endsjmpblk;
			this.m_stackChange = stack;
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x0010A14A File Offset: 0x0010914A
		internal bool EndsUncondJmpBlk()
		{
			return this.m_endsUncondJmpBlk;
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0010A152 File Offset: 0x00109152
		internal int StackChange()
		{
			return this.m_stackChange;
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004BD5 RID: 19413 RVA: 0x0010A15A File Offset: 0x0010915A
		public OperandType OperandType
		{
			get
			{
				return this.m_operand;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06004BD6 RID: 19414 RVA: 0x0010A162 File Offset: 0x00109162
		public FlowControl FlowControl
		{
			get
			{
				return this.m_ctrl;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06004BD7 RID: 19415 RVA: 0x0010A16A File Offset: 0x0010916A
		public OpCodeType OpCodeType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x0010A172 File Offset: 0x00109172
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return this.m_pop;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06004BD9 RID: 19417 RVA: 0x0010A17A File Offset: 0x0010917A
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return this.m_push;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x0010A182 File Offset: 0x00109182
		public int Size
		{
			get
			{
				return this.m_size;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06004BDB RID: 19419 RVA: 0x0010A18A File Offset: 0x0010918A
		public short Value
		{
			get
			{
				if (this.m_size == 2)
				{
					return (short)((int)this.m_s1 << 8 | (int)this.m_s2);
				}
				return (short)this.m_s2;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x0010A1AC File Offset: 0x001091AC
		public string Name
		{
			get
			{
				return this.m_stringname;
			}
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x0010A1B4 File Offset: 0x001091B4
		public override bool Equals(object obj)
		{
			return obj is OpCode && this.Equals((OpCode)obj);
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x0010A1CC File Offset: 0x001091CC
		public bool Equals(OpCode obj)
		{
			return obj.m_s1 == this.m_s1 && obj.m_s2 == this.m_s2;
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x0010A1EE File Offset: 0x001091EE
		public static bool operator ==(OpCode a, OpCode b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0010A1F8 File Offset: 0x001091F8
		public static bool operator !=(OpCode a, OpCode b)
		{
			return !(a == b);
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x0010A204 File Offset: 0x00109204
		public override int GetHashCode()
		{
			return this.m_stringname.GetHashCode();
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x0010A211 File Offset: 0x00109211
		public override string ToString()
		{
			return this.m_stringname;
		}

		// Token: 0x0400277C RID: 10108
		internal string m_stringname;

		// Token: 0x0400277D RID: 10109
		internal StackBehaviour m_pop;

		// Token: 0x0400277E RID: 10110
		internal StackBehaviour m_push;

		// Token: 0x0400277F RID: 10111
		internal OperandType m_operand;

		// Token: 0x04002780 RID: 10112
		internal OpCodeType m_type;

		// Token: 0x04002781 RID: 10113
		internal int m_size;

		// Token: 0x04002782 RID: 10114
		internal byte m_s1;

		// Token: 0x04002783 RID: 10115
		internal byte m_s2;

		// Token: 0x04002784 RID: 10116
		internal FlowControl m_ctrl;

		// Token: 0x04002785 RID: 10117
		internal bool m_endsUncondJmpBlk;

		// Token: 0x04002786 RID: 10118
		internal int m_stackChange;
	}
}
