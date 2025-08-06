shader_type canvas_item;

uniform float fov : hint_range(1, 179) = 90.0;
uniform bool cull_back = true;
uniform float y_rot : hint_range(-180, 180) = 0.0;
uniform float x_rot : hint_range(-180, 180) = 0.0;
uniform float inset : hint_range(0.0, 1.0) = 0.0;

#define PI 3.14159

vec3 get_rotated_position(vec2 uv) {
	float sin_b = sin(radians(y_rot));
	float cos_b = cos(radians(y_rot));
	float sin_c = sin(radians(x_rot));
	float cos_c = cos(radians(x_rot));

	mat3 inv_rot_mat;
	inv_rot_mat[0][0] = cos_b;
	inv_rot_mat[0][1] = 0.0;
	inv_rot_mat[0][2] = -sin_b;

	inv_rot_mat[1][0] = sin_b * sin_c;
	inv_rot_mat[1][1] = cos_c;
	inv_rot_mat[1][2] = cos_b * sin_c;

	inv_rot_mat[2][0] = sin_b * cos_c;
	inv_rot_mat[2][1] = -sin_c;
	inv_rot_mat[2][2] = cos_b * cos_c;

	float t = tan(radians(fov) * 0.5);
	vec3 p = inv_rot_mat * vec3(uv - 0.5, 0.5 / t);
	float v = (0.5 / t) + 0.5;
	p.xy *= v * inv_rot_mat[2].z;
	return p;
}

void vertex() {
	vec2 offset = (UV - vec2(0.5)) / TEXTURE_PIXEL_SIZE * tan(radians(fov) * 0.5) * (1.0 - inset);
	VERTEX += offset;
}

void fragment() {
	vec3 p = get_rotated_position(UV);
	if (cull_back && p.z <= 0.0) {
		discard;
	}

	float t = tan(radians(fov) * 0.5);
	vec2 uv = (p.xy / p.z) - (0.5 / t) * vec2(sin(radians(y_rot)), cos(radians(y_rot)));
	uv += 0.5;

	COLOR = texture(TEXTURE, uv);
	COLOR.a *= step(max(abs(uv.x - 0.5), abs(uv.y - 0.5)), 0.5);
}
