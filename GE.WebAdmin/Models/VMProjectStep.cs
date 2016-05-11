using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMProjectStep
    {
        public VMProjectStep()
        {
            Steps = new VMProjectStep[0];
        }
        public int Id { get; set; }
        public int? ParentStepId { get; set; }
        public VMProjectStep[] Steps { get; set; }
        public string Title { get; set; }
        public string Foreword { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate{ get; set; }
        public int Order { get; set; }
        public bool IsDone { get; set; }
    }
}