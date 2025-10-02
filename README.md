# GameEngineFoundations-Assignment3  

### What library did you use (SharpDX or OpenTK)?
The library I used is OpenTK. I first had to install OpenTK under dependencies to ensure that OpenTK can be used in my application.  

### Short explination of how you rendered the cube
I used my Assignment 2 application, rotating a 2D rectangle and upgraded it for this assignment, so it renders a 3D cube and rotates. Here are the modifications I did to achieve this:  
1) Created an elementBufferHandle variable to handle the EBO
2) Created an indices array to list the possible indices that create a triangle. I created 12 indices, as 2 triangles equals to 1 face of a cube.
3) Added DepthTest in the OnLoad function to create depth of the object
4) Updated the vertices array by changing the rectangle vertices to a square. I also changed the Z values so it renders a 3D object
5) Added the part to generate the EBO to store indices and reuse the vertices
6) Changed the color of the cube, to clearly show that it's a 3D object
7) Deleted the elementBufferHandle variable in the OnUnload function
8) Replaced DrawArrays with DrawElements, added the indices.Length and the necessary values to draw out the cube in the OnRenderFrame function


### Output:
<img width="1290" height="828" alt="image" src="https://github.com/user-attachments/assets/e93025ed-71ae-42e3-8b27-75fedbb317df" />
