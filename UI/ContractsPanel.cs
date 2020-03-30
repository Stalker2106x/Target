using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TargetGame.Entities;

namespace TargetGame.UI
{
  public class ContractsPanel
  {
    VerticalStackPanel _panel;
    List<Tuple<Label, Contract>> _indicators;

    public ContractsPanel()
    {
      _indicators = new List<Tuple<Label, Contract>>();
      _panel = new VerticalStackPanel();
      Desktop.Widgets.Add(_panel);
    }

    public void AddIndicator(Contract contract)
    {
      Stylesheet.Current.LabelStyle.Font = Resources.regularFont;
      Label indicator = new Label();
      indicator.HorizontalAlignment = HorizontalAlignment.Right;
      indicator.VerticalAlignment = VerticalAlignment.Top;
      indicator.Text = contract.GetMission() + " Reward: " + contract.reward.GetDescription();
      _panel.Widgets.Add(indicator);
      _indicators.Add(new Tuple<Label, Contract>(indicator, contract));
    }
    public void RemoveIndicator(Contract contract)
    {
      Tuple<Label, Contract> indicator = _indicators.First((it) => { return (it.Item2 == contract); });
      _panel.Widgets.Remove(indicator.Item1);
      _indicators.Remove(indicator);
    }

    public void Update()
    {
      foreach (Tuple<Label, Contract> indicator in _indicators) indicator.Item1.Text = indicator.Item2.GetMission() + " Reward: " + indicator.Item2.reward.GetDescription();
    }
  }
}
