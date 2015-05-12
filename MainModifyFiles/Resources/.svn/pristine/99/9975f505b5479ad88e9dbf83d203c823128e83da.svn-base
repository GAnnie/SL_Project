//$-----------------------------------------------------------------------------
//@ Lighted Shader		- Solid color shader which also uses vertex color.
//@
//# LIGHT PROBES        - YES
//# SHADOWS             - YES
//# BEAST LIGHTMAPPING  - NO
//# IGNORE PROJECTOR    - NO
//@
//@ Properties/Uniforms
//@
//# _echoColor          - Object color 
//#  
//&-----------------------------------------------------------------------------
Shader "echoLogin/Light/Solid/Color"
{
   	Properties 
	{
 		_echoColor ( "Color", Color )	= ( 1, 1, 1, 1 )    
  	}

	//=========================================================================
	SubShader 
	{
		Tags { "Queue" = "Geometry" "IgnoreProjector"="False" "RenderType"="echoLight" }

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

			float4	  _echoColor;
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			//#include "AutoLight.cginc"
			#include "EchoLogin-Light.cginc"
			
         	struct VertInput
            {
                float4 vertex	 : POSITION;
                float2 texcoord	 : TEXCOORD0;
			  	float3 normal    : NORMAL;
			  	float4 color     : COLOR;
            };

           	struct Varys
            {
            	half4 pos		: SV_POSITION;
			  	fixed3 dcolor	: TEXCOORD0;
//#ifndef SHADOWS_OFF			  	
//		       LIGHTING_COORDS(2,3)
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

				o.dcolor        = dcolor * v.color * _echoColor;
	   			o.pos			= mul ( UNITY_MATRIX_MVP, v.vertex );

//#ifndef SHADOWS_OFF			  	
//      			TRANSFER_VERTEX_TO_FRAGMENT(o);
//#endif

    			return o;
			}

			// ============================================= 	
			fixed4 frag ( Varys v ):COLOR
			{
//#ifndef SHADOWS_OFF			  	
//				fixed atten = LIGHT_ATTENUATION(v) * 2;
//				return fixed4 ( v.dcolor * atten, 1 );
//#else
				return fixed4 ( v.dcolor, 1 );
//#endif
			}

			ENDCG
		}



	//=========================================================================
		Pass 
		{
			Name "ShadowCaster"
	        Tags { "LightMode" = "ShadowCaster" }
	
	        Fog {Mode Off}
			ZWrite On ZTest LEqual Cull Off
	        Offset 1, 1
	
	        CGPROGRAM
	
	        #pragma vertex vert
	        #pragma fragment frag
	        #pragma multi_compile_shadowcaster
	        #pragma fragmentoption ARB_precision_hint_fastest
	
	        #include "UnityCG.cginc"
	
	        struct v2f 
	        { 
	            V2F_SHADOW_CASTER;
			};
	
			// ============================================= 	
	        v2f vert( appdata_base v )
	        {
	            v2f o;
	            TRANSFER_SHADOW_CASTER(o)
	            return o;
	        }
	
			// ============================================= 	
	        float4 frag( v2f i ) : COLOR
	        {
	            SHADOW_CASTER_FRAGMENT(i)
	        }
	
	        ENDCG
	    }
	    
	    
	//=========================================================================
	    Pass 
	    {
	        Name "ShadowCollector"
	        Tags { "LightMode" = "ShadowCollector" }
	
	        Fog {Mode Off}
	        ZWrite On ZTest LEqual
	
	        CGPROGRAM
	
	        #pragma vertex vert
	        #pragma fragment frag
	        #pragma fragmentoption ARB_precision_hint_fastest
	        #pragma multi_compile_shadowcollector
	
	        #define SHADOW_COLLECTOR_PASS
	
	        #include "UnityCG.cginc"
	
	        struct appdata 
	        {
				float4 vertex : POSITION;
	        };
	
	        struct v2f 
	        {
				V2F_SHADOW_COLLECTOR;
	        };
	
			// ============================================= 	
	        v2f vert (appdata v)
	        {
	            v2f o;
	            TRANSFER_SHADOW_COLLECTOR(o)
	            return o;
	        }
	
			// ============================================= 	
	        fixed4 frag (v2f i) : COLOR
	        {
	            SHADOW_COLLECTOR_FRAGMENT(i)
	        }
	
	        ENDCG
    	}
 	}
}
 
