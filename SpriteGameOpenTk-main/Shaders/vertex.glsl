// Vertex Shader: transforms positions, flips V in UVs (image origin vs GL origin)

#version 330 core

layout(location = 0) in vec2 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 vTexCoord;

uniform mat4 projection;
uniform mat4 model;


void main() {
    gl_Position = projection * model * vec4(aPosition, 0.0, 1.0);
    vTexCoord = vec2(aTexCoord.x, 1.0 - aTexCoord.y);                   // flip V so PNGs read intuitively
}
