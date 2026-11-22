# Karachi 4K Bedford Bus Simulator

Unity 3D Android project scaffold for a drivable, decorated Karachi-style Bedford local bus.

## Project target
- Android touch controls
- Exterior orbit and interior/driver camera views
- Procedurally generated bus body and cabin
- Animated passenger doors, steering wheel, wheels and lights
- Drive, brake and reverse input

## Open
1. Install Unity Hub and a Unity 2022.3 LTS editor with Android Build Support, Android SDK & NDK Tools, and OpenJDK.
2. Clone this repository.
3. Open the repository root in Unity Hub.
4. Open the generated runtime scene by creating a new scene; add an empty GameObject and attach `Karachi4KBusBootstrap`.
5. Press Play to generate the bus and test the desktop keyboard controls. On Android, the runtime touch overlay is created automatically.

## Controls
- W / Up: accelerate
- S / Down: brake
- R: reverse
- A / D: steer
- Space: handbrake
- E: toggle passenger door
- L: toggle lights
- C: switch camera
- Drag: orbit exterior camera

## Android build
In Unity: File → Build Settings → Android → Switch Platform → Build.
Set package identifier to `com.adeeldo.karachi4kbus`.

## Important
The current bus is a procedural, stylized starting model intended to establish the complete gameplay systems. Exact body detailing, authentic artwork, sound, and device performance tuning require a licensed reference/asset pass and testing on a real Android device. Do not claim a finished APK until a Unity Android build has actually been produced and tested.
