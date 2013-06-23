using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;

namespace WallpaperManager
{
    /// <summary>
    /// This class manages changing wallpapers..
    /// A default layout, the first layout should always be provided
    /// </summary>
    public class Manager
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, String pvParam, SPIF fWinIni);

        private Layout[] m_layouts;
        public Layout[] Layouts
        {
            get
            {
                return m_layouts;
            }
            set
            {
                if (value.Length == 0)
                    throw new Exception("No elements in array!");
                m_layouts = value;
                Update();
            }
        }


        public Manager(Layout[] layout)
        {
            logger.Debug("wallpaper Manager made");
            Layouts = layout;
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        private void UseDefault()
        {
            SetWallPaper(Layouts[0].source, Layouts[0].style);
        }

        private void Update()
        {
            //match the screen size with the layout, and then choose the wallpaper;

            //match the # of screens first.
            Screen[] allScreen = Screen.AllScreens;

            Queue<Layout> canididiates = new Queue<Layout>();

            logger.Trace("{0} layouts, {1} screens connected", Layouts.Length, allScreen.Length);

            foreach (Layout l in Layouts)
            {
                if (l.screens.Length == allScreen.Length)
                    canididiates.Enqueue(l);
            }
            logger.Trace("{0} canididates", canididiates.Count);
            if (canididiates.Count == 0)
            {
                //use the default..
                UseDefault();
            }
            else
            {
                bool hasFound = false;

                //match the screen resolutions...
                foreach (Layout l in Layouts)
                {
                    bool isPossible = true;
                    for (int i = 0; i < l.screens.Length; i++)
                    {
                        if (l.screens[i] != allScreen[i].Bounds)
                        {
                            isPossible = false;
                            break;
                        }
                    }

                    if (isPossible)
                    {
                        //found it! 
                        hasFound = true;
                        SetWallPaper(l.source, l.style);
                        break;
                    }
                }

                if (!hasFound)
                {
                    //we didn't find it
                    UseDefault();
                }
            }
        }

        void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            logger.Debug("Display settings changed");
            Update();
        }

        private static void SetWallPaper(string path, WallpaperStyle style)
        {
            logger.Info("Setting {0} as wallpaper with style {1}", path, style);
            
            if (!File.Exists(path))
                throw new FileNotFoundException();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"WallpaperStyle", 22.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());
            SystemParametersInfo(SPI.SPI_SETDESKWALLPAPER,
            0,
            path,
            SPIF.SPIF_UPDATEINIFILE | SPIF.SPIF_SENDWININICHANGE);
        }

    }
}
