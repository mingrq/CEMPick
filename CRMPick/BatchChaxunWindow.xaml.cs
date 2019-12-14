using CRMPick.Entity;
using CRMPick.Utils;
using mshtml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private bool CanOperation = false;//可以操作
        private int startss = 10000;//开始间隔毫秒
        private int endss = 20000;//结束间隔毫秒
        private InjectJs inject;
        private IHTMLDocument2 doc;
        private IHTMLWindow2 win;

        public BatchChaxunWindow()
        {
            InitializeComponent();
            this.ContentRendered += MLoad;
            this.webBrower.ObjectForScripting = new ChaXunScriptEvent(this);
        }

        private void MLoad(object sender, EventArgs e)
        {
            this.Topmost = false;
            this.webBrower.LoadCompleted += new LoadCompletedEventHandler(webbrowser_LoadCompleted);
            this.pathTb.Text = SelectFolder.getWinPath();
        }


        private void TbLostF(object sender, RoutedEventArgs e)
        {
            if (tbresouses.Text.Trim().Equals(""))
            {
                tbresouses.Text = "将客户资源复制到文本框中，点击查询，每点击一次查询自动搜索出该公司信息";
            }
        }


        private void TbGotF(object sender, RoutedEventArgs e)
        {
            if (tbresouses.Text.Trim().Equals("将客户资源复制到文本框中，点击查询，每点击一次查询自动搜索出该公司信息"))
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
            string tbresousess = tbresouses.Text;
            if (!tbresousess.Equals("将客户资源复制到文本框中，点击查询，每点击一次查询自动搜索出该公司信息"))
            {
                string[] companys = tbresousess.Split('\n');
                firstcompany = companys[0].Trim();//要查询的公司资源
                /*将第一条资源删除*/
                List<string> companylist = companys.ToList();
                companylist.RemoveAt(0);
                tbresouses.Text = string.Join("\n", companylist.ToArray());

            }
            return firstcompany;
        }


        /// <summary>
        /// 查询公司资源
        /// </summary>
        private void InquireCompany()
        {
            string company = getNextCompanyName();
            if (company.Equals(""))
            {
                //查询结束
                reshUi(1);
                MessageBox.Show("没有资源了，查询结束！！");
            }
            else
            {
                //查询
                searchjs(company);


                // Thread.Sleep(RandomTime());//延时
                InquireCompany();//循环
            }

        }

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
        }


        /// <summary>
        /// 查询公司资源
        /// </summary>
        /// <param name="company"></param>
        private void searchjs(string company)
        {
            if (doc == null)
            {
                doc = (IHTMLDocument2)webBrower.Document;
            }
            if (win == null)
            {
                win = (IHTMLWindow2)doc.parentWindow;
            }
            if (inject == null)
            {
                inject = new InjectJs(this.webBrower);
            }
            win.execScript(inject.getOverrideJs(), "javascript");//替换JS
            win.execScript("$('input.shy-input[id]').eq(0).val('" + company + "');searchFormContact.getWidget('')._setValue('0', 'companyName', '" + company + "');", "javascript");//添加公司资源JS
            win.execScript("overrideSearchOpportunity('viaContact');", "javascript");//查询JS
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
                if (tbresousess.Equals("") || tbresousess.Equals("将客户资源复制到文本框中，点击查询，每点击一次查询自动搜索出该公司信息"))
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
                MessageBox.Show("页面不正确，无法进行操作");
            }
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        private void getimgcheckcode()
        {
            CacheImage.GetCacheImage(webBrower, "imgcheckcode");
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
                starts.IsEnabled = false;
                ends.IsEnabled = false;
            }
            else
            {
                //未开始查询
                startBtn.Content = "开始查询";
                startBtn.IsEnabled = true;
                starts.IsEnabled = true;
                ends.IsEnabled = true;
            }
        }

        /// <summary>
        ///搜索结束、 解析json，分析公司资源状态
        /// </summary>
        /// <param name="json"></param>
        public void analyzeCompany(string json)
        {
            CustomerListClass customer= JsonConvert.DeserializeObject<CustomerListClass>(json);
            MessageBox.Show(customer.errorMsg);
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
    }

    [System.Runtime.InteropServices.ComVisible(true)]
    public class ChaXunScriptEvent
    {
        private BatchChaxunWindow batchChaxunWindow;

        public ChaXunScriptEvent(BatchChaxunWindow batchChaxunWindow)
        {
            this.batchChaxunWindow = batchChaxunWindow;
        }

        //供JS调用
        public void CsharpVoid(string json)
        {
            batchChaxunWindow.analyzeCompany(json);
        }
    }
}
