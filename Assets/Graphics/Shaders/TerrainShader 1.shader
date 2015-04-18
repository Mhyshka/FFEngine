 Shader "Custom/Terrain1"
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
    //Blend One DstColor
      Tags { "RenderType" = "Opaque" }
      
      
     // pass{
      	//name "Test"
      	
        CGPROGRAM
      	#pragma surface surf Lambert vertex:vert

		float4 _SliceOneColorLow;
		float4 _SliceOneColorTop;
		float _SliceOneFloorCount;
		sampler2D _SliceOneTexture;
		
		int _MinHeight;
		
	    struct Input
	    {
	      	float2 uv_SliceOneTexture;
	        float3 customColor;
	        float render;
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
		    		//o.sliceIndex = 0.0f;
		    		o.render = 1.0f;
		    	}
		    	
		    	o.customColor = float3(custColor.r,custColor.g, custColor.b);
	      }
	      
		void surf (Input IN, inout SurfaceOutput o)
	      {
	      	if(IN.render == 1.0f)
	     	 {
	      		o.Albedo = tex2D (_SliceOneTexture, IN.uv_SliceOneTexture).rgb;

	          	o.Albedo *= IN.customColor;
	         }
	         else
	         {
	         	o.Albedo = float3(1.0f,1.0f,1.0f);
	         }
	         //o.Albedo = float3(1.0f,1.0f,1.0f);
	      }
	     ENDCG
	    // SetTexture [_SliceTwoTexture] { combine previous * texture } 
	   // }
	    
	  //  pass{
    	//name "Test2"
        CGPROGRAM
      	#pragma surface surf Lambert vertex:vert
      
		float4 _SliceTwoColorLow;
		float4 _SliceTwoColorTop;
		float _SliceOneFloorCount;
		float _SliceTwoFloorCount;
		sampler2D _SliceTwoTexture;
		
		int _MinHeight;
		
	    struct Input
	    {
	      	float2 uv_SliceTwoTexture;
	        float3 customColor;
	        float render;
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

		    	if(lHeight > _MinHeight + _SliceOneFloorCount && lHeight < _MinHeight + _SliceOneFloorCount + _SliceTwoFloorCount)
		    	{
		    		float lRatio = (lHeight - _MinHeight - _SliceOneFloorCount) / _SliceTwoFloorCount;
		    		custColor = lerp(_SliceTwoColorLow,_SliceTwoColorTop,clamp(lRatio, 0.0f, 1.0f));
		    		o.render = 1.0f;
		    	}
		    	
		    	o.customColor = float3(custColor.r,custColor.g, custColor.b);
	      }
	      
		void surf (Input IN, inout SurfaceOutput o)
	      {
	         if(IN.render == 1.0f)
	     	 {
	      		o.Albedo = tex2D (_SliceTwoTexture, IN.uv_SliceTwoTexture).rgb;
	          	o.Albedo *= IN.customColor;
	         }
	         else
	         {
	         	o.Albedo = float3(1.0f,1.0f,1.0f);
	         }
	          //o.Albedo = float3(1.0f,1.0f,1.0f);
	      }
	      
	      
	   ENDCG
	  //}
	   
	  // SetTexture [_SliceTwoTexture] { combine previous * texture, 1.0f } 
	   
    } 
    Fallback "Diffuse"
    }