# Monster Coin Generation Logic

The objective is to implement the coin generation logic for each singing monster. Currently, `StageManager` attempts to spawn coins based on the Background Music (BGM) loop duration. However, this causes issues if the BGM is missing or out of sync, and it violates the design intent where each monster's own singing loop should dictate its coin generation (as hinted by the `OnLoopCompleted` event in `MonsterController`).

We will refactor this so that **each monster generates coins independently** when it finishes its own singing loop.

## User Review Required
> [!IMPORTANT]
> **Change in Timing**: Coins will now drop precisely when a monster finishes its own audio loop, rather than waiting for the global BGM loop to finish. This makes coin generation robust even if a Theme lacks BGM.
> **Optional Enhancement**: Should we add a `coinDropAmount` field to `MonsterDataSO` so rarer monsters drop more coins? (The current plan uses a base drop of 1 coin per loop as defined in the GDD, but we can easily add this field).

## Proposed Changes

### Data Layer
*(No immediate changes required unless we want to add `CoinDropAmount` to `MonsterDataSO`. We will stick to the default 1 coin for now).*

### Core Logic Layer
#### [MODIFY] `StageManager.cs`
- Subscribe to `MonsterController.OnLoopCompleted` when assigning a monster to a slot.
- Unsubscribe when clearing monsters or deactivating the stage.
- Create a new method `HandleMonsterLoopCompleted(MonsterController monster)` that spawns a coin directly at that monster's position.
- Remove the old `OnBgmLoopCompleted` BGM-based coin logic in `Update()` to prevent double-spawning and decouple the economy from the BGM.

### Gameplay/UI Layer
#### [MODIFY] `MonsterController.cs`
- Pass a reference of itself when firing the loop event, so `StageManager` knows exactly which monster completed its loop and where to spawn the coin.
- Update the event signature: `public event System.Action<MonsterController> OnLoopCompleted;`
- Update `HandleLoopCompleted` to invoke with `this`.

## Verification Plan
### Automated Tests
- None required for this specific logic change.

### Manual Verification
- Step 1: Enter Play Mode.
- Step 2: Drag and drop a monster onto a slot.
- Step 3: Wait for the monster's audio to complete one loop.
- Step 4: Verify that exactly 1 coin spawns from the monster's position, even if the Theme's BGM is muted or missing.
- Step 5: Collect the coin and verify the economy balance increases.
