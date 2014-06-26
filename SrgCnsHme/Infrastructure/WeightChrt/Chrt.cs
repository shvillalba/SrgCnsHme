using System.Drawing;
using System.IO;
using System.Web.UI.DataVisualization.Charting;

namespace SrgCnsHme.Infrastructure.WeightChrt
{
  public class Chrt : Chart
  {
    public Chrt()
    {
      Palette = ChartColorPalette.BrightPastel;
      BorderlineDashStyle = ChartDashStyle.Solid;
      BackSecondaryColor = Color.White;
      BackGradientStyle = GradientStyle.TopBottom;
      Legends.Add(new ChrtLegend());
      BorderSkin = new BorderSkin { SkinStyle = BorderSkinStyle.Emboss, PageColor = Color.Transparent };
      ChartAreas.Add(new ChrtArea());
    }

    public MemoryStream GenerateChart()
    {
      var memoryStream = new MemoryStream();
      SaveImage(memoryStream, ChartImageFormat.Png);
      memoryStream.Seek(0, SeekOrigin.Begin);
      return memoryStream;
    }
  }
}