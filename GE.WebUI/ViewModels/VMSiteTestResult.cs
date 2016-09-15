namespace GE.WebUI.ViewModels
{
    public sealed class VMSiteTestResult<TResult>
    {
        public VMSiteTestResult()
        {
            Results = new TResult[0];
        }

        public string SiteTestTitle { get; set; }
        public string SiteTestUrl { get; set; }
        public TResult[] Results { get; set; }
        public int BallsCount { get; set; }
    }
}
