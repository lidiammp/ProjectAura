using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class EmbraceRetical : MonoBehaviour
{
    public Texture embraceRetical;
    public Texture defaultRetical;
    private RawImage retical;
    // Start is called before the first frame update
    void Start()
    {
        retical = GetComponent<RawImage>();
        SetDefaultRetical();
    }

    public void SetEmbraceRetical(){
        retical.texture = embraceRetical;
    }

    public void SetDefaultRetical(){
        retical.texture = defaultRetical;
    }

}
