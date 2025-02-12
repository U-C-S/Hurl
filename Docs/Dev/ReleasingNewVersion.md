# New version release spec

## Update app version in the source code

The version information of the Application is currentlty hardcoded in
the source code. Following are the files that has version information
hardcoded

```
Source/Hurl.BrowserSelector/AssemblyInfo.cs
Source/Hurl.Library/Constants.cs
Source/Hurl.Settings/Hurl.Settings.csproj
Source/Launcher/Cargo.toml
Utils/installer.iss
```

This can potentially be a find-and-replace script in the future.

### Version format

Hurl mostly follows Semantic versioning: `x.y.z` in general.

But you might also notice a `x.y.z.r` pattern in couple of places.
The `r` part of the version keeps the count of number of releases
since the inception of Hurl. For `0.9.1.24` means that a semantic
version of `0.9.1` and 24th build that's publicly released. But ideally
this `r` part of the version should not matter and mostly just keeping
up something I randomly started.

## Triggering the Github CI build

GitHub Actions are used to create release build of the Hurl. The build
script is located in the the [./build.ps1](/build.ps1).

A build job is triggered from pushing a tag to remote, which uses the
build script to build all the necessary binaries, create a installer
and create a draft github release with resulting installer artifact.

To create a tag and push it to remote

```sh
git tag 'v0.9.1'
git push --tags
```

Should trigger the CI action and create a draft release in the github
releases.
