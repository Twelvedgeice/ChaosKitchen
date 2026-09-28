Shader "KitchenChao/BuiltIn/ScrollingArrow" {
Properties { _BaseMap("Arrow Texture",2D)="white"{} _BaseColor("Color",Color)=(1,1,1,1) _Speed("Scroll Speed",Vector)=(-1,0,0,0) }
SubShader {
Tags { "Queue"="Transparent" "RenderType"="Transparent" }
LOD 200
CGPROGRAM
#pragma surface surf Standard alpha:fade
#pragma target 3.0
sampler2D _BaseMap;
fixed4 _BaseColor;
float4 _Speed;
struct Input { float2 uv_BaseMap; };
void surf(Input IN, inout SurfaceOutputStandard o) {
 fixed4 c=tex2D(_BaseMap,IN.uv_BaseMap + _Time.y*_Speed.xy)*_BaseColor;
 o.Albedo=c.rgb; o.Alpha=c.a; o.Metallic=0; o.Smoothness=0.5;
}
ENDCG
}
Fallback "Transparent/Diffuse"
}