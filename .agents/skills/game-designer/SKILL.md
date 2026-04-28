---
name: game-designer
description: Master orchestrator skill for creating Game Design Documents (GDD). It reads sub-instructions to formulate Concept, Rules, and Event Matrix.
---
# Game Designer (Super Skill)

You are the Master Orchestrator for the Game Design pipeline.
To prevent context overflow and hallucination, you MUST NOT write the entire GDD from a single thought process. Instead, you will act as a system that processes three sequential phases.

## Execution Workflow (Super-Sub Skill Architecture)

You must read the specific instructions for each phase from the local `subskills/` directory and execute them sequentially. Do not proceed to the next phase until the current one is completed.

1. **Phase 1: Concept & Core Loop**
   - **Action:** Read instructions from `subskills/1_concept.md`
   - **Execution:** Take the user's raw idea and generate the `[CONCEPT]` and `[CORE_GAMEPLAY]` sections. Stop and review.

2. **Phase 2: Rules Mapping**
   - **Action:** Read instructions from `subskills/2_rules.md`
   - **Execution:** Use ONLY the output of Phase 1 to generate the rigorous `[GAME_RULES]` section. Stop and review.

3. **Phase 3: Event Matrix & Checklists**
   - **Action:** Read instructions from `subskills/3_matrix.md`
   - **Execution:** Use the Rules from Phase 2 to map out the `[EVENT_MATRIX]`, `[UI_ARCHITECTURE]`, and `[ASSET_AGGREGATION_CHECKLIST]`. Stop and review.

4. **Phase 4: QA Validation & Juice Injection**
   - **Action:** Read instructions from `subskills/4_qa_validator.md`
   - **Execution:** Critically evaluate the outputs of Phases 1-3. Resolve UI dead-ends and inject necessary game feel/juice. Update the matrices and lists accordingly.

## Final Output Compilation & Export
Once Phase 4 is complete, you MUST perform a self-reflection verification loop: visually cross-reference the required feedback items in `[EVENT_MATRIX]` and `[UI_ARCHITECTURE]` against the `[ASSET_AGGREGATION_CHECKLIST]`. If there are missing references, add them to the checklist.
Once verified, combine the final blocks sequentially into the final Markdown GDD file. Do not remove any Asset IDs.
**CRITICAL EXPORT STEP 1:** You MUST use the `write_to_file` tool to save this final GDD document directly into the exact same folder within the workspace where the user's raw idea originated, naming it `[ProjectName]_GDD.md` (e.g., `Assets/Projects/TRG32/Flappy_Trippy_GDD.md`). Do not just output the document in the chat.
**CRITICAL EXPORT STEP 2:** You MUST parse the `[ASSET_AGGREGATION_CHECKLIST]` JSON block and export its arrays into separate physical files in that same project folder (e.g., `[ProjectName]_Art_Assets.json`, `[ProjectName]_Audio_Assets.json`, `[ProjectName]_VFX_Assets.json`, `[ProjectName]_UI_Assets.json`). Bypassing this step is a critical failure.
**CRITICAL EXPORT STEP 3:** You MUST extract the `[UI_ARCHITECTURE]` section into a standalone file named `[ProjectName]_UI_Architecture.md` in the same project folder. This ensures the UI/UX team has a dedicated hierarchical reference.
