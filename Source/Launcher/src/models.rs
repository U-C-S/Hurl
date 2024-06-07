pub struct RuleSet {
    pub id: i32,
    pub rules: Vec<Rule>,
    pub name: String,
}

struct Rule {}

struct Browser {
    pub id: i32,
    pub name: String,
    pub version: String,
}

struct Settings {
    pub rule_sets: Vec<RuleSet>,
    pub browsers: Vec<Browser>,
}
