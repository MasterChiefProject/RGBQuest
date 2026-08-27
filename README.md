# RGBQuest

[![Repository checks](https://github.com/MasterChiefProject/RGBQuest/actions/workflows/repository.yml/badge.svg)](https://github.com/MasterChiefProject/RGBQuest/actions/workflows/repository.yml)
[![Unity](https://img.shields.io/badge/Unity-6000.0.47f1-black?logo=unity)](https://unity.com/)
[![WebGL](https://img.shields.io/badge/WebGL-browser%20build-5b7fff)](https://masterchiefproject.github.io/RGBQuest/)

**RGBQuest** is a three-stage first-person puzzle adventure built with Unity 6. Solve color-driven physics puzzles, place RGB cubes on matching pressure objectives, progress through animated doors and portals, then survive the combat and ghost encounters introduced in later stages.

**Playable WebGL build:** https://masterchiefproject.github.io/RGBQuest/

## Trailer and complete stage guide

[![RGBQuest Trailer and Stage Guide](https://img.youtube.com/vi/b8so2yYArQA/hqdefault.jpg)](https://www.youtube.com/watch?v=b8so2yYArQA)

**Watch:** https://www.youtube.com/watch?v=b8so2yYArQA

The video demonstrates the intended solution path through all three stages and doubles as a gameplay trailer and full walkthrough.

## Gameplay

RGBQuest begins as a physics-focused color puzzle game and gradually introduces more systems.

The core progression logic tracks four color states:

- Yellow
- Red
- Blue
- Purple

Colored physics objects activate matching pressure plates. Each plate drives its existing animation and lamp feedback while updating shared puzzle state. Once all required states are active, progression systems such as portal doors and level transitions become available.

Later stages add health and ammunition pickups, weapon handling, destructible targets, environmental hazards, and NavMesh-driven ghosts with distinct chase/scatter behavior.

### Game flow

```text
Main Menu
    ↓
Level 1
    ↓
Level 2
    ↓
Level 3
    ↓
Win Menu

Player death
    ↓
Death Menu
    ├── Retry → Level 1
    └── No    → Main Menu
```

## Controls

| Action | Keyboard / Mouse |
| --- | --- |
| Move | `WASD` or Arrow keys |
| Look | Mouse |
| Jump | `Space` |
| Sprint | `Left Shift` |
| Crouch | `C` |
| Pick up / drop a physics object | `E` |
| Throw a held object | `T` |
| Rotate a held object | Hold `R` + Mouse |
| Fire | Left Mouse / `Fire1` |

The WebGL version is intended primarily for desktop browsers with a keyboard and mouse.

## Production scenes

The release build contains exactly these six scenes:

```text
Assets/Scenes/MainMenu.unity
Assets/Scenes/Level1.unity
Assets/Scenes/Level2.unity
Assets/Scenes/Level3.unity
Assets/Scenes/DeathMenu.unity
Assets/Scenes/WinMenu.unity
```

`Assets/Scenes/Main.unity` remains a disabled legacy entry in Unity Build Settings. The production build helper deliberately ignores it without modifying the project's existing Build Settings file.

## Technology

- Unity `6000.0.47f1`
- C#
- Universal Render Pipeline `17.0.4`
- Unity Input System `1.14.0`
- Unity AI Navigation `2.0.8`
- Cinemachine `3.1.4`
- Unity physics / Rigidbody interactions
- NavMesh-based enemy navigation
- uGUI and TextMesh Pro
- WebGL / WebAssembly
- GitHub Pages

## Selected gameplay systems

- `Globals` stores shared run state, health, ammunition, weapon state, respawn data, and the four pressure-plate states.
- `PressurePlate` preserves the scene-authored trigger, Animator, `CubeGlow`, and color-state contract.
- `HoldToPickup` handles physics-object pickup, drop, throw, rotation, collision handling, and obstruction-aware placement.
- `Teleport` advances to the configured next scene after the color objectives are complete.
- `PortalDoor` retains its Animator and audio-driven open/close behavior.
- `GunController` manages weapon visibility, firing, projectile velocity, ammunition UI, muzzle effects, and sound.
- `GhostController` uses `NavMeshAgent` with scatter/chase modes and four distinct ghost roles.
- `HealthBox` and `AmmoBox` retain their trigger-based UI/audio pickup behavior.
- `DeathMenu` and `WinMenu` implement the failure and completion flow.

## Project structure

```text
RGBQuest/
├── .github/
│   └── workflows/
│       └── repository.yml
├── Assets/
│   ├── Editor/
│   │   └── RGBQuestWebGLBuild.cs
│   ├── Scenes/
│   ├── Scripts/
│   ├── Settings/
│   └── WebGLTemplates/
│       └── RGBQuest/
├── Packages/
├── ProjectSettings/
├── tests/
│   └── repository.test.mjs
├── ASSET-NOTICE.md
└── README.md
```

`docs/` is generated only after a successful production WebGL build and is then committed for GitHub Pages.

## Production-safety policy

The production additions are intentionally non-invasive. They do **not** replace gameplay scripts, scenes, Unity Build Settings, quality profiles, Graphics Settings, or URP assets.

RGBQuest already has separate project-authored PC and Mobile rendering/quality configurations. Those remain under Unity's control.

The production build helper only:

- validates the six release scene files
- requires WebGL to already be the active Build Profile
- applies project metadata (`MasterChiefProject`, `RGBQuest`, `1.0.0`)
- selects the custom RGBQuest WebGL template
- enables Gzip compression, decompression fallback, and browser data caching
- builds the explicit six-scene list into a staging directory
- replaces `docs/` only after Unity reports a successful build
- writes `docs/.nojekyll`

It deliberately does not switch render pipelines, change quality settings, change texture mip policy, alter cameras, or inject runtime rendering overrides.

## Run in the Unity Editor

Use Unity:

```text
6000.0.47f1
```

Open:

```text
Assets/Scenes/MainMenu.unity
```

Press **Play** and validate the existing scene flow before doing any WebGL work:

```text
START → Level 1
ABOUT → About panel
BACK → Main Menu panel
```

Continue through the three levels and verify pressure plates, lamps, pickups, doors, portals, weapon behavior, ghosts, death flow, and win flow.

The trailer provides the intended solution path:

```text
https://www.youtube.com/watch?v=b8so2yYArQA
```

## Build for GitHub Pages

### 1. Validate the normal Editor version

Before switching build profiles, confirm that the game renders and plays normally from `MainMenu.unity`.

### 2. Switch to WebGL manually

Use:

```text
File → Build Profiles → WebGL
```

Wait for Unity to finish the platform-specific import/recompile.

Then open `MainMenu.unity` and press **Play again before building**.

If materials become pink or scene behavior changes merely from activating WebGL, stop there and switch back. That indicates a project/platform rendering issue that should be investigated before producing the release build.

### 3. Validate the production setup

Run:

```text
RGBQuest → Validate Production Setup
```

### 4. Build

Run:

```text
RGBQuest → Build WebGL for GitHub Pages
```

The command builds first into:

```text
Builds/RGBQuestWebGLStaging/
```

Only a successful Unity build replaces:

```text
docs/
```

The staging directory is already covered by the project's Unity `.gitignore` pattern for `Builds/`.

## Test the browser build locally

Serve the generated `docs/` directory over HTTP:

```powershell
py -m http.server 8000 --directory docs
```

Open:

```text
http://localhost:8000/
```

Do not test Unity WebGL through a `file://` URL.

Verify the complete game, not only the Main Menu:

- START / ABOUT / BACK
- mouse look and keyboard movement
- physics pickup, drop, throw, and rotate
- all pressure plates and lamp feedback
- animated portal doors
- level transitions
- health and ammo pickups
- weapon visibility and firing
- ghosts
- Death Menu
- Win Menu
- fullscreen
- Trailer / Stage Guide link
- dark/light web-shell theme

## GitHub Pages

After committing the generated `docs/` directory, configure:

```text
Settings → Pages
Source: Deploy from a branch
Branch: main
Folder: /docs
```

Deployment URL:

```text
https://masterchiefproject.github.io/RGBQuest/
```

## Repository checks

GitHub Actions runs lightweight static validation without requiring a Unity license.

Run the same checks locally with Node.js 22+:

```powershell
node --check Assets\WebGLTemplates\RGBQuest\TemplateData\shell.js
node --test tests\repository.test.mjs
```

The checks intentionally protect the scene-bound contracts that are easy to break accidentally, including the Main Menu UnityEvent method names, pressure-plate serialized fields, pickup UI/audio fields, and animated portal-door behavior.

Unity compilation and Play Mode remain the authoritative tests for serialized scene references and gameplay.

## Repository cleanup

`UpgradeLog.htm` is an old Visual Studio migration report and is not part of the game. Remove it before the production commit:

```powershell
git rm UpgradeLog.htm
```

Do not blindly remove imported Unity asset directories. The project contains substantial third-party content and scene dependencies.

## Assets and licensing

RGBQuest contains imported models, textures, materials, shaders, audio, fonts, animations, editor tooling, and other third-party Unity content.

See [`ASSET-NOTICE.md`](ASSET-NOTICE.md) before redistributing or relicensing repository content.

No repository-wide license is asserted over third-party assets by this project documentation.

## Repository

Source: https://github.com/MasterChiefProject/RGBQuest
