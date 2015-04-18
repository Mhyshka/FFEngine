 Shader "Custom/Terrain2"
 {
    Properties
    {
    
     	_SliceOneColorLow ("SLiceOneLowColor", Color) = (1,1,1,1)
		_SliceOneColorTop ("SLiceOneTopColor", Color) = (1,1,1,1)
		_SliceOneFloorCount ("SliceOneFloorCount", float) = 0
		_SliceOneTexture ("SliceOneTexture", 2D) = "" { }
		
		_SliceTwoColorLow ("SLiceTwoLowColor", Color) = (1,1,1,1)
		_SliceTwoColorTop ("SLiceTwoTopColor", Color) = (1,1,1,1)
		_SliceTwoFloorCount ("SliceTwoFloorCount", float) = 0
		_SliceTwoTexture ("SliceTwoTexture", 2D) = "" { }
		
		_SliceThreeColorLow ("SLiceThreeLowColor", Color) = (1,1,1,1)
		_SliceThreeColorTop ("SLiceThreeTopColor", Color) = (1,1,1,1)
		_SliceThreeFloorCount ("SliceThreeFloorCount", float) = 0
		_SliceThreeTexture ("SliceThreeTexture", 2D) = "" { }
		
		_SliceFourColorLow ("SLiceFourLowColor", Color) = (1,1,1,1)
		_SliceFourColorTop ("SLiceFourTopColor", Color) = (1,1,1,1)
		_SliceFourFloorCount ("SliceFourFloorCount", float) = 0
		_SliceFourTexture ("SliceFourTexture", 2D) = "" { }
		
		_SliceFiveColorLow ("SLiceFiveLowColor", Color) = (1,1,1,1)
		_SliceFiveColorTop ("SLiceFiveTopColor", Color) = (1,1,1,1)
		_SliceFiveFloorCount ("SliceFiveFloorCount", float) = 0
		_SliceFiveTexture ("SliceFiveTexture", 2D) = "" { }
		
		_MinHeight ("MinHeight", int) = 0
    }
    SubShader
    {
   	  //Blend DstColor Zero
      Tags { "RenderType" = "Opaque" }

		
      CGPROGRAM
      #pragma surface surf WrapLambert vertex:vert
      
                 
      	float4 _SliceOneColorLow;
		float4 _SliceOneColorTop;
		float _SliceOneFloorCount;
		sampler2D _SliceOneTexture;
		
		float4 _SliceTwoColorLow;
		float4 _SliceTwoColorTop;
		float _SliceTwoFloorCount;
		sampler2D _SliceTwoTexture;
		
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
      		float2 uv_SliceOneTexture;
      		float2 uv_SliceTwoTexture;
      		float2 uv_SliceThreeTexture;
     	 	float2 uv_SliceFourTexture;
          	float3 customColor;
          	float sliceIndex;
      };
      
      void vert (inout appdata_full v, out Input o)
      {
          UNITY_INITIALIZE_OUTPUT(Input,o);
          
          float lHeight = v.vertex.y;
          
          if(v.normal.y != 1.0f)
          {
         	 //float3 offsetToAdd = v.normal * (1.0f-v.normal.y)/10.0f;
         	 //offsetToAdd.y = 0.0f;
          	 //v.vertex += float4(offsetToAdd, 0.0f);
          	// v.vertex.y += 0.1f;
          }
		
	    	float4 custColor;

	    	if (lHeight < _MinHeight + _SliceOneFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight) / _SliceOneFloorCount;
	    		custColor = lerp(_SliceOneColorLow,_SliceOneColorTop,clamp(lRatio, 0.0f, 1.0f));
	    		o.sliceIndex = 0.0f;
	    	}
	    	else if (lHeight < _MinHeight + _SliceOneFloorCount + _SliceTwoFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount) / _SliceTwoFloorCount;
	    		custColor = lerp(_SliceTwoColorLow,_SliceTwoColorTop,clamp(lRatio, 0.0f, 1.0f));
	    		o.sliceIndex = 1.0f;
	    	}
	    	else if (lHeight < _MinHeight + _SliceOneFloorCount + _SliceTwoFloorCount + _SliceThreeFloorCount )
	    	{
	    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount - _SliceTwoFloorCount) / _SliceThreeFloorCount;
	    		custColor = lerp(_SliceThreeColorLow,_SliceThreeColorTop,clamp(lRatio, 0.0f, 1.0f));
	    		o.sliceIndex = 2.0f;
	    	}
	    	else if (lHeight < _MinHeight + _SliceFourFloorCount + _SliceThreeFloorCount + _SliceOneFloorCount + _SliceTwoFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount - _SliceTwoFloorCount - _SliceThreeFloorCount) / _SliceFourFloorCount;
	    		custColor = lerp(_SliceFourColorLow,_SliceFourColorTop,clamp(lRatio, 0.0f, 1.0f));
	    		o.sliceIndex = 3.0f;
	    	}
	    	else //if (lHeight < _MinHeight + _SliceFiveFloorCount)
	    	{
	    		float lRatio = (lHeight - _MinHeight  - _SliceOneFloorCount - _SliceTwoFloorCount - _SliceThreeFloorCount - _SliceFourFloorCount) / _SliceFiveFloorCount;
	    		custColor = lerp(_SliceFiveColorLow,_SliceFiveColorTop,clamp(lRatio, 0.0f, 1.0f));
	    		o.sliceIndex = 4.0f;
	    	}
	    	
	    	o.customColor = float3(custColor.r,custColor.g, custColor.b);
      }
      
		void surf (Input IN, inout SurfaceOutput o)
      {
     	 o.Albedo = IN.customColor;
         switch(IN.sliceIndex)
         {
         	case 0.0f: o.Albedo *= tex2D (_SliceOneTexture, IN.uv_SliceOneTexture).rgb;
         	break;
         	
	        case 1.0f: o.Albedo *= tex2D (_SliceTwoTexture, IN.uv_SliceTwoTexture).rgb;
	        break;
         	
         	case 2.0f: o.Albedo *= tex2D (_SliceThreeTexture, IN.uv_SliceThreeTexture).rgb;
         	break;
         	
         	case 3.0f: o.Albedo *= tex2D (_SliceFourTexture, IN.uv_SliceFourTexture).rgb;
         	break;
         	
         	case 4.0f: o.Albedo *= tex2D (_SliceFiveTexture, IN.uv_SliceFourTexture).rgb;
         	break;
         	
         	default : o.Albedo *= float3(1.0f,1.0f,1.0f);
         	break;
         }
         // o.Albedo = IN.customColor;
      }
      
      
	      half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten)
	      {
	        half NdotL = dot (s.Normal, lightDir);
	        half diff = NdotL * 0.5 + 0.5;
	        half4 c;
	        c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
	        c.a = s.Alpha;
	        return c;
	    }
      ENDCG
    } 
    Fallback "Diffuse"
    }