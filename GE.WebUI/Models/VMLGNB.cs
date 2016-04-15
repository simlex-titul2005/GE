namespace GE.WebUI.Models
{
    public sealed class VMLGNB
    {
        /// <summary>
        /// default counstructor
        /// </summary>
        /// <param name="lnc">last news count</param>
        /// <param name="gc">games count</param>
        public VMLGNB(int lnc, int gc, int glnc) {
            News = new VMLGBNews[lnc];
            Games = new VMLGNBGame[gc];
        }

        public VMLGBNews[] News { get; set; }
        public VMLGNBGame[] Games { get; set; }
    }
}