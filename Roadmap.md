# 🗺️ Peach Puller – 2 Week Development Roadmap

**Total Time**: 40 hours  
**Schedule**: 8 sessions × 5 hours  
**Goal**: Fully playable prototype with core mechanics, 5–10 levels, and polished controls

---

## 🗓️ Week 1 — Core Systems & Mechanics

---

### ✅ Session 1 – Core Grid & Chameleon Physics (5 hrs)

**Goals**:
- Build foundational grid system
- Implement 1x3 chameleon movement & gravity

**Tasks**:
- Set up 2D grid and tile map
- Create chameleon entity (1x3 horizontal)
- Implement left/right movement and facing
- Add gravity: fall if no supporting tiles under any of the 3 body segments
- Hardcode basic level for testing

---

### ✅ Session 2 – Tongue Mechanics (5 hrs)

**Goals**:
- Implement tongue extension and peach pulling

**Tasks**:
- Extend tongue up to 6 tiles in facing direction
- Detect peach in line and latch
- Pull peach one tile per tap
- Prevent pulling through walls, other peaches, or chameleon
- Cancel tongue action when movement occurs

---

### ✅ Session 3 – Peach Entity & Interaction Rules (5 hrs)

**Goals**:
- Add peach behavior and interaction with the grid

**Tasks**:
- Place peaches on grid
- Set peaches to become solid walkable tiles after being pulled
- Prevent pulling peaches into invalid positions
- Add peach-eating detection
- Visual feedback for peach state (static vs moved)

---

### ✅ Session 4 – UI & Input Polish (5 hrs)

**Goals**:
- Finalize controls and on-screen UI

**Tasks**:
- On-screen D-Pad for movement (right side)
- Tongue action button (left side)
- Input feedback (button highlights, animation states)
- Add restart/reset level button
- Animate tongue flick and chameleon visuals

---

### 🎯 End of Week 1 Deliverable

- Fully playable core mechanic demo
- Grid system, gravity, tongue, and pulling implemented
- 1 functional test level with full logic
- Basic UI and input responsiveness

---

## 🗓️ Week 2 — Level Design, Polish, and Finalization

---

### ✅ Session 5 – Level System & Win/Loss Conditions (5 hrs)

**Goals**:
- Add progression, level win/fail logic

**Tasks**:
- Implement win condition: all peaches eaten
- Add fall/loss detection (no support)
- Reset/restart level functionality
- Level transition system (hardcoded or list-based)
- Optional: basic level select screen

---

### ✅ Session 6 – Build 5–10 Puzzle Levels (5 hrs)

**Goals**:
- Design and implement full puzzle levels

**Tasks**:
- Create 5–10 handcrafted levels
- Vary complexity:
  - Simple pull-to-cross
  - Peach chains
  - Gravity reliance
  - Movement traps
- Test and tweak for balance
- Add level skip (if stuck) and retry buttons

---

### ✅ Session 7 – Polish & Feedback (5 hrs)

**Goals**:
- Add visual/audio polish and prepare for feedback

**Tasks**:
- Add sound effects: tongue flick, peach thud, steps
- Add screen shake or visual feedback for actions
- Improve sprites, background, and tile aesthetics
- Add level intro hints or tutorial prompts

---

### ✅ Session 8 – Finalize Build & Export (5 hrs)

**Goals**:
- Final packaging and delivery

**Tasks**:
- Export WebGL/Android/Desktop build
- Add title screen and credits
- Add instructions or control hints
- Get feedback from playtesters
- Optional: upload to Itch.io or share privately

---

### 🎯 End of Week 2 Deliverable

- Fully playable prototype
- 5–10 complete puzzle levels
- Polished UI and visual/audio feedback
- Ready for playtesting or public release

---

## 🧪 Optional Stretch Features (If Time Permits)

- Undo button
- Peach variants (fragile, sticky, bounce)
- Time challenge per level
- Hats/colors for chameleon
- Leaderboard or local progress tracking