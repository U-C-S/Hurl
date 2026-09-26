# Using rules to speed things up

## What are rules?

Rules match incoming URLs and automatically open the associated browser or launch profile, skipping you a
selection step. Enable **Rule Matching** in **Hurl Settings > Rulesets**, or set
`AppSettings.RuleMatching` to `true`. It is disabled by default, adding rules alone does not enable it.

## Current Behavior on rule trigger

Hurl checks rules when a URL is passed to the app. The first matching ruleset wins. If no rule matches,
the selector opens. Holding the [Quick View shortcut](../README.md#quick-view-settings) takes precedence
over rules.

## Types of rules

- `String`: Matches the entire URL exactly and is case-sensitive. Differences in the scheme, path casing, query string, or trailing slash prevent a match.
- `Domain`: Simply the domain part of a URL,
  - example: For the URL `https://github.com/u-c-s/hurl`, `github.com` is the domain.
  - Probably the most useful rule type. By default it matches the host exactly, so `github.com` will not match `docs.github.com`.
  - To also match subdomains, prefix the domain with `*.` — a rule of `*.github.com` matches `github.com` itself
    as well as any subdomain such as `docs.github.com`.
- `Regex`: Uses .NET regular expressions against the full URL.
  - Matching is case-sensitive by default. use `(?i)` for case-insensitive matching.
  - Use `^` and `$` to anchor a pattern if it must match the whole URL.

> [!NOTE]
> String and regex rules are matched against the URL including its scheme, such as `https://`.
> Domain rules contain only the host, without a scheme, path, or port, and compare it case-insensitively.
> Domain matching requires an absolute URL such as `https://github.com/u-c-s/hurl`.

## What about rulesets?

Each ruleset is a collection of rules associated with one browser and an optional alternate launch. Any one rule
in the ruleset is enough to match. Multiple rulesets can target the same browser. When more than one ruleset
matches a URL, the one higher in the list wins.

- Rulesets can target hidden browsers too.
- a matching ruleset with a missing browser currently stops processing without opening the selector
- A missing alternate launch can fail to launch.
- Rulesets cannot target the built-in Quick View window currently.

## From UI

Open **Settings** from the selector menu, then select **Rulesets**.

1. Enable **Rule Matching**.
2. Select **Create** under **Create new Ruleset**. Enter a title, choose the target browser and optional **Alternate Launch**.
3. Add rules by selecting a type and entering the rule text. Select **Create** to store the ruleset.
4. Select **Test** under **Test Rules** and enter a URL. **Test against Existing Rules** checks the configured rulesets.
   **Test against Custom Rule** checks the type and rule entered in the dialog. Tests report matches without
   opening a browser and work even when automatic rule matching is disabled.

Use a ruleset's menu to **Edit**, **Move Up**, **Move Down**, or **Delete** it. The UI can reorder rulesets,
not reorder individual rules within them.

## From UserSettings.json

You can add new rules into the `Rulesets` property, which itself is a list of ruleset objects. Each ruleset object should contain these properties:

- `Id`: UUID for identifying this ruleset.
- `Rules`: An array of rule strings, even when there is only one rule. Any matching rule selects this ruleset's browser.
- `BrowserId`: The value should match the `Id` of one of the configured `Browsers`. It will be opened once a rule from the `Rules` property matches.
- `RulesetName`: Optional display name for the ruleset.
- `AlternateLaunchId`: The `Id` of an entry in the selected browser's `AlternateLaunches` array. Omit it or set it to `null` for the browser's default launch.

Sample Json file for more advanced editing:

```json
{
  "AppSettings": {
    "RuleMatching": true
  },
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

| Rule type | Format                         | Example                                                      |
| --------- | ------------------------------ | ------------------------------------------------------------ |
| String    | `s$<YourRule>` or `<YourRule>` | `https://github.com/U-C-S`                                   |
| Domain    | `d$<YourRule>`                 | `d$github.com` (exact) or `d$*.github.com` (with subdomains) |
| Regex     | `r$<YourRule>`                 | `r$.*open\.spotify\.com.*`                                   |

- The table shows rule strings before JSON escaping. In JSON, escape each backslash as `\\`, as in the sample above.
- For a string rule containing a literal `$` in hand-edited JSON, include `s$` explicitly so the first `$` is interpreted as the type separator.
- When using the UI, choose the rule type and enter the pattern without a `d$`, `r$`, or `s$` prefix.
