# Public Goods Game

Projet réalisé avec Unity dans le cadre du cours Services et Technologies Multimédia à l'Université de Genève.

Le jeu reprend le principe du *Public Goods Game* dans une version modifiée. Le joueur affronte trois personnages contrôlés par l'ordinateur et choisit, à chaque tour, quelle part de son argent investir dans un pot commun. Le but est de conserver un bon score personnel tout en observant les effets de la coopération, de la redistribution et de la punition.

## Déroulement

1. Choisir un mode de jeu.
2. Lire la courte description du mode.
3. Choisir si les personnalités des IA doivent être connues ou cachées.
4. Utiliser le slider pour sélectionner le pourcentage à investir.
5. Cliquer sur `INVEST` pour jouer le tour.

Pendant la partie, le classement est actualisé selon l'argent de chaque participant. Les investissements et éventuelles punitions des IA sont aussi affichés.

À la fin, le jeu présente le gagnant, un résumé de la coopération et plusieurs résultats collectifs : richesse totale du groupe, écart entre les joueurs et distance par rapport à un scénario de coopération maximale.

## Modes disponibles

- **Base Sandbox** : règles standards, cinq tours.
- **Bullet Sandbox** : partie courte de trois tours.
- **Long Sandbox** : partie de dix tours.
- **Rich AI** : chaque IA commence avec 300 pièces.
- **No Punishment** : aucune punition et une IA Free Rider investit 0 % pendant toute la partie.
- **High Multiplier** : le multiplicateur du pot passe de `1.6` à `5`.
- **Only Banker** : les trois IA utilisent le comportement Banker.
- **Only Gambler** : les trois IA utilisent le comportement Gambler.
- **Only Fast Food Manager** : les trois IA suivent la moyenne précédente.

Le tutoriel explique progressivement les règles, les personnalités, le multiplicateur, le coefficient individuel et le fonctionnement de la punition. Il contient également un tour interactif.

## Lancer le projet

Le projet utilise Unity `6000.4.0f1`.

1. Cloner ou télécharger le dépôt.
2. Ajouter le dossier du projet dans Unity Hub.
3. Ouvrir le projet avec la version indiquée ci-dessus.
4. Ouvrir `Assets/Scenes/MainMenu.unity`.
5. Lancer le mode Play.

Les scènes incluses dans le build sont :

- `MainMenu` ;
- `MainMenuSandbox` ;
- `PlayGame` ;
- `Tutorial`.

## Organisation principale

- `Assets/GameManager.cs` : règles de la partie, IA, classement et résultats.
- `Assets/MenuManager.cs` : navigation depuis le menu principal.
- `Assets/SandboxMenuManager.cs` : sélection des modes et popups du menu sandbox.
- `Assets/TutorialManager.cs` : progression et tour interactif du tutoriel.
- `Assets/Scenes/` : scènes principales du jeu.
