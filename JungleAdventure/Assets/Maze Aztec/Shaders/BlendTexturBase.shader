// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Maze/BlendTexturBase"
{
	Properties
	{
		_MainTex ("Texture 1", 2D) = "white" { }
		_SecTex ("Texture 2", 2D) = "white" { }
    
		_LightMap ("Lightmap", 2D) = "white" { }
		_LightIntensity ("Lightmap Intensity", Float) = 1.0
	}

	SubShader
	{
		Tags { "Queue"="Geometry" "IgnoreProjectors"="True" "RenderType"="Opaque" }
		Pass {
    
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _LightMap;
			sampler2D _SecTex;

			fixed _LightIntensity;
			fixed _SliderTexture;
 
			struct a2f 
			{
				float4 vertex : POSITION; 
				float2 uv_main : TEXCOORD0;
				float2 uv_light : TEXCOORD1;
			}; 

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				float2 uv_main : TEXCOORD0;
				float2 uv_light : TEXCOORD1;
				float2 uv_sec : TEXCOORD2;
			};

			uniform float4 _MainTex_ST; 
			uniform float4 _SecTex_ST;

			v2f vert (a2f v)
			{
				v2f o;
    
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv_light = v.uv_light;
    
				o.uv_main = TRANSFORM_TEX (v.uv_main, _MainTex);
				o.uv_sec = TRANSFORM_TEX (v.uv_main, _SecTex);
	
				return o;
			}

			fixed4 frag( v2f i ) : COLOR
			{
				float4 main = tex2D (_MainTex, i.uv_main);
				float4 sec = tex2D (_SecTex, i.uv_sec); 
	
				float4 light = tex2D (_LightMap, i.uv_light);
				float4 main2 = lerp (main , sec,  light.a);
	
				return main2 * light * _LightIntensity;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
