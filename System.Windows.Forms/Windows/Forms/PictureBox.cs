using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020005B0 RID: 1456
	[Docking(DockingBehavior.Ask)]
	[DefaultBindingProperty("Image")]
	[Designer("System.Windows.Forms.Design.PictureBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionPictureBox")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Image")]
	public class PictureBox : Control, ISupportInitialize
	{
		// Token: 0x06004B6C RID: 19308 RVA: 0x0011130C File Offset: 0x0011030C
		public PictureBox()
		{
			base.SetState2(2048, true);
			this.pictureBoxState = new BitVector32(12);
			base.SetStyle(ControlStyles.Opaque | ControlStyles.Selectable, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
			this.TabStop = false;
			this.savedSize = base.Size;
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06004B6D RID: 19309 RVA: 0x0011136E File Offset: 0x0011036E
		// (set) Token: 0x06004B6E RID: 19310 RVA: 0x00111376 File Offset: 0x00110376
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06004B6F RID: 19311 RVA: 0x0011137F File Offset: 0x0011037F
		// (set) Token: 0x06004B70 RID: 19312 RVA: 0x00111388 File Offset: 0x00110388
		[SRCategory("CatAppearance")]
		[DispId(-504)]
		[DefaultValue(BorderStyle.None)]
		[SRDescription("PictureBoxBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.RecreateHandle();
					this.AdjustSize();
				}
			}
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x001113D8 File Offset: 0x001103D8
		private Uri CalculateUri(string path)
		{
			Uri result;
			try
			{
				result = new Uri(path);
			}
			catch (UriFormatException)
			{
				path = Path.GetFullPath(path);
				result = new Uri(path);
			}
			return result;
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x00111414 File Offset: 0x00110414
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxCancelAsyncDescr")]
		public void CancelAsync()
		{
			this.pictureBoxState[2] = true;
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06004B73 RID: 19315 RVA: 0x00111423 File Offset: 0x00110423
		// (set) Token: 0x06004B74 RID: 19316 RVA: 0x0011142B File Offset: 0x0011042B
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		// Token: 0x140002A8 RID: 680
		// (add) Token: 0x06004B75 RID: 19317 RVA: 0x00111434 File Offset: 0x00110434
		// (remove) Token: 0x06004B76 RID: 19318 RVA: 0x0011143D File Offset: 0x0011043D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06004B77 RID: 19319 RVA: 0x00111448 File Offset: 0x00110448
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.ExStyle |= 512;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06004B78 RID: 19320 RVA: 0x0011149C File Offset: 0x0011049C
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06004B79 RID: 19321 RVA: 0x0011149F File Offset: 0x0011049F
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 50);
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06004B7A RID: 19322 RVA: 0x001114AC File Offset: 0x001104AC
		// (set) Token: 0x06004B7B RID: 19323 RVA: 0x00111514 File Offset: 0x00110514
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("PictureBoxErrorImageDescr")]
		[SRCategory("CatAsynchronous")]
		public Image ErrorImage
		{
			get
			{
				if (this.errorImage == null && this.pictureBoxState[8])
				{
					if (this.defaultErrorImage == null)
					{
						if (PictureBox.defaultErrorImageForThread == null)
						{
							PictureBox.defaultErrorImageForThread = new Bitmap(typeof(PictureBox), "ImageInError.bmp");
						}
						this.defaultErrorImage = PictureBox.defaultErrorImageForThread;
					}
					this.errorImage = this.defaultErrorImage;
				}
				return this.errorImage;
			}
			set
			{
				if (this.ErrorImage != value)
				{
					this.pictureBoxState[8] = false;
				}
				this.errorImage = value;
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06004B7C RID: 19324 RVA: 0x00111533 File Offset: 0x00110533
		// (set) Token: 0x06004B7D RID: 19325 RVA: 0x0011153B File Offset: 0x0011053B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x140002A9 RID: 681
		// (add) Token: 0x06004B7E RID: 19326 RVA: 0x00111544 File Offset: 0x00110544
		// (remove) Token: 0x06004B7F RID: 19327 RVA: 0x0011154D File Offset: 0x0011054D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06004B80 RID: 19328 RVA: 0x00111556 File Offset: 0x00110556
		// (set) Token: 0x06004B81 RID: 19329 RVA: 0x0011155E File Offset: 0x0011055E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x140002AA RID: 682
		// (add) Token: 0x06004B82 RID: 19330 RVA: 0x00111567 File Offset: 0x00110567
		// (remove) Token: 0x06004B83 RID: 19331 RVA: 0x00111570 File Offset: 0x00110570
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler FontChanged
		{
			add
			{
				base.FontChanged += value;
			}
			remove
			{
				base.FontChanged -= value;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06004B84 RID: 19332 RVA: 0x00111579 File Offset: 0x00110579
		// (set) Token: 0x06004B85 RID: 19333 RVA: 0x00111581 File Offset: 0x00110581
		[Localizable(true)]
		[Bindable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("PictureBoxImageDescr")]
		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.InstallNewImage(value, PictureBox.ImageInstallationType.DirectlySpecified);
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06004B86 RID: 19334 RVA: 0x0011158B File Offset: 0x0011058B
		// (set) Token: 0x06004B87 RID: 19335 RVA: 0x00111594 File Offset: 0x00110594
		[Localizable(true)]
		[DefaultValue(null)]
		[SRDescription("PictureBoxImageLocationDescr")]
		[SRCategory("CatAsynchronous")]
		[RefreshProperties(RefreshProperties.All)]
		public string ImageLocation
		{
			get
			{
				return this.imageLocation;
			}
			set
			{
				this.imageLocation = value;
				this.pictureBoxState[32] = !string.IsNullOrEmpty(this.imageLocation);
				if (string.IsNullOrEmpty(this.imageLocation) && this.imageInstallationType != PictureBox.ImageInstallationType.DirectlySpecified)
				{
					this.InstallNewImage(null, PictureBox.ImageInstallationType.DirectlySpecified);
				}
				if (this.WaitOnLoad && !this.pictureBoxState[64] && !string.IsNullOrEmpty(this.imageLocation))
				{
					this.Load();
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06004B88 RID: 19336 RVA: 0x00111610 File Offset: 0x00110610
		private Rectangle ImageRectangle
		{
			get
			{
				return this.ImageRectangleFromSizeMode(this.sizeMode);
			}
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x00111620 File Offset: 0x00110620
		private Rectangle ImageRectangleFromSizeMode(PictureBoxSizeMode mode)
		{
			Rectangle result = LayoutUtils.DeflateRect(base.ClientRectangle, base.Padding);
			if (this.image != null)
			{
				switch (mode)
				{
				case PictureBoxSizeMode.Normal:
				case PictureBoxSizeMode.AutoSize:
					result.Size = this.image.Size;
					break;
				case PictureBoxSizeMode.CenterImage:
					result.X += (result.Width - this.image.Width) / 2;
					result.Y += (result.Height - this.image.Height) / 2;
					result.Size = this.image.Size;
					break;
				case PictureBoxSizeMode.Zoom:
				{
					Size size = this.image.Size;
					float num = Math.Min((float)base.ClientRectangle.Width / (float)size.Width, (float)base.ClientRectangle.Height / (float)size.Height);
					result.Width = (int)((float)size.Width * num);
					result.Height = (int)((float)size.Height * num);
					result.X = (base.ClientRectangle.Width - result.Width) / 2;
					result.Y = (base.ClientRectangle.Height - result.Height) / 2;
					break;
				}
				}
			}
			return result;
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06004B8A RID: 19338 RVA: 0x00111788 File Offset: 0x00110788
		// (set) Token: 0x06004B8B RID: 19339 RVA: 0x001117F0 File Offset: 0x001107F0
		[SRCategory("CatAsynchronous")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("PictureBoxInitialImageDescr")]
		public Image InitialImage
		{
			get
			{
				if (this.initialImage == null && this.pictureBoxState[4])
				{
					if (this.defaultInitialImage == null)
					{
						if (PictureBox.defaultInitialImageForThread == null)
						{
							PictureBox.defaultInitialImageForThread = new Bitmap(typeof(PictureBox), "PictureBox.Loading.bmp");
						}
						this.defaultInitialImage = PictureBox.defaultInitialImageForThread;
					}
					this.initialImage = this.defaultInitialImage;
				}
				return this.initialImage;
			}
			set
			{
				if (this.InitialImage != value)
				{
					this.pictureBoxState[4] = false;
				}
				this.initialImage = value;
			}
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x00111810 File Offset: 0x00110810
		private void InstallNewImage(Image value, PictureBox.ImageInstallationType installationType)
		{
			this.StopAnimate();
			this.image = value;
			LayoutTransaction.DoLayoutIf(this.AutoSize, this, this, PropertyNames.Image);
			this.Animate();
			if (installationType != PictureBox.ImageInstallationType.ErrorOrInitial)
			{
				this.AdjustSize();
			}
			this.imageInstallationType = installationType;
			base.Invalidate();
			CommonProperties.xClearPreferredSizeCache(this);
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06004B8D RID: 19341 RVA: 0x0011185F File Offset: 0x0011085F
		// (set) Token: 0x06004B8E RID: 19342 RVA: 0x00111867 File Offset: 0x00110867
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		// Token: 0x140002AB RID: 683
		// (add) Token: 0x06004B8F RID: 19343 RVA: 0x00111870 File Offset: 0x00110870
		// (remove) Token: 0x06004B90 RID: 19344 RVA: 0x00111879 File Offset: 0x00110879
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x00111884 File Offset: 0x00110884
		[SRDescription("PictureBoxLoad0Descr")]
		[SRCategory("CatAsynchronous")]
		public void Load()
		{
			if (this.imageLocation == null || this.imageLocation.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("PictureBoxNoImageLocation"));
			}
			this.pictureBoxState[32] = false;
			PictureBox.ImageInstallationType installationType = PictureBox.ImageInstallationType.FromUrl;
			Image value;
			try
			{
				Uri uri = this.CalculateUri(this.imageLocation);
				if (uri.IsFile)
				{
					using (StreamReader streamReader = new StreamReader(uri.LocalPath))
					{
						value = Image.FromStream(streamReader.BaseStream);
						goto IL_A8;
					}
				}
				using (WebClient webClient = new WebClient())
				{
					using (Stream stream = webClient.OpenRead(uri.ToString()))
					{
						value = Image.FromStream(stream);
					}
				}
				IL_A8:;
			}
			catch
			{
				if (!base.DesignMode)
				{
					throw;
				}
				value = this.ErrorImage;
				installationType = PictureBox.ImageInstallationType.ErrorOrInitial;
			}
			this.InstallNewImage(value, installationType);
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x00111990 File Offset: 0x00110990
		[SRDescription("PictureBoxLoad1Descr")]
		[SRCategory("CatAsynchronous")]
		public void Load(string url)
		{
			this.ImageLocation = url;
			this.Load();
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x001119A0 File Offset: 0x001109A0
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadAsync0Descr")]
		public void LoadAsync()
		{
			if (this.imageLocation == null || this.imageLocation.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("PictureBoxNoImageLocation"));
			}
			if (this.pictureBoxState[1])
			{
				return;
			}
			this.pictureBoxState[1] = true;
			if ((this.Image == null || this.imageInstallationType == PictureBox.ImageInstallationType.ErrorOrInitial) && this.InitialImage != null)
			{
				this.InstallNewImage(this.InitialImage, PictureBox.ImageInstallationType.ErrorOrInitial);
			}
			this.currentAsyncLoadOperation = AsyncOperationManager.CreateOperation(null);
			if (this.loadCompletedDelegate == null)
			{
				this.loadCompletedDelegate = new SendOrPostCallback(this.LoadCompletedDelegate);
				this.loadProgressDelegate = new SendOrPostCallback(this.LoadProgressDelegate);
				this.readBuffer = new byte[4096];
			}
			this.pictureBoxState[32] = false;
			this.pictureBoxState[2] = false;
			this.contentLength = -1;
			this.tempDownloadStream = new MemoryStream();
			WebRequest state = WebRequest.Create(this.CalculateUri(this.imageLocation));
			new WaitCallback(this.BeginGetResponseDelegate).BeginInvoke(state, null, null);
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x00111AB0 File Offset: 0x00110AB0
		private void BeginGetResponseDelegate(object arg)
		{
			WebRequest webRequest = (WebRequest)arg;
			webRequest.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), webRequest);
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x00111AD8 File Offset: 0x00110AD8
		private void PostCompleted(Exception error, bool cancelled)
		{
			AsyncOperation asyncOperation = this.currentAsyncLoadOperation;
			this.currentAsyncLoadOperation = null;
			if (asyncOperation != null)
			{
				asyncOperation.PostOperationCompleted(this.loadCompletedDelegate, new AsyncCompletedEventArgs(error, cancelled, null));
			}
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x00111B0C File Offset: 0x00110B0C
		private void LoadCompletedDelegate(object arg)
		{
			AsyncCompletedEventArgs asyncCompletedEventArgs = (AsyncCompletedEventArgs)arg;
			Image value = this.ErrorImage;
			PictureBox.ImageInstallationType installationType = PictureBox.ImageInstallationType.ErrorOrInitial;
			if (!asyncCompletedEventArgs.Cancelled && asyncCompletedEventArgs.Error == null)
			{
				try
				{
					value = Image.FromStream(this.tempDownloadStream);
					installationType = PictureBox.ImageInstallationType.FromUrl;
				}
				catch (Exception error)
				{
					asyncCompletedEventArgs = new AsyncCompletedEventArgs(error, false, null);
				}
			}
			if (!asyncCompletedEventArgs.Cancelled)
			{
				this.InstallNewImage(value, installationType);
			}
			this.tempDownloadStream = null;
			this.pictureBoxState[2] = false;
			this.pictureBoxState[1] = false;
			this.OnLoadCompleted(asyncCompletedEventArgs);
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x00111BA0 File Offset: 0x00110BA0
		private void LoadProgressDelegate(object arg)
		{
			this.OnLoadProgressChanged((ProgressChangedEventArgs)arg);
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x00111BB0 File Offset: 0x00110BB0
		private void GetResponseCallback(IAsyncResult result)
		{
			if (this.pictureBoxState[2])
			{
				this.PostCompleted(null, true);
				return;
			}
			try
			{
				WebRequest webRequest = (WebRequest)result.AsyncState;
				WebResponse webResponse = webRequest.EndGetResponse(result);
				this.contentLength = (int)webResponse.ContentLength;
				this.totalBytesRead = 0;
				Stream responseStream = webResponse.GetResponseStream();
				responseStream.BeginRead(this.readBuffer, 0, 4096, new AsyncCallback(this.ReadCallBack), responseStream);
			}
			catch (Exception error)
			{
				this.PostCompleted(error, false);
			}
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x00111C40 File Offset: 0x00110C40
		private void ReadCallBack(IAsyncResult result)
		{
			if (this.pictureBoxState[2])
			{
				this.PostCompleted(null, true);
				return;
			}
			Stream stream = (Stream)result.AsyncState;
			try
			{
				int num = stream.EndRead(result);
				if (num > 0)
				{
					this.totalBytesRead += num;
					this.tempDownloadStream.Write(this.readBuffer, 0, num);
					stream.BeginRead(this.readBuffer, 0, 4096, new AsyncCallback(this.ReadCallBack), stream);
					if (this.contentLength != -1)
					{
						int progressPercentage = (int)(100f * ((float)this.totalBytesRead / (float)this.contentLength));
						if (this.currentAsyncLoadOperation != null)
						{
							this.currentAsyncLoadOperation.Post(this.loadProgressDelegate, new ProgressChangedEventArgs(progressPercentage, null));
						}
					}
				}
				else
				{
					this.tempDownloadStream.Seek(0L, SeekOrigin.Begin);
					if (this.currentAsyncLoadOperation != null)
					{
						this.currentAsyncLoadOperation.Post(this.loadProgressDelegate, new ProgressChangedEventArgs(100, null));
					}
					this.PostCompleted(null, false);
					Stream stream2 = stream;
					stream = null;
					stream2.Close();
				}
			}
			catch (Exception error)
			{
				this.PostCompleted(error, false);
				if (stream != null)
				{
					stream.Close();
				}
			}
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x00111D6C File Offset: 0x00110D6C
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadAsync1Descr")]
		public void LoadAsync(string url)
		{
			this.ImageLocation = url;
			this.LoadAsync();
		}

		// Token: 0x140002AC RID: 684
		// (add) Token: 0x06004B9B RID: 19355 RVA: 0x00111D7B File Offset: 0x00110D7B
		// (remove) Token: 0x06004B9C RID: 19356 RVA: 0x00111D8E File Offset: 0x00110D8E
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadCompletedDescr")]
		public event AsyncCompletedEventHandler LoadCompleted
		{
			add
			{
				base.Events.AddHandler(PictureBox.loadCompletedKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(PictureBox.loadCompletedKey, value);
			}
		}

		// Token: 0x140002AD RID: 685
		// (add) Token: 0x06004B9D RID: 19357 RVA: 0x00111DA1 File Offset: 0x00110DA1
		// (remove) Token: 0x06004B9E RID: 19358 RVA: 0x00111DB4 File Offset: 0x00110DB4
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadProgressChangedDescr")]
		public event ProgressChangedEventHandler LoadProgressChanged
		{
			add
			{
				base.Events.AddHandler(PictureBox.loadProgressChangedKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(PictureBox.loadProgressChangedKey, value);
			}
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x00111DC7 File Offset: 0x00110DC7
		private void ResetInitialImage()
		{
			this.pictureBoxState[4] = true;
			this.initialImage = this.defaultInitialImage;
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x00111DE2 File Offset: 0x00110DE2
		private void ResetErrorImage()
		{
			this.pictureBoxState[8] = true;
			this.errorImage = this.defaultErrorImage;
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x00111DFD File Offset: 0x00110DFD
		private void ResetImage()
		{
			this.InstallNewImage(null, PictureBox.ImageInstallationType.DirectlySpecified);
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06004BA2 RID: 19362 RVA: 0x00111E07 File Offset: 0x00110E07
		// (set) Token: 0x06004BA3 RID: 19363 RVA: 0x00111E0F File Offset: 0x00110E0F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		// Token: 0x140002AE RID: 686
		// (add) Token: 0x06004BA4 RID: 19364 RVA: 0x00111E18 File Offset: 0x00110E18
		// (remove) Token: 0x06004BA5 RID: 19365 RVA: 0x00111E21 File Offset: 0x00110E21
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x00111E2A File Offset: 0x00110E2A
		private bool ShouldSerializeInitialImage()
		{
			return !this.pictureBoxState[4];
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x00111E3B File Offset: 0x00110E3B
		private bool ShouldSerializeErrorImage()
		{
			return !this.pictureBoxState[8];
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x00111E4C File Offset: 0x00110E4C
		private bool ShouldSerializeImage()
		{
			return this.imageInstallationType == PictureBox.ImageInstallationType.DirectlySpecified && this.Image != null;
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x00111E64 File Offset: 0x00110E64
		// (set) Token: 0x06004BAA RID: 19370 RVA: 0x00111E6C File Offset: 0x00110E6C
		[DefaultValue(PictureBoxSizeMode.Normal)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("PictureBoxSizeModeDescr")]
		public PictureBoxSizeMode SizeMode
		{
			get
			{
				return this.sizeMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PictureBoxSizeMode));
				}
				if (this.sizeMode != value)
				{
					if (value == PictureBoxSizeMode.AutoSize)
					{
						this.AutoSize = true;
						base.SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
					}
					if (value != PictureBoxSizeMode.AutoSize)
					{
						this.AutoSize = false;
						base.SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, false);
						this.savedSize = base.Size;
					}
					this.sizeMode = value;
					this.AdjustSize();
					base.Invalidate();
					this.OnSizeModeChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140002AF RID: 687
		// (add) Token: 0x06004BAB RID: 19371 RVA: 0x00111EFA File Offset: 0x00110EFA
		// (remove) Token: 0x06004BAC RID: 19372 RVA: 0x00111F0D File Offset: 0x00110F0D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PictureBoxOnSizeModeChangedDescr")]
		public event EventHandler SizeModeChanged
		{
			add
			{
				base.Events.AddHandler(PictureBox.EVENT_SIZEMODECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(PictureBox.EVENT_SIZEMODECHANGED, value);
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06004BAD RID: 19373 RVA: 0x00111F20 File Offset: 0x00110F20
		// (set) Token: 0x06004BAE RID: 19374 RVA: 0x00111F28 File Offset: 0x00110F28
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x140002B0 RID: 688
		// (add) Token: 0x06004BAF RID: 19375 RVA: 0x00111F31 File Offset: 0x00110F31
		// (remove) Token: 0x06004BB0 RID: 19376 RVA: 0x00111F3A File Offset: 0x00110F3A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x00111F43 File Offset: 0x00110F43
		// (set) Token: 0x06004BB2 RID: 19378 RVA: 0x00111F4B File Offset: 0x00110F4B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		// Token: 0x140002B1 RID: 689
		// (add) Token: 0x06004BB3 RID: 19379 RVA: 0x00111F54 File Offset: 0x00110F54
		// (remove) Token: 0x06004BB4 RID: 19380 RVA: 0x00111F5D File Offset: 0x00110F5D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06004BB5 RID: 19381 RVA: 0x00111F66 File Offset: 0x00110F66
		// (set) Token: 0x06004BB6 RID: 19382 RVA: 0x00111F6E File Offset: 0x00110F6E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
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

		// Token: 0x140002B2 RID: 690
		// (add) Token: 0x06004BB7 RID: 19383 RVA: 0x00111F77 File Offset: 0x00110F77
		// (remove) Token: 0x06004BB8 RID: 19384 RVA: 0x00111F80 File Offset: 0x00110F80
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140002B3 RID: 691
		// (add) Token: 0x06004BB9 RID: 19385 RVA: 0x00111F89 File Offset: 0x00110F89
		// (remove) Token: 0x06004BBA RID: 19386 RVA: 0x00111F92 File Offset: 0x00110F92
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Enter
		{
			add
			{
				base.Enter += value;
			}
			remove
			{
				base.Enter -= value;
			}
		}

		// Token: 0x140002B4 RID: 692
		// (add) Token: 0x06004BBB RID: 19387 RVA: 0x00111F9B File Offset: 0x00110F9B
		// (remove) Token: 0x06004BBC RID: 19388 RVA: 0x00111FA4 File Offset: 0x00110FA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		// Token: 0x140002B5 RID: 693
		// (add) Token: 0x06004BBD RID: 19389 RVA: 0x00111FAD File Offset: 0x00110FAD
		// (remove) Token: 0x06004BBE RID: 19390 RVA: 0x00111FB6 File Offset: 0x00110FB6
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		// Token: 0x140002B6 RID: 694
		// (add) Token: 0x06004BBF RID: 19391 RVA: 0x00111FBF File Offset: 0x00110FBF
		// (remove) Token: 0x06004BC0 RID: 19392 RVA: 0x00111FC8 File Offset: 0x00110FC8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		// Token: 0x140002B7 RID: 695
		// (add) Token: 0x06004BC1 RID: 19393 RVA: 0x00111FD1 File Offset: 0x00110FD1
		// (remove) Token: 0x06004BC2 RID: 19394 RVA: 0x00111FDA File Offset: 0x00110FDA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Leave
		{
			add
			{
				base.Leave += value;
			}
			remove
			{
				base.Leave -= value;
			}
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x00111FE3 File Offset: 0x00110FE3
		private void AdjustSize()
		{
			if (this.sizeMode == PictureBoxSizeMode.AutoSize)
			{
				base.Size = base.PreferredSize;
				return;
			}
			base.Size = this.savedSize;
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x00112007 File Offset: 0x00111007
		private void Animate()
		{
			this.Animate(!base.DesignMode && base.Visible && base.Enabled && this.ParentInternal != null);
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x00112036 File Offset: 0x00111036
		private void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x00112040 File Offset: 0x00111040
		private void Animate(bool animate)
		{
			if (animate != this.currentlyAnimating)
			{
				if (animate)
				{
					if (this.image != null)
					{
						ImageAnimator.Animate(this.image, new EventHandler(this.OnFrameChanged));
						this.currentlyAnimating = animate;
						return;
					}
				}
				else if (this.image != null)
				{
					ImageAnimator.StopAnimate(this.image, new EventHandler(this.OnFrameChanged));
					this.currentlyAnimating = animate;
				}
			}
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x001120A6 File Offset: 0x001110A6
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopAnimate();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x001120B8 File Offset: 0x001110B8
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (this.image == null)
			{
				return CommonProperties.GetSpecifiedBounds(this).Size;
			}
			Size sz = this.SizeFromClientSize(Size.Empty) + base.Padding.Size;
			return this.image.Size + sz;
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x0011210C File Offset: 0x0011110C
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Animate();
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x0011211C File Offset: 0x0011111C
		private void OnFrameChanged(object o, EventArgs e)
		{
			if (base.Disposing || base.IsDisposed)
			{
				return;
			}
			if (base.InvokeRequired && base.IsHandleCreated)
			{
				lock (this.internalSyncObject)
				{
					if (this.handleValid)
					{
						base.BeginInvoke(new EventHandler(this.OnFrameChanged), new object[]
						{
							o,
							e
						});
					}
					return;
				}
			}
			base.Invalidate();
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x001121A4 File Offset: 0x001111A4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleDestroyed(EventArgs e)
		{
			lock (this.internalSyncObject)
			{
				this.handleValid = false;
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x001121E8 File Offset: 0x001111E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleCreated(EventArgs e)
		{
			lock (this.internalSyncObject)
			{
				this.handleValid = true;
			}
			base.OnHandleCreated(e);
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x0011222C File Offset: 0x0011122C
		protected virtual void OnLoadCompleted(AsyncCompletedEventArgs e)
		{
			AsyncCompletedEventHandler asyncCompletedEventHandler = (AsyncCompletedEventHandler)base.Events[PictureBox.loadCompletedKey];
			if (asyncCompletedEventHandler != null)
			{
				asyncCompletedEventHandler(this, e);
			}
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x0011225C File Offset: 0x0011125C
		protected virtual void OnLoadProgressChanged(ProgressChangedEventArgs e)
		{
			ProgressChangedEventHandler progressChangedEventHandler = (ProgressChangedEventHandler)base.Events[PictureBox.loadProgressChangedKey];
			if (progressChangedEventHandler != null)
			{
				progressChangedEventHandler(this, e);
			}
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x0011228C File Offset: 0x0011128C
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (this.pictureBoxState[32])
			{
				try
				{
					if (this.WaitOnLoad)
					{
						this.Load();
					}
					else
					{
						this.LoadAsync();
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
					this.image = this.ErrorImage;
				}
			}
			if (this.image != null)
			{
				this.Animate();
				ImageAnimator.UpdateFrames(this.image);
				Rectangle rect = (this.imageInstallationType == PictureBox.ImageInstallationType.ErrorOrInitial) ? this.ImageRectangleFromSizeMode(PictureBoxSizeMode.CenterImage) : this.ImageRectangle;
				pe.Graphics.DrawImage(this.image, rect);
			}
			base.OnPaint(pe);
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x00112334 File Offset: 0x00111334
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Animate();
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x00112343 File Offset: 0x00111343
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			this.Animate();
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x00112352 File Offset: 0x00111352
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.sizeMode == PictureBoxSizeMode.Zoom || this.sizeMode == PictureBoxSizeMode.StretchImage || this.sizeMode == PictureBoxSizeMode.CenterImage || this.BackgroundImage != null)
			{
				base.Invalidate();
			}
			this.savedSize = base.Size;
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x00112390 File Offset: 0x00111390
		protected virtual void OnSizeModeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[PictureBox.EVENT_SIZEMODECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x001123C0 File Offset: 0x001113C0
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", SizeMode: " + this.sizeMode.ToString("G");
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06004BD5 RID: 19413 RVA: 0x001123F4 File Offset: 0x001113F4
		// (set) Token: 0x06004BD6 RID: 19414 RVA: 0x00112403 File Offset: 0x00111403
		[Localizable(true)]
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxWaitOnLoadDescr")]
		[DefaultValue(false)]
		public bool WaitOnLoad
		{
			get
			{
				return this.pictureBoxState[16];
			}
			set
			{
				this.pictureBoxState[16] = value;
			}
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00112413 File Offset: 0x00111413
		void ISupportInitialize.BeginInit()
		{
			this.pictureBoxState[64] = true;
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00112423 File Offset: 0x00111423
		void ISupportInitialize.EndInit()
		{
			if (this.ImageLocation != null && this.ImageLocation.Length != 0 && this.WaitOnLoad)
			{
				this.Load();
			}
			this.pictureBoxState[64] = false;
		}

		// Token: 0x040030FF RID: 12543
		private const int readBlockSize = 4096;

		// Token: 0x04003100 RID: 12544
		private const int PICTUREBOXSTATE_asyncOperationInProgress = 1;

		// Token: 0x04003101 RID: 12545
		private const int PICTUREBOXSTATE_cancellationPending = 2;

		// Token: 0x04003102 RID: 12546
		private const int PICTUREBOXSTATE_useDefaultInitialImage = 4;

		// Token: 0x04003103 RID: 12547
		private const int PICTUREBOXSTATE_useDefaultErrorImage = 8;

		// Token: 0x04003104 RID: 12548
		private const int PICTUREBOXSTATE_waitOnLoad = 16;

		// Token: 0x04003105 RID: 12549
		private const int PICTUREBOXSTATE_needToLoadImageLocation = 32;

		// Token: 0x04003106 RID: 12550
		private const int PICTUREBOXSTATE_inInitialization = 64;

		// Token: 0x04003107 RID: 12551
		private BorderStyle borderStyle;

		// Token: 0x04003108 RID: 12552
		private Image image;

		// Token: 0x04003109 RID: 12553
		private PictureBoxSizeMode sizeMode;

		// Token: 0x0400310A RID: 12554
		private Size savedSize;

		// Token: 0x0400310B RID: 12555
		private bool currentlyAnimating;

		// Token: 0x0400310C RID: 12556
		private AsyncOperation currentAsyncLoadOperation;

		// Token: 0x0400310D RID: 12557
		private string imageLocation;

		// Token: 0x0400310E RID: 12558
		private Image initialImage;

		// Token: 0x0400310F RID: 12559
		private Image errorImage;

		// Token: 0x04003110 RID: 12560
		private int contentLength;

		// Token: 0x04003111 RID: 12561
		private int totalBytesRead;

		// Token: 0x04003112 RID: 12562
		private MemoryStream tempDownloadStream;

		// Token: 0x04003113 RID: 12563
		private byte[] readBuffer;

		// Token: 0x04003114 RID: 12564
		private PictureBox.ImageInstallationType imageInstallationType;

		// Token: 0x04003115 RID: 12565
		private SendOrPostCallback loadCompletedDelegate;

		// Token: 0x04003116 RID: 12566
		private SendOrPostCallback loadProgressDelegate;

		// Token: 0x04003117 RID: 12567
		private bool handleValid;

		// Token: 0x04003118 RID: 12568
		private object internalSyncObject = new object();

		// Token: 0x04003119 RID: 12569
		private Image defaultInitialImage;

		// Token: 0x0400311A RID: 12570
		private Image defaultErrorImage;

		// Token: 0x0400311B RID: 12571
		[ThreadStatic]
		private static Image defaultInitialImageForThread = null;

		// Token: 0x0400311C RID: 12572
		[ThreadStatic]
		private static Image defaultErrorImageForThread = null;

		// Token: 0x0400311D RID: 12573
		private static readonly object defaultInitialImageKey = new object();

		// Token: 0x0400311E RID: 12574
		private static readonly object defaultErrorImageKey = new object();

		// Token: 0x0400311F RID: 12575
		private static readonly object loadCompletedKey = new object();

		// Token: 0x04003120 RID: 12576
		private static readonly object loadProgressChangedKey = new object();

		// Token: 0x04003121 RID: 12577
		private BitVector32 pictureBoxState;

		// Token: 0x04003122 RID: 12578
		private static readonly object EVENT_SIZEMODECHANGED = new object();

		// Token: 0x020005B1 RID: 1457
		private enum ImageInstallationType
		{
			// Token: 0x04003124 RID: 12580
			DirectlySpecified,
			// Token: 0x04003125 RID: 12581
			ErrorOrInitial,
			// Token: 0x04003126 RID: 12582
			FromUrl
		}
	}
}
