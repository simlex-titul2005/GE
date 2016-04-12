namespace GE.WebUI.Models
{
    public sealed class VMMenuItem
    {
        public string Caption { get; set; }
        public string Url { get; set; }

        private string _controller;
        public string Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value.ToLower();
            }
        }

        private string _action;
        public string Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value.ToLower();
            }
        }

        public string Title { get; set; }

        public bool Show { get; set; }
    }
}