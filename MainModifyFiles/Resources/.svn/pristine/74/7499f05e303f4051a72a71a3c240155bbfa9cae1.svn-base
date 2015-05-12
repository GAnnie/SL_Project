Shader "Baoyu/Unlit/Hue-Fast"
{
 Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "black"{}
    }
    SubShader {
 
        Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
 
            #include "UnityCG.cginc"
            struct VertInput
            {
                float4 vertex	: POSITION;
                float2 texcoord	: TEXCOORD0;
            };
            
            struct v2f {
                half4  pos : SV_POSITION;
                half2  uv : TEXCOORD0;
            };
 
 			sampler2D _MainTex;
            half4 _MainTex_ST;
 
            v2f vert (VertInput v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
 
            sampler2D _MaskTex;
            
            uniform fixed4x4 _RHueShift;
            uniform fixed4x4 _GHueShift;
            uniform fixed4x4 _BHueShift;
 
            fixed4 frag(v2f i) : COLOR
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                
                fixed3 R_Color = mul(_RHueShift,texColor);
                fixed3 G_Color = mul(_GHueShift,texColor);
                fixed3 B_Color = mul(_BHueShift,texColor);
				
            	return fixed4(R_Color*mask.r+G_Color*mask.g+B_Color*mask.b+texColor*(1-mask.r-mask.g-mask.b),texColor.a);
//            	return fixed4(R_Color*mask.r+G_Color*mask.g+B_Color*mask.b+texColor*mask.a,texColor.a);
            }
            ENDCG
        }
    }
    Fallback "Baoyu/Unlit/Default"
 }