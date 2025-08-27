use tokio::net::windows::named_pipe::NamedPipeClient;

pub static PIPE_NAME: &str = r"\\.\pipe\HurlNamedPipe";

/// Initialize IPC mechanism
/// This function initializes the inter-process communication (IPC) mechanism.
/// by server is woken up when client connects. so sends a ping to server and
/// waits for a pong response. If the response is received, the function
/// confirms that the IPC mechanism is successfully initialized. else,
/// it tries again once after a short delay.
pub async fn init(client: &NamedPipeClient) {
    let max_retries = 2;
    let mut attempts = 0;
    while attempts < max_retries {
        if client.writable().await.is_ok() {
            let ping_message = b"ping";
            let _ = client.try_write(ping_message);
        }

        if client.readable().await.is_ok() {
            let mut buffer = [0u8; 16];
            if let Ok(n) = client.try_read(&mut buffer) {
                if n > 0 && &buffer[..n] == b"pong" {
                    break;
                }
            }
        }

        attempts += 1;
    }
}
