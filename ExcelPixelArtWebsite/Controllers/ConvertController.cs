using ExcelPixelArt;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelPixelArtWebsite.Controllers {
    public class ConvertController : Controller {

        protected override void OnResultExecuted(ResultExecutedContext filterContext) {
            GC.Collect();
            base.OnResultExecuted(filterContext);
        }


        [HttpPost]
        public FileResult Post() {
            if (Request.Files.Count == 0) {
                throw new ArgumentException("遺漏上傳檔案");
            }

            var Name = GetFileName(Request.Files[0].FileName);

            string[] whiteList = new string[] { "bmp", "jpg", "png", "gif" };

            if (!whiteList.Contains(Name.Item2.ToLower())) {
                throw new ArgumentException("不正確的圖片格式，必須為" + string.Join(",", whiteList));
            }

            var image = new Bitmap(Request.Files[0].InputStream);

            var converter = new PixelArtConverter();
            if (image.Width > 200 || image.Height > 200) {
                if (image.Width > image.Height) {
                    var zoom = 200.0 / image.Width;
                    image = converter.Zoom(image, 200, (int)(image.Height * zoom));
                } else {
                    var zoom = 200.0 / image.Height;
                    image = converter.Zoom(image, (int)(image.Width * zoom), 200);
                }
            }
            
            Stream result = converter.Convert(image);
            return File(result, "application/vnd.ms-excel", Name.Item1 + ".xlsx");
        }

        private Tuple<string, string> GetFileName(string FullName) {
            var temp = FullName.Split('.');
            var Ext = "";
            if (temp.Length > 1) {
                Ext = temp.Last();
            }

            var Name = "";
            if (temp.Length > 1) {
                Name = string.Join(".", temp.Take(temp.Length - 1));
            } else {
                Name = FullName;
            }

            return new Tuple<string, string>(Name, Ext);
        }
    }
}