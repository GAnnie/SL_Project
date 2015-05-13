
Shader "Baoyu/Unlit/Default"
{
	Properties 
	{
		_MainTex ("Texture", 2D) = "black" {} 
 	}

	//=========================================================================
	SubShader 
	{
		Tags { "Queue" = "Geometry" "IgnoreProjector"="True" "RenderType"="Opaque" }

    	Pass 
		{    
      		Cull Back
     		
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
//			#pragma fragmentoption ARB_precision_hint_fastest
//			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"

			sampler2D	_MainTex;
			float4		_MainTex_ST;

//#ifndef LIGHTMAP_OFF
//			sampler2D   unity_Lightmap;
//			float4   	unity_LightmapST;
//#endif

           	struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
//#ifndef LIGHTMAP_OFF
//			  	float4 texcoord1: TEXCOORD1;
//#endif
            };

           	struct Varys
            {
                half4 pos		: SV_POSITION;
                half2 tc1		: TEXCOORD0;
//#ifndef LIGHTMAP_OFF
//                half2 tc3		: TEXCOORD3;
//#endif
            };

			// ============================================= 	
			Varys vert ( VertInput  ad )
			{
				Varys v;

    			v.pos			= mul ( UNITY_MATRIX_MVP, ad.vertex );
   				v.tc1 	  		= TRANSFORM_TEX(ad.texcoord,_MainTex);
 
//#ifndef LIGHTMAP_OFF
//   				v.tc3 	  		= ( unity_LightmapST.xy * ad.texcoord1.xy ) + unity_LightmapST.zw;
//#endif
				return v;
			}
 	
			// ============================================= 	
			fixed4 frag ( Varys v ):COLOR
			{
//#ifndef LIGHTMAP_OFF
//			  	return   (fixed4 ( tex2D ( _MainTex, v.tc1 ).xyz * DecodeLightmap ( tex2D ( unity_Lightmap, v.tc3 ) ), 1.0 ));
//#else
    			return   (tex2D ( _MainTex, v.tc1 ));
//#endif
			}

			ENDCG
		}
 	}
 	
 	Fallback "Diffuse"
 }
