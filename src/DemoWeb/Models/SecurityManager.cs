using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DemoWeb.Models
{
    public class SecurityManager
    {
        //此處使用web.config設定允許存取來源IP，實務上可改用DB存放並加上管理介面
        private static string[] allowedClientIps =
            (ConfigurationManager.AppSettings["api:AllowedClientIps"] ?? string.Empty)
            .Split(',', ';');
        public static void Authorize(HttpRequestBase request)
        {
            if (!allowedClientIps.Contains(request.UserHostAddress))
                throw new ApplicationException("Client IP Denied");
            //如要求更嚴謹管控時可發放API Key，並要求附於Request Header
            //在此可檢查API Key是否合法，甚至API Key再綁定特定IP使用
            //...request.Cookies["X-Api-Key"]...
        }
    }
}