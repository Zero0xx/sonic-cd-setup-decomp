using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Media;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200049F RID: 1183
	[Designer("System.Windows.Forms.Design.MaskedTextBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("MaskInputRejected")]
	[SRDescription("DescriptionMaskedTextBox")]
	[DefaultBindingProperty("Text")]
	[DefaultProperty("Mask")]
	public class MaskedTextBox : TextBoxBase
	{
		// Token: 0x06004694 RID: 18068 RVA: 0x000FFF7C File Offset: 0x000FEF7C
		public MaskedTextBox()
		{
			MaskedTextProvider maskedTextProvider = new MaskedTextProvider("<>", CultureInfo.CurrentCulture);
			this.flagState[MaskedTextBox.IS_NULL_MASK] = true;
			this.Initialize(maskedTextProvider);
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x000FFFB8 File Offset: 0x000FEFB8
		public MaskedTextBox(string mask)
		{
			if (mask == null)
			{
				throw new ArgumentNullException();
			}
			MaskedTextProvider maskedTextProvider = new MaskedTextProvider(mask, CultureInfo.CurrentCulture);
			this.flagState[MaskedTextBox.IS_NULL_MASK] = false;
			this.Initialize(maskedTextProvider);
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x000FFFF8 File Offset: 0x000FEFF8
		public MaskedTextBox(MaskedTextProvider maskedTextProvider)
		{
			if (maskedTextProvider == null)
			{
				throw new ArgumentNullException();
			}
			this.flagState[MaskedTextBox.IS_NULL_MASK] = false;
			this.Initialize(maskedTextProvider);
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x00100024 File Offset: 0x000FF024
		private void Initialize(MaskedTextProvider maskedTextProvider)
		{
			this.maskedTextProvider = maskedTextProvider;
			if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				this.SetWindowText();
			}
			this.passwordChar = this.maskedTextProvider.PasswordChar;
			this.insertMode = InsertKeyMode.Default;
			this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE] = false;
			this.flagState[MaskedTextBox.BEEP_ON_ERROR] = false;
			this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR] = false;
			this.flagState[MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE] = false;
			this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = this.maskedTextProvider.IncludePrompt;
			this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = this.maskedTextProvider.IncludeLiterals;
			this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = true;
			this.caretTestPos = 0;
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06004698 RID: 18072 RVA: 0x001000FA File Offset: 0x000FF0FA
		// (set) Token: 0x06004699 RID: 18073 RVA: 0x001000FD File Offset: 0x000FF0FD
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool AcceptsTab
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600469A RID: 18074 RVA: 0x001000FF File Offset: 0x000FF0FF
		// (set) Token: 0x0600469B RID: 18075 RVA: 0x0010010C File Offset: 0x000FF10C
		[DefaultValue(true)]
		[SRDescription("MaskedTextBoxAllowPromptAsInputDescr")]
		[SRCategory("CatBehavior")]
		public bool AllowPromptAsInput
		{
			get
			{
				return this.maskedTextProvider.AllowPromptAsInput;
			}
			set
			{
				if (value != this.maskedTextProvider.AllowPromptAsInput)
				{
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, this.maskedTextProvider.Culture, value, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		// Token: 0x1400027A RID: 634
		// (add) Token: 0x0600469C RID: 18076 RVA: 0x0010016C File Offset: 0x000FF16C
		// (remove) Token: 0x0600469D RID: 18077 RVA: 0x0010016E File Offset: 0x000FF16E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AcceptsTabChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x0600469E RID: 18078 RVA: 0x00100170 File Offset: 0x000FF170
		// (set) Token: 0x0600469F RID: 18079 RVA: 0x00100180 File Offset: 0x000FF180
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxAsciiOnlyDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public bool AsciiOnly
		{
			get
			{
				return this.maskedTextProvider.AsciiOnly;
			}
			set
			{
				if (value != this.maskedTextProvider.AsciiOnly)
				{
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, this.maskedTextProvider.Culture, this.maskedTextProvider.AllowPromptAsInput, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, value);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x001001E0 File Offset: 0x000FF1E0
		// (set) Token: 0x060046A1 RID: 18081 RVA: 0x001001F2 File Offset: 0x000FF1F2
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxBeepOnErrorDescr")]
		[DefaultValue(false)]
		public bool BeepOnError
		{
			get
			{
				return this.flagState[MaskedTextBox.BEEP_ON_ERROR];
			}
			set
			{
				this.flagState[MaskedTextBox.BEEP_ON_ERROR] = value;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x00100205 File Offset: 0x000FF205
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool CanUndo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x00100208 File Offset: 0x000FF208
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				HorizontalAlignment horizontalAlignment = base.RtlTranslateHorizontal(this.textAlign);
				createParams.ExStyle &= -4097;
				switch (horizontalAlignment)
				{
				case HorizontalAlignment.Left:
				{
					CreateParams createParams2 = createParams;
					createParams2.Style = createParams2.Style;
					break;
				}
				case HorizontalAlignment.Right:
					createParams.Style |= 2;
					break;
				case HorizontalAlignment.Center:
					createParams.Style |= 1;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x0010027E File Offset: 0x000FF27E
		// (set) Token: 0x060046A5 RID: 18085 RVA: 0x0010028C File Offset: 0x000FF28C
		[SRDescription("MaskedTextBoxCultureDescr")]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public CultureInfo Culture
		{
			get
			{
				return this.maskedTextProvider.Culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!this.maskedTextProvider.Culture.Equals(value))
				{
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, value, this.maskedTextProvider.AllowPromptAsInput, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x060046A6 RID: 18086 RVA: 0x001002FA File Offset: 0x000FF2FA
		// (set) Token: 0x060046A7 RID: 18087 RVA: 0x0010033C File Offset: 0x000FF33C
		[SRCategory("CatBehavior")]
		[DefaultValue(MaskFormat.IncludeLiterals)]
		[SRDescription("MaskedTextBoxCutCopyMaskFormat")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public MaskFormat CutCopyMaskFormat
		{
			get
			{
				if (this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT])
				{
					if (this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS])
					{
						return MaskFormat.IncludePromptAndLiterals;
					}
					return MaskFormat.IncludePrompt;
				}
				else
				{
					if (this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS])
					{
						return MaskFormat.IncludeLiterals;
					}
					return MaskFormat.ExcludePromptAndLiterals;
				}
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MaskFormat));
				}
				if (value == MaskFormat.IncludePrompt)
				{
					this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = true;
					this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = false;
					return;
				}
				if (value == MaskFormat.IncludeLiterals)
				{
					this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = false;
					this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = true;
					return;
				}
				bool value2 = value == MaskFormat.IncludePromptAndLiterals;
				this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = value2;
				this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = value2;
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x060046A8 RID: 18088 RVA: 0x001003E4 File Offset: 0x000FF3E4
		// (set) Token: 0x060046A9 RID: 18089 RVA: 0x001003EC File Offset: 0x000FF3EC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public IFormatProvider FormatProvider
		{
			get
			{
				return this.formatProvider;
			}
			set
			{
				this.formatProvider = value;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x060046AA RID: 18090 RVA: 0x001003F5 File Offset: 0x000FF3F5
		// (set) Token: 0x060046AB RID: 18091 RVA: 0x00100408 File Offset: 0x000FF408
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("MaskedTextBoxHidePromptOnLeaveDescr")]
		[DefaultValue(false)]
		public bool HidePromptOnLeave
		{
			get
			{
				return this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE];
			}
			set
			{
				if (this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE] != value)
				{
					this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE] = value;
					if (!this.flagState[MaskedTextBox.IS_NULL_MASK] && !this.Focused && !this.MaskFull && !base.DesignMode)
					{
						this.SetWindowText();
					}
				}
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x00100469 File Offset: 0x000FF469
		// (set) Token: 0x060046AD RID: 18093 RVA: 0x00100476 File Offset: 0x000FF476
		private bool IncludeLiterals
		{
			get
			{
				return this.maskedTextProvider.IncludeLiterals;
			}
			set
			{
				this.maskedTextProvider.IncludeLiterals = value;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x00100484 File Offset: 0x000FF484
		// (set) Token: 0x060046AF RID: 18095 RVA: 0x00100491 File Offset: 0x000FF491
		private bool IncludePrompt
		{
			get
			{
				return this.maskedTextProvider.IncludePrompt;
			}
			set
			{
				this.maskedTextProvider.IncludePrompt = value;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x0010049F File Offset: 0x000FF49F
		// (set) Token: 0x060046B1 RID: 18097 RVA: 0x001004A8 File Offset: 0x000FF4A8
		[DefaultValue(InsertKeyMode.Default)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxInsertKeyModeDescr")]
		public InsertKeyMode InsertKeyMode
		{
			get
			{
				return this.insertMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(InsertKeyMode));
				}
				if (this.insertMode != value)
				{
					bool isOverwriteMode = this.IsOverwriteMode;
					this.insertMode = value;
					if (isOverwriteMode != this.IsOverwriteMode)
					{
						this.OnIsOverwriteModeChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x00100506 File Offset: 0x000FF506
		protected override bool IsInputKey(Keys keyData)
		{
			return (keyData & Keys.KeyCode) != Keys.Return && base.IsInputKey(keyData);
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x060046B3 RID: 18099 RVA: 0x0010051C File Offset: 0x000FF51C
		[Browsable(false)]
		public bool IsOverwriteMode
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return false;
				}
				switch (this.insertMode)
				{
				case InsertKeyMode.Default:
					return this.flagState[MaskedTextBox.INSERT_TOGGLED];
				case InsertKeyMode.Insert:
					return false;
				case InsertKeyMode.Overwrite:
					return true;
				default:
					return false;
				}
			}
		}

		// Token: 0x1400027B RID: 635
		// (add) Token: 0x060046B4 RID: 18100 RVA: 0x0010056E File Offset: 0x000FF56E
		// (remove) Token: 0x060046B5 RID: 18101 RVA: 0x00100581 File Offset: 0x000FF581
		[SRDescription("MaskedTextBoxIsOverwriteModeChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler IsOverwriteModeChanged
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_ISOVERWRITEMODECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_ISOVERWRITEMODECHANGED, value);
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x060046B6 RID: 18102 RVA: 0x00100594 File Offset: 0x000FF594
		// (set) Token: 0x060046B7 RID: 18103 RVA: 0x001005E0 File Offset: 0x000FF5E0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string[] Lines
		{
			get
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
				string[] lines;
				try
				{
					lines = base.Lines;
				}
				finally
				{
					this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
				}
				return lines;
			}
			set
			{
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x060046B8 RID: 18104 RVA: 0x001005E2 File Offset: 0x000FF5E2
		// (set) Token: 0x060046B9 RID: 18105 RVA: 0x00100608 File Offset: 0x000FF608
		[Editor("System.Windows.Forms.Design.MaskPropertyEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxMaskDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue("")]
		[MergableProperty(false)]
		public string Mask
		{
			get
			{
				if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return this.maskedTextProvider.Mask;
				}
				return string.Empty;
			}
			set
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK] == string.IsNullOrEmpty(value) && (this.flagState[MaskedTextBox.IS_NULL_MASK] || value == this.maskedTextProvider.Mask))
				{
					return;
				}
				string textOnInitializingMask = null;
				string mask = value;
				if (string.IsNullOrEmpty(value))
				{
					string textOutput = this.TextOutput;
					string text = this.maskedTextProvider.ToString(false, false);
					this.flagState[MaskedTextBox.IS_NULL_MASK] = true;
					if (this.maskedTextProvider.IsPassword)
					{
						this.SetEditControlPasswordChar(this.maskedTextProvider.PasswordChar);
					}
					this.SetWindowText(text, false, false);
					EventArgs empty = EventArgs.Empty;
					this.OnMaskChanged(empty);
					if (text != textOutput)
					{
						this.OnTextChanged(empty);
					}
					mask = "<>";
				}
				else
				{
					for (int i = 0; i < value.Length; i++)
					{
						char c = value[i];
						if (!MaskedTextProvider.IsValidMaskChar(c))
						{
							throw new ArgumentException(SR.GetString("MaskedTextBoxMaskInvalidChar"));
						}
					}
					if (this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						textOnInitializingMask = this.Text;
					}
				}
				MaskedTextProvider newProvider = new MaskedTextProvider(mask, this.maskedTextProvider.Culture, this.maskedTextProvider.AllowPromptAsInput, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
				this.SetMaskedTextProvider(newProvider, textOnInitializingMask);
			}
		}

		// Token: 0x1400027C RID: 636
		// (add) Token: 0x060046BA RID: 18106 RVA: 0x00100772 File Offset: 0x000FF772
		// (remove) Token: 0x060046BB RID: 18107 RVA: 0x00100785 File Offset: 0x000FF785
		[SRDescription("MaskedTextBoxMaskChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler MaskChanged
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_MASKCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_MASKCHANGED, value);
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x060046BC RID: 18108 RVA: 0x00100798 File Offset: 0x000FF798
		[Browsable(false)]
		public bool MaskCompleted
		{
			get
			{
				return this.maskedTextProvider.MaskCompleted;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x060046BD RID: 18109 RVA: 0x001007A5 File Offset: 0x000FF7A5
		[Browsable(false)]
		public bool MaskFull
		{
			get
			{
				return this.maskedTextProvider.MaskFull;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x060046BE RID: 18110 RVA: 0x001007B2 File Offset: 0x000FF7B2
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MaskedTextProvider MaskedTextProvider
		{
			get
			{
				if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return (MaskedTextProvider)this.maskedTextProvider.Clone();
				}
				return null;
			}
		}

		// Token: 0x1400027D RID: 637
		// (add) Token: 0x060046BF RID: 18111 RVA: 0x001007D8 File Offset: 0x000FF7D8
		// (remove) Token: 0x060046C0 RID: 18112 RVA: 0x001007EB File Offset: 0x000FF7EB
		[SRDescription("MaskedTextBoxMaskInputRejectedDescr")]
		[SRCategory("CatBehavior")]
		public event MaskInputRejectedEventHandler MaskInputRejected
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_MASKINPUTREJECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_MASKINPUTREJECTED, value);
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x001007FE File Offset: 0x000FF7FE
		// (set) Token: 0x060046C2 RID: 18114 RVA: 0x00100806 File Offset: 0x000FF806
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override int MaxLength
		{
			get
			{
				return base.MaxLength;
			}
			set
			{
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x060046C3 RID: 18115 RVA: 0x00100808 File Offset: 0x000FF808
		// (set) Token: 0x060046C4 RID: 18116 RVA: 0x0010080B File Offset: 0x000FF80B
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Multiline
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1400027E RID: 638
		// (add) Token: 0x060046C5 RID: 18117 RVA: 0x0010080D File Offset: 0x000FF80D
		// (remove) Token: 0x060046C6 RID: 18118 RVA: 0x0010080F File Offset: 0x000FF80F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MultilineChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x00100811 File Offset: 0x000FF811
		// (set) Token: 0x060046C8 RID: 18120 RVA: 0x00100820 File Offset: 0x000FF820
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxPasswordCharDescr")]
		[DefaultValue('\0')]
		public char PasswordChar
		{
			get
			{
				return this.maskedTextProvider.PasswordChar;
			}
			set
			{
				if (!MaskedTextProvider.IsValidPasswordChar(value))
				{
					throw new ArgumentException(SR.GetString("MaskedTextBoxInvalidCharError"));
				}
				if (this.passwordChar != value)
				{
					if (value == this.maskedTextProvider.PromptChar)
					{
						throw new InvalidOperationException(SR.GetString("MaskedTextBoxPasswordAndPromptCharError"));
					}
					this.passwordChar = value;
					if (!this.UseSystemPasswordChar)
					{
						this.maskedTextProvider.PasswordChar = value;
						if (this.flagState[MaskedTextBox.IS_NULL_MASK])
						{
							this.SetEditControlPasswordChar(value);
						}
						else
						{
							this.SetWindowText();
						}
						base.VerifyImeRestrictedModeChanged();
					}
				}
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x060046C9 RID: 18121 RVA: 0x001008AE File Offset: 0x000FF8AE
		internal override bool PasswordProtect
		{
			get
			{
				if (this.maskedTextProvider != null)
				{
					return this.maskedTextProvider.IsPassword;
				}
				return base.PasswordProtect;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x001008CA File Offset: 0x000FF8CA
		// (set) Token: 0x060046CB RID: 18123 RVA: 0x001008D8 File Offset: 0x000FF8D8
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue('_')]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("MaskedTextBoxPromptCharDescr")]
		public char PromptChar
		{
			get
			{
				return this.maskedTextProvider.PromptChar;
			}
			set
			{
				if (!MaskedTextProvider.IsValidInputChar(value))
				{
					throw new ArgumentException(SR.GetString("MaskedTextBoxInvalidCharError"));
				}
				if (this.maskedTextProvider.PromptChar != value)
				{
					if (value == this.passwordChar || value == this.maskedTextProvider.PasswordChar)
					{
						throw new InvalidOperationException(SR.GetString("MaskedTextBoxPasswordAndPromptCharError"));
					}
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, this.maskedTextProvider.Culture, this.maskedTextProvider.AllowPromptAsInput, value, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x00100977 File Offset: 0x000FF977
		// (set) Token: 0x060046CD RID: 18125 RVA: 0x0010097F File Offset: 0x000FF97F
		public new bool ReadOnly
		{
			get
			{
				return base.ReadOnly;
			}
			set
			{
				if (this.ReadOnly != value)
				{
					base.ReadOnly = value;
					if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						this.SetWindowText();
					}
				}
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x060046CE RID: 18126 RVA: 0x001009A9 File Offset: 0x000FF9A9
		// (set) Token: 0x060046CF RID: 18127 RVA: 0x001009BB File Offset: 0x000FF9BB
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxRejectInputOnFirstFailureDescr")]
		public bool RejectInputOnFirstFailure
		{
			get
			{
				return this.flagState[MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE];
			}
			set
			{
				this.flagState[MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE] = value;
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x060046D0 RID: 18128 RVA: 0x001009CE File Offset: 0x000FF9CE
		// (set) Token: 0x060046D1 RID: 18129 RVA: 0x001009DB File Offset: 0x000FF9DB
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxResetOnPrompt")]
		public bool ResetOnPrompt
		{
			get
			{
				return this.maskedTextProvider.ResetOnPrompt;
			}
			set
			{
				this.maskedTextProvider.ResetOnPrompt = value;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x060046D2 RID: 18130 RVA: 0x001009E9 File Offset: 0x000FF9E9
		// (set) Token: 0x060046D3 RID: 18131 RVA: 0x001009F6 File Offset: 0x000FF9F6
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxResetOnSpace")]
		public bool ResetOnSpace
		{
			get
			{
				return this.maskedTextProvider.ResetOnSpace;
			}
			set
			{
				this.maskedTextProvider.ResetOnSpace = value;
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x060046D4 RID: 18132 RVA: 0x00100A04 File Offset: 0x000FFA04
		// (set) Token: 0x060046D5 RID: 18133 RVA: 0x00100A11 File Offset: 0x000FFA11
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("MaskedTextBoxSkipLiterals")]
		public bool SkipLiterals
		{
			get
			{
				return this.maskedTextProvider.SkipLiterals;
			}
			set
			{
				this.maskedTextProvider.SkipLiterals = value;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x060046D6 RID: 18134 RVA: 0x00100A1F File Offset: 0x000FFA1F
		// (set) Token: 0x060046D7 RID: 18135 RVA: 0x00100A40 File Offset: 0x000FFA40
		public override string SelectedText
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return base.SelectedText;
				}
				return this.GetSelectedText();
			}
			set
			{
				this.SetSelectedTextInternal(value, true);
			}
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x00100A4A File Offset: 0x000FFA4A
		internal override void SetSelectedTextInternal(string value, bool clearUndo)
		{
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				base.SetSelectedTextInternal(value, true);
				return;
			}
			this.PasteInt(value);
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x00100A6E File Offset: 0x000FFA6E
		private void ImeComplete()
		{
			this.flagState[MaskedTextBox.IME_COMPLETING] = true;
			this.ImeNotify(1);
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x00100A88 File Offset: 0x000FFA88
		private void ImeNotify(int action)
		{
			HandleRef hWnd = new HandleRef(this, base.Handle);
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(hWnd);
			if (intPtr != IntPtr.Zero)
			{
				try
				{
					UnsafeNativeMethods.ImmNotifyIME(new HandleRef(null, intPtr), 21, action, 0);
				}
				finally
				{
					UnsafeNativeMethods.ImmReleaseContext(hWnd, new HandleRef(null, intPtr));
				}
			}
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00100AEC File Offset: 0x000FFAEC
		private void SetEditControlPasswordChar(char pwdChar)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(204, (int)pwdChar, 0);
				base.Invalidate();
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x060046DC RID: 18140 RVA: 0x00100B0C File Offset: 0x000FFB0C
		private char SystemPasswordChar
		{
			get
			{
				if (MaskedTextBox.systemPwdChar == '\0')
				{
					TextBox textBox = new TextBox();
					textBox.UseSystemPasswordChar = true;
					MaskedTextBox.systemPwdChar = textBox.PasswordChar;
					textBox.Dispose();
				}
				return MaskedTextBox.systemPwdChar;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x060046DD RID: 18141 RVA: 0x00100B43 File Offset: 0x000FFB43
		// (set) Token: 0x060046DE RID: 18142 RVA: 0x00100B78 File Offset: 0x000FFB78
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Bindable(true)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.MaskedTextBoxTextEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		public override string Text
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK] || this.flagState[MaskedTextBox.QUERY_BASE_TEXT])
				{
					return base.Text;
				}
				return this.TextOutput;
			}
			set
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					base.Text = value;
					return;
				}
				if (string.IsNullOrEmpty(value))
				{
					this.Delete(Keys.Delete, 0, this.maskedTextProvider.Length);
					return;
				}
				if (!this.RejectInputOnFirstFailure)
				{
					this.Replace(value, 0, this.maskedTextProvider.Length);
					return;
				}
				string textOutput = this.TextOutput;
				MaskedTextResultHint rejectionHint;
				if (this.maskedTextProvider.Set(value, out this.caretTestPos, out rejectionHint))
				{
					if (this.TextOutput != textOutput)
					{
						this.SetText();
					}
					base.SelectionStart = ++this.caretTestPos;
					return;
				}
				this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, rejectionHint));
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x060046DF RID: 18143 RVA: 0x00100C33 File Offset: 0x000FFC33
		[Browsable(false)]
		public override int TextLength
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return base.TextLength;
				}
				return this.GetFormattedDisplayString().Length;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x060046E0 RID: 18144 RVA: 0x00100C59 File Offset: 0x000FFC59
		private string TextOutput
		{
			get
			{
				return this.maskedTextProvider.ToString();
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060046E1 RID: 18145 RVA: 0x00100C66 File Offset: 0x000FFC66
		// (set) Token: 0x060046E2 RID: 18146 RVA: 0x00100C70 File Offset: 0x000FFC70
		[DefaultValue(HorizontalAlignment.Left)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("TextBoxTextAlignDescr")]
		public HorizontalAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (this.textAlign != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
					}
					this.textAlign = value;
					base.RecreateHandle();
					this.OnTextAlignChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1400027F RID: 639
		// (add) Token: 0x060046E3 RID: 18147 RVA: 0x00100CC4 File Offset: 0x000FFCC4
		// (remove) Token: 0x060046E4 RID: 18148 RVA: 0x00100CD7 File Offset: 0x000FFCD7
		[SRCategory("CatPropertyChanged")]
		[SRDescription("RadioButtonOnTextAlignChangedDescr")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_TEXTALIGNCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_TEXTALIGNCHANGED, value);
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060046E5 RID: 18149 RVA: 0x00100CEA File Offset: 0x000FFCEA
		// (set) Token: 0x060046E6 RID: 18150 RVA: 0x00100D0C File Offset: 0x000FFD0C
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(MaskFormat.IncludeLiterals)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxTextMaskFormat")]
		public MaskFormat TextMaskFormat
		{
			get
			{
				if (this.IncludePrompt)
				{
					if (this.IncludeLiterals)
					{
						return MaskFormat.IncludePromptAndLiterals;
					}
					return MaskFormat.IncludePrompt;
				}
				else
				{
					if (this.IncludeLiterals)
					{
						return MaskFormat.IncludeLiterals;
					}
					return MaskFormat.ExcludePromptAndLiterals;
				}
			}
			set
			{
				if (this.TextMaskFormat == value)
				{
					return;
				}
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MaskFormat));
				}
				string text = this.flagState[MaskedTextBox.IS_NULL_MASK] ? null : this.TextOutput;
				if (value == MaskFormat.IncludePrompt)
				{
					this.IncludePrompt = true;
					this.IncludeLiterals = false;
				}
				else if (value == MaskFormat.IncludeLiterals)
				{
					this.IncludePrompt = false;
					this.IncludeLiterals = true;
				}
				else
				{
					bool flag = value == MaskFormat.IncludePromptAndLiterals;
					this.IncludePrompt = flag;
					this.IncludeLiterals = flag;
				}
				if (text != null && text != this.TextOutput)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x00100DBC File Offset: 0x000FFDBC
		public override string ToString()
		{
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return base.ToString();
			}
			bool includePrompt = this.IncludePrompt;
			bool includeLiterals = this.IncludeLiterals;
			string result;
			try
			{
				this.IncludePrompt = (this.IncludeLiterals = true);
				result = base.ToString();
			}
			finally
			{
				this.IncludePrompt = includePrompt;
				this.IncludeLiterals = includeLiterals;
			}
			return result;
		}

		// Token: 0x14000280 RID: 640
		// (add) Token: 0x060046E8 RID: 18152 RVA: 0x00100E2C File Offset: 0x000FFE2C
		// (remove) Token: 0x060046E9 RID: 18153 RVA: 0x00100E3F File Offset: 0x000FFE3F
		[SRCategory("CatFocus")]
		[SRDescription("MaskedTextBoxTypeValidationCompletedDescr")]
		public event TypeValidationEventHandler TypeValidationCompleted
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_VALIDATIONCOMPLETED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_VALIDATIONCOMPLETED, value);
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x060046EA RID: 18154 RVA: 0x00100E52 File Offset: 0x000FFE52
		// (set) Token: 0x060046EB RID: 18155 RVA: 0x00100E64 File Offset: 0x000FFE64
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxUseSystemPasswordCharDescr")]
		[DefaultValue(false)]
		public bool UseSystemPasswordChar
		{
			get
			{
				return this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR];
			}
			set
			{
				if (value != this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR])
				{
					if (value)
					{
						if (this.SystemPasswordChar == this.PromptChar)
						{
							throw new InvalidOperationException(SR.GetString("MaskedTextBoxPasswordAndPromptCharError"));
						}
						this.maskedTextProvider.PasswordChar = this.SystemPasswordChar;
					}
					else
					{
						this.maskedTextProvider.PasswordChar = this.passwordChar;
					}
					this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR] = value;
					if (this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						this.SetEditControlPasswordChar(this.maskedTextProvider.PasswordChar);
					}
					else
					{
						this.SetWindowText();
					}
					base.VerifyImeRestrictedModeChanged();
				}
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x060046EC RID: 18156 RVA: 0x00100F0E File Offset: 0x000FFF0E
		// (set) Token: 0x060046ED RID: 18157 RVA: 0x00100F16 File Offset: 0x000FFF16
		[DefaultValue(null)]
		[Browsable(false)]
		public Type ValidatingType
		{
			get
			{
				return this.validatingType;
			}
			set
			{
				if (this.validatingType != value)
				{
					this.validatingType = value;
				}
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x060046EE RID: 18158 RVA: 0x00100F28 File Offset: 0x000FFF28
		// (set) Token: 0x060046EF RID: 18159 RVA: 0x00100F2B File Offset: 0x000FFF2B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool WordWrap
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x00100F2D File Offset: 0x000FFF2D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void ClearUndo()
		{
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x00100F2F File Offset: 0x000FFF2F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected override void CreateHandle()
		{
			if (!this.flagState[MaskedTextBox.IS_NULL_MASK] && base.RecreatingHandle)
			{
				this.SetWindowText(this.GetFormattedDisplayString(), false, false);
			}
			base.CreateHandle();
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00100F60 File Offset: 0x000FFF60
		private void Delete(Keys keyCode, int startPosition, int selectionLen)
		{
			this.caretTestPos = startPosition;
			if (selectionLen == 0)
			{
				if (keyCode == Keys.Back)
				{
					if (startPosition == 0)
					{
						return;
					}
					startPosition--;
				}
				else if (startPosition + selectionLen == this.maskedTextProvider.Length)
				{
					return;
				}
			}
			int endPosition = (selectionLen > 0) ? (startPosition + selectionLen - 1) : startPosition;
			string textOutput = this.TextOutput;
			int position;
			MaskedTextResultHint maskedTextResultHint;
			if (this.maskedTextProvider.RemoveAt(startPosition, endPosition, out position, out maskedTextResultHint))
			{
				if (this.TextOutput != textOutput)
				{
					this.SetText();
					this.caretTestPos = startPosition;
				}
				else if (selectionLen > 0)
				{
					this.caretTestPos = startPosition;
				}
				else if (maskedTextResultHint == MaskedTextResultHint.NoEffect)
				{
					if (keyCode == Keys.Delete)
					{
						this.caretTestPos = this.maskedTextProvider.FindEditPositionFrom(startPosition, true);
					}
					else
					{
						if (this.maskedTextProvider.FindAssignedEditPositionFrom(startPosition, true) == MaskedTextProvider.InvalidIndex)
						{
							this.caretTestPos = this.maskedTextProvider.FindAssignedEditPositionFrom(startPosition, false);
						}
						else
						{
							this.caretTestPos = this.maskedTextProvider.FindEditPositionFrom(startPosition, false);
						}
						if (this.caretTestPos != MaskedTextProvider.InvalidIndex)
						{
							this.caretTestPos++;
						}
					}
					if (this.caretTestPos == MaskedTextProvider.InvalidIndex)
					{
						this.caretTestPos = startPosition;
					}
				}
				else if (keyCode == Keys.Back)
				{
					this.caretTestPos = startPosition;
				}
			}
			else
			{
				this.OnMaskInputRejected(new MaskInputRejectedEventArgs(position, maskedTextResultHint));
			}
			base.SelectInternal(this.caretTestPos, 0, this.maskedTextProvider.Length);
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x001010B4 File Offset: 0x001000B4
		public override char GetCharFromPosition(Point pt)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			char charFromPosition;
			try
			{
				charFromPosition = base.GetCharFromPosition(pt);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return charFromPosition;
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x00101100 File Offset: 0x00100100
		public override int GetCharIndexFromPosition(Point pt)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			int charIndexFromPosition;
			try
			{
				charIndexFromPosition = base.GetCharIndexFromPosition(pt);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return charIndexFromPosition;
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x0010114C File Offset: 0x0010014C
		internal override int GetEndPosition()
		{
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return base.GetEndPosition();
			}
			int num = this.maskedTextProvider.FindEditPositionFrom(this.maskedTextProvider.LastAssignedPosition + 1, true);
			if (num == MaskedTextProvider.InvalidIndex)
			{
				num = this.maskedTextProvider.LastAssignedPosition + 1;
			}
			return num;
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x001011A3 File Offset: 0x001001A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int GetFirstCharIndexOfCurrentLine()
		{
			return 0;
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x001011A6 File Offset: 0x001001A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int GetFirstCharIndexFromLine(int lineNumber)
		{
			return 0;
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x001011AC File Offset: 0x001001AC
		private string GetFormattedDisplayString()
		{
			bool includePrompt = !this.ReadOnly && (base.DesignMode || !this.HidePromptOnLeave || this.Focused);
			return this.maskedTextProvider.ToString(false, includePrompt, true, 0, this.maskedTextProvider.Length);
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x001011FD File Offset: 0x001001FD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetLineFromCharIndex(int index)
		{
			return 0;
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x00101200 File Offset: 0x00100200
		public override Point GetPositionFromCharIndex(int index)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			Point positionFromCharIndex;
			try
			{
				positionFromCharIndex = base.GetPositionFromCharIndex(index);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return positionFromCharIndex;
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x0010124C File Offset: 0x0010024C
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			Size preferredSizeCore;
			try
			{
				preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return preferredSizeCore;
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x00101298 File Offset: 0x00100298
		private string GetSelectedText()
		{
			int startPosition;
			int num;
			base.GetSelectionStartAndLength(out startPosition, out num);
			if (num == 0)
			{
				return string.Empty;
			}
			bool includePrompt = (this.CutCopyMaskFormat & MaskFormat.IncludePrompt) != MaskFormat.ExcludePromptAndLiterals;
			bool includeLiterals = (this.CutCopyMaskFormat & MaskFormat.IncludeLiterals) != MaskFormat.ExcludePromptAndLiterals;
			return this.maskedTextProvider.ToString(true, includePrompt, includeLiterals, startPosition, num);
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x001012E6 File Offset: 0x001002E6
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (Application.RenderWithVisualStyles && base.IsHandleCreated && base.BorderStyle == BorderStyle.Fixed3D)
			{
				SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1025);
			}
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x00101324 File Offset: 0x00100324
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SetSelectionOnHandle();
			if (this.flagState[MaskedTextBox.IS_NULL_MASK] && this.maskedTextProvider.IsPassword)
			{
				this.SetEditControlPasswordChar(this.maskedTextProvider.PasswordChar);
			}
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00101364 File Offset: 0x00100364
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnIsOverwriteModeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[MaskedTextBox.EVENT_ISOVERWRITEMODECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00101394 File Offset: 0x00100394
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return;
			}
			Keys keys = e.KeyCode;
			if (keys == Keys.Return || keys == Keys.Escape)
			{
				this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = false;
			}
			if (keys == Keys.Insert && e.Modifiers == Keys.None && this.insertMode == InsertKeyMode.Default)
			{
				this.flagState[MaskedTextBox.INSERT_TOGGLED] = !this.flagState[MaskedTextBox.INSERT_TOGGLED];
				this.OnIsOverwriteModeChanged(EventArgs.Empty);
				return;
			}
			if (e.Control && char.IsLetter((char)keys))
			{
				Keys keys2 = keys;
				if (keys2 != Keys.H)
				{
					this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = false;
					return;
				}
				keys = Keys.Back;
			}
			if ((keys == Keys.Delete || keys == Keys.Back) && !this.ReadOnly)
			{
				int num;
				int num2;
				base.GetSelectionStartAndLength(out num, out num2);
				Keys modifiers = e.Modifiers;
				if (modifiers != Keys.Shift)
				{
					if (modifiers == Keys.Control)
					{
						if (num2 == 0)
						{
							if (keys == Keys.Delete)
							{
								num2 = this.maskedTextProvider.Length - num;
							}
							else
							{
								num2 = ((num == this.maskedTextProvider.Length) ? num : (num + 1));
								num = 0;
							}
						}
					}
				}
				else if (keys == Keys.Delete)
				{
					keys = Keys.Back;
				}
				if (!this.flagState[MaskedTextBox.HANDLE_KEY_PRESS])
				{
					this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = true;
				}
				this.Delete(keys, num, num2);
				e.SuppressKeyPress = true;
			}
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x001014F8 File Offset: 0x001004F8
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return;
			}
			if (!this.flagState[MaskedTextBox.HANDLE_KEY_PRESS])
			{
				this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = true;
				if (!char.IsLetter(e.KeyChar))
				{
					return;
				}
			}
			if (!this.ReadOnly)
			{
				int startPosition;
				int num;
				base.GetSelectionStartAndLength(out startPosition, out num);
				string textOutput = this.TextOutput;
				MaskedTextResultHint rejectionHint;
				if (this.PlaceChar(e.KeyChar, startPosition, num, this.IsOverwriteMode, out rejectionHint))
				{
					if (this.TextOutput != textOutput)
					{
						this.SetText();
					}
					base.SelectionStart = ++this.caretTestPos;
					if (ImeModeConversion.InputLanguageTable == ImeModeConversion.KoreanTable)
					{
						int num2 = this.maskedTextProvider.FindUnassignedEditPositionFrom(this.caretTestPos, true);
						if (num2 == MaskedTextProvider.InvalidIndex)
						{
							this.ImeComplete();
						}
					}
				}
				else
				{
					this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, rejectionHint));
				}
				if (num > 0)
				{
					this.SelectionLength = 0;
				}
				e.Handled = true;
			}
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x00101608 File Offset: 0x00100608
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.flagState[MaskedTextBox.IME_COMPLETING])
			{
				this.flagState[MaskedTextBox.IME_COMPLETING] = false;
			}
			if (this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION])
			{
				this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION] = false;
			}
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x00101664 File Offset: 0x00100664
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMaskChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[MaskedTextBox.EVENT_MASKCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x00101694 File Offset: 0x00100694
		private void OnMaskInputRejected(MaskInputRejectedEventArgs e)
		{
			if (this.BeepOnError)
			{
				SoundPlayer soundPlayer = new SoundPlayer();
				soundPlayer.Play();
			}
			MaskInputRejectedEventHandler maskInputRejectedEventHandler = base.Events[MaskedTextBox.EVENT_MASKINPUTREJECTED] as MaskInputRejectedEventHandler;
			if (maskInputRejectedEventHandler != null)
			{
				maskInputRejectedEventHandler(this, e);
			}
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x001016D6 File Offset: 0x001006D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void OnMultilineChanged(EventArgs e)
		{
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x001016D8 File Offset: 0x001006D8
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[MaskedTextBox.EVENT_TEXTALIGNCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x00101708 File Offset: 0x00100708
		private void OnTypeValidationCompleted(TypeValidationEventArgs e)
		{
			TypeValidationEventHandler typeValidationEventHandler = base.Events[MaskedTextBox.EVENT_VALIDATIONCOMPLETED] as TypeValidationEventHandler;
			if (typeValidationEventHandler != null)
			{
				typeValidationEventHandler(this, e);
			}
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x00101736 File Offset: 0x00100736
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnValidating(CancelEventArgs e)
		{
			this.PerformTypeValidation(e);
			base.OnValidating(e);
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00101748 File Offset: 0x00100748
		protected override void OnTextChanged(EventArgs e)
		{
			bool value = this.flagState[MaskedTextBox.QUERY_BASE_TEXT];
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			try
			{
				base.OnTextChanged(e);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = value;
			}
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x001017A4 File Offset: 0x001007A4
		private void Replace(string text, int startPosition, int selectionLen)
		{
			MaskedTextProvider maskedTextProvider = (MaskedTextProvider)this.maskedTextProvider.Clone();
			int num = this.caretTestPos;
			MaskedTextResultHint maskedTextResultHint = MaskedTextResultHint.NoEffect;
			int num2 = startPosition + selectionLen - 1;
			if (this.RejectInputOnFirstFailure)
			{
				if (!((startPosition > num2) ? maskedTextProvider.InsertAt(text, startPosition, out this.caretTestPos, out maskedTextResultHint) : maskedTextProvider.Replace(text, startPosition, num2, out this.caretTestPos, out maskedTextResultHint)))
				{
					this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, maskedTextResultHint));
				}
			}
			else
			{
				MaskedTextResultHint maskedTextResultHint2 = maskedTextResultHint;
				int i = 0;
				while (i < text.Length)
				{
					char c = text[i];
					if (this.maskedTextProvider.VerifyEscapeChar(c, startPosition))
					{
						goto IL_BF;
					}
					int num3 = maskedTextProvider.FindEditPositionFrom(startPosition, true);
					if (num3 != MaskedTextProvider.InvalidIndex)
					{
						startPosition = num3;
						goto IL_BF;
					}
					this.OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition, MaskedTextResultHint.UnavailableEditPosition));
					IL_109:
					i++;
					continue;
					IL_BF:
					int num4 = (num2 >= startPosition) ? 1 : 0;
					bool overwrite = num4 > 0;
					if (!this.PlaceChar(maskedTextProvider, c, startPosition, num4, overwrite, out maskedTextResultHint2))
					{
						this.OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition, maskedTextResultHint2));
						goto IL_109;
					}
					startPosition = this.caretTestPos + 1;
					if (maskedTextResultHint2 == MaskedTextResultHint.Success && maskedTextResultHint != maskedTextResultHint2)
					{
						maskedTextResultHint = maskedTextResultHint2;
						goto IL_109;
					}
					goto IL_109;
				}
				if (selectionLen > 0 && startPosition <= num2)
				{
					if (!maskedTextProvider.RemoveAt(startPosition, num2, out this.caretTestPos, out maskedTextResultHint2))
					{
						this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, maskedTextResultHint2));
					}
					if (maskedTextResultHint == MaskedTextResultHint.NoEffect && maskedTextResultHint != maskedTextResultHint2)
					{
						maskedTextResultHint = maskedTextResultHint2;
					}
				}
			}
			bool flag = this.TextOutput != maskedTextProvider.ToString();
			this.maskedTextProvider = maskedTextProvider;
			if (flag)
			{
				this.SetText();
				this.caretTestPos = startPosition;
				base.SelectInternal(this.caretTestPos, 0, this.maskedTextProvider.Length);
				return;
			}
			this.caretTestPos = num;
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00101954 File Offset: 0x00100954
		private void PasteInt(string text)
		{
			int startPosition;
			int selectionLen;
			base.GetSelectionStartAndLength(out startPosition, out selectionLen);
			if (string.IsNullOrEmpty(text))
			{
				this.Delete(Keys.Delete, startPosition, selectionLen);
				return;
			}
			this.Replace(text, startPosition, selectionLen);
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x00101988 File Offset: 0x00100988
		private object PerformTypeValidation(CancelEventArgs e)
		{
			object obj = null;
			if (this.validatingType != null)
			{
				string text = null;
				if (!this.flagState[MaskedTextBox.IS_NULL_MASK] && !this.maskedTextProvider.MaskCompleted)
				{
					text = SR.GetString("MaskedTextBoxIncompleteMsg");
				}
				else
				{
					string value;
					if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						value = this.maskedTextProvider.ToString(false, this.IncludeLiterals);
					}
					else
					{
						value = base.Text;
					}
					try
					{
						obj = Formatter.ParseObject(value, this.validatingType, typeof(string), null, null, this.formatProvider, null, Formatter.GetDefaultDataSourceNullValue(this.validatingType));
					}
					catch (Exception innerException)
					{
						if (ClientUtils.IsSecurityOrCriticalException(innerException))
						{
							throw;
						}
						if (innerException.InnerException != null)
						{
							innerException = innerException.InnerException;
						}
						text = innerException.GetType().ToString() + ": " + innerException.Message;
					}
				}
				bool isValidInput = false;
				if (text == null)
				{
					isValidInput = true;
					text = SR.GetString("MaskedTextBoxTypeValidationSucceeded");
				}
				TypeValidationEventArgs typeValidationEventArgs = new TypeValidationEventArgs(this.validatingType, isValidInput, obj, text);
				this.OnTypeValidationCompleted(typeValidationEventArgs);
				if (e != null)
				{
					e.Cancel = typeValidationEventArgs.Cancel;
				}
			}
			return obj;
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x00101AB4 File Offset: 0x00100AB4
		private bool PlaceChar(char ch, int startPosition, int length, bool overwrite, out MaskedTextResultHint hint)
		{
			return this.PlaceChar(this.maskedTextProvider, ch, startPosition, length, overwrite, out hint);
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x00101ACC File Offset: 0x00100ACC
		private bool PlaceChar(MaskedTextProvider provider, char ch, int startPosition, int length, bool overwrite, out MaskedTextResultHint hint)
		{
			this.caretTestPos = startPosition;
			if (startPosition >= this.maskedTextProvider.Length)
			{
				hint = MaskedTextResultHint.UnavailableEditPosition;
				return false;
			}
			if (length > 0)
			{
				int endPosition = startPosition + length - 1;
				return provider.Replace(ch, startPosition, endPosition, out this.caretTestPos, out hint);
			}
			if (overwrite)
			{
				return provider.Replace(ch, startPosition, out this.caretTestPos, out hint);
			}
			return provider.InsertAt(ch, startPosition, out this.caretTestPos, out hint);
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00101B38 File Offset: 0x00100B38
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool flag = base.ProcessCmdKey(ref msg, keyData);
			if (!flag && keyData == (Keys)131137)
			{
				base.SelectAll();
				flag = true;
			}
			return flag;
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x00101B64 File Offset: 0x00100B64
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessKeyMessage(ref Message m)
		{
			bool flag = base.ProcessKeyMessage(ref m);
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return flag;
			}
			return (m.Msg == 258 && base.ImeWmCharsToIgnore > 0) || flag;
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x00101BA6 File Offset: 0x00100BA6
		private void ResetCulture()
		{
			this.Culture = CultureInfo.CurrentCulture;
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00101BB3 File Offset: 0x00100BB3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void ScrollToCaret()
		{
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x00101BB5 File Offset: 0x00100BB5
		private void SetMaskedTextProvider(MaskedTextProvider newProvider)
		{
			this.SetMaskedTextProvider(newProvider, null);
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x00101BC0 File Offset: 0x00100BC0
		private void SetMaskedTextProvider(MaskedTextProvider newProvider, string textOnInitializingMask)
		{
			newProvider.IncludePrompt = this.maskedTextProvider.IncludePrompt;
			newProvider.IncludeLiterals = this.maskedTextProvider.IncludeLiterals;
			newProvider.SkipLiterals = this.maskedTextProvider.SkipLiterals;
			newProvider.ResetOnPrompt = this.maskedTextProvider.ResetOnPrompt;
			newProvider.ResetOnSpace = this.maskedTextProvider.ResetOnSpace;
			if (this.flagState[MaskedTextBox.IS_NULL_MASK] && textOnInitializingMask == null)
			{
				this.maskedTextProvider = newProvider;
				return;
			}
			int position = 0;
			MaskedTextResultHint maskedTextResultHint = MaskedTextResultHint.NoEffect;
			MaskedTextProvider maskedTextProvider = this.maskedTextProvider;
			bool flag = maskedTextProvider.Mask == newProvider.Mask;
			string a;
			bool flag2;
			if (textOnInitializingMask != null)
			{
				a = textOnInitializingMask;
				flag2 = !newProvider.Set(textOnInitializingMask, out position, out maskedTextResultHint);
			}
			else
			{
				a = this.TextOutput;
				int i = maskedTextProvider.AssignedEditPositionCount;
				int num = 0;
				int num2 = 0;
				while (i > 0)
				{
					num = maskedTextProvider.FindAssignedEditPositionFrom(num, true);
					if (flag)
					{
						num2 = num;
					}
					else
					{
						num2 = newProvider.FindEditPositionFrom(num2, true);
						if (num2 == MaskedTextProvider.InvalidIndex)
						{
							newProvider.Clear();
							position = newProvider.Length;
							maskedTextResultHint = MaskedTextResultHint.UnavailableEditPosition;
							break;
						}
					}
					if (!newProvider.Replace(maskedTextProvider[num], num2, out position, out maskedTextResultHint))
					{
						flag = false;
						newProvider.Clear();
						break;
					}
					num++;
					num2++;
					i--;
				}
				flag2 = !MaskedTextProvider.GetOperationResultFromHint(maskedTextResultHint);
			}
			this.maskedTextProvider = newProvider;
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				this.flagState[MaskedTextBox.IS_NULL_MASK] = false;
			}
			if (flag2)
			{
				this.OnMaskInputRejected(new MaskInputRejectedEventArgs(position, maskedTextResultHint));
			}
			if (newProvider.IsPassword)
			{
				this.SetEditControlPasswordChar('\0');
			}
			EventArgs empty = EventArgs.Empty;
			if (textOnInitializingMask != null || maskedTextProvider.Mask != newProvider.Mask)
			{
				this.OnMaskChanged(empty);
			}
			this.SetWindowText(this.GetFormattedDisplayString(), a != this.TextOutput, flag);
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x00101D97 File Offset: 0x00100D97
		private void SetText()
		{
			this.SetWindowText(this.GetFormattedDisplayString(), true, false);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x00101DA7 File Offset: 0x00100DA7
		private void SetWindowText()
		{
			this.SetWindowText(this.GetFormattedDisplayString(), false, true);
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x00101DB8 File Offset: 0x00100DB8
		private void SetWindowText(string text, bool raiseTextChangedEvent, bool preserveCaret)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			try
			{
				if (preserveCaret)
				{
					this.caretTestPos = base.SelectionStart;
				}
				this.WindowText = text;
				if (raiseTextChangedEvent)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
				if (preserveCaret)
				{
					base.SelectionStart = this.caretTestPos;
				}
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x00101E30 File Offset: 0x00100E30
		private bool ShouldSerializeCulture()
		{
			return !CultureInfo.CurrentCulture.Equals(this.Culture);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x00101E45 File Offset: 0x00100E45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void Undo()
		{
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x00101E47 File Offset: 0x00100E47
		public object ValidateText()
		{
			return this.PerformTypeValidation(null);
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x00101E50 File Offset: 0x00100E50
		private bool WmClear()
		{
			if (!this.ReadOnly)
			{
				int startPosition;
				int selectionLen;
				base.GetSelectionStartAndLength(out startPosition, out selectionLen);
				this.Delete(Keys.Delete, startPosition, selectionLen);
				return true;
			}
			return false;
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x00101E7C File Offset: 0x00100E7C
		private bool WmCopy()
		{
			if (this.maskedTextProvider.IsPassword)
			{
				return false;
			}
			string selectedText = this.GetSelectedText();
			try
			{
				IntSecurity.ClipboardWrite.Assert();
				if (selectedText.Length == 0)
				{
					Clipboard.Clear();
				}
				else
				{
					Clipboard.SetText(selectedText);
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			return true;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x00101EE0 File Offset: 0x00100EE0
		private bool WmImeComposition(ref Message m)
		{
			if (ImeModeConversion.InputLanguageTable == ImeModeConversion.KoreanTable)
			{
				byte b = 0;
				if ((m.LParam.ToInt32() & 8) != 0)
				{
					b = 1;
				}
				else if ((m.LParam.ToInt32() & 2048) != 0)
				{
					b = 2;
				}
				if (b != 0 && this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION])
				{
					return this.flagState[MaskedTextBox.IME_COMPLETING];
				}
			}
			return false;
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x00101F50 File Offset: 0x00100F50
		private bool WmImeStartComposition()
		{
			int num;
			int num2;
			base.GetSelectionStartAndLength(out num, out num2);
			int num3 = this.maskedTextProvider.FindEditPositionFrom(num, true);
			if (num3 != MaskedTextProvider.InvalidIndex)
			{
				if (num2 > 0 && ImeModeConversion.InputLanguageTable == ImeModeConversion.KoreanTable)
				{
					int num4 = this.maskedTextProvider.FindEditPositionFrom(num + num2 - 1, false);
					if (num4 < num3)
					{
						this.ImeComplete();
						this.OnMaskInputRejected(new MaskInputRejectedEventArgs(num, MaskedTextResultHint.UnavailableEditPosition));
						return true;
					}
					num2 = num4 - num3 + 1;
					this.Delete(Keys.Delete, num3, num2);
				}
				if (num != num3)
				{
					this.caretTestPos = num3;
					base.SelectionStart = this.caretTestPos;
				}
				this.SelectionLength = 0;
				return false;
			}
			this.ImeComplete();
			this.OnMaskInputRejected(new MaskInputRejectedEventArgs(num, MaskedTextResultHint.UnavailableEditPosition));
			return true;
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x00102004 File Offset: 0x00101004
		private void WmPaste()
		{
			if (this.ReadOnly)
			{
				return;
			}
			string text;
			try
			{
				IntSecurity.ClipboardRead.Assert();
				text = Clipboard.GetText();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
				return;
			}
			this.PasteInt(text);
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x00102050 File Offset: 0x00101050
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && base.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rect = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						graphics.DrawRectangle(new Pen(VisualStyleInformation.TextControlBorder), rect);
						rect.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rect);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x00102120 File Offset: 0x00101120
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 183)
			{
				if (msg != 123)
				{
					if (msg != 183)
					{
						goto IL_5C;
					}
					return;
				}
			}
			else
			{
				switch (msg)
				{
				case 197:
				case 199:
					return;
				case 198:
					break;
				default:
					if (msg == 772)
					{
						return;
					}
					if (msg == 791)
					{
						this.WmPrint(ref m);
						return;
					}
					goto IL_5C;
				}
			}
			base.ClearUndo();
			base.WndProc(ref m);
			return;
			IL_5C:
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				base.WndProc(ref m);
				return;
			}
			int msg2 = m.Msg;
			switch (msg2)
			{
			case 7:
				this.WmSetFocus();
				base.WndProc(ref m);
				return;
			case 8:
				base.WndProc(ref m);
				this.WmKillFocus();
				return;
			default:
				switch (msg2)
				{
				case 269:
					if (this.WmImeStartComposition())
					{
						return;
					}
					break;
				case 270:
					this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION] = true;
					break;
				case 271:
					if (this.WmImeComposition(ref m))
					{
						return;
					}
					break;
				default:
					switch (msg2)
					{
					case 768:
						if (!this.ReadOnly && this.WmCopy())
						{
							this.WmClear();
							return;
						}
						return;
					case 769:
						this.WmCopy();
						return;
					case 770:
						this.WmPaste();
						return;
					case 771:
						this.WmClear();
						return;
					}
					break;
				}
				base.WndProc(ref m);
				return;
			}
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x00102268 File Offset: 0x00101268
		private void WmKillFocus()
		{
			base.GetSelectionStartAndLength(out this.caretTestPos, out this.lastSelLength);
			if (this.HidePromptOnLeave && !this.MaskFull)
			{
				this.SetWindowText();
				base.SelectInternal(this.caretTestPos, this.lastSelLength, this.maskedTextProvider.Length);
			}
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x001022BA File Offset: 0x001012BA
		private void WmSetFocus()
		{
			if (this.HidePromptOnLeave && !this.MaskFull)
			{
				this.SetWindowText();
			}
			base.SelectInternal(this.caretTestPos, this.lastSelLength, this.maskedTextProvider.Length);
		}

		// Token: 0x040021AD RID: 8621
		private const bool forward = true;

		// Token: 0x040021AE RID: 8622
		private const bool backward = false;

		// Token: 0x040021AF RID: 8623
		private const string nullMask = "<>";

		// Token: 0x040021B0 RID: 8624
		private const byte imeConvertionNone = 0;

		// Token: 0x040021B1 RID: 8625
		private const byte imeConvertionUpdate = 1;

		// Token: 0x040021B2 RID: 8626
		private const byte imeConvertionCompleted = 2;

		// Token: 0x040021B3 RID: 8627
		private static readonly object EVENT_MASKINPUTREJECTED = new object();

		// Token: 0x040021B4 RID: 8628
		private static readonly object EVENT_VALIDATIONCOMPLETED = new object();

		// Token: 0x040021B5 RID: 8629
		private static readonly object EVENT_TEXTALIGNCHANGED = new object();

		// Token: 0x040021B6 RID: 8630
		private static readonly object EVENT_ISOVERWRITEMODECHANGED = new object();

		// Token: 0x040021B7 RID: 8631
		private static readonly object EVENT_MASKCHANGED = new object();

		// Token: 0x040021B8 RID: 8632
		private static char systemPwdChar;

		// Token: 0x040021B9 RID: 8633
		private int lastSelLength;

		// Token: 0x040021BA RID: 8634
		private int caretTestPos;

		// Token: 0x040021BB RID: 8635
		private static int IME_ENDING_COMPOSITION = BitVector32.CreateMask();

		// Token: 0x040021BC RID: 8636
		private static int IME_COMPLETING = BitVector32.CreateMask(MaskedTextBox.IME_ENDING_COMPOSITION);

		// Token: 0x040021BD RID: 8637
		private static int HANDLE_KEY_PRESS = BitVector32.CreateMask(MaskedTextBox.IME_COMPLETING);

		// Token: 0x040021BE RID: 8638
		private static int IS_NULL_MASK = BitVector32.CreateMask(MaskedTextBox.HANDLE_KEY_PRESS);

		// Token: 0x040021BF RID: 8639
		private static int QUERY_BASE_TEXT = BitVector32.CreateMask(MaskedTextBox.IS_NULL_MASK);

		// Token: 0x040021C0 RID: 8640
		private static int REJECT_INPUT_ON_FIRST_FAILURE = BitVector32.CreateMask(MaskedTextBox.QUERY_BASE_TEXT);

		// Token: 0x040021C1 RID: 8641
		private static int HIDE_PROMPT_ON_LEAVE = BitVector32.CreateMask(MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE);

		// Token: 0x040021C2 RID: 8642
		private static int BEEP_ON_ERROR = BitVector32.CreateMask(MaskedTextBox.HIDE_PROMPT_ON_LEAVE);

		// Token: 0x040021C3 RID: 8643
		private static int USE_SYSTEM_PASSWORD_CHAR = BitVector32.CreateMask(MaskedTextBox.BEEP_ON_ERROR);

		// Token: 0x040021C4 RID: 8644
		private static int INSERT_TOGGLED = BitVector32.CreateMask(MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR);

		// Token: 0x040021C5 RID: 8645
		private static int CUTCOPYINCLUDEPROMPT = BitVector32.CreateMask(MaskedTextBox.INSERT_TOGGLED);

		// Token: 0x040021C6 RID: 8646
		private static int CUTCOPYINCLUDELITERALS = BitVector32.CreateMask(MaskedTextBox.CUTCOPYINCLUDEPROMPT);

		// Token: 0x040021C7 RID: 8647
		private char passwordChar;

		// Token: 0x040021C8 RID: 8648
		private Type validatingType;

		// Token: 0x040021C9 RID: 8649
		private IFormatProvider formatProvider;

		// Token: 0x040021CA RID: 8650
		private MaskedTextProvider maskedTextProvider;

		// Token: 0x040021CB RID: 8651
		private InsertKeyMode insertMode;

		// Token: 0x040021CC RID: 8652
		private HorizontalAlignment textAlign;

		// Token: 0x040021CD RID: 8653
		private BitVector32 flagState;
	}
}
