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
//-----------搜索失败-----------
"            var json = \"\";\n" +
"            window.external.CsharpVoid(0,json);\n" +
//------------------
"        },\n" +
"        onSuccess: function (ret) {\n" +
"            window.allCustomer.conflictOpportunityIds = new Array(); // 查询完毕后，清空提交判单申请的结果。提交撞单机会Id\n" +
"            window.allCustomer.costTime.queryForList = new Date().getTime() - queryBeginTime;\n" +
"            exceptionMsg.hide();\n" +
"            //判断这次请求验证码是否输入正确，正确的话展示结果，错误的提示重新输入\n" +
"            if (ret.errorMsg == 'checkcode_error') {\n" +
"                imagespacher.update(checkcodestr + checkcodestrerror);//请求之后验证码要消失掉\n" +
//-----------搜索成功-----------
"            var json =JSON.stringify(ret).toString();\n" +
"            window.external.CsharpVoid(1,json);\n" +
//------------------
"                return;\n" +
"            } else if (ret.errorMsg == 'checkcode_need') {\n" +
"                imagespacher.update(checkcodestr + '<font color=\"red\">本次操作需要输入验证码后才能继续，请输入验证码后重新搜索！</font>');\n" +
//-----------搜索成功-----------
"            var json =JSON.stringify(ret).toString();\n" +
"            window.external.CsharpVoid(1,json);\n" +
//------------------
"                return;\n" +
"            } else {\n" +
"                imagespacher.update('');\n" +
"            }\n" +

//-----------搜索成功-----------
"            var json =JSON.stringify(ret).toString();\n" +
"            window.external.CsharpVoid(1,json);\n" +
//------------------

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
"        }\n" +
"    });\n" +
"    window.allCustomer.costTime.searchOpportunity = new Date().getTime() - beginTime;\n" +
"}";
            return js;
        }

        /// <summary>
        /// 获取挑入js
        /// </summary>
        /// <returns></returns>
        public string getOverridePickInJs()
        {
            string pickjs = "function overrDoPick(pickType){\n" +
"        // 如果没有可挑入的机会可选 时提醒销售\n" +
"        var canPickRows = selectOpp.getRows();\n" +
"        if(canPickRows.length == 0){\n" +
"            window.external.DoPick(1);\n" +
"            shy.alert(\"您挑入的公司名有重复，有违规风险，不可挑入，请仔细检查撞单（非销售角色没有挑入权限）\");\n" +
"            return;\n" +
"        }\n" +
"\n" +
"        var rows = getSelectedRows();\n" +
"        if(rows.length == 0){\n" +
"            window.external.DoPick(2);\n" +
"            shy.alert(\"请选中挑入的机会\");\n" +
"            return;\n" +
"        }\n" +
"\n" +
"        var param = queryForm.getRow();\n" +
"        param.set(\"selectedOpp\",rows);\n" +
"        param.set(\"from\",result.getRow().from);\n" +
"        // 转介绍挑入检查loginId\n" +
"        if(pickType != undefined && pickType == 'introCustPick'){\n" +
"            if(param.sourceGlobalId == undefined || param.sourceGlobalId.length <= 0){\n" +
"                window.external.DoPick(3);\n" +
"                shy.alert('源介绍客户Id不能为空且必须在你名下!');\n" +
"                return;\n" +
"            }\n" +
"        }\n" +
"        // 根据是否是简单leadsSource选择框，选择最终的leadsSource\n" +
"        if(simpleLeadsSourceHiddenInput.getValue() == \"1\") {\n" +
"            param.set(\"depotLeadsSourceFinal\", param.depotLeadsSource3);\n" +
"        }else {\n" +
"            param.set(\"depotLeadsSourceFinal\", param.depotLeadsSource2);\n" +
"        }\n" +
"        var ret;\n" +
"        if(pickType != undefined && pickType == 'introCustPick'){\n" +
"            ret = shy.rpc.pickIntroCustomer(param);\n" +
"        }else{\n" +
"            ret = shy.rpc.selectOpp(param);\n" +
"        }\n" +
"\n" +
"        failOppListModel.load(ret.failOppList);\n" +
"        successOppListModel.load(ret.successOppList);\n" +
"        successMessageModel.load(ret.successMessage);\n" +
"\n" +
"        resultPanel.show();\n" +
"        if(ret.incall && \"success\" == ret.errorMessage){\n" +
"            alitalkOnlineHbox.show();\n" +
"            successOppListGrid.show();\n" +
"        }\n" +
"\n" +
"        if(failOppListModel.getRow()!=null){\n" +
"            failOppListGrid.show();\n" +
"        }else{\n" +
"            failOppListGrid.hide();\n" +
"        }\n" +
"\n" +
"        errorMessage.setText(overrGetErrorMessage(ret));\n" +
"    }\n" +
"\n" +
"    function overrGetErrorMessage(ret){\n" +
"        if(ret.errorMessage==\"reachDeoptLimit\"){\n" +
"            window.external.DoPick(4);\n" +
"            if(ret.deoptLimit!=0){\n" +
"                return \"已达到仓库上限\"+ret.deoptLimit+\"或者没有设置仓库上限\";\n" +
"            }else{\n" +
"                return \"已达到仓库上限或者没有设置仓库上限\";\n" +
"            }\n" +
"        }else if(ret.errorMessage== \"reachSelectLimit\"){\n" +
"            window.external.DoPick(5);\n" +
"            if(ret.daySelectLimit!=0){\n" +
"                return \"已达到日挑入上限\"+ret.daySelectLimit+\",不能挑入\";\n" +
"            }else if(ret.monthSelectLimit!=0){\n" +
"                return \"已达到月挑入上限\"+ret.monthSelectLimit+\",不能挑入\";\n" +
"            }else{\n" +
"                return \"已达到挑入上限,不能挑入\";\n" +
"            }\n" +
"        }else if(ret.errorMessage==\"GroupFull\"){\n" +
"            window.external.DoPick(6);\n" +
"            return \"该组中没有可用的sales\";\n" +
"        }else if(ret.errorMessage==\"hasFailOppList\"){\n" +
"            window.external.DoPick(7);\n" +
"            return \"挑入失败的机会\";\n" +
"        }else if(ret.errorMessage==\"napoliSendSuccess\"){\n" +
"            window.external.DoPick(8);\n" +
"            return \"提交成功，系统后台正在转移客户\";\n" +
"        }else if(ret.errorMessage==\"success\"){\n" +
"            window.external.DoPick(0);\n" +
"            return \"客户已经成功挑入给您，请先搜索撞单后再跟进！\";\n" +
"        }else if(ret.errorMessage==\"noTargetSales\"){\n" +
"            window.external.DoPick(9);\n" +
"            return \"挑入失败，没有目标销售\";\n" +
"        }else if(ret.errorMessage == \"introsource_get_globalid_null\"){\n" +
"            return \"挑入机会的GlobalID为空\";\n" +
"        }else if(ret.errorMessage == \"introsource_group_full\"){\n" +
"            return \"该组中没有可用的sales\";\n" +
"        }else if(ret.errorMessage == \"introsource_no_oppids\"){\n" +
"            window.external.DoPick(12);\n" +
"            return \"挑入机会不能为空，请检查机会是否正常后再试\";\n" +
"        }else if(ret.errorMessage == \"introsource_pick_failed\"){\n" +
"            window.external.DoPick(13);\n" +
"            return \"挑入失败,请检查库容或者机会是否正常后再试\";\n" +
"        }else if(ret.errorMessage == \"introsource_pick_all_failed\"){\n" +
"            return \"挑入机会全部失败,请检查库容或者机会是否正常后再试\";\n" +
"        }else if(ret.errorMessage == \"introsource_can_not_pickorcreate\"){\n" +
"            return \"没有权限转介绍新增或挑入\";\n" +
"        }else if(ret.errorMessage == \"introsource_loginid_to_memberid_null\"){\n" +
"            return \"源介绍客户ID无法转换成AliID,请更改id后重新操作\";\n" +
"        }else if(ret.errorMessage == \"introsource_memberid_not_normal\"){\n" +
"            return \"源介绍客户的AliID非正常状态,请更改id后重新操作\";\n" +
"        }else if(ret.errorMessage == \"introsource_memberid_not_cbu\"){\n" +
"            return \"源介绍客户的AliID非中文站注册,不能更改,请更改id后重新操作\";\n" +
"        }else if(ret.errorMessage == \"introsource_customer_with_invalid_member_id\"){\n" +
"            return \"源介绍客户的AliID在网站不存在,请更改id后重新操作\";\n" +
"        }else if(ret.errorMessage == \"introsource_aliid_to_globalid_null\"){\n" +
"            return \"源介绍客户ID无法转换成GlobalID,请更改id后重新操作\";\n" +
"        }else if(ret.errorMessage == \"introsource_opportunity_null\"){\n" +
"            return \"源介绍客户ID没有对应的机会,请更改id后重新操作\";\n" +
"        }else if(ret.errorMessage == \"introsource_opportunity_not_in_depot\"){\n" +
"            return \"源介绍客户ID的机会不在你的仓库内,请更改id后重新操作\";\n" +
"        }else if(ret.errorMessage == \"introsource_no_introduce_capacity\"){\n" +
"            return \"转介绍上限已满\";\n" +
"        }else if(ret.errorMessage == \"introsource_create_relation_error\"){\n" +
"            return \"创建转介绍关系失败\";\n" +
"        }else if(ret.errorMessage == \"introsource_add_customer_limit_error\"){\n" +
"            return \"增加转介绍量失败\";\n" +
"        }else if(ret.errorMessage == \"create_customer_error_because_no_selling_permission\"){\n" +
"            return \"没有售卖权限\";\n" +
"        }else if(ret.errorMessage == \"create_customer_error_because_beyond_depot_limit\"){\n" +
"            window.external.DoPick(14);\n" +
"            return \"分发目标的库容已满\";\n" +
"        }else{\n" +
"            return ret.errorMessage;\n" +
"        }\n" +
"    }";

            return pickjs;
        }

       

        /// <summary>
        /// 获取session
        /// </summary>
        /// <returns></returns>
        private string getSession()
        {
            string session = "";
            string uri = webBrower.Source.ToString();
            string url = "https://crm.alibaba-inc.com/noah/presale/work/allCustomer.vm";
            if (url.IndexOf(uri) > -1)
            {
                HTMLDocument document = webBrower.Document as HTMLDocument;
                string html = document.body.innerHTML;
                int index = html.IndexOf("param.sessionid");
                string cutHtml = html.Substring(index);
                string needCutsession = cutHtml.Substring(0, cutHtml.IndexOf(";") - 1);
                session = needCutsession.Substring(needCutsession.IndexOf("\"") + 1);
            }
            return session;
        }
    }
}
