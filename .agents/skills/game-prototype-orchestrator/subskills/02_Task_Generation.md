# Phase 2: Ahead-of-Time Task Generation

**Trigger:** Called by `game-prototype-orchestrator` after Ingestion & Verification.
**Inputs:** Verified GDD and Asset Mappings.
**Outputs:** Task pack `.md` files in `Assets/Projects/[ProjectName]/.agents/tasks/`.

## Execution Steps:
1. Act as a Principal AI Tech Lead. Your job is to convert the GDD into a flawless Step-by-Step AI Production Guide, identical to the standard seen in `Flappy_Trippy_Unity_AI_Step_By_Step.md`.
2. Do NOT lump everything into 3 random files. You MUST group tasks into logical "Task Packs" (e.g., `Pack_A_Setup.md`, `Pack_B_DataLayer.md`, `Pack_C_GameplayCore.md`), saved in `Assets/Projects/[ProjectName]/.agents/tasks/`.

## 🧠 Architectural Rules for Task Generation (LEARNED FROM CLAUDE)
When designing the tasks, you MUST embed these rules:
1. **Folder Standards:** Start by generating a `SETUP-01` task that creates a strict `Assets/_Project/` hierarchy (`Scripts/`, `Prefabs/`, `ScriptableObjects/`, `Scenes/`). 
2. **Data-Driven Logic:** Never hardcode stats in Monobehaviours. You MUST generate tasks to create `ScriptableObjects` (e.g., `PlayerConfigSO`, `WeaponConfigSO`) *before* any runtime object is built.
3. **Pool Over Instantiate:** Enforce Object Pooling for all projectiles and repeated elements.
4. **No FindObjectOfType:** All systems must be explicitly hooked via Inspector references or a Service Locator (`Bootstrapper.cs`).

## 🧱 The AI Execution Template (CRITICAL)
Every single task defined inside your "Task Packs" MUST strictly follow this exact template block. Do not deviate.

```markdown
### Task [TASK_ID] - [Task Name]
**Mục tiêu:** [Objective]
**Người/AI nhận task:** [Executor]
**Đầu vào:** [Required conditions]

**Các bước thao tác trong Editor/Code:**
1. [Create Script X]
2. [Create GameObject Y]
3. [Compile]

**GameObject / Prefab cần tạo:**
- [Exact Name] -> [Absolute Hierarchy Path, e.g., Run/Gameplay/PlayerRoot]

**Script cần gắn:**
- [Component Name]

**Cách hook reference trên Inspector:**
- [Variable] = [Value or Object]
```

## 📋 The Acceptance Output (Bắt buộc)
Instruct the executor (Phase 3) that after finishing a task, they MUST output a checklist matching the exact format from Claude's Section 27:

```markdown
# [TASK_ID]
## File đã tạo/sửa
- [Path to .cs]
## Object đã tạo trong scene
- [Absolute Path]
## Serialized fields cần set
- [Mappings]
## Test thủ công
- [Action -> Expected Result]
## Trạng thái
- Done / Blocked
```
