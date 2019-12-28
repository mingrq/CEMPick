using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMPick.Entity
{
   


    //如果好用，请收藏地址，帮忙分享。
    public class MemberIndaysListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int pageCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderByAsc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gmtCreate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gmtCreateStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageStart { get; set; }
        /// <summary>
        /// 河北
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string searchType { get; set; }
        /// <summary>
        /// 衡水赞程伟业精密机械制造有限公司
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 石家庄
        /// </summary>
        public string city { get; set; }
    }

    public class AllCustomerLeadListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 梁红强
        /// </summary>
        public string applyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 河北
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string memberId { get; set; }
        /// <summary>
        /// 定州市宇强体育用品厂
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 保定
        /// </summary>
        public string city { get; set; }
    }

    public class AllCustomerOpportunityListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string encryptGlobalId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastOperationType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ownerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string provinceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string canPick { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string encryptNickName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderArrived { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string productType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orgId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gmtlastOperate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerAlitalkStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isTp { get; set; }
        /// <summary>
        /// /Alibaba/诚信通/销售/渠道销售部/渠道/渠道华北大区/京津冀区/北京渠道/党L组/河北驰业网络科技有限公司衡水市冀州区分公司
        /// </summary>
        public string orgFullNamePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string renewFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ownerAlitalk { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerAlitalk { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string depotOrSea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string opportunityId { get; set; }
        /// <summary>
        /// 张洁
        /// </summary>
        public string ownerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string encryptAliId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string globalId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salesId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ownerAlitalkStatus { get; set; }
        /// <summary>
        /// 衡水赞程伟业精密机械制造有限公司
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aliId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subAction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string memberId { get; set; }
    }

    public class CustomerListClass
    {
        /// <summary>
        /// 
        /// </summary>
        public int memberIndaysCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string leadExceedFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MemberIndaysListItem> memberIndaysList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string memberExceedFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int allCustomerOpportunityCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AllCustomerLeadListItem> allCustomerLeadList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int allCustomerLeadCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AllCustomerOpportunityListItem> allCustomerOpportunityList { get; set; }
    }
}
