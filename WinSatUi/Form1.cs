using System.Collections.Specialized;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WinSatUi
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public string appName = Properties.Resources.SystemRatingTool;
        public string testDate = "";
        public string idImgur = Properties.Settings.Default.ClientID;
        public NotifyIcon notifyIcon;

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
            string path = @"C:\Windows\Performance\WinSAT\DataStore\";
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
            if (!string.IsNullOrWhiteSpace(highDir))
            {
                xml.Load(highDir);
                XmlNodeList xnList = xml.SelectNodes("/WinSAT/WinSPR");
                foreach (XmlNode xnNode in xnList)
                {
                    testDate = lastHigh.ToString();
                    cpuValLabel.Text = xnNode["CpuScore"].InnerText.ToString();
                    memValLabel.Text = xnNode["MemoryScore"].InnerText.ToString();
                    graphicsValLabel.Text = xnNode["GraphicsScore"].InnerText.ToString();
                    gameValLabel.Text = xnNode["GamingScore"].InnerText.ToString();
                    diskValLabel.Text = xnNode["DiskScore"].InnerText.ToString();
                    valueBox.Text = Properties.Resources.ScoresFrom + testDate;
                }
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
            XML();
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            stopButton.Enabled = true;
            metroProgressSpinner1.Visible = true;
            metroProgressSpinner1.Enabled = true;
            valueBox.Enabled = false;
            await asyncTest();
            metroProgressSpinner1.Visible = false;
            valueBox.Enabled = true;
            startButton.Enabled = true;
            stopButton.Enabled = false;
            XML();
        }

        private void TakeScreenshot(string screenshotName)
        {
            Rectangle bounds = this.Bounds;
            bounds.Height = Math.Min(bounds.Height, 315);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                string picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                string screenshotPath = Path.Combine(picturesFolder, screenshotName.Replace(".", "").Replace(":", "").Replace(" ", "_").Replace("jpg", ".jpg"));
                bitmap.Save(screenshotPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                showBalloon(Properties.Resources.ScreenshotSavedAt, screenshotPath);

                DialogResult result = MessageBox.Show(WinSatUi.Properties.Resources.DoYouWantToShareTheValuesViaImgur, WinSatUi.Properties.Resources.ShareViaImgur, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (imgurLink(screenshotPath))
                    {
                        Process.Start(Properties.Settings.Default.imgUrl);
                    }
                    else
                    {
                        showBalloon(WinSatUi.Properties.Resources.FailedToUploadToImgur, " ");
                    }
                }
            }
        }

        private bool uploadValues(WebClient w, string idImgur)
        {
            try
            {
                w.Headers.Add("Authorization", "Client-ID " + idImgur);
                return true;
            }
            catch (Exception ex)
            {
                showBalloon(WinSatUi.Properties.Resources.Error, ex.Message);
                return false;
            }
        }

        public bool imgurLink(string localFile)
        {
            string sLink = string.Empty;
            string sHash = string.Empty;
            var values = new NameValueCollection {
                { "image", Convert.ToBase64String(File.ReadAllBytes(localFile)) },
                { "title", Path.GetFileNameWithoutExtension(localFile) }
            };

            using (var w = new WebClient())
            {
                if (uploadValues(w, idImgur))
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(new MemoryStream(w.UploadValues(new Uri("https://api.imgur.com/3/upload.xml"), values))))
                            sLink = sr.ReadToEnd();

                        if (!string.IsNullOrEmpty(sLink))
                        {
                            sHash = new Regex(@"<deletehash>(.*?)</deletehash>", RegexOptions.Multiline).Match(sLink).Groups[1].Value.Trim();
                            sLink = new Regex(@"<link>(.*?)</link>", RegexOptions.Multiline).Match(sLink).Groups[1].Value.Trim();
                            Properties.Settings.Default.imgUrl = sLink;
                            Properties.Settings.Default.deleteUrl = string.Format("http://imgur.com/delete/" + sHash);
                            Properties.Settings.Default.Save();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        showBalloon(WinSatUi.Properties.Resources.Error, ex.Message);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        private void cameraButton_Click(object sender, EventArgs e)
        {
            TakeScreenshot("screenshot-" + testDate + ".jpg");
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
        private void showBalloon(string title, string body)
        {
            if (notifyIcon == null)
            {
                notifyIcon = new NotifyIcon();
                notifyIcon.Icon = SystemIcons.Exclamation;
                notifyIcon.Visible = true;
            }

            if (title != null)
            {
                notifyIcon.BalloonTipTitle = title;
            }

            if (body != null)
            {
                notifyIcon.BalloonTipText = body;
            }

            notifyIcon.ShowBalloonTip(5000);
        }
    }
}
