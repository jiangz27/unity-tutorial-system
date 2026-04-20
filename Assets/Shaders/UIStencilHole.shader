Shader "Custom/UI/Stencil_Hole"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // 定义 Alpha Clip，这样如果图片是透明形状，洞也是那个形状
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        // --- 关键部分：模板测试设置 ---
        Stencil
        {
            Ref 1           // 这里的参考值是 1
            Comp Always     // 总是通过测试（不管本来是啥，我都要写）
            Pass Replace    // 测试通过后，把 Buffer 里的值替换为 Ref 值 (即变成 1)
        }

        // --- 关键部分：关闭颜色输出 ---
        ColorMask 0     // 不输出任何颜色（RGB和A都不输出），它是隐形的
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
            fixed _Cutoff;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                // 如果像素太透明，就丢弃，不要写 Stencil，这样可以支持不规则形状
                clip(col.a - _Cutoff); 
                return col;
            }
            ENDCG
        }
    }
}