using Hurl.Library.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Hurl.Settings.Views.Dialogs;

public sealed partial class ViewRulesDialog : Page
{
    public ViewRulesDialog(Ruleset ruleset)
    {
        this.InitializeComponent();

        Rules = ruleset.Rules?.Select(x => new Rule(x)).ToList();
    }

    public readonly List<Rule>? Rules;
}
