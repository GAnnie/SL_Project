//$-----------------------------------------------------------------------------
//@ Lighted Shader		- Uses alpha channel for lightmapping.
//@
//# LIGHT PROBES        - YES
//# SHADOWS             - YES
//# BEAST LIGHTMAPPING  - NO
//# IGNORE PROJECTOR    - NO
//@
//@ Properties/Uniforms
//@
//# _echoUV         	- The UV offset of texture Vector4 ( u1, v1, 0, 0 ) 
//#  
//&-----------------------------------------------------------------------------
Shader "echoLogin/Light/Cutout/10-Fastest"
{
   	Properties 
	{
    	_MainTex ("Texture Alpha is cutout", 2D)		= "black" {} 
       	_echoUV("UV Offset u1 v1", Vector )				= ( 0, 0, 0, 0 )
  	}
   	
	//=========================================================================
	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="False" "RenderType"="TransparentCutout"}

		// for front face
    	Pass 
		{    
			Name "FRONT"
			Tags { "LightMode" = "ForwardBase" }
       	 	
       	 	ZWrite Off
      	 	Blend SrcAlpha OneMinusSrcAlpha
      		Cull Back
     		
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile ECHO_POINT ECHO_DIRECTIONAL ECHO_POINTANDDIRECTIONAL ECHO_LIGHTPROBE
			#pragma multi_compile_fwdbase

			sampler2D 	_MainTex;
			float4	  	_MainTex_ST;
			float4 		_MainTex_TexelSize;
			float4		_echoUV;

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			//#include "AutoLight.cginc"
			#include "EchoLogin-Light.cginc"

         	struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
			  	float3 normal   : NORMAL;
            };

           	struct Varys
            {
            	half4 pos		: SV_POSITION;
                half2 tc1		: TEXCOORD0;
			  	fixed3 dcolor	: TEXCOORD1;
//#ifndef SHADOWS_OFF			  	
//		       LIGHTING_COORDS(3,4)
//#endif
            };

			// ============================================= 	
			Varys vert ( VertInput v )
			{
				Varys o;
				float3 dcolor =  _echoAmbientLightColor;

#if ( ECHO_POINT || ECHO_POINTANDDIRECTIONAL )
				dcolor += EchoCalcLight_Point ( v.normal, v.vertex, 0 );
#endif

#if ( ECHO_DIRECTIONAL || ECHO_POINTANDDIRECTIONAL )
				dcolor += EchoCalcLight_Directional ( v.normal );
#endif

#if ECHO_LIGHTPROBE
				dcolor += ShadeSH9 ( float4 ( v.normal, 1 ) );
#endif

				o.dcolor        = dcolor;
	   			o.pos			= mul ( UNITY_MATRIX_MVP, v.vertex );
   				o.tc1 			= ( _MainTex_ST.xy * v.texcoord.xy ) + _echoUV.xy + _MainTex_ST.zw;

#if UNITY_UV_STARTS_AT_TOP
				if ( _MainTex_TexelSize.y < 0.0 )
					o.tc1.y = 1.0-o.tc1.y;
#endif

//#ifndef SHADOWS_OFF			  	
//      			TRANSFER_VERTEX_TO_FRAGMENT(o);
//#endif
    			return o;
			}

			// ============================================= 	
			fixed4 frag ( Varys v ):COLOR
			{
				fixed4 fcolor = tex2D ( _MainTex, v.tc1 );
//#ifndef SHADOWS_OFF			  	
//				fixed atten = LIGHT_ATTENUATION(v) * 2;
//				return fixed4 ( ( fcolor.xyz * v.dcolor ) * atten, fcolor.w );
//#else
				return fixed4 ( fcolor.xyz * v.dcolor, fcolor.w );
//#endif
				
			}

			ENDCG
		}

		// for back face
    	Pass 
		{    
			Name "BACK"
			Tags { "LightMode" = "ForwardBase" }
       	 	
       	 	ZWrite Off
      	 	Blend SrcAlpha OneMinusSrcAlpha
      		Cull front
     		
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile ECHO_POINT ECHO_DIRECTIONAL ECHO_POINTANDDIRECTIONAL ECHO_LIGHTPROBE
			#pragma multi_compile_fwdbase

			sampler2D 	_MainTex;
			float4	  	_MainTex_ST;
			float4 		_MainTex_TexelSize;
			float4		_echoUV;

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			//#include "AutoLight.cginc"
			#include "EchoLogin-Light.cginc"

         	struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
			  	float3 normal   : NORMAL;
            };

           	struct Varys
            {
            	half4 pos		: SV_POSITION;
                half2 tc1		: TEXCOORD0;
			  	fixed3 dcolor	: TEXCOORD1;
//#ifndef SHADOWS_OFF			  	
//		       LIGHTING_COORDS(3,4)
//#endif
            };

			// ============================================= 	
			Varys vert ( VertInput v )
			{
				Varys o;
				float3 dcolor =  _echoAmbientLightColor;

				v.normal *= -1;

#if ( ECHO_POINT || ECHO_POINTANDDIRECTIONAL )
				dcolor += EchoCalcLight_Point ( v.normal, v.vertex, 0 );
#endif

#if ( ECHO_DIRECTIONAL || ECHO_POINTANDDIRECTIONAL )
				dcolor += EchoCalcLight_Directional ( v.normal );
#endif

#if ECHO_LIGHTPROBE
				dcolor += ShadeSH9 ( float4 ( v.normal, 1 ) );
#endif

				o.dcolor        = dcolor;
	   			o.pos			= mul ( UNITY_MATRIX_MVP, v.vertex );
   				o.tc1 			= ( _MainTex_ST.xy * v.texcoord.xy ) + _echoUV.xy + _MainTex_ST.zw;

#if UNITY_UV_STARTS_AT_TOP
				if ( _MainTex_TexelSize.y < 0.0 )
					o.tc1.y = 1.0-o.tc1.y;
#endif

//#ifndef SHADOWS_OFF			  	
//      			TRANSFER_VERTEX_TO_FRAGMENT(o);
//#endif
    			return o;
			}

			// ============================================= 	
			fixed4 frag ( Varys v ):COLOR
			{
				fixed4 fcolor = tex2D ( _MainTex, v.tc1 );
//#ifndef SHADOWS_OFF			  	
//				fixed atten = LIGHT_ATTENUATION(v) * 2;
//				return fixed4 ( ( fcolor.xyz * v.dcolor ) * atten, fcolor.w );
//#else
				return fixed4 ( fcolor.xyz * v.dcolor, fcolor.w );
//#endif
				
			}

			ENDCG
		}
 	}
 	
 	Fallback "Diffuse"
	//Fallback "echoLogin/Light/Solid/Color"
}
 
