namespace Hurl.BrowserSelector.Globals
{
    internal class RuleGlobal
    {
        private static RuleGlobal _instance = new();

        private string _rule;

        public static string Value
        {
            get
            {
                return _instance._rule;
            }
            set
            {
                _instance._rule = value;
            }
        }
    }
}
