void ToonShading_float(in float3 Normal, in float ToonRampSmoothness, in float3 ClipSpacePos, in float3 worldPos, in float4 ToonRampTinting,
in ToonRampOffset , out float3 ToonRampOutput, out float3 Direction)
{
   #ifdef SHADERGRAPH_PREVIEW
    ToonRampOutput=float3 (0.5,0.5,0);
    Direction=float3 (0.5,0.5,0);
    #else 
        #if SHADOW_SCREEN
        half4 shadowCoord= ComputeScreenPos(ClipSpacePos);
        #else
        half4 shadowCoord = TranformWorldToShadowCoord(worldPos);
    #endif
    
    

}
