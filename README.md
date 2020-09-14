# WPF Arkanoid!

This projects shows implementation of a simple 2D Arkanoid game in **WPF** with usage of the **MVVM pattern**. This project was created using Microsoft Visual Studio Community 2019 (Version 16.7.2) with only default .NET libraries and packages.

# Files

- **Views** folder - Contains XAML files as Views of the MVVM pattern.
- **ViewModels** folder - Contains View Models, commands and value converters.
- **Models** folder - Contains classes describing game state and logic.
- **Resources/Levels** folder - Contains XML representation of the individual game levels for easy editing.

## Game mechanics
Player uses a **paddle** (green horizontal rectangle) to control **ball** movement and try to hit and destroy all available **bricks** in each level. Upon destruction of all bricks in a level a new level is loaded.

The speed of all balls is gradually increasing over time so it motivates the player to finish the level as fast as possible.

Player initially has several lives and looses them when the last ball hits a **death trigger**. When all lives are spent, the game is over. When all levels are finished, the game is won.

Color of a brick indicates its health. Brick is destroyed when it reaches health value of 0 or less and points are added to player score. Each ball has a damage value, which is indicated by the ball's color.

**Collectables** can be spawned upon any brick destruction and have different effects when picked by the player paddle. Some effects are desireable and some are negative.
Currently implemented collectables:
- Add life
- Increase paddle width
- Decrease paddle width
- Clone all balls
- Decrease ball speed
- Increase ball damage

## Controls
- **Left/Right** keyboard arrow - Move player paddle sideways
- **Up/Space** - Release pinned ball
