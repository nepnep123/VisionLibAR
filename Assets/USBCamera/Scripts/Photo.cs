using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosIkaros
{
    public class Photo : MonoBehaviour
    {
        public RawImage screenImage;
        public string fileName;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnPreview()
        {
            Album.currentPhoto = gameObject.GetComponent<Photo>();
            Album.startPreview = true;
        }
    }
}
