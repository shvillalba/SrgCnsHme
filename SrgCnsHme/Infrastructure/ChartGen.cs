using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using SrgCnsHme.Models;

namespace SrgCnsHme.Infrastructure
{
  /// <summary>
  /// Chart generator sample that uses random numbers to show a "stock price" sequence of "prices"
  /// For details, see: http://guyellisrocks.com/coding/create-a-chart-using-net-4-and-asp-net-mvc/
  /// </summary>
  public class ChartGen
  {
    public Chart Chart { get; set; }
    public MemoryStream GenerateChart(int symbolId)
    {
      var priceList = GetPrices().ToList();

      Chart = new Chart
      {
        BorderSkin = { SkinStyle = BorderSkinStyle.Emboss },
        BackColor = ColorTranslator.FromHtml("#D3DFF0"),
        BorderlineDashStyle = ChartDashStyle.Solid,
        Palette = ChartColorPalette.BrightPastel,
        BackSecondaryColor = Color.White,
        BackGradientStyle = GradientStyle.TopBottom,
        BorderlineWidth = 2,
        BorderlineColor = Color.FromArgb(26, 59, 105),
        Width = Unit.Pixel(500),
        Height = Unit.Pixel(300)
      };
      Chart.Customize += chart_Customize;

      var series1 = new Series("Series1")
      {
        ChartArea = "ca1",
        ChartType = SeriesChartType.Candlestick,
        Font = new Font("Verdana", 8.25f, FontStyle.Regular),
        BorderColor = Color.FromArgb(180, 26, 59, 105)
      };

      foreach (var dayBar in priceList)
      {
        var upDay = dayBar.Open < dayBar.Close;
        series1.Points.Add(new DataPoint
        {
          BackSecondaryColor = upDay ? Color.LimeGreen : Color.Red,
          BorderColor = Color.Black,
          Color = upDay ? Color.LimeGreen : Color.Red,
          AxisLabel = dayBar.Date.ToString("dd-MMM-yy"),
          YValues = new[] { dayBar.High, dayBar.Low, dayBar.Open, dayBar.Close }
        });
      }

      Chart.Series.Add(series1);

      var ca1 = new ChartArea("ca1")
      {
        BackColor = Color.FromArgb(64, 165, 191, 228),
        BorderColor = Color.FromArgb(64, 64, 64, 64),
        BorderDashStyle = ChartDashStyle.Solid,
        BackSecondaryColor = Color.White,
        ShadowColor = Color.Transparent,
        BackGradientStyle = GradientStyle.TopBottom,
        Area3DStyle =
        {
          Rotation = 10,
          Perspective = 10,
          Inclination = 15,
          IsRightAngleAxes = false,
          WallWidth = 0,
          IsClustered = false
        },
        AxisY = { LineColor = Color.FromArgb(64, 64, 64, 64) }
      };

      ca1.AxisX.MajorGrid.LineColor = Color.Transparent;
      ca1.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 255);
      ca1.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

      var max = priceList.Select(a => a.High).Max();
      var min = priceList.Select(a => a.Low).Min();
      var rangeAdjust = (max - min) * 0.03;
      max += rangeAdjust;
      min -= rangeAdjust;
      ca1.AxisY.Minimum = min;
      ca1.AxisY.Maximum = max;

      Chart.ChartAreas.Add(ca1);

      var memoryStream = new MemoryStream();
      Chart.SaveImage(memoryStream, ChartImageFormat.Png);
      memoryStream.Seek(0, SeekOrigin.Begin);

      return memoryStream;
    }

    void chart_Customize(object sender, EventArgs e)
    {
      var yAxisLabels = Chart.ChartAreas["ca1"].AxisY.CustomLabels;

      foreach (var t in yAxisLabels)
      {
        var price = Convert.ToDecimal(t.Text);
        // Do your formatting of price here
        t.Text = (price / 100).ToString("0.00");
      }
    }

    readonly Random _r = new Random((int)DateTime.Now.Ticks);
    IEnumerable<ChartSamplePrice> GetPrices()
    {
      var close = _r.Next(4000, 6000);
      for (var i = -20; i < 1; i++)
      {
        var open = _r.Next(close - 30, close + 30);
        close = _r.Next(open - 70, open + 70);
        var high = Math.Max(open, close);
        high = _r.Next(high, high + 100);
        var low = Math.Min(open, close);
        low = _r.Next(low - 100, low);

        yield return new ChartSamplePrice
        {
          Date = DateTime.Now.AddDays(i).Date,
          Open = open,
          High = high,
          Low = low,
          Close = close
        };
      }
    }
  }
}



