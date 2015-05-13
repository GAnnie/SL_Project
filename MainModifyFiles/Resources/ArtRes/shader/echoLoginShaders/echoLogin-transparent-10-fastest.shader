//$-----------------------------------------------------------------------------
//@ Transparent Shader	- The fastest transparent textured shader of this group.
//@
//# LIGHT PROBES        - NO
//# SHADOWS             - NO
//# BEAST LIGHTMAPPING  - YES
//# IGNORE PROJECTOR    - NO
//@
//@ Properties/Uniforms
//@
//# _echoUV         	- The UV offset of texture Vector4 ( u1, v1, 0, 0 ) 
//# _echoScale          - Scale mesh in shader
//#  
//&-----------------------------------------------------------------------------
Shader "echoLogin/Transparent/10-Fastest-nature"
{
	Properties 
   	{
		_MainTex ("Texture", 2D) 		= "gray" {}
      	_echoUV("UV Offset u1 v1", Vector )		= ( 0, 0, 0, 0 )
      	_echoScale ("Scale XYZ", Vector )		= ( 1.0, 1.0, 1.0, 1.0 )
   	}
 
	//=========================================================================
	SubShader 
	{
		Tags { "Queue"="AlphaTest+200" "IgnoreProjector"="False" "RenderType"="TransparentCutout" }

    	Pass 
		{    
      	 	ZWrite Off
      	 	Blend SrcAlpha OneMinusSrcAlpha
      		Cull OFF
     		
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"

			sampler2D	_MainTex;
			float4		_MainTex_ST;
			float4		_MainTex_TexelSize;

#ifndef LIGHTMAP_OFF
			sampler2D   unity_Lightmap;
			float4   	unity_LightmapST;
#endif

			float4      _echoUV;
			float4      _echoScale;

         	struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
#ifndef LIGHTMAP_OFF
			  	float4 texcoord1: TEXCOORD1;
#endif
            };

           	struct Varys
            {
                half4 pos		: SV_POSITION;
                half2 tc1		: TEXCOORD0;
#ifndef LIGHTMAP_OFF
                half2 tc3		: TEXCOORD3;
#endif
            };

 			//=============================================
			Varys vert ( VertInput ad )
			{
				Varys v;

    			v.pos	= mul ( UNITY_MATRIX_MVP, ad.vertex * _echoScale );
  				v.tc1 	= ( _MainTex_ST.xy * ad.texcoord.xy ) + _echoUV.xy + _MainTex_ST.zw;

#ifndef LIGHTMAP_OFF
   				v.tc3 	= ( unity_LightmapST.xy * ad.texcoord1.xy ) + unity_LightmapST.zw;
#endif

#if UNITY_UV_STARTS_AT_TOP
				if ( _MainTex_TexelSize.y < 0 )
					v.tc1.y = 1.0-v.tc1.y;
#endif
				return v;
			}
 	
 			//=============================================
			fixed4 frag ( Varys v ):COLOR
			{
#ifndef LIGHTMAP_OFF
				fixed4 fcolor = tex2D ( _MainTex, v.tc1 );
				return fixed4 ( fcolor.xyz * DecodeLightmap ( tex2D ( unity_Lightmap, v.tc3 ) ), fcolor.w );
#else
				return tex2D ( _MainTex, v.tc1 );
#endif
			}

			ENDCG
		}
 	}
 	
 	Fallback "Diffuse"
 	
}
