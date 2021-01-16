namespace ZeroTierTestXamarin.Droid
{
    public struct CallbackMessage
    {
        public int eventCode;
        /* Pointers to structures that contain details about the 
        subject of the callback */
        public System.IntPtr node;
        public System.IntPtr network;
        public System.IntPtr netif;
        public System.IntPtr route;
        public System.IntPtr path;
        public System.IntPtr peer;
        public System.IntPtr addr;
    }
}