Shader "Hidden/FOV_WriteStencil"
{
    SubShader
    {
        Tags { "Queue"="Geometry+10" "RenderType"="Opaque" }
        ColorMask 0
        ZWrite Off
        ZTest Always
        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }
        Pass { }
    }
}
