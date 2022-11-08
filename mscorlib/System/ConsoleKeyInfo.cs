using System;

namespace System
{
	// Token: 0x02000095 RID: 149
	[Serializable]
	public struct ConsoleKeyInfo
	{
		// Token: 0x060007FE RID: 2046 RVA: 0x0001A278 File Offset: 0x00019278
		public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
		{
			if (key < (ConsoleKey)0 || key > (ConsoleKey)255)
			{
				throw new ArgumentOutOfRangeException("key", Environment.GetResourceString("ArgumentOutOfRange_ConsoleKey"));
			}
			this._keyChar = keyChar;
			this._key = key;
			this._mods = (ConsoleModifiers)0;
			if (shift)
			{
				this._mods |= ConsoleModifiers.Shift;
			}
			if (alt)
			{
				this._mods |= ConsoleModifiers.Alt;
			}
			if (control)
			{
				this._mods |= ConsoleModifiers.Control;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0001A2F0 File Offset: 0x000192F0
		public char KeyChar
		{
			get
			{
				return this._keyChar;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0001A2F8 File Offset: 0x000192F8
		public ConsoleKey Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0001A300 File Offset: 0x00019300
		public ConsoleModifiers Modifiers
		{
			get
			{
				return this._mods;
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001A308 File Offset: 0x00019308
		public override bool Equals(object value)
		{
			return value is ConsoleKeyInfo && this.Equals((ConsoleKeyInfo)value);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001A320 File Offset: 0x00019320
		public bool Equals(ConsoleKeyInfo obj)
		{
			return obj._keyChar == this._keyChar && obj._key == this._key && obj._mods == this._mods;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001A351 File Offset: 0x00019351
		public static bool operator ==(ConsoleKeyInfo a, ConsoleKeyInfo b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001A35B File Offset: 0x0001935B
		public static bool operator !=(ConsoleKeyInfo a, ConsoleKeyInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001A367 File Offset: 0x00019367
		public override int GetHashCode()
		{
			return (int)((ConsoleModifiers)this._keyChar | this._mods);
		}

		// Token: 0x0400037A RID: 890
		private char _keyChar;

		// Token: 0x0400037B RID: 891
		private ConsoleKey _key;

		// Token: 0x0400037C RID: 892
		private ConsoleModifiers _mods;
	}
}
