# RGBQuest

[![Repository checks](https://github.com/MasterChiefProject/RGBQuest/actions/workflows/repository.yml/badge.svg)](https://github.com/MasterChiefProject/RGBQuest/actions/workflows/repository.yml)
[![Unity](https://img.shields.io/badge/Unity-6000.0.47f1-black?logo=unity)](https://unity.com/)
[![WebGL](https://img.shields.io/badge/WebGL-browser%20build-5b7fff)](https://masterchiefproject.github.io/RGBQuest/)

**RGBQuest** is a three-stage first-person puzzle adventure built with Unity 6. The game combines RGB color-state puzzles, physics-based object manipulation, animated progression gates, combat, pickups, and NavMesh-driven ghosts.

**Playable WebGL build:** https://masterchiefproject.github.io/RGBQuest/

## Trailer and complete stage guide

[![RGBQuest Trailer and Stage Guide](https://img.youtube.com/vi/b8so2yYArQA/hqdefault.jpg)](https://www.youtube.com/watch?v=b8so2yYArQA)

**Trailer / walkthrough:** https://www.youtube.com/watch?v=b8so2yYArQA

The video demonstrates the intended solution path through all three stages and provides a complete gameplay walkthrough.

## Gameplay

RGBQuest begins with physics-focused color puzzles and introduces additional combat and survival mechanics as the player progresses.

Four shared color states drive the core puzzle system:

- Yellow
- Red
- Blue
- Purple

Colored physics objects activate matching pressure plates. Each plate updates the shared puzzle state while driving its animation and lamp feedback. Completing the required combination unlocks progression through portal doors and scene transitions.

Later stages add ammunition and health pickups, weapon handling, destructible targets, environmental hazards, and ghosts with distinct chase/scatter behavior.

## Game flow

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
| Move | `WASD` / Arrow Keys |
| Look | Mouse |
| Jump | `Space` |
| Sprint | `Left Shift` |
| Crouch | `C` |
| Pick up / drop physics object | `E` |
| Throw held object | `T` |
| Rotate held object | Hold `R` + Mouse |
| Fire | Left Mouse / `Fire1` |

The WebGL build is designed primarily for desktop browsers with keyboard and mouse input.

## Production scenes

The release build contains six scenes:

```text
Assets/Scenes/MainMenu.unity
Assets/Scenes/Level1.unity
Assets/Scenes/Level2.unity
Assets/Scenes/Level3.unity
Assets/Scenes/DeathMenu.unity
Assets/Scenes/WinMenu.unity
```

`Assets/Scenes/Main.unity` remains a disabled legacy scene and is not part of the production build.

## Technology

- Unity `6000.0.47f1`
- C#
- Universal Render Pipeline `17.0.4`
- Unity Input System `1.14.0`
- Unity AI Navigation `2.0.8`
- Cinemachine `3.1.4`
- Rigidbody-based physics interactions
- NavMesh enemy navigation
- uGUI and TextMesh Pro
- WebGL / WebAssembly
- GitHub Pages

## Gameplay architecture

- `Globals` stores shared run state, health, ammunition, weapon state, respawn data, and pressure-plate state.
- `PressurePlate` connects scene triggers, Animator state, `CubeGlow` feedback, and shared color-state progression.
- `HoldToPickup` manages physics-object pickup, placement, drop, throw, rotation, and collision handling.
- `Teleport` advances to the configured scene when the puzzle state is complete.
- `PortalDoor` coordinates animated and audio-driven puzzle gates.
- `GunController` manages firing, projectile velocity, ammunition UI, muzzle effects, and weapon audio.
- `GhostController` uses `NavMeshAgent` with scatter/chase modes and distinct ghost roles.
- `HealthBox` and `AmmoBox` implement trigger-based pickups with UI and audio feedback.
- `DeathMenu` and `WinMenu` implement failure and completion flow.

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
├── docs/                         # Deployable GitHub Pages build
├── tests/
│   └── repository.test.mjs
├── ASSET-NOTICE.md
└── README.md
```

## WebGL build design

RGBQuest uses separate project-authored PC and Mobile rendering profiles. The WebGL build retains the project's browser-oriented rendering configuration rather than changing scene content or gameplay logic during packaging.

The production build helper performs deployment-focused work:

- validates the six release scenes
- requires WebGL as the active build target
- applies `MasterChiefProject / RGBQuest / 1.0.0` metadata
- selects the custom RGBQuest WebGL template
- enables Gzip compression
- enables decompression fallback
- enables browser data caching
- builds into `Builds/RGBQuestWebGLStaging/`
- publishes `docs/` only after a successful Unity build
- creates `docs/.nojekyll`

This keeps browser packaging separate from scene-authored gameplay behavior.

## Unity workflow

The project targets:

```text
Unity 6000.0.47f1
```

The primary editor entry scene is:

```text
Assets/Scenes/MainMenu.unity
```

The scene flow exposes the main menu, About panel, three gameplay stages, death flow, and victory flow.

The production WebGL command is:

```text
RGBQuest > Build WebGL for GitHub Pages
```

A separate validation command is available through:

```text
RGBQuest > Validate Production Setup
```

## Local WebGL validation

The generated `docs/` build is served over HTTP:

```powershell
py -m http.server 8000 --directory docs
```

Local URL:

```text
http://localhost:8000/
```

A full browser smoke test covers menu flow, movement, physics-object interaction, pressure plates, lamps, doors, scene transitions, pickups, weapon behavior, ghosts, death flow, win flow, fullscreen, and the custom browser shell.

## Automated checks

GitHub Actions provides lightweight regression coverage without requiring a Unity license on the hosted runner.

Local verification:

```powershell
node --check Assets\WebGLTemplates\RGBQuest\TemplateData\shell.js
node --test tests\repository.test.mjs
```

The repository checks protect important scene-bound contracts, including:

- Main Menu UnityEvent method names
- pressure-plate serialized fields
- pickup UI/audio fields
- portal-door animation/audio behavior
- production scene list
- Unity version and package versions
- WebGL build metadata and packaging

Unity compilation and Play Mode provide the final validation layer for serialized scene references and runtime behavior.

## Deployment

The committed `docs/` build is published through GitHub Pages.

**Live build:** https://masterchiefproject.github.io/RGBQuest/

## Assets and licensing

RGBQuest contains imported models, textures, materials, shaders, audio, fonts, animations, editor tooling, and other third-party Unity content.

See [`ASSET-NOTICE.md`](ASSET-NOTICE.md) for redistribution and provenance information. The project documentation does not assert a repository-wide license over third-party assets.

## Repository

Source: https://github.com/MasterChiefProject/RGBQuest
