//CoreFramework lighting stuff

#include "EchoLogin.cginc"

#if defined (ECHOFIXEDLIGHT)
	fixed4    _echoAmbientLightColor;
	fixed4    _echoPointLightColor;
	fixed4    _echoDirLightColor;
#else
	float4    _echoAmbientLightColor;
	float4    _echoPointLightColor;
	float4    _echoDirLightColor;
#endif


// ============================================= 	
inline float3 EchoLightDir_Point ( float4 ivertex, int index )
{
 	return normalize ( float3 ( unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index] ) - mul ( _Object2World, ivertex ).xyz );
}

// ============================================= 	
inline float3 EchoLightDir_Directional()
{
	return  _WorldSpaceLightPos0.xyz;
}

// =============================================
inline float3 EchoCalcLight_Directional ( float3 inormal )
{
	return _echoDirLightColor.xyz * max ( 0.0, dot ( EchoNormalDir ( inormal ), EchoLightDir_Directional() ) );
} 	

// =============================================
inline float3 EchoCalcLight_Directional ( float idotprod )
{
	return _echoDirLightColor.xyz * max ( 0.0, idotprod );
} 	

// =============================================
inline float3 EchoCalcLight_Point (  float3 inormal, float4 ivertex, int index )
{
	return _echoPointLightColor.xyz * max ( 0.0, dot ( EchoNormalDir ( inormal ), EchoLightDir_Point ( ivertex, index ) ) );
} 	

// =============================================
inline float3 EchoCalcLight_Point ( float idotprod, int index )
{
	return _echoPointLightColor.xyz * max ( 0.0, idotprod );
} 	

