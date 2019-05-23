using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SlimDX.RawInput;
using Timer = System.Timers.Timer;

namespace Tanks
{
    public partial class TanksForm : Form
    {
        private Timer UpdateTimer { get; }

        public TanksForm()
        {
            InitializeComponent();
            TanksGame = new Game(Width, Height);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            Paint += OnPaint;
            UpdateTimer = new Timer(5500);
            UpdateTimer.Elapsed += Tick;
            UpdateTimer.Enabled = true;
        }

        private void InitializeKeyboard()
        {
            Device.RegisterDevice(SlimDX.Multimedia.UsagePage.Generic, SlimDX.Multimedia.UsageId.Keyboard, DeviceFlags.None);
            Device. += new EventHandler<KeyboardInputEventArgs>(KeyboardInput);
        }


        private static readonly IReadOnlyDictionary<Key, InputAction> KeyMappings = new Dictionary<Key, InputAction>
        {
            [Key.LeftArrow] = InputAction.RotateLeft,
            [Key.RightArrow] = InputAction.RotateRight,
            [Key.Space] = InputAction.Shoot,
            [Key.UpArrow] = InputAction.MoveForward,
            [Key.DownArrow] = InputAction.MoveBackward,
            [Key.W] = InputAction.MoveForward,
            [Key.A] = InputAction.RotateLeft,
            [Key.D] = InputAction.RotateRight,
            [Key.S] = InputAction.MoveBackward
        };

        private Game TanksGame { get; }

        private void Tick(object sender, EventArgs e)
        {
            ReadInput();
            TanksGame.Update();
            Invalidate();
        }

        private void KeyboardInput(object sender, KeyboardInputEventArgs args)
        {
            var state = args.State;
            state.
            
            if (keyboard.Acquire().IsSuccess)
            {
                {
                    var pressed = keyboard.GetCurrentState().PressedKeys;

                    if (pressed.Count > 0)
                    {
                        foreach (var mapping in KeyMappings)
                        {
                            if (pressed.Contains(mapping.Key))
                            {
                                TanksGame.PendingActions.Add(mapping.Value);
                            }
                        }
                    }

                    keyboard.Unacquire();
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs args)
        {
            #region Graphics settings
            args.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            args.Graphics.CompositingMode = CompositingMode.SourceOver;
            args.Graphics.SmoothingMode = SmoothingMode.None;
            args.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            args.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            #endregion
            args.Graphics.Clear(Color.White);

            foreach (var projectile in TanksGame.CurrentLevel.Projectiles)
            {
                args.Graphics.FillRectangle(Brushes.DeepPink, projectile.X, projectile.Y, 5, 5);
            }

            foreach (var actor in TanksGame.CurrentLevel.Actors)
            {
                args.Graphics.RotateTransform(actor.Rotation * 0.01745329F);
                args.Graphics.FillRectangle(Brushes.Lime, actor.X, actor.Y, 5, 5);
            }
        }
    }
}
