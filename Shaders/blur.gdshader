shader_type canvas_item;

uniform float amount : hint_range(0.0, 5.0) = 2.0;
uniform sampler2D screen_texture : hint_screen_texture, filter_linear_mipmap;

void fragment()
{
    vec4 sum = vec4(0.0);
    int blurSize = 5; // Adjust blur strength

    for (int x = -blurSize; x <= blurSize; x++)
    {
        for (int y = -blurSize; y <= blurSize; y++)
        {
            vec2 offset = vec2(float(x), float(y)) * 0.002 * amount;
            sum += textureLod(screen_texture, SCREEN_UV + offset, amount);
        }
    }

    COLOR = sum / float((blurSize * 2 + 1) * (blurSize * 2 + 1)); // Normalize blur effect
}
