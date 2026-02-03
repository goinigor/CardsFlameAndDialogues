# Cards, Flame and Dialogues

## Overview

### Cards, Flame and Dialogues technical assignment:

**Create a new project from scratch and complete the 3 tasks below.**

1. **"Ace of Shadows"**

   Create 144 sprites stacked like cards in a deck, with each top card partially
   covering the one below. Every 1 second the top card should move smoothly to
   another stack. Display a counter above each stack and show a message when all
   animations are finished.

2. **“Magic Words”**

   Create a system that combines text and Unicode emojis to render character
   dialogue using data from the endpoint below. Load the data dynamically at
   runtime and handle cases where avatar URLs may not load or data is missing.
   https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords
3. **“Phoenix Flame”**

   Create a particle effect demo that shows a great fire effect. Add a UI button that
   controls the fire colour using an animator controller. The fire should transition
   smoothly from orange to green to blue and loop back to orange.


   **Technical requirements:**

   ● Write your code in C# and use Unity6.

   ● Each task should be accessed via an in-game menu.

   ● Render responsively for both mobile and desktop devices.

   ● Display the fps in the top left corner.

   ● Build for WebGL and provide a link to the hosted version of the app.

## Project Structure

```
Assets/
|--- Scripts/
|   |--- Bootstrap.cs                 # Application entry point
|   |---Core/                         # Core systems and utilities
|   |   |--- API.cs                   # HTTP API service for data fetching
|   |   |--- ServiceLocator/          # Dependency injection system
|   |   |--- SceneController/         # Scene loading management
|   |   |---Pooling/                  # Object pooling system
|   |--- Features/                    # Game features
|   |   |--- CardsShuffle/            # Card shuffling animation system
|   |   |--- Dialogues/               # Dialogue system with avatars
|   |   |--- PhoenixFlame/            # Phoenix flame visual effects system
|   |--- UI/                          # User interface components
|   |--- Misc/                        # Utility scripts
|--- Scenes/                          # Unity scenes
|--- Configs/                         # Scriptable object configurations
|--- ...                              # Standard Unity assets
```

## Core Architecture

### Service Locator Pattern

The project uses a service locator pattern for dependency injection, implemented in `ServiceLocator.cs`:

```csharp
// Register services
ServiceLocator.Register<API>(new API());
ServiceLocator.Register<ISceneController>(_sceneControllerService);

// Resolve services
var api = ServiceLocator.Resolve<API>();
var sceneController = ServiceLocator.Resolve<ISceneController>();
```

### Bootstrap

The `Bootstrap.cs` class is the application entry point:
- Initializes core services
- Registers dependencies in the service locator
- Loads the initial scene

## Core Systems

### API Service (`Core/API.cs`)

Handles HTTP requests for external data fetching:
- **Generic GET requests**: `Get<T>(url, onSuccess, onFail)`
- **Texture downloading**: `DownloadTexture2D(url)`
- **Async/await support** with cancellation tokens
- **Error handling** and timeout management

### Scene Controller (`Core/SceneController/`)

Manages scene loading with loading screen:
- **Async scene loading** with progress tracking
- **Minimum load time** to ensure full transitions through the loading screen
- **Loading screen integration**

### Object Pooling (`Core/Pooling/`)

Generic object pooling system:
- **Auto-expansion** feature
- **Pre-warming** for initial pool size

## Game Features

### 1. Cards Shuffle System (`Features/CardsShuffle/`)

Implements animated card shuffling from one deck to another:

#### Components:
- **CardsShuffleSystem**: Main system controller
- **CardPool**: Object pooling for card objects
- **ICardsAnimationBehaviour**: Animation interface
- **CardsAnimationBehaviourCurves**: Current implementation of the animation interface with custom animations
- **DeckCountView**: UI counter for deck sizes

#### Flow:
1. Creates specified number of cards from pool
2. Animates cards dropping into start deck
3. Waits for delay period before shuffling
4. Animates shuffle to end deck with delays
5. Updates deck counters in real-time
6. Shows ending text when complete

### 2. Dialogue System (`Features/Dialogues/`)

Dialogue system with avatar support:

#### Components:
- **DialoguesSystem**: Main dialogue controller
- **DialogueAvatarsCache**: Avatar texture caching

#### Features:
- **Remote data loading** from JSON APIs
- **Avatar caching** with texture downloading
- **Fallback handling** for missing assets

#### Data Structure:
```json
{
  "dialogue": [
    {
      "name": "CharacterName",
      "text": "Dialogue text"
    }
  ],
  "avatars": [
    {
      "name": "CharacterName",
      "url": "https://example.com/avatar.png",
      "position": "left"
    }
  ]
}
```

### 3. Phoenix Flame System (`Features/PhoenixFlame/`)

Visual effects system for phoenix flame animations:
- **Color control** and animation
- **Particle effects** integration

## Utility Scripts

### AspectRatioCameraFitter (`Misc/AspectRatioCameraFitter.cs`)

Automatically adjusts camera position based on screen aspect ratio

### FPS Counter (`Misc/FPSCounter.cs`)

Performance monitoring tool

## Scene Organization

### Build Index Structure:
1. **Boot** (Index 0): Bootstrap scene with service initialization (Entry point)
2. **Menu** (Index 1): Main menu scene
3. **Other scenes** (indices are binded in the Menu scene)

### General Flow Aspects

- Start Boot scene. Bootstrap started, initializes core services (API, ISceneController) and start Menu scene.
- Each next loaded scene has SceneContext object with the SceneContext attached to it. SceneContext contains list of
AbstractMonoInstaller's
- AbstractMonoInstaller can be treated as an entry point of the scene. There we can create presenters,
Resolve dependencies, Initialize need features and start their behaviour.
- Each SceneContext has implemented IDisposable patter, and clear resources on SceneContext gameObject OnDestroy method.

## Technical Specifications

### Code Patterns:
- **MVP (Model-View-Presenter)** pattern for UI systems
- **Service Locator** for dependency injection
- **Async/await** for asynchronous operations
- **IDisposable** pattern for resource cleanup
