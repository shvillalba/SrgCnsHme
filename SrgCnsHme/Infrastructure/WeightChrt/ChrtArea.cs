using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace SrgCnsHme.Infrastructure.WeightChrt
{
  public sealed class ChrtArea : ChartArea
  {
    public ChrtArea()
    {
      Name = "WeightChartArea";
      BorderColor = Color.FromArgb(64, 64, 64, 64);
      BorderDashStyle = ChartDashStyle.Solid;
      BackSecondaryColor = Color.White;
      BackColor = Color.FromArgb(64, 165, 191, 228);
      ShadowColor = Color.Transparent;
      BackGradientStyle = GradientStyle.TopBottom;
      Area3DStyle = new ChartArea3DStyle
      {
        LightStyle = LightStyle.Realistic,
        PointDepth = 350,
        IsRightAngleAxes = false,
        WallWidth = 0,
        IsClustered = false
      };
      Position = new ElementPosition { Y = 18, Height = 80, Width = 94, X = 2 };
      AxisY = new Axis
      {
        Interval = 2,
        LabelStyle = { Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold) },
        LineColor = Color.FromArgb(64, 64, 64, 64),
        MajorGrid = { LineColor = Color.FromArgb(64, 64, 64, 64) },
        Minimum = 125,
        Maximum = 163
      };
      AxisX = new Axis
      {
        LineColor = Color.FromArgb(64, 64, 64, 64),
        LabelAutoFitMaxFontSize = 8,
        Interval = 1,
        IsMarginVisible = false,
        LabelStyle =
        {
          Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold),
          Format = "{0:M/d/yy}",
          IsEndLabelVisible = true
        },
        MajorGrid = { LineColor = Color.FromArgb(64, 64, 64, 64) }
      };
    }
  }
}