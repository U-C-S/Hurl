# Releasing a new version

## Update version information

Release versions are set in these files:

| File                                                                       | Values                                                             |
| -------------------------------------------------------------------------- | ------------------------------------------------------------------ |
| [Source/Meta.Shared.props](../../Source/Meta.Shared.props)                 | `Version` and `FileVersion` for the app                            |
| [Source/Hurl.Library/Constants.cs](../../Source/Hurl.Library/Constants.cs) | `VERSION`, the version displayed by Hurl                           |
| [Utils/installer.iss](../../Utils/installer.iss)                           | `MyAppVersion`, used for the installer and its version information |

## Version format

In the format of `x.y.z.n` (windows) or `x.y.z-<type>-n` (human/git tags)
- The `x.y.z` mostly tries to follow semantic versioning (or maybe [zeroVer](https://0ver.org/) :D)
- `n` is release number.
  - Increments are done when a same version is released multiple times
  - like in the case of alpha, snapshot releases which also control the `<type>`, while it's semeantic version
  is the same.
  - Fox example, 
    - in case of `v0.10.0-alpha-2` it's windows format would be `0.10.0.002`
    - for the stable release, `v0.10.0`, windows format would be `0.10.0.100` 

## And the Rest

Depending on the type of release, you can either run the `release` or `release-snapshot` github actions. which
automatically creates a release (or a draft release). 

Not expanding on this much.