using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020005A4 RID: 1444
	[ComVisible(true)]
	[DefaultBindingProperty("Value")]
	[SRDescription("DescriptionNumericUpDown")]
	[DefaultProperty("Value")]
	[DefaultEvent("ValueChanged")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class NumericUpDown : UpDownBase, ISupportInitialize
	{
		// Token: 0x06004AC3 RID: 19139 RVA: 0x0010F39C File Offset: 0x0010E39C
		public NumericUpDown()
		{
			base.SetState2(2048, true);
			this.Text = "0";
			this.StopAcceleration();
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06004AC4 RID: 19140 RVA: 0x0010F3F8 File Offset: 0x0010E3F8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NumericUpDownAccelerationCollection Accelerations
		{
			get
			{
				if (this.accelerations == null)
				{
					this.accelerations = new NumericUpDownAccelerationCollection();
				}
				return this.accelerations;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06004AC5 RID: 19141 RVA: 0x0010F413 File Offset: 0x0010E413
		// (set) Token: 0x06004AC6 RID: 19142 RVA: 0x0010F41C File Offset: 0x0010E41C
		[SRCategory("CatData")]
		[DefaultValue(0)]
		[SRDescription("NumericUpDownDecimalPlacesDescr")]
		public int DecimalPlaces
		{
			get
			{
				return this.decimalPlaces;
			}
			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("DecimalPlaces", SR.GetString("InvalidBoundArgument", new object[]
					{
						"DecimalPlaces",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture),
						"99"
					}));
				}
				this.decimalPlaces = value;
				this.UpdateEditText();
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06004AC7 RID: 19143 RVA: 0x0010F48C File Offset: 0x0010E48C
		// (set) Token: 0x06004AC8 RID: 19144 RVA: 0x0010F494 File Offset: 0x0010E494
		[DefaultValue(false)]
		[SRDescription("NumericUpDownHexadecimalDescr")]
		[SRCategory("CatAppearance")]
		public bool Hexadecimal
		{
			get
			{
				return this.hexadecimal;
			}
			set
			{
				this.hexadecimal = value;
				this.UpdateEditText();
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06004AC9 RID: 19145 RVA: 0x0010F4A3 File Offset: 0x0010E4A3
		// (set) Token: 0x06004ACA RID: 19146 RVA: 0x0010F4CC File Offset: 0x0010E4CC
		[SRDescription("NumericUpDownIncrementDescr")]
		[SRCategory("CatData")]
		public decimal Increment
		{
			get
			{
				if (this.accelerationsCurrentIndex != -1)
				{
					return this.Accelerations[this.accelerationsCurrentIndex].Increment;
				}
				return this.increment;
			}
			set
			{
				if (value < 0m)
				{
					throw new ArgumentOutOfRangeException("Increment", SR.GetString("InvalidArgument", new object[]
					{
						"Increment",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.increment = value;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06004ACB RID: 19147 RVA: 0x0010F522 File Offset: 0x0010E522
		// (set) Token: 0x06004ACC RID: 19148 RVA: 0x0010F52A File Offset: 0x0010E52A
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("NumericUpDownMaximumDescr")]
		[SRCategory("CatData")]
		public decimal Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				this.maximum = value;
				if (this.minimum > this.maximum)
				{
					this.minimum = this.maximum;
				}
				this.Value = this.Constrain(this.currentValue);
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06004ACD RID: 19149 RVA: 0x0010F564 File Offset: 0x0010E564
		// (set) Token: 0x06004ACE RID: 19150 RVA: 0x0010F56C File Offset: 0x0010E56C
		[SRDescription("NumericUpDownMinimumDescr")]
		[SRCategory("CatData")]
		[RefreshProperties(RefreshProperties.All)]
		public decimal Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				this.minimum = value;
				if (this.minimum > this.maximum)
				{
					this.maximum = value;
				}
				this.Value = this.Constrain(this.currentValue);
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06004ACF RID: 19151 RVA: 0x0010F5A1 File Offset: 0x0010E5A1
		// (set) Token: 0x06004AD0 RID: 19152 RVA: 0x0010F5A9 File Offset: 0x0010E5A9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x140002A5 RID: 677
		// (add) Token: 0x06004AD1 RID: 19153 RVA: 0x0010F5B2 File Offset: 0x0010E5B2
		// (remove) Token: 0x06004AD2 RID: 19154 RVA: 0x0010F5BB File Offset: 0x0010E5BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06004AD3 RID: 19155 RVA: 0x0010F5C4 File Offset: 0x0010E5C4
		private bool Spinning
		{
			get
			{
				return this.accelerations != null && this.buttonPressedStartTime != -1L;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06004AD4 RID: 19156 RVA: 0x0010F5DD File Offset: 0x0010E5DD
		// (set) Token: 0x06004AD5 RID: 19157 RVA: 0x0010F5E5 File Offset: 0x0010E5E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x140002A6 RID: 678
		// (add) Token: 0x06004AD6 RID: 19158 RVA: 0x0010F5EE File Offset: 0x0010E5EE
		// (remove) Token: 0x06004AD7 RID: 19159 RVA: 0x0010F5F7 File Offset: 0x0010E5F7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06004AD8 RID: 19160 RVA: 0x0010F600 File Offset: 0x0010E600
		// (set) Token: 0x06004AD9 RID: 19161 RVA: 0x0010F608 File Offset: 0x0010E608
		[SRCategory("CatData")]
		[Localizable(true)]
		[SRDescription("NumericUpDownThousandsSeparatorDescr")]
		[DefaultValue(false)]
		public bool ThousandsSeparator
		{
			get
			{
				return this.thousandsSeparator;
			}
			set
			{
				this.thousandsSeparator = value;
				this.UpdateEditText();
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06004ADA RID: 19162 RVA: 0x0010F617 File Offset: 0x0010E617
		// (set) Token: 0x06004ADB RID: 19163 RVA: 0x0010F630 File Offset: 0x0010E630
		[SRCategory("CatAppearance")]
		[SRDescription("NumericUpDownValueDescr")]
		[Bindable(true)]
		public decimal Value
		{
			get
			{
				if (base.UserEdit)
				{
					this.ValidateEditText();
				}
				return this.currentValue;
			}
			set
			{
				if (value != this.currentValue)
				{
					if (!this.initializing && (value < this.minimum || value > this.maximum))
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'Minimum'",
							"'Maximum'"
						}));
					}
					this.currentValue = value;
					this.OnValueChanged(EventArgs.Empty);
					this.currentValueChanged = true;
					this.UpdateEditText();
				}
			}
		}

		// Token: 0x140002A7 RID: 679
		// (add) Token: 0x06004ADC RID: 19164 RVA: 0x0010F6D5 File Offset: 0x0010E6D5
		// (remove) Token: 0x06004ADD RID: 19165 RVA: 0x0010F6EE File Offset: 0x0010E6EE
		[SRCategory("CatAction")]
		[SRDescription("NumericUpDownOnValueChangedDescr")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.onValueChanged = (EventHandler)Delegate.Combine(this.onValueChanged, value);
			}
			remove
			{
				this.onValueChanged = (EventHandler)Delegate.Remove(this.onValueChanged, value);
			}
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x0010F707 File Offset: 0x0010E707
		public void BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0010F710 File Offset: 0x0010E710
		private decimal Constrain(decimal value)
		{
			if (value < this.minimum)
			{
				value = this.minimum;
			}
			if (value > this.maximum)
			{
				value = this.maximum;
			}
			return value;
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x0010F73F File Offset: 0x0010E73F
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new NumericUpDown.NumericUpDownAccessibleObject(this);
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x0010F748 File Offset: 0x0010E748
		public override void DownButton()
		{
			this.SetNextAcceleration();
			if (base.UserEdit)
			{
				this.ParseEditText();
			}
			decimal num = this.currentValue;
			try
			{
				num -= this.Increment;
				if (num < this.minimum)
				{
					num = this.minimum;
					if (this.Spinning)
					{
						this.StopAcceleration();
					}
				}
			}
			catch (OverflowException)
			{
				num = this.minimum;
			}
			this.Value = num;
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x0010F7C4 File Offset: 0x0010E7C4
		public void EndInit()
		{
			this.initializing = false;
			this.Value = this.Constrain(this.currentValue);
			this.UpdateEditText();
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x0010F7E5 File Offset: 0x0010E7E5
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (base.InterceptArrowKeys && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && !this.Spinning)
			{
				this.StartAcceleration();
			}
			base.OnKeyDown(e);
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x0010F818 File Offset: 0x0010E818
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (base.InterceptArrowKeys && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
			{
				this.StopAcceleration();
			}
			base.OnKeyUp(e);
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x0010F844 File Offset: 0x0010E844
		protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			base.OnTextBoxKeyPress(source, e);
			NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
			string numberDecimalSeparator = numberFormat.NumberDecimalSeparator;
			string numberGroupSeparator = numberFormat.NumberGroupSeparator;
			string negativeSign = numberFormat.NegativeSign;
			string text = e.KeyChar.ToString();
			if (char.IsDigit(e.KeyChar))
			{
				return;
			}
			if (!text.Equals(numberDecimalSeparator) && !text.Equals(numberGroupSeparator))
			{
				if (text.Equals(negativeSign))
				{
					return;
				}
				if (e.KeyChar == '\b')
				{
					return;
				}
				if (this.Hexadecimal)
				{
					if (e.KeyChar >= 'a' && e.KeyChar <= 'f')
					{
						return;
					}
					if (e.KeyChar >= 'A' && e.KeyChar <= 'F')
					{
						return;
					}
				}
				if ((Control.ModifierKeys & (Keys.Control | Keys.Alt)) != Keys.None)
				{
					return;
				}
				e.Handled = true;
				SafeNativeMethods.MessageBeep(0);
			}
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x0010F90E File Offset: 0x0010E90E
		protected virtual void OnValueChanged(EventArgs e)
		{
			if (this.onValueChanged != null)
			{
				this.onValueChanged(this, e);
			}
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x0010F925 File Offset: 0x0010E925
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (base.UserEdit)
			{
				this.UpdateEditText();
			}
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x0010F93C File Offset: 0x0010E93C
		internal override void OnStartTimer()
		{
			this.StartAcceleration();
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x0010F944 File Offset: 0x0010E944
		internal override void OnStopTimer()
		{
			this.StopAcceleration();
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x0010F94C File Offset: 0x0010E94C
		protected void ParseEditText()
		{
			try
			{
				if (!string.IsNullOrEmpty(this.Text) && (this.Text.Length != 1 || !(this.Text == "-")))
				{
					if (this.Hexadecimal)
					{
						this.Value = this.Constrain(Convert.ToDecimal(Convert.ToInt32(this.Text, 16)));
					}
					else
					{
						this.Value = this.Constrain(decimal.Parse(this.Text, CultureInfo.CurrentCulture));
					}
				}
			}
			catch
			{
			}
			finally
			{
				base.UserEdit = false;
			}
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0010F9F8 File Offset: 0x0010E9F8
		private void SetNextAcceleration()
		{
			if (this.Spinning && this.accelerationsCurrentIndex < this.accelerations.Count - 1)
			{
				long ticks = DateTime.Now.Ticks;
				long num = ticks - this.buttonPressedStartTime;
				long num2 = 10000000L * (long)this.accelerations[this.accelerationsCurrentIndex + 1].Seconds;
				if (num > num2)
				{
					this.buttonPressedStartTime = ticks;
					this.accelerationsCurrentIndex++;
				}
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x0010FA73 File Offset: 0x0010EA73
		private void ResetIncrement()
		{
			this.Increment = NumericUpDown.DefaultIncrement;
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x0010FA80 File Offset: 0x0010EA80
		private void ResetMaximum()
		{
			this.Maximum = NumericUpDown.DefaultMaximum;
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x0010FA8D File Offset: 0x0010EA8D
		private void ResetMinimum()
		{
			this.Minimum = NumericUpDown.DefaultMinimum;
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0010FA9A File Offset: 0x0010EA9A
		private void ResetValue()
		{
			this.Value = NumericUpDown.DefaultValue;
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x0010FAA8 File Offset: 0x0010EAA8
		private bool ShouldSerializeIncrement()
		{
			return !this.Increment.Equals(NumericUpDown.DefaultIncrement);
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x0010FACC File Offset: 0x0010EACC
		private bool ShouldSerializeMaximum()
		{
			return !this.Maximum.Equals(NumericUpDown.DefaultMaximum);
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x0010FAF0 File Offset: 0x0010EAF0
		private bool ShouldSerializeMinimum()
		{
			return !this.Minimum.Equals(NumericUpDown.DefaultMinimum);
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0010FB14 File Offset: 0x0010EB14
		private bool ShouldSerializeValue()
		{
			return !this.Value.Equals(NumericUpDown.DefaultValue);
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0010FB38 File Offset: 0x0010EB38
		private void StartAcceleration()
		{
			this.buttonPressedStartTime = DateTime.Now.Ticks;
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0010FB58 File Offset: 0x0010EB58
		private void StopAcceleration()
		{
			this.accelerationsCurrentIndex = -1;
			this.buttonPressedStartTime = -1L;
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0010FB6C File Offset: 0x0010EB6C
		public override string ToString()
		{
			string text = base.ToString();
			string text2 = text;
			return string.Concat(new string[]
			{
				text2,
				", Minimum = ",
				this.Minimum.ToString(CultureInfo.CurrentCulture),
				", Maximum = ",
				this.Maximum.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0010FBD4 File Offset: 0x0010EBD4
		public override void UpButton()
		{
			this.SetNextAcceleration();
			if (base.UserEdit)
			{
				this.ParseEditText();
			}
			decimal num = this.currentValue;
			try
			{
				num += this.Increment;
				if (num > this.maximum)
				{
					num = this.maximum;
					if (this.Spinning)
					{
						this.StopAcceleration();
					}
				}
			}
			catch (OverflowException)
			{
				num = this.maximum;
			}
			this.Value = num;
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x0010FC50 File Offset: 0x0010EC50
		private string GetNumberText(decimal num)
		{
			string result;
			if (this.Hexadecimal)
			{
				result = ((long)num).ToString("X", CultureInfo.InvariantCulture);
			}
			else
			{
				result = num.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
			}
			return result;
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0010FCBC File Offset: 0x0010ECBC
		protected override void UpdateEditText()
		{
			if (this.initializing)
			{
				return;
			}
			if (base.UserEdit)
			{
				this.ParseEditText();
			}
			if (this.currentValueChanged || (!string.IsNullOrEmpty(this.Text) && (this.Text.Length != 1 || !(this.Text == "-"))))
			{
				this.currentValueChanged = false;
				base.ChangingText = true;
				this.Text = this.GetNumberText(this.currentValue);
			}
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x0010FD35 File Offset: 0x0010ED35
		protected override void ValidateEditText()
		{
			this.ParseEditText();
			this.UpdateEditText();
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x0010FD44 File Offset: 0x0010ED44
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = base.PreferredHeight;
			int num = this.Hexadecimal ? 16 : 10;
			int largestDigit = this.GetLargestDigit(0, num);
			int num2 = (int)Math.Floor(Math.Log(Math.Max(-(double)this.Minimum, (double)this.Maximum), (double)num));
			decimal num3;
			if (largestDigit != 0 || num2 == 1)
			{
				num3 = largestDigit;
			}
			else
			{
				num3 = this.GetLargestDigit(1, num);
			}
			for (int i = 0; i < num2; i++)
			{
				num3 = num3 * num + largestDigit;
			}
			int width = TextRenderer.MeasureText(this.GetNumberText(num3), this.Font).Width;
			int width2 = base.SizeFromClientSize(width, preferredHeight).Width + this.upDownButtons.Width;
			return new Size(width2, preferredHeight) + this.Padding.Size;
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0010FE40 File Offset: 0x0010EE40
		private int GetLargestDigit(int start, int end)
		{
			int result = -1;
			int num = -1;
			for (int i = start; i < end; i++)
			{
				char c;
				if (i < 10)
				{
					c = i.ToString(CultureInfo.InvariantCulture)[0];
				}
				else
				{
					c = (char)(65 + (i - 10));
				}
				Size size = TextRenderer.MeasureText(c.ToString(), this.Font);
				if (size.Width >= num)
				{
					num = size.Width;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x040030D4 RID: 12500
		private const int DefaultDecimalPlaces = 0;

		// Token: 0x040030D5 RID: 12501
		private const bool DefaultThousandsSeparator = false;

		// Token: 0x040030D6 RID: 12502
		private const bool DefaultHexadecimal = false;

		// Token: 0x040030D7 RID: 12503
		private const int InvalidValue = -1;

		// Token: 0x040030D8 RID: 12504
		private static readonly decimal DefaultValue = 0m;

		// Token: 0x040030D9 RID: 12505
		private static readonly decimal DefaultMinimum = 0m;

		// Token: 0x040030DA RID: 12506
		private static readonly decimal DefaultMaximum = 100m;

		// Token: 0x040030DB RID: 12507
		private static readonly decimal DefaultIncrement = 1m;

		// Token: 0x040030DC RID: 12508
		private int decimalPlaces;

		// Token: 0x040030DD RID: 12509
		private decimal increment = NumericUpDown.DefaultIncrement;

		// Token: 0x040030DE RID: 12510
		private bool thousandsSeparator;

		// Token: 0x040030DF RID: 12511
		private decimal minimum = NumericUpDown.DefaultMinimum;

		// Token: 0x040030E0 RID: 12512
		private decimal maximum = NumericUpDown.DefaultMaximum;

		// Token: 0x040030E1 RID: 12513
		private bool hexadecimal;

		// Token: 0x040030E2 RID: 12514
		private decimal currentValue = NumericUpDown.DefaultValue;

		// Token: 0x040030E3 RID: 12515
		private bool currentValueChanged;

		// Token: 0x040030E4 RID: 12516
		private EventHandler onValueChanged;

		// Token: 0x040030E5 RID: 12517
		private bool initializing;

		// Token: 0x040030E6 RID: 12518
		private NumericUpDownAccelerationCollection accelerations;

		// Token: 0x040030E7 RID: 12519
		private int accelerationsCurrentIndex;

		// Token: 0x040030E8 RID: 12520
		private long buttonPressedStartTime;

		// Token: 0x020005A5 RID: 1445
		[ComVisible(true)]
		internal class NumericUpDownAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06004AFE RID: 19198 RVA: 0x0010FED8 File Offset: 0x0010EED8
			public NumericUpDownAccessibleObject(NumericUpDown owner) : base(owner)
			{
			}

			// Token: 0x17000EC9 RID: 3785
			// (get) Token: 0x06004AFF RID: 19199 RVA: 0x0010FEE4 File Offset: 0x0010EEE4
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.ComboBox;
				}
			}

			// Token: 0x06004B00 RID: 19200 RVA: 0x0010FF08 File Offset: 0x0010EF08
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < this.GetChildCount())
				{
					if (index == 0)
					{
						return ((UpDownBase)base.Owner).TextBox.AccessibilityObject.Parent;
					}
					if (index == 1)
					{
						return ((UpDownBase)base.Owner).UpDownButtonsInternal.AccessibilityObject.Parent;
					}
				}
				return null;
			}

			// Token: 0x06004B01 RID: 19201 RVA: 0x0010FF60 File Offset: 0x0010EF60
			public override int GetChildCount()
			{
				return 2;
			}
		}
	}
}
