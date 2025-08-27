#![windows_subsystem = "windows"]

use std::{
    env,
    io::{self, Read},
    process::Command,
};

use byteorder::{NativeEndian, ReadBytesExt};
use models::NativeMessage;
use tokio::net::windows::named_pipe;

mod ipc;
mod models;
mod utils;

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

    // TODO?: Cache the path of Hurl.exe in a file when first run
    let hurl_exe_path = {
        let current_exe_path = env::current_exe().unwrap();
        let current_dir = current_exe_path.parent().unwrap();
        current_dir.join("Hurl.exe")
    };

    let args = env::args().collect::<Vec<String>>();
    let trimed_args = &args[1..];

    let pipe_conn = named_pipe::ClientOptions::new().open(ipc::PIPE_NAME);

    match pipe_conn {
        Ok(client) => {
            ipc::init(&client).await;

            client.writable().await.unwrap();
            let args_str = match native_msg_url {
                Some(ref url) => serde_json::to_string(&vec![url]).unwrap(),
                None => serde_json::to_string(&trimed_args).unwrap(),
            };
            let _ = client.try_write(args_str.as_bytes());
        }
        Err(_e) => {
            // write_log_to_file(e.to_string().as_str());
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
