    Shader "Projector/Color"
    {
        Properties
        {
            _Color ("Main Colour", Color) = (1,1,1,1)
            _ShadowTex ("Cookie", 2D) = "" { TexGen ObjectLinear }
        }
     
        Subshader
        {
            Tags { "RenderType"="Transparent"  "Queue"="Transparent"}
            Pass
            {
                ZWrite Off
                Offset -1, -1
                Blend SrcAlpha OneMinusSrcAlpha
     
                SetTexture [_ShadowTex]
                {
                    constantColor [_Color]
                    combine texture * constant
                    Matrix [_Projector]
                }
            }
        }
    }