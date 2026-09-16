# Using rules to speeds things up

## What are rules?

They are text strings, which when configured can automatically open a browser for you, instead of prompting to select a browser. Thus saving a step. Each time you open a link via Hurl, it searches through all the configured rules to see if any matches with the URL and if it matches, it automatically opens the associated browser.

### Types of rules

- `String`: This does simple text matching. Triggers if your URL matches exactly with this plain rule.
- `Domain`: Simply the domain part of a URL, example: For the URL `https://github.com/u-c-s/hurl`, `github.com` is the domain. Probably the most useful rule type. By default it matches the host exactly, so `github.com` will not match `docs.github.com`. To also match subdomains, prefix the domain with `*.` — a rule of `*.github.com` matches `github.com` itself as well as any subdomain such as `docs.github.com`.
- `Regex`: Uses Regular Expressions to do text matching. Use <https://regex101.com> with (.NET/C# flavor) to test your rules.

> [!NOTE]
> URLs typically have the `https://` prefix which also needs to be taken care by the rules you have written.

## What about rulesets?

Each ruleset is a collection of rules. You can have as many rules as you want inside a ruleset. Each ruleset is associated to a browser and you can have multiple rulesets for the same browser. Additionally, rulesets are prioritized as per the ordering. If the same rule exists in two different rulesets, the ruleset that's higher in the list is triggered first.

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

You can add new rules into the `Rulesets` property, which itself is a list of ruleset objects. Each ruleset object should contain these properties:

- `Rules`: It can be a single rule or multiple rules. Rules are prioritized: the first rule that matches will trigger the opening of a browser.
- `BrowserId`: The value should match the `Id` of one of the configured `Browsers`. It will be opened once a rule from the `Rules` property matches.
- `RulesetName`: To name the ruleset. Only for visual purposes.
- `AlternateLaunchId` to select a browser's alternate launch profile. Omit it or set it to `null` for the browser's default launch.

Sample Json file for more advanced editing:

```json
{
  "Browsers": [
    {
      "Id": "2b36a1fe-97f7-4509-ae6e-5c2c61602af4",
      "Name": "Firefox Nightly",
      "ExePath": "C:\\Program Files\\Firefox Nightly\\firefox.exe"
    },
    {
      "Id": "e48b823f-c4b0-4218-a7e2-a8c80231228a",
      "Name": "Google Chrome Dev",
      "ExePath": "C:\\Program Files\\Google\\Chrome Dev\\Application\\chrome.exe"
    }
  ],
  "Rulesets": [
    {
      "Id": "6f1e9e11-5a02-4e4f-a4f2-b86dbf85d8af",
      "RulesetName": "Googly",
      "Rules": ["d$google.com"],
      "BrowserId": "e48b823f-c4b0-4218-a7e2-a8c80231228a"
    },
    {
      "Id": "41f1ed96-77f5-43cc-a920-e1052c8f620d",
      "Rules": ["https://github.com/u-c-s", "r$.*open\\.spotify\\.com.*"],
      "BrowserId": "2b36a1fe-97f7-4509-ae6e-5c2c61602af4"
    }
  ]
}
```

Note that when adding rules to _UserSettings.json_ directly, follow the below pattern:

| Rule type | Format | Example |
| --- | --- | --- |
| String | `s$<YourRule>` or `<YourRule>` | `https://github.com/U-C-S` |
| Domain | `d$<YourRule>` | `d$github.com` (exact) or `d$*.github.com` (with subdomains) |
| Regex  | `r$<YourRule>` | `r$.*open\\.spotify\\.com.*` |
