/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Drawing;

namespace MidtermProject
{
    public class Mesh
    {
        // Vertex buffer object variable
        private int _vbo;

        // Vertex array object variable
        private int _vao;

        // Number of vertexes
        private int _vertexCount;

        public Mesh(float[] vertices)
        {
            _vertexCount = vertices.Length;

            _vao = GL.GenVertexArray();

            // Bind VAO
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();

            // Bind VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);
        }

        // Funtion to create the room
        public static Mesh CreateRoom()
        {
            float[] _vertices =
            {
                // First three vertexes are for position (x, y, z)
                // Next three vertexes are for normals
                // Last two vertexes are for texture
                -9.0f, 0.0f, -9.0f,  0.0f, 1.0f, 0.0f,  0.0f, 0.0f,
                 9.0f, 0.0f, -9.0f,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f,
                 9.0f, 0.0f,  9.0f,  0.0f, 1.0f, 0.0f,  1.0f, 1.0f,
                -9.0f, 0.0f, -9.0f,  0.0f, 1.0f, 0.0f,  0.0f, 0.0f,
                 9.0f, 0.0f,  9.0f,  0.0f, 1.0f, 0.0f,  1.0f, 1.0f,
                -9.0f, 0.0f,  9.0f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f,

                -9.0f, 6.0f, -9.0f,  0.0f, -1.0f ,0.0f,  0.0f, 0.0f,
                 9.0f, 6.0f, -9.0f,  0.0f, -1.0f, 0.0f,  1.0f, 0.0f,
                 9.0f, 6.0f,  9.0f,  0.0f, -1.0f, 0.0f,  1.0f, 1.0f,
                -9.0f, 6.0f, -9.0f,  0.0f, -1.0f, 0.0f,  0.0f, 0.0f,
                 9.0f, 6.0f,  9.0f,  0.0f, -1.0f, 0.0f,  1.0f, 1.0f,
                -9.0f, 6.0f,  9.0f,  0.0f, -1.0f, 0.0f,  0.0f, 1.0f,

                -9.0f, 0.0f,  -9.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
                 9.0f, 0.0f,  -9.0f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f,
                 9.0f, 6.0f,  -9.0f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,
                -9.0f, 0.0f,  -9.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
                 9.0f, 6.0f,  -9.0f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,
                -9.0f, 6.0f,  -9.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,

                -9.0f, 0.0f,  9.0f,   0.0f, 0.0f, -1.0f,  0.0f, 0.0f,
                 9.0f, 0.0f,  9.0f,   0.0f, 0.0f, -1.0f,  1.0f, 0.0f,
                 9.0f, 6.0f,  9.0f,   0.0f, 0.0f, -1.0f,  1.0f, 1.0f,
                -9.0f, 0.0f,  9.0f,   0.0f, 0.0f, -1.0f,  0.0f, 0.0f,
                 9.0f, 6.0f,  9.0f,   0.0f, 0.0f, -1.0f,  1.0f, 1.0f,
                -9.0f, 6.0f,  9.0f,   0.0f, 0.0f, -1.0f,  0.0f, 1.0f,

                -9.0f, 0.0f, -9.0f,   1.0f, 0.0f, 0.0f,  0.0f, 0.0f,
                -9.0f, 0.0f,  9.0f,   1.0f, 0.0f, 0.0f,  1.0f, 0.0f,
                -9.0f, 6.0f,  9.0f,   1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
                -9.0f, 0.0f, -9.0f,   1.0f, 0.0f, 0.0f,  0.0f, 0.0f,
                -9.0f, 6.0f,  9.0f,   1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
                -9.0f, 6.0f, -9.0f,   1.0f, 0.0f, 0.0f,  0.0f, 1.0f,

                 9.0f, 0.0f, -9.0f,  -1.0f, 0.0f, 0.0f,  0.0f, 0.0f,
                 9.0f, 0.0f,  9.0f,  -1.0f, 0.0f, 0.0f,  1.0f, 0.0f,
                 9.0f, 6.0f,  9.0f,  -1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
                 9.0f, 0.0f, -9.0f,  -1.0f, 0.0f, 0.0f,  0.0f, 0.0f,
                 9.0f, 6.0f,  9.0f,  -1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
                 9.0f, 6.0f, -9.0f,  -1.0f, 0.0f, 0.0f,  0.0f, 1.0f,
            };

            // Returning the list of vertices
            return new Mesh(_vertices);
        }

        // Function to create the cube
        public static Mesh CreateCube()
        {
            float[] _vertices =
            {
                // First three vertexes are for position (x, y, z)
                // Next three vertexes are for normals
                // Last two vertexes are for texture (using 0,0 since no texture is applied)
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,

                 0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,

                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,

                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            };

            // Returning the list of vertices
            return new Mesh(_vertices);
        }

        // Function to create the triangular prism
        public static Mesh CreateTriangularPrism()
        {
            List <float> vertices_list = new List<float>();

            // Add vertices for the triangular prism
            // Bottom triangle
            Vector3 v1 = new Vector3(0.0f, -0.4f, 0.4f);       
            Vector3 v2 = new Vector3(-0.4f, -0.4f, -0.4f);     
            Vector3 v3 = new Vector3(0.4f, -0.4f, -0.4f);      

            // Top triangle
            Vector3 v4 = new Vector3(0.0f, 0.4f, 0.4f);        
            Vector3 v5 = new Vector3(-0.4f, 0.4f, -0.4f);     
            Vector3 v6 = new Vector3(0.4f, 0.4f, -0.4f);       

            // Adding vertices to the list
            vertices_list.AddRange(new float[] { v1.X, v1.Y, v1.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f });   
            vertices_list.AddRange(new float[] { v2.X, v2.Y, v2.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f });  
            vertices_list.AddRange(new float[] { v3.X, v3.Y, v3.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f });   
            vertices_list.AddRange(new float[] { v4.X, v4.Y, v4.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });   
            vertices_list.AddRange(new float[] { v5.X, v5.Y, v5.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });   
            vertices_list.AddRange(new float[] { v6.X, v6.Y, v6.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });   

            // Creating rectangule faces and adding to the list
            //Rectangle Face 1
            Vector3 r1 = v1;   
            Vector3 r2 = v4;   
            Vector3 r3 = v6;   
            Vector3 r4 = v3;   
            vertices_list.AddRange(new float[] { r1.X, r1.Y, r1.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });     // Triangle 1/2 for Rectangle Face 1
            vertices_list.AddRange(new float[] { r2.X, r2.Y, r2.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });     // Triangle 1/2 for Rectangle Face 1
            vertices_list.AddRange(new float[] { r3.X, r3.Y, r3.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });     // Triangle 1/2 for Rectangle Face 1
            vertices_list.AddRange(new float[] { r1.X, r1.Y, r1.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });     // Triangle 2/2 for Rectangle Face 1
            vertices_list.AddRange(new float[] { r3.X, r3.Y, r3.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });     // Triangle 2/2 for Rectangle Face 1
            vertices_list.AddRange(new float[] { r4.X, r4.Y, r4.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f });     // Triangle 2/2 for Rectangle Face 1

            // Rectangle Face 2
            Vector3 s1 = v2;   
            Vector3 s2 = v5;   
            Vector3 s3 = v4;   
            Vector3 s4 = v1;   
            vertices_list.AddRange(new float[] { s1.X, s1.Y, s1.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });    // Triangle 1/2 for Rectangle Face 2
            vertices_list.AddRange(new float[] {s2.X, s2.Y, s2.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });     // Triangle 1/2 for Rectangle Face 2
            vertices_list.AddRange(new float[] { s3.X, s3.Y, s3.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });    // Triangle 1/2 for Rectangle Face 2
            vertices_list.AddRange(new float[] {s1.X, s1.Y, s1.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });     // Triangle 2/2 for Rectangle Face 2
            vertices_list.AddRange(new float[] { s3.X, s3.Y, s3.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });    // Triangle 2/2 for Rectangle Face 2
            vertices_list.AddRange(new float[] {s4.X, s4.Y, s4.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });     // Triangle 2/2 for Rectangle Face 2

            // Rectangle Face 3
            Vector3 t1 = v3;   
            Vector3 t2 = v6;   
            Vector3 t3 = v5;   
            Vector3 t4 = v2;  
            vertices_list.AddRange(new float[] { t1.X, t1.Y, t1.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });     // Triangle 1/2 for Rectangle Face 3
            vertices_list.AddRange(new float[] { t2.X, t2.Y, t2.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });     // Triangle 1/2 for Rectangle Face 3
            vertices_list.AddRange(new float[] { t3.X, t3.Y, t3.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });     // Triangle 1/2 for Rectangle Face 3
            vertices_list.AddRange(new float[] { t1.X, t1.Y, t1.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });     // Triangle 2/2 for Rectangle Face 3
            vertices_list.AddRange(new float[] {t3.X, t3.Y, t3.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });      // Triangle 2/2 for Rectangle Face 3
            vertices_list.AddRange(new float[] {t4.X, t4.Y, t4.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });      // Triangle 2/2 for Rectangle Face 3

            // Returning the list of vertices
            return new Mesh(vertices_list.ToArray());
        }

        // Funtion to create the pyramid
        public static Mesh CreatePyramid()
        {
            List<float> vertices_list_pyramid = new List<float>();

            // Vertices of the pyramid
            Vector3 p1 = new Vector3(-0.5f, -0.5f, 0.5f);   //base vector
            Vector3 p2 = new Vector3(-0.5f, -0.5f, -0.5f);  //base vector
            Vector3 p3 = new Vector3(0.5f, -0.5f, -0.5f);   //base vector
            Vector3 p4 = new Vector3(0.5f, -0.5f, 0.5f);    //base vector
            Vector3 p5 = new Vector3(0.0f, 0.5f, 0.0f);     // top vector

            // Creating faces and adding them to list
            // Square Face
            vertices_list_pyramid.AddRange(new float[] { p1.X, p1.Y, p1.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f }); // Triangle 1/2 for Square Face
            vertices_list_pyramid.AddRange(new float[] { p2.X, p2.Y, p2.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f }); // Triangle 1/2 for Square Face
            vertices_list_pyramid.AddRange(new float[] { p3.X, p3.Y, p3.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f }); // Triangle 1/2 for Square Face
            vertices_list_pyramid.AddRange(new float[] { p1.X, p1.Y, p1.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f }); // Triangle 2/2 for Square Face
            vertices_list_pyramid.AddRange(new float[] { p3.X, p3.Y, p3.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f }); // Triangle 2/2 for Square Face
            vertices_list_pyramid.AddRange(new float[] { p4.X, p4.Y, p4.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f }); // Triangle 2/2 for Square Face

            // Triangle Face 1
            vertices_list_pyramid.AddRange(new float[] { p1.X, p1.Y, p1.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f }); 
            vertices_list_pyramid.AddRange(new float[] { p2.X, p2.Y, p2.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f }); 
            vertices_list_pyramid.AddRange(new float[] { p5.X, p5.Y, p5.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f }); 

            // Triangle Face 2
            vertices_list_pyramid.AddRange(new float[] { p2.X, p2.Y, p2.Z, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f });
            vertices_list_pyramid.AddRange(new float[] { p3.X, p3.Y, p3.Z, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f }); 
            vertices_list_pyramid.AddRange(new float[] { p5.X, p5.Y, p5.Z, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f }); 

            // Triangle Face 3
            vertices_list_pyramid.AddRange(new float[] { p3.X, p3.Y, p3.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f }); 
            vertices_list_pyramid.AddRange(new float[] { p4.X, p4.Y, p4.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f }); 
            vertices_list_pyramid.AddRange(new float[] { p5.X, p5.Y, p5.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f }); 

            // Triangle Face 4
            vertices_list_pyramid.AddRange(new float[] { p1.X, p1.Y, p1.Z, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f }); 
            vertices_list_pyramid.AddRange(new float[] { p4.X, p4.Y, p4.Z, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f }); 
            vertices_list_pyramid.AddRange(new float[] { p5.X, p5.Y, p5.Z, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f }); 

            // Return the list of vertices
            return new Mesh(vertices_list_pyramid.ToArray());

        }


        // Function to display shapes with texture
        public void DisplayTexturedShapes(Texture t)
        {
            GL.BindVertexArray(_vao);
            t.UseTexture();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount / 8);
        }

        // Function to display/render the shapes
        public void DisplayShapes()
        {
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount / 8);
        }


    }
}


