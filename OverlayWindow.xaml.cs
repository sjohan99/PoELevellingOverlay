﻿using System;
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

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            LockWindow();
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

        int counter = 0;

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

        public void UpdateText()
        {
            instructionText.Inlines.Clear();
            instructionText.Inlines.Add(new Run(counter.ToString()));
        }

        public void inc()
        {
            counter++;
            instructionText.Inlines.Clear();
            instructionText.Inlines.Add(new Run(counter.ToString()));
        }

        public void dec()
        {
            counter--;
            instructionText.Inlines.Clear();
            instructionText.Inlines.Add(new Run(counter.ToString()));
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
