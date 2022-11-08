using System;

namespace System.Diagnostics
{
	// Token: 0x020002C4 RID: 708
	[Serializable]
	internal class LogSwitch
	{
		// Token: 0x06001B66 RID: 7014 RVA: 0x00047507 File Offset: 0x00046507
		private LogSwitch()
		{
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x00047510 File Offset: 0x00046510
		public LogSwitch(string name, string description, LogSwitch parent)
		{
			if (name == null || parent == null)
			{
				throw new ArgumentNullException((name == null) ? "name" : "parent");
			}
			if (name.Length == 0)
			{
				throw new ArgumentOutOfRangeException("Name", Environment.GetResourceString("Argument_StringZeroLength"));
			}
			this.strName = name;
			this.strDescription = description;
			this.iLevel = LoggingLevels.ErrorLevel;
			this.iOldLevel = this.iLevel;
			parent.AddChildSwitch(this);
			this.ParentSwitch = parent;
			this.ChildSwitch = null;
			this.iNumChildren = 0;
			this.iChildArraySize = 0;
			Log.m_Hashtable.Add(this.strName, this);
			Log.AddLogSwitch(this);
			Log.iNumOfSwitches++;
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000475CC File Offset: 0x000465CC
		internal LogSwitch(string name, string description)
		{
			this.strName = name;
			this.strDescription = description;
			this.iLevel = LoggingLevels.ErrorLevel;
			this.iOldLevel = this.iLevel;
			this.ParentSwitch = null;
			this.ChildSwitch = null;
			this.iNumChildren = 0;
			this.iChildArraySize = 0;
			Log.m_Hashtable.Add(this.strName, this);
			Log.AddLogSwitch(this);
			Log.iNumOfSwitches++;
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001B69 RID: 7017 RVA: 0x00047640 File Offset: 0x00046640
		public virtual string Name
		{
			get
			{
				return this.strName;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001B6A RID: 7018 RVA: 0x00047648 File Offset: 0x00046648
		public virtual string Description
		{
			get
			{
				return this.strDescription;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001B6B RID: 7019 RVA: 0x00047650 File Offset: 0x00046650
		public virtual LogSwitch Parent
		{
			get
			{
				return this.ParentSwitch;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x00047658 File Offset: 0x00046658
		// (set) Token: 0x06001B6D RID: 7021 RVA: 0x00047660 File Offset: 0x00046660
		public virtual LoggingLevels MinimumLevel
		{
			get
			{
				return this.iLevel;
			}
			set
			{
				this.iLevel = value;
				this.iOldLevel = value;
				string strParentName = (this.ParentSwitch != null) ? this.ParentSwitch.Name : "";
				if (Debugger.IsAttached)
				{
					Log.ModifyLogSwitch((int)this.iLevel, this.strName, strParentName);
				}
				Log.InvokeLogSwitchLevelHandlers(this, this.iLevel);
			}
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x000476BB File Offset: 0x000466BB
		public virtual bool CheckLevel(LoggingLevels level)
		{
			return this.iLevel <= level || (this.ParentSwitch != null && this.ParentSwitch.CheckLevel(level));
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000476DE File Offset: 0x000466DE
		public static LogSwitch GetSwitch(string name)
		{
			return (LogSwitch)Log.m_Hashtable[name];
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000476F0 File Offset: 0x000466F0
		private void AddChildSwitch(LogSwitch child)
		{
			if (this.iChildArraySize <= this.iNumChildren)
			{
				int num;
				if (this.iChildArraySize == 0)
				{
					num = 10;
				}
				else
				{
					num = this.iChildArraySize * 3 / 2;
				}
				LogSwitch[] array = new LogSwitch[num];
				if (this.iNumChildren > 0)
				{
					Array.Copy(this.ChildSwitch, array, this.iNumChildren);
				}
				this.iChildArraySize = num;
				this.ChildSwitch = array;
			}
			this.ChildSwitch[this.iNumChildren++] = child;
		}

		// Token: 0x04000A84 RID: 2692
		internal string strName;

		// Token: 0x04000A85 RID: 2693
		internal string strDescription;

		// Token: 0x04000A86 RID: 2694
		private LogSwitch ParentSwitch;

		// Token: 0x04000A87 RID: 2695
		private LogSwitch[] ChildSwitch;

		// Token: 0x04000A88 RID: 2696
		internal LoggingLevels iLevel;

		// Token: 0x04000A89 RID: 2697
		internal LoggingLevels iOldLevel;

		// Token: 0x04000A8A RID: 2698
		private int iNumChildren;

		// Token: 0x04000A8B RID: 2699
		private int iChildArraySize;
	}
}
