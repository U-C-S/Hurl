#![windows_subsystem = "windows"]

use std::{env, process::Command};

use tokio::net::windows::named_pipe::ClientOptions;

mod models;

static USER_SETTINGS: &str = "C:\\Users\\uchan\\AppData\\Roaming\\Hurl\\UserSettings.json";
static PIPE_NAME: &str = r"\\.\pipe\HurlNamedPipe";

#[tokio::main]
async fn main() {
    let hurl_exe_path = {
        let current_exe_path = env::current_exe().unwrap();
        let current_dir = current_exe_path.parent().unwrap();
        current_dir.join("Hurl.exe")
    };

    let args: Vec<String> = env::args().collect();
    let args_str = serde_json::to_string(&args).unwrap();

    // let file = std::fs::read_to_string(USER_SETTINGS).unwrap();
    // let user_settings: models::Settings = serde_json::from_str(&file).unwrap();
    // let user_settings_json = serde_json::to_string(&user_settings).unwrap();

    let pipe_conn = ClientOptions::new().open(PIPE_NAME);
    match pipe_conn {
        Ok(client) => {
            println!("Connected to the server");
            client.writable().await.unwrap();
            let _ = client.try_write(args_str.as_bytes());
        }
        Err(_) => {
            println!("Failed to connect to the server");
            let _ = Command::new(hurl_exe_path).args(args).spawn();
        }
    }
}
