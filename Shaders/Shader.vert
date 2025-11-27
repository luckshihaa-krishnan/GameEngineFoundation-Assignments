
#version 330 core
layout(location=0) in vec3 aPosition; //vertex position input
                    
uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProj;

void main()
{
    gl_Position =  uProj * uView * uModel * vec4(aPosition, 1.0); //transform vertex position
}