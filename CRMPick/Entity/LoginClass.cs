using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMPick.Entity
{
    class LoginClass
    {
        public int code { get; set; }
        /// <summary>
        /// 登录成功
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserClass result { get; set; }

    }
}
