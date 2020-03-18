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
    /// BatchChaxunWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BatchChaxunWindow : Window
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();



        private bool CanOperation = false;//可以操作
        private int startss = 10000;//开始间隔毫秒
        private int endss = 20000;//结束间隔毫秒
        private InjectJs inject;
        private IHTMLDocument2 doc;
        private IHTMLWindow2 win;
        private string resource;//正在搜索的资源
        private string excelPath = "";
        private UserClass user;
        private string hint = "将客户资源复制到文本框中，点击开始采集，程序将采集到的信息保存到指定的Excel中";
        private string filesavepath = Directory.GetCurrentDirectory() + "\\excelpath.txt";//excel路径保存文件
        string excelpath;
        private int codeerr = 0;//验证码错误次数
        private bool clockstop = false;//定时关闭 true：停止 false：继续

        public BatchChaxunWindow(UserClass user)
        {
            InitializeComponent();
            DeleteCookies deleteCookies = new DeleteCookies();
            deleteCookies.SuppressWininetBehavior();
            if (File.Exists(filesavepath))
            {
                //账号信息文件存在
                StreamReader reader = new StreamReader(filesavepath);
                string acce = reader.ReadToEnd();
                if (!acce.Trim().Equals(""))
                {
                    string[] accearray = acce.Split('\n');
                    for (int i = 0; i < accearray.Length; i++)
                    {
                        string exce = accearray[i];
                        if (exce.IndexOf("caiji") >= 0)
                        {
                            this.pathTb.Text = exce.Substring(exce.IndexOf('@') + 1);
                        }
                    }
                    reader.Close();
                }
            }
            if (this.pathTb.Text.Trim().Equals(""))
            {
                this.pathTb.Text = SelectFolder.getWinPath();
            }
            this.user = user;
            xianzhi.Content = "*最多输入" + user.gatherresourcecount + "条资源!";
            this.ContentRendered += MLoad;
            this.webBrower.ObjectForScripting = new ChaXunScriptEvent(this);
        }

        private void MLoad(object sender, EventArgs e)
        {
            this.Topmost = false;
            this.webBrower.LoadCompleted += new LoadCompletedEventHandler(webbrowser_LoadCompleted);

            if (!ExcelOperation.CheckExcelExist())
            {
                //没有Excel
                MessageBox.Show("你的电脑上没有安装Excel");
                return;
            }
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
                firstcompany = companys[0].Trim();//要查询的公司资源
                /*将第一条资源删除*/
                List<string> companylist = companys.ToList();
                companylist.RemoveAt(0);
                tbresouses.Text = string.Join("\r", companylist.ToArray());

            }
            resource = firstcompany;
            return firstcompany;
        }


        /// <summary>
        /// 查询公司资源
        /// </summary>
        private void InquireCompany()
        {
            IntPtr pHandle = GetCurrentProcess();
            SetProcessWorkingSetSize(pHandle, -1, -1);
            string company = getNextCompanyName();
            if (company.Equals(""))
            {
                //查询结束
                reshUi(1);
                tbresouses.Text = hint;
                MessageBox.Show("没有资源了，查询结束！！");
            }
            else
            {
                //查询
                searchjs(company);
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
        /// 开始查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clockstop = false;
            //获取excel放置位置
            excelpath = pathTb.Text.Trim();
            if (excelpath.Equals(""))
            {
                MessageBox.Show("请设置文件保存路径!");
                return;
            }

            //保存设置的文件路径
            Thread thread = new Thread(SaveUserAcc);
            thread.Start();

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
                //判断本页面是否创建excel，没有创建就创建
                if (excelPath.Equals(""))
                {
                    excelPath = ExcelOperation.CreateExcel(excelpath, 1);//创建excel
                }
                if (!excelPath.Equals(""))
                {
                    string tbresousess = tbresouses.Text.Trim();
                    if (tbresousess.Equals("") || tbresousess.Equals(hint))
                    {
                        MessageBox.Show("没有资源了");
                    }
                    else
                    {
                        reshUi(0);
                        InquireCompany();
                    }
                }
                else
                {
                    MessageBox.Show("Excel创建失败!");
                }
            }
            else
            {
                MessageBox.Show("页面不正确，无法进行操作");
            }
        }

        /// <summary>
        /// 将文件保存到本地
        /// </summary>
        private void SaveUserAcc()
        {
            FileStream fs = null;
            StreamWriter sw = null;
            fs = new FileStream(filesavepath, FileMode.OpenOrCreate);
            sw = new StreamWriter(fs);
            sw.WriteLine("caiji@" + excelpath + "\n");
            sw.Close();
            fs.Close();
        }



        /// <summary>
        /// 调整ui
        /// </summary>
        private void reshUi(int startOrover)
        {
            if (startOrover == 0)
            {
                //正在查询
                startBtn.Content = "正在采集";
                startBtn.IsEnabled = false;
                startBtn.Visibility = Visibility.Collapsed;
                starts.IsEnabled = false;
                ends.IsEnabled = false;
                pathTb.IsEnabled = false;
                pathsele.IsEnabled = false;
                pauseBtn.Visibility = Visibility.Visible;
            }
            else
            {
                //未开始查询
                startBtn.Content = "开始采集";
                startBtn.IsEnabled = true;
                startBtn.Visibility = Visibility.Visible;
                starts.IsEnabled = true;
                ends.IsEnabled = true;
                pauseBtn.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 暂停调整ui
        /// </summary>
        private void pauseReshUi()
        {
            startBtn.Content = "继续采集";
            startBtn.IsEnabled = true;
            startBtn.Visibility = Visibility.Visible;
            pauseBtn.Visibility = Visibility.Collapsed;
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
                MessageBox.Show("连接服务器失败！");
                reshUi(0);
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
        private void AnalyzeCompany(object jsons)
        {
            string json = (string)jsons;
            try
            {
                CustomerListClass customer = JsonConvert.DeserializeObject< CustomerListClass>(json);
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
                    //搜索成功
                    List<AllCustomerOpportunityListItem> AllCustomerOpportunityList = customer.allCustomerOpportunityList;
                    if (AllCustomerOpportunityList.Count == 0)
                    {
                        //没有查询到符合条件的客户
                        resourceRecord(resource, null, 0);
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
                                    resourceRecord(resource, item, 3);
                                    SeaCustomerOpportunityList = null;
                                    resourceNameDifferentCount = 0;
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

                sw.WriteLine(json+"\n"+e.ToString());
                sw.Close();
                fs.Close();
                MessageBox.Show("提示:阿里数据格式改变，请联系研发部！！");
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
        /// 选择文件夹路径按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFolderButtonClick(object sender, RoutedEventArgs e)
        {
            string path = SelectFolder.SelectFolderUtils();
            if (!path.Equals(""))
            {
                pathTb.Text = path;
            }
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
            string globalId = "";
            string orderArrived = "";
            string type = "";
            string saler = "";
            string time = "";
            string organization = "";
            switch (tag)
            {
                case 0://没有资源
                    result = "没有搜索到这个资源";
                    break;
                case 1://公司名称不符
                    result = "没有搜索到名称匹配资源";
                    break;
                case 2://公海
                    globalId = item.globalId;
                    orderArrived = "未到单";
                    type = "公海";
                    time = item.gmtlastOperate;
                    organization = "公海";
                    break;
                case 3://仓库
                    globalId = item.globalId;
                    if (item.orderArrived.Equals("n"))
                    {
                        orderArrived = "未到单";
                    }
                    else if (item.orderArrived.Equals("y"))
                    {
                        orderArrived = "已到单";
                    }
                    type = "仓库中";
                    saler = item.ownerName;
                    time = item.gmtlastOperate;
                    organization = item.orgFullNamePath;
                    break;
            }
            ExcelOperation.WriteToExcel(1, excelPath, resource, result, globalId, orderArrived, type, saler, time, organization);
            result = null;
            globalId = null;
            orderArrived = null;
            type = null;
            saler = null;
            time = null;
            organization = null;
            Thread.Sleep(RandomTime());//延时

            this.Dispatcher.BeginInvoke((Action)(delegate ()
            {
                //要执行的方法
                if (!clockstop)
                {
                    InquireCompany();//循环
                }
            }));

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
    }

    /// <summary>
    /// js调用C#类
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ChaXunScriptEvent
    {
        private BatchChaxunWindow batchChaxunWindow;

        public ChaXunScriptEvent(BatchChaxunWindow batchChaxunWindow)
        {
            this.batchChaxunWindow = batchChaxunWindow;
        }

        //供JS调用
        public void CsharpVoid(int tag, string json)
        {
            batchChaxunWindow.AnalyzeCompanyThead(tag, json);
        }
    }
}
