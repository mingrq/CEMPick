using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMPick.Entity
{
    public class UserClass
    {
        public string team { set; get; }
        public string username { set; get; }
        public string userpw { set; get; }
        public string facility { set; get; }
        public string facilitytwo { set; get; }
        public string limited { set; get; }
        public int logincount { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
