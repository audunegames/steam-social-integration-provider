# Audune Steam Social Integration Provider

[![openupm](https://img.shields.io/npm/v/com.audune.social.steam?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.audune.social.steam/)

Provider for [Audune Social Integration](https://github.com/audunegames/social-integration) for connecting the Steamworks SDK to the social integration.

See the [wiki](https://github.com/audunegames/steam-social-integration-provider/wiki) of the repository to get started with the package.

## Features

* A social provider component connecting the Steamworks SDK through Steamworks .NET to [Audune Social Integration](https://github.com/audunegames/social-integration).
* Get the current user and their relationships for Steam.
* Set rich presence to Steam.
* Handle activation of game overlays for Steam.

## Installation

### Requirements

This package depends on the following packages:

* [Social Integration](https://openupm.com/packages/com.audune.social/), version **0.1.0** or higher.
* [Steamworks .NET](https://openupm.com/packages/com.rlabrecque.steamworks.net/), version **20.0.0** or higher.

If you're installing the required packages from the [OpenUPM registry](https://openupm.com/), make sure to add a scoped registry with the URL `https://package.openupm.com` and the required scopes before installing the packages.

### Installing from the OpenUPM registry

To install this package as a package from the OpenUPM registry in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Edit › Project Settings... › Package Manager**.
* Add the following Scoped Registry, or edit the existing OpenUPM entry to include the new Scope:

```
Name:     package.openupm.com
URL:      https://package.openupm.com
Scope(s): com.audune.social.steam
```

* Navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package by name...**
* Enter the following name in the corresponding field and click **Add**:

```
com.audune.social.steam
```

### Installing as a Git package

To install this package as a Git package in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package from git URL...**
* Enter the following URL in the URL field and click **Add**:

```
https://github.com/audunegames/steam-social-integration-provider.git
```

## Usage

To use this social provider, add the `Steam Social Provider` component to the game object where your `Social System` lives. Set the Steam AppID and you're all set!

## License

This package is licensed under the GNU LGPL 3.0 license. See `LICENSE.txt` for more information.
