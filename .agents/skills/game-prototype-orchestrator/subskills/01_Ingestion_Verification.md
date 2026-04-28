# Phase 1: Ingestion & Verification

**Trigger:** Called by `game-prototype-orchestrator` when starting a new prototype.
**Inputs:** GDD (Markdown) and Asset Manifests (JSON/CSVs).
**Outputs:** Verification Logs and Warning placeholders in the active session.

## Execution Steps:
1. Read the provided Game Design Document (GDD) and Asset Manifests.
2. Verify the physical presence of referenced 2D sprites and Sound Fx in the workspace.
3. **Mocking/Placeholder Strategy**: If any required asset is missing, do NOT halt execution. Instead, assign an **explicit Unity API Placeholder mapping**. 
   - *For missing Sprites/Models:* Map to Unity Built-in Primitives (e.g., `PrimitiveType.Quad` or `PrimitiveType.Cube`) with a specific Material Color.
   - *For missing Audio:* Map to `null` or a default Unity error beep if available.
4. Log a markdown `<WARNING>` to the chat detailing exactly which assets are missing and their technical placeholder mapping, ensuring the next phases know exactly what API to call.
