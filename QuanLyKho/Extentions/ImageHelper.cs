﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace QuanLyCTDT.Extentions
{
    public class ImageHelper
    {
        private static ImageHelper _me;
        public static ImageHelper Me
        {
            get
            {
                if (_me == null)

                    _me = new ImageHelper();
                return _me;
            }
        }
        public static bool ResizeImage(string originalFullPath, int contentImageLength, byte[] imageData)
        {
            try
            {
                if ((contentImageLength / 1024) > 300)//kb
                {
                    Bitmap bitmap = new Bitmap(originalFullPath);
                    int iwidth = bitmap.Width;
                    int iheight = bitmap.Height;
                    bitmap.Dispose();
                    System.Drawing.Image objOptImage = new System.Drawing.Bitmap(iwidth, iheight, System.Drawing.Imaging.
                        PixelFormat.Format16bppRgb555);
                    // GET THE ORIGINAL IMAGE.
                    using (System.Drawing.Image objImg = System.Drawing.Image.FromFile(originalFullPath))
                    {
                        using (System.Drawing.Graphics oGraphic = System.Drawing.Graphics.FromImage(objOptImage))
                        {
                            var _1 = oGraphic;
                            System.Drawing.Rectangle oRectangle = new System.Drawing.Rectangle(0, 0, iwidth, iheight);

                            _1.DrawImage(objImg, oRectangle);
                        }
                        objImg.Dispose();
                    }


                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public bool SaveCroppedImage(Image image, string filePath, string fileName, ref Byte[] imageBytes, int maxWidth = 600, int? Width = null, int? Height = null)
        {
            var extension = fileName.Substring(fileName.Length - 3, 3).ToLower();

            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == (extension == "png" ? "image/png" : "image/jpeg")).FirstOrDefault();

            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                int resizedW = 0, resizedH = 0;
                if (Width.HasValue && Height.HasValue)
                {
                    resizedW = Width.Value;
                    resizedH = Height.Value;
                }
                else
                {
                    int originalW = image.Width;
                    int originalH = image.Height;
                    resizedW = maxWidth;
                    resizedH = (originalH * resizedW) / originalW;
                }
                bitmap = new System.Drawing.Bitmap(resizedW, resizedH);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }
                finalImage = bitmap;
            }
            catch { }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                    //quality should be in the range 
                    finalImage.Save(filePath, jpgInfo, encParams);
                    //get  byte 
                    imageBytes = File.ReadAllBytes(filePath);
                    return true;
                }
            }
            catch (Exception ex) { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            //imageBytes = null;
            return false;
        }
        private string GetNewPathForDupesOnServer(string path, ref string fn)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int counter = 1;
            string newFullPath = path;
            string new_file_name = filename + extension;
            while (System.IO.File.Exists(newFullPath))
            {
                string newFilename = string.Format("{0}({1}){2}", filename, counter, extension);
                new_file_name = newFilename;
                newFullPath = Path.Combine(directory, newFilename);
                counter++;
            };
            fn = new_file_name;
            return newFullPath;
        }

        public void TryDeleteImageUpdated(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
