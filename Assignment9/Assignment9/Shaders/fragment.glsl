#version 330 core
out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D ourTexture;
uniform bool hasTexture;
uniform vec3 objectColor;

void main()
{
    if (hasTexture){
        FragColor = texture(ourTexture, TexCoord);
    }
    else{
        FragColor = vec4(objectColor, 1.0);
    }
}
