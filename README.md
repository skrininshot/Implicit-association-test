# Implicit Association Test (IAT)

Unity-based IAT application. Currently ships with Race IAT, measuring automatic associations between Black/White faces and Good/Bad words. The architecture supports adding new IAT variants (Gender, Age, etc.) without code changes.

## Features
- **Race IAT** with two-block congruent/incongruent design.
- **Universal architecture** – all test configuration lives in `IatTestConfig` Scriptable Objects. No code required to add new tests.
- **Custom localization** – runtime language switching, event‑driven UI updates, separate JSON files per language.
- **MVC pattern** – clear separation of Views, Models, Controllers/Services.
- **Zenject** for dependency injection.
- **Addressables** for asynchronous loading of images and localization.
- **WebGL** deployment to GitHub Pages via CI/CD (GitHub Actions).

## Tech Stack
- Unity 2022.3, C#
- Zenject, Addressables
- Custom LocalizationService

## Live Build
[https://skrininshot.github.io/Implicit-association-test/](https://skrininshot.github.io/Implicit-association-test/)

## Run Locally
1. Clone the repository.
2. Open in Unity 2022.3.x.
3. Open scene `Assets/_Project/Scenes/Main.unity`.
4. Press Play for editor testing; build WebGL for browser.

## Adding New Tests
New IAT variants are added entirely through the Editor – no programming needed.

1. **Create a configuration**  
   `Assets → Create → IAT → New IAT`  
   Fill in the test ID, categories (faces, words), phase instructions, interpretation keys, etc.

2. **Add a preview button**  
   Create a prefab of `SelectIATButtonView` (or use the existing one) and assign it to the `Button Preview` field of your new config.

3. **Provide localized strings**  
   Add all required keys (instructions, results, interpretations) to `ru.json` and `en.json` using the test‑specific prefixes (`gender_`, `age_`, etc.).

4. **Register the config**  
   Drag the new `IatTestConfig` asset into the list of configs inside `Resources/Questionnaire Config`.

5. **Rebuild Addressables** if you added new image assets.

The test will appear automatically in the selection screen. The existing generator, checker, results handler and localization service will work unchanged.

## License
MIT
