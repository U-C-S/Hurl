#![windows_subsystem = "windows"]

use std::{
    env,
    io::{self, Read},
    process::Command,
};

use byteorder::{NativeEndian, ReadBytesExt};
use models::NativeMessage;
use tokio::net::windows::named_pipe;

mod models;

static PIPE_NAME: &str = r"\\.\pipe\HurlNamedPipe";

#[tokio::main]
async fn main() {
    // Try get the url from browser through native messaging
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

    let pipe_conn = named_pipe::ClientOptions::new().open(PIPE_NAME);
    match pipe_conn {
        Ok(client) => {
            client.writable().await.unwrap();
            let _ = client.try_write(args_str.as_bytes());
        }
        Err(_) => {
            let _ = Command::new(hurl_exe_path).args(args).spawn();
        }
    }
}

pub fn read_input<R: Read>(mut input: R) -> io::Result<Vec<u8>> {
    match input.read_u32::<NativeEndian>() {
        Ok(len) => {
            let mut buffer = vec![0; len as usize];
            input.read_exact(&mut buffer)?;
            Ok(buffer)
        }
        Err(_) => Err(io::Error::new(io::ErrorKind::Other, "Failed to read input")),
    }
}

// static USER_SETTINGS: &str = "C:\\Users\\uchan\\AppData\\Roaming\\Hurl\\UserSettings.json";

// let file = std::fs::read_to_string(USER_SETTINGS).unwrap();
// let user_settings: models::Settings = serde_json::from_str(&file).unwrap();
// let user_settings_json = serde_json::to_string(&user_settings).unwrap();
