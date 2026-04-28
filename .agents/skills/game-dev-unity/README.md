# 🎮 Skill Framework: Game Dev Unity (Master Router)

Welcome to the Master Skill directory dedicated to Unity game development. This system is designed as a **Token-Optimized Router** to solve Token limits (Context Window) bloat when loading too many rules into the AI's memory at once.

## 🏗 System Architecture

This directory operates on a **Master ➠ Sub-files** model.

Instead of scattering Unity rules across dozens of independent folders (which confuses the AI and causes misses), everything is consolidated here.

1. **`SKILL.md` (Master Router)**
   - The only file containing the YAML Metadata trigger for the system.
   - When activated, it forces the AI to process a routing flowchart (Mermaid diagram).
   - This diagram strictly dictates EXACTLY ONE specialized sub-file the AI is allowed to read for the ongoing Task type.

2. **The Trimmed Sub-files (in `sub-skill/`)**
   - `unity-safe-scripting.md`: Prevents memory leaks, GC allocations, handles C# Events.
   - `unity-architecture-patterns.md`: Decoupling (Model-View), anti-Singleton rules, Data-Driven logic via ScriptableObjects.
   - `unity-ui-optimization.md`: Anti Canvas-Rebuild, Raycast handling, Event Cameras.
   - `unity-physics.md`: FixedUpdate logic, Raycast NonAlloc, MovePosition vs Transform.
   - `unity-rendering.md`: URP/HDRP hygiene, Shader Graph priority, batching constraints, and LOD setup.
   - `unity-assets.md`: Strictly mandates Addressables over Resources, and sets Texture/Audio compression rules.
   - `unity-networking.md`: Server Authority enforcement (NGO), bandwidth optimization, and client prediction.
   - `unity-profiling.md`: Enforces mandatory Memory Profiler / Frame Debugger usage and asserts target FPS caps for mobile/console.
   - `unity-audio.md`: Dictates AudioMixer routing arrays and strictly defines AudioSource object-pooling rules.
   - **Token Optimization:** All these files have been stripped of redundant metadata (YAML blocks, lengthy Overviews) to free up 100% of the Context Window for actionable rules (Core Pattern).

## 🛡 Supreme Rule (DevArchitecture)

Regardless of which sub-file the AI picks, `SKILL.md` enforces a strict overarching rule: **Every code or architecture output must 100% mirror the global DevArchitechture manifesto.**
- Overengineering any solution violates **Simplicity First**.
- Touching unrelated code violates **Surgical Changes**.

## 🚀 Usage Guide

- **For the User:** You don't need to do anything special. Just write prompt (e.g., *"Code a shooting gun"*, *"Fix FPS drops in the main menu"*, *"Setup multiplayer"*). The system's Semantic Search will catch this `SKILL.md`, and the AI will automatically handle the internal routing.
- **For the AI/Agent:** You must cross-reference the User's request using the Router Mermaid flowchart. Only load the `.md` file specified by the flowchart. Never load all 9 sub-files simultaneously.

## 🛠 Maintenance and Expansion

If a Developer wishes to add a new Unity branch (E.g., `unity-xr.md`):
1. Create a `.md` file inside the `sub-skill/` directory.
2. DO NOT add `---` YAML Metadata headers. Jump straight into the `## Core Pattern`.
3. Open `SKILL.md`, go to the `mermaid` codeblock, and add a new arrow `-->` pointing to the new `.md` file. Done!
