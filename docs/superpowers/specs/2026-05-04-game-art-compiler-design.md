# Game Art Compiler - Game Design Principles Update (Design Spec)

## Overview
This specification details the update to the `game-art-compiler` skill to incorporate **Game Design Principles**. The current skill acts as a Data Ingestion Pipeline for extracting "Visual DNA" (shape language, material, color). The update introduces a new dimension: **Gameplay Affordance and Design Logic**. 

The goal is to ensure the AI (specifically `game-art-orchestrator`) enforces functional game design logic (e.g., readability, affordance, visual contrast) and not just aesthetic style.

## Architecture & Logic Changes

### 1. Step 2: VLM Semantic Tagging (Dynamic Context)
The prompt instructions for the Vision-Language Model (VLM) will be updated to include a two-pass classification and extraction step:

**A. Asset Categorization**
The VLM must classify each reference image into one of the following categories:
- `[Character]`
- `[Environment]`
- `[Prop/Interactable]`
- `[UI]`

**B. Contextual Extraction**
Based on the category, the VLM will extract specific Game Design Principles:
- **`[Character]`**: Silhouette Readability, Body Alignment, Proportion Rhythms.
- **`[Environment]`**: Guiding Lines, Depth Contrast (Foreground vs. Background separation), Scale perception.
- **`[Prop/Interactable]`**: Affordance (e.g., sharp/red = danger/obstacle, round/bright = collectible/safe).

### 2. Step 4: Synthesize DNA and Rule Files
The extracted Game Design Principles will be synthesized into the output rule schemas.

**A. `Generation_DNA.md` Updates**
Introduce a new, hardcoded section:
`# IV. GAMEPLAY AFFORDANCE & DESIGN LOGIC`
This section will explicitly list the rules generated from the dynamic extraction in Step 2. It will focus heavily on constraints that the `game-art-orchestrator` must respect to ensure gameplay readability.

**B. `Evaluation_Rules.json` Updates**
Expand the JSON schema to include a new evaluation array.
```json
{
  "evaluation_criteria": {
    "visual_dna": [...],
    "product_logic": [...],
    "technical_excellence": [...],
    "gameplay_affordance": [
      "Does the shape communicate its gameplay function?",
      "Is it distinct from the background (Visual Hierarchy)?"
    ]
  }
}
```

## Expected Flow
1. User requests to compile a style folder.
2. `game-art-compiler` invokes the VLM.
3. VLM categorizes each image and extracts both Visual DNA and Game Design Principles (Affordance, Hierarchy, Silhouette).
4. Embedding calculation continues normally.
5. The skill writes the updated `Generation_DNA.md` (now with 4 pillars) and the updated `Evaluation_Rules.json` to the style directory.
6. `game-art-orchestrator` will subsequently read these rules and generate assets that strictly adhere to the game's functional design rules.
