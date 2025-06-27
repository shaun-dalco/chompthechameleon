# 🦎 Chomp The Chameleon — Game Design Specification

## 🎯 Game Goal
Control a 1x3-sized chameleon and use your tongue to pull peaches closer. Your objective is to **eat all the peaches** in each level by reaching them and pulling them into your mouth.

---

## 🎮 Core Gameplay Loop
1. Move the chameleon across a single-screen grid level.
2. Use your tongue to target and pull peaches.
3. Strategically place peaches to create walkable paths.
4. Use gravity and physics to your advantage.
5. Eat all peaches to win the level.

---

## 🕹 Control Scheme

### ▶ Right Side (Movement)
- **D-Pad** (Up / Down / Left / Right):
  - Moves the chameleon one tile in the given direction.
  - **Down** does nothing (gravity is automatic).
  - Any movement **cancels the tongue action**, but pulled peaches remain in place.

### ◀ Left Side (Tongue)
- **Single Button**:
  - **First tap**: Extend tongue in the direction the chameleon is facing (up to 6 tiles).
  - **Next taps**: Pull the peach one tile toward the chameleon per tap.
  - If a peach is blocked, it does not move.

---

## 🦎 Chameleon Properties
- Size: **1x3 horizontal**.
- Can move left or right, and rotate facing direction.
- Subject to **gravity**:
  - If none of the 3 body tiles are supported from below, the chameleon **falls**.
  - Standing on ground or pulled peaches counts as support.
  - **Partial support (even 1 tile)** prevents falling.

---

## 🍑 Peach Mechanics
- Peaches are initially unreachable.
- Can be pulled toward the chameleon using the tongue.
- Become **solid tiles** once moved (can be walked or stood on).
- Cannot be pulled through:
  - Walls
  - Other peaches
  - The chameleon

---

## 📏 World Rules
- Grid-based, single-screen levels.
- Peaches, pits, and walls occupy grid tiles.
- All actions (movement, pulling) are discrete and turn-based.

---

## 💥 Fail States
- Chameleon falls off the level (no support).
- Chameleon becomes **softlocked** (unable to reach or pull remaining peaches).

---

## ✅ Win Condition
- **Eat all peaches** by pulling them fully into the chameleon's tile.

---

## 🧠 Level Design Progression
- **Tutorial Levels**:
  - Teach tongue extension and pull.
  - Demonstrate gravity and partial support.
- **Midgame Levels**:
  - Chain pulling
  - Precise ordering
  - Gravity traps
- **Advanced Levels**:
  - Fakeouts
  - Tight platforming
  - Requiring partial support awareness

---

## 📋 Prototype Development Tasks

| Task                 | Description                                                      |
|----------------------|------------------------------------------------------------------|
| 🧩 Grid System        | 2D grid of tiles (empty, wall, peach, chameleon)                |
| 🦎 Chameleon Entity   | 1x3 player logic, movement, and gravity                         |
| 🍑 Peach Logic        | Pullable object, becomes solid when moved                       |
| 🎯 Tongue Mechanic    | Extend up to 6 tiles, latch and pull peach 1 tile per tap       |
| 🎮 Input System       | D-Pad for movement, single tongue button                        |
| 🌍 Level Data         | Hardcoded test levels (walls, pits, peaches)                    |

---

## ✨ Optional Future Features
- Undo button
- Peaches with special properties (bouncy, sticky, fragile)
- Level select screen with star ratings
- Chameleon customization (colors, hats)
- Sound effects and animations (tongue flick, squish, bounce)

