using CRMPick.Entity;
using CRMPick.Utils;
using mshtml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// BatchTiaoRuWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BatchTiaoRuWindow : Window
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();


        private bool XunHuanTiaoRuc = false;//是否循环挑入
        private bool CanOperation = false;//可以操作
        private int startss = 10000;//开始间隔毫秒
        private int endss = 20000;//结束间隔毫秒
        private InjectJs inject;
        private IHTMLDocument2 doc;
        private IHTMLWindow2 win;
        private string resource;//正在搜索的资源
        private UserClass user;
        private string hint = "将客户资源复制到文本框中，点击开始挑入";
        private int codeerr = 0;//验证码错误次数
        private string pickurl = "";//挑入页面网址
        private bool clockstop = false;//定时关闭 true：停止 false：继续
        private bool pausestop = false;//定时关闭 true：停止 false：继续

        public BatchTiaoRuWindow(UserClass user)
        {
            InitializeComponent();
            //清除cookies
            DeleteCookies deleteCookies = new DeleteCookies();
            deleteCookies.SuppressWininetBehavior();
            this.user = user;
            xianzhi.Content = "*最多输入" + user.tiaoruresourcecount + "条资源!";
            this.ContentRendered += MLoad;
            this.webBrower.ObjectForScripting = new TiaoRuScriptEvent(this);
            this.pickWebBrowser.ObjectForScripting = new TiaoRuPickScriptEvent(this);
        }



        private void MLoad(object sender, EventArgs e)
        {
            this.Topmost = false;
            this.webBrower.LoadCompleted += new LoadCompletedEventHandler(webbrowser_LoadCompleted);
            this.pickWebBrowser.LoadCompleted += new LoadCompletedEventHandler(Pickwebbrowser_LoadCompleted);
            WebUtils.SuppressScriptErrors(this.webBrower, true);
            WebUtils.SuppressScriptErrors(this.pickWebBrowser, true);
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
        /// 查询公司资源
        /// </summary>
        private void InquireCompany()
        {
            try
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
            }catch(Exception we)
            {
                string errorSaveFiler = Directory.GetCurrentDirectory() + "\\errorlog.txt";//用户账号保存文件
                try
                {
                    File.AppendAllText(errorSaveFiler, "\r\n" + DateTime.Now.ToString() + "      " + we.ToString());
                }
                catch
                {

                }
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
        /// 挑入网页加载完毕监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pickwebbrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {

            string uri = pickWebBrowser.Source.ToString();
            if (pickurl.IndexOf(uri) > -1)
            {
                //页面加载完毕执行挑入
                IHTMLDocument2 pickdoc = (IHTMLDocument2)pickWebBrowser.Document;
                IHTMLWindow2 pickwin = (IHTMLWindow2)pickdoc.parentWindow;
                mshtml.HTMLDocument htmlDoc = pickWebBrowser.Document as mshtml.HTMLDocument;
                var head = htmlDoc.getElementsByTagName("head").Cast<HTMLHeadElement>().First();
                var script = (IHTMLScriptElement)htmlDoc.createElement("script");
                script.src = "https://demo.22com.cn/crm/json2.js";
                head.appendChild((IHTMLDOMNode)script);
                InjectJs inject = new InjectJs(this.pickWebBrowser);
                Thread thr = new Thread(() =>
                {
                    //这里还可以处理些比较耗时的事情。
                    Thread.Sleep(1000);//延时10秒
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        pickwin.execScript(inject.getOverridePickInJs(), "javascript");
                        pickwin.execScript("_shy_.alert_close();", "javascript");//关闭弹窗JS
                        pickwin.execScript("selectOpp.getWidget('').select(0,true);", "javascript");
                        pickwin.execScript("overrDoPick()", "javascript");
                    }));
                });
                thr.Start();

            }
        }


        /// <summary>
        /// 开始查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (examineCan())
            {
                clockstop = false;
                pausestop = false;
                reshUi(0);
                InquireCompany();
            }
        }



        /// <summary>
        /// 检查是否可以挑入
        /// </summary>
        private bool examineCan()
        {

            //获取间隔时间
            if (starts.Text.Trim().Equals("") && !ends.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入最小秒数");
                return false;
            }
            else if (!starts.Text.Trim().Equals("") && ends.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入最大秒数");
                return false;
            }
            else if (!starts.Text.Trim().Equals("") && !ends.Text.Trim().Equals(""))
            {
                startss = int.Parse(starts.Text) * 1000;
                endss = int.Parse(ends.Text) * 1000;
            }
            //开始挑入
            if (CanOperation)
            {

                string tbresousess = tbresouses.Text.TrimEnd('\r').Trim();
                if (tbresousess.Equals("") || tbresousess.Equals(hint))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                MessageBox.Show("页面不正确，请重新打开程序");
                return false;
            }
        }






        /// <summary>
        /// 调整ui
        /// </summary>
        private void reshUi(int startOrover)
        {
            if (startOrover == 0)
            {
                //正在查询
                startBtn.Content = "正在挑入";
                startBtn.IsEnabled = false;
                startBtn.Visibility = Visibility.Collapsed;
                starts.IsEnabled = false;
                ends.IsEnabled = false;
                datestart.IsEnabled = false;
                timePickerstart.IsEnabled = false;
                dateover.IsEnabled = false;
                timePickerover.IsEnabled = false;
                tingshiqidongbtn.IsEnabled = false;
                pauseBtn.Visibility = Visibility.Visible;
            }
            else
            {
                //未开始查询
                startBtn.Content = "开始挑入";
                startBtn.IsEnabled = true;
                startBtn.Visibility = Visibility.Visible;
                starts.IsEnabled = true;
                ends.IsEnabled = true;
                datestart.IsEnabled = true;
                timePickerstart.IsEnabled = true;
                dateover.IsEnabled = true;
                timePickerover.IsEnabled = true;
                tingshiqidongbtn.IsEnabled = true;
                pauseBtn.Visibility = Visibility.Collapsed;
                //关闭定时关闭线程
                if (thrStop != null)
                {
                    thrStop.Abort();
                }
            }
        }

        /// <summary>
        /// 暂停调整ui
        /// </summary>
        private void pauseReshUi()
        {
            startBtn.Content = "继续挑入";
            startBtn.IsEnabled = true;
            startBtn.Visibility = Visibility.Visible;
            pauseBtn.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {
            pausestop = true;
            MessageBox.Show("暂停");
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
                if (XunHuanTiaoRuc)
                {
                    this.Dispatcher.BeginInvoke((Action)(delegate ()
                    {
                        addresource(resource);
                    }));
                }
                Inquire();
            }
            else
            {
                t = new Thread(AnalyzeCompany);//创建了线程还未开启
                t.Start(json);//用来给函数传递参数，开启线程
            }

        }

        /// <summary>
        ///搜索结束、 解析json，分析公司资源状态
        /// </summary>
        /// <param name="jsons"></param>
        public void AnalyzeCompany(object jsons)
        {
            string json = (string)jsons;
            Console.WriteLine(json);
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
                                Thread.Sleep(2500);//延时2秒
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
                    //搜索成功
                    List<AllCustomerOpportunityListItem> AllCustomerOpportunityList = customer.allCustomerOpportunityList;
                    if (AllCustomerOpportunityList.Count == 0)
                    {
                        //没有查询到符合条件的客户
                        resourceRecord(resource, null, 0);
                    }else if (customer.allCustomerLeadList.Count>0)
                    {
                        //待分发
                        if (XunHuanTiaoRuc)
                        {
                            this.Dispatcher.BeginInvoke((Action)(delegate ()
                            {
                                addresource(resource);
                            }));
                        }
                        Inquire();
                    }
                    else
                    {
                        List<AllCustomerOpportunityListItem> SeaCustomerOpportunityList = null;
                        int resourceNameDifferentCount = 0;
                        //存在资源
                        for (int i = 0; i < AllCustomerOpportunityList.Count; i++)
                        {
                            //1、先判断所有资源名称是否相同
                            AllCustomerOpportunityListItem item = AllCustomerOpportunityList[i];
                            if (item.companyName.Equals(resource))
                            {
                                //2、判断是否有在库中的资源,有在仓库的直接记录后开始下一条资源查询
                                if (item.productType.Equals("1") && item.depotOrSea.Equals("depot"))
                                {
                                    SeaCustomerOpportunityList = null;
                                    resourceNameDifferentCount = 0;
                                    if (XunHuanTiaoRuc)
                                    {
                                        this.Dispatcher.BeginInvoke((Action)(delegate ()
                                        {
                                            addresource(resource);
                                        }));
                                    }
                                    Inquire();
                                    return;
                                }
                                else
                                {
                                    if (SeaCustomerOpportunityList == null)
                                    {
                                        SeaCustomerOpportunityList = new List<AllCustomerOpportunityListItem>();
                                    }
                                    SeaCustomerOpportunityList.Add(item);
                                }
                            }
                            else
                            {
                                //资源名称与搜索名称不同
                                resourceNameDifferentCount++;
                            }
                        }
                        if (resourceNameDifferentCount == AllCustomerOpportunityList.Count)
                        {
                            //4、所有资源名称都不正确
                            resourceRecord(resource, null, 1);
                        }
                        else
                        {
                            //3、所有资源都在公海，将信息记录
                            resourceRecord(resource, SeaCustomerOpportunityList[0], 2);
                        }
                        resourceNameDifferentCount = 0;
                        SeaCustomerOpportunityList = null;
                    }
                }

            }
            catch (Exception e)
            {
                string errorFiler = Directory.GetCurrentDirectory() + "\\errorlog.txt";//用户账号保存文件

                FileStream fs = new FileStream(errorFiler, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(json + "\n" + e.ToString());
                sw.Close();
                fs.Close();
                MessageBox.Show("提示:数据格式错误，请联系研发部！！");
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
        /// 资源记录，保存到excel
        /// </summary>
        /// <param name="resource">搜索的资源名称</param>
        /// <param name="item"></param>
        /// <param name="tag">0：没有资源 ；1：公司名称不符；2：公海 ；3：仓库</param>
        private void resourceRecord(string resource, AllCustomerOpportunityListItem item, int tag)
        {
            string result = "";
            switch (tag)
            {
                case 0://没有资源
                    result = "没有搜索到这个资源";
                   
                    result = null;
                    Inquire();
                    break;
                case 1://公司名称不符
                    result = "没有搜索到名称匹配资源";
                    result = null;
                    Inquire();
                    break;
                case 2://公海
                    this.Dispatcher.BeginInvoke((Action)(delegate ()
                    {
                        UsePickWebBrowser(item);
                    }));

                    break;
                case 3://仓库
                    if (XunHuanTiaoRuc)
                    {
                        this.Dispatcher.BeginInvoke((Action)(delegate ()
                        {
                            addresource(resource);
                        }));
                    }
                    else
                    {
                        result = "已在仓库";
                     
                        result = null;
                    }
                    Inquire();
                    break;
            }
        }

        /// <summary>
        /// 使用挑入窗口
        /// </summary>
        private void UsePickWebBrowser(AllCustomerOpportunityListItem item)
        {
            pickurl = "https://crm.alibaba-inc.com/noah/opportunity/pickInfo.cxul?globalId=" + item.encryptGlobalId + "&from=leads&source=recommendHide";
            this.pickWebBrowser.Source = new Uri(pickurl);
        }

        /// <summary>
        /// 循环挑入
        /// </summary>
        private void Inquire()
        {
            try
            {
                IntPtr pHandle = GetCurrentProcess();
                SetProcessWorkingSetSize(pHandle, -1, -1);
            }
            catch
            {

            }
            Thread.Sleep(RandomTime());//延时

            this.Dispatcher.BeginInvoke((Action)(delegate ()
            {
                //要执行的方法
                if (!clockstop && !pausestop)
                {
                    InquireCompany();//循环
                }
                if (clockstop)
                {
                    MessageBox.Show("结束挑入");
                    reshUi(1);
                }
            }));
        }

        



        /// <summary>
        /// 挑入结果
        /// </summary>
        /// <param name="tag">
        /////   0:挑入成功
        ////    1:挑入的公司名重复，不可挑
        ////    2:选择挑入机会
        ////    3:源介绍客户Id不能为空且必须在你名下
        ////    4:已达到仓库上限或者没有设置仓库上限
        ////    6:该组中没有可用的sales
        ////    7:挑入失败的机会
        ////    8:提交成功，系统后台正在转移客户
        ////    9:挑入失败，没有目标销售
        ////    12:挑入机会不能为空，请检查机会是否正常后再试
        ////    13:挑入失败,请检查库容或者机会是否正常后再试
        ////    14:分发目标的库容已满
        /// </param>
        public void DoPick(string json)
        {
            Console.WriteLine(json);
            Thread thr = new Thread(() =>
            {   
                Inquire();
            });
            thr.Start();
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
        /// 循环挑入按钮点击监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XunHuanTiaoRu_Checked(object sender, RoutedEventArgs e)
        {
            XunHuanTiaoRuc = XunHuanTiaoRu.IsChecked == true ? true : false;
        }


        /// <summary>
        /// 定时启动按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clockstop = false;
            pausestop = false;
            if (examineCan())
            {
                string datestart = this.datestart.SelectedDate.ToString();//开始时间
                if (datestart == null || datestart.Equals(""))
                {
                    MessageBox.Show("请设置日期！！");
                    return;
                }
                else
                {
                    string dateStartString = this.datestart.SelectedDate.ToString().Substring(0, this.datestart.SelectedDate.ToString().IndexOf(' ')) + " " + timePickerstart.TimeSpan;
                    long settime = GetTimeString.getSetTime(dateStartString);
                    long nowtime = GetTimeString.getNowTime();
                    if ((settime - nowtime).ToString().Length > 9)
                    {
                        MessageBox.Show("设置时间太长！！");
                        return;
                    }
                    int tim = (int)(settime - nowtime);
                    if (tim > 0)
                    {
                        reshUi(0);//调整ui
                                  //开启倒计时
                        Thread thr = new Thread(() =>
                        {
                            Thread.Sleep(tim);
                            this.Dispatcher.Invoke(new Action(() =>
                            {

                                InquireCompany();
                                timingStop();

                            }));
                        });
                        thr.Start();
                    }
                    else
                    {
                        MessageBox.Show("设置的时间已经过去了！！");
                    }
                }
            }
        }


        Thread thrStop;
        /// <summary>
        /// 开启定时关闭线程
        /// </summary>
        private void timingStop()
        {
            string dateend = dateover.SelectedDate.ToString();//结束时间
            string dateOverString = this.dateover.SelectedDate.ToString().Substring(0, this.dateover.SelectedDate.ToString().IndexOf(' ')) + " " + timePickerover.TimeSpan;                                 //存在结束时间
            if (dateend != null && !dateend.Equals(""))
            {
                thrStop = new Thread(() =>
                {
                    long settime = GetTimeString.getSetTime(dateOverString);
                    long nowtime = GetTimeString.getNowTime();
                    int tim;
                    if ((settime - nowtime).ToString().Length > 9)
                    {
                        tim = 999999999;
                    }
                    else
                    {
                        tim = (int)(settime - nowtime);
                    }
                    Thread.Sleep(tim);
                    clockstop = true;
                });
                thrStop.Start();
            }
        }
    }


    /// <summary>
    /// js调用C#类
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class TiaoRuScriptEvent
    {
        private BatchTiaoRuWindow batchTiaoRuWindow;

        public TiaoRuScriptEvent(BatchTiaoRuWindow batchTiaoRuWindow)
        {
            this.batchTiaoRuWindow = batchTiaoRuWindow;
        }

        //供JS调用
        public void CsharpVoid(int tag, string json)
        {
            batchTiaoRuWindow.AnalyzeCompanyThead(tag, json);
        }
    }


    /// <summary>
    /// js挑入调用C#类
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class TiaoRuPickScriptEvent
    {
        private BatchTiaoRuWindow batchTiaoRuWindow;

        public TiaoRuPickScriptEvent(BatchTiaoRuWindow batchTiaoRuWindow)
        {
            this.batchTiaoRuWindow = batchTiaoRuWindow;
        }

        /// <summary>
        /// 挑入结果
        /// </summary>
        /// <param name="tag"></param>
        public void DoPick(string json)
        {
            batchTiaoRuWindow.DoPick(json);
        }
    }
}
