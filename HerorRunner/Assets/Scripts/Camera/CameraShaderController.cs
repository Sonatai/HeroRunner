using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]

public class CameraShaderController : MonoBehaviour
{

   public static CameraShaderController i;
   public Material material;

   public float tintEffect = 0;
   
   
   void Awake()
   {
      if (i == null)
         i = this;
   }
   
   
   void Start() 
   {
      if (!SystemInfo.supportsImageEffects || null == material || 
         null == material.shader || !material.shader.isSupported)
      {
         enabled = false;
         return;
      }
   }

   void OnRenderImage(RenderTexture source, RenderTexture destination)
   {
      material.SetFloat("_Multilayer",tintEffect);
      Graphics.Blit(source, destination, material);
   }

   public void Reset()
   {
      tintEffect = 0;
   }

   public void startTintAnimation()
   {
      Sequence s = DOTween.Sequence();
      s.Append(DOTween.To(() => tintEffect, x => tintEffect = x, 10f, 2f));
      s.Append(DOTween.To(() => tintEffect, x => tintEffect = x, 0f, 2f));
   }
}