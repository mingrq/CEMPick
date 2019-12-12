using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMPick.Entity
{
   

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
        public List<string> memberIndaysList { get; set; }
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
        public List<string> allCustomerLeadList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int allCustomerLeadCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AllCustomerOpportunityListItem> allCustomerOpportunityList { get; set; }
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
        /// /Alibaba/诚信通/销售/渠道销售部/渠道/渠道华北大区/京津冀区/北京渠道/党A组/河北驰业网络科技有限公司
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
        /// 驰业网络沧州
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
        /// 廊坊阿米巴建材有限公司
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
}
