Shader "ORCA/Standard Surface/Opaque TwoSided" 
{
	Properties 
	{
								_Color ("Color", Color)				= (1,1,1,1)
		[NoScaleOffset]			_MainTex ("Albedo (RGBA)", 2D)		= "white" {}
		[NoScaleOffset]			_Spec("Specular (ORM)", 2D)			= "green" {}
		[Normal][NoScaleOffset] _Norm("Normal Map (XY)", 2D)		= "bump" {}
		[NoScaleOffset]			_Glow("Emission (RGB)", 2D)			= "black" {}
		[HDR]					_GlowColor("Emission Color", Color) = (0.0, 0.0, 0.0, 1.0)
								_ClipValue("Clip", Range(0, 1))		= 0.25
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 400
		
		Cull Off

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alphatest:_ClipValue addshadow

		#pragma target 5.0

		uniform sampler2D	_MainTex;
		uniform sampler2D	_Spec;
		uniform sampler2D	_Norm;
		uniform sampler2D	_Glow;

		uniform half4		_Color;
		uniform half4		_GlowColor;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_Norm;
			float2 uv_Spec;
			float2 uv_Glow;
		};


		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb + (UNITY_LIGHTMODEL_AMBIENT * _Color).rgb;
			o.Alpha = c.a;

			/// My original assumption (based on a quick Google search) was that these 
			/// compressed 2-channel normals were created by dropping the Z-channel of a regular
			/// 3-channel normal. I was looking through Volumn 2 of GPU-Pro when I just happened
			/// to come across a metion of these normals in their section on blending normals. 
			/// It turns out that this isn't a simple case of just dropping the Z-channel, but 
			/// there's a bit of division of the Z-channel into the other channels in there. 
			/// Lucky for me, they had already done the math for converting from 2-channel 
			/// compressed normal over to 3-channel normal. They also mention that these 
			/// 2-channel normals have a max slope of 45 degrees.
			///
			/// - Francisco Chavez-Tejeda
			///
			half2 compressedNorm = 2.0 * tex2D(_Norm, IN.uv_Norm) - 1.0;
			half3 normDir = half3(compressedNorm.r, compressedNorm.g, 1.0);
			o.Normal = normalize(normDir);

			/// Pulling the Occulsion, Roughness, and Metallic values from the Specular Texture
			half4 orm = tex2D(_Spec, IN.uv_Spec);
			o.Occlusion = orm.x;
			o.Smoothness = 1.0 - orm.y;
			o.Metallic = orm.z;

			o.Emission = _GlowColor.xyz * (tex2D(_Glow, IN.uv_MainTex)).xyz;
		}
		ENDCG
	}
}
