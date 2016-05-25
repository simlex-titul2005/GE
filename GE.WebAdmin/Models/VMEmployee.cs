namespace GE.WebAdmin.Models
{
    public sealed class VMEmployee
    {
        public string Id { get; set; }

        public VMUser User { get; set; }

        public string Email
        {
            get
            {
                return User != null ? User.Email : null;
            }
        }

        public string NikName
        {
            get
            {
                return User != null ? User.NikName : null;
            }
        }
    }
}