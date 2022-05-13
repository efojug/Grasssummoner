using Microsoft.Win32;
using Panuon.UI.Silver;
using System;
using System.IO;
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
            OpenFileDialog SelectGameDialog = new OpenFileDialog();
            SelectGameDialog.Multiselect = false;
            SelectGameDialog.Title = "请选择游戏本体文件";
            SelectGameDialog.InitialDirectory = "C:\\";
            SelectGameDialog.Filter = "应用程序|*.exe";
            SelectGameDialog.DereferenceLinks = true;
            if ((bool)SelectGameDialog.ShowDialog())
            {
                GameDir.Text = SelectGameDialog.FileName;
                return;
            }
        }

        private void SelectProxy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog SelectProxyDialog = new OpenFileDialog();
            SelectProxyDialog.Multiselect = false;
            SelectProxyDialog.Title = "请选择数据抓取脚本";
            SelectProxyDialog.InitialDirectory = "C:\\";
            SelectProxyDialog.Filter = "Python文件|*.py";
            SelectProxyDialog.DereferenceLinks = true;
            if ((bool)SelectProxyDialog.ShowDialog())
            {
                ProxyDir.Text = SelectProxyDialog.FileName;
                return;
            }
        }

        private void SelectGrasscutter_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog SelectServerDialog = new OpenFileDialog();
            SelectServerDialog.Multiselect = false;
            SelectServerDialog.Title = "请选择服务器主体文件";
            SelectServerDialog.InitialDirectory = "C:\\";
            SelectServerDialog.Filter = "Jar文件|*.jar";
            SelectServerDialog.DereferenceLinks = true;
            if ((bool)SelectServerDialog.ShowDialog())
            {
                ServerDir.Text = SelectServerDialog.FileName;
                return;
            }
        }

        private void saveconfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter config = new StreamWriter("grasssummoner.config"))
                {
                    config.WriteLine(GameDir.Text);
                    config.WriteLine(ProxyDir.Text);
                    config.WriteLine(ServerDir.Text);
                    config.Close();
                    MessageBoxX.Show("保存成功", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBoxX.Show(ex.Message, "错误");
            }
        }

        private void loadconfig_Click(object sender, RoutedEventArgs e)
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
            if(ProxyDir.Text != "")
            {
                string iProxyDir = ProxyDir.Text;
            }
            else
            {
                MessageBoxX.Show("请填写proxy.py文件路径");
            }
            return;
        }

        private void OnlyStartServer_Click(object sender, RoutedEventArgs e)
        {
            if(ServerDir.Text != "")
            {
                string iServerDir = ServerDir.Text;
            }
            else
            {
                MessageBoxX.Show("请填写grasscutter.jar路径");
            }
            return;
        }

        private void OnlyStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (GameDir.Text != "")
            {
                string iGameDir = GameDir.Text;
            }
            else
            {
                MessageBoxX.Show("请填写游戏文件路径");
            }
            return;
        }
        private void StartAll_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
