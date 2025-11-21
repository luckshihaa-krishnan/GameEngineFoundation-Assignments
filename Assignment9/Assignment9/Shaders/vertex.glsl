#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;

out vec2 TexCoord;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProj;

void main()
{
    TexCoord = aTexCoord;
    gl_Position = uProj * uView * uModel * vec4(aPosition, 1.0);
}
