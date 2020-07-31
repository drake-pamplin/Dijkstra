# Dijkstra
The Dijkstra algorithm demo repo.

This is a Unity project used to demonstrate the properties of the Dijkstra pathfinding algorithm.

When loaded into Unity:

 1. Press the play button and the map file loaded into the "Maze One" slot in the GameManager object will be loaded.
 2. Click on the point that you want to start at.
 3. Click on the point that you want to end at.
 4. Press "Space" to start the pathfinding algorithm.
 5. Map can be refreshed with "Space" after algorithm has finished running.
 
 New maps can be made and loaded into the "Resources/Maps" folder. Once the map is made, simple type the name into "Map" field in the GameManager object in the Inspector. Maps must be in "<FILENAME>.txt" format with "0" as open tiles and "x" as walls (see example below):
  
8
16
0x000xxxxxxxxxx0
0x0x000000000000
0x0xxxxxxxxxxxxx
0x00000000000000
0x0xxxx0xxxxxxx0
0x0000x0x0000000
0xxxxxx0xxxxxxxx
0000000000000000

First line is the number of rows. Second line is the number of columns. Following those two is the map itself.
