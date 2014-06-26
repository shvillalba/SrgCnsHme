using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using HtmlAgilityPack;

namespace SrgCnsHme.Infrastructure
{
  public static class ShvExtensions
  {
    public static int ToPersonId(this string name)
    {
      switch (name)
      {
        case "Sergio":
          return 1;
        case "Consuelo":
          return 2;
        default:
          return -1;
      }
    }

    #region code may have to be moved to LearnMvc4 project since SrgCnsHme is being uploaded to AppHarbor, which can not work with LinqPad
    private static MvcHtmlString _flush;
    static string Title(string title)
    {
      return "<h5 class='greenish'>" + title + "</h5>";
    }

    //public static T Dump<T>(this T o, string title = "")
    //{
    //  using (var writer = LINQPad.Util.CreateXhtmlWriter(true))
    //  {
    //    writer.Write(o);
    //    var doc = new HtmlDocument();
    //    doc.LoadHtml(writer.ToString());
    //    var html = "";
    //    if (o.ToString() != "")
    //      try
    //      {
    //        var tbl = doc.DocumentNode.SelectNodes("//table")[0];
    //        html = tbl.OuterHtml;
    //      }
    //      catch (Exception)
    //      {
    //        html = doc.DocumentNode.SelectNodes("//body")[0].InnerHtml;
    //      }
    //    _flush = MvcHtmlString.Create(_flush + Title(title) + html);
    //    return o;
    //  }
    //}

    //public static MvcHtmlString Dumpx<T>(this T o, string title = "")
    //{
    //  using (var writer = LINQPad.Util.CreateXhtmlWriter(true))
    //  {
    //    writer.Write(o);
    //    var otherTbs = _flush;
    //    _flush = null;
    //    var doc = new HtmlDocument();
    //    doc.LoadHtml(writer.ToString());
    //    var html = "";
    //    if (o.ToString() != "")
    //      try
    //      {
    //        var tbl = doc.DocumentNode.SelectNodes("//table")[0];
    //        html = tbl.OuterHtml;
    //      }
    //      catch (Exception)
    //      {
    //        html = doc.DocumentNode.SelectNodes("//body")[0].InnerHtml;
    //      }
    //    return MvcHtmlString.Create(otherTbs + Title(title) + html);
    //  }
    //}
    #endregion
    public static MvcHtmlString Img(this HtmlHelper helper, string src, string alt = "",
    int height = 100, int width = 100, string imgClass = "")
    {
      var builder = new TagBuilder("img");
      builder.MergeAttribute("src", src);
      builder.MergeAttribute("alt", alt);

      //LEARN: changed it to style so a previous css rule setting height/width would be over-ruled.
      //builder.MergeAttribute("height", height.ToString(CultureInfo.InvariantCulture) +"px");
      //builder.MergeAttribute("width", width.ToString(CultureInfo.InvariantCulture) +"px");
      var h = height.ToString(CultureInfo.InvariantCulture) + "px";
      var w = width.ToString(CultureInfo.InvariantCulture) + "px";

      builder.MergeAttribute("style", "height:" + h + ";width:" + w);
      builder.MergeAttribute("class", imgClass);
      return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
    }

    /// <summary>
    /// Converts an enum type to an IDictionary&lt;int,string&gt;
    /// Although this will appear as an extension method on all type objects
    /// it will throw an exception on all types except enum
    /// For Details, see here: http://guyellisrocks.com/coding/asp-net-mvc-dropdownlist-from-enum/
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IDictionary<int, string> EnumToDictionary(this Type type)
    {
      if (!type.IsEnum)
      {
        throw new InvalidCastException("The EnumToDictionary() extension method can only be used on types of enum. All other types will throw this exception.");
      }

      // The Enum.GetValues() function returns an array of objects which are actually ints,
      // that's why we cast them to ints before calling the ToDictionary function.
      // The ToDictionary() extension method takes two lambda expressions each of which
      // returns the type that corresponds to the dictionary's types. Because we've cast
      // the array to ints the first param in the ToDictionary is used as is. The second
      // param gets the name of the Enum.
      Dictionary<int, string> dictionary =
          Enum.GetValues(type).Cast<int>().ToDictionary(a => a, a => Enum.GetName(type, a));

      return dictionary;
    }

  }
}