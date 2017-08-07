#version 330

in vec3 vPosition;
in vec4 vColor;
in vec2 vUV;

out vec4 color;
out vec2 uv;
out int fsUseTexture0;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 mvp;

uniform int useTexture0;

void main()
{
    gl_Position = mvp * vec4(vPosition, 1.0);
	fsUseTexture0 = useTexture0;
	uv = vUV;
	color = vColor;
}
