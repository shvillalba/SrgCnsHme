using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;


namespace SrgCnsHme.Infrastructure.WeightChrt
{
  public class ChrtTitle : Title
  {
    public ChrtTitle()
    {
      TextStyle = TextStyle.Shadow;
      Font = new Font("Trebuchet MS", 14f, FontStyle.Bold);
      Alignment = ContentAlignment.TopRight;
      IsDockedInsideChartArea = false;
    }
  }
}