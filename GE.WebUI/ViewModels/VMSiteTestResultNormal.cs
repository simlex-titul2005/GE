namespace GE.WebUI.ViewModels
{
    public sealed class VMSiteTestResultNormal
    {
        public string SubjectTitle { get; set; }

        public string QuestionText { get; set; }

        public bool IsCorrect { get; set; }

        public VMSiteTestStepNormal Step { get; set; }

        public int SecondCount(int lettersInSecond)
        {
            return Step.LettersCount / lettersInSecond;
        }
    }
}
