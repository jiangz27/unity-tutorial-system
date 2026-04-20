Shader "Custom/UI/Stencil_MaskBg"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (0,0,0,0.8) // 默认半透明黑
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" // 注意：渲染队列要比 Hole 晚一点点，或者依靠层级顺序
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        // --- 关键部分：模板测试设置 ---
        Stencil
        {
            Ref 1           // 参考值 1
            Comp NotEqual   // 比较规则：只有当 Buffer 里的值“不等于”1 时，才渲染
            Pass Keep       // 渲染完保持原样
        }

        ColorMask RGBA  // 正常输出颜色
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return tex2D(_MainTex, i.texcoord) * i.color;
            }
            ENDCG
        }
    }
}