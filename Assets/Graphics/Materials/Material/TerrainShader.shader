 Shader "Custom/Terrain"
 {
    Properties
    {
    
     	_SliceOneColorLow ("SLiceOneLowColor", Color) = (1,1,1,0.5)
		_SliceOneColorTop ("SLiceOneTopColor", Color) = (1,1,1,0.5)
		_SliceOneFloorCount ("SliceOneFloorCount", float) = 0
		_SliceOneTexture ("Texture", 2D) = "white" {}
		_SliceTwoColorLow ("SLiceTwoLowColor", Color) = (1,1,1,0.5)
		_SliceTwoColorTop ("SLiceTwoTopColor", Color) = (1,1,1,0.5)
		_SliceTwoFloorCount ("SliceTwoFloorCount", float) = 0
		_SliceTwoTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_SliceThreeColorLow ("SLiceThreeLowColor", Color) = (1,1,1,0.5)
		_SliceThreeColorTop ("SLiceThreeTopColor", Color) = (1,1,1,0.5)
		_SliceThreeFloorCount ("SliceThreeFloorCount", float) = 0
		_SliceThreeTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_SliceFourColorLow ("SLiceFourLowColor", Color) = (1,1,1,0.5)
		_SliceFourColorTop ("SLiceFourTopColor", Color) = (1,1,1,0.5)
		_SliceFourFloorCount ("SliceFourFloorCount", float) = 0
		_SliceFourTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_SliceFiveColorLow ("SLiceFiveLowColor", Color) = (1,1,1,0.5)
		_SliceFiveColorTop ("SLiceFiveTopColor", Color) = (1,1,1,0.5)
		_SliceFiveFloorCount ("SliceFiveFloorCount", float) = 0
		_SliceFiveTexture ("Base (RGB) Alpha (A)", 2D) = "white" {}
		
		_MinHeight ("MinHeight", int) = 0
    }
    SubShader
    {
      Tags { "RenderType" = "Opaque" }
      
      CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
      #pragma vertex vert
      #pragma surface surf Lambert vertex:vert
      
		float4 _SliceOneColorLow;
		float4 _SliceOneColorTop;
		float _SliceOneFloorCount;
		sampler2D _SliceOneTexture;
		float4 _SliceOneTexture_ST;
		
		float4 _SliceTwoColorLow;
		float4 _SliceTwoColorTop;
		float _SliceTwoFloorCount;
		sampler2D _SliceTwoTexture;
		float4 _SliceTwoTexture_ST;
		
		float4 _SliceThreeColorLow;
		float4 _SliceThreeColorTop;
		float _SliceThreeFloorCount;
		sampler2D _SliceThreeTexture;
		
		float4 _SliceFourColorLow;
		float4 _SliceFourColorTop;
		float _SliceFourFloorCount;
		sampler2D _SliceFourTexture;
		
		float4 _SliceFiveColorLow;
		float4 _SliceFiveColorTop;
		float _SliceFiveFloorCount;
		sampler2D _SliceFiveTexture;
		
		
		int _MinHeight;
		
      struct Input
      {
          float2 uv_MainTex;
          float3 customColor;
          float sliceIndex;
      };
      
      void vert (inout appdata_full v, out Input o)
      {
          UNITY_INITIALIZE_OUTPUT(Input,o);
          
          float lHeight = v.vertex.y;
		
	    	float4 custColor;
	    	
	    	
	
	    	
	    	if (lHeight < _MinHeight + _SliceOneFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight) / _SliceOneFloorCount;
	    		custColor = lerp(_SliceOneColorLow,_SliceOneColorTop,clamp(lRatio, 0, 1));
	    		o.sliceIndex = 0;
	    		o.uv_MainTex = TRANSFORM_TEX(v.texcoord,_SliceOneTexture);
	    	}
	    	else if (lHeight < _MinHeight + _SliceOneFloorCount + _SliceTwoFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount) / _SliceTwoFloorCount;
	    		custColor = lerp(_SliceTwoColorLow,_SliceTwoColorTop,clamp(lRatio, 0, 1));
	    		o.sliceIndex = 1;
	    		o.uv_MainTex = TRANSFORM_TEX(v.texcoord,_SliceTwoTexture);
	    	}
	    	else if (lHeight < _MinHeight + _SliceThreeFloorCount + _SliceOneFloorCount + _SliceTwoFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount - _SliceTwoFloorCount) / _SliceThreeFloorCount;
	    		custColor = lerp(_SliceThreeColorLow,_SliceThreeColorTop,clamp(lRatio, 0, 1));
	    		o.sliceIndex = 2;
	    	}
	    	else if (lHeight < _MinHeight + _SliceFourFloorCount + _SliceThreeFloorCount + _SliceOneFloorCount + _SliceTwoFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount - _SliceTwoFloorCount - _SliceThreeFloorCount) / _SliceFourFloorCount;
	    		custColor = lerp(_SliceFourColorLow,_SliceFourColorTop,lRatio);
	    		o.sliceIndex = 3;
	    	}
	    	else //if (lHeight < _MinHeight + _SliceFiveFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight  - _SliceOneFloorCount - _SliceTwoFloorCount - _SliceThreeFloorCount - _SliceFourFloorCount) / _SliceFiveFloorCount;
	    		custColor = lerp(_SliceFiveColorLow,_SliceFiveColorTop,clamp(lRatio, 0, 1));
	    		o.sliceIndex = 4;
	    	}
	    	
	    	
	    	o.customColor = float3(custColor.r,custColor.g, custColor.b);
      }
      
      void surf (Input IN, inout SurfaceOutput o)
      {
         int index = IN.sliceIndex;
         switch(index)
         {
         	case 0: o.Albedo = tex2D (_SliceOneTexture, IN.uv_MainTex).rgb;
         	break;
         	
         	case 1: o.Albedo = tex2D (_SliceTwoTexture, IN.uv_MainTex).rgb;
         	break;
         	
         	case 2: o.Albedo = tex2D (_SliceThreeTexture, IN.uv_MainTex).rgb;
         	break;
         	
         	case 3: o.Albedo = tex2D (_SliceFourTexture, IN.uv_MainTex).rgb;
         	break;
         	
         	case 4: o.Albedo = tex2D (_SliceFiveTexture, IN.uv_MainTex).rgb;
         	break;
         	
         	default : o.Albedo = float3(1.0f,1.0f,1.0f);
         	break;
         }
          o.Albedo *= IN.customColor;
      }
      ENDCG
    } 
    Fallback "Diffuse"
    }