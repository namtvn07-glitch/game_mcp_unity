---
description: Debug a bug using disciplined methodology
---

# Debug Workflow

> Debug bugs using skill-based methodology. Routes to the right debugging approach.
> Flow: `/debug [description]` → fix → `/review` or `/finish`

## Trigger
User calls:
- `/debug [description]` → Start debugging

## Step 1: Assess Bug Type

Classify the bug based on user description and initial evidence:

| Type | Characteristics | Route To |
|------|----------------|----------|
| **Hard bug** | Intermittent, regression, multi-threaded, no clear stack trace | `phase-gated-debugging` skill |
| **Simple bug** | Clear error message, obvious location, reproducible | `bug-hunter` skill |
| **Unknown** | Not enough info yet | Start with `bug-hunter`, escalate to `phase-gated-debugging` if stuck after 2 fix attempts |

### Decision Checklist:
- [ ] Can you reproduce it consistently? (No → hard bug)
- [ ] Is the stack trace pointing to a clear location? (No → hard bug)
- [ ] Does it involve multi-threaded code / GatherThread? (Yes → hard bug)
- [ ] Is it a regression (worked before, broken now)? (Yes → hard bug, use `git bisect`)

> [!CAUTION]
> **If classified as hard bug**: You MUST follow `phase-gated-debugging` strictly.
> NO code edits until root cause is confirmed with user in Phase 3.

## Step 1.5: Check Known Gotchas
// turbo
Before debugging, check known issues:
1. Read `docs/learned/` files — search for symptoms matching the bug description
2. If match found → apply known fix first, verify
3. If no match → continue to Step 2

> [!TIP]
> Many bugs have been encountered and documented before. Checking learned docs first saves time.

## Step 2: Execute Skill Protocol

### Route A: `phase-gated-debugging` (Hard bugs)
Follow the 5-phase protocol exactly:
1. **REPRODUCE** — Run failing test/path 2-3 times. No code reading.
2. **ISOLATE** — Read code + add `// DEBUG` logging only. Re-run.
3. **ROOT CAUSE** — Analyze with "5 Whys". Present to user. **WAIT for confirmation.**
4. **FIX** — Minimal change addressing confirmed root cause only.
5. **VERIFY** — Re-run original test. For intermittent bugs, run 5+ times.

### Route B: `bug-hunter` (Simple bugs)
Follow the evidence trail:
1. **Reproduce** — Get exact steps.
2. **Gather Evidence** — Logs, stack trace, error messages.
3. **Hypothesis** — State what you think is wrong.
4. **Test** — Add logging to prove/disprove.
5. **Fix root cause** — Not the symptom.
6. **Verify** — Run tests.

### Escalation Rule:
If `bug-hunter` fails after 2 fix attempts → switch to `phase-gated-debugging`.
State: "Simple approach isn't working. Switching to disciplined phase-gated debugging."

## Step 3: After Fix

### 3.1 Run Tests
// turbo
```bash
cd source/ads-lib && ./gradlew compileJava
```

### 3.2 Document (if non-obvious root cause)
Add to relevant `docs/learned/` file:
```markdown
### [Module] [Short symptom description]
- **Symptom**: [what user saw]
- **Root cause**: [actual problem]
- **Fix**: [what was changed]
- **Prevention**: [how to avoid in future]
```

### 3.3 Offer Next Steps
```markdown
✅ Bug fixed and verified.

Next:
- `/review` — Review the fix before committing
- `/finish` — Extract learnings and commit
- Or continue working
```

---

## XReport-Specific Bug Patterns

| Pattern | Common In | Key Technique |
|---------|-----------|---------------|
| GatherThread sync failure | Meta/TikTok threads | Check token validity → API response → DB write |
| Oracle deadlock | Concurrent MERGE operations | Check lock table, reduce batch size |
| Silent data loss | Catch blocks swallowing exceptions | Search for empty catch blocks |
| ClickHouse query timeout | Large date ranges, missing indexes | Check ORDER BY key alignment, add WHERE filters |
| DIC rendering error | Missing VN.dic, tab/space mix | Validate with `docs/reference/dic-master.md` |
| JSP model mismatch | `functionName` ≠ `ModuleName` | Cross-check .dic and .jsp names |
| Status stuck at PENDING | Service UPDATE not firing | Trace PENDING → PROCESSING transition |

---

## Integration Points

- **Before**: Bug is reported or discovered during `/execute`
- **After**: `/review` or `/finish` to close the loop
- **Parallel**: Can be used standalone, doesn't require `/plan`

---

*Uses `phase-gated-debugging` and `bug-hunter` skills.*
