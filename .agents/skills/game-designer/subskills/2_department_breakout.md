# Phase 2: Department Breakout Generation

**Role:** You are the Lead Producer breaking down the verified GDD into actionable departmental specifications.

## Pre-requisite
You MUST have obtained human approval for Phase 1 (`[ProjectName]_Master_GDD.md`) before running this phase.

## Action
Read the `[ProjectName]_Master_GDD.md`. Autonomously extract and formulate requirements specifically for Art, Dev, Audio/VFX, and UI.
Use the `write_to_file` tool to create four distinct Markdown files in the project folder.

### File 1: `[ProjectName]_Art_Requirements.md`
- List of all 2D/3D sprites/models needed.
- Animation states required per entity.
- Visual style guidelines extracted from the concept.

### File 2: `[ProjectName]_Dev_Architecture.md`
- Core scripts needed (e.g. `GameManager`, `PlayerController`).
- Data structures and save states.
- List of core managers and physics requirements.

### File 3: `[ProjectName]_AudioVFX_List.md`
- Complete SFX list (e.g. `SFX_Jump`, `SFX_Hit`).
- Complete BGM list (e.g. `BGM_MainTheme`).
- Associated VFX (e.g. `VFX_Explosion_Particle`).

### File 4: `[ProjectName]_UI_Plan.md`
- Detailed hierarchical list of all UI Screens and Popups.
- For each screen, list buttons, text fields, and essential UX flow.
- Ensure screen names are uniquely identifiable (e.g. `UI_Screen_MainMenu`).

---
**CRITICAL:** Do not stop or wait for approval after creating these 4 files. Immediately proceed to Phase 3.
