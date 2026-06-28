# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MAFHOOM is a 2D multiplayer educational game built in Unity 6 (6000.3.8f1). Instructors create vocabulary/term puzzles; students join via session code and solve them in networked maze/gameplay scenes. Networking is handled by **Photon PUN 2** (embedded in `Mafhoom/Assets/Photon/`).

## Build & Development

Open the project in the Unity Editor (Unity 6000.3.8f1). There are no CLI build scripts or custom Makefile targets.

- **Open project:** Load `C:\Users\HALEI\Playground` as the Unity project root (uses `Assets/` and `ProjectSettings/` at the root).
- **Command-line build (Windows):**
  ```
  Unity.exe -projectPath "C:\Users\HALEI\Playground" -buildTarget Win64 -buildWindowsPlayer "Build/MAFHOOM.exe" -quit
  ```
- **IDE:** Visual Studio (via `MAFHOOM.slnx`) or JetBrains Rider. VSCode is configured for debugging via `Attach to Unity` (`vstuc` extension).
- **Tests:** Unity Test Framework (`com.unity.test-framework 1.6.0`) is available but no test suites exist yet. Run via Unity Editor → Window → General → Test Runner.

## Architecture

### Dual Asset Structure (Important)

The repo has **two asset trees**:
- `Assets/` — Unity project root used by the Editor; contains scenes and render pipeline settings.
- `Mafhoom/Assets/` — contains all game scripts (`C#CODES/`), scenes (`Scenes_dst/`), Photon library, prefabs, audio, and sprites.

All 29 game scripts live in `Mafhoom/Assets/C#CODES/`.

### Scene Flow

```
Scene_1_Home → (role select) → Scene2/Scene3_hessa (character intro)
    → Scene_4_Hessa_Character_Select → Scene_4_Gameplay (instructor)
                                     → Scene_5_SoomiMaze (student maze)
    → FinalScene_Exit
```

Transitions use `SceneManager.LoadScene()`. Fades are handled by `SceneFader.cs` / `SceneFader1.cs` (coroutine-based).

### Networking (Photon PUN 2)

- `PhotonLauncher.cs` — singleton; handles connection, room creation/joining.
- `RoomTermsManager.cs` — stores puzzle terms as pipe-separated strings in Photon room custom properties for cross-client sync.
- `InstructorSessionController.cs` / `StudentSessionManager.cs` — instructor creates the session; students join via session code.
- Scripts that implement Photon callbacks must inherit from `MonoBehaviourPunCallbacks` or implement `IPunObservable`.
- Player ownership is checked via `photonView.IsMine` before processing input (e.g., `TopDownMovement.cs`).

### Puzzle System

- `PuzzleGenerator.cs` (singleton) — scrambles word characters for puzzles.
- `RoomTermsManager.cs` — persists term list across networked clients.
- `PuzzleRoomTrigger.cs` — detects when a player enters a puzzle room trigger zone.
- `StudentProgressManager.cs` — tracks per-student puzzle completion.

### Singleton Managers

Several managers use the singleton pattern and persist across scenes:
`PhotonLauncher`, `PuzzleGenerator`, `RoomTermsManager`, `StudentSessionManager`, `BGMManager`

### UI Conventions

- All text uses **TextMeshPro** (`TMP_Text` / `TextMeshProUGUI`).
- Hover effects implement `IPointerEnterHandler` / `IPointerExitHandler`.
- Animations (fade, typewriter, pop-in) are coroutine-based.

## Key Packages

| Package | Version | Purpose |
|---|---|---|
| Photon PUN 2 | embedded | Multiplayer networking |
| com.unity.inputsystem | 1.18.0 | New Input System (WASD movement) |
| com.unity.render-pipelines.universal | 17.3.0 | URP 2D renderer |
| TextMeshPro | 3.0.9 | All in-game text |
| com.unity.2d.animation | 13.0.4 | Sprite animation |

## Self-improvement
 
After any correction from the user: append to tasks/corrections.log.
If the correction is non-trivial, persist into lessons:
Plugin-specific first ({plugin}/tasks/lessons.md).
Shared only if cross-plugin (tasks/lessons.md).
CLAUDE.md only if critical (must-follow everywhere).
If a Skill tool returns disable-model-invocation, the skill exists. Read ~/.claude/skills/{name}/skill.md directly and follow it manually.

## When to stop and ask the user

Default to asking, don't guess, when:

- Scope is ambiguous.
- A git operation is destructive (--force, -D, --hard, rm of tracked files).
- Deleting data, files, or tables.
- Change crosses plugin boundaries.
- Unclear whether task is "new" or "modify existing".
- Operation is irreversible.
- Codex/bot review suggests a "fix" - bots lack business context, may revert intended design.
- User's request conflicts with a rule in this file.