# Blockade

A Unity implementation of the classic board game Blockade, where players strategically navigate across the board while blocking their opponent's path.

It was made in the context of a group project in my last year of bachelor with a group of 15 students. I was responsible of the UI team. I developed the implementation of the DTOs received from the game logic section into the interface and the error management of the whole UI.

## Game Rules

### Overview
- 2 players game
- Each player has 2 pawns
- Players take turns moving pawns and placing walls
- Goal: Get your pawns to the opposite side of the board

### Gameplay
- On your turn, you can either:
  1. Move one pawn two square (horizontally, vertically, or diagonally), if possible, or only one square if a wall or another pawn is blocking you
  2. Place a wall to block your opponent's path
- Each player starts with 9 vertical and 9 horizontal walls
- Walls cannot overlap or cross each other
- Players must always leave at least one possible path to the goal

### Victory Condition
The first player to move both pawns to their opponent's starting spots wins the game.

## Project Structure

```
My project/
├── Assets/                   # Game assets and scripts
│   ├── Animations/           # Game animations
│   ├── Prefabs/              # Reusable game objects
│   │   └── UIManager         # UI elements and menus
│   ├── Resources/            # Game resources
│   ├── Scenes/               # Game scenes/levels
│   └── Scripts/              # Game logic
│       ├── GameLogic/        # Core game mechanics
│       └── IHM/              # UI and player interaction
├── Library/                  # Unity cache files
├── Packages/                 # Unity packages
└── ProjectSettings/          # Unity project configuration
```

### Key Components

#### Scripts
- `GameManager`: Controls game flow and player turns
- `Board`: Manages the game board and piece placement
- `Player`: Handles player actions and state
- `UIManager`: Controls menus and user interface

#### Features
- Local multiplayer (Player vs Player)
- AI opponent (Player vs Computer)
- Online multiplayer support
- Customizable settings
- Game history tracking

## Technical Details
- Built with Unity 2021
- Uses TextMeshPro for UI
- Implements DTO pattern for game state management
- Support for multiple languages

## Installation

1. Clone the repository
2. Open the project in Unity
3. Open the main scene in `Assets/Scenes`
4. Press Play to test the game

---

For development questions or contributions, please refer to the project documentation.
