# Unity Dev Learnings & Patterns

## Code Architecture & Patterns

- **Prefab Variants for Data-Driven Spawning:** When spawning entities configured via ScriptableObjects (like Themes or Monsters), prefer linking `Prefab Variants` inside the ScriptableObject rather than hardcoding a generic base prefab and injecting properties. This encapsulates hierarchy-specific data (like child `Transforms` or `SpriteRenderers`) entirely within the variant, keeping Manager classes decoupled from visual setup.

## Gameplay & Physics

- **Physics2D with Orthographic Camera:** Avoid using `Camera.ScreenPointToRay` combined with `Physics2D.Raycast` for drag-and-drop or touch detection. `ScreenPointToRay` sets the Z origin to the camera's `nearClipPlane` (e.g., -10), which can cause 2D casts to miss or behave unpredictably. Instead, use `Camera.ScreenToWorldPoint` to get the XY coordinates and check with `Physics2D.OverlapPoint`.

## Audio

- **Audio Quantization on Mobile:** Prefer using Silence Padding (trimming original audio and adding silence to match the Grid) over Time-Stretch to prevent pitch distortion and maintain Zero Latency.
