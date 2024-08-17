fn main() {
    let mut res = winresource::WindowsResource::new();
    res.set_icon("../Hurl.BrowserSelector/Assets/internet.ico");
    res.compile().unwrap();
}
