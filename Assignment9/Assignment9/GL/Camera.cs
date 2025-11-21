/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using System;
using OpenTK.Mathematics;

namespace Assignment9
{
    public class Camera
    {
        // Vectors for directions poitning outwards from the camera to define how it's rotated
        public Vector3 _front = -Vector3.UnitZ;
        public Vector3 _up = Vector3.UnitY;
        public Vector3 _right = Vector3.UnitX;

        // Rotation around the X axis
        public float _pitch;

        // Rotation around the Y axis
        public float _yaw = -MathHelper.PiOver2;

        // Field of View of camera
        public float _fov = MathHelper.PiOver2;

        // Position of camera
        public Vector3 Position { get; set; }

        // Aspect Ratio of viewport
        public float AspectRatio { private get; set; }

        // Constructor that initializes position, aspect ratio and updates vectors
        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
            UpdateVectors();
        }


        public Vector3 Front => _front;
        public Vector3 Up => _up;
        public Vector3 Right => _right;

        // Function to move camera based on vector and distance
        public void MoveCamera(Vector3 moveVector, float distance)
        {
            Position += _front * moveVector.Z * distance;   // Forward and backward movement
            Position += _up * moveVector.Y * distance;      // Up and down movement
            Position += _right * moveVector.X * distance;   // Left and right movement
        }

        // Function to get view matrix using LookAt function
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        // Function to get the projection matrix
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        }


        // Function to handle mouse movement
        public void OnMouseMove(float deltaX, float deltaY)
        {
            _yaw += deltaX;
            _pitch -= deltaY;

            // Clamp the pitch to prevent flipping
            if (_pitch > 89.0f)
            {
                _pitch = 89.0f;
            }

            // Clamp the pitch to prevent flipping
            if (_pitch < -89.0f)
            {
                _pitch = -89.0f;
            }
            UpdateVectors();
        }

        // Function to update the direction vertices
        public void UpdateVectors()
        {
            // Front matrix is calculated
            _front.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
            _front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
            _front.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));

            // Making sure vectors are normalized
            _front = Vector3.Normalize(_front);

            // Recalculate right and up vector using Cross product
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        }
    }
}