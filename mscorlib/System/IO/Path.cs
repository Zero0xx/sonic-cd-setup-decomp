using System;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x020005C0 RID: 1472
	[ComVisible(true)]
	public static class Path
	{
		// Token: 0x06003689 RID: 13961 RVA: 0x000B75F0 File Offset: 0x000B65F0
		public static string ChangeExtension(string path, string extension)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				string text = path;
				int num = path.Length;
				while (--num >= 0)
				{
					char c = path[num];
					if (c == '.')
					{
						text = path.Substring(0, num);
						break;
					}
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						break;
					}
				}
				if (extension != null && path.Length != 0)
				{
					if (extension.Length == 0 || extension[0] != '.')
					{
						text += ".";
					}
					text += extension;
				}
				return text;
			}
			return null;
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x000B7680 File Offset: 0x000B6680
		public static string GetDirectoryName(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				path = Path.FixupPath(path);
				int rootLength = Path.GetRootLength(path);
				int num = path.Length;
				if (num > rootLength)
				{
					num = path.Length;
					if (num == rootLength)
					{
						return null;
					}
					while (num > rootLength && path[--num] != Path.DirectorySeparatorChar && path[num] != Path.AltDirectorySeparatorChar)
					{
					}
					return path.Substring(0, num);
				}
			}
			return null;
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x000B76EC File Offset: 0x000B66EC
		internal static int GetRootLength(string path)
		{
			Path.CheckInvalidPathChars(path);
			int i = 0;
			int length = path.Length;
			if (length >= 1 && Path.IsDirectorySeparator(path[0]))
			{
				i = 1;
				if (length >= 2 && Path.IsDirectorySeparator(path[1]))
				{
					i = 2;
					int num = 2;
					while (i < length)
					{
						if ((path[i] == Path.DirectorySeparatorChar || path[i] == Path.AltDirectorySeparatorChar) && --num <= 0)
						{
							break;
						}
						i++;
					}
				}
			}
			else if (length >= 2 && path[1] == Path.VolumeSeparatorChar)
			{
				i = 2;
				if (length >= 3 && Path.IsDirectorySeparator(path[2]))
				{
					i++;
				}
			}
			return i;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x000B778D File Offset: 0x000B678D
		internal static bool IsDirectorySeparator(char c)
		{
			return c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x000B77A1 File Offset: 0x000B67A1
		public static char[] GetInvalidPathChars()
		{
			return (char[])Path.RealInvalidPathChars.Clone();
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x000B77B2 File Offset: 0x000B67B2
		public static char[] GetInvalidFileNameChars()
		{
			return (char[])Path.InvalidFileNameChars.Clone();
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x000B77C4 File Offset: 0x000B67C4
		public static string GetExtension(string path)
		{
			if (path == null)
			{
				return null;
			}
			Path.CheckInvalidPathChars(path);
			int length = path.Length;
			int num = length;
			while (--num >= 0)
			{
				char c = path[num];
				if (c == '.')
				{
					if (num != length - 1)
					{
						return path.Substring(num, length - num);
					}
					return string.Empty;
				}
				else if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
				{
					break;
				}
			}
			return string.Empty;
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000B7830 File Offset: 0x000B6830
		public static string GetFullPath(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			return fullPathInternal;
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x000B7860 File Offset: 0x000B6860
		internal static string GetFullPathInternal(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return Path.NormalizePath(path, true);
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000B7884 File Offset: 0x000B6884
		internal static string NormalizePath(string path, bool fullCheck)
		{
			if (Environment.nativeIsWin9x())
			{
				return Path.NormalizePathSlow(path, fullCheck);
			}
			return Path.NormalizePathFast(path, fullCheck);
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000B789C File Offset: 0x000B689C
		internal static string NormalizePathSlow(string path, bool fullCheck)
		{
			if (fullCheck)
			{
				path = path.TrimEnd(new char[0]);
				Path.CheckInvalidPathChars(path);
			}
			int i = 0;
			char[] array = new char[Path.MaxPath];
			int num = 0;
			uint num2 = 0U;
			uint num3 = 0U;
			bool flag = false;
			uint num4 = 0U;
			int num5 = -1;
			bool flag2 = false;
			bool flag3 = true;
			bool flag4 = false;
			if (path.Length > 0 && (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar))
			{
				array[num++] = '\\';
				i++;
				num5 = 0;
			}
			while (i < path.Length)
			{
				char c = path[i];
				if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
				{
					if (num4 == 0U)
					{
						if (num3 > 0U)
						{
							int num6 = num5 + 1;
							if (path[num6] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							if (num3 >= 2U)
							{
								if (flag2 && num3 > 2U)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
								}
								if (path[num6 + 1] == '.')
								{
									int num7 = num6 + 2;
									while ((long)num7 < (long)num6 + (long)((ulong)num3))
									{
										if (path[num7] != '.')
										{
											throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
										}
										num7++;
									}
									num3 = 2U;
								}
								else
								{
									if (num3 > 1U)
									{
										throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
									}
									num3 = 1U;
								}
							}
							if ((long)num + (long)((ulong)num3) + 1L >= (long)Path.MaxPath)
							{
								throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
							}
							if (num3 == 2U)
							{
								array[num++] = '.';
							}
							array[num++] = '.';
							flag = false;
						}
						if (num2 > 0U && flag3 && i + 1 < path.Length && (path[i + 1] == Path.DirectorySeparatorChar || path[i + 1] == Path.AltDirectorySeparatorChar))
						{
							array[num++] = Path.DirectorySeparatorChar;
						}
					}
					num3 = 0U;
					num2 = 0U;
					if (!flag)
					{
						flag = true;
						if (num + 1 >= Path.MaxPath)
						{
							throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
						}
						array[num++] = Path.DirectorySeparatorChar;
					}
					num4 = 0U;
					num5 = i;
					flag2 = false;
					flag3 = false;
					if (flag4)
					{
						array[num] = '\0';
						Path.TryExpandShortFileName(array, ref num, 260);
						flag4 = false;
					}
				}
				else if (c == '.')
				{
					num3 += 1U;
				}
				else if (c == ' ')
				{
					num2 += 1U;
				}
				else
				{
					if (c == '~')
					{
						flag4 = true;
					}
					flag = false;
					if (flag3 && c == Path.VolumeSeparatorChar)
					{
						char c2 = (i > 0) ? path[i - 1] : ' ';
						if (num3 != 0U || num4 < 1U || c2 == ' ')
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						flag2 = true;
						if (num4 > 1U)
						{
							uint num8 = 0U;
							while ((ulong)num8 < (ulong)((long)num) && array[(int)((UIntPtr)num8)] == ' ')
							{
								num8 += 1U;
							}
							if (num4 - num8 == 1U)
							{
								array[0] = c2;
								num = 1;
							}
						}
						num4 = 0U;
					}
					else
					{
						num4 += 1U + num3 + num2;
					}
					if (num3 > 0U || num2 > 0U)
					{
						int num9 = (num5 >= 0) ? (i - num5 - 1) : i;
						if (num9 > 0)
						{
							path.CopyTo(num5 + 1, array, num, num9);
							num += num9;
						}
						num3 = 0U;
						num2 = 0U;
					}
					if (num + 1 >= Path.MaxPath)
					{
						throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
					}
					array[num++] = c;
					num5 = i;
				}
				i++;
			}
			if (num4 == 0U && num3 > 0U)
			{
				int num10 = num5 + 1;
				if (path[num10] != '.')
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
				}
				if (num3 >= 2U)
				{
					if (flag2 && num3 > 2U)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
					}
					if (path[num10 + 1] == '.')
					{
						int num11 = num10 + 2;
						while ((long)num11 < (long)num10 + (long)((ulong)num3))
						{
							if (path[num11] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							num11++;
						}
						num3 = 2U;
					}
					else
					{
						if (num3 > 1U)
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						num3 = 1U;
					}
				}
				if ((long)num + (long)((ulong)num3) >= (long)Path.MaxPath)
				{
					throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
				}
				if (num3 == 2U)
				{
					array[num++] = '.';
				}
				array[num++] = '.';
			}
			if (num == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
			}
			array[num] = '\0';
			if (fullCheck && (Path.CharArrayStartsWithOrdinal(array, num, "http:", false) || Path.CharArrayStartsWithOrdinal(array, num, "file:", false)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathUriFormatNotSupported"));
			}
			if (flag4)
			{
				Path.TryExpandShortFileName(array, ref num, Path.MaxPath);
			}
			int num12 = 1;
			char[] array3;
			int num13;
			if (fullCheck)
			{
				char[] array2 = new char[Path.MaxPath + 1];
				num12 = Win32Native.GetFullPathName(array, Path.MaxPath + 1, array2, IntPtr.Zero);
				if (num12 > Path.MaxPath)
				{
					array2 = new char[num12];
					num12 = Win32Native.GetFullPathName(array, num12, array2, IntPtr.Zero);
					if (num12 > Path.MaxPath)
					{
						throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
					}
				}
				if (num12 == 0 && array[0] != '\0')
				{
					__Error.WinIOError();
				}
				else if (num12 < Path.MaxPath)
				{
					array2[num12] = '\0';
				}
				if (Environment.nativeIsWin9x())
				{
					for (int j = 0; j < 260; j++)
					{
						if (array2[j] == '\0')
						{
							num12 = j;
							break;
						}
					}
				}
				array3 = array2;
				num13 = num12;
				flag4 = false;
				uint num14 = 0U;
				while ((ulong)num14 < (ulong)((long)num13) && !flag4)
				{
					if (array2[(int)((UIntPtr)num14)] == '~')
					{
						flag4 = true;
					}
					num14 += 1U;
				}
				if (flag4 && !Path.TryExpandShortFileName(array2, ref num13, Path.MaxPath))
				{
					int num15 = Array.LastIndexOf<char>(array2, Path.DirectorySeparatorChar, num13 - 1, num13);
					if (num15 >= 0)
					{
						char[] array4 = new char[num13 - num15 - 1];
						Array.Copy(array2, num15 + 1, array4, 0, num13 - num15 - 1);
						array2[num15] = '\0';
						bool flag5 = Path.TryExpandShortFileName(array2, ref num15, Path.MaxPath);
						array2[num15] = Path.DirectorySeparatorChar;
						Array.Copy(array4, 0, array2, num15 + 1, array4.Length);
						if (flag5)
						{
							num13 = num15 + 1 + array4.Length;
						}
					}
				}
			}
			else
			{
				array3 = array;
				num13 = num;
			}
			if (num12 != 0 && array3[0] == '\\' && array3[1] == '\\')
			{
				int k;
				for (k = 2; k < num12; k++)
				{
					if (array3[k] == '\\')
					{
						k++;
						break;
					}
				}
				if (k == num12)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegalUNC"));
				}
				if (Path.CharArrayStartsWithOrdinal(array3, num13, "\\\\?\\globalroot", true))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathGlobalRoot"));
				}
			}
			if (num13 >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (num12 == 0)
			{
				int num16 = Marshal.GetLastWin32Error();
				if (num16 == 0)
				{
					num16 = 161;
				}
				__Error.WinIOError(num16, path);
				return null;
			}
			return new string(array3, 0, num13);
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000B7F5C File Offset: 0x000B6F5C
		private static bool CharArrayStartsWithOrdinal(char[] array, int numChars, string compareTo, bool ignoreCase)
		{
			if (numChars < compareTo.Length)
			{
				return false;
			}
			if (ignoreCase)
			{
				string value = new string(array, 0, compareTo.Length);
				return compareTo.Equals(value, StringComparison.OrdinalIgnoreCase);
			}
			for (int i = 0; i < compareTo.Length; i++)
			{
				if (array[i] != compareTo[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000B7FB0 File Offset: 0x000B6FB0
		private static bool TryExpandShortFileName(char[] buffer, ref int bufferLength, int maxBufferSize)
		{
			char[] array = new char[Path.MaxPath + 1];
			int num = Win32Native.GetLongPathName(buffer, array, Path.MaxPath);
			if (num >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (num == 0)
			{
				return false;
			}
			if (Environment.nativeIsWin9x())
			{
				for (int i = 0; i < 260; i++)
				{
					if (array[i] == '\0')
					{
						num = i;
						break;
					}
				}
			}
			Buffer.BlockCopy(array, 0, buffer, 0, 2 * num);
			bufferLength = num;
			buffer[bufferLength] = '\0';
			return true;
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000B8029 File Offset: 0x000B7029
		private unsafe static void SafeSetStackPointerValue(char* buffer, int index, char value)
		{
			if (index >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			buffer[index] = value;
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000B804C File Offset: 0x000B704C
		internal unsafe static string NormalizePathFast(string path, bool fullCheck)
		{
			if (fullCheck)
			{
				path = path.TrimEnd(new char[0]);
				Path.CheckInvalidPathChars(path);
			}
			int i = 0;
			char* ptr = stackalloc char[2 * Path.MaxPath];
			int num = 0;
			uint num2 = 0U;
			uint num3 = 0U;
			bool flag = false;
			uint num4 = 0U;
			int num5 = -1;
			bool flag2 = false;
			bool flag3 = true;
			bool flag4 = false;
			if (path.Length > 0 && (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar))
			{
				Path.SafeSetStackPointerValue(ptr, num++, '\\');
				i++;
				num5 = 0;
			}
			while (i < path.Length)
			{
				char c = path[i];
				if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
				{
					if (num4 == 0U)
					{
						if (num3 > 0U)
						{
							int num6 = num5 + 1;
							if (path[num6] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							if (num3 >= 2U)
							{
								if (flag2 && num3 > 2U)
								{
									throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
								}
								if (path[num6 + 1] == '.')
								{
									int num7 = num6 + 2;
									while ((long)num7 < (long)num6 + (long)((ulong)num3))
									{
										if (path[num7] != '.')
										{
											throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
										}
										num7++;
									}
									num3 = 2U;
								}
								else
								{
									if (num3 > 1U)
									{
										throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
									}
									num3 = 1U;
								}
							}
							if (num3 == 2U)
							{
								Path.SafeSetStackPointerValue(ptr, num++, '.');
							}
							Path.SafeSetStackPointerValue(ptr, num++, '.');
							flag = false;
						}
						if (num2 > 0U && flag3 && i + 1 < path.Length && (path[i + 1] == Path.DirectorySeparatorChar || path[i + 1] == Path.AltDirectorySeparatorChar))
						{
							Path.SafeSetStackPointerValue(ptr, num++, Path.DirectorySeparatorChar);
						}
					}
					num3 = 0U;
					num2 = 0U;
					if (!flag)
					{
						flag = true;
						Path.SafeSetStackPointerValue(ptr, num++, Path.DirectorySeparatorChar);
					}
					num4 = 0U;
					num5 = i;
					flag2 = false;
					flag3 = false;
					if (flag4)
					{
						Path.SafeSetStackPointerValue(ptr, num, '\0');
						Path.TryExpandShortFileName(ptr, ref num, 260);
						flag4 = false;
					}
				}
				else if (c == '.')
				{
					num3 += 1U;
				}
				else if (c == ' ')
				{
					num2 += 1U;
				}
				else
				{
					if (c == '~')
					{
						flag4 = true;
					}
					flag = false;
					if (flag3 && c == Path.VolumeSeparatorChar)
					{
						char c2 = (i > 0) ? path[i - 1] : ' ';
						if (num3 != 0U || num4 < 1U || c2 == ' ')
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						flag2 = true;
						if (num4 > 1U)
						{
							uint num8 = 0U;
							while ((ulong)num8 < (ulong)((long)num) && ptr[num8] == ' ')
							{
								num8 += 1U;
							}
							if (num4 - num8 == 1U)
							{
								*ptr = c2;
								num = 1;
							}
						}
						num4 = 0U;
					}
					else
					{
						num4 += 1U + num3 + num2;
					}
					if (num3 > 0U || num2 > 0U)
					{
						int num9 = (num5 >= 0) ? (i - num5 - 1) : i;
						if (num9 > 0)
						{
							for (int j = 0; j < num9; j++)
							{
								Path.SafeSetStackPointerValue(ptr, num++, path[num5 + 1 + j]);
							}
						}
						num3 = 0U;
						num2 = 0U;
					}
					Path.SafeSetStackPointerValue(ptr, num++, c);
					num5 = i;
				}
				i++;
			}
			if (num4 == 0U && num3 > 0U)
			{
				int num10 = num5 + 1;
				if (path[num10] != '.')
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
				}
				if (num3 >= 2U)
				{
					if (flag2 && num3 > 2U)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
					}
					if (path[num10 + 1] == '.')
					{
						int num11 = num10 + 2;
						while ((long)num11 < (long)num10 + (long)((ulong)num3))
						{
							if (path[num11] != '.')
							{
								throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
							}
							num11++;
						}
						num3 = 2U;
					}
					else
					{
						if (num3 > 1U)
						{
							throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
						}
						num3 = 1U;
					}
				}
				if (num3 == 2U)
				{
					Path.SafeSetStackPointerValue(ptr, num++, '.');
				}
				Path.SafeSetStackPointerValue(ptr, num++, '.');
			}
			if (num == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegal"));
			}
			Path.SafeSetStackPointerValue(ptr, num, '\0');
			if (fullCheck && (Path.CharArrayStartsWithOrdinal(ptr, num, "http:", false) || Path.CharArrayStartsWithOrdinal(ptr, num, "file:", false)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathUriFormatNotSupported"));
			}
			if (flag4)
			{
				Path.TryExpandShortFileName(ptr, ref num, Path.MaxPath);
			}
			int num12 = 1;
			char* ptr4;
			int num13;
			if (fullCheck)
			{
				char* ptr2 = stackalloc char[2 * (Path.MaxPath + 1)];
				num12 = Win32Native.GetFullPathName(ptr, Path.MaxPath + 1, ptr2, IntPtr.Zero);
				if (num12 > Path.MaxPath)
				{
					char* ptr3 = stackalloc char[2 * num12];
					ptr2 = ptr3;
					num12 = Win32Native.GetFullPathName(ptr, num12, ptr2, IntPtr.Zero);
				}
				if (num12 >= Path.MaxPath)
				{
					throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
				}
				if (num12 == 0 && *ptr != '\0')
				{
					__Error.WinIOError();
				}
				else if (num12 < Path.MaxPath)
				{
					ptr2[num12] = '\0';
				}
				ptr4 = ptr2;
				num13 = num12;
				flag4 = false;
				uint num14 = 0U;
				while ((ulong)num14 < (ulong)((long)num13) && !flag4)
				{
					if (ptr2[num14] == '~')
					{
						flag4 = true;
					}
					num14 += 1U;
				}
				if (flag4 && !Path.TryExpandShortFileName(ptr2, ref num13, Path.MaxPath))
				{
					int num15 = -1;
					for (int k = num13 - 1; k >= 0; k--)
					{
						if (ptr2[k] == Path.DirectorySeparatorChar)
						{
							num15 = k;
							break;
						}
					}
					if (num15 >= 0)
					{
						if (num13 >= Path.MaxPath)
						{
							throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
						}
						int num16 = num13 - num15 - 1;
						char* ptr5 = stackalloc char[2 * num16];
						Buffer.memcpy(ptr2, num15 + 1, ptr5, 0, num16);
						Path.SafeSetStackPointerValue(ptr2, num15, '\0');
						bool flag5 = Path.TryExpandShortFileName(ptr2, ref num15, Path.MaxPath);
						Path.SafeSetStackPointerValue(ptr2, num15, Path.DirectorySeparatorChar);
						if (num15 + 1 + num16 >= Path.MaxPath)
						{
							throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
						}
						Buffer.memcpy(ptr5, 0, ptr2, num15 + 1, num16);
						if (flag5)
						{
							num13 = num15 + 1 + num16;
						}
					}
				}
			}
			else
			{
				ptr4 = ptr;
				num13 = num;
			}
			if (num12 != 0 && *ptr4 == '\\' && ptr4[1] == '\\')
			{
				int l;
				for (l = 2; l < num12; l++)
				{
					if (ptr4[l] == '\\')
					{
						l++;
						break;
					}
				}
				if (l == num12)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathIllegalUNC"));
				}
				if (Path.CharArrayStartsWithOrdinal(ptr4, num13, "\\\\?\\globalroot", true))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_PathGlobalRoot"));
				}
			}
			if (num13 >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (num12 == 0)
			{
				int num17 = Marshal.GetLastWin32Error();
				if (num17 == 0)
				{
					num17 = 161;
				}
				__Error.WinIOError(num17, path);
				return null;
			}
			return new string(ptr4, 0, num13);
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000B871C File Offset: 0x000B771C
		private unsafe static bool CharArrayStartsWithOrdinal(char* array, int numChars, string compareTo, bool ignoreCase)
		{
			if (numChars < compareTo.Length)
			{
				return false;
			}
			if (ignoreCase)
			{
				string value = new string(array, 0, compareTo.Length);
				return compareTo.Equals(value, StringComparison.OrdinalIgnoreCase);
			}
			for (int i = 0; i < compareTo.Length; i++)
			{
				if (array[i] != compareTo[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x000B8774 File Offset: 0x000B7774
		private unsafe static bool TryExpandShortFileName(char* buffer, ref int bufferLength, int maxBufferSize)
		{
			char* ptr = stackalloc char[2 * (Path.MaxPath + 1)];
			int longPathName = Win32Native.GetLongPathName(buffer, ptr, Path.MaxPath);
			if (longPathName >= Path.MaxPath)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			if (longPathName == 0)
			{
				return false;
			}
			Buffer.memcpy(ptr, 0, buffer, 0, longPathName);
			bufferLength = longPathName;
			buffer[bufferLength] = '\0';
			return true;
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000B87D0 File Offset: 0x000B77D0
		internal static string FixupPath(string path)
		{
			return Path.NormalizePath(path, false);
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000B87E8 File Offset: 0x000B77E8
		public static string GetFileName(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				int length = path.Length;
				int num = length;
				while (--num >= 0)
				{
					char c = path[num];
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						return path.Substring(num + 1, length - num - 1);
					}
				}
			}
			return path;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000B8844 File Offset: 0x000B7844
		public static string GetFileNameWithoutExtension(string path)
		{
			path = Path.GetFileName(path);
			if (path == null)
			{
				return null;
			}
			int length;
			if ((length = path.LastIndexOf('.')) == -1)
			{
				return path;
			}
			return path.Substring(0, length);
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x000B8875 File Offset: 0x000B7875
		public static string GetPathRoot(string path)
		{
			if (path == null)
			{
				return null;
			}
			path = Path.FixupPath(path);
			return path.Substring(0, Path.GetRootLength(path));
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000B8894 File Offset: 0x000B7894
		public static string GetTempPath()
		{
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			StringBuilder stringBuilder = new StringBuilder(260);
			uint tempPath = Win32Native.GetTempPath(260, stringBuilder);
			string path = stringBuilder.ToString();
			if (tempPath == 0U)
			{
				__Error.WinIOError();
			}
			return Path.GetFullPathInternal(path);
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000B88DC File Offset: 0x000B78DC
		public static string GetRandomFileName()
		{
			byte[] array = new byte[10];
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			rngcryptoServiceProvider.GetBytes(array);
			char[] array2 = IsolatedStorage.ToBase32StringSuitableForDirName(array).ToCharArray();
			array2[8] = '.';
			return new string(array2, 0, 12);
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x000B8918 File Offset: 0x000B7918
		public static string GetTempFileName()
		{
			string tempPath = Path.GetTempPath();
			new FileIOPermission(FileIOPermissionAccess.Write, tempPath).Demand();
			StringBuilder stringBuilder = new StringBuilder(260);
			if (Win32Native.GetTempFileName(tempPath, "tmp", 0U, stringBuilder) == 0U)
			{
				__Error.WinIOError();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000B8960 File Offset: 0x000B7960
		public static bool HasExtension(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				int num = path.Length;
				while (--num >= 0)
				{
					char c = path[num];
					if (c == '.')
					{
						return num != path.Length - 1;
					}
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						break;
					}
				}
			}
			return false;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x000B89BC File Offset: 0x000B79BC
		public static bool IsPathRooted(string path)
		{
			if (path != null)
			{
				Path.CheckInvalidPathChars(path);
				int length = path.Length;
				if ((length >= 1 && (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar)) || (length >= 2 && path[1] == Path.VolumeSeparatorChar))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x000B8A10 File Offset: 0x000B7A10
		public static string Combine(string path1, string path2)
		{
			if (path1 == null || path2 == null)
			{
				throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
			}
			Path.CheckInvalidPathChars(path1);
			Path.CheckInvalidPathChars(path2);
			if (path2.Length == 0)
			{
				return path1;
			}
			if (path1.Length == 0)
			{
				return path2;
			}
			if (Path.IsPathRooted(path2))
			{
				return path2;
			}
			char c = path1[path1.Length - 1];
			if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar && c != Path.VolumeSeparatorChar)
			{
				return path1 + Path.DirectorySeparatorChar + path2;
			}
			return path1 + path2;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x000B8AA4 File Offset: 0x000B7AA4
		internal static void CheckSearchPattern(string searchPattern)
		{
			if ((Environment.OSInfo & Environment.OSName.Win9x) != Environment.OSName.Invalid && Path.CanPathCircumventSecurityNative(searchPattern))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidSearchPattern"));
			}
			int num;
			while ((num = searchPattern.IndexOf("..", StringComparison.Ordinal)) != -1)
			{
				if (num + 2 == searchPattern.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidSearchPattern"));
				}
				if (searchPattern[num + 2] == Path.DirectorySeparatorChar || searchPattern[num + 2] == Path.AltDirectorySeparatorChar)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidSearchPattern"));
				}
				searchPattern = searchPattern.Substring(num + 2);
			}
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x000B8B3C File Offset: 0x000B7B3C
		internal static void CheckInvalidPathChars(string path)
		{
			foreach (int num in path)
			{
				if (num == 34 || num == 60 || num == 62 || num == 124 || num < 32)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPathChars"));
				}
			}
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x000B8B8C File Offset: 0x000B7B8C
		internal static string InternalCombine(string path1, string path2)
		{
			if (path1 == null || path2 == null)
			{
				throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
			}
			Path.CheckInvalidPathChars(path1);
			Path.CheckInvalidPathChars(path2);
			if (path2.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathEmpty"), "path2");
			}
			if (Path.IsPathRooted(path2))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_Path2IsRooted"), "path2");
			}
			int length = path1.Length;
			if (length == 0)
			{
				return path2;
			}
			char c = path1[length - 1];
			if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar && c != Path.VolumeSeparatorChar)
			{
				return path1 + Path.DirectorySeparatorChar + path2;
			}
			return path1 + path2;
		}

		// Token: 0x060036A7 RID: 13991
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanPathCircumventSecurityNative(string partOfPath);

		// Token: 0x04001C95 RID: 7317
		internal const int MAX_PATH = 260;

		// Token: 0x04001C96 RID: 7318
		internal const int MAX_DIRECTORY_PATH = 248;

		// Token: 0x04001C97 RID: 7319
		public static readonly char DirectorySeparatorChar = '\\';

		// Token: 0x04001C98 RID: 7320
		public static readonly char AltDirectorySeparatorChar = '/';

		// Token: 0x04001C99 RID: 7321
		public static readonly char VolumeSeparatorChar = ':';

		// Token: 0x04001C9A RID: 7322
		[Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
		public static readonly char[] InvalidPathChars = new char[]
		{
			'"',
			'<',
			'>',
			'|',
			'\0',
			'\u0001',
			'\u0002',
			'\u0003',
			'\u0004',
			'\u0005',
			'\u0006',
			'\a',
			'\b',
			'\t',
			'\n',
			'\v',
			'\f',
			'\r',
			'\u000e',
			'\u000f',
			'\u0010',
			'\u0011',
			'\u0012',
			'\u0013',
			'\u0014',
			'\u0015',
			'\u0016',
			'\u0017',
			'\u0018',
			'\u0019',
			'\u001a',
			'\u001b',
			'\u001c',
			'\u001d',
			'\u001e',
			'\u001f'
		};

		// Token: 0x04001C9B RID: 7323
		private static readonly char[] RealInvalidPathChars = new char[]
		{
			'"',
			'<',
			'>',
			'|',
			'\0',
			'\u0001',
			'\u0002',
			'\u0003',
			'\u0004',
			'\u0005',
			'\u0006',
			'\a',
			'\b',
			'\t',
			'\n',
			'\v',
			'\f',
			'\r',
			'\u000e',
			'\u000f',
			'\u0010',
			'\u0011',
			'\u0012',
			'\u0013',
			'\u0014',
			'\u0015',
			'\u0016',
			'\u0017',
			'\u0018',
			'\u0019',
			'\u001a',
			'\u001b',
			'\u001c',
			'\u001d',
			'\u001e',
			'\u001f'
		};

		// Token: 0x04001C9C RID: 7324
		private static readonly char[] InvalidFileNameChars = new char[]
		{
			'"',
			'<',
			'>',
			'|',
			'\0',
			'\u0001',
			'\u0002',
			'\u0003',
			'\u0004',
			'\u0005',
			'\u0006',
			'\a',
			'\b',
			'\t',
			'\n',
			'\v',
			'\f',
			'\r',
			'\u000e',
			'\u000f',
			'\u0010',
			'\u0011',
			'\u0012',
			'\u0013',
			'\u0014',
			'\u0015',
			'\u0016',
			'\u0017',
			'\u0018',
			'\u0019',
			'\u001a',
			'\u001b',
			'\u001c',
			'\u001d',
			'\u001e',
			'\u001f',
			':',
			'*',
			'?',
			'\\',
			'/'
		};

		// Token: 0x04001C9D RID: 7325
		public static readonly char PathSeparator = ';';

		// Token: 0x04001C9E RID: 7326
		internal static readonly int MaxPath = 260;
	}
}
