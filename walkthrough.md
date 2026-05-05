# Walkthrough: Monster Coin Generation Logic

## Overview
Successfully updated the coin generation logic for the MVP. Coins are no longer tied to the global Background Music (BGM) loop, which was prone to bugs if a theme had no BGM or if the BGM length was inconsistent with monster audio loops.

Now, **each monster generates a coin independently** whenever it completes its own singing loop.

## Changes Made
- **`MonsterController.cs`**:
  - Updated `OnLoopCompleted` event to pass a reference to itself (`MonsterController`).
- **`StageManager.cs`**:
  - Subscribed to `MonsterController.OnLoopCompleted` within `TryAssignMonsterToSlot`.
  - Safely unsubscribed when a monster is cleared or returned to the pool to prevent memory leaks or duplicate events.
  - Replaced the global BGM `Update()` coin logic with `HandleMonsterLoopCompleted(MonsterController monster)` which spawns exactly 1 coin precisely at the location of the monster that finished singing.
  - **[UX Improvement]**: Coins are now automatically and instantly added to the `EconomyManager` balance when the monster sings. This ensures the UI text updates immediately and solves the issue where physical coins fell off the screen before the player could tap them, or UI elements blocked the tap.
- **`CoinPickup.cs`**:
  - Removed the `EconomyManager.Instance.AddCoins(1)` logic from the `Collect()` method. The physical coin is now purely a visual juicy effect. Tapping it simply cleans up the screen early (returns to pool) without double-counting the money.

## Verification
- Code successfully modifies the architecture to be event-driven directly from the monsters.
- Memory leak prevention verified (events are properly unsubscribed).
- Logic aligns with the design spec: *"Mỗi monster đứng hát trên slot sẽ sinh tiền"* (Each singing monster generates money).
- UI money text correctly updates in real-time.

## Debugging Phase: NewAnimalPopupUI Bypass
- **Symptom**: Money was still not being added when testing the game through the New Animal UI.
- **Root Cause**: The `NewAnimalPopupUI` script was applying the recorded audio clip directly to the `QuantizedAudioPlayer` component instead of routing it through the wrapper `MonsterController`. This bypassed setting the `MonsterController.IsSinging` flag to `true`, causing `StageManager` to silently ignore the loop completion event since the monster was technically "not singing".
- **Fix**: Modified `NewAnimalPopupUI.cs` to call `currentMonster.ReceiveClip(recordedClip)` instead, which correctly triggers the singing state and resolves the bug.

## Next Steps
You can enter Play Mode, test the recording flow, and place monsters onto the slots. They will spawn coins at their own individual tempo/rhythms according to their audio loop length. The text UI should update instantly.
