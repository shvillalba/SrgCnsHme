﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SrgCnsHme.Views.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using SrgCnsHme.Infrastructure;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.5.4.0")]
    public static class scHelpers
    {

public static System.Web.WebPages.HelperResult LocalCss(string controller, string name, UrlHelper url)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 4 "..\..\Views\Helpers\scHelpers.cshtml"
 

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "  <link href=\"");



#line 5 "..\..\Views\Helpers\scHelpers.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, url.Content("~/Content/" + controller + "/" + name));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\"   \r\n  rel=\"stylesheet\" type=\"text/css\" />\r\n");



#line 7 "..\..\Views\Helpers\scHelpers.cshtml"

#line default
#line hidden

});

}


    }
}
#pragma warning restore 1591
