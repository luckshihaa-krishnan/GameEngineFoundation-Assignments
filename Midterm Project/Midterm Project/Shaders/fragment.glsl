#version 330 core

struct Lighting{					// structure for lighting 
	vec3 lightPos;
	vec3 lightAmbient;
	vec3 lightDiffuse;
	vec3 lightSpecular;
};

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;

out vec4 FragColor;

uniform Lighting light;				// from light structure
uniform int lightOn;				// light on or off 
uniform vec3 viewPos;
uniform vec3 objectColor;
uniform sampler2D ourTexture;
uniform int tex;

void main(){

	vec3 baseColor;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	vec3 finalCal;

	if (lightOn == 0){										// if light if off, color is black
		FragColor = vec4(0.0, 0.0, 0.0, 1.0);
		return;
	}

	if(tex == 1){											
		baseColor = texture(ourTexture, TexCoord).rgb;		// if texture is being used for the object, use texture
	}
	else{
		baseColor = objectColor;							// if texture is not used, use solid color
	}

	vec3 norm = normalize(Normal);								// Normalize normal vector
	vec3 lightDir = normalize(light.lightPos - FragPos);		// Get light direction

	float diff = max(dot(norm, lightDir), 0.0);					// Get diffuse calculation

	vec3 viewDir = normalize(viewPos - FragPos);				// Get view direction
	vec3 reflectDir = reflect(-lightDir, norm);

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);	// Specular calculation with shininess factor of 32

	ambient = light.lightAmbient * baseColor;					// Ambient calculation
	diffuse = light.lightDiffuse * diff * baseColor;			// Diffuse calculation
	specular = light.lightSpecular * spec;						// Specular calculation

	finalCal = ambient + diffuse + specular;					// Final color calculation
	FragColor = vec4(finalCal, 1.0);							// Set fragment color
	
}


