# MAFHOOM — Product & Design Guidelines

> Living document. Update this whenever a design decision is made, a feature is scoped, or a standard changes.  
> For Claude-specific coding rules, see `CLAUDE.md`.

---

## 1. Vision

**MAFHOOM** is a multiplayer pixel-art educational game set inside a pixelated recreation of **Prince Sultan University**. Instructors upload their lecture content; the game transforms it into real-time, networked concept puzzles students solve together inside a maze built from their own campus.

### One-sentence pitch
> *Your lecture. Your campus. Your game.*

### Design north star
Professional enough for a university setting. Cool enough that students want to play it. Every screen should feel like it belongs in a polished indie game — not a school portal.

---

## 2. Visual Identity

### 2.1 Art Style
- **Pixel art** — 16×16 / 32×32 tile grid, clean outlines, limited palette per scene
- Reference games: *Celeste*, *Undertale*, *Stardew Valley* — not 8-bit retro kitsch, but modern pixel art with intentional design
- All environments are **pixelated recreations of PSU campus** (halls, lecture rooms, corridors, outdoor plazas) using tilesets built from reference photos
- Characters are pixel sprites (already started: Hesham and Wadeema avatars)
- UI panels use a **pixel-border / pixel-shadow** style — not rounded blur cards

### 2.2 Color Palette

Use this 12-color palette across the entire game. Do not introduce colors outside it without a documented reason.

| Role | Name | Hex | Use |
|---|---|---|---|
| Background deep | **Void** | `#0D0D1A` | Scene backgrounds, dark panels |
| Background mid | **Campus Night** | `#1A1A2E` | Card backgrounds, modals |
| Background light | **Hall Stone** | `#2A2A4A` | Secondary panels, borders |
| Primary accent | **PSU Blue** | `#3B5BDB` | Primary buttons, active states, links |
| Secondary accent | **Gold** | `#F5A623` | Highlights, score counters, START button |
| Success | **Chalk Green** | `#40C057` | Correct answers, unlocked doors, success banners |
| Danger | **Red Marker** | `#FA5252` | Wrong answers, delete buttons, errors |
| Text primary | **Paper** | `#F8F9FA` | Main text on dark backgrounds |
| Text secondary | **Mist** | `#ADB5BD` | Subtitles, placeholder text, captions |
| Overlay | **Ink** | `#000000` | Fade overlays, shadows |
| Chalkboard | **Slate** | `#2D3748` | Onboarding slide backgrounds |
| Pixel grid | **Pixel Dim** | `#495057` | Tile borders, separator lines |

### 2.3 Typography

**Two fonts — one English, one Arabic. Both must be loaded as TMP Font Assets.**

#### English / Latin
**Recommended: Press Start 2P** (pixel, retro-professional) for headers + UI labels  
**Recommended: VT323** (softer pixel) for body text and slide content  
Download: Google Fonts → import TTF → create TMP Font Asset in `Mafhoom/Assets/Fonts/`

#### Arabic
**Recommended: Cairo** (clean, readable, modern — available in Google Fonts)  
Arabic content is right-to-left. Always set `m_isRightToLeft: 1` on Arabic TMP components.  
Do not use Press Start 2P for Arabic — it has no Arabic glyphs.

#### Size Scale
| Role | Size | Font |
|---|---|---|
| Scene title | 48 | Press Start 2P |
| Section header | 32 | Press Start 2P |
| Button label | 18 | Press Start 2P |
| Body / slide text | 22 | VT323 |
| Caption / hint | 16 | VT323 |
| Arabic body | 28 | Cairo |

---

## 3. Scene Specifications

### Scene 1 — Home / Role Select

**Purpose:** First screen every user sees. Shows the MAFHOOM brand, runs onboarding slides for first-time users, then presents the Instructor / Student role choice.

**Current state (after bug fixes on `trialME`):**
- Background: `background mafhoom.jpg` via `BG_Classroom` Image ✓
- PageSlider enabled, 3 onboarding slides ✓
- SceneFader creates its own overlay, fades correctly ✓
- FadeOverlay initial alpha fixed to 0 ✓

**Onboarding — 3 slides:**

| Page | Title | Content summary |
|---|---|---|
| 1 | WHAT IS MAFHOOM? | Mission statement — turns lectures into multiplayer maze puzzles |
| 2 | HOW TO PLAY? | 5 rules — instructor adds terms → students join by code → maze → solve puzzles → escape |
| 3 | MADE BY STUDENTS | Credits + "What if revising felt like a game?" → MAFHOOM |

**Planned UI improvements for Scene 1:**
- Replace current fonts with Press Start 2P (headers) + VT323 (body)
- Redesign onboarding panel as a pixel-bordered chalkboard card (not a plain white panel)
- Add subtle animated pixel dust / chalk particles behind the onboarding panel
- Background `mafhoom.jpg` should be slightly dimmed (dark overlay, ~0.4 alpha) behind onboarding panel so text is readable — this replaces the "blur" idea
- Role select buttons (Instructor / Student) should use pixel-art button sprites with hover state

**Role Select after onboarding:**
- Two large pixel-art buttons: `INSTRUCTOR` and `STUDENT`
- INSTRUCTOR: PSU Blue (`#3B5BDB`) with a desk/teacher icon
- STUDENT: Gold (`#F5A623`) with a backpack/student icon
- Clicking either triggers `SceneFader.FadeToScene()` → smooth black fade → target scene

---

### Scene 2/3 — Character Intro (instructor / student paths)

*Not detailed yet — revisit after Scene 1 is complete.*

---

### Future: Scene — AI Slide Upload (instructor flow)

*See Section 5 — AI Integration.*

---

## 4. Feature Roadmap

### Now (active sprint — Scene 1)
- [x] Fix PageSlider disabled bug
- [x] Fix SceneFader black-screen bug
- [x] Fix FadeOverlay blocking background
- [ ] Replace fonts (Press Start 2P + VT323 + Cairo TMP assets)
- [ ] Redesign onboarding panel (pixel border, chalkboard style)
- [ ] Add dim overlay behind onboarding so background is readable
- [ ] Redesign role-select buttons with pixel art sprites

### Next (Scene 2 / 3 polish)
- [ ] Audit Scene2_hessa and Scene3_hessa for same class of bugs (disabled components, fade issues)
- [ ] Consistent font system across all scenes
- [ ] Consistent button style

### Major Feature — AI Lecture-to-Puzzle
- [ ] Backend service / Unity integration for slide parsing (see Section 5)
- [ ] Instructor UI: "Upload Slides" panel in Lecture Content screen
- [ ] AI term extraction + instructor review flow
- [ ] Integration with existing `RoomTermsManager` pipe-separated term storage

### Future Features (backlog)
- [ ] **Leaderboard** — per-session real-time ranking visible to instructor and students
- [ ] **Hint system** — students can request a hint (costs time penalty); instructor sees hint usage
- [ ] **Replay mode** — instructor can replay the session with highlighted mistakes per student
- [ ] **Multi-language support** — UI language toggle (Arabic / English)
- [ ] **Custom avatars** — more character choices beyond Hesham / Wadeema
- [ ] **More puzzle types** — fill-in-the-blank, matching pairs, definition reveal (not just anagram)
- [ ] **Export results** — instructor downloads session summary as PDF after session ends
- [ ] **Tutorial maze** — guided first run so students know the controls before joining a live session
- [ ] **Accessibility** — colorblind mode, text scaling, reduced motion option

---

## 5. AI Integration — Architecture

### 5.1 What it does
Instructor uploads lecture slides → AI reads them → extracts the key academic terms → returns a ranked list → instructor reviews and edits → starts session with those terms.

The terms feed directly into the existing `RoomTermsManager` (pipe-separated string, synced to all Photon clients). The puzzle system (`PuzzleGenerator`) scrambles them as before. **Zero changes to the puzzle or networking layer required.**

### 5.2 Input formats supported
| Format | Approach |
|---|---|
| PDF | Extract text server-side via pdf-parse (Node) or PyMuPDF (Python) |
| PPTX | Parse OOXML on server — extract slide text nodes |
| Plain text | Pass directly to AI — no parsing needed |
| Images / screenshots | Send to AI vision endpoint (Claude's vision or GPT-4o vision) for OCR + extraction |

### 5.3 Architecture — two options

**Option A — Lightweight backend (recommended)**
```
Unity (Instructor Panel)
  │
  ▼  HTTP POST (multipart/form-data: file + session_id)
Backend Service  ←── Node.js or Python FastAPI, hosted on Vercel / Railway / Render (free tier)
  │  1. Extract text from file
  │  2. Call AI API (Claude / GPT-4o)
  │     Prompt: "Extract the 5–10 most important academic terms from this lecture content.
  │              Return JSON: { terms: [ { term: string, definition: string } ] }"
  │  3. Return JSON to Unity
  ▼
Unity parses JSON → populates Lecture Content term list
  │
  ▼  Instructor reviews / edits / deletes terms
  │
  ▼  Clicks "Start Live Session" → terms pushed to Photon room properties
```

**Option B — Direct from Unity (simpler, no server)**
- Call the AI API directly from Unity using `UnityWebRequest`
- Only works cleanly for plain-text and small PDFs
- API key lives in the build (security risk — use only for prototype / demo)
- Upgrade to Option A before any public release

### 5.4 Prompt template (Claude API)
```
System:
You are an educational content extractor for a university game called MAFHOOM.
Extract academic vocabulary terms from the lecture content provided.
Return ONLY valid JSON, no explanation.

User:
Lecture content:
<CONTENT>

Return JSON in this exact shape:
{
  "terms": [
    { "term": "string (max 20 chars)", "definition": "string (1 sentence)" }
  ]
}
Rules:
- 5 to 10 terms maximum
- Terms must be nouns or noun phrases
- Prefer domain-specific vocabulary over common words
- Sort by importance (most central concept first)
```

### 5.5 Unity integration points
- New script: `AITermExtractor.cs` in `Mafhoom/Assets/C#CODES/`
- Called from `InstructorTermsUI.cs` after instructor uploads/pastes content
- On success: populates the term list exactly as if the instructor typed them manually
- Instructor can edit, delete, or add terms after AI runs — AI output is a suggestion, not final
- On failure: show error banner, instructor enters terms manually (existing flow)

---

## 6. UI Component Standards

All UI components follow pixel-art conventions. These rules apply to every scene.

### Buttons
- Default: pixel-bordered rectangle, `PSU Blue` fill, `Paper` text, Press Start 2P 18pt
- Hover: `Gold` border glow (2px outer border animates in), slight scale 1.05
- Pressed: scale 0.97, darken fill by 10%
- Disabled: `Pixel Dim` fill, `Mist` text, no interaction
- Use `ButtonHoversEffect.cs` (already exists) for hover animation
- Never use Unity's default button style

### Panels / Cards
- Background: `Campus Night` (`#1A1A2E`)
- Border: 2–4px solid `Hall Stone` (`#2A2A4A`), pixel-stepped corners (no CSS border-radius equivalent)
- Header stripe: `PSU Blue` at top of panel, `Paper` text

### Text input fields
- Background: `Void` (`#0D0D1A`)
- Border: `Hall Stone`, switches to `PSU Blue` on focus
- Placeholder: `Mist` colour
- Always TMP Input Field (not legacy Input Field)

### Modals / overlays
- Background: `Ink` at 70% alpha behind modal
- Modal panel: standard Card style
- Always animated in (slide up or fade in, 0.25s)

### Onboarding slides (Scene 1 specific)
- Slide background: `Slate` (`#2D3748`) — chalkboard colour
- Title: Press Start 2P 32pt, `Gold`
- Body: VT323 22pt, `Paper`
- Red emphasis text: VT323 22pt, `Red Marker`
- Character avatar: right-aligned pixel sprite, animated idle

---

## 7. Asset Naming & Folder Conventions

```
Mafhoom/Assets/
├── Backgrounds/          background images (scene BG)
├── C#CODES/              ALL scripts (29 existing + new)
├── Fonts/                TMP Font Assets (.asset files)
│   ├── PressStart2P-Regular.asset
│   ├── VT323-Regular.asset
│   └── Cairo-Regular.asset
├── Sprites/
│   ├── UI/               buttons, panels, borders, icons
│   ├── Characters/       player + NPC sprites + animation frames
│   ├── Tileset/          campus room tilesets (32×32)
│   └── FX/               particles, dust, glows
├── Audio/
│   ├── BGM/              background music tracks
│   └── SFX/              click, correct, wrong, door open, etc.
├── Scenes_dst/           all Unity scene files
├── Prefabs/              reusable GameObjects
└── Animations/           Animator controllers + animation clips
```

Naming rules:
- Sprites: `noun_descriptor_state.png` → `btn_instructor_default.png`, `tile_hallway_wall.png`
- Scripts: `PascalCase.cs` matching class name exactly
- Scenes: `Scene_N_Name.unity` (consistent prefix)
- TMP Font Assets: match font family name exactly

---

## 8. Audio Standards

- BGM: looping, scene-specific (see `BGMManager.cs`)
- SFX: short (< 0.5s), triggered on button press, answer correct/wrong, door events
- All sounds go through `AudioSource` components — never `AudioSource.PlayClipAtPoint` in hot paths
- BGM volume: 0.4 (background, not intrusive)
- SFX volume: 0.8

---

## 9. Networking Standards (Photon PUN 2)

No changes to the Photon layer unless explicitly required by a feature.

- Session code = Photon room name
- Term list = pipe-separated string in Photon room custom properties (`RoomTermsManager`)
- Student progress = updated via RPC or custom player properties
- AI-generated terms are loaded into the **instructor's local term list first** — only pushed to Photon when instructor clicks "Start Live Session". This keeps AI latency off the critical path.

---

## 10. Development Workflow

### Branch strategy
- `main` — stable, tested, merged only via PR
- Feature branches: `feature/scene1-font-overhaul`, `feature/ai-term-extractor`, etc.
- Bug fix branches: `fix/description`
- Current active: `trialME` (Scene 1 bug fixes)

### Before opening any PR
1. Test in Unity Play mode — golden path + edge cases
2. No console errors or warnings added
3. No `.meta` files deleted or regenerated
4. No `ProjectSettings/` changes without explicit approval
5. CLAUDE.md and this document updated if conventions changed

### Scene 1 checklist before shipping
- [ ] Background visible on load
- [ ] Onboarding: 3 slides navigate correctly (NEXT/BACK/START)
- [ ] START button closes onboarding, reveals role-select buttons
- [ ] Clicking INSTRUCTOR → fade to black → Scene2_hessa loads → fades in
- [ ] Clicking STUDENT → fade to black → Scene3_hessa loads → fades in
- [ ] Returning to Scene 1: onboarding skipped (onboardingSeenThisSession = true)
- [ ] BGM plays from scene load, stops when leaving
- [ ] No console errors in any of the above flows

---

*Last updated: 2026-06-29 — HALEISSA + Claude Sonnet 4.6*
