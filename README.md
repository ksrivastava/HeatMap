# HeatMap
Unity tool for making customizable heat maps

The Heatmap Tool allows developers to rapidly add a telemetry infrastructure to track custom events, and visualize the collected data point in a heatmap that overlays the game level map. For example, a developer can track the point of player’s deaths to evaluate the difficulty of the level, or he can track how a player progresses through the map to evaluate the spatial characteristics of the level map. The power of the tool comes from the multi-client server architecture where each instance of the game posts data to a remote server, which in turn stores it in the database layer. The developer can fetch the data and visualize it on her local instance. The tool uses SimpleJSON (http://wiki.unity3d.com/index.php/SimpleJSON) to help facilitate the communication between clients and server, and either MongoDB or MySQL to store the data.

While developing the original level for Contra, my partner and I thought that the level we had created was both challenging and fun. Since we had been playing the game for hours for research, had implemented the mechanics, and knew the level layout ahead of time, we underestimated the difficulty of our level; this was evident during play testing since many players found it considerably hard to get past some of the obstacles. This led me to make a tool to allow developers to track spatial statistics about custom events.

I started the project by tracking deaths for player in 2D games, and then plotting spherical points at those locations in the game as death-maps. This also required me to write the server side code (PHP) and configure the database layer (MongoDb). In the next version, I expanded to 3D games and plotted the position of a player over time to produce progression-maps. In the third version, I created a way to track any custom event, e.g. player power up, resource acquisition points, or enemy kill points. I also shifted from spherical points to an overlaying map that was colored to reflect the event density. Lastly, in the final version, I added support for MySQL to allow the tool to be used for a wider audience.

The main challenge I faced was to prevent my tool from blocking the game UI thread, especially network latency during fetching the data, and the computations latency when plotting large about of data. This was resolved by using Unity’s Coroutines to making asynchronous network calls and creating only a few data points per frame during visualization.

Another challenge was to decide on how to create a heatmap when all the data had been fetched. The first approach I sought was to create a terrain over the level where, by using a heightmap, the peaks of the terrain would correspond to the even density around that region, and the color a part of the terrain would be relative to the height. An orthographic camera would view the terrain and level from the top, thus producing a heatmap. However, this method was too complex, and I went for a more elegant solution. I decided to split the level into grids and render a cube at the tracked event’s position and color the cube on a red-blue (red being high density, and blue being low) scale according to density of the events in that area.