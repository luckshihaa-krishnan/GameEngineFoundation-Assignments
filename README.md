# Assignment 9

## The collision detection method you used
The collision detection method I used is the AABB (Axis-Aligned Bounding Box). I used this method as the objects and the room are not circular objects, so to make it easier, I can use the AABB method so I can add a bounding box for collision detection. The camera is the player in this application, meaning collisions will be checked between where the camera is positioned and the objects and room. If the camera tries to go through the object or leave the room, it is considered as a collision. This method prevents the camera from going through the objects and leaving the room.


## How your collision and movement integration works.
The way the collision and movement integration works is first to check the camera's position. As each time the camera moves, it first checks for the camera's new position to see if any collisions are detected. If there is a collision, the movement does not occur and camera stays in place. This prevents the camera from going through the object and leaving the room. If there is no collision, the camera's position is updated.


## Any challenges encountered and how you solved them.
One of the challenges I faced was that the camera was going through objects and leaving the room. I fixed this by recalculating the camera's position by first checking its new position to see if there are collisions. Another issue I faced was testing the collision. It was difficult for me to test if the collision worked because when the camera gets close to an object, it’s hard to see if a collision occurs. I fixed this issue by adjusting the speed of the camera, and when testing, I made sure to create collision at the side of the object, so I can easily identify if collision occurs and works.


## References
A helpful resource I used to check for collision : https://learnopengl.com/In-Practice/2D-Game/Collisions/Collision-detection
