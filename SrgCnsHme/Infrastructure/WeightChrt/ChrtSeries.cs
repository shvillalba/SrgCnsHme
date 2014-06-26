using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace SrgCnsHme.Infrastructure.WeightChrt
{
  public class ChrtSeries : Series
  {
    public ChrtSeries()
    {
      BorderColor = Color.FromArgb(180, 26, 59, 105);
      XValueType = ChartValueType.Date;
      YValueType = ChartValueType.Double;
      LabelFormat = "{0:000.0}";
      MarkerStyle = MarkerStyle.Diamond;
      IsVisibleInLegend = true;
      ChartType = SeriesChartType.Line;
    }
  }
}