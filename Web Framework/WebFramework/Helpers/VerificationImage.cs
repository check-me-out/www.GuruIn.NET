namespace WebFramework.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public static class VerificationImage
    {
        public static byte[] CreateImage(string verificationCode)
        {
            var bitmap = new Bitmap(Convert.ToInt32(Math.Ceiling((double)verificationCode.Length * 19)), 50);
            var graphics = Graphics.FromImage(bitmap);
            byte[] result;
            try
            {
                var random = new Random();
                graphics.Clear(Color.AliceBlue);
                for (int i = 0; i < 25; i++)
                {
                    int x = random.Next(bitmap.Width);
                    int x2 = random.Next(bitmap.Width);
                    int y = random.Next(bitmap.Height);
                    int y2 = random.Next(bitmap.Height);
                    graphics.DrawLine(new Pen(Color.Silver), x, y, x2, y2);
                }

                var font = new Font("Comic Sans MS", 15f, FontStyle.Regular);
                new LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                graphics.DrawString(verificationCode, font, new SolidBrush(Color.Green), 10f, 10f);
                for (int j = 0; j < 100; j++)
                {
                    int x3 = random.Next(bitmap.Width);
                    int y3 = random.Next(bitmap.Height);
                    bitmap.SetPixel(x3, y3, Color.FromArgb(random.Next()));
                }

                graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1);
                var memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, ImageFormat.Gif);

                result = memoryStream.ToArray();
            }
            catch (Exception)
            {
                result = new byte[0];
            }
            finally
            {
                graphics.Dispose();
                bitmap.Dispose();
            }
            return result;
        }

        public static string CreateRandomCode(int codeLength)
        {
            string text = string.Empty;
            string result;
            try
            {
                var array = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
                var num = -1;
                var random = new Random();
                for (int i = 0; i < codeLength; i++)
                {
                    if (num != -1)
                    {
                        random = new Random(i * num * (int)DateTime.Now.Ticks);
                    }
                    int num2 = random.Next(62);
                    if (num != -1 && num == num2)
                    {
                        result = VerificationImage.CreateRandomCode(codeLength);
                        return result;
                    }
                    num = num2;
                    text += array[num2];
                }
                result = text;
            }
            catch (Exception)
            {
                result = text;
            }

            return result;
        }
    }
}