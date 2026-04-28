---
name: game-art-orchestrator
description: Automates the process of generating game assets matching a specific art style using RAG, Few-Shot visual prompting (via generate_image), and a Human-In-The-Loop VLM evaluation pipeline.
---

# Game Artist Auto Orchestrator
This skill provides a 2-phase pipeline for strictly adhering to a stylistic `Generation_DNA` and Global rules when generating new game assets.

## Workflow Execution Steps

### Phase 1: Retrieve & Prompt Engineering (RAG & Few-Shot)
Whenever the user asks you to "draw", "generate", or "create" an asset, BEFORE generating the image, you MUST:
1. Identify the requested style from the prompt. If not explicitly specified, look at the workspace `Assets/GameArtist/StyleLibrary/` and pick the closest matching subfolder based on semantic meaning.
2. Read the `Generation_DNA.md` inside that style folder.
3. Automatically retrieve RAG context by executing the retrieval script via terminal:
   `python .agents/skills/game-art-orchestrator/scripts/retrieve_orchestrator_context.py <relative_path_to_style> "<user_prompt>"`
   *(For example: `python .agents/skills/game-art-orchestrator/scripts/retrieve_orchestrator_context.py Assets/GameArtist/StyleLibrary/SciFi "laser blaster"`)*
4. Read the `=== ORCHESTRATOR CONTEXT PAYLOAD ===` printed to the console by the script. **CRITICAL:** Do NOT attempt to manually parse the index JSON files or manually select reference images. You MUST strictly use the text rules and absolute image paths provided by the script.
5. Assemble your final payload by combining the local `Generation_DNA.md` with the retrieved Global RAG rules and Image paths.
6. **Background Rule Parsing**: If the requested asset belongs to an isolated category (e.g., `Characters`, `Items`, `Obstacles`, `UI`, `VFX`), you MUST explicitly inject the text `"strictly transparent background, no background, pure opacity"` into your image generation prompt.
7. Execute the `generate_image` tool (Gemini Nanobanana integration), passing the combined DNA rules into `Prompt`, and the paths of the Few-shot images into `ImagePaths`. **IMPORTANT: At this stage, append instructions to focus ONLY on Step 1: Sketching & Silhouettes. Do not generate full colors or polish yet.**

### Phase 2: Sequential Rendering Pipeline (Sketch -> Polish)
Instead of generating multiple variants simultaneously, the orchestration follows a structured 2-step approval process:

1. **Step 1: Sketching & Silhouettes**
   Present the generated sketch/silhouette to the user for confirmation. **CRITICAL: You MUST embed the generated image directly in your chat response using absolute markdown syntax: `![Image Name](absolute/path/to/artifact.png)` so the user can see it. On Windows, you MUST replace all backslashes (`\`) with forward slashes (`/`) or use `file:///` format so markdown parses correctly.**
   **HUMAN-IN-THE-LOOP CHECKPOINT**: Stop your execution and ask the user:
   - "Do you approve this sketch/silhouette?"
   - "Are there any structural adjustments needed before we proceed to coloring?"
   If the user requests adjustments, update the prompt and generate a new sketch. Loop until approved.

2. **Step 2: Flat Colors, Shading & Material Polish**
   Once (and ONLY after) the user confirms the sketch, update the generation prompt to apply **Flat Colors, Shading, Material Polish and Post-processing** while strictly maintaining the approved silhouette. Execute the `generate_image` tool to create the final render.

3. (Optional) Run `python scripts/downscale_image.py <image_paths>` if the final image is large and you anticipate Token Overflow when reading it.
4. Adopt the persona in `prompts/evaluator-vlm.md`. Evaluate the final rendered image against the user's initial prompt, the retrieved Global Rules, and the `Evaluation_Rules.json`. Provide a **Binary Validation Checklist** followed by an Aesthetic Score (0-100) and `Correction_Guidance`.
5. **FINAL CHECKPOINT**: Present the final image and Evaluator scoring to the user. **CRITICAL: You MUST embed the final image directly in your chat response using absolute markdown syntax: `![Image Name](absolute/path/to/artifact.png)`. Remember to use forward slashes (`/`) for paths.** Ask them:
   - "Do you approve this final render?" (If yes, trigger Phase 3: Asset Finalization & Export).
   - "Should we proceed to Round 2 (Prompt Translation and repolishing) using the correction guidance?"
   - "Would you like to manually override/add any text to the correction guidance before Round 2 runs?"
6. If the user requests Round 2, use the Orchestrator to perform a **Prompt Translation** on the updated `Correction_Guidance`. Convert the qualitative feedback into explicit technical parameters (update positive text, inject negative keywords, adjust weights) and re-run Step 2 rendering.

### Phase 3: Asset Finalization & Export
When the user explicitly approves a generated variant:
1. Identify the target **Project Name** from the user's initial prompt or context (e.g., `FlappyTrippy`). If unknown, verify with the user before exporting.
2. Identify the semantic category of the asset (e.g., `Characters`, `Environments`, `Items`, `UI`, `Obstacles`).
3. Construct the absolute export directory path using this professional structure:
   `<workspace>/<Project_Name>/GameAssets/<Style_Name>/<Category>/`
4. Execute a **copy** command (do NOT use move/delete) to transfer the approved image artifact from the `.gemini/artifacts/` folder into the target directory, ensuring the original remains in artifacts for rendering.
5. Rename the file to a clean, standardized format: `[object_name]_[YYYY-MM-DD].png` (e.g., `laser_pistol_2026-04-20.png`).
6. Read `<workspace>/<Project_Name>/GameAssets/Generated_Asset_Catalog.md`. Use your code editing tool to append a new row to the table in this file, logging the Category, Asset Name, Target Style, Absolute File Path, and a short 1-sentence prompt description. If the catalog file doesn't exist yet, create it with a standard markdown table header.
7. Confirm completion by providing the user with the final absolute path to their new production-ready asset. **CRITICAL: You MUST embed the final exported image directly in your chat response using absolute markdown syntax: `![Image Name](absolute/path/to/exported_asset.png)`. Make sure to replace all backslashes (`\`) with forward slashes (`/`).**
