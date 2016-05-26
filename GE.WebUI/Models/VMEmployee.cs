namespace GE.WebUI.Models
{
    public sealed class VMEmployee
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }

        public string Description { get; set; }

        public VMUser User { get; set; }
    }
}