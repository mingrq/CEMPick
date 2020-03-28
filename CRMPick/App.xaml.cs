using CRMPick.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CRMPick
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorSaveFiler = Directory.GetCurrentDirectory() + "\\errorlog.txt";//用户账号保存文件
            try
            {
                File.AppendAllText(errorSaveFiler, "\r\n"+ DateTime.Now.ToString()+"      " + e.Exception.ToString());
            }
            catch
            {

            }
           
        }
    }
   

}
