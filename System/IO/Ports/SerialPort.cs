using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.IO.Ports
{
	// Token: 0x020007AF RID: 1967
	[MonitoringDescription("SerialPortDesc")]
	public class SerialPort : Component
	{
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06003C37 RID: 15415 RVA: 0x001012E8 File Offset: 0x001002E8
		// (remove) Token: 0x06003C38 RID: 15416 RVA: 0x00101301 File Offset: 0x00100301
		[MonitoringDescription("SerialErrorReceived")]
		public event SerialErrorReceivedEventHandler ErrorReceived;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06003C39 RID: 15417 RVA: 0x0010131A File Offset: 0x0010031A
		// (remove) Token: 0x06003C3A RID: 15418 RVA: 0x00101333 File Offset: 0x00100333
		[MonitoringDescription("SerialPinChanged")]
		public event SerialPinChangedEventHandler PinChanged;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06003C3B RID: 15419 RVA: 0x0010134C File Offset: 0x0010034C
		// (remove) Token: 0x06003C3C RID: 15420 RVA: 0x00101365 File Offset: 0x00100365
		[MonitoringDescription("SerialDataReceived")]
		public event SerialDataReceivedEventHandler DataReceived;

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x0010137E File Offset: 0x0010037E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Stream BaseStream
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("BaseStream_Invalid_Not_Open"));
				}
				return this.internalSerialStream;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x0010139E File Offset: 0x0010039E
		// (set) Token: 0x06003C3F RID: 15423 RVA: 0x001013A6 File Offset: 0x001003A6
		[Browsable(true)]
		[DefaultValue(9600)]
		[MonitoringDescription("BaudRate")]
		public int BaudRate
		{
			get
			{
				return this.baudRate;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("BaudRate", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.BaudRate = value;
				}
				this.baudRate = value;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003C40 RID: 15424 RVA: 0x001013DC File Offset: 0x001003DC
		// (set) Token: 0x06003C41 RID: 15425 RVA: 0x00101401 File Offset: 0x00100401
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool BreakState
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.BreakState;
			}
			set
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				this.internalSerialStream.BreakState = value;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003C42 RID: 15426 RVA: 0x00101427 File Offset: 0x00100427
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int BytesToWrite
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.BytesToWrite;
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x0010144C File Offset: 0x0010044C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int BytesToRead
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.BytesToRead + this.CachedBytesToRead;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003C44 RID: 15428 RVA: 0x00101478 File Offset: 0x00100478
		private int CachedBytesToRead
		{
			get
			{
				return this.readLen - this.readPos;
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x00101487 File Offset: 0x00100487
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CDHolding
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.CDHolding;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06003C46 RID: 15430 RVA: 0x001014AC File Offset: 0x001004AC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CtsHolding
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.CtsHolding;
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06003C47 RID: 15431 RVA: 0x001014D1 File Offset: 0x001004D1
		// (set) Token: 0x06003C48 RID: 15432 RVA: 0x001014DC File Offset: 0x001004DC
		[MonitoringDescription("DataBits")]
		[Browsable(true)]
		[DefaultValue(8)]
		public int DataBits
		{
			get
			{
				return this.dataBits;
			}
			set
			{
				if (value < 5 || value > 8)
				{
					throw new ArgumentOutOfRangeException("DataBits", SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", new object[]
					{
						5,
						8
					}));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.DataBits = value;
				}
				this.dataBits = value;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x0010153B File Offset: 0x0010053B
		// (set) Token: 0x06003C4A RID: 15434 RVA: 0x00101543 File Offset: 0x00100543
		[MonitoringDescription("DiscardNull")]
		[DefaultValue(false)]
		[Browsable(true)]
		public bool DiscardNull
		{
			get
			{
				return this.discardNull;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.DiscardNull = value;
				}
				this.discardNull = value;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06003C4B RID: 15435 RVA: 0x00101560 File Offset: 0x00100560
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool DsrHolding
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.DsrHolding;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06003C4C RID: 15436 RVA: 0x00101585 File Offset: 0x00100585
		// (set) Token: 0x06003C4D RID: 15437 RVA: 0x001015A6 File Offset: 0x001005A6
		[DefaultValue(false)]
		[Browsable(true)]
		[MonitoringDescription("DtrEnable")]
		public bool DtrEnable
		{
			get
			{
				if (this.IsOpen)
				{
					this.dtrEnable = this.internalSerialStream.DtrEnable;
				}
				return this.dtrEnable;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.DtrEnable = value;
				}
				this.dtrEnable = value;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06003C4E RID: 15438 RVA: 0x001015C3 File Offset: 0x001005C3
		// (set) Token: 0x06003C4F RID: 15439 RVA: 0x001015CC File Offset: 0x001005CC
		[MonitoringDescription("Encoding")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Encoding");
				}
				if (!(value is ASCIIEncoding) && !(value is UTF8Encoding) && !(value is UnicodeEncoding) && !(value is UTF32Encoding) && ((value.CodePage >= 50000 && value.CodePage != 54936) || value.GetType().Assembly != typeof(string).Assembly))
				{
					throw new ArgumentException(SR.GetString("NotSupportedEncoding", new object[]
					{
						value.WebName
					}), "value");
				}
				this.encoding = value;
				this.decoder = this.encoding.GetDecoder();
				this.maxByteCountForSingleChar = this.encoding.GetMaxByteCount(1);
				this.singleCharBuffer = null;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06003C50 RID: 15440 RVA: 0x00101694 File Offset: 0x00100694
		// (set) Token: 0x06003C51 RID: 15441 RVA: 0x0010169C File Offset: 0x0010069C
		[Browsable(true)]
		[DefaultValue(Handshake.None)]
		[MonitoringDescription("Handshake")]
		public Handshake Handshake
		{
			get
			{
				return this.handshake;
			}
			set
			{
				if (value < Handshake.None || value > Handshake.RequestToSendXOnXOff)
				{
					throw new ArgumentOutOfRangeException("Handshake", SR.GetString("ArgumentOutOfRange_Enum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.Handshake = value;
				}
				this.handshake = value;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x001016D6 File Offset: 0x001006D6
		[Browsable(false)]
		public bool IsOpen
		{
			get
			{
				return this.internalSerialStream != null && this.internalSerialStream.IsOpen;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06003C53 RID: 15443 RVA: 0x001016ED File Offset: 0x001006ED
		// (set) Token: 0x06003C54 RID: 15444 RVA: 0x001016F8 File Offset: 0x001006F8
		[MonitoringDescription("NewLine")]
		[DefaultValue("\n")]
		[Browsable(false)]
		public string NewLine
		{
			get
			{
				return this.newLine;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[]
					{
						"NewLine"
					}));
				}
				this.newLine = value;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06003C55 RID: 15445 RVA: 0x0010173D File Offset: 0x0010073D
		// (set) Token: 0x06003C56 RID: 15446 RVA: 0x00101745 File Offset: 0x00100745
		[DefaultValue(Parity.None)]
		[MonitoringDescription("Parity")]
		[Browsable(true)]
		public Parity Parity
		{
			get
			{
				return this.parity;
			}
			set
			{
				if (value < Parity.None || value > Parity.Space)
				{
					throw new ArgumentOutOfRangeException("Parity", SR.GetString("ArgumentOutOfRange_Enum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.Parity = value;
				}
				this.parity = value;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06003C57 RID: 15447 RVA: 0x0010177F File Offset: 0x0010077F
		// (set) Token: 0x06003C58 RID: 15448 RVA: 0x00101787 File Offset: 0x00100787
		[Browsable(true)]
		[DefaultValue(63)]
		[MonitoringDescription("ParityReplace")]
		public byte ParityReplace
		{
			get
			{
				return this.parityReplace;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.ParityReplace = value;
				}
				this.parityReplace = value;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06003C59 RID: 15449 RVA: 0x001017A4 File Offset: 0x001007A4
		// (set) Token: 0x06003C5A RID: 15450 RVA: 0x001017AC File Offset: 0x001007AC
		[DefaultValue("COM1")]
		[Browsable(true)]
		[MonitoringDescription("PortName")]
		public string PortName
		{
			get
			{
				return this.portName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PortName");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PortNameEmpty_String"), "PortName");
				}
				if (value.StartsWith("\\\\", StringComparison.Ordinal))
				{
					throw new ArgumentException(SR.GetString("Arg_SecurityException"), "PortName");
				}
				if (this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Cant_be_set_when_open", new object[]
					{
						"PortName"
					}));
				}
				this.portName = value;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06003C5B RID: 15451 RVA: 0x00101836 File Offset: 0x00100836
		// (set) Token: 0x06003C5C RID: 15452 RVA: 0x00101840 File Offset: 0x00100840
		[MonitoringDescription("ReadBufferSize")]
		[Browsable(true)]
		[DefaultValue(4096)]
		public int ReadBufferSize
		{
			get
			{
				return this.readBufferSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Cant_be_set_when_open", new object[]
					{
						"value"
					}));
				}
				this.readBufferSize = value;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x0010188B File Offset: 0x0010088B
		// (set) Token: 0x06003C5E RID: 15454 RVA: 0x00101893 File Offset: 0x00100893
		[MonitoringDescription("ReadTimeout")]
		[DefaultValue(-1)]
		[Browsable(true)]
		public int ReadTimeout
		{
			get
			{
				return this.readTimeout;
			}
			set
			{
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("ReadTimeout", SR.GetString("ArgumentOutOfRange_Timeout"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.ReadTimeout = value;
				}
				this.readTimeout = value;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06003C5F RID: 15455 RVA: 0x001018CD File Offset: 0x001008CD
		// (set) Token: 0x06003C60 RID: 15456 RVA: 0x001018D8 File Offset: 0x001008D8
		[MonitoringDescription("ReceivedBytesThreshold")]
		[Browsable(true)]
		[DefaultValue(1)]
		public int ReceivedBytesThreshold
		{
			get
			{
				return this.receivedBytesThreshold;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("ReceivedBytesThreshold", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
				}
				this.receivedBytesThreshold = value;
				if (this.IsOpen)
				{
					SerialDataReceivedEventArgs e = new SerialDataReceivedEventArgs(SerialData.Chars);
					this.CatchReceivedEvents(this, e);
				}
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06003C61 RID: 15457 RVA: 0x0010191C File Offset: 0x0010091C
		// (set) Token: 0x06003C62 RID: 15458 RVA: 0x0010193D File Offset: 0x0010093D
		[Browsable(true)]
		[MonitoringDescription("RtsEnable")]
		[DefaultValue(false)]
		public bool RtsEnable
		{
			get
			{
				if (this.IsOpen)
				{
					this.rtsEnable = this.internalSerialStream.RtsEnable;
				}
				return this.rtsEnable;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.RtsEnable = value;
				}
				this.rtsEnable = value;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06003C63 RID: 15459 RVA: 0x0010195A File Offset: 0x0010095A
		// (set) Token: 0x06003C64 RID: 15460 RVA: 0x00101962 File Offset: 0x00100962
		[Browsable(true)]
		[MonitoringDescription("StopBits")]
		[DefaultValue(StopBits.One)]
		public StopBits StopBits
		{
			get
			{
				return this.stopBits;
			}
			set
			{
				if (value < StopBits.One || value > StopBits.OnePointFive)
				{
					throw new ArgumentOutOfRangeException("StopBits", SR.GetString("ArgumentOutOfRange_Enum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.StopBits = value;
				}
				this.stopBits = value;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06003C65 RID: 15461 RVA: 0x0010199C File Offset: 0x0010099C
		// (set) Token: 0x06003C66 RID: 15462 RVA: 0x001019A4 File Offset: 0x001009A4
		[Browsable(true)]
		[DefaultValue(2048)]
		[MonitoringDescription("WriteBufferSize")]
		public int WriteBufferSize
		{
			get
			{
				return this.writeBufferSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Cant_be_set_when_open", new object[]
					{
						"value"
					}));
				}
				this.writeBufferSize = value;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06003C67 RID: 15463 RVA: 0x001019EF File Offset: 0x001009EF
		// (set) Token: 0x06003C68 RID: 15464 RVA: 0x001019F7 File Offset: 0x001009F7
		[MonitoringDescription("WriteTimeout")]
		[Browsable(true)]
		[DefaultValue(-1)]
		public int WriteTimeout
		{
			get
			{
				return this.writeTimeout;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("WriteTimeout", SR.GetString("ArgumentOutOfRange_WriteTimeout"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.WriteTimeout = value;
				}
				this.writeTimeout = value;
			}
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x00101A34 File Offset: 0x00100A34
		public SerialPort(IContainer container)
		{
			container.Add(this);
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x00101AF8 File Offset: 0x00100AF8
		public SerialPort()
		{
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x00101BB5 File Offset: 0x00100BB5
		public SerialPort(string portName) : this(portName, 9600, Parity.None, 8, StopBits.One)
		{
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x00101BC6 File Offset: 0x00100BC6
		public SerialPort(string portName, int baudRate) : this(portName, baudRate, Parity.None, 8, StopBits.One)
		{
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x00101BD3 File Offset: 0x00100BD3
		public SerialPort(string portName, int baudRate, Parity parity) : this(portName, baudRate, parity, 8, StopBits.One)
		{
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x00101BE0 File Offset: 0x00100BE0
		public SerialPort(string portName, int baudRate, Parity parity, int dataBits) : this(portName, baudRate, parity, dataBits, StopBits.One)
		{
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x00101BF0 File Offset: 0x00100BF0
		public SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		{
			this.PortName = portName;
			this.BaudRate = baudRate;
			this.Parity = parity;
			this.DataBits = dataBits;
			this.StopBits = stopBits;
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x00101CD2 File Offset: 0x00100CD2
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x00101CDB File Offset: 0x00100CDB
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.IsOpen)
			{
				this.internalSerialStream.Flush();
				this.internalSerialStream.Close();
				this.internalSerialStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x00101D0C File Offset: 0x00100D0C
		public void DiscardInBuffer()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			this.internalSerialStream.DiscardInBuffer();
			this.readPos = (this.readLen = 0);
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x00101D4C File Offset: 0x00100D4C
		public void DiscardOutBuffer()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			this.internalSerialStream.DiscardOutBuffer();
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x00101D74 File Offset: 0x00100D74
		public static string[] GetPortNames()
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			string[] array = null;
			RegistryPermission registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\HARDWARE\\DEVICEMAP\\SERIALCOMM");
			registryPermission.Assert();
			try
			{
				registryKey = Registry.LocalMachine;
				registryKey2 = registryKey.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM", false);
				if (registryKey2 != null)
				{
					string[] valueNames = registryKey2.GetValueNames();
					array = new string[valueNames.Length];
					for (int i = 0; i < valueNames.Length; i++)
					{
						array[i] = (string)registryKey2.GetValue(valueNames[i]);
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			if (array == null)
			{
				array = new string[0];
			}
			return array;
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x00101E20 File Offset: 0x00100E20
		public void Open()
		{
			if (this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_already_open"));
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			this.internalSerialStream = new SerialStream(this.portName, this.baudRate, this.parity, this.dataBits, this.stopBits, this.readTimeout, this.writeTimeout, this.handshake, this.dtrEnable, this.rtsEnable, this.discardNull, this.parityReplace);
			this.internalSerialStream.SetBufferSizes(this.readBufferSize, this.writeBufferSize);
			this.internalSerialStream.ErrorReceived += this.CatchErrorEvents;
			this.internalSerialStream.PinChanged += this.CatchPinChangedEvents;
			this.internalSerialStream.DataReceived += this.CatchReceivedEvents;
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x00101F00 File Offset: 0x00100F00
		public int Read(byte[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			int num = 0;
			if (this.CachedBytesToRead >= 1)
			{
				num = Math.Min(this.CachedBytesToRead, count);
				Buffer.BlockCopy(this.inBuffer, this.readPos, buffer, offset, num);
				this.readPos += num;
				if (num == count)
				{
					if (this.readPos == this.readLen)
					{
						this.readPos = (this.readLen = 0);
					}
					return count;
				}
				if (this.BytesToRead == 0)
				{
					return num;
				}
			}
			this.readLen = (this.readPos = 0);
			int count2 = count - num;
			num += this.internalSerialStream.Read(buffer, offset + num, count2);
			this.decoder.Reset();
			return num;
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00102022 File Offset: 0x00101022
		public int ReadChar()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			return this.ReadOneChar(this.readTimeout);
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x00102048 File Offset: 0x00101048
		private int ReadOneChar(int timeout)
		{
			int num = 0;
			if (this.decoder.GetCharCount(this.inBuffer, this.readPos, this.CachedBytesToRead) != 0)
			{
				int num2 = this.readPos;
				do
				{
					this.readPos++;
				}
				while (this.decoder.GetCharCount(this.inBuffer, num2, this.readPos - num2) < 1);
				try
				{
					this.decoder.GetChars(this.inBuffer, num2, this.readPos - num2, this.oneChar, 0);
				}
				catch
				{
					this.readPos = num2;
					throw;
				}
				return (int)this.oneChar[0];
			}
			if (timeout != 0)
			{
				int tickCount = Environment.TickCount;
				for (;;)
				{
					int num3;
					if (timeout == -1)
					{
						num3 = this.internalSerialStream.ReadByte(-1);
					}
					else
					{
						if (timeout - num < 0)
						{
							break;
						}
						num3 = this.internalSerialStream.ReadByte(timeout - num);
						num = Environment.TickCount - tickCount;
					}
					this.MaybeResizeBuffer(1);
					this.inBuffer[this.readLen++] = (byte)num3;
					if (this.decoder.GetCharCount(this.inBuffer, this.readPos, this.readLen - this.readPos) >= 1)
					{
						goto Block_8;
					}
				}
				throw new TimeoutException();
				Block_8:
				this.decoder.GetChars(this.inBuffer, this.readPos, this.readLen - this.readPos, this.oneChar, 0);
				this.readLen = (this.readPos = 0);
				return (int)this.oneChar[0];
			}
			int num4 = this.internalSerialStream.BytesToRead;
			if (num4 == 0)
			{
				num4 = 1;
			}
			this.MaybeResizeBuffer(num4);
			this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, num4);
			if (this.ReadBufferIntoChars(this.oneChar, 0, 1, false) == 0)
			{
				throw new TimeoutException();
			}
			return (int)this.oneChar[0];
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00102224 File Offset: 0x00101224
		public int Read(char[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			return this.InternalRead(buffer, offset, count, this.readTimeout, false);
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x001022BC File Offset: 0x001012BC
		private int InternalRead(char[] buffer, int offset, int count, int timeout, bool countMultiByteCharsAsOne)
		{
			if (count == 0)
			{
				return 0;
			}
			int tickCount = Environment.TickCount;
			int bytesToRead = this.internalSerialStream.BytesToRead;
			this.MaybeResizeBuffer(bytesToRead);
			this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, bytesToRead);
			int charCount = this.decoder.GetCharCount(this.inBuffer, this.readPos, this.CachedBytesToRead);
			if (charCount > 0)
			{
				return this.ReadBufferIntoChars(buffer, offset, count, countMultiByteCharsAsOne);
			}
			if (timeout == 0)
			{
				throw new TimeoutException();
			}
			int maxByteCount = this.Encoding.GetMaxByteCount(count);
			int num;
			for (;;)
			{
				this.MaybeResizeBuffer(maxByteCount);
				this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, maxByteCount);
				num = this.ReadBufferIntoChars(buffer, offset, count, countMultiByteCharsAsOne);
				if (num > 0)
				{
					break;
				}
				if (timeout != -1 && timeout - (Environment.TickCount - tickCount) <= 0)
				{
					goto Block_6;
				}
			}
			return num;
			Block_6:
			throw new TimeoutException();
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x001023AC File Offset: 0x001013AC
		private int ReadBufferIntoChars(char[] buffer, int offset, int count, bool countMultiByteCharsAsOne)
		{
			int num = Math.Min(count, this.CachedBytesToRead);
			DecoderReplacementFallback decoderReplacementFallback = this.encoding.DecoderFallback as DecoderReplacementFallback;
			if (this.encoding.IsSingleByte && this.encoding.GetMaxCharCount(num) == num && decoderReplacementFallback != null && decoderReplacementFallback.MaxCharCount == 1)
			{
				this.decoder.GetChars(this.inBuffer, this.readPos, num, buffer, offset);
				this.readPos += num;
				if (this.readPos == this.readLen)
				{
					this.readPos = (this.readLen = 0);
				}
				return num;
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = this.readPos;
			do
			{
				int num5 = Math.Min(count - num3, this.readLen - this.readPos - num2);
				if (num5 <= 0)
				{
					break;
				}
				num2 += num5;
				num5 = this.readPos + num2 - num4;
				int charCount = this.decoder.GetCharCount(this.inBuffer, num4, num5);
				if (charCount > 0)
				{
					if (num3 + charCount > count && !countMultiByteCharsAsOne)
					{
						break;
					}
					int num6 = num5;
					do
					{
						num6--;
					}
					while (this.decoder.GetCharCount(this.inBuffer, num4, num6) == charCount);
					this.decoder.GetChars(this.inBuffer, num4, num6 + 1, buffer, offset + num3);
					num4 = num4 + num6 + 1;
				}
				num3 += charCount;
			}
			while (num3 < count && num2 < this.CachedBytesToRead);
			this.readPos = num4;
			if (this.readPos == this.readLen)
			{
				this.readPos = (this.readLen = 0);
			}
			return num3;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x00102538 File Offset: 0x00101538
		public int ReadByte()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (this.readLen != this.readPos)
			{
				return (int)this.inBuffer[this.readPos++];
			}
			this.decoder.Reset();
			return this.internalSerialStream.ReadByte();
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0010259C File Offset: 0x0010159C
		public string ReadExisting()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			byte[] array = new byte[this.BytesToRead];
			if (this.readPos < this.readLen)
			{
				Buffer.BlockCopy(this.inBuffer, this.readPos, array, 0, this.CachedBytesToRead);
			}
			this.internalSerialStream.Read(array, this.CachedBytesToRead, array.Length - this.CachedBytesToRead);
			Decoder decoder = this.Encoding.GetDecoder();
			int charCount = decoder.GetCharCount(array, 0, array.Length);
			int num = array.Length;
			if (charCount == 0)
			{
				Buffer.BlockCopy(array, 0, this.inBuffer, 0, array.Length);
				this.readPos = 0;
				this.readLen = array.Length;
				return "";
			}
			do
			{
				decoder.Reset();
				num--;
			}
			while (decoder.GetCharCount(array, 0, num) == charCount);
			this.readPos = 0;
			this.readLen = array.Length - (num + 1);
			Buffer.BlockCopy(array, num + 1, this.inBuffer, 0, array.Length - (num + 1));
			return this.Encoding.GetString(array, 0, num + 1);
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x001026A8 File Offset: 0x001016A8
		public string ReadLine()
		{
			return this.ReadTo(this.NewLine);
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x001026B8 File Offset: 0x001016B8
		public string ReadTo(string value)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[]
				{
					"value"
				}));
			}
			int tickCount = Environment.TickCount;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			char c = value[value.Length - 1];
			int bytesToRead = this.internalSerialStream.BytesToRead;
			this.MaybeResizeBuffer(bytesToRead);
			this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, bytesToRead);
			if (this.singleCharBuffer != null)
			{
				goto IL_C3;
			}
			this.singleCharBuffer = new char[this.maxByteCountForSingleChar];
			string result;
			try
			{
				for (;;)
				{
					IL_C3:
					int num2;
					if (this.readTimeout == -1)
					{
						num2 = this.InternalRead(this.singleCharBuffer, 0, 1, this.readTimeout, true);
					}
					else
					{
						if (this.readTimeout - num < 0)
						{
							break;
						}
						int tickCount2 = Environment.TickCount;
						num2 = this.InternalRead(this.singleCharBuffer, 0, 1, this.readTimeout - num, true);
						num += Environment.TickCount - tickCount2;
					}
					stringBuilder.Append(this.singleCharBuffer, 0, num2);
					if (c == this.singleCharBuffer[num2 - 1] && stringBuilder.Length >= value.Length)
					{
						bool flag = true;
						for (int i = 2; i <= value.Length; i++)
						{
							if (value[value.Length - i] != stringBuilder[stringBuilder.Length - i])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							goto Block_11;
						}
					}
				}
				throw new TimeoutException();
				Block_11:
				string text = stringBuilder.ToString(0, stringBuilder.Length - value.Length);
				if (this.readPos == this.readLen)
				{
					this.readPos = (this.readLen = 0);
				}
				result = text;
			}
			catch
			{
				byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
				if (bytes.Length > 0)
				{
					int cachedBytesToRead = this.CachedBytesToRead;
					byte[] array = new byte[cachedBytesToRead];
					if (cachedBytesToRead > 0)
					{
						Buffer.BlockCopy(this.inBuffer, this.readPos, array, 0, cachedBytesToRead);
					}
					this.readPos = 0;
					this.readLen = 0;
					this.MaybeResizeBuffer(bytes.Length + cachedBytesToRead);
					Buffer.BlockCopy(bytes, 0, this.inBuffer, this.readLen, bytes.Length);
					this.readLen += bytes.Length;
					if (cachedBytesToRead > 0)
					{
						Buffer.BlockCopy(array, 0, this.inBuffer, this.readLen, cachedBytesToRead);
						this.readLen += cachedBytesToRead;
					}
				}
				throw;
			}
			return result;
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x00102970 File Offset: 0x00101970
		public void Write(string text)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (text.Length == 0)
			{
				return;
			}
			byte[] bytes = this.encoding.GetBytes(text);
			this.internalSerialStream.Write(bytes, 0, bytes.Length, this.writeTimeout);
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x001029D0 File Offset: 0x001019D0
		public void Write(char[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			if (buffer.Length == 0)
			{
				return;
			}
			byte[] bytes = this.Encoding.GetBytes(buffer, offset, count);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x00102A70 File Offset: 0x00101A70
		public void Write(byte[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			if (buffer.Length == 0)
			{
				return;
			}
			this.internalSerialStream.Write(buffer, offset, count, this.writeTimeout);
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x00102B11 File Offset: 0x00101B11
		public void WriteLine(string text)
		{
			this.Write(text + this.NewLine);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x00102B28 File Offset: 0x00101B28
		private void CatchErrorEvents(object src, SerialErrorReceivedEventArgs e)
		{
			SerialErrorReceivedEventHandler errorReceived = this.ErrorReceived;
			SerialStream serialStream = this.internalSerialStream;
			if (errorReceived != null && serialStream != null)
			{
				lock (serialStream)
				{
					if (serialStream.IsOpen)
					{
						errorReceived(this, e);
					}
				}
			}
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x00102B7C File Offset: 0x00101B7C
		private void CatchPinChangedEvents(object src, SerialPinChangedEventArgs e)
		{
			SerialPinChangedEventHandler pinChanged = this.PinChanged;
			SerialStream serialStream = this.internalSerialStream;
			if (pinChanged != null && serialStream != null)
			{
				lock (serialStream)
				{
					if (serialStream.IsOpen)
					{
						pinChanged(this, e);
					}
				}
			}
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00102BD0 File Offset: 0x00101BD0
		private void CatchReceivedEvents(object src, SerialDataReceivedEventArgs e)
		{
			SerialDataReceivedEventHandler dataReceived = this.DataReceived;
			SerialStream serialStream = this.internalSerialStream;
			if (dataReceived != null && serialStream != null)
			{
				lock (serialStream)
				{
					bool flag = false;
					try
					{
						flag = (serialStream.IsOpen && (SerialData.Eof == e.EventType || this.BytesToRead >= this.receivedBytesThreshold));
					}
					catch
					{
					}
					finally
					{
						if (flag)
						{
							dataReceived(this, e);
						}
					}
				}
			}
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x00102C68 File Offset: 0x00101C68
		private void CompactBuffer()
		{
			Buffer.BlockCopy(this.inBuffer, this.readPos, this.inBuffer, 0, this.CachedBytesToRead);
			this.readLen = this.CachedBytesToRead;
			this.readPos = 0;
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x00102C9C File Offset: 0x00101C9C
		private void MaybeResizeBuffer(int additionalByteLength)
		{
			if (additionalByteLength + this.readLen <= this.inBuffer.Length)
			{
				return;
			}
			if (this.CachedBytesToRead + additionalByteLength <= this.inBuffer.Length / 2)
			{
				this.CompactBuffer();
				return;
			}
			int num = Math.Max(this.CachedBytesToRead + additionalByteLength, this.inBuffer.Length * 2);
			byte[] dst = new byte[num];
			Buffer.BlockCopy(this.inBuffer, this.readPos, dst, 0, this.CachedBytesToRead);
			this.readLen = this.CachedBytesToRead;
			this.readPos = 0;
			this.inBuffer = dst;
		}

		// Token: 0x04003521 RID: 13601
		public const int InfiniteTimeout = -1;

		// Token: 0x04003522 RID: 13602
		private const int defaultDataBits = 8;

		// Token: 0x04003523 RID: 13603
		private const Parity defaultParity = Parity.None;

		// Token: 0x04003524 RID: 13604
		private const StopBits defaultStopBits = StopBits.One;

		// Token: 0x04003525 RID: 13605
		private const Handshake defaultHandshake = Handshake.None;

		// Token: 0x04003526 RID: 13606
		private const int defaultBufferSize = 1024;

		// Token: 0x04003527 RID: 13607
		private const string defaultPortName = "COM1";

		// Token: 0x04003528 RID: 13608
		private const int defaultBaudRate = 9600;

		// Token: 0x04003529 RID: 13609
		private const bool defaultDtrEnable = false;

		// Token: 0x0400352A RID: 13610
		private const bool defaultRtsEnable = false;

		// Token: 0x0400352B RID: 13611
		private const bool defaultDiscardNull = false;

		// Token: 0x0400352C RID: 13612
		private const byte defaultParityReplace = 63;

		// Token: 0x0400352D RID: 13613
		private const int defaultReceivedBytesThreshold = 1;

		// Token: 0x0400352E RID: 13614
		private const int defaultReadTimeout = -1;

		// Token: 0x0400352F RID: 13615
		private const int defaultWriteTimeout = -1;

		// Token: 0x04003530 RID: 13616
		private const int defaultReadBufferSize = 4096;

		// Token: 0x04003531 RID: 13617
		private const int defaultWriteBufferSize = 2048;

		// Token: 0x04003532 RID: 13618
		private const int maxDataBits = 8;

		// Token: 0x04003533 RID: 13619
		private const int minDataBits = 5;

		// Token: 0x04003534 RID: 13620
		private const string defaultNewLine = "\n";

		// Token: 0x04003535 RID: 13621
		private const string SERIAL_NAME = "\\Device\\Serial";

		// Token: 0x04003536 RID: 13622
		private int baudRate = 9600;

		// Token: 0x04003537 RID: 13623
		private int dataBits = 8;

		// Token: 0x04003538 RID: 13624
		private Parity parity;

		// Token: 0x04003539 RID: 13625
		private StopBits stopBits = StopBits.One;

		// Token: 0x0400353A RID: 13626
		private string portName = "COM1";

		// Token: 0x0400353B RID: 13627
		private Encoding encoding = Encoding.ASCII;

		// Token: 0x0400353C RID: 13628
		private Decoder decoder = Encoding.ASCII.GetDecoder();

		// Token: 0x0400353D RID: 13629
		private int maxByteCountForSingleChar = Encoding.ASCII.GetMaxByteCount(1);

		// Token: 0x0400353E RID: 13630
		private Handshake handshake;

		// Token: 0x0400353F RID: 13631
		private int readTimeout = -1;

		// Token: 0x04003540 RID: 13632
		private int writeTimeout = -1;

		// Token: 0x04003541 RID: 13633
		private int receivedBytesThreshold = 1;

		// Token: 0x04003542 RID: 13634
		private bool discardNull;

		// Token: 0x04003543 RID: 13635
		private bool dtrEnable;

		// Token: 0x04003544 RID: 13636
		private bool rtsEnable;

		// Token: 0x04003545 RID: 13637
		private byte parityReplace = 63;

		// Token: 0x04003546 RID: 13638
		private string newLine = "\n";

		// Token: 0x04003547 RID: 13639
		private int readBufferSize = 4096;

		// Token: 0x04003548 RID: 13640
		private int writeBufferSize = 2048;

		// Token: 0x04003549 RID: 13641
		private SerialStream internalSerialStream;

		// Token: 0x0400354A RID: 13642
		private byte[] inBuffer = new byte[1024];

		// Token: 0x0400354B RID: 13643
		private int readPos;

		// Token: 0x0400354C RID: 13644
		private int readLen;

		// Token: 0x0400354D RID: 13645
		private char[] oneChar = new char[1];

		// Token: 0x0400354E RID: 13646
		private char[] singleCharBuffer;
	}
}
