using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace SrgCnsHme.Infrastructure.WeightChrt
{
  public class ChrtLegend : Legend
  {
    public ChrtLegend()
    {
      Enabled = true;
      IsTextAutoFit = false;
      Name = "Default";
      BackColor = Color.Transparent;
      Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold);
      IsDockedInsideChartArea = false;
      Docking = Docking.Top;
      Position = new ElementPosition { Height = 30, Width = 60, Y = 0, X = 0, Auto = false };
    }

    public override sealed string Name
    {
      get { return base.Name; }
      set { base.Name = value; }
    }
  }
}