
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapController : MonoBehaviour
{
    [System.Serializable]
    public class mapObject
    {
        public GameObject obj;
        public Color color;
        public GameObject indicator;
        public RectTransform t;
    }


    private static MapController instance;
    
    public static MapController i
    {
        get
        { 
            return instance; 
        }
    }
    
    public GameObject indicatorPrefab;

    public List<mapObject> objects;

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(this.gameObject);
        }
 
        instance = this;
    }

    public void mapFog(bool isFoggy)
    {
        foreach (mapObject o in objects)
        {
            float alpha = 1;
            if (isFoggy)
                alpha = 0;
            
            o.indicator.GetComponent<Image>().DOColor(new Color(o.color.r, o.color.g, o.color.b, alpha), 1f);
        }
    }

    void Start()
    {
        foreach (mapObject m in objects)
        {
            m.indicator = Instantiate(indicatorPrefab);
            m.indicator.transform.SetParent(transform,false);
            m.indicator.GetComponent<Image>().color = new Color(m.color.r,m.color.g,m.color.b);
            m.t = m.indicator.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (mapObject m in objects)
        { 
            float x = ExtensionMethods.remap(m.obj.transform.position.x,Globals.gridBorder[0],Globals.gridBorder[1],-450f,450f);
            float y = ExtensionMethods.remap(m.obj.transform.position.z,Globals.gridBorder[2],Globals.gridBorder[3],-450f,450f);
            m.t.localPosition = new Vector3(x,y,0);
        }
    }
    
}

public static class ExtensionMethods {
 
    public static float remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
   
}
