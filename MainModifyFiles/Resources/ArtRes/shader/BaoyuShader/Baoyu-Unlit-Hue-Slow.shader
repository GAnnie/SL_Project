Shader "Baoyu/Unlit/Hue-Slow"
{
 Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "black"{}
        
        R_Vec("R_Vec", Vector) = (50,1,1,0)
        G_Vec("G_Vec", Vector) = (50,1,1,0)
        B_Vec("B_Vec", Vector) = (50,1,1,0)
    }
    SubShader {
 
        Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
 
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
 
            half4 _MainTex_ST;
 
            v2f vert (VertInput v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
 
            sampler2D _MainTex;
            sampler2D _MaskTex;
            
            half3 R_Vec;
            half3 G_Vec;
            half3 B_Vec;
            
            //T_HSV = T_RGB*T_H*T_S*T_V*T_YIQ
            fixed3 TransformHSV(fixed3 inColor, half H,half S,half V)
            {
            	half VSU = V*S*cos(radians(H));
                half VSW = V*S*sin(radians(H));
            
               	fixed3x3 T_HSV={(.299*V+.701*VSU+.168*VSW),(.587*V-.587*VSU+.330*VSW),(.114*V-.114*VSU-.497*VSW),
               			   		(.299*V-.299*VSU-.328*VSW),(.587*V+.413*VSU+.035*VSW),(.114*V-.114*VSU+.292*VSW),
               			   		(.299*V-.3*VSU+1.25*VSW),(.587*V-.588*VSU-1.05*VSW),(.114*V+.886*VSU-.203*VSW)};
               			   		
            	return mul(T_HSV,inColor);
            }
            
            fixed3 TransformHSV(fixed3 inColor, half3 shift)
            {
            	half VSU = shift.z*shift.y*cos(radians(shift.x));
                half VSW = shift.z*shift.y*sin(radians(shift.x));
            
               	fixed3x3 T_HSV={(.299*shift.z+.701*VSU+.168*VSW),(.587*shift.z-.587*VSU+.330*VSW),(.114*shift.z-.114*VSU-.497*VSW),
               			   		(.299*shift.z-.299*VSU-.328*VSW),(.587*shift.z+.413*VSU+.035*VSW),(.114*shift.z-.114*VSU+.292*VSW),
               			   		(.299*shift.z-.3*VSU+1.25*VSW),(.587*shift.z-.588*VSU-1.05*VSW),(.114*shift.z+.886*VSU-.203*VSW)};
               			   		
            	return mul(T_HSV,inColor);
            }
 
            fixed4 frag(v2f i) : COLOR
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                
                fixed3 R_Color = TransformHSV(texColor, R_Vec);
                fixed3 G_Color = TransformHSV(texColor, G_Vec);
                fixed3 B_Color = TransformHSV(texColor, B_Vec);
				
            	return fixed4(R_Color*mask.r+G_Color*mask.g+B_Color*mask.b+texColor*(1-mask.r-mask.g-mask.b),texColor.a);
//            	return fixed4(R_Color*mask.r+G_Color*mask.g+B_Color*mask.b+texColor*mask.a,texColor.a);
            }
            ENDCG
        }
    }
    Fallback "Baoyu/Unlit/Default"
 }