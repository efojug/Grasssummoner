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
                    MessageBoxX.Show(ex.Message, "错误");
                }
            }
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
                    config.Close();

                    MessageBoxX.Show("保存完成", "提示");
                }
            }
            catch (Exception ex)
            {
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
                    MessageBoxX.Show("加载完成", "提示");
                }
                catch { }
            }
            else
            {
                MessageBoxX.Show("未找到配置文件，请先保存", "错误");
            }
        }

        private void OnlyStartMitmproxy_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(ProxyDir.Text.ToString()))
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
                MessageBoxX.Show("请填写proxy.py文件路径");
            }
            return;
        }

        private void OnlyStartServer_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(ServerDir.Text))
            {
                try
                {
                    ProcessStartInfo StartServer = new ProcessStartInfo
                    {
                        FileName = @"java",
                        Arguments = @" -jar " + ServerDir.Text.ToString(),
                        //FileName = ServerDir.Text,
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    Process.Start(StartServer);
                }
                catch (Exception wcnmsl)
                {
                    MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("请填写grasscutter.jar路径");
            }
            return;
        }

        private void OnlyStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(GameDir.Text.ToString()))
            {
                try
                {
                    Process.Start(GameDir.Text.ToString());
                }
                catch (Exception wcnmsl)
                {
                    MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("请填写游戏文件路径");
            }
            return;
        }
        private void StartAll_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ServerDir.Text.ToString()) && !string.IsNullOrEmpty(GameDir.Text) && !string.IsNullOrEmpty(ProxyDir.Text.ToString()))
            {
                try
                {
                    MessageBoxX.Show("请确认已开启系统代理", "注意");
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
                    MessageBoxX.Show("启动失败，错误信息:\n"+wcnmsl.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("文件路径填写错误或未填写");
            }
        }

        private void ServerMode_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ServerDir.Text.ToString()) && !string.IsNullOrEmpty(ProxyDir.Text.ToString()))
            {
                try
                {
                    MessageBoxX.Show("请确认已开启系统代理", "注意");
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
                    MessageBoxX.Show("启动失败，错误信息:\n" + wcnmsl.ToString());
                }
            }
            else
            {
                MessageBoxX.Show("文件路径填写错误或未填写");
            }
        }
    }
}
