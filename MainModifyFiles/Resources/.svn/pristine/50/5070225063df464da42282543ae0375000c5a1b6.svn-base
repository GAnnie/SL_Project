
Shader "Baoyu/Unlit/Xray"
{
	Properties 
	{
		_MainTex ("Texture", 2D)								= "black" {} 
//		_echoCutoff ("Cutoff Alpha value", Range ( 0, 1.0 ) ) 	= 0.1   
		_xrayAlpha ("Xray Alhpa value",Range(0,1.0))			= 0.4
 	}

	//=========================================================================
	SubShader 
	{
 		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" }
	
		Pass{
			ZWrite Off
			ZTest GEqual
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

		  	sampler2D	_MainTex;
			fixed 		_xrayAlpha;

           	struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
            };

           	struct Varys
            {
                half4 pos		: SV_POSITION;
                half2 tc1		: TEXCOORD0;
            };

			//=============================================
			Varys vert ( VertInput  ad )
			{
				Varys v;
    			v.pos			= mul ( UNITY_MATRIX_MVP, ad.vertex );
   				v.tc1 	  		= ad.texcoord;

				return v;
			}
 	
			//=============================================
			fixed4 frag ( Varys v ):COLOR
			{
    			fixed4 fcolor = tex2D ( _MainTex, v.tc1 );
    			fcolor.a = _xrayAlpha;
    			return ( fcolor );
			}

			ENDCG
		}
		
    	Pass 
		{    
      		Cull Off
      		ZTest LEqual
      		ZWrite On
      		Blend Off
     		
			CGPROGRAM
			
 			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D	_MainTex;
//			float		_echoCutoff;
			
           	struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
            };

           	struct Varys
            {
                half4 pos		: SV_POSITION;
                half2 tc1		: TEXCOORD0;
            };

			//=============================================
			Varys vert ( VertInput  ad )
			{
				Varys v;

    			v.pos			= mul ( UNITY_MATRIX_MVP, ad.vertex );
   				v.tc1 	  		= ad.texcoord;
				return v;
			}
 	
			//=============================================
			fixed4 frag ( Varys v ):COLOR
			{
    			fixed4 fcolor = tex2D ( _MainTex, v.tc1 );

//    			clip ( fcolor.w - _echoCutoff );
//    			fcolor.w = 1.0;

    			return ( fcolor );
			}

			ENDCG
		}
 	}
 	Fallback "Baoyu/Unlit/Default"
 }
