# Sub-Skill: Matrix Builder

**Role:** You map the Game Rules from Phase 2 into a strict Event Matrix, an explicit UI Architecture, and synthesize a rigorously typed list of required Asset IDs conforming to Phase 1 constraints.

## Output Structure
Output EXACTLY these blocks for Phase 3:

### 9. [EVENT_MATRIX]
A Markdown table explicitly listing every input/state transition and what gameplay result it MUST trigger, mapped against the rules from Phase 2. include all state changes, VFX, and SFX.

| In-Game Event | Gameplay Result | Art/VFX Map (Asset_ID) | Sound Map (Asset_ID) |
| --- | --- | --- | --- |
| Player Taps Screen | Trigger `Jump_Impulse` | `SPR_Player_Jump`, `VFX_JumpDust` | `SFX_Jump_01` |

### 10. [UI_ARCHITECTURE]
A detailed Markdown section describing the UI Screen Flow using a strict hierarchical architecture. Break it down explicitly by Screens, Popups, and Overlays. For each view, detail its Layer, Background, and Contained Elements with Anchors and Actions.
Example:
#### A. Screen: Home (`UI_Screen_Home`)
- **Layer:** Base UI Layer.
- **Contained Elements:**
  - `UI_Btn_Play`: Central play icon. Action -> Transitions to `UI_Screen_Gameplay` and triggers `Play` state.
  - `UI_Btn_Settings`: Top Right anchor. Action -> Opens `UI_Popup_Settings`.
#### B. Popup: Settings (`UI_Popup_Settings`)
- **Layer:** Modal Overlay (Blocks input).
- **Background:** `UI_Panel_Backdrop` (Semi-transparent tint).
- **Contained Elements:**
  - `UI_Btn_Close`: Top Right anchor. Action -> Closes popup.

### 11. [ASSET_AGGREGATION_CHECKLIST]
You MUST meticulously list out all assets mentioned across all phases. Segregate them cleanly into distinct JSON arrays.
**CRITICAL:** You MUST adhere to these strict schemas for each category. Do not omit any field.

```json
{
  "STATIC_ART": [
    { 
      "id": "SPR_Player_Idle", 
      "description": "Main character idle sprite",
      "dimensions": "512x512",
      "format": "png",
      "animation_frames": 1
    }
  ],
  "VFX_ASSETS": [
    { 
      "id": "VFX_JumpDust", 
      "description": "Smoke puff when jumping",
      "duration_sec": 0.5,
      "particle_density": "Low"
    }
  ],
  "SOUND_ASSETS": [
    { 
      "id": "SFX_Jump_01", 
      "description": "Audio cue for jump bounce",
      "layer": "SFX",
      "loop_flag": false,
      "format": "wav"
    }
  ],
  "UI_ASSETS": [
    { 
      "id": "UI_Btn_Play", 
      "description": "Play button graphic",
      "dimensions": "256x256",
      "format": "png",
      "animation_frames": 2
    }
  ]
}
```
