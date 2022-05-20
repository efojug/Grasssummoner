using Microsoft.Win32;
using Panuon.UI.Silver;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace Grasssummoner
{
    public partial class MainWindow : WindowX
    {

        public MainWindow()
        {
            InitializeComponent();
            string path = @"grasssummoner.config";
            if (File.Exists(path))
            {
                try
                {
                    string[] contents = File.ReadAllLines(path);
                    GameDir.Text = contents[0];
                    ProxyDir.Text = contents[1];
                    ServerDir.Text = contents[2];
                    IPAdress.Text = contents[3];
                    IPPort.Text = contents[4];
                }
                catch { }
            }
            else
            {
                try
                {
                    FileStream newConfig = new FileStream(path, FileMode.Create);
                    newConfig.Close();
                }
                catch (Exception ex)
                {
                    caonima();
                    MessageBoxX.Show(ex.Message, "错误");
                }
            }
        }

        private void caonima()
        {
            fuck.Visibility = Visibility.Visible;
        }

        private void SelectGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog SelectGameDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "请选择游戏本体文件",
                InitialDirectory = "C:\\",
                Filter = "应用程序|*.exe",
                DereferenceLinks = true
            };
            if ((bool)SelectGameDialog.ShowDialog())
            {
                GameDir.Text = SelectGameDialog.FileName;
                return;
            }
        }

        private void SelectProxy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog SelectProxyDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "请选择数据抓取脚本",
                InitialDirectory = "C:\\",
                Filter = "Python文件|*.py",
                DereferenceLinks = true
            };
            if ((bool)SelectProxyDialog.ShowDialog())
            {
                ProxyDir.Text = SelectProxyDialog.FileName;
                return;
            }
        }

        private void SelectGrasscutter_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog SelectServerDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "请选择服务器主体文件",
                InitialDirectory = "C:\\",
                Filter = "Jar文件|*.jar",
                DereferenceLinks = true
            };
            if ((bool)SelectServerDialog.ShowDialog())
            {
                ServerDir.Text = SelectServerDialog.FileName;
                return;
            }
        }

        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter config = new StreamWriter("grasssummoner.config"))
                {
                    config.WriteLine(GameDir.Text.ToString());
                    config.WriteLine(ProxyDir.Text.ToString());
                    config.WriteLine(ServerDir.Text.ToString());
                    config.WriteLine(IPAdress.Text.ToString());
                    config.WriteLine(IPPort.Text.ToString());
                    config.Close();

                    MessageBoxX.Show("保存完成", "提示");
                }
            }
            catch (Exception ex)
            {
                caonima();
                MessageBoxX.Show(ex.Message, "错误");
            }
        }

        private void LoadConfig_Click(object sender, RoutedEventArgs e)
        {
            string path = @"grasssummoner.config";
            if (File.Exists(path))
            {
                try
                {
                    string[] contents = File.ReadAllLines(path);
                    GameDir.Text = contents[0];
                    ProxyDir.Text = contents[1];
                    ServerDir.Text = contents[2];
                    IPAdress.Text = contents[3];
                    IPPort.Text = contents[4];
                    MessageBoxX.Show("加载完成", "提示");
                }
                catch (Exception sb)
                {
                    MessageBoxX.Show("加载失败：通常因为编码错误导致乱码或配置文件为空，错误信息:\n" + sb.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("未找到配置文件，请先保存", "错误");
            }
        }

        private void OnlyStartMitmproxy_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(ProxyDir.Text.ToString()) && File.Exists(ProxyDir.Text.ToString()))
            {
                try
                {
                    ProcessStartInfo StartMitmProxy = new ProcessStartInfo
                    {
                        FileName = @"mitmdump",
                        Arguments = @" -s " + ProxyDir.Text.ToString() + " --ssl-insecure --set block_global=false --listen-port 8080",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    Process.Start(StartMitmProxy);
                }
                catch (Exception wcnmsl)
                {
                    MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("请正确填写proxy.py文件路径");
            }
        }

        private void OnlyStartServer_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(ServerDir.Text.ToString()) && File.Exists(ServerDir.Text.ToString()))
            {
                try
                {
                    ProcessStartInfo StartServer = new ProcessStartInfo
                    {
                        FileName = @"java",
                        Arguments = @" -jar " + ServerDir.Text.ToString(),
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    Process.Start(StartServer);
                }
                catch (Exception wcnmsl)
                {
                    caonima();
                    MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("请正确填写grasscutter.jar路径");
            }
        }

        private void OnlyStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(GameDir.Text.ToString()) && File.Exists(GameDir.Text.ToString()))
            {
                try
                {
                    Process.Start(GameDir.Text.ToString());
                }
                catch (Exception wcnmsl)
                {
                    caonima();
                    MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("请正确填写游戏文件路径");
            }
        }
        private void StartAll_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ServerDir.Text.ToString()) && !string.IsNullOrEmpty(GameDir.Text) && !string.IsNullOrEmpty(ProxyDir.Text.ToString()))
            {
                if (File.Exists(ServerDir.Text.ToString()) && File.Exists(ProxyDir.Text.ToString()) && File.Exists(GameDir.Text.ToString()))
                {
                    try
                    {
                        ProcessStartInfo StartMitmProxy = new ProcessStartInfo
                        {
                            FileName = @"mitmdump",
                            Arguments = @" -s " + ProxyDir.Text.ToString() + " --ssl-insecure --set block_global=false --listen-port 8080",
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        ProcessStartInfo OnlyStartServer = new ProcessStartInfo
                        {
                            FileName = @"java",
                            Arguments = @" -jar " + ServerDir.Text.ToString(),
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        Process.Start(StartMitmProxy);
                        Thread.Sleep(500);
                        Process.Start(OnlyStartServer);
                        Thread.Sleep(3500);
                        Process.Start(GameDir.Text.ToString());
                    }
                    catch (Exception wcnmsl)
                    {
                        caonima();
                        MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                    }
                }
                else
                {
                    MessageBoxX.Show("文件路径未填写");
                }
            }
            else
            {
                MessageBoxX.Show("找不到文件");
            }
        }

        private void ServerMode_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ServerDir.Text.ToString()) && !string.IsNullOrEmpty(ProxyDir.Text.ToString()))
            {
                if(File.Exists(ServerDir.Text.ToString()) && File.Exists(ProxyDir.Text.ToString()))
                {
                    try
                    {
                        ProcessStartInfo StartMitmProxy = new ProcessStartInfo
                        {
                            FileName = @"mitmdump",
                            Arguments = @" -s " + ProxyDir.Text.ToString() + " --ssl-insecure --set block_global=false --listen-port 8080",
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        ProcessStartInfo OnlyStartServer = new ProcessStartInfo
                        {
                            FileName = @"java",
                            Arguments = @" -jar " + ServerDir.Text.ToString(),
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        Process.Start(StartMitmProxy);
                        Thread.Sleep(500);
                        Process.Start(OnlyStartServer);
                    }
                    catch (Exception wcnmsl)
                    {
                        caonima();
                        MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                    }
                }
                else
                {
                    MessageBoxX.Show("文件路径未填写");
                }
            }
            else
            {
                MessageBoxX.Show("找不到文件");
            }
        }
        private void fuck_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Show("装mongodb的时候不要选Install mongodb Compass\n 一键启动找不到指定的文件重装mitmproxy和java\n 服务端最后一行报Cluster description not yet avaiable. Waitinng for 30000 ms before time out的重装mongodb\n proxy.py里面报任何错都不要管，只有服务端报错才有用", "emotional damage");
        }

        private void VPN_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IPAdress.Text.ToString()) && !string.IsNullOrEmpty(IPPort.Text.ToString()))
            {
                try
                {
                    ProcessStartInfo EnableProxy = new ProcessStartInfo
                    {
                        FileName = @"cmd",
                        Arguments = @" /c reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings"" /v ProxyEnable /t REG_DWORD /d 1 /f",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    ProcessStartInfo SetProxyServer = new ProcessStartInfo
                    {
                        FileName = @"cmd",
                        Arguments = @" /c reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings"" /v ProxyServer /d " + @"""" + IPAdress.Text.ToString() + ":" + IPPort.Text.ToString() + @"""" + " /f",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    ProcessStartInfo SetProxyOverride = new ProcessStartInfo
                    {
                        FileName = @"cmd",
                        Arguments = @" /c reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings"" /v ProxyOverride /t REG_SZ /d """" /f",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };
                    Process.Start(EnableProxy);
                    Process.Start(SetProxyServer);
                    Process.Start(SetProxyOverride);
                }
                catch (Exception ex)
                {
                    
                    MessageBoxX.Show("代理开启失败，错误信息：\n" + ex.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("请填写IP地址和端口");
            }
        }

        private void VPN_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo DisableProxy = new ProcessStartInfo
                {
                    FileName = @"cmd",
                    Arguments = @" /c reg add ""HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings"" /v ProxyEnable /t REG_DWORD /d 0 /f",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };
                Process.Start(DisableProxy);
            }
            catch (Exception sb)
            {
                MessageBoxX.Show("关闭失败，错误信息：\n" + sb.ToString());
            }
        }
    }
}
