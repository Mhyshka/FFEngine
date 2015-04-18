Shader "Custom/ShaderV2" {
	Properties {
		_SliceOneColorLow ("SLiceOneLowColor", Color) = (1,1,1,0.5)
		_SliceOneColorTop ("SLiceOneTopColor", Color) = (1,1,1,0.5)
		_SliceOneFloorCount ("SliceOneFloorCount", int) = 0
		_SliceOneTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_SliceTwoColorLow ("SLiceTwoLowColor", Color) = (1,1,1,0.5)
		_SliceTwoColorTop ("SLiceTwoTopColor", Color) = (1,1,1,0.5)
		_SliceTwoFloorCount ("SliceTwoFloorCount", int) = 0
		_SliceTwoTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_SliceThreeColorLow ("SLiceThreeLowColor", Color) = (1,1,1,0.5)
		_SliceThreeColorTop ("SLiceThreeTopColor", Color) = (1,1,1,0.5)
		_SliceThreeFloorCount ("SliceThreeFloorCount", int) = 0
		_SliceThreeTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_SliceFourColorLow ("SLiceFourLowColor", Color) = (1,1,1,0.5)
		_SliceFourColorTop ("SLiceFourTopColor", Color) = (1,1,1,0.5)
		_SliceFourFloorCount ("SliceFourFloorCount", int) = 0
		_SliceFourTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_SliceFiveColorLow ("SLiceFiveLowColor", Color) = (1,1,1,0.5)
		_SliceFiveColorTop ("SLiceFiveTopColor", Color) = (1,1,1,0.5)
		_SliceFiveFloorCount ("SliceFiveFloorCount", int) = 0
		_SliceFiveTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		
		
		_MinHeight ("MinHeight", int) = 0
		_MaxHeight ("MaxHeight", int) = 100
	}
	SubShader
	{
		Pass 
		{
			CGPROGRAM
	    	#pragma vertex vert
			#pragma fragment frag
   			#include "UnityCG.cginc"

			struct v2f 
			{
				float4 pos : SV_POSITION;
				float3 color : COLOR0;
			};
			
			float4 _SliceOneColorLow;
			float4 _SliceOneColorTop;
			int _SliceOneFloorCount;
			sampler2D _SliceOneTexture;
			float4 _SliceTwoColorLow;
			float4 _SliceTwoColorTop;
			int _SliceTwoFloorCount;
			sampler2D _SliceTwoTexture;
			float4 _SliceThreeColorLow;
			float4 _SliceThreeColorTop;
			int _SliceThreeFloorCount;
			sampler2D _SliceThreeTexture;
			float4 _SliceFourColorLow;
			float4 _SliceFourColorTop;
			int _SliceFourFloorCount;
			sampler2D _SliceFourTexture;
			float4 _SliceFiveColorLow;
			float4 _SliceFiveColorTop;
			int _SliceFiveFloorCount;
			sampler2D _SliceFiveTexture;
			int _MinHeight;
			int _MaxHeight;
			
			
	    
			v2f vert (appdata_base v)
	   		{	
		        v2f o;
		    	
		    	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		    	
		    	float lHeight = v.vertex.y;
       			
		    	
		    	if (lHeight < _MinHeight + _SliceOneFloorCount)
		    	{
		    		float lRatio = (lHeight - _MinHeight) / _SliceOneFloorCount;
		    		o.color = lerp(_SliceOneColorLow,_SliceOneColorTop,lRatio);
		    	}
		    	else if (lHeight < _MinHeight + _SliceOneFloorCount + _SliceTwoFloorCount)
		    	{
		    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount) / _SliceThreeFloorCount;
		    		o.color = lerp(_SliceTwoColorLow,_SliceTwoColorTop,lRatio);
		    	}
		    	else if (lHeight < _MinHeight + _SliceThreeFloorCount + _SliceOneFloorCount + _SliceTwoFloorCount)
		    	{
		    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount - _SliceTwoFloorCount) / _SliceFourFloorCount;
		    		o.color = lerp(_SliceThreeColorLow,_SliceThreeColorTop,lRatio);
		    	}
		    	else if (lHeight < _MinHeight + _SliceFourFloorCount + _SliceThreeFloorCount + _SliceOneFloorCount + _SliceTwoFloorCount)
		    	{
		    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount - _SliceTwoFloorCount - _SliceThreeFloorCount) / _SliceFiveFloorCount;
		    		o.color = lerp(_SliceFourColorLow,_SliceFourColorTop,lRatio);
		    	}
		    	else //if (lHeight < _MinHeight + _SliceFiveFloorCount)
		    	{
		    		float lRatio = (lHeight - _MinHeight  - _SliceOneFloorCount - _SliceTwoFloorCount - _SliceThreeFloorCount - _SliceFourFloorCount) / _SliceFiveFloorCount;
		    		o.color = lerp(_SliceFiveColorLow,_SliceFiveColorTop,clamp(lRatio, 0, 1));
		    	}
		    	
		    	
		    
		        return o;
	        }
	        
	        fixed4 frag (v2f i) : COLOR
		    {
		        return fixed4 (i.color, 1);
		    }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
