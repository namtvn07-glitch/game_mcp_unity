# Game Art Compiler Update Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Update `game-art-compiler` skill to extract and enforce Game Design Principles based on asset categorization.

**Architecture:** Modify the VLM Semantic Tagging instructions in `SKILL.md` to classify images and extract context-specific rules (character, environment, prop). Add `# IV. GAMEPLAY AFFORDANCE & DESIGN LOGIC` to the `Generation_DNA.md` schema and `gameplay_affordance` to `Evaluation_Rules.json`.

**Tech Stack:** Markdown (Prompting).

---

### Task 1: Update VLM Semantic Tagging Rules

**Files:**
- Modify: `e:\_Project_2026\UnityMCP\Unity-MCP\.agents\skills\game-art-compiler\SKILL.md`

- [ ] **Step 1: Update Step 2 in SKILL.md**

Modify Step 2 to add the Asset Categorization and Contextual Extraction rules as specified in the design doc.

```diff
- ### 2. VLM Semantic Tagging (RAG-Optimized)
- - Process all valid image files (`.png`, `.jpg`, `.jpeg`) inside the folder.
- - Execute a VLM check (e.g. via Gemini/Flash) for EACH image.
- - Ask the VLM to produce a highly dense, searchable string (Embedding-Ready) instead of loose sentences. It must explicitly extract:
-   1. **Shape Language** (e.g. rounded, sharp)
-   2. **Material** (e.g. metallic, plastic, flat-color)
-   3. **Complexity Level** / Rarity (e.g. basic, highly detailed)
-   4. **Color Palette** (e.g. neon green, high contrast)
- - Also generate a **RAG Hook Tag** (e.g. `"tag: UI_Icon, material: metallic, rarity: epic, visibility: high_contrast"`).
- - Keep a JSON array in memory or disk containing: `[{"filename":"...", "semantic_metadata":"..."}]`
+ ### 2. VLM Semantic Tagging (RAG-Optimized & Dynamic Context)
+ - Process all valid image files (`.png`, `.jpg`, `.jpeg`) inside the folder.
+ - Execute a VLM check (e.g. via Gemini/Flash) for EACH image.
+ - **Pass 1: Asset Categorization:** The VLM must classify the image into one of: `[Character]`, `[Environment]`, `[Prop/Interactable]`, or `[UI]`.
+ - **Pass 2: Contextual Extraction & Visual DNA:** Ask the VLM to produce a highly dense, searchable string (Embedding-Ready). It must explicitly extract:
+   1. **Visual DNA:** Shape Language, Material, Complexity Level / Rarity, and Color Palette.
+   2. **Game Design Principles (Based on Category):**
+      - `[Character]`: Silhouette Readability, Body Alignment, Proportion Rhythms.
+      - `[Environment]`: Guiding Lines, Depth Contrast (Foreground vs. Background), Scale perception.
+      - `[Prop/Interactable]`: Affordance (e.g., sharp/red = danger, round/bright = collectible).
+ - Also generate a **RAG Hook Tag** (e.g. `"tag: UI_Icon, material: metallic, rarity: epic, visibility: high_contrast"`).
+ - Keep a JSON array in memory or disk containing: `[{"filename":"...", "semantic_metadata":"..."}]`
```

### Task 2: Update Output Schema (DNA and JSON)

**Files:**
- Modify: `e:\_Project_2026\UnityMCP\Unity-MCP\.agents\skills\game-art-compiler\SKILL.md`

- [ ] **Step 1: Update Step 4A and 4B in SKILL.md**

Modify Step 4 to include the new pillar in `Generation_DNA.md` and the new field in `Evaluation_Rules.json`.

```diff
-   **A. `Generation_DNA.md`**
-   Write this file using `write_to_file` into the style directory. It must use strict nested Markdown headings (for text-splitters) containing the following three pillars:
-   - **`# I. VISUAL DNA`**: Consistency rules, Shape Language, Color Script (Rarity), and Material Polish.
-   - **`# II. PRODUCT LOGIC`**: Readability (3-Second Rule), Progression Logic (Level upgrades), The Juice (Animation states), and UI Interaction rules (if applicable).
-   - **`# III. TECHNICAL EXCELLENCE`**: Mesh / Topology rules, Texel Density, Modular design / Recolor limits.
-   Include positive anchors and strict negative safeguards under these headings.
+   **A. `Generation_DNA.md`**
+   Write this file using `write_to_file` into the style directory. It must use strict nested Markdown headings (for text-splitters) containing the following FOUR pillars:
+   - **`# I. VISUAL DNA`**: Consistency rules, Shape Language, Color Script (Rarity), and Material Polish.
+   - **`# II. PRODUCT LOGIC`**: Readability (3-Second Rule), Progression Logic (Level upgrades), The Juice (Animation states), and UI Interaction rules (if applicable).
+   - **`# III. TECHNICAL EXCELLENCE`**: Mesh / Topology rules, Texel Density, Modular design / Recolor limits.
+   - **`# IV. GAMEPLAY AFFORDANCE & DESIGN LOGIC`**: Synthesized rules from the dynamic context extraction (e.g., silhouette rules, guiding lines, affordance logic).
+   Include positive anchors and strict negative safeguards under these headings.
    
    **B. `Evaluation_Rules.json`**
    Write this file using `write_to_file` into the style directory. It must conform to this schema:
    ```json
    {
      "semantic_metadata": {
        "supported_types": ["asset", "ui"],
        "vector_keywords": ["list", "of", "dense", "visual", "keywords"]
      },
      "evaluation_criteria": {
        "visual_dna": ["Are shapes consistent?", "Does it match standard rarity palettes?"],
        "product_logic": ["Does it pass the 3-second readability rule?"],
-       "technical_excellence": ["Are details readable at thumbnail size?"]
+       "technical_excellence": ["Are details readable at thumbnail size?"],
+       "gameplay_affordance": ["Does the shape communicate its gameplay function?", "Is it distinct from the background (Visual Hierarchy)?"]
      },
      "forbidden_elements": ["List of anti-patterns"]
    }
    ```
```

*(Note: Per user request, no `git commit` steps are included in this plan.)*
