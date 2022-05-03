using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoELevellingOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OverlayWindow overlayWindow;
        InstructionReader reader;

        public MainWindow()
        {
            InitializeComponent();
            overlayWindow = new OverlayWindow();
            reader = new InstructionReader();
        }



        private void OpenOverlayWindow(object sender, RoutedEventArgs e)
        {
            //OverlayWindow overlay = new OverlayWindow();
            if (overlayWindow.Visibility == Visibility.Visible)
            {
                overlayWindow.Hide();
            }
            else
            {
                overlayWindow.Show();
            }
        }

        private void UnlockOverlay(object sender, RoutedEventArgs e)
        {
            if (overlayWindow != null)
            {
                if (overlayWindow.WindowLocked)
                {
                    overlayWindow.UnlockWindow();
                    overlayButton.Content = "Lock overlay";
                }
                else
                {
                    overlayWindow.LockWindow();
                    overlayButton.Content = "Unlock overlay";
                }
            }
            
        }

        private void OpenDirectoryDialog(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                Properties.Settings.Default["InstructionPath"] = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }


        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;

        //Modifiers:
        private const uint MOD_NONE = 0x0000; //[NONE]
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
                                             //CAPS LOCK:
        private const uint VK_CAPITAL = 0x14;
        private const uint VK_RIGHT = 0x27;
        private const uint VK_LEFT = 0x25;

        private HwndSource source;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);

            RegisterHotKey(handle, HOTKEY_ID, MOD_CONTROL, VK_CAPITAL);//CTRL + CAPS_LOCK
            RegisterHotKey(handle, HOTKEY_ID, MOD_CONTROL | MOD_ALT, VK_RIGHT);
            RegisterHotKey(handle, HOTKEY_ID, MOD_CONTROL | MOD_ALT, VK_LEFT);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == VK_RIGHT){
                                overlayWindow.inc();
                            }
                            else if (vkey == VK_LEFT)
                            {
                                overlayWindow.dec();
                            }
                            Trace.WriteLine(vkey.ToString());
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
