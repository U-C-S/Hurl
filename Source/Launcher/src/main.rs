#![windows_subsystem = "windows"]

use std::{
    env,
    fs::OpenOptions,
    io::{self, Read, prelude::*},
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
                Ok(val) => {
                    write_log_to_file(&format!("Received URL via Native Msg: {}", val.url));
                    Some(val.url)
                }
                Err(_) => None,
            }
        }
        Err(_) => None,
    };

    let hurl_exe_path = {
        let current_exe_path = env::current_exe().unwrap();
        let current_dir = current_exe_path.parent().unwrap();
        current_dir.join("Hurl.exe")

        // ; String::from("../../../Hurl.BrowserSelector/bin/Debug/net8.0-windows/Hurl.exe")
    };

    let args = env::args().collect::<Vec<String>>();
    write_log_to_file(&format!("Received args: {:?}", args));
    let trimed_args = &args[1..];

    let pipe_conn = named_pipe::ClientOptions::new().open(PIPE_NAME);
    match pipe_conn {
        Ok(client) => {
            client.writable().await.unwrap();

            let args_str = match native_msg_url {
                Some(ref url) => serde_json::to_string(&vec![url]).unwrap(),
                None => serde_json::to_string(&trimed_args).unwrap(),
            };
            let _ = client.try_write(args_str.as_bytes());
        }
        Err(e) => {
            // append the error log to file
            write_log_to_file(&format!("Failed to connect to pipe: {}", e));

            // If the pipe connection fails, we can still run Hurl.exe with the arguments
            let args = match native_msg_url {
                Some(url) => vec![url],
                None => trimed_args.to_vec(),
            };
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

fn write_log_to_file(message: &str) {
    let log_file = OpenOptions::new()
        .create(true)
        .append(true)
        .open(r"C:\Users\uchan\AppData\Roaming\Hurl\log.txt");
    match log_file {
        Ok(mut file) => {
            let now = chrono::Local::now();
            let formatted_time = now.format("%Y-%m-%d %H:%M:%S").to_string();
            if let Err(e) = writeln!(file, "{}: {}", formatted_time, message) {
                eprintln!("Failed to write error log: {}", e);
            }
        }
        Err(e) => {
            eprintln!("Failed to open log file: {}", e);
        }
    }
}
