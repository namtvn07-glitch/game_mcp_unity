# Monster-vox UI Integration Guide

## 1. Icon Setup
All newly generated icons (Arrow Right, Refresh, Play, Check) have been auto-cropped and resized to fit a maximum dimension of 256px.
- In Unity, set Texture Type to **Sprite (2D and UI)**.
- These icons can be nested as children inside a base button graphic (e.g., `UI_Btn_Round` or `UI_Btn_Large`).
- Uncheck `Raycast Target` on child icons to prevent them from blocking the click events of their parent Buttons.
- Ensure `Preserve Aspect` is checked on the Image component.

## 2. Slot Frame Setup
The `UI_Slot_Base` image is resized to max 512px.
- Use this as the background for the Monster Inventory slots in the `UI_Screen_Stage` and Store interfaces.
- If the frame needs to scale non-uniformly (e.g., for a wider horizontal list), use **9-Slicing**:
  1. Open the Sprite Editor for `UI_Slot_Base_2026-05-05.png`.
  2. Drag the green border lines to isolate the corners so they don't stretch.
  3. In the Image component on the UI, set Image Type to `Sliced`.

## 3. General Best Practices
- **VRAM Optimization:** All UI elements have had their magenta backgrounds removed and transparent boundaries cropped to save memory and avoid overlapping transparent raycast targets.
- **Hierarchy:** Build compound UI elements by separating the background graphic (Button/Panel) from the iconography, allowing independent scaling and coloring.
