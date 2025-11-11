// Fragment shader: samples sub-react of the sheet using uOffset/uSize

#version 330 core

in vec2 vTexCoord;
out vec4 color;

uniform sampler2D uTexture;     // bound to texture unit 0
uniform vec2 uOffset;           // normalized UV start (0..1)
uniform vec2 uSize;             // normalized UV size  (0..1)


void main() {
    vec2 uv = uOffset + vTexCoord * uSize;
    color = texture(uTexture, uv);
}
