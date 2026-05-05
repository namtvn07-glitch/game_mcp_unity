---
description: Code review before commit - check patterns and conventions
---

# Code Review Workflow

> Review code against project conventions before committing.
> Ensure compliance with patterns in GEMINI.md and learned docs.

## Trigger
User calls:
- `/review` → Review changed files (git diff)
- `/review [file_path]` → Review specific file
- `/review [module]` → Review entire module

## Step 1: Identify Scope
// turbo

### 1.1 If no argument:
```bash
# Get list of changed files
git diff --name-only HEAD
git diff --name-only --staged

# IMPORTANT: Also check untracked files (new files)
git status --short
# Files starting with ?? are untracked (newly created)
# Files starting with M are modified
# Files starting with A are staged
```

### 1.2 If file path provided:
Review that specific file

### 1.3 If module name provided:
Review by module path (see `.agents/rules/GEMINI.md` "Ad Network Modules")

## Step 2: Load Review Checklist
// turbo
Load rules from:
1. `.agents/rules/GEMINI.md` → Code Patterns, Critical Rules
2. `docs/reference/dic-master.md` (if .dic files in scope)
3. `docs/learned/` (relevant stack file)

## Step 3: Review Code
// turbo

### 3.1 Database/SQL Review:
| Check | Rule |
|-------|------|
| Upsert | MERGE, not DELETE+INSERT |
| Columns | Explicit column names, no SELECT * |
| Parameters | `?` placeholders, no string concat |
| Connection | try-with-resources |
| NULL | NVL() for nullable |
| Date | SQL: `DD/MM/YYYY HH24:MI:SS` |

### 3.2 Java Review:
| Check | Rule |
|-------|------|
| Comments | NO comments (except complex SQL) |
| Imports | No unused imports |
| Exceptions | Handle appropriately |
| Resources | Close properly |
| Naming | Follow project convention |

### 3.3 UI (.dic) & Report (.dic) Review:
| Check | Rule |
|-------|------|
| Indent | Tabs (not spaces) |
| SQL | `?` placeholders |
| VN.dic | Corresponding file exists, Dictionary for new fields |
| Validation | Mandatory fields validated |
| Report | DateRange validation, Limit clause, Measures Format |

### 3.4 Security Review:
- No hardcoded credentials/tokens
- No sensitive data in logs
- `?` placeholders for SQL
- Validate user input

### 3.5 System Impact Analysis:
// turbo
```bash
grep -rl "ChangedFileName" source/ads-lib/
```
| Check | Action |
|-------|--------|
| **Shared utilities** | Grep callers — other modules using it? |
| **Database schema** | Affects views, stored procedures? |
| **API contracts** | JSP response changed → which frontend consumes it? |
| **Shared includes** | `common.jsp`, `model_common.jsp` → other reports affected? |
| **Thread/SyncGroup** | Config needs update? |

> [!CAUTION]
> Modifying shared utility (commons/, HttpUtil, DBAction) → MUST review ALL callers.

### 3.6 Duplicate Code Check:
// turbo
```bash
grep -rn "unique_snippet_from_changed_code" source/ads-lib/
```
- Same code in 2+ places? → Extract shared method/utility
- Copy-paste with only variable name changes? → Create shared function with parameters
- Duplicate SQL/JS/DIC? → Move to common file

> [!WARNING]
> Duplicate found → suggest refactor before approving.

### 3.7 Skill-Based Review Lenses (apply when relevant)

#### API Integration Lens (if files touch Service layer + external API)
> **Reference**: `api-patterns` skill
- [ ] Response format consistent with other integrations?
- [ ] Error responses don't expose internal details?
- [ ] Rate limiting respected?
- [ ] Pagination follows established pattern?

#### Error Handling Lens (if files have try-catch blocks)
> **Reference**: `error-handling-patterns` skill
- [ ] Specific exception types caught (not generic Exception)?
- [ ] Retry logic on transient failures?
- [ ] Error context logged (operation + params + message)?
- [ ] Silent catch blocks? (catch + ignore = 🔴 Critical)

#### Concurrency Lens (if files touch GatherThread or multi-threaded code)
- [ ] Thread-safe data structures used?
- [ ] Connection properly closed in finally/try-with-resources?
- [ ] Shared state properly synchronized?

### 3.8 Compile Gate (if Java files in scope)
// turbo
```bash
cd source/ads-lib && ./gradlew compileJava
```
If compile fails → add to report as 🔴 Critical issue.

## Step 4: Generate Report
Output format:
```markdown
# 📋 Code Review Report

## Summary
- **Files reviewed**: X
- **Issues found**: Y (Z critical)
- **Status**: ✅ Ready / ⚠️ Needs fixes / ❌ Major issues

---

## Issues Found

### 🔴 Critical
1. **[file.java:123]**: [description]
   - **Rule violated**: [rule from GEMINI.md]
   - **Suggested fix**: [how to fix]

### 🟡 Warning
1. **[file.dic:45]**: [description]
   - **Suggestion**: [improvement]

### 🟢 Info/Style
1. **[file.java:78]**: [minor note]

---

## Approved ✅
- [file1.java] - No issues
- [file2.dic] - No issues

---

## Checklist Summary
| Category | Status |
|----------|--------|
| SQL Patterns | ✅/❌ |
| Java Patterns | ✅/❌ |
| UI Patterns | ✅/❌ |
| Security | ✅/❌ |
| Impact | ✅/❌ |
| Duplicates | ✅/❌ |
```

## Step 5: Offer Fix
If issues were found:
```markdown
> Would you like me to auto-fix the issues above?
> - `yes` - Fix all
> - `yes critical` - Fix critical only
> - `no` - No thanks, I'll fix manually
```

---

## Quick Commands

| Command | Action |
|---------|--------|
| `/review` | Review uncommitted changes |
| `/review --staged` | Review staged changes only |
| `/review path/to/file.java` | Review specific file |
| `/review admob` | Review admob module |

---

## Common Findings & Auto-fixes

| Issue | Auto-fix |
|-------|----------|
| Missing VN.dic entry | Add Dictionary line |
| Spaces → tabs | Convert whitespace |
| Missing NVL | Wrap with NVL(x, 0) |
| Unused import | Remove import line |
