using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Abp.Authorization;
using HC.WeChat.Configuration;
using HC.WeChat.Controllers;
using HC.WeChat.Dto;
using HC.WeChat.Models.WeChat;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.DrawingCore.Imaging;
using System.DrawingCore;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace HC.WeChat.Web.Host.Controllers
{
    public class WeChatFileController : WeChatControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public WeChatFileController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        public async Task<IActionResult> BanquetPhotoSave(IFormFile imgs)
        {
            var date = Request;
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            WeChatFile result = new WeChatFile();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    result.Uid = Guid.NewGuid().ToString();
                    string newFileName = result.Uid + "." + fileExt; //随机生成新的文件名
                    var fileDire = webRootPath + "/upload/BanquetPhotos/";
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    var filePath = fileDire + newFileName;

                    result.Url = filePath;
                    result.Name = newFileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(result);
        }

        [AbpAllowAnonymous]
        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var data = Request.Form.Files["imgs"];
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var fileDire = webRootPath + "/upload/BanquetPhotos/";
            var filePath = fileDire + "a.jpg";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await data.CopyToAsync(stream);
            }
            return Ok(new { msg = "OK" });
        }

        [AbpAllowAnonymous]
        public FileResult ExportExcel()
        {
            //var newFile = @"newbook.core.xlsx";

            //using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            using (var fs = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("Sheet1");
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                //ICreationHelper cH = wb.GetCreationHelper();
                var rowIndex = 0;
                IRow row = sheet1.CreateRow(rowIndex);
                row.Height = 30 * 80;
                var cell = row.CreateCell(0);
                var font = workbook.CreateFont();
                font.IsBold = true;
                font.Color = HSSFColor.DarkBlue.Index2;
                cell.CellStyle.SetFont(font);

                cell.SetCellValue("A very long piece of text that I want to auto-fit innit, yeah. Although if it gets really, really long it'll probably start messing up more.");
                sheet1.AutoSizeColumn(0);
                rowIndex++;

                // 新增試算表。
                var sheet2 = workbook.CreateSheet("My Sheet");
                // 建立儲存格樣式。
                var style1 = workbook.CreateCellStyle();
                style1.FillForegroundColor = HSSFColor.Blue.Index2;
                style1.FillPattern = FillPattern.SolidForeground;

                var style2 = workbook.CreateCellStyle();
                style2.FillForegroundColor = HSSFColor.Yellow.Index2;
                style2.FillPattern = FillPattern.SolidForeground;

                // 設定儲存格樣式與資料。
                var cell2 = sheet2.CreateRow(0).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(0);

                cell2 = sheet2.CreateRow(1).CreateCell(0);
                cell2.CellStyle = style2;
                cell2.SetCellValue(1);

                cell2 = sheet2.CreateRow(2).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(2);

                cell2 = sheet2.CreateRow(3).CreateCell(0);
                cell2.CellStyle = style2;
                cell2.SetCellValue(3);

                cell2 = sheet2.CreateRow(4).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(4);

                workbook.Write(fs);
                return File(fs, "application/ms-excel", "core.xlsx");
            }
        }


        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        public async Task<IActionResult> MarketingInfoPosts(IFormFile[] image, string fileName, Guid name)
        {
            //var files = Request.Form.Files;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var imageName = "";
            foreach (var formFile in image)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    //var i = 0;
                    //fileName = fileName + new DateTime().ToString("yyMMddHH") + i++.ToString();
                    name = name == Guid.Empty ? Guid.NewGuid() : name;
                    string newName = name + fileExt; //新的文件名
                    var fileDire = webRootPath + string.Format("/upload/{0}/", fileName);
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    var filePath = fileDire + newName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    imageName = filePath.Substring(webRootPath.Length);
                }
            }

            return Ok(new { imageName });
        }

        /// <summary>
        /// HTML中图片上传
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        public async Task<IActionResult> MarketingHTMLContentPosts(IFormFile[] file)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string location = Guid.NewGuid().ToString();
            foreach (var formFile in file)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    string newName = location + fileExt; //新的文件名
                    var fileDire = webRootPath + string.Format("/gawechat/imgs/");
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    var filePath = fileDire + newName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    location = filePath.Substring(webRootPath.Length);
                }
            }
            return Ok(new { location });
        }

        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        [AbpAllowAnonymous]
        //public async Task<IActionResult> FilesPosts(IFormFile[] files, string folder)
        public async Task<IActionResult> FilesPosts(string folder, string newName)
        {
            var files = Request.Form.Files;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var saveUrl = "";
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string fileName = newName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = Guid.NewGuid().ToString();
                    }
                    string newFileName = fileName + fileExt; //新的文件名
                    var fileDire = webRootPath + string.Format("/upload/{0}/", folder);
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    var filePath = fileDire + newFileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    saveUrl = filePath.Substring(webRootPath.Length);
                }
            }
            return Ok(saveUrl);
        }

        [HttpPost]
        [AbpAllowAnonymous]
        //public async Task<IActionResult> FilesPostsBase64([FromBody]WechatImgBase64 input)
        public Task<IActionResult> FilesPostsBase64([FromBody]WechatImgBase64 input)
        {
            if (!string.IsNullOrWhiteSpace(input.imageBase64))
            {
                var reg = new Regex("data:image/(.*);base64,");
                input.imageBase64 = reg.Replace(input.imageBase64, "");
                byte[] imageByte = Convert.FromBase64String(input.imageBase64);
                //var memorystream = new MemoryStream(imageByte);

                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                string fileExt = Path.GetExtension(input.fileName); //文件扩展名，不含“.”
                string newFileName = Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                var fileDire = webRootPath + "/upload/shop/";
                if (!Directory.Exists(fileDire))
                {
                    Directory.CreateDirectory(fileDire);
                }

                var filePath = fileDire + newFileName;
                //2018-7-6 压缩后保存
                using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(imageByte))
                {
                    //如果高度大于200 就需要压缩
                    if (image.Height > 200)
                    {
                        var width = (int)((200 / image.Height) * image.Width);
                        image.Mutate(x => x.Resize(width, 200));
                    }
                    image.Save(filePath);
                }
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await memorystream.CopyToAsync(stream);
                //}
                var saveUrl = filePath.Substring(webRootPath.Length);
                return Task.FromResult((IActionResult)Json(new APIResultDto() { Code = 0, Msg = "上传数据成功", Data = saveUrl }));
            }
            return Task.FromResult((IActionResult)Json(new APIResultDto() { Code = 901, Msg = "上传数据不能为空" }));
        }

        /// <summary>
        /// 后台导入数据
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        public async Task<IActionResult> MarketingExcelInfoPosts(IFormFile[] files, string fileName)
        {
            //var files = Request.Form.Files;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = fileName + fileExt; //新的文件名
                    var fileDire = webRootPath + "/upload/files/";
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    var filePath = fileDire + newFileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok();
        }

        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        //[HttpPost]
        [AbpAllowAnonymous]
        //public async Task<IActionResult> FilesPosts(IFormFile[] files, string folder)
        public async Task<string> FilesImgPostsCompress(IFormFileCollection files, string folder, string newName)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            var saveUrl = "";
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string fileName = newName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = Guid.NewGuid().ToString();
                    }
                    string newFileName = fileName + fileExt; //新的文件名
                    var fileDire = webRootPath + string.Format("/upload/{0}/", folder);
                    if (!Directory.Exists(fileDire))
                    {
                        Directory.CreateDirectory(fileDire);
                    }

                    var filePath = fileDire + newFileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    saveUrl = filePath;

                }
            }
            return saveUrl;
        }

        [RequestFormSizeLimit(valueCountLimit: 2147483647)]
        [HttpPost]
        [AbpAllowAnonymous]
        //public async Task<IActionResult> FilesPosts(IFormFile[] files, string folder)
        public async Task<IActionResult> FilesPostsCompress(string folder, bool isCompress = false)
        {
            var saveUrl = "";
            var files = Request.Form.Files;
            string webRootPath = _hostingEnvironment.WebRootPath;
            var url = await FilesImgPostsCompress(files, folder, null);
            var endRote= string.Format("/upload/{0}/", folder + "com");
            var fileDireCom = webRootPath + endRote;
            if (!Directory.Exists(fileDireCom))
            {
                Directory.CreateDirectory(fileDireCom);
            }
            if (isCompress)
            {
                fileDireCom = fileDireCom + url.Substring(webRootPath.Length + endRote.Length);
                Bitmap sourceImage = new Bitmap(url);
                Compress(sourceImage, fileDireCom, 50L);
                saveUrl = fileDireCom.Substring(webRootPath.Length);
            }
            else {
                saveUrl = url.Substring(webRootPath.Length);
            }

            return Ok(saveUrl);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitmap">传入的Bitmap对象</param>
        /// <param name="destStream">压缩后的Stream对象</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with 给定的 quality level
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            float xWidth = srcBitmap.Width;
            float yWidth = srcBitmap.Height;
            //当图片大于宽度大于750时对图片的宽度进行压缩
            if (xWidth > 780)
            {
                var nyWidth = (yWidth / xWidth) * 780;
                Bitmap newImage = new Bitmap((int)(780), (int)(nyWidth));
                Graphics g = Graphics.FromImage(newImage);

                g.DrawImage(srcBitmap, 0, 0, 780, nyWidth);
                g.Dispose();
                srcBitmap = newImage;
            }
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);

            //Bitmap bmp1 = new Bitmap(@"C:\Users\lt\Desktop\测试图片\2167006.jpg");

            //myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //bmp1.Save(@"D:\CodeWord\GuangAn\GAWeChat\aspnet-core\src\HC.WeChat.Web.Host\wwwroot\upload\feedbackcom\test50.jpg", myImageCodecInfo, myEncoderParameters);

           
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitMap">传入的Bitmap对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level);
            s.Close();
        }    
    }
}