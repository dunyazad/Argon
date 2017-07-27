using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using QuickFont;
using QuickFont.Configuration;

namespace ArtificialNature
{
    public class LibArgon : GameWindow
    {
        public enum InputType { Keyboard, Mouse };

        Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        public Scene ActiveScene { get; set; }

        public LibArgon()
        {
            VSync = VSyncMode.Off;
            //WindowBorder = WindowBorder.Fixed;
            WindowBorder = WindowBorder.Resizable;
        }

        public Scene CreateScene(string name)
        {
            if(scenes.ContainsKey(name))
            {
                return scenes[name];
            }
            else
            {
                Scene scene = new Scene(name);
                scenes.Add(name, scene);
                if(ActiveScene == null)
                {
                    ActiveScene = scene;
                }
                return scene;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var kvp in scenes)
            {
                kvp.Value.Resize(ClientRectangle.Width, ClientRectangle.Height);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            foreach (var kvp in scenes)
            {
                kvp.Value.Resize(ClientRectangle.Width, ClientRectangle.Height);
            }

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            foreach (var kvp in scenes)
            {
                kvp.Value.Render();
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            foreach (var kvp in scenes)
            {
                kvp.Value.Update(e.Time);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            scenes.Clear();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'a':
                    {
                        var forward = ActiveScene.MainCamera.TargetPosition - ActiveScene.MainCamera.EyePosition;
                        forward.Normalize();
                        var right = Vector3.Cross(forward, ActiveScene.MainCamera.UpDirection);
                        ActiveScene.MainCamera.EyePosition -= right;
                        ActiveScene.MainCamera.TargetPosition -= right;
                    }
                    break;
                case 'd':
                    {
                        var forward = ActiveScene.MainCamera.TargetPosition - ActiveScene.MainCamera.EyePosition;
                        forward.Normalize();
                        var right = Vector3.Cross(forward, ActiveScene.MainCamera.UpDirection);
                        ActiveScene.MainCamera.EyePosition += right;
                        ActiveScene.MainCamera.TargetPosition += right;
                    }
                    break;
                case 'w':
                    {
                        var forward = ActiveScene.MainCamera.TargetPosition - ActiveScene.MainCamera.EyePosition;
                        forward.Normalize();
                        ActiveScene.MainCamera.EyePosition += forward;
                        ActiveScene.MainCamera.TargetPosition += forward;
                    }
                    break;
                case 's':
                    {
                        var forward = ActiveScene.MainCamera.TargetPosition - ActiveScene.MainCamera.EyePosition;
                        forward.Normalize();
                        ActiveScene.MainCamera.EyePosition -= forward;
                        ActiveScene.MainCamera.TargetPosition -= forward;
                    }
                    break;
                default:
                    break;
            }
            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.BackSpace:
                case Key.Enter:
                case Key.F9:
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
        }
    }
}
