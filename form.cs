        private SerialPort comPort;
        private string response;
        public Thread ReadDataFromSerial;

        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxCOMPORT.Items.AddRange(ports);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                comPort = new SerialPort();
                comPort.PortName = cBoxCOMPORT.Text;
                comPort.BaudRate = Convert.ToInt32(cBoxBAUDRATE.Text);
                comPort.DataBits = Convert.ToInt32(cBoxDATABITS.Text);
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxSTOPBITS.Text);
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxPARITYBITS.Text); ;
                comPort.Open();
                progressBar1.Value = 100;
                btnOpen.Enabled = false;


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                MessageBox.Show(exception.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (comPort.IsOpen)
            {
                comPort.Close();
                progressBar1.Value = 0;
                tBoxDataOut.Clear();
                btnSendData.Enabled = true;
                btnOpen.Enabled = true;
            }
           
        }
       
        private void btnSendData_Click(object sender, EventArgs e)
        {
            
            if (comPort.IsOpen)
            {
                
                ReadSerialData();
                btnSendData.Enabled = false;
                btnOpen.Enabled = false;

            }
        }
        
        private void ReadSerialData()
        {
            try
            {
                ReadDataFromSerial = new Thread(ReadSerial);
                ReadDataFromSerial.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show(@"read serial thread " + e.Message);
            }

        }

        private void ReadSerial()
        {
            while (comPort.IsOpen)
            {
                try
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        comPort.Write((SesnorTypeComboBox.SelectedIndex + 1).ToString());
                        Debug.WriteLine(SesnorTypeComboBox.SelectedIndex + 1);
                    }));
                    response = comPort.ReadLine();
                    DisplayDataFromSerial(response);
                    Thread.Sleep(500);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private delegate void DisplayDataFromSerialDelegate(string re);

        private void DisplayDataFromSerial(string response)
        {
            if (tBoxDataOut.InvokeRequired)
            {
                DisplayDataFromSerialDelegate displayDataFromSerialDelegate = DisplayDataFromSerial;
                Invoke(displayDataFromSerialDelegate, response);
            }
            else
            {
                tBoxDataOut.Text = response;
            }
        }
