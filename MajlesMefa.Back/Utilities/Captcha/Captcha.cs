using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MajlesMefa.Back.Utilities.Captcha
{
    public static class Captcha
    {
        //private const string Letters = "۰۱۲۳۴۵۶۷۸۹abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@#$!";
        //private const string PersianNumbers = "۰۱۲۳۴۵۶۷۸۹";

        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@#$!";
        private const string PersianNumbers = "0123456789";

        public static string GenerateCaptchaCode()
        {
            var rand = new Random();
            var maxRand = Letters.Length - 1;
            var persianNumbersCount = PersianNumbers.Length;

            var sb = new StringBuilder();

            // ابتدا یک عدد فارسی تصادفی اضافه می‌کنیم
            var persianIndex = rand.Next(persianNumbersCount);
            sb.Append(PersianNumbers[persianIndex]);

            // سپس ۴ کاراکتر تصادفی از بین تمام کاراکترها اضافه می‌کنیم
            for (var i = 0; i < 4; i++)
            {
                var index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            // حالا کاراکترها را به صورت تصادفی مخلوط می‌کنیم
            return ShuffleString(sb.ToString());
        }

        // تابع برای مخلوط کردن کاراکترهای رشته
        private static string ShuffleString(string input)
        {
            var rand = new Random();
            var chars = input.ToCharArray();

            for (int i = chars.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }

            return new string(chars);
        }



        public static bool ValidateCaptchaCode(string userInputCaptcha, HttpContext context)
        {
            var isValid = string.Equals(userInputCaptcha, context.Session.GetString("CaptchaCode"), StringComparison.CurrentCultureIgnoreCase); //  RC4.Decrypt(key, context.Request.Cookies["_cpCode"]), StringComparison.CurrentCultureIgnoreCase);
            //context.Response.Cookies.Delete("_cpCode");
            return isValid;
        }

        public static (string captchaCode, byte[] captchaByte) GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using var baseMap = new Bitmap(width, height);
            using var graph = Graphics.FromImage(baseMap);
            var rand = new Random();

            graph.Clear(GetRandomLightColor());

            DrawCaptchaCode();
            DrawDisorderLine();
            AdjustRippleEffect();

            var ms = new MemoryStream();

            baseMap.Save(ms, ImageFormat.Png);

            return (captchaCode, ms.ToArray());  //new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray() };

            int GetFontSize(int imageWidth, int captchaCodeCount)
            {
                var averageSize = imageWidth / captchaCodeCount;

                return Convert.ToInt32(averageSize);
            }

            Color GetRandomDeepColor()
            {
                int redlow = 160, greenLow = 100, blueLow = 160;
                return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
            }

            Color GetRandomLightColor()
            {
                const int low = 180;
                const int high = 255;

                var nRend = rand.Next(high) % (high - low) + low;
                var nGreen = rand.Next(high) % (high - low) + low;
                var nBlue = rand.Next(high) % (high - low) + low;

                return Color.FromArgb(nRend, nGreen, nBlue);
            }

            void DrawCaptchaCode()
            {
                var fontBrush = new SolidBrush(Color.Black);
                var fontSize = Math.Min(GetFontSize(width, captchaCode.Length), height - 20);

                // Use more distorted fonts
                FontFamily[] fontFamilies = {
                FontFamily.GenericSerif,
                FontFamily.GenericSansSerif,
                FontFamily.GenericMonospace
            };

                for (var i = 0; i < captchaCode.Length; i++)
                {
                    fontBrush.Color = GetRandomDeepColor();

                    // Random font selection
                    var fontFamily = fontFamilies[rand.Next(fontFamilies.Length)];
                    var font = new Font(fontFamily, fontSize,
                                       FontStyle.Bold | FontStyle.Italic,  // ← Add Italic
                                       GraphicsUnit.Pixel);

                    var shiftPx = fontSize / 4;  // ← Increased distortion range

                    float x = 10 + i * (width - 20) / captchaCode.Length + rand.Next(-shiftPx, shiftPx);
                    x = Math.Clamp(x, 10, width - fontSize - 10);

                    float y = rand.Next(10, height - fontSize - 10);
                    y = Math.Clamp(y, 10, height - fontSize - 10);

                    // Add rotation
                    graph.TranslateTransform(x, y);
                    graph.RotateTransform(rand.Next(-15, 15));
                    graph.DrawString(captchaCode[i].ToString(), font, fontBrush, 0, 0);
                    graph.ResetTransform();
                }
            }

            void DrawDisorderLine()
            {
                var linePen = new Pen(new SolidBrush(Color.Black), 3);

                // Increase from 3-5 to 10-15 lines
                for (var i = 0; i < rand.Next(5, 8); i++)  // ← Increased lines
                {
                    linePen.Color = GetRandomDeepColor();
                    linePen.Width = rand.Next(1, 4);  // ← Vary line thickness

                    var startPoint = new Point(rand.Next(5, width - 5), rand.Next(5, height - 5));
                    var endPoint = new Point(rand.Next(5, width - 5), rand.Next(5, height - 5));
                    graph.DrawLine(linePen, startPoint, endPoint);
                }

                // Add random dots/ellipses
                for (var i = 0; i < rand.Next(20, 30); i++)  // ← Add dots
                {
                    var brush = new SolidBrush(GetRandomDeepColor());
                    var size = rand.Next(2, 5);
                    var x = rand.Next(0, width);
                    var y = rand.Next(0, height);
                    graph.FillEllipse(brush, x, y, size, size); 
                }
            }

            void AdjustRippleEffect()
            {
                const short nWave = 8;
                var nWidth = baseMap.Width;
                var nHeight = baseMap.Height;

                var pt = new Point[nWidth, nHeight];

                for (var x = 0; x < nWidth; ++x)
                {
                    for (var y = 0; y < nHeight; ++y)
                    {
                        var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                        var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                        var newX = x + xo;
                        var newY = y + yo;

                        if (newX > 0 && newX < nWidth)
                        {
                            pt[x, y].X = (int)newX;
                        }
                        else
                        {
                            pt[x, y].X = 0;
                        }


                        if (newY > 0 && newY < nHeight)
                        {
                            pt[x, y].Y = (int)newY;
                        }
                        else
                        {
                            pt[x, y].Y = 0;
                        }
                    }
                }

                var bSrc = (Bitmap)baseMap.Clone();

                var bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                var bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                var scanline = bitmapData.Stride;

                var scan0 = bitmapData.Scan0;
                var srcScan0 = bmSrc.Scan0;

                unsafe
                {
                    var p = (byte*)(void*)scan0;
                    var pSrc = (byte*)(void*)srcScan0;

                    var nOffset = bitmapData.Stride - baseMap.Width * 3;

                    for (var y = 0; y < nHeight; ++y)
                    {
                        for (var x = 0; x < nWidth; ++x)
                        {
                            var xOffset = pt[x, y].X;
                            var yOffset = pt[x, y].Y;

                            if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                            {
                                if (pSrc != null)
                                {
                                    p[0] = pSrc[yOffset * scanline + xOffset * 3];
                                    p[1] = pSrc[yOffset * scanline + xOffset * 3 + 1];
                                    p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
                                }
                            }

                            p += 3;
                        }
                        p += nOffset;
                    }
                }

                baseMap.UnlockBits(bitmapData);
                bSrc.UnlockBits(bmSrc);
                bSrc.Dispose();
            }
        }
    }
}

