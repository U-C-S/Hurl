#![windows_subsystem = "windows"]

use std::{
    env,
    io::{self, Read, Stdin},
    process::Command,
};

use byteorder::{NativeEndian, ReadBytesExt};
use clap::Parser;
use serde::Serialize;
use tokio::net::windows::named_pipe;

use crate::models::{NativeMessage, TemporaryDefaultBrowser};

mod models;

static PIPE_NAME: &str = r"\\.\pipe\HurlNamedPipe";

#[tokio::main]
async fn main() {
    let native_msg_url = try_get_native_message(io::stdin());
    let args = env::args().collect::<Vec<String>>();
    let cli_args = parse_cli_args(&args);

    let app_config_dir = {
        let roaming_dir = dirs::data_dir().unwrap();
        roaming_dir.join("Hurl")
    };

    let temp_default_path = app_config_dir.join("TempDefault.json");
    if temp_default_path.exists() {
        let file_content = std::fs::read_to_string(&temp_default_path).unwrap();
        let json_val: Result<TemporaryDefaultBrowser, _> = serde_json::from_str(&file_content);
        if let Ok(val) = json_val {
            if val.valid_till > chrono::Utc::now() {
                Command::new(&val.target_browser.exe_path);
                return;
            } else {
                let _ = std::fs::remove_file(&temp_default_path);
            }
        }
    }

    let pipe_conn = named_pipe::ClientOptions::new().open(PIPE_NAME);
    match pipe_conn {
        Ok(client) => {
            client.writable().await.unwrap();

            let args_str = match native_msg_url {
                Some(ref url) => serde_json::to_string(&vec![url]).unwrap(),
                None => serde_json::to_string(&cli_args).unwrap(),
            };
            let _ = client.try_write(args_str.as_bytes());
        }
        Err(_) => {
            let hurl_exe_path = {
                let current_exe_path = env::current_exe().unwrap();
                let current_dir = current_exe_path.parent().unwrap();
                current_dir.join("Hurl.exe")
            };
            let args = match native_msg_url {
                Some(url) => vec![url],
                None => args[1..].to_vec(),
            };
            let _ = Command::new(hurl_exe_path).args(args).spawn();
        }
    }
}

fn try_get_native_message(mut input: Stdin) -> Option<String> {
    let input = match input.read_u32::<NativeEndian>() {
        Ok(len) => {
            let mut buffer = vec![0; len as usize];
            let x = input.read_exact(&mut buffer);
            if x.is_err() {
                Err(io::Error::new(io::ErrorKind::Other, "Failed to read input"))
            } else {
                Ok(buffer)
            }
        }
        Err(_) => Err(io::Error::new(io::ErrorKind::Other, "Failed to read input")),
    };

    match input {
        Ok(val) => {
            let json_val: Result<NativeMessage, _> = serde_json::from_slice(&val);
            match json_val {
                Ok(val) => Some(val.url),
                Err(_) => None,
            }
        }
        Err(_) => None,
    }
}

#[derive(Parser, Debug, Serialize)]
#[command(about = None, long_about = None, )]
struct CliArgs {
    url: Option<String>,
}

fn parse_cli_args(args: &Vec<String>) -> CliArgs {
    let cli = CliArgs::parse_from(args);
    cli
}
