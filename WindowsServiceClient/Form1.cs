using System;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Windows.Forms;

namespace WindowsServiceClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string serviceFilePath = $"{Application.StartupPath}\\MyWindowsService.exe";
        string serviceName = "MyService";




        //判断服务是否存在
        private bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        //安装服务
        private void InstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        //卸载服务
        private void UninstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                installer.Uninstall(null);
            }
        }
        //启动服务
        private void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        //停止服务
        private void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }
        //事件：安装服务


        private void button_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (this.IsServiceExisted(serviceName)) this.UninstallService(serviceName);
                this.InstallService(serviceFilePath);
                MessageBox.Show("服务安装成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务安装失败");
            }
        }

        //事件：启动服务


        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
                this.ServiceStart(serviceName);
            MessageBox.Show("服务启动成功");
        }
        //事件：停止服务

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (this.IsServiceExisted(serviceName))
                {
                    this.ServiceStop(serviceName);

                }
                MessageBox.Show("服务停止成功");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //事件：卸载服务
        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (this.IsServiceExisted(serviceName))
                    this.ServiceStop(serviceName);
                this.UninstallService(serviceFilePath);
                MessageBox.Show("卸载服务成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
