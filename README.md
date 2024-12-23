# Survivor: A C# and XML developed shoot'em up game using MonoGame

## Important warning !
Depending on your OS you may have trouble with the file **font.spritefont**. Most of the gamers use Windows that is why Arial is the original font used in the game but it may not be natively installed on Linux (Ubuntu for example).
So if there is an issue with this file while trying to run the game, relax and follow those simple steps :
1. Open the file, his path is : **/Content/images/font.spritefont**
2. Replace the line **\<FontName>Arial\</FontName>** by **\<FontName>Liberation Sans\</FontName>**.
3. Save the file
4. Run the game
5. Enjoy it !

We’re really sorry for any inconvenience this may cause and appreciate your understanding.

## Game Description
"**Survivor**" is a shoot'em up game where you control a character who must defeat waves of enemies to upgrade his abilities.
As you kill enemies, you collect XP, which allow you to upgrade your character's abilities, making him stronger or faster.<br>
**Surviving is your only goal !**<br>
The game is built using **MonoGame** for the game engine, and it relies on **XML** for data management, and **C#** for the game logic.

### Key Features
- **Upgrade System**: Kill enemies to be more powerful.
- **Increasing Difficulty**: As you defeat enemies your XP level increases making enemies spawn faster and stronger, adding an increasing challenge to the gameplay.
- **Fancy Game Design**: Thanks to Nox and her drawings skills, the game looks very nice !
- **Player Stats**: Track your score at the end of each game with your time survived and amount of kills !

## Project Structure

### C_Sharp
Contains all the **core C# code** responsible for the game’s mechanics.
Includes the logic for the game’s **player control**, **enemy behaviors**, **XP and upgrade systems**.

### Content
Contains all the **assets** used in the game.
**Sprites and backgrounds** are stored here and loaded during gameplay.

### CSS
Contains **stylesheets** for HTML pages.

### HTML
Contains **HTML files** generated from XML data.

### XML
Contains **XML files** with structured data used by the game.
XML files in this folder contain data for game configurations, player profiles, enemy stats, high scores, and more.

### XSD
Contains **XML Schema Definition** files.
These files define the structure and rules for the XML files in the project.

### XSLT
Contains **XSLT files** for transforming XML data into HTML.
These transformations are applied to the XML files to generate dynamic web pages for displaying game information.

© 2024 | COLOMBAN N. - DELEUZE-DORDRON A. - YAHA S. | All rights reserved.

