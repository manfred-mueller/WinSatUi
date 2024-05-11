using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace WinSatUi
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public string appName = Properties.Resources.SystemRatingTool;
        
        private Task asyncTest()
        {
            return Task.Factory.StartNew(() =>
            {
                startTest();
            });
        }

        public void startTest()
        {
            Process process = new Process();
            process.StartInfo.FileName = "winsat.exe";
            process.StartInfo.Arguments = "formal";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();

        }
        private void findSystem()
        {
            string subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey skey = key.OpenSubKey(subKey);

            string verString = skey.GetValue("CurrentBuildNumber").ToString();
            int verint = int.Parse(verString);
            buildLabel.Text = "Build: " + verString;

            if (verint == 7600 || verint == 7601)
            {
                versionBox.Image = Properties.Resources.windows7;
            }
            else if (verint == 9200)
            {
                versionBox.Image = Properties.Resources.windows8;
            }
            else if (verint == 9600)
            {
                versionBox.Image = Properties.Resources.windows81;
            }
            else if (verint >= 10240 && verint < 22000)
            {
                versionBox.Image = Properties.Resources.windows10;
            }
            else if (verint >= 22000)
            {
                versionBox.Image = Properties.Resources.windows11;
            }
            else
            {
                MessageBox.Show(Properties.Resources.UnknownOperatingSystem);
                Application.Exit();
            }
        }

        private void XML()
        {
            string path=@"C:\Windows\Performance\WinSAT\DataStore\";
            string highDir = "";
            DateTime lastHigh = new DateTime(1900, 1, 1);

            foreach (string subdir in Directory.GetFiles(path, "*.xml"))
            {
                FileInfo fi = new FileInfo(subdir);
                DateTime created = fi.LastWriteTime;

                if (subdir.IndexOf("Formal.Assessment") != -1)
                {
                    if (created > lastHigh)
                    {
                        highDir = subdir;
                        lastHigh = created;
                    }
                }                
            }
            

            XmlDocument xml = new XmlDocument();
            xml.Load(highDir);
            XmlNodeList xnList = xml.SelectNodes("/WinSAT/WinSPR");
            foreach (XmlNode xnNode in xnList)
            {
                cpuValLabel.Text = xnNode["CpuScore"].InnerText.ToString();
                memValLabel.Text = xnNode["MemoryScore"].InnerText.ToString();
                graphicsValLabel.Text = xnNode["GraphicsScore"].InnerText.ToString();
                gameValLabel.Text = xnNode["GamingScore"].InnerText.ToString();
                diskValLabel.Text = xnNode["DiskScore"].InnerText.ToString();
                valueBox.Text = WinSatUi.Properties.Resources.ScoresFrom + lastHigh.ToString();
            }

        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.WinSatUi;
            System.Management.SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_ComputerSystem");
            using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    process.Get();
                    this.Text = Properties.Resources.SystemRatingTool + process["Model"];
                }
            }
            stopButton.Enabled = false;
            findSystem();             
            metroProgressSpinner1.Value = 40;
            valueBox.Focus();
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            recentButton.Enabled = false;
            stopButton.Enabled = true;
            metroProgressSpinner1.Visible = true;
            metroProgressSpinner1.Enabled = true;
            valueBox.Enabled = false;
            await asyncTest();
            metroProgressSpinner1.Visible = false;
            valueBox.Enabled = true;
            startButton.Enabled = true;
            recentButton.Enabled = true;
            stopButton.Enabled = false;
            XML();
        }

        private void recentButton_Click(object sender, EventArgs e)
        {
            XML();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("WinSAT");
            foreach (var process in processes)
            {
                process.Kill();
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("WinSAT");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }


    }
}
