using CRMPick.Utils;
using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public BatchChaxunWindow()
        {
            InitializeComponent();
            this.ContentRendered += MLoad;
        }

        private void MLoad(object sender, EventArgs e)
        {
            this.Topmost = false;
            this.webBrower.LoadCompleted += new LoadCompletedEventHandler(webbrowser_LoadCompleted);
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



                InquireCompany();//循环
            }



           
        }

        /// <summary>
        /// 查询公司资源
        /// </summary>
        /// <param name="company"></param>
        private void searchjs(string company)
        {
            string js = "searchFormContact.getWidget('')._setValue('0', 'companyName', " + company + ");searchOpportunity('viaContact');";
            IHTMLDocument2 doc = (IHTMLDocument2)webBrower.Document;
            IHTMLWindow2 win = (IHTMLWindow2)doc.parentWindow;
            /**
             *$('input.shy-input[id]').eq(0).val('内容');//设置输入框内容
                searchFormContact.getWidget('')._setValue('0', 'companyName', '内容');//执行js
                searchOpportunity('viaContact')//搜索资源
             */
            win.execScript("$('input.shy-input[id]').eq(0).val('" + company + "');searchFormContact.getWidget('')._setValue('0', 'companyName', '" + company + "');searchOpportunity('viaContact');", "javascript");//使用JS

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
            
            //if (CanOperation)
            //{
            //    string tbresousess = tbresouses.Text.Trim();
            //    if (tbresousess.Equals("")|| tbresousess.Equals("将客户资源复制到文本框中，点击查询，每点击一次查询自动搜索出该公司信息"))
            //    {
            //        MessageBox.Show("没有资源了");
            //    }
            //    else
            //    {
            //        reshUi(0);
            //        InquireCompany();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("页面不正确，无法进行操作");
            //}
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
    }
}
