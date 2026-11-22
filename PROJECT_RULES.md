# Karachi 4K Bedford Bus Simulator — Project Rules

These rules govern the complete project and all future implementation work.

## Product goal
Build a polished Android 3D simulator based on the Karachi colourful, decorated local Bedford-style 4K bus reference confirmed by the user. The target is the specific Karachi local-bus look, not a modern coach and not merely 4K-resolution graphics.

## User communication rules
- Do not send routine progress reports, step-by-step updates, internal work logs, or line-by-line implementation explanations.
- Do not ask the user to approve every small step.
- Do not perform or report a separate GitHub test after every step.
- Work through the complete scope as one integrated development effort.
- Report only when a meaningful blocker requires user input, or when the final APK and delivery summary are ready.
- Do not claim a feature, test, build, or delivery is complete unless it has actually been completed and verified.

## Repository rules
- Use this repository as the project source of truth: https://github.com/adeeldo-ctrl/4k-bus-model
- Keep project files, source code, assets, configuration, and documentation organized in this repository.
- Preserve existing user work; inspect before replacing files.
- Do not create noisy commits for every tiny edit. Group related work into coherent milestones.

## Bus and simulator requirements
- Recreate the confirmed Karachi 4K Bedford-style local bus appearance, including its body proportions, front, windows, bumpers, wheels, paintwork, and colourful Pakistani truck-art decoration.
- Create a detailed exterior and passenger/driver interior.
- Support 360-degree exterior and interior inspection with camera controls.
- Include driver-seat view, steering wheel, dashboard, mirrors, seats, and passenger cabin.
- Implement functional doors, headlights, indicators, brake lights, and wheels.
- Implement a playable driving mode with steering, accelerator, brake, reverse, and suitable mobile touch controls.
- Include camera switching between exterior, interior, and driver views.
- Optimize rendering, controls, memory, and performance for Android devices.
- Use original or properly licensed assets. Do not assume that a reference photograph itself grants rights to reuse it as a game asset.

## Technology and implementation
- Prefer Unity 3D with C# for the Android simulator unless repository inspection reveals a strong technical reason to use another suitable engine.
- Keep controls touch-friendly and interface readable on phone screens.
- Separate vehicle model, materials, animation, input, camera, audio, and gameplay systems into maintainable components.
- Provide clear Android build configuration and reproducible build instructions.

## Integrated quality requirements
- Validate the complete application together after implementation, not through user-facing step-by-step test reports.
- Check startup, scene loading, bus appearance, camera modes, touch controls, vehicle movement, doors, lights, and Android build output.
- Fix critical and major defects found during integrated verification before delivery.
- Do not describe a test as passed unless it was actually run.

## Final delivery
- The final target is a working Android APK, accompanied by the complete project source and concise installation/use instructions.
- If an APK cannot be built in the available environment, state the exact blocker plainly and provide the complete build-ready project; never label a source-only result as an APK.
