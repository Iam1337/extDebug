Shader "extDebug/Gizmos/Line"
{
	Properties
	{
	    // Alpha
	    _Alpha ("Alpha", Range(0.0, 1.0)) = 1
        
	    // ZTest
		[Enum(extDebug.Gizmos.ZTest)] 
	    _ZTest("ZTest", Int) = 4
    }

    SubShader
	{
		Tags
	    { 
	        "Queue" = "Transparent"
	        "IgnoreProjector" = "True"
	        "RenderType" = "Transparent"
        }
	    
        Pass
		{
		    // SHADER COMMANDS
			Blend SrcAlpha OneMinusSrcAlpha
		    Cull Off
            ZTest [_ZTest] 
		    ZWrite Off

		    // SHADER CODE
            CGPROGRAM
            
	        #include "UnityCG.cginc"

            // Main Properties
	        float _Alpha;

            // Main Structures
	        struct a2f
	        {
                fixed4 colour : COLOR;
                float4 vertex : POSITION;
            };

	        struct v2f
	        {
                fixed4 colour : COLOR;
                float4 pos : POSITION;
            };

            // Main Methods
            #pragma vertex vert
	        v2f vert (a2f IN)
	        {
		        v2f o;
                o.colour = IN.colour;
                o.pos = UnityObjectToClipPos(IN.vertex);
                return o;	
	        }

            #pragma fragment frag
	        void frag (v2f i, out fixed4 colour : SV_Target)
	        {
		        colour = i.colour;
	            colour.a *= _Alpha;
	        }
			
			ENDCG
		}

	}

}