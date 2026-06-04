# Implicit Association Test (IAT)

Unity-based IAT application. Currently implements the Race IAT, measuring automatic associations between racial categories (Black/White faces) and evaluative attributes (Good/Bad words). Architecture supports rapid addition of new test categories.

## Features
- Race IAT with two-block congruent/incongruent design.
- Custom localization system with runtime language switching and event-driven UI updates.
- MVC architecture (Models, Views, Services/Controllers).
- Dependency injection via Zenject.
- Asynchronous asset loading with Unity Addressables (faces, localization JSONs).
- WebGL deployment to GitHub Pages via CI/CD (GitHub Actions).

## Tech Stack
- Unity 2022.3, C#
- Zenject, Addressables
- Custom LocalizationService
- MVC pattern

## Live Build
[https://skrininshot.github.io/Implicit-association-test/](https://skrininshot.github.io/Implicit-association-test/)

## Run Locally
1. Clone the repository.
2. Open in Unity 2022.3.x.
3. Open scene `Assets/_Project/Scenes/Main.unity`.
4. Press Play for editor testing; build WebGL for browser.

## Adding New Tests
1. Implement `IQuestionnaireResultsModel`, `IQuestionnaireGenerationSettings`, `IQuestionnaireGenerator`, `ICorrectAnswerChecker` and `IResultsHandler` for the new IAT variant.
2. Add localization keys for instructions and results.
3. Register in Zenject container. Core MVC and localization require no changes.

## License
MIT
