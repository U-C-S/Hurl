#![windows_subsystem = "windows"]

use std::{
    env,
    io::{self, Read},
    process::Command,
};

use byteorder::{NativeEndian, ReadBytesExt};
use models::NativeMessage;
use tokio::net::windows::named_pipe::ClientOptions;

mod models;

static PIPE_NAME: &str = r"\\.\pipe\HurlNamedPipe";

#[tokio::main]
async fn main() {
    let native_msg_url = match read_input(io::stdin()) {
        Ok(val) => {
            let json_val: Result<NativeMessage, _> = serde_json::from_slice(&val);
            match json_val {
                Ok(val) => Some(val.url),
                Err(_) => None,
            }
        }
        Err(_) => None,
    };

    let hurl_exe_path = {
        let current_exe_path = env::current_exe().unwrap();
        let current_dir = current_exe_path.parent().unwrap();
        current_dir.join("Hurl.exe")
    };

    let args: Vec<String> = env::args().collect();
    let args_str = match native_msg_url {
        Some(url) => serde_json::to_string(&vec![url]).unwrap(),
        None => serde_json::to_string(&args).unwrap(),
    };

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

pub fn read_input<R: Read>(mut input: R) -> io::Result<Vec<u8>> {
    let length = input.read_u32::<NativeEndian>().unwrap();
    let mut buffer = vec![0; length as usize];
    input.read_exact(&mut buffer)?;
    Ok(buffer)
}

// static USER_SETTINGS: &str = "C:\\Users\\uchan\\AppData\\Roaming\\Hurl\\UserSettings.json";

// let file = std::fs::read_to_string(USER_SETTINGS).unwrap();
// let user_settings: models::Settings = serde_json::from_str(&file).unwrap();
// let user_settings_json = serde_json::to_string(&user_settings).unwrap();
