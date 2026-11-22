# Karachi 4K Bedford Bus Simulator

Unity 2022.3 LTS Android project for a drivable Karachi-style decorated Bedford local bus.

## Open the project
1. Install Unity Hub and Unity 2022.3 LTS with Android Build Support, Android SDK & NDK Tools, and OpenJDK.
2. Clone this repository and open its root folder in Unity Hub.
3. Allow the editor to finish importing scripts. The editor setup utility creates the starter scene at `Assets/Scenes/Karachi4KBus.unity` and adds it to Build Settings.
4. Press Play to run the generated prototype.

## Controls
- **W / Up Arrow:** accelerate
- **S / Down Arrow:** brake
- **A / D:** steer
- **R:** select reverse
- **Space:** handbrake
- **E:** passenger door
- **L:** headlights
- **C:** cycle exterior, cabin and driver cameras
- **Mouse drag / touch drag away from HUD:** rotate exterior or cabin camera

On Android, the touch HUD provides steering, accelerator, brake, reverse, passenger door, lights, indicators, hazard lights and camera switching. A live speed and gear readout is displayed.

## Android APK
1. In Unity, open **File → Build Settings** and select **Android**.
2. Select **Switch Platform** if Android is not already active.
3. Confirm package identifier `com.adeeldo.karachi4kbus` and Landscape orientation in Player Settings.
4. Connect an Android device or select **Build** to create an APK.

## Current development status
This repository is an early playable prototype scaffold. The bus body, cabin and Karachi street are currently generated from simple procedural geometry. They are not yet the final detailed Bedford model, authentic truck-art texture set, fully modelled city, or polished release-quality simulator. The project has not yet been built into or verified as an Android APK. Do not treat the source project as a completed APK.
