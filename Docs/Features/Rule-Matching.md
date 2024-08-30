## About
### What are Rules ?
They are text strings, which when configured can automatically open a browser for you, instead of prompting for selecting a browser.. Thus saving a extra step. Each time, you open a link in Hurl, It goes through all the configured rules to see if any matches with link and if it matches, It automatically opens the associated Browser.

### What about Rulesets ?
Each Ruleset is a collection of rules. You can have as many rules as you want inside a Ruleset. Each Ruleset is associated to a Browser and You can have multiple rulesets for same browser. Additionally, Rulesets prioritized as per the ordering. If same rule exists in two different rulesets, the ruleset that's higher in the list is triggered first.

## Types of Rules
- `String`: This does simple text matching. Triggers if your URL matches exactly with this plain rule.
- `Domain`: Simply the domain part of a URL, ex: For the URL `https://github.com/u-c-s/hurl`, `github.com` is the domain. Probably the most useful rule type.
- `Regex`: Uses Regular Expressions to do text matching. Use https://regex101.com with (.NET/C# flavor) to test your rules.
- `Glob`: (Prefer using `Domain` rule over this, possibly could get breaking changes or removed completely in future versions) Uses Glob Pattern matching. Use https://www.digitalocean.com/community/tools/glob to test your rules

> Note that URLs typically have `https://` prefix which also need to be taken care by the rules you have written. 

## From UI

Adding new rule can be done from UI by clicking `+ Rules` button in the Hurl main window.

- Use `Create` option to create a new Ruleset
  - Choose the Browser and the available Alternate Launches
  - Add any number of Rules with any rule type by selecting a rule type and entering the rule for each.
  - Save!
- Use `Test` to the newly added rules
  - Enter the URL you want to test the rules with.
  - `Test against Existing Rules` will test URL with above added rules.
  - `Test with Rule` option can test against the specific rule you entered here.
- Note that, you can rearrange Rulesets ordering from the UI, but not the rules in the ruleset itself.


## From UserSettings.json

You can add new rules into `AutoRoutingRules` property, which itself is a list of Rules Objects. Each rule object should contain 2 properties 
  - `Rules` : it can be a single rule or multiple rules. (rules are prioritiesed, so first rule that matches will trigger the opening of browser)
  - `BrowserName`: The value should be a Browser name that matches one of the `Browsers` names. which will be opened once a rule from the `Rules` property matches. Use `_Hurl` value to open Hurl
  - `Name` To name the ruleset. Only for visual purposes. does not have any functionality.
  - `AltLaunchIndex` to select the Alt Launch for a browser. It's a index number.

Sample Json file for more advanced editing.... 
```json
{
  "Browsers": [
    {
      "Name": "Firefox Nightly",
      "ExePath": "C:\\Program Files\\Firefox Nightly\\firefox.exe"
    },
    {
      "Name": "Google Chrome Dev",
      "ExePath": "C:\\Program Files\\Google\\Chrome Dev\\Application\\chrome.exe"
    }
  ],
  "Rulesets": [
    {
      "Name": "Googly",
      "Rules": ["g$*.google.com*"],
      "BrowserName": "Google Chrome Dev"
    },
    {
      "Rules": ["https://github.com/u-c-s", "r$.*open\\.spotify\\.com.*"],
      "BrowserName": "Firefox Nightly",
    }
  ]
}
```

Note that when adding Rules to UserSettings.json directly, follow the below pattern.

| Rule Type | Format | Example |
| --- | --- | --- | 
| String | `s$__your_rule__` or `__your_rule__` | `https://github.com/U-C-S` |
| Domain | `d$__your_rule__` | `d$github.com` |
| Regex  | `r$__your_rule__` | `r$.*open\\.spotify\\.com.*` |
| Glob   | `g$__your_rule__` | `g$*.google.com*` |
