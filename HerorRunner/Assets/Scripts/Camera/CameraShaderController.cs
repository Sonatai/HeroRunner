using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]

public class CameraShaderController : MonoBehaviour
{

   public static CameraShaderController i;
   [Header("Properties")]
   [SerializeField]
   private Material material;

   [SerializeField] private TimeController controller;

   public float tintEffect = 0;

   public Camera camera;
   private bool isPumping = false;
   public float threshhold = 0.01f;
   public int frequencyRange = 4;

   public AudioSource clickTrack;
   


   void Awake()
   {
      if (i == null)
         i = this;
   }
   private void Update()
   {
      material.SetFloat("_MaxTime",controller.MaxTime);
      material.SetFloat("_TimeLeft",controller.TimeLeft);
      getLoudness();

   }

   private void getLoudness()
   {
      float[] spectrum = new float[64];
      clickTrack.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
      float bass = spectrum[40];
      if(bass > threshhold)
         cameraPump();
   }

   void cameraPump()
   {
      if (!isPumping)
      {
         isPumping = true;
         Sequence pumpSequence = DOTween.Sequence();
         pumpSequence.Append(camera.DOFieldOfView(60, 0.05f));
         pumpSequence.Append(camera.DOFieldOfView(70, 0.15f)); //150bpm song
         pumpSequence.AppendCallback(delegate { isPumping = false; });
      }
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
      Graphics.Blit(source, destination, material);
   }

   public void Reset()
   {
      tintEffect = 0;
      isPumping = false;
   }

   public void startTintAnimation()
   {
      Sequence s = DOTween.Sequence();
      s.Append(DOTween.To(() => tintEffect, x => tintEffect = x, 10f, 2f));
      s.Append(DOTween.To(() => tintEffect, x => tintEffect = x, 0f, 2f));
   }
}