using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Waterfall_Scroll : MonoBehaviour
{
    public Vector2 offset = new Vector2(0,0.5f);
    Renderer m_renderer;
    // Start is called before the first frame update
    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_renderer.material.mainTextureOffset += offset * Time.deltaTime;
    }
}
