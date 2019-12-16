using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
namespace CRMPick.Utils
{
    class ExcelOperation
    {
        /// <summary>
        /// 检查excal是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckExcelExist()
        {
            bool isExist = false;
            Application excelApp = new Application();
            if (excelApp != null)
            {
                isExist = true;
            }
            return isExist;
        }

      
        /// <summary>
        /// 创建excel表格
        /// </summary>
        /// <param name="path">文件所在路径</param>
        /// <param name="tag">标记 1：创建采集表格 ；2：创建挑入表格</param>
        /// <returns></returns>
        public static string CreateExcel(string path,int tag)
        {
            string filename="";
            if (tag==1)
            {
                filename = "批量采集结果"+GetTimeString.gettime();
            }
            else
            {
                filename = "批量挑入结果"+ GetTimeString.gettime();
            }
            Application excelApp = new Application();
            Workbook workBook = excelApp.Workbooks.Add(true);
            Worksheet workSheet = workBook.ActiveSheet as Worksheet;
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;
            string dir = path + "\\" + filename + ".xlsx";
            int i = 1;
            while (File.Exists(dir))
            {
                dir = path + "\\" + filename + "(" + i + ")" + ".xlsx";
                i++;
            }
            if (tag==1)
            {
                workSheet.Name = "采集结果";
                //headline
                workSheet.Cells[1, 1] = "公司名称";
                workSheet.Cells[1, 2] = "采集结果";
                workSheet.Cells[1, 3] = "客户ID";
                workSheet.Cells[1, 4] = "到单";
                workSheet.Cells[1, 5] = "状态";
                workSheet.Cells[1, 6] = "所属销售";
                workSheet.Cells[1, 7] = "操作时间";
                workSheet.Cells[1, 8] = "销售组织";
            }
            else
            {
                workSheet.Name = "挑入结果";
                //headline
                workSheet.Cells[1, 1] = "公司名称";
                workSheet.Cells[1, 2] = "挑入结果";
            }
            
            workBook.SaveAs(dir);
            workBook.Close(false, Missing.Value, Missing.Value);
            excelApp.Quit();
            workSheet = null;
            workBook = null;
            excelApp = null;
            GC.Collect();
            return dir;
        }

        /// <summary>
        /// 写入Excel
        /// </summary>
        /// <param name="tag">标记 1：创建采集表格 ；2：创建挑入表格</param>
        /// <param name="excelName">excel全路径</param>
        /// <param name="companyName">公司名称</param>
        /// <param name="result">采集/挑入结果</param>
        /// <param name="globalId">客户Id</param>
        /// <param name="orderArrived">到单</param>
        /// <param name="type">状态</param>
        /// <param name="saler">所属销售</param>
        /// <param name="time">操作时间</param>
        /// <param name="organization">销售组织</param>
        public static void WriteToExcel(int tag, string excelName,string companyName,string result,string globalId,string orderArrived,string type,string saler,string time,string organization)
        {
            //open
            object Nothing = System.Reflection.Missing.Value;
            var app = new Application();
            app.Visible = false;
            Workbook mybook = app.Workbooks.Open(excelName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);
            Worksheet mysheet = (Worksheet)mybook.Worksheets[1];
            mysheet.Activate();
            //get activate sheet max row count
            int maxrow = mysheet.UsedRange.Rows.Count + 1;
            mysheet.Cells[maxrow, 1] = companyName;
            mysheet.Cells[maxrow, 2] = result;
            if (tag==1)
            {
                mysheet.Cells[maxrow, 3] = globalId;
                mysheet.Cells[maxrow, 4] = orderArrived;
                mysheet.Cells[maxrow, 5] = type;
                mysheet.Cells[maxrow, 6] = saler;
                mysheet.Cells[maxrow, 7] = time;
                mysheet.Cells[maxrow, 8] = organization;
            }
            mybook.Save();
            mybook.Close(false, Missing.Value, Missing.Value);
            mybook = null;
            //quit excel app
            app.Quit();
        }
        }
    }
