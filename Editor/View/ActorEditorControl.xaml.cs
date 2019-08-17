﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics;

namespace WindEditor.View
{
    /// <summary>
    /// Interaction logic for ActorEditorControl.xaml
    /// </summary>
    public partial class ActorEditorControl : UserControl
    {
        public ActorEditorControl()
        {
            InitializeComponent();
            WindowsFormsHost.EnableWindowsFormsInterop();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorFormat cF = new ColorFormat(8);
            GraphicsMode gm = new GraphicsMode(cF, 24, 8, 1);
            glControlHost.Child = new GLControl(gm);
            glControlHost.SizeChanged += GlControlHost_SizeChanged;
            glControlHost.PreviewKeyDown += GlControlHost_PreviewKeyDown;
            glControlHost.PreviewKeyUp += GlControlHost_PreviewKeyUp;
            glControlHost.Child.MouseDown += GlControlHost_MouseDown;
            glControlHost.Child.MouseUp += GlControlHost_MouseUp;
            glControlHost.Child.MouseWheel += GlControlHost_MouseWheel;
        }

        private void GlControlHost_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            WInput.SetMouseState(WinFormToWPFMouseButton(e), false);
        }

        private void GlControlHost_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            WInput.SetMouseState(WinFormToWPFMouseButton(e), true);
        }

        private void GlControlHost_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            WInput.SetMouseScrollDelta(e.Delta);
        }

        private void GlControlHost_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            WInput.SetKeyboardState(e.Key, false);
        }

        private void GlControlHost_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            WInput.SetKeyboardState(e.Key, true);
        }

        private void GlControlHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            m_viewModel.WindEditor.OnViewportResized((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private static System.Windows.Input.MouseButton WinFormToWPFMouseButton(System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Input.MouseButton btn = System.Windows.Input.MouseButton.Left;
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    btn = System.Windows.Input.MouseButton.Left;
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    btn = System.Windows.Input.MouseButton.Middle;
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    btn = System.Windows.Input.MouseButton.Right;
                    break;
                case System.Windows.Forms.MouseButtons.XButton1:
                    btn = System.Windows.Input.MouseButton.XButton1;
                    break;
                case System.Windows.Forms.MouseButtons.XButton2:
                    btn = System.Windows.Input.MouseButton.XButton2;
                    break;
            }

            return btn;
        }
    }
}
