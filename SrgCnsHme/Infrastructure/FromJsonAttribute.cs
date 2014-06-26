using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SrgCnsHme.Infrastructure
{
  public class FromJsonAttribute : CustomModelBinderAttribute
  {
    private readonly static JavaScriptSerializer Serializer = new JavaScriptSerializer();

    public override IModelBinder GetBinder()
    {
      return new JsonModelBinder();
    }

    private class JsonModelBinder : IModelBinder
    {
      public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
      {
        var stringified = controllerContext.HttpContext.Request[bindingContext.ModelName];
        return string.IsNullOrEmpty(stringified) ? null : Serializer.Deserialize(stringified, bindingContext.ModelType);
      }
    }
  }
}