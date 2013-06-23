Console.WriteLine("Welcome to Wall Paper Manager");

            Screen[] allScreen = Screen.AllScreens;
            Screen p = Screen.PrimaryScreen;
            Rectangle r = p.Bounds;
            Console.WriteLine(p.DeviceName + " " + r.Width + "x" + r.Height);
            Console.WriteLine(p.ToString());


            foreach (Screen s in allScreen)
            {
                Rectangle t = s.Bounds;
                Console.WriteLine(s.DeviceName + " " + t.Width + "x" + t.Height);
            }

            //this part will find out the name!
#if CPP
            var device = new DISPLAY_DEVICE();
            device.cb = Marshal.SizeOf(device);
            try
            {
                for (uint id = 0; EnumDisplayDevices(null, id, ref device, 0); id++)
                {
                    Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}", id, device.DeviceName, device.DeviceString, device.StateFlags, device.DeviceID, device.DeviceKey));
                    Console.WriteLine();
                    device.cb = Marshal.SizeOf(device);

                    EnumDisplayDevices(device.DeviceName, 0, ref device, 0);

                    Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}, {5}", id, device.DeviceName, device.DeviceString, device.StateFlags, device.DeviceID, device.DeviceKey));
                    device.cb = Marshal.SizeOf(device);

                    device.cb = Marshal.SizeOf(device);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("{0}", ex.ToString()));
            }
            
#endif