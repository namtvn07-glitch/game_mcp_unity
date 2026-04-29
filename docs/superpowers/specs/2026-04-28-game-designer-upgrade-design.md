# Design Specification: Game-Designer Skill Pipeline Upgrade

## Overview
Re-architect the `game-designer` skill to transform it from a single-run JSON dumper into an autonomous sequentially executed, multi-phase document & visual generator. The skill will act as a pipeline requiring human approval only once (after the core GDD is written), before automatically executing breakouts, UI wireframing, cross-validation, and integration mapping.

## Architecture & Data Flow

1. **Phase 1: GDD Master (Human Gate)**
   - Agent interprets raw idea and produces `[ProjectName]_Master_GDD.md`.
   - Content: Coreloop, Gameplay, Features, Business/Development Strategy.
   - **System Instruction**: Agent MUST Halt and explicitly prompt the user for approval. 

2. **Phase 2: Department Breakout (Autonomous)**
   - Pre-requisite: User approved Phase 1.
   - Action: Agent parses `Master_GDD` and splits the requirements physically into 4 markdown files:
     - `[ProjectName]_Art_Requirements.md`
     - `[ProjectName]_Dev_Architecture.md`
     - `[ProjectName]_AudioVFX_List.md`
     - `[ProjectName]_UI_Plan.md`

3. **Phase 3: UI Wireframing (Autonomous)**
   - Pre-requisite: Phase 2 completion.
   - Action: Agent reads `[ProjectName]_UI_Plan.md`. For each UI screen listed, the agent will autonomously invoke the `generate_image` tool using a standardized prompt (e.g., `Monochrome UI wireframe layout for [screen_name], clear UX elements, flat design`).
   - Deliverable: Rendered UI images saved to the project directory alongside the documents.

4. **Phase 4: Cross-Validation & Auto-Correction (Autonomous)**
   - Pre-requisite: Phase 2 and 3 completion.
   - Action: The agent self-reflects and cross-references all 4 layout `.md` files.
   - Test: "Is there a particle effect defined in Dev that is missing in AudioVFX?" "Is there a button on the UI wireframes/plan missing a SFX definition?"
   - Resolution: Automatically append missing entries into the respective files.

5. **Phase 5: Integration Map (Autonomous)**
   - Pre-requisite: Phase 4 validation passed.
   - Action: Synthesize how these assets and code tie together into `[ProjectName]_Integration_Map.md`.
   - Content: Event triggers mapping (e.g. Action -> UI flow -> Audio triggered -> VFX played -> Code logic executed).

## Delivery Requirements
- Replace `SKILL.md` to dictate this overarching loop and strictly enforce the stop condition at Phase 1.
- Delete old files (or overwrite) in `subskills/` and replace with:
  - `1_gdd_core.md`
  - `2_department_breakout.md`
  - `3_ui_wireframing.md`
  - `4_cross_validation.md`
  - `5_integration_map.md`
