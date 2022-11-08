using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x0200060B RID: 1547
	public class SendKeys
	{
		// Token: 0x060050E1 RID: 20705 RVA: 0x00127790 File Offset: 0x00126790
		static SendKeys()
		{
			Application.ThreadExit += SendKeys.OnThreadExit;
			SendKeys.messageWindow = new SendKeys.SKWindow();
			SendKeys.messageWindow.CreateControl();
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x00127AF5 File Offset: 0x00126AF5
		private SendKeys()
		{
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x00127AFD File Offset: 0x00126AFD
		private static void AddEvent(SendKeys.SKEvent skevent)
		{
			if (SendKeys.events == null)
			{
				SendKeys.events = new Queue();
			}
			SendKeys.events.Enqueue(skevent);
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x00127B1C File Offset: 0x00126B1C
		private static bool AddSimpleKey(char character, int repeat, IntPtr hwnd, int[] haveKeys, bool fStartNewChar, int cGrp)
		{
			int num = (int)UnsafeNativeMethods.VkKeyScan(character);
			if (num != -1)
			{
				if (haveKeys[0] == 0 && (num & 256) != 0)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 16, fStartNewChar, hwnd));
					fStartNewChar = false;
					haveKeys[0] = 10;
				}
				if (haveKeys[1] == 0 && (num & 512) != 0)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 17, fStartNewChar, hwnd));
					fStartNewChar = false;
					haveKeys[1] = 10;
				}
				if (haveKeys[2] == 0 && (num & 1024) != 0)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 18, fStartNewChar, hwnd));
					fStartNewChar = false;
					haveKeys[2] = 10;
				}
				SendKeys.AddMsgsForVK(num & 255, repeat, haveKeys[2] > 0 && haveKeys[1] == 0, hwnd);
				SendKeys.CancelMods(haveKeys, 10, hwnd);
			}
			else
			{
				int num2 = SafeNativeMethods.OemKeyScan((short)('ÿ' & character));
				for (int i = 0; i < repeat; i++)
				{
					SendKeys.AddEvent(new SendKeys.SKEvent(258, (int)character, num2 & 65535, hwnd));
				}
			}
			if (cGrp != 0)
			{
				fStartNewChar = true;
			}
			return fStartNewChar;
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x00127C18 File Offset: 0x00126C18
		private static void AddMsgsForVK(int vk, int repeat, bool altnoctrldown, IntPtr hwnd)
		{
			for (int i = 0; i < repeat; i++)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(altnoctrldown ? 260 : 256, vk, SendKeys.fStartNewChar, hwnd));
				SendKeys.AddEvent(new SendKeys.SKEvent(altnoctrldown ? 261 : 257, vk, SendKeys.fStartNewChar, hwnd));
			}
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x00127C74 File Offset: 0x00126C74
		private static void CancelMods(int[] haveKeys, int level, IntPtr hwnd)
		{
			if (haveKeys[0] == level)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 16, false, hwnd));
				haveKeys[0] = 0;
			}
			if (haveKeys[1] == level)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 17, false, hwnd));
				haveKeys[1] = 0;
			}
			if (haveKeys[2] == level)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(261, 18, false, hwnd));
				haveKeys[2] = 0;
			}
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x00127CD8 File Offset: 0x00126CD8
		private static void InstallHook()
		{
			if (SendKeys.hhook == IntPtr.Zero)
			{
				SendKeys.hook = new NativeMethods.HookProc(new SendKeys.SendKeysHookProc().Callback);
				SendKeys.stopHook = false;
				SendKeys.hhook = UnsafeNativeMethods.SetWindowsHookEx(1, SendKeys.hook, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null)), 0);
				if (SendKeys.hhook == IntPtr.Zero)
				{
					throw new SecurityException(SR.GetString("SendKeysHookFailed"));
				}
			}
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x00127D50 File Offset: 0x00126D50
		private static void TestHook()
		{
			SendKeys.hookSupported = new bool?(false);
			try
			{
				NativeMethods.HookProc pfnhook = new NativeMethods.HookProc(SendKeys.EmptyHookCallback);
				IntPtr intPtr = UnsafeNativeMethods.SetWindowsHookEx(1, pfnhook, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null)), 0);
				SendKeys.hookSupported = new bool?(intPtr != IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(null, intPtr));
				}
			}
			catch
			{
			}
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x00127DD0 File Offset: 0x00126DD0
		private static IntPtr EmptyHookCallback(int code, IntPtr wparam, IntPtr lparam)
		{
			return IntPtr.Zero;
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x00127DD8 File Offset: 0x00126DD8
		private static void LoadSendMethodFromConfig()
		{
			if (SendKeys.sendMethod == null)
			{
				SendKeys.sendMethod = new SendKeys.SendMethodTypes?(SendKeys.SendMethodTypes.Default);
				try
				{
					string text = ConfigurationManager.AppSettings.Get("SendKeys");
					if (text.Equals("JournalHook", StringComparison.OrdinalIgnoreCase))
					{
						SendKeys.sendMethod = new SendKeys.SendMethodTypes?(SendKeys.SendMethodTypes.JournalHook);
					}
					else if (text.Equals("SendInput", StringComparison.OrdinalIgnoreCase))
					{
						SendKeys.sendMethod = new SendKeys.SendMethodTypes?(SendKeys.SendMethodTypes.SendInput);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x00127E58 File Offset: 0x00126E58
		private static void JournalCancel()
		{
			if (SendKeys.hhook != IntPtr.Zero)
			{
				SendKeys.stopHook = false;
				if (SendKeys.events != null)
				{
					SendKeys.events.Clear();
				}
				SendKeys.hhook = IntPtr.Zero;
			}
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x00127E8C File Offset: 0x00126E8C
		private static byte[] GetKeyboardState()
		{
			byte[] array = new byte[256];
			UnsafeNativeMethods.GetKeyboardState(array);
			return array;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x00127EAC File Offset: 0x00126EAC
		private static void SetKeyboardState(byte[] keystate)
		{
			UnsafeNativeMethods.SetKeyboardState(keystate);
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x00127EB8 File Offset: 0x00126EB8
		private static void ClearKeyboardState()
		{
			byte[] keyboardState = SendKeys.GetKeyboardState();
			keyboardState[20] = 0;
			keyboardState[144] = 0;
			keyboardState[145] = 0;
			SendKeys.SetKeyboardState(keyboardState);
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x00127EE8 File Offset: 0x00126EE8
		private static int MatchKeyword(string keyword)
		{
			for (int i = 0; i < SendKeys.keywords.Length; i++)
			{
				if (string.Equals(SendKeys.keywords[i].keyword, keyword, StringComparison.OrdinalIgnoreCase))
				{
					return SendKeys.keywords[i].vk;
				}
			}
			return -1;
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x00127F2C File Offset: 0x00126F2C
		private static void OnThreadExit(object sender, EventArgs e)
		{
			try
			{
				SendKeys.UninstallJournalingHook();
			}
			catch
			{
			}
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x00127F54 File Offset: 0x00126F54
		private static void ParseKeys(string keys, IntPtr hwnd)
		{
			int i = 0;
			int[] array = new int[3];
			int[] array2 = array;
			int num = 0;
			SendKeys.fStartNewChar = true;
			int length = keys.Length;
			while (i < length)
			{
				int repeat = 1;
				char c = keys[i];
				char c2 = c;
				switch (c2)
				{
				case '%':
					if (array2[2] != 0)
					{
						throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
						{
							keys
						}));
					}
					SendKeys.AddEvent(new SendKeys.SKEvent((array2[1] != 0) ? 256 : 260, 18, SendKeys.fStartNewChar, hwnd));
					SendKeys.fStartNewChar = false;
					array2[2] = 10;
					break;
				case '&':
				case '\'':
				case '*':
					goto IL_490;
				case '(':
					num++;
					if (num > 3)
					{
						throw new ArgumentException(SR.GetString("SendKeysNestingError"));
					}
					if (array2[0] == 10)
					{
						array2[0] = num;
					}
					if (array2[1] == 10)
					{
						array2[1] = num;
					}
					if (array2[2] == 10)
					{
						array2[2] = num;
					}
					break;
				case ')':
					if (num < 1)
					{
						throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
						{
							keys
						}));
					}
					SendKeys.CancelMods(array2, num, hwnd);
					num--;
					if (num == 0)
					{
						SendKeys.fStartNewChar = true;
					}
					break;
				case '+':
					if (array2[0] != 0)
					{
						throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
						{
							keys
						}));
					}
					SendKeys.AddEvent(new SendKeys.SKEvent(256, 16, SendKeys.fStartNewChar, hwnd));
					SendKeys.fStartNewChar = false;
					array2[0] = 10;
					break;
				default:
					if (c2 != '^')
					{
						switch (c2)
						{
						case '{':
						{
							int num2 = i + 1;
							if (num2 + 1 < length && keys[num2] == '}')
							{
								int num3 = num2 + 1;
								while (num3 < length && keys[num3] != '}')
								{
									num3++;
								}
								if (num3 < length)
								{
									num2++;
								}
							}
							while (num2 < length && keys[num2] != '}' && !char.IsWhiteSpace(keys[num2]))
							{
								num2++;
							}
							if (num2 >= length)
							{
								throw new ArgumentException(SR.GetString("SendKeysKeywordDelimError"));
							}
							string text = keys.Substring(i + 1, num2 - (i + 1));
							if (char.IsWhiteSpace(keys[num2]))
							{
								while (num2 < length && char.IsWhiteSpace(keys[num2]))
								{
									num2++;
								}
								if (num2 >= length)
								{
									throw new ArgumentException(SR.GetString("SendKeysKeywordDelimError"));
								}
								if (char.IsDigit(keys[num2]))
								{
									int num4 = num2;
									while (num2 < length && char.IsDigit(keys[num2]))
									{
										num2++;
									}
									repeat = int.Parse(keys.Substring(num4, num2 - num4), CultureInfo.InvariantCulture);
								}
							}
							if (num2 >= length)
							{
								throw new ArgumentException(SR.GetString("SendKeysKeywordDelimError"));
							}
							if (keys[num2] != '}')
							{
								throw new ArgumentException(SR.GetString("InvalidSendKeysRepeat"));
							}
							int num5 = SendKeys.MatchKeyword(text);
							if (num5 != -1)
							{
								if (array2[0] == 0 && (num5 & 65536) != 0)
								{
									SendKeys.AddEvent(new SendKeys.SKEvent(256, 16, SendKeys.fStartNewChar, hwnd));
									SendKeys.fStartNewChar = false;
									array2[0] = 10;
								}
								if (array2[1] == 0 && (num5 & 131072) != 0)
								{
									SendKeys.AddEvent(new SendKeys.SKEvent(256, 17, SendKeys.fStartNewChar, hwnd));
									SendKeys.fStartNewChar = false;
									array2[1] = 10;
								}
								if (array2[2] == 0 && (num5 & 262144) != 0)
								{
									SendKeys.AddEvent(new SendKeys.SKEvent(256, 18, SendKeys.fStartNewChar, hwnd));
									SendKeys.fStartNewChar = false;
									array2[2] = 10;
								}
								SendKeys.AddMsgsForVK(num5, repeat, array2[2] > 0 && array2[1] == 0, hwnd);
								SendKeys.CancelMods(array2, 10, hwnd);
							}
							else
							{
								if (text.Length != 1)
								{
									throw new ArgumentException(SR.GetString("InvalidSendKeysKeyword", new object[]
									{
										keys.Substring(i + 1, num2 - (i + 1))
									}));
								}
								SendKeys.fStartNewChar = SendKeys.AddSimpleKey(text[0], repeat, hwnd, array2, SendKeys.fStartNewChar, num);
							}
							i = num2;
							break;
						}
						case '|':
							goto IL_490;
						case '}':
							throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
							{
								keys
							}));
						case '~':
						{
							int num5 = 13;
							SendKeys.AddMsgsForVK(num5, repeat, array2[2] > 0 && array2[1] == 0, hwnd);
							break;
						}
						default:
							goto IL_490;
						}
					}
					else
					{
						if (array2[1] != 0)
						{
							throw new ArgumentException(SR.GetString("InvalidSendKeysString", new object[]
							{
								keys
							}));
						}
						SendKeys.AddEvent(new SendKeys.SKEvent(256, 17, SendKeys.fStartNewChar, hwnd));
						SendKeys.fStartNewChar = false;
						array2[1] = 10;
					}
					break;
				}
				IL_4AB:
				i++;
				continue;
				IL_490:
				SendKeys.fStartNewChar = SendKeys.AddSimpleKey(keys[i], repeat, hwnd, array2, SendKeys.fStartNewChar, num);
				goto IL_4AB;
			}
			if (num != 0)
			{
				throw new ArgumentException(SR.GetString("SendKeysGroupDelimError"));
			}
			SendKeys.CancelMods(array2, 10, hwnd);
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x00128434 File Offset: 0x00127434
		private static void SendInput(byte[] oldKeyboardState, Queue previousEvents)
		{
			SendKeys.AddCancelModifiersForPreviousEvents(previousEvents);
			NativeMethods.INPUT[] array = new NativeMethods.INPUT[2];
			array[0].type = 1;
			array[1].type = 1;
			array[1].inputUnion.ki.wVk = 0;
			array[1].inputUnion.ki.dwFlags = 6;
			array[0].inputUnion.ki.dwExtraInfo = IntPtr.Zero;
			array[0].inputUnion.ki.time = 0;
			array[1].inputUnion.ki.dwExtraInfo = IntPtr.Zero;
			array[1].inputUnion.ki.time = 0;
			int num = Marshal.SizeOf(typeof(NativeMethods.INPUT));
			uint num2 = 0U;
			int count;
			lock (SendKeys.events.SyncRoot)
			{
				bool flag = UnsafeNativeMethods.BlockInput(true);
				try
				{
					count = SendKeys.events.Count;
					SendKeys.ClearGlobalKeys();
					for (int i = 0; i < count; i++)
					{
						SendKeys.SKEvent skevent = (SendKeys.SKEvent)SendKeys.events.Dequeue();
						array[0].inputUnion.ki.dwFlags = 0;
						if (skevent.wm == 258)
						{
							array[0].inputUnion.ki.wVk = 0;
							array[0].inputUnion.ki.wScan = (short)skevent.paramL;
							array[0].inputUnion.ki.dwFlags = 4;
							array[1].inputUnion.ki.wScan = (short)skevent.paramL;
							num2 += UnsafeNativeMethods.SendInput(2U, array, num) - 1U;
						}
						else
						{
							array[0].inputUnion.ki.wScan = 0;
							if (skevent.wm == 257 || skevent.wm == 261)
							{
								NativeMethods.INPUT[] array2 = array;
								int num3 = 0;
								array2[num3].inputUnion.ki.dwFlags = (array2[num3].inputUnion.ki.dwFlags | 2);
							}
							if (SendKeys.IsExtendedKey(skevent))
							{
								NativeMethods.INPUT[] array3 = array;
								int num4 = 0;
								array3[num4].inputUnion.ki.dwFlags = (array3[num4].inputUnion.ki.dwFlags | 1);
							}
							array[0].inputUnion.ki.wVk = (short)skevent.paramL;
							num2 += UnsafeNativeMethods.SendInput(1U, array, num);
							SendKeys.CheckGlobalKeys(skevent);
						}
						Thread.Sleep(1);
					}
					SendKeys.ResetKeyboardUsingSendInput(num);
				}
				finally
				{
					SendKeys.SetKeyboardState(oldKeyboardState);
					if (flag)
					{
						UnsafeNativeMethods.BlockInput(false);
					}
				}
			}
			if ((ulong)num2 != (ulong)((long)count))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x00128710 File Offset: 0x00127710
		private static void AddCancelModifiersForPreviousEvents(Queue previousEvents)
		{
			if (previousEvents == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			while (previousEvents.Count > 0)
			{
				SendKeys.SKEvent skevent = (SendKeys.SKEvent)previousEvents.Dequeue();
				bool flag4;
				if (skevent.wm == 257 || skevent.wm == 261)
				{
					flag4 = false;
				}
				else
				{
					if (skevent.wm != 256 && skevent.wm != 260)
					{
						continue;
					}
					flag4 = true;
				}
				if (skevent.paramL == 16)
				{
					flag = flag4;
				}
				else if (skevent.paramL == 17)
				{
					flag2 = flag4;
				}
				else if (skevent.paramL == 18)
				{
					flag3 = flag4;
				}
			}
			if (flag)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 16, false, IntPtr.Zero));
				return;
			}
			if (flag2)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(257, 17, false, IntPtr.Zero));
				return;
			}
			if (flag3)
			{
				SendKeys.AddEvent(new SendKeys.SKEvent(261, 18, false, IntPtr.Zero));
			}
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x001287F8 File Offset: 0x001277F8
		private static bool IsExtendedKey(SendKeys.SKEvent skEvent)
		{
			return skEvent.paramL == 38 || skEvent.paramL == 40 || skEvent.paramL == 37 || skEvent.paramL == 39 || skEvent.paramL == 33 || skEvent.paramL == 34 || skEvent.paramL == 36 || skEvent.paramL == 35 || skEvent.paramL == 45 || skEvent.paramL == 46;
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x0012886B File Offset: 0x0012786B
		private static void ClearGlobalKeys()
		{
			SendKeys.capslockChanged = false;
			SendKeys.numlockChanged = false;
			SendKeys.scrollLockChanged = false;
			SendKeys.kanaChanged = false;
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x00128888 File Offset: 0x00127888
		private static void CheckGlobalKeys(SendKeys.SKEvent skEvent)
		{
			if (skEvent.wm == 256)
			{
				int paramL = skEvent.paramL;
				switch (paramL)
				{
				case 20:
					SendKeys.capslockChanged = !SendKeys.capslockChanged;
					return;
				case 21:
					SendKeys.kanaChanged = !SendKeys.kanaChanged;
					break;
				default:
					switch (paramL)
					{
					case 144:
						SendKeys.numlockChanged = !SendKeys.numlockChanged;
						return;
					case 145:
						SendKeys.scrollLockChanged = !SendKeys.scrollLockChanged;
						return;
					default:
						return;
					}
					break;
				}
			}
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x00128908 File Offset: 0x00127908
		private static void ResetKeyboardUsingSendInput(int INPUTSize)
		{
			if (!SendKeys.capslockChanged && !SendKeys.numlockChanged && !SendKeys.scrollLockChanged && !SendKeys.kanaChanged)
			{
				return;
			}
			NativeMethods.INPUT[] array = new NativeMethods.INPUT[2];
			array[0].type = 1;
			array[0].inputUnion.ki.dwFlags = 0;
			array[1].type = 1;
			array[1].inputUnion.ki.dwFlags = 2;
			if (SendKeys.capslockChanged)
			{
				array[0].inputUnion.ki.wVk = 20;
				array[1].inputUnion.ki.wVk = 20;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
			if (SendKeys.numlockChanged)
			{
				array[0].inputUnion.ki.wVk = 144;
				array[1].inputUnion.ki.wVk = 144;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
			if (SendKeys.scrollLockChanged)
			{
				array[0].inputUnion.ki.wVk = 145;
				array[1].inputUnion.ki.wVk = 145;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
			if (SendKeys.kanaChanged)
			{
				array[0].inputUnion.ki.wVk = 21;
				array[1].inputUnion.ki.wVk = 21;
				UnsafeNativeMethods.SendInput(2U, array, INPUTSize);
			}
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x00128A8D File Offset: 0x00127A8D
		public static void Send(string keys)
		{
			SendKeys.Send(keys, null, false);
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x00128A98 File Offset: 0x00127A98
		private static void Send(string keys, Control control, bool wait)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (keys == null || keys.Length == 0)
			{
				return;
			}
			if (!wait && !Application.MessageLoop)
			{
				throw new InvalidOperationException(SR.GetString("SendKeysNoMessageLoop"));
			}
			Queue previousEvents = null;
			if (SendKeys.events != null && SendKeys.events.Count != 0)
			{
				previousEvents = (Queue)SendKeys.events.Clone();
			}
			SendKeys.ParseKeys(keys, (control != null) ? control.Handle : IntPtr.Zero);
			if (SendKeys.events == null)
			{
				return;
			}
			SendKeys.LoadSendMethodFromConfig();
			byte[] keyboardState = SendKeys.GetKeyboardState();
			if (SendKeys.sendMethod.Value != SendKeys.SendMethodTypes.SendInput)
			{
				if (SendKeys.hookSupported == null && SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.Default)
				{
					SendKeys.TestHook();
				}
				if (SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.JournalHook || SendKeys.hookSupported.Value)
				{
					SendKeys.ClearKeyboardState();
					SendKeys.InstallHook();
					SendKeys.SetKeyboardState(keyboardState);
				}
			}
			if (SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.SendInput || (SendKeys.sendMethod.Value == SendKeys.SendMethodTypes.Default && !SendKeys.hookSupported.Value))
			{
				SendKeys.SendInput(keyboardState, previousEvents);
			}
			if (wait)
			{
				SendKeys.Flush();
			}
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x00128BAC File Offset: 0x00127BAC
		public static void SendWait(string keys)
		{
			SendKeys.SendWait(keys, null);
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x00128BB5 File Offset: 0x00127BB5
		private static void SendWait(string keys, Control control)
		{
			SendKeys.Send(keys, control, true);
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x00128BBF File Offset: 0x00127BBF
		public static void Flush()
		{
			Application.DoEvents();
			while (SendKeys.events != null && SendKeys.events.Count > 0)
			{
				Application.DoEvents();
			}
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x00128BE4 File Offset: 0x00127BE4
		private static void UninstallJournalingHook()
		{
			if (SendKeys.hhook != IntPtr.Zero)
			{
				SendKeys.stopHook = false;
				if (SendKeys.events != null)
				{
					SendKeys.events.Clear();
				}
				UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(null, SendKeys.hhook));
				SendKeys.hhook = IntPtr.Zero;
			}
		}

		// Token: 0x0400350B RID: 13579
		private const int HAVESHIFT = 0;

		// Token: 0x0400350C RID: 13580
		private const int HAVECTRL = 1;

		// Token: 0x0400350D RID: 13581
		private const int HAVEALT = 2;

		// Token: 0x0400350E RID: 13582
		private const int UNKNOWN_GROUPING = 10;

		// Token: 0x0400350F RID: 13583
		private const int SHIFTKEYSCAN = 256;

		// Token: 0x04003510 RID: 13584
		private const int CTRLKEYSCAN = 512;

		// Token: 0x04003511 RID: 13585
		private const int ALTKEYSCAN = 1024;

		// Token: 0x04003512 RID: 13586
		private static SendKeys.KeywordVk[] keywords = new SendKeys.KeywordVk[]
		{
			new SendKeys.KeywordVk("ENTER", 13),
			new SendKeys.KeywordVk("TAB", 9),
			new SendKeys.KeywordVk("ESC", 27),
			new SendKeys.KeywordVk("ESCAPE", 27),
			new SendKeys.KeywordVk("HOME", 36),
			new SendKeys.KeywordVk("END", 35),
			new SendKeys.KeywordVk("LEFT", 37),
			new SendKeys.KeywordVk("RIGHT", 39),
			new SendKeys.KeywordVk("UP", 38),
			new SendKeys.KeywordVk("DOWN", 40),
			new SendKeys.KeywordVk("PGUP", 33),
			new SendKeys.KeywordVk("PGDN", 34),
			new SendKeys.KeywordVk("NUMLOCK", 144),
			new SendKeys.KeywordVk("SCROLLLOCK", 145),
			new SendKeys.KeywordVk("PRTSC", 44),
			new SendKeys.KeywordVk("BREAK", 3),
			new SendKeys.KeywordVk("BACKSPACE", 8),
			new SendKeys.KeywordVk("BKSP", 8),
			new SendKeys.KeywordVk("BS", 8),
			new SendKeys.KeywordVk("CLEAR", 12),
			new SendKeys.KeywordVk("CAPSLOCK", 20),
			new SendKeys.KeywordVk("INS", 45),
			new SendKeys.KeywordVk("INSERT", 45),
			new SendKeys.KeywordVk("DEL", 46),
			new SendKeys.KeywordVk("DELETE", 46),
			new SendKeys.KeywordVk("HELP", 47),
			new SendKeys.KeywordVk("F1", 112),
			new SendKeys.KeywordVk("F2", 113),
			new SendKeys.KeywordVk("F3", 114),
			new SendKeys.KeywordVk("F4", 115),
			new SendKeys.KeywordVk("F5", 116),
			new SendKeys.KeywordVk("F6", 117),
			new SendKeys.KeywordVk("F7", 118),
			new SendKeys.KeywordVk("F8", 119),
			new SendKeys.KeywordVk("F9", 120),
			new SendKeys.KeywordVk("F10", 121),
			new SendKeys.KeywordVk("F11", 122),
			new SendKeys.KeywordVk("F12", 123),
			new SendKeys.KeywordVk("F13", 124),
			new SendKeys.KeywordVk("F14", 125),
			new SendKeys.KeywordVk("F15", 126),
			new SendKeys.KeywordVk("F16", 127),
			new SendKeys.KeywordVk("MULTIPLY", 106),
			new SendKeys.KeywordVk("ADD", 107),
			new SendKeys.KeywordVk("SUBTRACT", 109),
			new SendKeys.KeywordVk("DIVIDE", 111),
			new SendKeys.KeywordVk("+", 107),
			new SendKeys.KeywordVk("%", 65589),
			new SendKeys.KeywordVk("^", 65590)
		};

		// Token: 0x04003513 RID: 13587
		private static bool stopHook;

		// Token: 0x04003514 RID: 13588
		private static IntPtr hhook;

		// Token: 0x04003515 RID: 13589
		private static NativeMethods.HookProc hook;

		// Token: 0x04003516 RID: 13590
		private static Queue events;

		// Token: 0x04003517 RID: 13591
		private static bool fStartNewChar;

		// Token: 0x04003518 RID: 13592
		private static SendKeys.SKWindow messageWindow;

		// Token: 0x04003519 RID: 13593
		private static SendKeys.SendMethodTypes? sendMethod = null;

		// Token: 0x0400351A RID: 13594
		private static bool? hookSupported = null;

		// Token: 0x0400351B RID: 13595
		private static bool capslockChanged;

		// Token: 0x0400351C RID: 13596
		private static bool numlockChanged;

		// Token: 0x0400351D RID: 13597
		private static bool scrollLockChanged;

		// Token: 0x0400351E RID: 13598
		private static bool kanaChanged;

		// Token: 0x0200060C RID: 1548
		private enum SendMethodTypes
		{
			// Token: 0x04003520 RID: 13600
			Default = 1,
			// Token: 0x04003521 RID: 13601
			JournalHook,
			// Token: 0x04003522 RID: 13602
			SendInput
		}

		// Token: 0x0200060D RID: 1549
		private class SKWindow : Control
		{
			// Token: 0x060050FE RID: 20734 RVA: 0x00128C34 File Offset: 0x00127C34
			public SKWindow()
			{
				base.SetState(524288, true);
				base.SetState2(8, false);
				base.SetBounds(-1, -1, 0, 0);
				base.Visible = false;
			}

			// Token: 0x060050FF RID: 20735 RVA: 0x00128C64 File Offset: 0x00127C64
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 75)
				{
					try
					{
						SendKeys.JournalCancel();
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0200060E RID: 1550
		private class SKEvent
		{
			// Token: 0x06005100 RID: 20736 RVA: 0x00128C98 File Offset: 0x00127C98
			public SKEvent(int a, int b, bool c, IntPtr hwnd)
			{
				this.wm = a;
				this.paramL = b;
				this.paramH = (c ? 1 : 0);
				this.hwnd = hwnd;
			}

			// Token: 0x06005101 RID: 20737 RVA: 0x00128CC3 File Offset: 0x00127CC3
			public SKEvent(int a, int b, int c, IntPtr hwnd)
			{
				this.wm = a;
				this.paramL = b;
				this.paramH = c;
				this.hwnd = hwnd;
			}

			// Token: 0x04003523 RID: 13603
			internal int wm;

			// Token: 0x04003524 RID: 13604
			internal int paramL;

			// Token: 0x04003525 RID: 13605
			internal int paramH;

			// Token: 0x04003526 RID: 13606
			internal IntPtr hwnd;
		}

		// Token: 0x0200060F RID: 1551
		private class KeywordVk
		{
			// Token: 0x06005102 RID: 20738 RVA: 0x00128CE8 File Offset: 0x00127CE8
			public KeywordVk(string key, int v)
			{
				this.keyword = key;
				this.vk = v;
			}

			// Token: 0x04003527 RID: 13607
			internal string keyword;

			// Token: 0x04003528 RID: 13608
			internal int vk;
		}

		// Token: 0x02000610 RID: 1552
		private class SendKeysHookProc
		{
			// Token: 0x06005103 RID: 20739 RVA: 0x00128D00 File Offset: 0x00127D00
			public virtual IntPtr Callback(int code, IntPtr wparam, IntPtr lparam)
			{
				NativeMethods.EVENTMSG eventmsg = (NativeMethods.EVENTMSG)UnsafeNativeMethods.PtrToStructure(lparam, typeof(NativeMethods.EVENTMSG));
				if (UnsafeNativeMethods.GetAsyncKeyState(19) != 0)
				{
					SendKeys.stopHook = true;
				}
				switch (code)
				{
				case 1:
				{
					this.gotNextEvent = true;
					SendKeys.SKEvent skevent = (SendKeys.SKEvent)SendKeys.events.Peek();
					eventmsg.message = skevent.wm;
					eventmsg.paramL = skevent.paramL;
					eventmsg.paramH = skevent.paramH;
					eventmsg.hwnd = skevent.hwnd;
					eventmsg.time = SafeNativeMethods.GetTickCount();
					Marshal.StructureToPtr(eventmsg, lparam, true);
					break;
				}
				case 2:
					if (this.gotNextEvent)
					{
						if (SendKeys.events != null && SendKeys.events.Count > 0)
						{
							SendKeys.events.Dequeue();
						}
						SendKeys.stopHook = (SendKeys.events == null || SendKeys.events.Count == 0);
					}
					break;
				default:
					if (code < 0)
					{
						UnsafeNativeMethods.CallNextHookEx(new HandleRef(null, SendKeys.hhook), code, wparam, lparam);
					}
					break;
				}
				if (SendKeys.stopHook)
				{
					SendKeys.UninstallJournalingHook();
					this.gotNextEvent = false;
				}
				return IntPtr.Zero;
			}

			// Token: 0x04003529 RID: 13609
			private bool gotNextEvent;
		}
	}
}
