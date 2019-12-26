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
          
             string userSaveFiler = Directory.GetCurrentDirectory() + "\\errorlog.txt";//用户账号保存文件
       
        FileStream fs = new FileStream(userSaveFiler, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs);
         
            sw.WriteLine($"抓到未知异常：", e.Exception);
            sw.Close();
            fs.Close();
        }
    }
   

}
