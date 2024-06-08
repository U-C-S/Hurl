// #![windows_subsystem = "windows"]

use tokio::net::windows::named_pipe::{self, ClientOptions};

mod models;

// use windows::{core::*, Win32::System::Pipes::CallNamedPipeA};

const USER_SETTINGS: &str = "C:\\Users\\uchan\\AppData\\Roaming\\Hurl\\UserSettings.json";
const PIPE_NAME: &str = r"\\.\pipe\HurlNamedPipe";

#[tokio::main]
async fn main() {
    let file = std::fs::read_to_string(USER_SETTINGS).unwrap();
    let user_settings: models::Settings = serde_json::from_str(&file).unwrap();
    let user_settings_json = serde_json::to_string(&user_settings).unwrap();

    print!("Waiting for server to be ready...");
    let client = ClientOptions::new().open(PIPE_NAME).unwrap();

    client.writable().await.unwrap();
    client.try_write(user_settings_json.as_bytes());

    println!("{:?}", user_settings);
}
