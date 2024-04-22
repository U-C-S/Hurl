using Hurl.BrowserSelector.Globals;
using Hurl.Library;
using System.Diagnostics;

namespace Hurl.BrowserSelector.Helpers
{
    internal class AutoRulesCheck
    {
        public static bool Start(string link)
        {
            var settings = SettingsGlobal.Value;
            if (settings?.Rulesets == null) return false;

#if DEBUG
            Stopwatch sw = new();
            sw.Start();
#endif
            foreach (var rules in settings.Rulesets)
            {
                var isHurl = rules.BrowserName == "_Hurl";
                if (isHurl && RuleMatch.CheckMultiple(link, rules.Rules))
                {
                    return false;
                }
                else
                {
                    var x = settings.Browsers.Find(x => x.Name == rules.BrowserName);
                    if (x != null)
                    {
                        if (RuleMatch.CheckMultiple(link, rules.Rules))
                        {
                            string Args = string.IsNullOrEmpty(x.LaunchArgs) ? link : link + " " + x.LaunchArgs;
                            Process.Start(x.ExePath, Args);

                            return true;
                        }
                    }
                }
            }
#if DEBUG
            sw.Stop();
            Debug.WriteLine(sw.Elapsed.TotalSeconds);
#endif
            return false;
        }
    }
}
