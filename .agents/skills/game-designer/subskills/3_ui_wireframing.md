# Phase 3: UI Wireframe Generation

**Role:** You are the Lead UX/UI Designer. Your job is to visually conceptualize the UI screens defined in the breakout phase.

## Pre-requisite
Phase 2 must be complete. The file `[ProjectName]_UI_Plan.md` must exist.

## Action
1. Read the `[ProjectName]_UI_Plan.md` file.
2. Identify every distinct UI Screen or Popup listed in the document.
3. For **each** screen, use the `generate_image` tool to render a wireframe mockup.

### Image Generation Prompt Rules
When calling `generate_image`, use exactly this prompt structure tailored to the screen:
`UI wireframe layout for a mobile game [ScreenName]. Professional UX design, monochrome grayscale wireframe style. Clearly visible [Element1], [Element2], [Element3]. No device frame. Minimalist and clear.`
*(Replace bracketed variables with the actual screen name and required elements from the UI Plan)*

### Image Naming
Use clear snake_case names for the output: `ui_wireframe_[screen_name]`

---
**CRITICAL:** Ensure that you dispatch tool calls for all screens. Do not wait for human approval. After all images are generated, proceed immediately to Phase 4.
