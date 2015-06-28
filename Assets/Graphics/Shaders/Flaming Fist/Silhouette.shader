Shader "Flaming Fist/Silhouette" {
    Properties
    {
   		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		
        _OutlineColor ("OutlineColor", Color) = (1,0,0,1)
        _OutlineWidth ("OutlineWidth", Float ) = 0.0075
    }
    SubShader
    {
	    Tags
	    {
	    	"RenderType"="Opaque"
	    }

		Pass
        {
        	Name "Silhouette"
        	ZWrite Off
        	Cull Back
        	Tags
	        {
	            "RenderType"="Opaque"
	            "Queue" = "Geometry-100"
	        }
       
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	        #include "UnityCG.cginc"
	        #pragma target 5.0
	        
	        uniform float4 _OutlineColor;
	        uniform float _OutlineWidth;
	        struct VertexInput
	        {
	            float4 vertex : POSITION;
	            float3 normal : NORMAL;
	        };
	        
	        struct VertexOutput
	        {
	            float4 pos : SV_POSITION;
	            float3 normalDir : TEXCOORD0;
	        };
	        
	        VertexOutput vert (VertexInput v)
	        {
	            VertexOutput o = (VertexOutput)0;
	            
	            o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
	            
	            float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
	            
	            
	            float outlineSize = (sqrt(distance(_WorldSpaceCameraPos,objPos.rgb))*_OutlineWidth);
	            //outlineSize = clamp(outlineSize, outlineSize, outlineSize * 3);
	            v.vertex.rgb += (v.normal * outlineSize).rgb;
	            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	            return o;
	        }
	        
	        fixed4 frag(VertexOutput i) : COLOR
	        {
	            float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
	            i.normalDir = normalize(i.normalDir);
	/////// Vectors:
	            float3 normalDirection = i.normalDir;
	////// Lighting:
	            float3 finalColor = _OutlineColor.rgb;
	            fixed4 finalRGBA = fixed4(finalColor,1);
	            return finalRGBA;
	        }
	        ENDCG
        }

        CGPROGRAM
        #include "UnityCG.cginc"
		#pragma surface surf Standard fullforwardshadows 
		#pragma target 5.0
		
        struct VertexInput
        {
            float4 vertex : POSITION;
        };
        
        struct VertexOutput
        {
            float4 pos : SV_POSITION;
            float3 normalDir : TEXCOORD0;
            float4 color : COLOR;
        };
        
        VertexOutput vert (VertexInput v)
        {
            VertexOutput o = (VertexOutput)0;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            return o;
        }
        
        fixed4 frag(VertexOutput i) : COLOR
        {
            float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
            i.normalDir = normalize(i.normalDir);
/////// Vectors:
            float3 normalDirection = i.normalDir;
////// Lighting:
            float3 finalColor = i.color;
            fixed4 finalRGBA = fixed4(finalColor,1);
            return finalRGBA;
        }
		
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
	    ENDCG
    }
    FallBack "Diffuse"
}
