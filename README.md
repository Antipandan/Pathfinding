# Pathfinding
Pathfinding using the A* algorithm. Visualization using Unity version 6000.0.58.f2

<p align="center">
  <img src="Assets/Video/Pathfinding.GIF" alt="Demo" width="800" />
</p>

## Setup
Download zip file and extract contents into and empty Unity project

## Use
This demonstration is intended to be used in Unity Playmode. Settings can be applied in both editmode and 
playmode. Certain settings are not able to take hold during editmode. Those settings will be applied on startup.
The Unity inspector provides the user with three scripts that modify the behavior / visualization of the pathfinding 
algorithm. Users will find the relevant scripts in the inspector when clicking on the GameObject 'WorldCanvas' in
the Unity Hierarchy. <br><br/>
Users are able to modify the generation of walkable squares in the 'GenerateMap' script. Users are able to modify
the color of squares in the 'DrawMap' script. Users are able to modify the behavior of the A* algorithm in the 
'AStarPathfinding' script. Users are also able to customize the layout of the board in the scene view. By clicking on
the walkable squares, users are able to modify the square type in the script 'Square'. Users are only intended make
a square into either a wallSquare or a regularSquare. For this feature to work, the modell requires that either an 
already existing count of walkable squares equal to the number of rows times the number of columns exist, or that
the script generates a grid during runtime and users modify the generated grid. Modifications made to a generated
grid will not persist when rerunning.

## Notes
While this modell accomodates for a wide range of setups, there are certain settings that can cause the
performance and usability of the modell to be plummet.

## Bugs
1) Users are not able to change the color of all squares of a given type during editmode. 
2) Users are not able to change the 'Weight', 'G' and 'H' of a given square.
3) Changing the color of a given square during runtime, causes big performance dips
4) changing the dimensions of a grid will cause very big performance dips
