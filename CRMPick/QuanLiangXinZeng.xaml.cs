using CRMPick.Entity;
using CRMPick.Utils;
using mshtml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMPick
{
    /// <summary>
    /// QuanLiangXinZeng.xaml 的交互逻辑
    /// </summary>
    public partial class QuanLiangXinZeng : Window
    {

        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();


        [DllImport("kernel32.dll")]
        private static extern int Beep(int dwFreq, int dwDuration);


        private bool CanOperation = false;//可以操作
        private int startss = 10000;//开始间隔毫秒
        private int endss = 20000;//结束间隔毫秒
        private InjectJs inject;
        private IHTMLDocument2 doc;
        private IHTMLWindow2 win;
        private string resource;//正在搜索的资源
        private UserClass user;
        private string hint = "将客户资源复制到文本框中，点击开始查询";
        private int codeerr = 0;//验证码错误次数
        private bool clockstop = false;//定时关闭 true：停止 false：继续
        public QuanLiangXinZeng()
        {
            InitializeComponent();
            DeleteCookies deleteCookies = new DeleteCookies();
            deleteCookies.SuppressWininetBehavior();
            this.user = user;
            this.ContentRendered += MLoad;
            this.webBrower.ObjectForScripting = new ChaXunScriptEvent(this);
        }


        private void MLoad(object sender, EventArgs e)
        {
            this.Topmost = false;
            this.webBrower.LoadCompleted += new LoadCompletedEventHandler(webbrowser_LoadCompleted);
            WebUtils.SuppressScriptErrors(this.webBrower, true);
        }


        private void TbLostF(object sender, RoutedEventArgs e)
        {
            if (tbresouses.Text.Trim().Equals(""))
            {
                tbresouses.Text = hint;
            }
        }

        private void TbGotF(object sender, RoutedEventArgs e)
        {
            if (tbresouses.Text.Trim().Equals(hint))
            {
                tbresouses.Text = "";
            }

        }

        /// <summary>
        /// 开始查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clockstop = false;


            //获取间隔时间
            if (starts.Text.Trim().Equals("") && !ends.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入最小秒数");
                return;
            }
            else if (!starts.Text.Trim().Equals("") && ends.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入最大秒数");
                return;
            }
            else if (!starts.Text.Trim().Equals("") && !ends.Text.Trim().Equals(""))
            {
                startss = int.Parse(starts.Text) * 1000;
                endss = int.Parse(ends.Text) * 1000;
            }


            //开始查询
            if (CanOperation)
            {
                string tbresousess = tbresouses.Text.Trim();
                if (tbresousess.Equals("") || tbresousess.Equals(hint))
                {

                }
                else
                {
                    reshUi(0);
                    string company = getNextCompanyName();
                    if (company.Equals(""))
                    {
                        //查询结束
                        reshUi(1);
                        tbresouses.Text = hint;
                    }
                    else
                    {
                        //查询
                        searchjs(company);
                    }
                }
            }
            else
            {
                MessageBox.Show("页面不正确，无法进行操作");
            }
        }
        /// <summary>
        /// 文本变化监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textChanged(object sender, TextChangedEventArgs e)
        {
            if (user != null)
            {
                TextBox textBox = sender as TextBox;
                string tbresousess = textBox.Text.Replace("\r\n", "\r").Replace("\n", "\r");
                string[] companys = tbresousess.Split('\r');
                if (companys.Length > user.gatherresourcecount)
                {
                    List<string> companylist = companys.ToList();
                    companylist.RemoveRange(user.gatherresourcecount, companys.Length - user.gatherresourcecount);
                    tbresouses.Text = string.Join("\r", companylist.ToArray());
                }
            }
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {
            clockstop = true;
            Thread thr = new Thread(() =>
            {
                //这里还可以处理些比较耗时的事情。
                Thread.Sleep(2000);//延时2秒
                this.Dispatcher.Invoke(new Action(() =>
                {
                    pauseReshUi();
                }));
            });
            thr.Start();
        }

        /// <summary>
        /// 暂停调整ui
        /// </summary>
        private void pauseReshUi()
        {
            startBtn.Content = "继续查询";
            startBtn.IsEnabled = true;
            startBtn.Visibility = Visibility.Visible;
            pauseBtn.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 网页加载完毕监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webbrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string uri = webBrower.Source.ToString();
            string url = "https://crm.alibaba-inc.com/noah/presale/work/allCustomer.vm";
            if (url.IndexOf(uri) > -1)
            {
                CanOperation = true;
                replaceJs();
            }
            else
            {
                CanOperation = false;
            }

        }

        /// <summary>
        /// 页面加载完成
        /// 注入js
        /// </summary>
        private void replaceJs()
        {
            mshtml.HTMLDocument htmlDoc = webBrower.Document as mshtml.HTMLDocument;
            var head = htmlDoc.getElementsByTagName("head").Cast<HTMLHeadElement>().First();
            var script = (IHTMLScriptElement)htmlDoc.createElement("script");
            script.src = "https://demo.22com.cn/crm/json2.js";
            head.appendChild((IHTMLDOMNode)script);
            getWinScript();
            if (inject == null)
            {
                inject = new InjectJs(this.webBrower);
            }
            win.execScript(inject.getOverrideJs(), "javascript");//替换JS
        }


        /// <summary>
        /// 执行js前需要获取html的window
        /// </summary>
        private void getWinScript()
        {
            if (doc == null)
            {
                doc = (IHTMLDocument2)webBrower.Document;
            }
            if (win == null)
            {
                win = (IHTMLWindow2)doc.parentWindow;
            }
        }

        /// <summary>
        /// js调用C#类
        /// </summary>
        [System.Runtime.InteropServices.ComVisible(true)]
        public class ChaXunScriptEvent
        {
            private QuanLiangXinZeng quanLiang;

            public ChaXunScriptEvent(QuanLiangXinZeng quanLiang)
            {
                this.quanLiang = quanLiang;
            }

            //供JS调用
            public void CsharpVoid(int tag, string json)
            {
                quanLiang.AnalyzeCompanyThead(tag, json);
            }
        }

        /// <summary>
        ///搜索结束、 解析json，分析公司资源状态
        /// </summary>
        /// <param name="jsons"></param>
        private void AnalyzeCompany(object jsons)
        {
            string json = (string)jsons;
            try
            {
                CustomerListClass customer = JsonConvert.DeserializeObject<CustomerListClass>(json);
                string err = customer.errorMsg;//搜索错误信息

                //判断这次请求验证码是否输入正确，正确的话展示结果，错误的提示重新输入

                if ((err != null && err.Equals("checkcode_error")) || (err != null && err.Equals("checkcode_need")))
                {
                    codeerr++;
                    //验证码错误,请求之后验证码要消失掉
                    this.Dispatcher.BeginInvoke((Action)(delegate ()
                    {
                        //要执行的方法
                        if (codeerr <= 4)
                        {
                            getWinScript();
                            win.execScript("reloadcode();", "javascript");//刷新验证码
                            Thread thr = new Thread(() =>
                            {
                                //这里还可以处理些比较耗时的事情。
                                Thread.Sleep(2000);//延时2秒
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    verfiyCode();//打码
                                }));
                            });
                            thr.Start();
                        }
                        else
                        {
                            MessageBox.Show("验证码错误次数过多！");
                            reshUi(1);
                        }

                    }));
                }
                else
                {
                    codeerr = 0;
                    int resourceNameDifferentCount = 0;
                    //搜索成功
                    List<AllCustomerOpportunityListItem> AllCustomerOpportunityList = customer.allCustomerOpportunityList;
                    if (AllCustomerOpportunityList.Count > 0)
                    {
                        //存在资源
                        for (int i = 0; i < AllCustomerOpportunityList.Count; i++)
                        {
                            //1、先判断所有资源名称是否相同
                            AllCustomerOpportunityListItem item = AllCustomerOpportunityList[i];
                            if (item.companyName.Equals(resource))
                            {
                                //提示
                                resourceNameDifferentCount++;
                            }
                        }
                    }
                    if (resourceNameDifferentCount == 0)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            addresource(resource);
                        }));

                        //要执行的方法
                        if (!clockstop)
                        {
                            InquireCompany();//循环
                        }

                    }
                    else
                    {
                        resourceNameDifferentCount = 0;
                        int a = 0X7FF;
                        int b = 1000;
                        Beep(a, b);

                        MessageBoxResult result = MessageBox.Show(resource + "   发现");
                        if (result == MessageBoxResult.OK)
                        {
                            //要执行的方法
                            if (!clockstop)
                            {
                                InquireCompany();//循环
                            }
                        }


                    }
                }
            }
            catch (Exception e)
            {
                string errorSaveFiler = Directory.GetCurrentDirectory() + "\\errorlog.txt";//用户账号保存文件
                try
                {
                    File.AppendAllText(errorSaveFiler, "\r\n" + DateTime.Now.ToString() + "      " + e.ToString());
                }
                catch
                {

                }
                MessageBox.Show("提示:数据格式改变，请联系研发部！！");
            }
        }

        /// <summary>
        /// 需要验证码，打码
        /// </summary>
        private void verfiyCode()
        {
            CacheImage cacheImage = new CacheImage();
            CacheImage.MyDelegate myDelegate = new CacheImage.MyDelegate(imgcheckCode);
            cacheImage.GetCacheImage(webBrower, "imgcheckcode", myDelegate, this);
        }

        public void imgcheckCode(string verificationCode)
        {
            win.execScript("$('#textcheckcode').val('" + verificationCode + "');", "javascript");//将打码后的验证码添加到输入框
            win.execScript("overrideSearchOpportunity('viaContact');", "javascript");//查询JS
        }


        /// <summary>
        /// 查询公司资源
        /// </summary>
        private void InquireCompany()
        {
            try
            {
                Thread thr = new Thread(() =>
                {
                    //这里还可以处理些比较耗时的事情。
                    Thread.Sleep(RandomTime());//延时2秒
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        string company = getNextCompanyName();
                        if (company.Equals(""))
                        {
                            //查询结束
                            reshUi(1);
                            tbresouses.Text = hint;
                        }
                        else
                        {
                            //查询
                            searchjs(company);
                        }
                    }));
                });
                thr.Start();
            }
            catch(Exception exc)
            {
                string errorSaveFiler = Directory.GetCurrentDirectory() + "\\errorlog.txt";//用户账号保存文件
                try
                {
                    File.AppendAllText(errorSaveFiler, "\r\n" + DateTime.Now.ToString() + "      " + exc.ToString());
                }
                catch
                {

                }
            }
            

        }
        /// <summary>
        /// 查询公司资源
        /// </summary>
        /// <param name="company"></param>
        private void searchjs(string company)
        {
            getWinScript();
            win.execScript("_shy_.alert_close();", "javascript");//关闭弹窗JS
            win.execScript("$('input.shy-input[id]').eq(0).val('" + company + "');searchFormContact.getWidget('')._setValue('0', 'companyName', '" + company + "');", "javascript");//添加公司资源JS
            win.execScript("overrideSearchOpportunity('viaContact');", "javascript");//查询JS
        }
        /// <summary>
        /// 获取下一条公司资源
        /// </summary>
        /// <returns></returns>
        private string getNextCompanyName()
        {
            string firstcompany = "";
            string tbresousess = tbresouses.Text.Replace("\r\n", "\r").Replace("\n", "\r").TrimEnd('\r');
            if (!tbresousess.Equals(hint))
            {
                string[] companys = tbresousess.Split('\r');
                companys = companys.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                if (companys.Length > 0)
                {
                    firstcompany = companys[0].Trim();//要查询的公司资源
                                                      /*将第一条资源删除*/
                    List<string> companylist = companys.ToList();
                    companylist.RemoveAt(0);
                    tbresouses.Text = string.Join("\r", companylist.ToArray());
                }

            }
            resource = firstcompany;
            return firstcompany;
        }

        /**
        * 添加资源
        */
        public void addresource(string resource)
        {
            //将仓库中的资源添加到搜索列表中
            string tbresousess = tbresouses.Text.Replace("\r\n", "\r").Replace("\n", "\r").TrimEnd('\r');
            if (!tbresousess.Equals(hint))
            {
                string[] companys = tbresousess.Split('\r');
                companys = companys.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                List<string> companylist = companys.ToList();
                companylist.Add(resource);
                tbresouses.Text = string.Join("\r", companylist.ToArray());
            }
            else
            {
                tbresouses.Text = resource;
            }
        }

        /// <summary>
        /// 随机延时数
        /// </summary>
        /// <returns></returns>
        public int RandomTime()
        {
            Random ran = new Random();
            int RandKey = ran.Next(startss, endss);
            return RandKey;
        }
        /// <summary>
        /// 调整ui
        /// </summary>
        private void reshUi(int startOrover)
        {
            if (startOrover == 0)
            {
                //正在查询
                startBtn.Content = "正在查询";
                startBtn.IsEnabled = false;
                startBtn.Visibility = Visibility.Collapsed;
                starts.IsEnabled = false;
                ends.IsEnabled = false;
                pauseBtn.Visibility = Visibility.Visible;
            }
            else
            {
                //未开始查询
                startBtn.Content = "开始查询";
                startBtn.IsEnabled = true;
                startBtn.Visibility = Visibility.Visible;
                starts.IsEnabled = true;
                ends.IsEnabled = true;
                pauseBtn.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 开启子线程操作查询数据
        /// </summary>
        /// <param name="json"></param>
        Thread t;
        public void AnalyzeCompanyThead(int tag, string json)
        {
            if (tag == 0)
            {
                //搜索失败
                //MessageBox.Show("连接服务器失败！");
                //reshUi(0);
                if (!clockstop)
                {
                    InquireCompany();//循环
                }
            }
            else
            {
                t = new Thread(AnalyzeCompany);//创建了线程还未开启
                t.Start(json);//用来给函数传递参数，开启线程
            }

        }
    }
}
