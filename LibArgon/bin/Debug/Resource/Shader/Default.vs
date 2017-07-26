#version 330

in vec3 vPosition;
in vec4 vColor;
out vec4 color;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 mvp;

void main()
{
    gl_Position = mvp * vec4(vPosition, 1.0);
    color = vColor;
}

