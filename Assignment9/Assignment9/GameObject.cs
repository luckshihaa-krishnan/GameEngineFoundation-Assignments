/*
* Name: Luckshihaa Krishnan 
* Student ID: 186418216
* Section: GAM 531 NSA 
*/

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;


namespace Assignment9
{
    public class GameObject
    {
        public Mesh _mesh;
        public Vector3 _position;
        public Vector3 _size;
        public Vector3 _color;

        // Constructor
        public GameObject(Mesh mesh, Vector3 pos, Vector3 size, Vector3 color)
        {
            _mesh = mesh;
            _position = pos;
            _size = size;
            _color = color;
        }

        // Set color after creation
        public void SetColor(Vector3 c) {
            _color = c;
        }

        // Render the object using shader
        public void Render(Shader shader)
        {
            shader.Use();
            Matrix4 model = Matrix4.CreateTranslation(_position);
            shader.SetMatrix4("uModel", model);
            shader.SetVector3("objectColor", _color);
            _mesh.DisplayShapes();
        }
    }
}
