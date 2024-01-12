using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ChaosIkaros
{
    public class Album : MonoBehaviour
    {
        public int previewWidth = 640;
        public int previewHeight = 480;
        public GameObject albumRoot;
        public GameObject photoPrefab;
        public GameObject albumParent;
        public Scrollbar albumScrollbar;
        public GameObject previewPanel;
        public RawImage previewImage;
        public Text pathText;
        public static Photo currentPhoto;
        public static bool startPreview = false;
        private string savePath;
        public List<string> fileList = new List<string>();
        private List<Photo> photoContainer = new List<Photo>();
        // Use this for initialization
        void Start()
        {
            savePath = Application.dataPath + "/Album";
#if  UNITY_ANDROID
            savePath = Application.persistentDataPath + "/Album";
#endif
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
        }
        public void InitAlbum()
        {
            LoadPhotos(savePath);
            albumScrollbar.value = 1;
        }
        public void SwitchAlbum()
        {
            albumRoot.SetActive(!albumRoot.activeSelf);
            if (albumRoot.activeSelf)
                InitAlbum();
        }
        public void LoadPhotos(string parentPath)
        {
            string[] dirs = Directory.GetFiles(parentPath);
            List<string> tempList = new List<string>();
            if (dirs.Length == 0)
                return;
            //List<Texture2D> loadedFiles = new List<Texture2D>();
            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i].Contains(".jpg") && !dirs[i].Contains(".meta"))
                {
                    tempList.Add(dirs[i]);
                }
            }
            for (int i = 0; i < tempList.Count; i++)
            {
                if (!fileList.Contains(tempList[i]))
                {
                    fileList.Add(tempList[i]);
                    GameObject newPhoto = GameObject.Instantiate(photoPrefab);
                    photoContainer.Add(newPhoto.GetComponent<Photo>());
                    newPhoto.transform.SetParent(albumParent.transform);
                    newPhoto.GetComponent<Photo>().fileName = tempList[i];
                    string[] paras = tempList[i].Replace(savePath, "").Split('_');
                    paras[0] = paras[0].Substring(1);
                    File.ReadAllBytes(tempList[i]);
                    Texture2D texture = new Texture2D(int.Parse(paras[0]), int.Parse(paras[1]), TextureFormat.RGBA32, false);
                    texture.LoadImage(File.ReadAllBytes(tempList[i]));
                    newPhoto.GetComponent<Photo>().screenImage.texture = texture;
                    newPhoto.GetComponent<Photo>().screenImage.material.mainTexture = texture;
                }
            }
        }
        public void SavePhoto(Texture2D rawPhoto)
        {
            byte[] rawBytes = rawPhoto.EncodeToJPG();
            string finalPath = savePath + "/" + rawPhoto.width + "_" + rawPhoto.height + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffffff") + ".jpg";
            FileStream fileStream = File.Open(finalPath, FileMode.OpenOrCreate);
            fileStream.Write(rawBytes, 0, rawBytes.Length);
            fileStream.Flush();
            fileStream.Close();
        }
        public void DeletePhoto()
        {
            if (currentPhoto != null)
            {
                if (currentPhoto.fileName != "")
                {
                    File.Delete(currentPhoto.fileName);
                    fileList.Remove(currentPhoto.fileName);
                }
                photoContainer.Remove(currentPhoto);
                Destroy(currentPhoto.gameObject);
                currentPhoto = null;
            }
            SwitchPreviewPanel();
        }
        public void SwitchPreviewPanel()
        {
            startPreview = false;
            previewPanel.SetActive(!previewPanel.activeSelf);
            if (previewPanel.activeSelf)
            {
                pathText.text = "File Path: " + currentPhoto.fileName;
                previewImage.texture = currentPhoto.screenImage.texture;
                previewImage.gameObject.GetComponent<RectTransform>().sizeDelta
                    = new Vector2(currentPhoto.screenImage.texture.width, currentPhoto.screenImage.texture.height);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (startPreview)
                SwitchPreviewPanel();
        }
    }
}
