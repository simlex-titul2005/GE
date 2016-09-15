namespace GE.WebUI.ViewModels
{
    public sealed class VMSiteTestStepNormal
    {
        public int SubjectId { get; set; }
        public int QuestionId { get; set; }
        public int LettersCount { get; set; }

        //bals
        public int BallsSubjectShow { get; set; } = -8;
        public int BallsGoodRead { get; set; }
        public int BallsBadRead { get; set; }
    }
}
