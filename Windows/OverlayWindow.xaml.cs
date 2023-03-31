using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace PoELevellingOverlay
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {

        public bool WindowLocked { get; set; } = true;
        InstructionReader reader = new InstructionReader();

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            LockWindow();
            reader.init();
            UpdateText(reader.getCurrentInstruction());
        }

        public void LockWindow()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowServices.SetWindowExTransparent(hwnd);
            WindowLocked = true;
        }

        public void UnlockWindow()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowServices.SetWindowExNotTransparent(hwnd);
            WindowLocked = false;
            Trace.WriteLine("Unlocked Window");
        }

        public OverlayWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window) sender;
            window.Topmost = true;
        }

        public void UpdateText(string text)
        {
            progress.Inlines.Clear();
            progress.Inlines.Add(new Run(reader.getProgress()));
            instructionText.Inlines.Clear();
            instructionText.Inlines.Add(new Run(text));
            SaveProgress();
        }

        public void inc()
        {
            UpdateText(reader.getNextInstruction());
        }

        public void dec()
        {
            UpdateText(reader.getPreviousInstruction());
        }

        private void On_Close(object sender, EventArgs e)
        {

        }

        private void SaveProgress()
        {
            Properties.Settings.Default["SavedAct"] = reader.Act;
            Properties.Settings.Default["SavedStep"] = reader.Instruction;
            Properties.Settings.Default.Save();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left )
            {
                this.DragMove();
            }
        }
    }
}
