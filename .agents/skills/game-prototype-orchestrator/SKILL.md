---
name: game-prototype-orchestrator
description: Automates the creation of Unity prototypes by orchestrating GDD analysis, granular task generation, and sequential execution via MCP.
---

# Game Prototype Orchestrator

This skill is the master router for automatically converting game documentation (GDD and Asset JSONs) into playable Unity prototypes using MCP. It orchestrates three specialized sub-skills in sequence.

## Execution Flow

Whenever the user asks you to "create prototype", "build this in unity", or "start prototype phase", you MUST load and execute the following sub-skills in exact order:

### 1. Ingestion & Verification
First, use your file viewing tools to read and follow the instructions in:
**`subskills/01_Ingestion_Verification.md`**

*(Wait to ensure all assets are accounted for and Placeholders are mapped before proceeding)*

### 2. Ahead-of-Time Task Generation (CRITICAL)
Next, use your file viewing tools to read and follow the instructions in:
**`subskills/02_Task_Generation.md`**

*(Wait to ensure all `.md` task files are completely written to `Assets/Projects/[ProjectName]/.agents/tasks/` before proceeding)*

### 3. Automated MCP Execution & Reporting
Finally, use your file viewing tools to read and follow the instructions in:
**`subskills/03_MCP_Execution.md`**

Make sure to strictly adhere to the "Fire-and-forget" philosophy—do not halt for human review between task files during step 3. Once complete, inform the user the prototype is ready for manual inspection.
