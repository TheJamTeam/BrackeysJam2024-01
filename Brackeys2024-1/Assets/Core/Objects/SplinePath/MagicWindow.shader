Shader "Unlit/MagicWindow"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue"="Geometry-1" }
        LOD 100
        ZWrite On
        ZTest Always

        Blend Zero One

        Pass
        {

        }
    }
}