use std::fs::OpenOptions;
use std::io::Write;

#[allow(dead_code)]
pub fn write_log_to_file(message: &str) {
    let log_file = OpenOptions::new()
        .create(true)
        .append(true)
        .open(r"log.txt");
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
