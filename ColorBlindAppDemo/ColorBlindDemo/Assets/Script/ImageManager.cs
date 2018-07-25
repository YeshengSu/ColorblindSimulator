using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour {

    //base
    private UIManager UIManager;
    private GameObject imageContent;
    private GameObject canvas;

    //button
    private GameObject takePhotoButton;
    private GameObject switchCameraButton;
    private GameObject deleteButton;

    //tips UI
    private GameObject SelectedIcon;

    //prefab
    private GameObject ImagePrefab;
    private GameObject TakePhotoEffectPrefab;

    //temporary
    private GameObject cameraImage;
    private GameObject SelectedImage;

    private const string folderName = "SamplePhoto";

    // Use this for initialization
    void Start () {
        //base
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        imageContent = GameObject.Find("Canvas/SampleSetting/Camera & Sample/ImageView/Viewport/Content");
        canvas = GameObject.Find("Canvas");

        //button
        takePhotoButton = GameObject.Find("Canvas/TakePhotoBut");
        switchCameraButton = GameObject.Find("Canvas/SwitchCamBut");
        deleteButton = GameObject.Find("Canvas/SampleSetting/Camera & Sample/Delete");
        takePhotoButton.GetComponent<Button>().onClick.AddListener(TakePhoto);
        deleteButton.GetComponent<Button>().onClick.AddListener(Delect);
        deleteButton.GetComponent<Button>().interactable = false;

        //prefab
        ImagePrefab = Resources.Load("Image") as GameObject;
        TakePhotoEffectPrefab = Resources.Load("TakePhotoEffect") as GameObject;
        
        //add image
        for (int i = 1; i < imageContent.transform.childCount; i++)
        {
            GameObject tempObj = imageContent.transform.GetChild(i).gameObject;
            tempObj.GetComponent<Button>().onClick.AddListener(delegate {
                OnClick(tempObj);
                deleteButton.GetComponent<Button>().interactable = false;
                takePhotoButton.SetActive(false);
                switchCameraButton.SetActive(false);
            });
        }

        if (imageContent.transform.childCount > 0)
        {
            cameraImage = imageContent.transform.GetChild(0).gameObject;
            cameraImage.GetComponent<Button>().onClick.AddListener(delegate {
                OnClick(cameraImage);
                deleteButton.GetComponent<Button>().interactable = false;
                takePhotoButton.SetActive(true);
                switchCameraButton.SetActive(true);

            });
        }

        //tips UI
        GameObject SelectedPrefab = Resources.Load("Selected") as GameObject;
        SelectedIcon = GameObject.Instantiate(SelectedPrefab, cameraImage.transform);
        SelectedIcon.GetComponent<RectTransform>().sizeDelta = cameraImage.GetComponent<RectTransform>().sizeDelta;

        SelectedImage = cameraImage;

        //read all texture in the disk
        ReadTexture();

    }

    void OnClick(GameObject clickImage){
        SelectedImage = clickImage;
        SelectedIcon.transform.SetParent(clickImage.transform);
        SelectedIcon.transform.position = clickImage.transform.position;
        UIManager.SelectTexture(SelectedImage.GetComponent<RawImage>().texture);
    }

    void Delect()
    {
        GameObject deleteImage = SelectedImage;
        OnClick(cameraImage);
        deleteButton.GetComponent<Button>().interactable = false;
        takePhotoButton.SetActive(true);
        switchCameraButton.SetActive(true);
        DeleteTextureFile(deleteImage.name);
        Destroy(deleteImage);
    }

    void TakePhoto()
    {
        StartCoroutine(TakeScreenShot());
    }

    private IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D photo;

        //create photo
        RenderTexture photoRenderTexture = Resources.Load("PhoneCamera") as RenderTexture;
        photo = new Texture2D(photoRenderTexture.width, photoRenderTexture.height, TextureFormat.RGB24, false);

        //read texture
        RenderTexture.active = photoRenderTexture;
        photo.ReadPixels(new Rect(0, 0, photoRenderTexture.width, photoRenderTexture.height), 0, 0);
        photo.Apply();
        RenderTexture.active = null;

        //save photo in the disk
        string now;
        byte[] bytes = photo.EncodeToPNG();
        string filename = ScreenShotName(out now);
        if (!System.IO.Directory.Exists(string.Format("{0}/{1}", Application.persistentDataPath, folderName)))
            System.IO.Directory.CreateDirectory(string.Format("{0}/{1}", Application.persistentDataPath, folderName));
        System.IO.File.WriteAllBytes(filename, bytes);

        //add texture
        AddTextureInContent(photo, now);

        GameObject effect = GameObject.Instantiate(TakePhotoEffectPrefab, canvas.transform );
        effect.GetComponentInChildren<RawImage>().texture = photo;
    }

    public void ReadTexture()
    {
        if (!System.IO.Directory.Exists(string.Format("{0}/{1}", Application.persistentDataPath, folderName)))
            return;
        DirectoryInfo info = new DirectoryInfo(string.Format("{0}/{1}/", Application.persistentDataPath, folderName));
        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
        foreach (FileInfo file in files)
        {
            string[] strs = file.Name.Split('.');
            if (strs[strs.Length-1] != "png")
                continue;

            Texture2D newTexture = LoadTextureToFile(file.Name);
            AddTextureInContent(newTexture, strs[0]);
        }
    }

    Texture2D LoadTextureToFile(string filename)
    {
        Texture2D load_texture;
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(string.Format("{0}/{1}/{2}", Application.persistentDataPath, folderName, filename));
        load_texture = new Texture2D(1, 1);
        load_texture.LoadImage(bytes);
        return load_texture;
    }

    void DeleteTextureFile(string filename)
    {
        File.Delete(string.Format("{0}/{1}/{2}.png", Application.persistentDataPath, folderName, filename));
    }

    void AddTextureInContent(Texture2D texture, string gameObjectName)
    {
        GameObject NewObject = GameObject.Instantiate(ImagePrefab, imageContent.transform);
        NewObject.name = gameObjectName;
        NewObject.GetComponent<Button>().onClick.AddListener(delegate {
            OnClick(NewObject);
            deleteButton.GetComponent<Button>().interactable = true;
            takePhotoButton.SetActive(false);
            switchCameraButton.SetActive(false);
        });
        NewObject.GetComponent<RawImage>().texture = texture;
    }

    public static string ScreenShotName(out string now)
    {
        now = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        return string.Format("{0}/{1}/{2}.png",
                             Application.persistentDataPath,
                             folderName,
                             now);
    }

}
