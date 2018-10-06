using DemoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class CodecApiController : JsonNetController
    {
        [HttpPost]
        public ActionResult EncryptString(string encKey, string rawText)
        {
            try
            {
                SecurityManager.Authorize(Request);
                return Json(new ApiResult<byte[]>(
                    CodecModule.EncrytString(encKey, rawText)));
            }
            catch (Exception ex)
            {
                return Json(new ApiError("500", ex.Message));
            }
        }

        public class DecryptParameter
        {
            public string EncKey { get; set; }
            public List<byte[]> EncData { get; set; }
        }

        [HttpPost]
        public ActionResult BatchDecryptData(DecryptParameter decData)
        {
            try
            {
                SecurityManager.Authorize(Request);
                return Json(new ApiResult<List<string>>(
                    CodecModule.DecryptData(decData.EncKey, decData.EncData)));
            }
            catch (Exception ex)
            {
                return Json(new ApiError("500", ex.Message));
            }
        }
    }
}