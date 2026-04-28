# Phase 3: Automated MCP Execution & Reporting

**Trigger:** Called by `game-prototype-orchestrator` after all task files have been successfully generated.
**Inputs:** Task `.md` files in `Assets/Projects/[ProjectName]/.agents/tasks/`.
**Outputs:** Unity modifications via MCP, and final `prototype_summary.md`.

## Execution Steps:

### Execution (Try-Catch Protocol)
1. Iterate over the task files within `Assets/Projects/[ProjectName]/.agents/tasks/` in numerical order.
2. Parse the `.md` instructions. You MUST strictly adhere to the Task Definition blocks.
3. Fire the MCP commands. 
4. **Automated Error Handling (CRITICAL)**: After each MCP tool call, read the return payload. 
   - If SUCCESS: Proceed to step 5.
   - If ERROR: Attempt to diagnose and re-run (max 2 retry attempts). If it still fails, **ABORT PIPELINE IMMEDIATELY**.

### Verification Output requirement (LEARNED FROM CLAUDE)
5. Before moving to the next task inside the `.md` file, you MUST generate an internal acceptance payload mirroring this template to ensure nothing was hallucinated:
```markdown
# [TASK_ID]
## File đã tạo/sửa
## Object đã tạo trong scene
## Serialized fields cần set
## Trạng thái (Done/Blocked)
```

### Completion
6. If the pipeline successfully executes all tasks, or if it aborts, conclude execution by generating a final `Assets/.agents/prototype_summary.md`.
7. Inform the user in the chat about the final status.
