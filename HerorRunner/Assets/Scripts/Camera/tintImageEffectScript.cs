using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]

public class tintImageEffectScript : MonoBehaviour {

   public Material material;
   
   void Start() 
   {
      if (!SystemInfo.supportsImageEffects || null == material || 
         null == material.shader || !material.shader.isSupported)
      {
         enabled = false;
         return;
      }

      //TODO: Set value equal to the time bar
      float value = 0;
      material.SetFloat("_Multilayer",value);
   }

   void OnRenderImage(RenderTexture source, RenderTexture destination)
   {
      Graphics.Blit(source, destination, material);
   }
}