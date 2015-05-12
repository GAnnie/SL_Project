// CORE Framework include file

// =============================================
inline float3 EchoNormalDir ( float3 ivertex )
{
//	return mul ( ivertex , float3x3( _World2Object[0].xyz, _World2Object[1].xyz, _World2Object[2].xyz) );
	return float3 ( mul ( ivertex , (float3x3)_World2Object ) );
}

// =============================================
inline float4 EchoVertexPos ( float4 ivertexpos )
{
	return mul ( _Object2World, ivertexpos );
}

// =============================================
inline float3 EchoObjViewDir ( float4 ivertexpos )
{
	return normalize ( ObjSpaceViewDir ( ivertexpos ) );
}

// =============================================
inline float3 EchoWorldViewDir ( float3 ivertexpos )
{
	return normalize ( _WorldSpaceCameraPos - ivertexpos );
}

// =============================================
inline float3 EchoSpecular ( float3 icolor, float ishine, float3 ilightdir, float3 inormaldir, float3 iviewdir )
{
	return icolor * pow ( max ( 0.0, dot ( reflect ( -ilightdir, inormaldir ), iviewdir ) ), float3 ( ishine, ishine, ishine ) ) ;
}

// =============================================
inline float EchoShieldHit ( in float4 ihitvec, in float ihitmix, float3 inormaldir )
{
	// passed in uniforms used in IF should only be checked once per drawcall on ios ( dont know about android )
	if ( ihitvec.w > 0.0 )
	{
		float dotprod = dot ( inormaldir, normalize ( ihitvec.xyz ) );
		
		if ( dotprod <= ihitmix )
			return clamp ( ( ihitmix - dotprod ) * 8.0, 0.0, 1.0 );
	}
	
	return ( 0.0 );
}

// =============================================
inline float EchoWave ( float itexu, float iamount, float ispeed )
{
	return  ( itexu * sin ( itexu * iamount - ( _Time * ispeed ) ) );
}

// =============================================
inline float EchoRipple ( float2 itexuv, float iamount, float ispeed, float iheight, float icenterx, float icentery )
{
	itexuv.x += icenterx;
	itexuv.y += icentery;

	return sin ( length ( itexuv ) * iamount - ispeed * _Time ) * iheight;
}

// =============================================
inline float3 EchoReflect ( float4 ivertex, float3 ivnormal )
{
	float3 reflection = reflect ( normalize ( mul ( UNITY_MATRIX_MV , ivertex ) ), float3 ( normalize ( mul ( (float3x3)UNITY_MATRIX_MV , ivnormal ) ) ) );
	
	reflection.z += 1.0;
	
	return reflection;
}

// =============================================
inline float echoRand ( float2 inv )
{
	return (sin(dot(inv.xy ,float2(12.9898,78.233))) * 43758.5453);
}
