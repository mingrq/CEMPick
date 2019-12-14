using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMPick.Utils
{
    class SelectFolder
    {
        /// <summary>
        /// 选择文件夹弹窗
        /// </summary>
        /// <returns></returns>
        public static string SelectFolderUtils()
        {
            string path = "";
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                path = openFileDialog.SelectedPath;
            }
            return path;
        }


        /// <summary>
        /// 获取桌面路径
        /// </summary>
        /// <returns></returns>
        public static string getWinPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

       
    }
}
