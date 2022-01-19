/**
** better approach for the same reason but at this time
**
** with background worker!
** YOU MUST PUT IN THE FORM A BACKGROUNDWORKER    
**/


private void btnClose_Click(object sender, EventArgs e)
        {
            if (comPort.IsOpen)
            {
                if (backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                }
                comPort.Close();
                progressBar1.Value = 0;
                tBoxDataOut.Clear();
                btnSendData.Enabled = true;
                //MessageBox.Show("The connection is closed");
                btnOpen.Enabled = true;
            }
           
        }
       
        private void btnSendData_Click(object sender, EventArgs e)
        {
            
            if (comPort.IsOpen)
            {
                
                //ReadSerialData();
                btnSendData.Enabled = false;
                btnOpen.Enabled = false;
                
                /*
                * THIS CODE BELOW WILL NEVER REACH BECAUSE THE btnOpen.Enabled -> false;
                *
                */
                
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(@"Busy!!");
                }

            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadSerial();
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                backgroundWorker1.ReportProgress(0);
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
                    Invoke(new MethodInvoker(() =>
                    {
                        tBoxDataOut.Text = response;
                    }));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
