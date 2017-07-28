#version 330

in vec3 vPosition;
in vec4 vColor;
in vec2 vUV;

out vec4 color;
out vec2 uv;
out int _useTexture0;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 mvp;

uniform int useTexture0;

void main()
{
    gl_Position = mvp * vec4(vPosition, 1.0);
	_useTexture0 = useTexture0;
	if(useTexture0 > 0)
	{
		color = vec4(1, 1, 1, 1);
	}
	else
	{
		color = vColor;
	}
}
