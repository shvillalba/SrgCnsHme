using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using SrgCnsHme.Models;

namespace SrgCnsHme.Infrastructure.WeightChrt
{
  public sealed class WeightChrt: Chrt
  {
    public WeightChrt()
    {
      ID = "WeightChart";
      Width = 900;
      Height = 420;
      BackColor = Color.FromArgb(211, 223, 240);
      BorderWidth = 2;
      BorderColor = Color.FromArgb(26, 59, 105);
      Titles.Add(new ChrtTitle { Name = "Title1", Text = "S & C Weight Record" });
    }

    public void AddWeights(List<Weight> wts)
    {
      var name = wts[0].Person.ShortName;
      var color = Color.FromArgb(101, 161, 240);
      if (name == "Consuelo")
        color = Color.FromArgb(242, 175, 68);
      var ser = new ChrtSeries { Name = name + "WeightSeries", LegendText = name, Color = color};
      foreach (var wt in wts)
      {
        var p = new DataPoint();
        p.SetValueXY(wt.Date, wt.WeightInLb);
        ser.Points.Add(p);
      }
      Series.Add(ser);
    }
  }
}