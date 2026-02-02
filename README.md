# 🏪 Shop System (Test Task) - Unity Developer

### Overview

This project was implemented as a **technical test task** to demonstrate architectural design, modular code organization, and gameplay logic structuring in Unity. The assignment required creating a small, isolated **shop system** where different gameplay domains interact only through a shared core layer.

---

## 🎯 Objectives

* Implement a **domain‑isolated architecture** (Core, Gold, Health, Location, VIP, Shop)
* Demonstrate **clean code principles** (SOLID, single responsibility, no cross‑domain dependencies)
* Create flexible **ScriptableObject‑based bundles** configurable by game designers
* Simulate **backend interaction** with a 3‑second delay on purchase
* Follow the **MVC pattern** for clarity and separation of logic/UI

---

## 🧩 Domain Structure

| Domain       | Responsibility                                                                                                                                                                        |
| ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Core**     | Shared base abstractions, `PlayerData`, coroutine runner, object pooling, and scene management. Defines the communication layer between systems without referencing domain specifics. |
| **Gold**     | Manages player gold. Provides and consumes currency through `ProvideOperation` and `ConsumeOperation` derived from core interfaces.                                                   |
| **Health**   | Manages player health. Supports both fixed and percentage‑based health operations.                                                                                                    |
| **Location** | Handles player's current location (string value). Contains operations to change or reset the current area.                                                                            |
| **VIP**      | Manages VIP status duration (TimeSpan). Supports extension and reduction of VIP time.                                                                                                 |
| **Shop**     | Implements the store UI and purchasing logic. Handles bundle display, async purchase simulation, and scene switching between shop and detailed view.                                  |

Each domain exists as a separate **assembly definition (.asmdef)** and depends **only on Core**, ensuring full modularity.

---

## 🧠 Key Features & Decisions

### 1. Core‑Driven Data Model

`PlayerData` acts as a central runtime container for player variables using **generic keys and values**. No explicit domain knowledge (e.g., `health`, `gold`, etc.) exists in Core.

### 2. Modular Operations ("Bricks")

Each domain defines its own **ScriptableObject‑based operations** implementing `IPlayerDataOperationInfo`. This enables designers to build bundles as combinations of costs and rewards without touching code.

### 3. Asynchronous Purchase Flow

Purchases are processed with an **async 3‑second delay**, simulating server communication. During this time, the Buy button is locked and displays *"Processing..."*.

### 4. UI Layer (MVC)

UI communicates with controllers inside each domain via events. The Shop view dynamically reacts to player resource changes (e.g., disabling Buy when resources are insufficient).

### 5. Extensibility

Adding a new domain (e.g., Leaderboard or Energy) requires **no modifications to Shop or Core** - only implementation of new operations in the new domain.

---

## 🧪 Implemented Gameplay Flow

1. Player sees all bundles on the main Shop scene.
2. Each bundle card includes its name, info button ("i"), and a Buy button.
3. Clicking *Buy* triggers an async coroutine to simulate a backend request.
4. Once complete, operations are executed: resources are consumed and rewards granted.
5. The top bar reflects updated player stats (Gold, Health, Location, VIP Time).
6. Each resource can be increased manually with a “+” button to test dynamic UI updates.

---

## 🛠️ Technical Highlights

* **Zenject‑ready architecture** (ProjectInstaller included)
* **Generic operation parameters** (`IntAmountParameter`, `PercentAmountParameter`)
* **Strongly‑typed data keys** (`PlayerDataKey<T>`)
* **Object pooling and coroutine runner** under Core utilities
* **Editor guard tools** (e.g., `AssetListGuard`) for asset consistency
* **Async/await pattern** instead of coroutines where possible

---

### 📂 Folder Structure Example

```
Scripts/
 ├─ Core/
 │   ├─ Player Data/
 │   ├─ Scene Loader/
 │   ├─ Coroutine Runner/
 │   └─ ObjectPool.cs
 ├─ Gold/
 ├─ Health/
 ├─ Location/
 ├─ VIP/
 └─ Shop/
```

---

### ✅ Result

This solution demonstrates a **clean modular gameplay framework**, ready for rapid feature addition and designer‑driven content creation - all within isolated Unity assemblies and a testable, maintainable architecture.
