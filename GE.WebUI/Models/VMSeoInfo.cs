using SX.WebCore;
using System.Linq;

namespace GE.WebUI.Models
{
    public sealed class VMSeoInfo : SxSeoInfo
    {
        private bool _isEmty = false;
        public bool IsEmpty 
        { 
            get
            {
                return _isEmty;
            }
            set
            {
                _isEmty = value;
            }
        }

        public string KeywordsString
        { 
            get
            {
                var res = string.Empty;
                var keywords=base.Keywords.ToArray();
                if(keywords.Any())
                {
                    
                    foreach (var keyword in keywords)
                    {
                        res += keyword.Value + ";";
                    }
                    res=res.Substring(0, res.Length-1);
                }

                return res;
            }
        }
    }
}