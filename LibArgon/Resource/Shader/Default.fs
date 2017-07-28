#version 330

in vec4 color;
in vec2 uv;
in int fsUseTexture0;

out vec4 outputColor;

uniform sampler2D Texture0;

void main()
{
    //fsUseTexture0 < 0 ? outputColor = texture(Texture0, uv) : outputColor = color;
    outputColor = color;
}
