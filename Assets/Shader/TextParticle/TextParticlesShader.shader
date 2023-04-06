Shader "ACT/Text Particles"
{
    Properties
    {
        [MainTexture]_MainTex ("Texture", 2D) = "black" {}
        _Cols ("Columns Count", Int) = 5
        _Rows ("Rows Count", Int) = 4
        _Intensity ("Color Intensity", float) = 1
        _Padding ("Padding", range(0.9,1)) = 1.00
    }
    SubShader
    {            
        Tags 
        {
            "Queue"="Transparent+1"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline" 
            "UniversalMaterialType"="UnLit"
        }
        LOD 500
        ZWrite Off
        ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha
        Stencil
        {
            Comp Always
            Ref 255
            Pass Replace
        }

        Pass
        {
            Name "Draw Number"
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata
            {
                float4 vertex : POSITION;
                half4 color : COLOR;
                float4 uv : TEXCOORD0;
                // 必须是float 精度。以此为计算的也必须是float精度
                float4 customData1 : TEXCOORD1;
            };           

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 customData1 : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float _Cols;
                float _Rows;
                half _Intensity;
                float _Padding;
            CBUFFER_END
            
            v2f vert (appdata v)
            {
                v2f o;
                // 1 - 11
                float textLength = fmod(v.customData1.w, 100);//返回 x/y 的浮点余数。

                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                
                o.uv = v.uv.xy * float2(textLength, 1/_Rows);
                o.color = v.color;
                o.customData1 = v.customData1;

                return o;
            }
            
            half4 frag (v2f v) : SV_Target
            {
                float2 uv = v.uv;
                // 第n个字符
                // 从custom data中取出第2n和2n+1个数
                // xyzw分别代表6个数，一共24个
                // ind = 0~10
                half fracX = frac(uv.x);
                //return half4(ind.xxx/12,1);
                
                half x = 0;
                half y = 0;
                // dataInd = 0~3
                half dataInd = floor(uv.x / 3);
                //return half4(dataInd.xxx/3,1);
                
                float sum = v.customData1[dataInd];
                // 0-2
                half modX = fmod(uv.x,3);
                //return half4(dataInd.xxx/2,1);
                // /1 /100 /10000
                if (modX < 1)
                {
                    sum = sum / 10000;
                }
                else if (modX < 2)
                {
                    sum = sum /100;
                }
                
                y =  round(fmod(sum,10));
                sum = sum/10;
                x =  round(fmod(sum,10));
                float2 origin = float2(x/_Cols , y/_Rows);               
                uv = float2(fracX/_Cols , uv.y)+ origin;

                origin = origin + float2(0.5f/_Cols, 0.5f/ _Rows);
                uv = origin+(uv-origin)*_Padding;
                // 透明度和颜色要粒子传进来
                half4 color = _MainTex.Sample(sampler_MainTex, uv , 1);
                color *= v.color;

                // if (fracX<0.02 || fracX>0.98)
                //     color.a = 0;
                
                return half4(color.rgb * _Intensity, color.a);
            }
            ENDHLSL
        }
    }
}
