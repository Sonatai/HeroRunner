using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]

public class tintImageEffectScript : MonoBehaviour {

   [Header("Properties")]
   [SerializeField]
   private Material material;

   [SerializeField] private TimeController controller;
   
   void Start() 
   {
      if (!SystemInfo.supportsImageEffects || null == material || 
         null == material.shader || !material.shader.isSupported)
      {
         enabled = false;
         return;
      }

   }

   private void Update()
   {
      material.SetFloat("_MaxTime",controller.MaxTime);
      material.SetFloat("_TimeLeft",controller.TimeLeft);
   }

   void OnRenderImage(RenderTexture source, RenderTexture destination)
   {
      Graphics.Blit(source, destination, material);
   }
}