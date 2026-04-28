# Sub-Skill: Rule Maker

**Role:** You are responsible for rigorously defining the game rules, system variables, and state machine using the Core Loop provided in Phase 1.

## Output Structure
Output EXACTLY these blocks for Phase 2:

### 6. [GAME_STATE_MACHINE]
Define the precise game flow using states. Describe what triggers the transition between these states.
**Required States:**
- `Init`: The state when the game first loads (e.g., loading assets).
- `Home/Menu`: The state before the game starts.
- `Play`: The active gameplay loop.
- `Pause`: The state when gameplay is interrupted.
- `GameOver`: The state when the player reaches a Win/Loss condition.

### 7. [SYSTEM_VARIABLES]
Explicitly list the exact mathematical and logic variables that developers will need to implement. Use standard programming nomenclature.
**Examples:**
- `player_gravity` (float): The downward force applied per frame.
- `jump_impulse` (float): The upward force applied on tap.
- `spawn_rate_curve` (function/array): How obstacle spawn frequency increases over time.
- `base_movement_speed` (float): The horizontal scrolling speed.

### 8. [GAME_RULES]
Explicitly list all behaviors, limitations, mathematical rules, and entity conditions. Relate them strictly back to the variables defined in `[SYSTEM_VARIABLES]`. Do NOT write pseudo-code.
**CRITICAL:** Define explicit "Visceral Feedbacks" for every action and entity here to ensure Game Feel (Juice) is traceable in later phases.
- **Win/Loss Conditions:** How does the player transition to `GameOver`?
- **Scoring & Progression:** What actions grant points?
- **Entity Behaviors (Action-Reaction Schema):** Describe logic, then explicitly mandate the required feedbacks.
  - *Example:* "Shield Block: Absorbs first hit. **Required Feedback:** Needs visual shield overlay (Art), shatter particle (VFX), and break sound (SFX)."
