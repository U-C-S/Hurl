use serde::{Deserialize, Serialize};

#[derive(Debug, Serialize, Deserialize)]
#[serde(rename_all = "PascalCase")]
pub struct RuleSet {
    pub rules: Vec<String>,
    pub browser_name: String,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(rename_all = "PascalCase")]
struct Rule {}

#[derive(Debug, Serialize, Deserialize)]
#[serde(rename_all = "PascalCase")]
pub struct Browser {
    pub name: String,
    pub exe_path: String,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(rename_all = "PascalCase")]
pub struct Settings {
    pub rulesets: Vec<RuleSet>,
    pub browsers: Vec<Browser>,
}

#[derive(Deserialize, Serialize)]
pub struct NativeMessage {
    pub url: String,
}

// public class TemporaryDefaultBrowser
// {
//     public Browser TargetBrowser { get; set; }

//     public DateTime SelectedAt { get; set; }

//     public DateTime ValidTill { get; set; }
// }

#[derive(Deserialize)]
#[serde(rename_all = "PascalCase")]
pub struct TemporaryDefaultBrowser {
    pub target_browser: Browser,
    pub selected_at: String,
    pub valid_till: chrono::DateTime<chrono::Utc>,
}
