Shader "Flaming Fist/HardNormal" {
    Properties
    {	
        _Color ("Color", Color) = (1,0,0,1)
    }
    SubShader
    {
    	Tags
	    {
	    	"RenderType"="Opaque"
	    }
	    
	    Pass
        {
        	Name "HardNormal"
        	ZWrite On
        	Cull Back
        	//Lighting Off
       
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	        #include "UnityCG.cginc"
	        #include "UnityShaderVariables.cginc"
	        #pragma target 5.0
	        
	        uniform float4 _Color;
	        float _glow;
	        struct VertexInput
	        {
	            float4 vertex : POSITION;
	            float3 normal : NORMAL;
	            float4 tangent : TANGENT;
	        };
	        
	        struct VertexOutput
	        {
	            float4 pos : SV_POSITION;
	            float3 toward : Direction;
	        };
	        
	        VertexOutput vert (VertexInput v)
	        {
	        	_glow = cos(_Time*150)/3;
	            VertexOutput o = (VertexOutput)0;
	            
	            float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
	            float3 direction = normalize(_WorldSpaceCameraPos - objPos);
	            o.toward = (dot(UNITY_MATRIX_IT_MV[2].xyz, v.normal) + 0.5) / 1.5;// - 1/3 + _glow;
	            
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);

	            return o;
	        }
	        
	        fixed4 frag(VertexOutput i) : COLOR
	        {
	            float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
	/////// Vectors:
	////// Lighting:
	            float3 finalColor = _Color.rgb * i.toward;
	            fixed4 finalRGBA = fixed4(finalColor,1);
	            return finalRGBA;
	        }
	        ENDCG
        }
    }
    FallBack "Diffuse"
}