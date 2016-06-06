﻿using OpenTK.Graphics.OpenGL;
using OpenTK;
using WindEditor;
using System.IO;
using J3DRenderer.JStudio;
using GameFormatReader.Common;
using System.ComponentModel;
using J3DRenderer.JStudio.Animation;

namespace J3DRenderer
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public JStudio3D LoadedModel { get { return m_model; } }


        private GLControl m_glControl;
        private System.Diagnostics.Stopwatch m_dtStopwatch;

        // Rendering
        private WCamera m_renderCamera;
        private SimpleObjRenderer m_stockMesh;
        private int m_viewportHeight;
        private int m_viewportWidth;
        private JStudio3D m_model;
        private BCK m_testAnim;
        private float m_timeSinceStartup;

        public MainWindowViewModel()
        {
            m_renderCamera = new WCamera();
            m_renderCamera.Transform.Position = new Vector3(500, 75, 500);
            m_renderCamera.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, WMath.DegreesToRadians(45f));
            m_dtStopwatch = new System.Diagnostics.Stopwatch();
        }

        internal void OnMainEditorWindowLoaded(GLControl child)
        {
            m_glControl = child;

            Obj obj = new Obj();
            obj.Load("Framework/EditorCube.obj");
            m_stockMesh = new SimpleObjRenderer(obj);

            m_model = new JStudio3D();
            //m_model.LoadFromStream(new EndianBinaryReader(File.ReadAllBytes("resources/cl.bdl"), Endian.Big));
            m_model.LoadFromStream(new EndianBinaryReader(File.ReadAllBytes("resources/ba.bdl"), Endian.Big));
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LoadedModel"));

            m_testAnim = new BCK();
            m_testAnim.LoadFromStream(new EndianBinaryReader(File.ReadAllBytes("resources/wait01.bck"), Endian.Big));


            // Set up the Editor Tick Loop
            System.Windows.Forms.Timer editorTickTimer = new System.Windows.Forms.Timer();
            editorTickTimer.Interval = 16; //ms
            editorTickTimer.Tick += (o, args) =>
            {
                DoApplicationTick();
            };
            editorTickTimer.Enabled = true;
        }

        private void DoApplicationTick()
        {
            // Poll the mouse at a high resolution
            System.Drawing.Point mousePos = m_glControl.PointToClient(System.Windows.Forms.Cursor.Position);

            mousePos.X = WMath.Clamp(mousePos.X, 0, m_glControl.Width);
            mousePos.Y = WMath.Clamp(mousePos.Y, 0, m_glControl.Height);
            WInput.SetMousePosition(new Vector2(mousePos.X, mousePos.Y));

            ProcessTick();
            WInput.Internal_UpdateInputState();

            m_glControl.SwapBuffers();
        }

        private void ProcessTick()
        {
            System.Random rnd = new System.Random(m_glControl.GetHashCode());
            //GL.ClearColor(0.15f, 0.83f, 0.10f, 1f);
            GL.ClearColor(rnd.Next(255) / 255f, rnd.Next(255) / 255f, rnd.Next(255) / 255f, 1f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.Viewport(0, 0, m_viewportWidth, m_viewportHeight);

            float deltaTime = m_dtStopwatch.ElapsedMilliseconds / 1000f;
            m_dtStopwatch.Restart();

            m_renderCamera.Tick(deltaTime);

            deltaTime = WMath.Clamp(deltaTime, 0, 0.25f); // quater second max because debugging

            m_timeSinceStartup += deltaTime;
            m_testAnim.ApplyAnimationToPose(m_model.JNT1Tag.Joints.ToArray(), m_timeSinceStartup);

            // Render something
            //m_stockMesh.Render(m_renderCamera.ViewMatrix, m_renderCamera.ProjectionMatrix, Matrix4.Identity);
            m_model.Render(m_renderCamera.ViewMatrix, m_renderCamera.ProjectionMatrix, Matrix4.Identity);
        }

        internal void OnViewportResized(int width, int height)
        {
            m_viewportWidth = width;
            m_viewportHeight = height;
            m_renderCamera.AspectRatio = width / (float)height;
        }
    }
}
