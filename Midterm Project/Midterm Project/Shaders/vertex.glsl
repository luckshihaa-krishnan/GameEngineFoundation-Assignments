#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoord;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProj;

void main()
{
    FragPos = vec3(uModel * vec4(aPosition, 1.0));
    Normal = mat3(transpose(inverse(uModel))) * aNormal;            // using transpose and inverse here instead of Game.cs
    Normal = normalize(Normal);                                     // normalize the normal vector
    gl_Position = uProj * uView * uModel * vec4(aPosition, 1.0);    // calculate vertex position
    TexCoord = aTexCoord;                                           // pass texture coordinates to fragment shader

}
