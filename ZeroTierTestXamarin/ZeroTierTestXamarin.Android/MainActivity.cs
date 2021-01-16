using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Shape;
using Java.IO;

using Console = System.Console;
using Environment = System.Environment;

namespace ZeroTierTestXamarin.Droid
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CSharpCallback(IntPtr msg);
    
    public interface ZeroTierEventListener {

        /*
         * Called when an even occurs in the native section of the ZeroTier library service
         */
        public void onZeroTierEvent(long nwid, int eventCode);
    }
    
    [Activity(Label = "ZeroTierTestXamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        // [DllImport("wg-go")]
        // private static extern string wgVersion();
        //
        [DllImport("zt")]
        public static extern int zts_allow_local_conf(int allowed);
        //
        // [DllImport("libzt.so", CallingConvention = CallingConvention.Cdecl)]
        // public static extern Int64 zts_get_node_id();

        // [DllImport("libZeroTierOneJNI")]
        // [DllImport("ZeroTierOneJNI")]
        // private static extern string Java_com_zerotier_sdk_Node_version();

        //[DllImport("zt")]
        //private static extern string Java_com_zerotier_sdk_Node_version();
        
        [DllImport("zt")]
        public static extern int zts_start(string path, CSharpCallback callback, int port);

        private CSharpCallback _callback;
        private void MyCallback(IntPtr msg)
        {

            var callbackMessage = Marshal.PtrToStructure<CallbackMessage>(msg);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ulong nwid = 0xabfd31bd47bef75e;
            int err = 0;
            
            Console.WriteLine("Waiting for ZeroTier to come online...");

            GC.Collect();
            
            var nodeId = zts_allow_local_conf(1);
            //var ffff = Java_com_zerotier_sdk_Node_version();
            
            _callback = new CSharpCallback(MyCallback);
            //var functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate(_callback);
            var currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if ((err = zts_start(currentDirectory, _callback,  9994)) == 1)
            {
                throw new Exception(string.Format("Failed to start/join network [{0:X}]", nwid));
            }
 
            var fooTxt = currentDirectory + "/foo.txt";
            System.IO.File.WriteAllText(fooTxt, "text");
            var fileInfo = new System.IO.FileInfo(fooTxt);
            var path = fileInfo.FullName;
            //var foo = LibZt.zts_startjoin(System.IO.Directory.GetCurrentDirectory(), nwid);
            //System.Diagnostics.Debug.WriteLine("XXXXX" + err);
            //System.Diagnostics.Debug.WriteLine(Java_com_zerotier_sdk_Node_version());
            //System.Diagnostics.Debug.WriteLine(wgVersion());
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}