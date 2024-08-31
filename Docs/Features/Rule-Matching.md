# About rules

## What are rules?

They are text strings, which when configured can automatically open a browser for you, instead of prompting for selecting a browser. Thus saving an extra step. Each time you open a link in Hurl, it goes through all the configured rules to see if any matches with the URL and if it matches, it automatically opens the associated browser.

### Types of rules

- `String`: This does simple text matching. Triggers if your URL matches exactly with this plain rule.
- `Domain`: Simply the domain part of a URL, example: For the URL `https://github.com/u-c-s/hurl`, `github.com` is the domain. Probably the most useful rule type.
- `Regex`: Uses Regular Expressions to do text matching. Use <https://regex101.com> with (.NET/C# flavor) to test your rules.
- `Glob`: (Prefer using `Domain` rule over this, possibly could get breaking changes or removed completely in future versions) Uses Glob Pattern matching. Use [this website](https://www.digitalocean.com/community/tools/glob) to test your rules.

> [!NOTE]
> URLs typically have the `https://` prefix which also needs to be taken care by the rules you have written.

## What about rulesets?

Each ruleset is a collection of rules. You can have as many rules as you want inside a ruleset. Each ruleset is associated to a browser and you can have multiple rulesets for same browser. Additionally, rulesets are prioritized as per the ordering. If the same rule exists in two different rulesets, the ruleset that's higher in the list is triggered first.

## From UI

Adding a new rule can be done from UI by selecting **Rules** in the Hurl main window.

- Select **Create** to create a new ruleset
  - Choose the browser and the available Alternate Launches
  - Add any number of rules with any rule type by selecting a rule type and entering the rule for each
  - Save
- Select **Test** to the newly added rules
  - Enter the URL you want to test the rules with.
  - **Test against existing rules** will test URL with above added rules.
  - **Test with rule** will test against the specific rule you entered here.
- Note that you can rearrange rulesets ordering from the UI, but not the rules in the ruleset itself.

## From UserSettings.json

You can add new rules into `AutoRoutingRules` property, which itself is a list of Rules Objects. Each rule object should contain 2 properties:

- `Rules`: It can be a single rule or multiple rules. Rules are prioritized: the first rule that matches will trigger the opening of browser.
- `BrowserName`: The value should be a Browser name that matches one of the `Browsers` names. It will be opened once a rule from the `Rules` property matches. Use `_Hurl` to open Hurl.
- `Name`: To name the ruleset. Only for visual purposes.
- `AltLaunchIndex` to select the Alt Launch for a browser. It's an index number.

Sample Json file for more advanced editing:

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

Note that when adding rules to _UserSettings.json_ directly, follow the below pattern:

| Rule type | Format | Example |
| --- | --- | --- |
| String | `s$<YourRule>` or `<YourRule>` | `https://github.com/U-C-S` |
| Domain | `d$<YourRule>` | `d$github.com` |
| Regex  | `r$<YourRule>` | `r$.*open\\.spotify\\.com.*` |
| Glob   | `g$<YourRule>` | `g$*.google.com*` |
