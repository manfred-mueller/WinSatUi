using System.Collections.Specialized;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Task = System.Threading.Tasks.Task;
using System.Management;

namespace WinSatUi
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public string appName = Properties.Resources.SystemRatingTool;
        public string testDate = "";
        public string processor;
        public string memoryString;
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
                // Parse processor model.
                string processor =
                    ParseXmlData(
                        "WinSAT/SystemConfig/Processor/Instance",
                        "ProcessorName",
                        highDir);
                // Parse memory size.
                string memory =
                    ParseXmlData(
                        "WinSAT/SystemConfig/Memory/DIMM",
                        "MemoryType",
                        highDir);
                if (memory.Equals("22"))
                {
                    memoryString = "DDR2";
                }
                else if (memory.Equals("24"))
                {
                    memoryString = "DDR3";
                }
                else if (memory.Equals("26"))
                {
                    memoryString = "DDR4";
                }
                else if (memory.Equals("28"))
                {
                    memoryString = "DDR5";
                }

                // Parse memory size.
                string memorySizeString =
                    ParseXmlData(
                        "WinSAT/SystemConfig/Memory/TotalPhysical",
                        "Size",
                        highDir);

                long memorySize;

                if (!long.TryParse(memorySizeString, out memorySize))
                    memorySize = 0;

                // Parse graphics card model.
                string graphics =
                    ParseXmlData(
                        "WinSAT/SystemConfig/Graphics",
                        "AdapterDescription",
                        highDir);

                // Parse VRAM size.
                string vramSizeMegaBytes =
                    ParseXmlData(
                        "WinSAT/SystemConfig/Graphics",
                        "DedicatedVideoMemory",
                        highDir);

                long vramSize;

                if (!long.TryParse(vramSizeMegaBytes, out vramSize))
                    vramSize = 0;
                int myVramSize = (int)(vramSize / 1024 / 1024);

                // Parse disk model.
                string disk =
                    ParseXmlData(
                        "WinSAT/SystemConfig/Disk/SystemDisk",
                        "Model",
                        highDir);

                // Parse disk model.
                string diskSizeMegaBytes =
                    ParseXmlData(
                        "WinSAT/SystemConfig/Disk/SystemDisk",
                        "Size",
                        highDir);
                long diskSize;
                if (!long.TryParse(diskSizeMegaBytes, out diskSize))
                    diskSize = 0;
                int myDiskSize = (int)(diskSize / 1024 / 1024 / 1024);
                string messageText = string.Format("CPU: {0}\nMemory: {1} {2}\nGraphics: {3}, {4} MB VRAM\nDisk: {5} {6} {7}GB\n", processor, memorySizeString, memoryString, graphics, myVramSize, detect_primary_disk(), disk, myDiskSize);
                MessageBox.Show(messageText, "Systeminformation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        internal static string ParseXmlData(string nodeName, string innerNodeName, string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode node;

                xmlDoc.Load(filePath);

                node = xmlDoc.SelectSingleNode(nodeName);

                if (node != null)
                {
                    XmlNode innerNode =
                        node.SelectSingleNode(
                            innerNodeName);

                    return innerNode != null
                        ? innerNode.InnerText
                        : null;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string detect_primary_disk()
        {
            // Retrieve information about all disk drives
            ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject disk in diskSearcher.Get())
            {
                string diskId = disk["DeviceID"].ToString();

                // Check if the disk contains the system directory
                ManagementObjectSearcher partitionSearcher = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + diskId + "'} WHERE ResultClass=Win32_DiskPartition");

                foreach (ManagementObject partition in partitionSearcher.Get())
                {
                    ManagementObjectSearcher logicalDiskSearcher = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE ResultClass=Win32_LogicalDisk");

                    foreach (ManagementObject logicalDisk in logicalDiskSearcher.Get())
                    {
                        string driveType = logicalDisk["DriveType"].ToString();
                        string driveLetter = logicalDisk["DeviceID"].ToString();

                        // DriveType 3 represents the local disk
                        if (driveType == "3" && System.IO.Path.GetPathRoot(Environment.SystemDirectory) == driveLetter)
                        {
                            // Return the type of the disk based on its rotation speed
                            if (disk["RotationSpeed"] != null)
                                return "HDD";
                            else
                                return "SSD/NVMe";
                        }
                    }
                }
            }

            // If no disk is found containing the system directory, return SSD/NVMe by default
            return "SSD/NVMe";
        }
    }
}
