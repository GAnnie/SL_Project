Shader "Baoyu/Baoyu-Terrain-No-Light" 
{
	Properties 
	{
		_Splat0 ("Layer 1", 2D) = "white" {}
		_Splat1 ("Layer 2", 2D) = "white" {}
		_Splat2 ("Layer 3", 2D) = "white" {}
		_Splat3 ("Layer 4", 2D) = "white" {}
		_Control ("Control (RGBA)", 2D) = "white" {}
		//_MainTex ("Never Used", 2D) = "white" {}
	}
	
	
	SubShader 
	{
		Tags { 
			"RenderType"="Opaque" 
			}

		Pass
		{
			CGPROGRAM
			#pragma exclude_renderers xbox360 ps3
			#pragma vertex vert
		    #pragma fragment frag		
			//#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase		    	
			#include "UnityCG.cginc"
			
			sampler2D _Control;
			sampler2D _Splat0 ;
			sampler2D _Splat1 ;
			sampler2D _Splat2 ;
			sampler2D _Splat3 ;									
	 
	 		float4 _Control_ST;
			float4 _Splat0_ST ;
			float4 _Splat1_ST ;
			float4 _Splat2_ST ;
			float4 _Splat3_ST ;	
	 
#ifndef LIGHTMAP_OFF
			sampler2D   unity_Lightmap;
			float4   	unity_LightmapST;
#endif
				 
			struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
#ifndef LIGHTMAP_OFF
			  	float4 texcoord1: TEXCOORD1;
#endif
            };	 
				 
				 
			struct v2f 
			{
	   			float4 pos   : SV_POSITION;
				half2  uv[5] : TEXCOORD0;
#ifndef LIGHTMAP_OFF
				half2 lmapuv : TEXCOORD6;	
#endif													
			};
			
			v2f vert (VertInput v)
			{
			    v2f o;
			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

			    o.uv[0] = TRANSFORM_TEX( v.texcoord, _Splat0);
			    o.uv[1] = TRANSFORM_TEX( v.texcoord, _Splat1);
			    o.uv[2] = TRANSFORM_TEX( v.texcoord, _Splat2);
			    o.uv[3] = TRANSFORM_TEX( v.texcoord, _Splat3);			    			    
			    o.uv[4] = TRANSFORM_TEX( v.texcoord, _Control );
			    
#ifndef LIGHTMAP_OFF			    
			    o.lmapuv = ( unity_LightmapST.xy * v.texcoord1.xy ) + unity_LightmapST.zw;
#endif			    
			    return o;
			}
			
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 splat_control = tex2D (_Control, i.uv[4]).rgba;
					
				fixed3 lay1 = tex2D (_Splat0, i.uv[0]);
				fixed3 lay2 = tex2D (_Splat1, i.uv[1]);
				fixed3 lay3 = tex2D (_Splat2, i.uv[2]);
				fixed3 lay4 = tex2D (_Splat3, i.uv[3]);
				
				fixed4 o;
				o.a = 0.0;
				o.rgb = (lay1 * splat_control.r + lay2 * splat_control.g + lay3 * splat_control.b + lay4 * splat_control.a);
				
#ifndef LIGHTMAP_OFF				
				return fixed4 ( o.rgb * DecodeLightmap ( tex2D ( unity_Lightmap, i.lmapuv ) ), 1.0 );
#else				
				return o;
#endif				
			}
	
			ENDCG
		}
		

	} 
	FallBack "VertexLit"
}
