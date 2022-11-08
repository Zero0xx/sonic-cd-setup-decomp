using System;

namespace System.Windows.Forms
{
	// Token: 0x0200029B RID: 667
	internal class Command : WeakReference
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x0005245C File Offset: 0x0005145C
		public Command(ICommandExecutor target) : base(target, false)
		{
			Command.AssignID(this);
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x0005246C File Offset: 0x0005146C
		public virtual int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x00052474 File Offset: 0x00051474
		protected static void AssignID(Command cmd)
		{
			lock (Command.internalSyncObject)
			{
				int i;
				if (Command.cmds == null)
				{
					Command.cmds = new Command[20];
					i = 0;
				}
				else
				{
					int num = Command.cmds.Length;
					if (Command.icmdTry >= num)
					{
						Command.icmdTry = 0;
					}
					for (i = Command.icmdTry; i < num; i++)
					{
						if (Command.cmds[i] == null)
						{
							goto IL_FB;
						}
					}
					for (i = 0; i < Command.icmdTry; i++)
					{
						if (Command.cmds[i] == null)
						{
							goto IL_FB;
						}
					}
					for (i = 0; i < num; i++)
					{
						if (Command.cmds[i].Target == null)
						{
							goto IL_FB;
						}
					}
					i = Command.cmds.Length;
					num = Math.Min(65280, 2 * i);
					if (num <= i)
					{
						GC.Collect();
						for (i = 0; i < num; i++)
						{
							if (Command.cmds[i] == null || Command.cmds[i].Target == null)
							{
								goto IL_FB;
							}
						}
						throw new ArgumentException(SR.GetString("CommandIdNotAllocated"));
					}
					Command[] destinationArray = new Command[num];
					Array.Copy(Command.cmds, 0, destinationArray, 0, i);
					Command.cmds = destinationArray;
				}
				IL_FB:
				cmd.id = i + 256;
				Command.cmds[i] = cmd;
				Command.icmdTry = i + 1;
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000525C0 File Offset: 0x000515C0
		public static bool DispatchID(int id)
		{
			Command commandFromID = Command.GetCommandFromID(id);
			return commandFromID != null && commandFromID.Invoke();
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000525E0 File Offset: 0x000515E0
		protected static void Dispose(Command cmd)
		{
			lock (Command.internalSyncObject)
			{
				if (cmd.id >= 256)
				{
					cmd.Target = null;
					if (Command.cmds[cmd.id - 256] == cmd)
					{
						Command.cmds[cmd.id - 256] = null;
					}
					cmd.id = 0;
				}
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x00052658 File Offset: 0x00051658
		public virtual void Dispose()
		{
			if (this.id >= 256)
			{
				Command.Dispose(this);
			}
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x00052670 File Offset: 0x00051670
		public static Command GetCommandFromID(int id)
		{
			Command result;
			lock (Command.internalSyncObject)
			{
				if (Command.cmds == null)
				{
					result = null;
				}
				else
				{
					int num = id - 256;
					if (num < 0 || num >= Command.cmds.Length)
					{
						result = null;
					}
					else
					{
						result = Command.cmds[num];
					}
				}
			}
			return result;
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x000526D0 File Offset: 0x000516D0
		public virtual bool Invoke()
		{
			object target = this.Target;
			if (!(target is ICommandExecutor))
			{
				return false;
			}
			((ICommandExecutor)target).Execute();
			return true;
		}

		// Token: 0x0400158E RID: 5518
		private const int idMin = 256;

		// Token: 0x0400158F RID: 5519
		private const int idLim = 65536;

		// Token: 0x04001590 RID: 5520
		private static Command[] cmds;

		// Token: 0x04001591 RID: 5521
		private static int icmdTry;

		// Token: 0x04001592 RID: 5522
		private static object internalSyncObject = new object();

		// Token: 0x04001593 RID: 5523
		internal int id;
	}
}
