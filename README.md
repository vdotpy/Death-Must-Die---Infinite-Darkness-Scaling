# Infinite Darkness — Star Crux Scaling Uncapped

Break all boundaries and create the ultimate hard-mode sandbox in **Death Must Die**. This mod completely removes the rigid caps on the vanilla difficulty tracking framework, giving you absolute freedom to scale the game's mechanics into total chaos.

I have certain changes planned to hopefully clean up the UI and fix some overlapping. Feel free to let me know of anything you would like to see added to this mod, or share an idea for another mod you'd like to see.

---

## 📑 Features

* **Absolute Limit Removal:** Completely deletes the hardcoded level limits on every single vanilla Star Crux difficulty modifier row.
* **Dynamic Numerical Display:** Replaces the static, uninformative "Max" text label with your true, exact numerical level rank (e.g., `Lv. 12`, `Lv. 50`, `Lv. 200`) right alongside the base points per tier.
* **Unbounded Darkness Point Scoring:** Rewrites the internal calculation scoring mechanics to mathematically track and register your cumulative total difficulty points infinitely past the vanilla 100 limit.
* **True Engine Scaling:** Seamlessly passes your chosen point allocations straight into the live monster spawning logic. If you set enemy health or damage to +10,000%, the physics and combat loops natively enforce it in-game.

---

## 🛠️ Installation Instructions

1. Ensure you have the latest version of **BepInEx 5** installed for *Death Must Die*.
2. Download and open the `InfiniteDarkness.dll` file from releases.
3. Navigate to your main game directory and open the BepInEx Plugins folder. ie ".../Death Must Die/BepInEx/plugins/"
4. Drag and drop the main mod file "InfiniteDarkness.dll" into that plugins folder.      
5. Run the game, open the star crux menu, and ensure you are able to assign points beyond the default value.

---

## ⚠️ Performance & Stability Warning

Because this mod completely strips away the game developers' built-in safety rails to give you absolute sandbox freedom, pushing modifiers to extreme, absurd heights can cause serious performance issues.

* **Lag & Frame Drops:** Cranking attributes like enemy movement ( and likely many others) too high forces the Unity physics engine and pathfinding matrix to run calculations exponentially faster than normal frames can process. This can cause severe lag or sudden frame-rate drops.
* **Game Crashes:** Forcing extreme stat thresholds (such as health pools, speed rates, or multiplier percentages reaching deep into the tens of thousands) may possibly cause the underlying engine data variables to hit a numeric limit overflow or trigger animation exceptions. This will result in a hard freeze or immediate crash to the desktop.

**Recommendation:** I mostly use this to increase the health pools of enemies, as I find darkness 100 far too easy with my optimized builds. I wanted to give users complete freedom over difficulty values though, so experiment and have fun.
