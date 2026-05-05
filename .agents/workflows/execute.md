---
description: Execute the implementation plan
---

# Execute Workflow

> Execute the approved implementation plan.
> Flow: `/plan` → review → `/execute` → `/finish`

## Trigger
User calls:
- `/execute` → Auto-detect task from conversation and execute
- `/execute [task_name]` → Execute specific task
- `/execute --step [N]` → Execute only step N of the plan

## Step 1: Locate Task Context
Find task context from the current conversation:

### 1.1 If user did NOT provide task_name:
1. Find `task.md` and `implementation_plan.md` created in this conversation
2. Check conversation content to identify the feature
3. Ask for confirmation if uncertain

### 1.2 If user DID provide task_name:
Find task context in the current conversation's `implementation_plan.md`

## Step 2: Validate Task State
Verify the task is ready to execute:

```markdown
✅ Conditions to proceed:
- [ ] Task file exists
- [ ] implementation_plan.md has "Proposed Changes" section
- [ ] User has approved plan (or implicit approval via /execute)

❌ Block if:
- Task already completed
- Plan has not been created
```

> [!TIP]
> If user calls `/execute` right after `/plan`, treat as implicit approval.

## Step 3: Execute Implementation

### 3.1 Read plan from "Proposed Changes" in implementation_plan.md:
// turbo
- Database Schema changes
- Java Layer changes
- UI Layer (.dic files) changes

### 3.2 Execute in order:
// turbo
```
1. Database changes (if any)
   ↓
2. Java Layer (models → repos → services → gather threads)
   ↓
3. UI Layer (main .dic FIRST, VN.dic AFTER)
   ↓
4. Compilation check
```

### 3.3 Critical Reminders:
- **DIC files**: Tabs (not spaces), `?` placeholders for SQL
- **VN.dic**: MUST update when adding new fields
- **Java**: NO comments, try-with-resources for Connection
- **SQL**: MERGE for upserts, NVL() for NULL handling

### 3.4 Checkpoint after each layer (MANDATORY):
// turbo
After completing each layer, **force-save `task.md`** before continuing:

```
✅ DB done → save task.md (mark DB [x]) → continue Java
✅ Java done → save task.md (mark Java [x]) → continue UI
✅ UI done → save task.md (mark UI [x]) → continue Tests
```

> [!IMPORTANT]
> If interrupted mid-execution, the next `/execute` will read `task.md`,
> see which layers are already `[x]` → skip them, only execute remaining `[ ]` layers.

### 3.5 Error Handling Standard (Mandatory for Service layer)
> **Reference**: `error-handling-patterns` skill

When writing Service-layer code, apply:
- [ ] **Try-catch granularity**: Catch specific exceptions, not generic `Exception`
- [ ] **Retry on transient**: Ad network API calls → use retry with exponential backoff
- [ ] **Circuit breaker**: If integration fails N times consecutively → skip and log, don't crash thread
- [ ] **Error context**: Every catch block must log: operation name, input params, error message

## Step 4: Run Tests
// turbo
```bash
cd source/ads-lib && ./gradlew compileJava
```

If tests fail:
Refer to the Error Handling section below.

## Step 5: Update task.md
Update `task.md` in brain/:
- Mark completed checklist items as `[x]`
- Record files changed and test results
- Record results in implementation_plan.md verification section

## Step 6: Report to User
```markdown
✅ **Executed**: [task_name]

### Changes Made:
| File | Action |
|------|--------|
| file1.java | MODIFY |
| file2.dic | NEW |

### Test Results:
- Compile: ✅
- Unit Tests: ✅ (X passed)

### Next Steps:
1. Manual verification (if needed)
2. Call `/finish` when verification is done
```

---

## Quick Commands

| Command | Action |
|---------|--------|
| `/execute` | Execute current task |
| `/execute task_name` | Execute specific task |
| `/execute --step 1` | Execute only step 1 |
| `/execute --dry-run` | Show what would be done without executing |

---

## Error Handling

| Error | Action |
|-------|--------|
| Task file not found | Ask user to create plan first |
| Build failed | Show error, try 3 times, ask user |
| DIC syntax error | Check tabs/spaces, verify tag closing |

---

## Integration Points

- **Before**: `/plan` creates the task file
- **After**: `/finish` extracts learnings and archives
- **Alternative**: `/debug` for quick fixes without full plan

---

*Part of the Plan → Execute → Finish workflow.*
