using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GraphicsTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Random rd = new Random();
            Application.Run(new Func<System.Windows.Forms.Form>(() =>
            {
                System.Windows.Forms.Form window = new Form();
                window.Shown += new EventHandler((sender1, args1) =>
                {
                    const int a = 10;
                    var body_parts = new LinkedList<Point>(new Point[] { new Point(100, 100), new Point(100, 100 + a) });
                    var direction = new Queue<int>(new int[] { 0 });
                    var graphics = window.CreateGraphics();
                    var brush = new SolidBrush(Color.Green);
                    var grow_point = new Point(50, 50);
                    var move_body = new Action<bool>((add) =>
                    {
                        if (body_parts.First.Value != grow_point) body_parts.RemoveLast();
                        else do
                            {
                                grow_point = new Point(rd.Next(20) * a, rd.Next(20) * a);
                            } while (body_parts.Find(grow_point) != null);
                        if (direction.Peek() == 0) body_parts.AddFirst(new Point(body_parts.First().X, body_parts.First().Y - a));
                        if (direction.Peek() == 1) body_parts.AddFirst(new Point(body_parts.First().X + a, body_parts.First().Y));
                        if (direction.Peek() == 2) body_parts.AddFirst(new Point(body_parts.First().X, body_parts.First().Y + a));
                        if (direction.Peek() == 3) body_parts.AddFirst(new Point(body_parts.First().X - a, body_parts.First().Y));
                        for (int i = 1; i < body_parts.Count; ++i)
                        {
                            if (body_parts.First.Value == body_parts.ElementAt(i)) throw new System.Exception();
                        }
                        if (body_parts.First.Value.X < 0 || body_parts.First.Value.Y < 0 || body_parts.First.Value.X > 200 || body_parts.First.Value.Y > 200) throw new System.Exception();
                        graphics.Clear(System.Drawing.Color.White);
                        brush.Color = System.Drawing.Color.Green;
                        for (var i = 0; i < body_parts.Count(); ++i)
                        {
                            if (i == 0) brush.Color = System.Drawing.Color.Red;
                            else
                                if (i == body_parts.Count() - 1) brush.Color = System.Drawing.Color.Black;
                                else brush.Color = System.Drawing.Color.Green;
                            graphics.FillRectangle(brush, new Rectangle(body_parts.ElementAt(i), new Size(a, a)));
                        }
                        brush.Color = Color.BlueViolet;
                        graphics.FillRectangle(brush, new Rectangle(grow_point, new Size(a, a)));
                        if (direction.Count > 1) direction.Dequeue();
                    });


                    window.KeyPreview = true;
                    window.KeyUp += new KeyEventHandler((sender, args) =>
                    {
                        if (args.KeyCode == Keys.Up) if (direction.Peek() != 2) direction.Enqueue(0);
                        if (args.KeyCode == Keys.Right) if (direction.Peek() != 3) direction.Enqueue(1);
                        if (args.KeyCode == Keys.Down) if (direction.Peek() != 0) direction.Enqueue(2);
                        if (args.KeyCode == Keys.Left) if (direction.Peek() != 1) direction.Enqueue(3);
                    });

                    var th = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                    {
                        try
                        {
                            while (true)
                            {
                                move_body(true);
                                System.Threading.Thread.Sleep(200);
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox.Show("Fail");
                        }

                    }));
                    th.IsBackground = true;
                    th.Start();
                });
                window.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                window.Size = new System.Drawing.Size(200, 200);
                return window;
            })());
        }
    }
}
