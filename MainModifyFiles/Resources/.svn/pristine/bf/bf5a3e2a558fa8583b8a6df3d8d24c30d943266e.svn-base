//$-----------------------------------------------------------------------------
//@ Lighted Shader		- Uses alpha channel for lightmapping.
//@
//# LIGHT PROBES        - YES
//# SHADOWS             - YES
//# BEAST LIGHTMAPPING  - NO
//# PROJECTOR           - NO
//@
//@ Properties/Uniforms
//@
//# _echoUV         	- The UV offset of texture Vector4 ( u1, v1, 0, 0 ) 
//#  
//&-----------------------------------------------------------------------------
Shader "echoLogin/Light/Cutout/12-Discard"
{
   	Properties 
	{
    	_MainTex ("Texture Alpha is cutout", 2D)		= "black" {} 
       	_echoUV ("UV Offset u1 v1", Vector )			= ( 0, 0, 0, 0 )
       	_Cutoff ("Alpha cutoff", Range(0.01,0.99)) = 0.1
       	_castLight ("Cast Light", Float) = 1.0
  	}
   	
	//=========================================================================
	SubShader 
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="False" "RenderType"="TransparentCutout"}

		// for front face
    	Pass 
		{    
			Tags { "LightMode" = "ForwardBase" }
       	 	
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
			
			fixed _Cutoff;
			fixed  _castLight;
			
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
				dcolor += EchoCalcLight_Directional ( v.normal * _castLight );
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

				clip ( fcolor.a < _Cutoff ? -1:1 );
				
//#ifndef SHADOWS_OFF			  	
//				fixed atten = LIGHT_ATTENUATION(v) * 2;
//				fcolor.xyz = ( fcolor.xyz * v.dcolor ) * atten;
//#else
				fcolor.xyz *= v.dcolor;
//#endif

				return fcolor;
				
			}

			ENDCG
		}

		// for back face
    	Pass 
		{    
			Tags { "LightMode" = "ForwardBase" }
       	 	
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

			fixed _Cutoff;
			
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

				// might have to comment this line out, depending on how models are made
				v.normal *= -1;

#if ( ECHO_POINT || ECHO_POINTANDDIRECTIONAL )
				dcolor += EchoCalcLight_Point ( v.normal, v.vertex, 0 );
#endif

#if ( ECHO_DIRECTIONAL || ECHO_POINTANDDIRECTIONAL )
				dcolor += EchoCalcLight_Directional ( v.normal * 0 );
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
				
				clip ( fcolor.a < _Cutoff ? -1:1 );

//#ifndef SHADOWS_OFF			  	
//				fixed atten = LIGHT_ATTENUATION(v) * 2;
//				fcolor.xyz = ( fcolor.xyz * v.dcolor ) * atten;
//#else
				fcolor.xyz *= v.dcolor;
//#endif

				return fcolor;
				
			}

			ENDCG
		}
 	}
 	
 	Fallback "Diffuse"
	//Fallback "echoLogin/Light/Solid/Color"
}
 
