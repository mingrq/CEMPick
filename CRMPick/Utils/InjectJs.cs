using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CRMPick.Utils
{
    class InjectJs
    {
        private WebBrowser webBrower;
        public InjectJs(WebBrowser webBrower)
        {
            this.webBrower = webBrower;
        }

        public string getOverrideJs()
        {
            string js = "function overrideSearchOpportunity(via) {\n" +

"    window.allCustomer.via = via;\n" +
"    var beginTime = new Date().getTime();\n" +
"    // 1 获得页面表单\n" +
"    var param = null;\n" +
"    if (via == \"viaContact\") {\n" +
"        param = searchFormContact.getRow();\n" +
"    } else if (via = \"viaCustomer\") {\n" +
"        param = searchFormCustomer.getRow();\n" +
"    }\n" +
"    // 2.1  如果一个查询条件都没填，就直接返回。\n" +
"    if (via == \"viaContact\" && empty(param.companyName) && empty(param.applyName) && empty(param.phoneArea) && empty(param.phone) && empty(param.mobile) && empty(param.faxArea) && empty(param.fax) && empty(param.email)) {\n" +
"        return;\n" +
"    }\n" +
"    if (via == \"viaCustomer\" && empty(param.globalId) && empty(param.memberId) && empty(param.identifyCardNumber) && empty(param.aliyunID)) {\n" +
"        return;\n" +
"    }\n" +
"    // 2.2 联系人维度，如果只输入的区号没输号码，除了 电话区号+公司名 的情况以外，都提示错误。\n" +
"    if (via == \"viaContact\" && ((!empty(param.phoneArea) && empty(param.phone)) || (!empty(param.faxArea) && empty(param.fax)))) {\n" +
"        if ((param.companyName != undefined && param.companyName != \"\") && (param.applyName == undefined || param.applyName == \"\") && (param.phoneArea != undefined && param.phoneArea != \"\") && (param.phone == undefined || param.phone == \"\") && (param.mobile == undefined || param.mobile == \"\") && (param.faxArea == undefined || param.faxArea == \"\") && (param.fax == undefined || param.fax == \"\") && (param.email == undefined || param.email == \"\")) {\n" +
"            // 只输入了 公司名+电话区号，ok\n" +
"        } else {\n" +
"            shy.alert(\"单一区号条件仅支持 公司名+电话区号的 联合查询，其他情况请同时输入 区号+号码\");\n" +
"            return;\n" +
"        }\n" +
"    }\n" +
"    // 2.3 客户维度如果同时输入了memberId和globalId提示错误\n" +
"    if (via == \"viaCustomer\" && !empty(param.globalId) && !empty(param.memberId)) {\n" +
"        shy.alert(\"不支持同时输入LoginId 和  客户ID 两个条件查询。\");\n" +
"        return;\n" +
"    }\n" +
"    // 3 查询条件字段长度通用验证器， 隐藏掉LeadsCount的文本\n" +
"    allCustomerValidator.validate(param);\n" +
"    //textLeadsCount.hide();\n" +
"    // 4 增加搜索按钮的搜索维度\n" +
"    param.set('via', via);\n" +
"    // 4.1 如果globalId不为空，就通过globalId来搜素。如果memberId不为空就通过memberId来搜索。globalId和memberId不可能同时为空，前面判断了\n" +
"    if (!empty(param.globalId)) {\n" +
"        param.set('idType', 'globalId');\n" +
"        param.set('idText', param.globalId);\n" +
"    } else if (!empty(param.memberId)) {\n" +
"        param.set('idType', 'memberId');\n" +
"        param.set('idText', param.memberId);\n" +
"    } else if (!empty(param.aliyunID)) {\n" +
"        param.set('idType', 'aliyunID');\n" +
"        param.set('idText', param.aliyunID);\n" +
"    } else {\n" +
"        param.set('idType', 'globalId'); // 异常情况，防止后续代码问题。默认globalId\n" +
"        param.set('idText', '');\n" +
"    }\n" +
"    // 5 如果只输入了 公司名和电话区号 ，走公司名+电话区号and的条件搜索，否则走全部条件or条件搜索\n" +
"    if ((param.companyName != undefined && param.companyName != \"\") && (param.applyName == undefined || param.applyName == \"\") && (param.phoneArea != undefined && param.phoneArea != \"\") && (param.phone == undefined || param.phone == \"\") && (param.mobile == undefined || param.mobile == \"\") && (param.faxArea == undefined || param.faxArea == \"\") && (param.fax == undefined || param.fax == \"\") && (param.email == undefined || param.email == \"\")) {\n" +
"        param.set('searchType', 'companyAndAreaCode');\n" +
"    } else {\n" +
"        param.set('searchType', 'allOr');\n" +
"    }\n" +
"    // 6.1 把查询条件保存在Js对象里面\n" +
"    allCustomer.queryParam = param;\n" +
"    // 6.2 清空上一轮的搜索结果\n" +
"    opportunityList.load([]);\n" +
"    if (\"y\" == \"y\") {\n" +
"        leadList.load([]);\n" +
"    }\n" +
"    customerList.load([]);\n" +
"    // 7 调用Java端。返回值存在页面变量上allCustomer.queryResult\n" +
"    var queryBeginTime = new Date().getTime();\n" +
"    param.hasLeadViewPermission = 'y';\n" +
"    // skip=0,max=100 是没有用的。全部客户没有分页，最大的返回100条的限制在Repository里面，超过100抛异常\n" +
"    if (via == \"viaContact\") {//联系人部分搜索需要验证码\n" +
"        var hck = jQuery(\"#hiddencheckcode\").val();\n" +
"        if (hck == '1') {//标识这次请求是输入验证码后的再次搜索，此时要设置参数进来\n" +
"            param.checkcode = jQuery(\"#textcheckcode\").val();\n" +
"            param.sessionid = \"" + getSession() + "\";\n" +
"        }\n" +
"    }\n" +
"    search_as_opportunity_rpc.queryForList(param, 0, 100, {\n" +
"        onFailure: function (ex) {\n" +
"            exceptionMsg.setText(ex.getMessage());\n" +
"            exceptionMsg.show();\n" +
"            imagespacher.update('');\n" +
"        },\n" +
"        onSuccess: function (ret) {\n" +
"            window.allCustomer.conflictOpportunityIds = new Array(); // 查询完毕后，清空提交判单申请的结果。提交撞单机会Id\n" +
"            window.allCustomer.costTime.queryForList = new Date().getTime() - queryBeginTime;\n" +
"            exceptionMsg.hide();\n" +
"            //判断这次请求验证码是否输入正确，正确的话展示结果，错误的提示重新输入\n" +
"            if (ret.errorMsg == 'checkcode_error') {\n" +
"                imagespacher.update(checkcodestr + checkcodestrerror);//请求之后验证码要消失掉\n" +
"                return;\n" +
"            } else if (ret.errorMsg == 'checkcode_need') {\n" +
"                imagespacher.update(checkcodestr + '<font color=\"red\">本次操作需要输入验证码后才能继续，请输入验证码后重新搜索！</font>');\n" +
"                return;\n" +
"            } else {\n" +
"                imagespacher.update('');\n" +
"            }\n" +
"\n" +
"            allCustomer.queryResult = ret.allCustomerOpportunityList;\n" +
"            if (empty(allCustomer.queryResult)) {\n" +
"                exceptionMsg.setText(\"没有查询到符合条件的客户。\");\n" +
"                exceptionMsg.show();\n" +
"                if (empty(ret.allCustomerLeadList) && empty(ret.memberIndaysList)) {\n" +
"                    textCustomerCount.hide();\n" +
"                    customerDiv.hide();\n" +
"                    if (\"y\" == \"y\") {\n" +
"                        textLeadsCount.hide();\n" +
"                        leadDiv.hide();\n" +
"                        commonTaskAddLink.hide();\n" +
"                    }\n" +
"                    return;\n" +
"                }\n" +
"            }\n" +
"            if (\"y\" == \"y\") {\n" +
"                //待分发资源\n" +
"                if (!empty(ret.allCustomerLeadList) && ret.allCustomerLeadList.length > 0) {\n" +
"                    if (ret.leadExceedFlag == \"1\") {\n" +
"                        textLeadsCount.setText(\"重复或未分配的新leads搜索结果过多，请缩小搜索范围\");\n" +
"                        textLeadsCount.show();\n" +
"                        leadDiv.hide();\n" +
"                        commonTaskAddLink.hide();\n" +
"                    } else {\n" +
"                        textLeadsCount.setText(\"存在\" + ret.allCustomerLeadCount + \"条，系统待分发资源不可挑，违规者资源抽回！\");\n" +
"                        textLeadsCount.show();\n" +
"                        commonTaskAddLink.show();\n" +
"                        leadList.load(ret.allCustomerLeadList);\n" +
"                        leadDiv.show();\n" +
"                    }\n" +
"\n" +
"                } else {\n" +
"                    leadDiv.hide();\n" +
"                    textLeadsCount.hide();\n" +
"                    commonTaskAddLink.hide();\n" +
"                }\n" +
"            }\n" +
"            //15天会员\n" +
"            if (!empty(ret.memberIndaysList) && ret.memberIndaysList.length > 0) {\n" +
"                if (ret.memberExceedFlag == \"1\") {\n" +
"                    textCustomerCount.setText(\"15天注册资源返回过多,请缩小查询范围。\");\n" +
"                    textCustomerCount.show();\n" +
"                    customerDiv.hide();\n" +
"                } else {\n" +
"                    customerList.load(ret.memberIndaysList);\n" +
"                    customerDiv.show();\n" +
"                    textCustomerCount.show();\n" +
"                    //textCustomerCount.setText(\"存在\" + ret.memberIndaysCount + \"条，15天注册资源不可挑，违规者资源抽回！\");\n" +
"                }\n" +
"            } else {\n" +
"                customerDiv.hide();\n" +
"                textCustomerCount.hide();\n" +
"            }\n" +
"            // 8 根据产品线显示OppCustomer的信息\n" +
"            searchOppBySelectProductType(selectProductLine);\n" +
"            window.allCustomer.costTime.total = new Date().getTime() - queryBeginTime;\n" +
"            Behaviour.addLog(shy.json(window.allCustomer.costTime));\n" +
"            var json =JSON.stringify(ret).toString();\n" +
"            window.external.CsharpVoid(json);\n" +
"        }\n" +
"    });\n" +
"    window.allCustomer.costTime.searchOpportunity = new Date().getTime() - beginTime;\n" +
"}";
            return js;
        }

        /// <summary>
        /// 获取session
        /// </summary>
        /// <returns></returns>
        private string getSession()
        {
            HTMLDocument document = webBrower.Document as HTMLDocument;
            string html = document.body.innerHTML;
            int index = html.IndexOf("param.sessionid");
            string cutHtml = html.Substring(index);
            string needCutsession = cutHtml.Substring(0, cutHtml.IndexOf(";") - 1);
            Console.WriteLine("needCutsession:  " + needCutsession);
            string session = needCutsession.Substring(needCutsession.IndexOf("\"") + 1);
            Console.WriteLine("session:  " + session);
            return session;
        }
    }
}
