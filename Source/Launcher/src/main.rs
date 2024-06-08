#![windows_subsystem = "windows"]

mod models;

use windows::{core::*, Win32::System::Pipes::CallNamedPipeA};

const USER_SETTINGS: &str = "C:\\Users\\uchan\\AppData\\Roaming\\Hurl\\UserSettings.json";
const PIPE_NAME: &str = "\\\\.\\pipe\\HurlNamedPipe";

fn main() {
    let file = std::fs::read_to_string(USER_SETTINGS).unwrap();
    let user_settings: models::Settings = serde_json::from_str(&file).unwrap();
    let user_settings_json = serde_json::to_string(&user_settings).unwrap();
    std::fs::write("./lol.txt", &user_settings_json);
    // anonymous pipes
    // let (tx, rx) = std::sync::mpsc::channel();

    unsafe {
        let x: PCSTR = PCSTR(PIPE_NAME.as_ptr());
        let lpinbuffer = user_settings_json.as_ptr() as *const std::ffi::c_void;
        let ninbuffersize = user_settings_json.len() as u32;
        let lpbytesread = std::ptr::null_mut();
        let x = CallNamedPipeA(
            x,
            Some(lpinbuffer),
            ninbuffersize,
            None,
            0,
            lpbytesread,
            100000,
        );
    }

    println!("{:?}", user_settings);
}
