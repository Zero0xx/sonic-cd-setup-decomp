using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms.VisualStyles
{
	// Token: 0x02000893 RID: 2195
	public static class VisualStyleInformation
	{
		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x06006CBB RID: 27835 RVA: 0x0018EE19 File Offset: 0x0018DE19
		public static bool IsSupportedByOS
		{
			get
			{
				return OSFeature.Feature.IsPresent(OSFeature.Themes);
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x06006CBC RID: 27836 RVA: 0x0018EE2A File Offset: 0x0018DE2A
		public static bool IsEnabledByUser
		{
			get
			{
				return VisualStyleInformation.IsSupportedByOS && SafeNativeMethods.IsAppThemed();
			}
		}

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x06006CBD RID: 27837 RVA: 0x0018EE3C File Offset: 0x0018DE3C
		internal static string ThemeFilename
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, null, 0, null, 0);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x06006CBE RID: 27838 RVA: 0x0018EE78 File Offset: 0x0018DE78
		public static string ColorScheme
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetCurrentThemeName(null, 0, stringBuilder, stringBuilder.Capacity, null, 0);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x06006CBF RID: 27839 RVA: 0x0018EEB4 File Offset: 0x0018DEB4
		public static string Size
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetCurrentThemeName(null, 0, null, 0, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x06006CC0 RID: 27840 RVA: 0x0018EEF0 File Offset: 0x0018DEF0
		public static string DisplayName
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.DisplayName, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x06006CC1 RID: 27841 RVA: 0x0018EF34 File Offset: 0x0018DF34
		public static string Company
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Company, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x06006CC2 RID: 27842 RVA: 0x0018EF78 File Offset: 0x0018DF78
		public static string Author
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Author, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x06006CC3 RID: 27843 RVA: 0x0018EFBC File Offset: 0x0018DFBC
		public static string Copyright
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Copyright, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x06006CC4 RID: 27844 RVA: 0x0018F000 File Offset: 0x0018E000
		public static string Url
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Url, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x06006CC5 RID: 27845 RVA: 0x0018F044 File Offset: 0x0018E044
		public static string Version
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Version, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x06006CC6 RID: 27846 RVA: 0x0018F088 File Offset: 0x0018E088
		public static string Description
		{
			get
			{
				if (VisualStyleInformation.IsEnabledByUser)
				{
					StringBuilder stringBuilder = new StringBuilder(512);
					SafeNativeMethods.GetThemeDocumentationProperty(VisualStyleInformation.ThemeFilename, VisualStyleDocProperty.Description, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x06006CC7 RID: 27847 RVA: 0x0018F0CC File Offset: 0x0018E0CC
		public static bool SupportsFlatMenus
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.Window.Caption.Active);
					}
					return SafeNativeMethods.GetThemeSysBool(new HandleRef(null, VisualStyleInformation.visualStyleRenderer.Handle), VisualStyleSystemProperty.SupportsFlatMenus);
				}
				return false;
			}
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06006CC8 RID: 27848 RVA: 0x0018F124 File Offset: 0x0018E124
		public static int MinimumColorDepth
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.Window.Caption.Active);
					}
					int result = 0;
					SafeNativeMethods.GetThemeSysInt(new HandleRef(null, VisualStyleInformation.visualStyleRenderer.Handle), VisualStyleSystemProperty.MinimumColorDepth, ref result);
					return result;
				}
				return 0;
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06006CC9 RID: 27849 RVA: 0x0018F184 File Offset: 0x0018E184
		public static Color TextControlBorder
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.TextBox.TextEdit.Normal);
					}
					return VisualStyleInformation.visualStyleRenderer.GetColor(ColorProperty.BorderColor);
				}
				return SystemColors.WindowFrame;
			}
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06006CCA RID: 27850 RVA: 0x0018F1D8 File Offset: 0x0018E1D8
		public static Color ControlHighlightHot
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (VisualStyleInformation.visualStyleRenderer == null)
					{
						VisualStyleInformation.visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
					}
					else
					{
						VisualStyleInformation.visualStyleRenderer.SetParameters(VisualStyleElement.Button.PushButton.Normal);
					}
					return VisualStyleInformation.visualStyleRenderer.GetColor(ColorProperty.AccentColorHint);
				}
				return SystemColors.ButtonHighlight;
			}
		}

		// Token: 0x040040B1 RID: 16561
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}
