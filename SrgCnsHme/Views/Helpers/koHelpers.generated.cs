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
    public static class koHelpers
    {

public static System.Web.WebPages.HelperResult KoTextTd(string name, string extraClasses = "", string extraAttributes = "")
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 4 "..\..\Views\Helpers\koHelpers.cshtml"
 

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "  <td class=\"pointerCursor\"\r\n      data-bind=\"visible:!inEditMode(), text: ");



#line 6 "..\..\Views\Helpers\koHelpers.cshtml"
   WebViewPage.WriteTo(@__razor_helper_writer, name);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "  ,  css: {markedForDeletion: markedForDeletion}\" ></td>\r\n");



WebViewPage.WriteLiteralTo(@__razor_helper_writer, "  <td data-bind=\"visible: inEditMode()\">\r\n    <input type=\"text\" \r\n      class=\"e" +
"ditCursor middle span2 wdth90 ");



#line 9 "..\..\Views\Helpers\koHelpers.cshtml"
 WebViewPage.WriteTo(@__razor_helper_writer, extraClasses);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" ");



#line 9 "..\..\Views\Helpers\koHelpers.cshtml"
                WebViewPage.WriteTo(@__razor_helper_writer, extraAttributes);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, " \r\n      data-bind=\"value: ");



#line 10 "..\..\Views\Helpers\koHelpers.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, name);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" />\r\n  </td>\r\n");



#line 12 "..\..\Views\Helpers\koHelpers.cshtml"

#line default
#line hidden

});

}


    }
}
#pragma warning restore 1591
