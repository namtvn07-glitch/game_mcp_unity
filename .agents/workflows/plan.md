---
description: Create an implementation plan for a new feature
---

# Plan Workflow

> Create an implementation plan for a new feature.
> Flow: `/plan` → review → `/execute` → `/finish`

## Step 1: Understand the Feature Request
Ask clarifying questions if needed:
- What is the main objective?
- Which module(s) are affected? (AdMob, Meta, TikTok, Adjust, UI/DIC, ClickHouse)
- Any reference files or existing patterns to follow?

## Step 2: Collect SMART POLE Context Atoms
> **Reference**: Read the `smart-pole-context-analyzer` skill (global) before proceeding.

Before research, scan for **SP-Flaws** (missing atoms):

### 🔴 CORE (Mandatory - Ask user if missing!)
- [ ] **Aim (A)**: Specific objective? Success criteria?
- [ ] **Outline (O)**: Scope (include/exclude)? Desired structure?

### 🟡 CONTEXTUALIZER (Auto-fill from codebase research)
- [ ] **Locale (L)**: Which platform? Which tech stack?
- [ ] **Resource (R)**: Tools available? Constraints?
- [ ] **Mastery (M)**: Does user need detailed or high-level explanation?

### 🟢 ACCELERATOR (Optional - enhance quality)
- [ ] **Example (E)**: Reference implementation?
- [ ] **Style (S)**: Desired output format?
- [ ] **Time (T)**: Deadline? Duration?

> [!CAUTION]
> **If Aim or Outline is missing, MUST ask user before continuing!**
> Do not assume scope — this leads to rework.

## Step 3: Design Exploration (If Needed)
> **Trigger**: If the feature is ambiguous, has multiple viable approaches,
> or involves architecture decisions — invoke the `brainstorming` skill FIRST.

Checklist - skip to Step 4 if ALL are "No":
- [ ] Is the scope ambiguous? (user said "make X better" without specifics)
- [ ] Are there 2+ valid approaches? (new table vs. alter existing)
- [ ] Is this a new capability with no existing pattern?

If ANY is "Yes" → activate `brainstorming` skill → get validated design → THEN continue to Step 4.

## Step 4: Research Codebase
// turbo
Read relevant context files (SSOT):
- DIC syntax: `docs/reference/dic-master.md`
- Project overview: `docs/reference/project-overview.md`
- ClickHouse: `docs/reference/clickhouse-optimization/SKILL.md`
- Frontend/UI: `docs/reference/frontend-design/SKILL.md`
- Stack-specific: `docs/learned/` (relevant file)

Search codebase for similar implementations:

| Type | Location |
|------|----------|
| Java | `source/ads-lib/src/com/ogs/ads/` |
| Reports | `source/ads-lib/run/web/com/ogs/ads/report/` |
| CRUD | `source/ads-lib/run/web/com/ogs/ads/system/` |

## Step 5: Create Task & Plan Artifacts

Create 2 artifacts (auto-saved to `brain/<conversation-id>/`):

### 5.1 Create `task.md` (checklist tracking):
```markdown
# Task: [Feature Name]
> Created: [YYYY-MM-DD]

## Checklist (Remove inapplicable items)
- [ ] Research codebase
- [ ] Plan approved
- [ ] Database changes
- [ ] Java layer changes
- [ ] UI layer changes (.dic)
- [ ] Tests passed
- [ ] Learnings extracted
```

### 5.2 Create `implementation_plan.md` (detailed plan):
```markdown
# [Feature Name]

Brief description of the feature and its objective.

## User Review Required
> [!IMPORTANT]
> [Key decisions requiring user approval]

## Proposed Changes

### Database Schema
#### [NEW/MODIFY] table_name
- Description

### Java Layer
#### [MODIFY] ClassName.java
- Description

### UI Layer
#### [MODIFY] module/file.dic
- Description

### API Integration (if applicable)
> **Reference**: `api-patterns` skill — read before designing any new API integration.
> Decision checklist:
> - [ ] Consistent response format with existing integrations?
> - [ ] Rate limiting strategy defined?
> - [ ] Error response format standardized?
> - [ ] Pagination approach matches existing pattern?

## Verification Plan
### Automated Tests
- `cd source/ads-lib && ./gradlew compileJava`

### Manual Verification
- Step 1: ...
- Step 2: ...
```

## Step 6: Self-Review Plan ⚠️ NEVER TRUST YOUR FIRST PLAN!
Before presenting to user, MUST verify:

### Checklist:
- [ ] **Completeness**: Have I covered all affected files?
- [ ] **Consistency**: Do patterns match existing codebase conventions?
- [ ] **Dependencies**: Did I miss any dependencies or imports?
- [ ] **Edge Cases**: What could go wrong? How to handle errors?
- [ ] **Existing Code**: Did I check for similar existing implementations to reuse?
- [ ] **DB Updates**: Are MERGE statements used for upserts? Unique indexes defined?
- [ ] **Status Flow**: Does it follow PENDING → PROCESSING → SUCCESS/FAILED pattern?

### System Impact Analysis:
- [ ] Shared utilities — other modules using this function/class?
- [ ] Database schema — affects views, stored procedures?
- [ ] API contracts — JSP endpoint changed → which frontend consumes it?
- [ ] Shared includes — `common.jsp`, `model_common.jsp` → other reports affected?

### Reusable Code Check:
```bash
grep -rn "similar_pattern" source/ads-lib/src/ source/ads-lib/run/web/
```
- [ ] Any existing utility/common method/JS function to reuse instead of writing new?

### State Machine Verification:
If the feature has state transitions (thread processing, UI flows, API pipelines), **create a Mermaid state diagram** in the task file.
Forces mapping all flows → catches logic gaps before coding.

### Final Check:
1. Re-read plan as if you're a skeptical reviewer
2. Search for at least ONE more related file you might have missed
3. Double-check all file paths exist
4. Verify SQL follows Oracle conventions (if applicable)

> [!CAUTION]
> If plan feels "too simple", you probably missed something. Dig deeper!

## Step 7: Request User Review
Present `implementation_plan.md` to user:
- Summarize the plan and key decisions
- Ask user to approve or request changes
- **STOP. Wait for user approval before continuing.**

## Step 8: After Approval
User can:
- Call `/execute` to implement the plan
- Or call `/execute` right after `/plan` (implicit approval)
