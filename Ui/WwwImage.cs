using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace aus.Ui
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class WwwImage : MonoBehaviour
    {
        public string ImageUrl;
        public bool NoCache = false;
        public ResizeOptionEnum ResizeOption = ResizeOptionEnum.FIT_TO_SIZE;

        public enum ResizeOptionEnum
        {
            // texture fit to this transform
            FIT_TO_SIZE,
            PRESERVE_RATIO,
            NATIVE_SIZE,
        }

        [Header("image preset")]
        public Texture2D InitialImage;
        public Texture2D LoadingImage;
        public Texture2D FailImage;

        public void SetImageUrl(string url, bool cache = true)
        {
            NoCache = !cache;
            ImageUrl = url;
            SetLocalPath(ImageUrl);
            Refresh();
        }

        public void SetImageCache(string path)
        {
            NoCache = false;
            ImageUrl = "file://" + path;
            localFilePath = path;
            Refresh();
        }

        public bool DeleteCacheFile()
        {
            if (File.Exists(localFilePath) == false) return false;
            File.Delete(localFilePath);
            return true;
        }

        public void Refresh()
        {
            // TODO: loading, no-source or fail image support
            StartCoroutine(BeginDownload());
        }

        void Awake()
        {
            target = GetComponent<Image>();
            if (string.IsNullOrEmpty(ImageUrl) || enabled == false) return;
            SetLocalPath(ImageUrl);
        }

        void Start()
        {
            ApplyTexture(InitialImage);
            Refresh();
        }

        private Image target;
        private string localFilePath;

        private IEnumerator BeginDownload()
        {
            if (string.IsNullOrEmpty(ImageUrl)) yield break; // invalid url

            string localPath = localFilePath;

            if (File.Exists(localPath) && NoCache == false)
            {
                yield return ApplySprite();
                yield break;
            }

            ApplyTexture(LoadingImage);
            // TODO: download into temporary file to handle errors on the file
            var www = new WWW(ImageUrl);
            yield return www;
            if (string.IsNullOrEmpty(www.error) == false)
            { // failed to download
                Debug.LogError(www.error + " : " + ImageUrl);
                ApplyTexture(FailImage);
                yield break;
            }
            try
            {
                File.WriteAllBytes(localPath, www.bytes);
            }
            catch (System.Exception e)
            {
                Debug.LogError("WwwImage failed : " + e.ToString());
                ApplyTexture(FailImage);
                try
                {
                    File.Delete(localPath);
                }
                catch (System.Exception)
                {
                }
            }
            yield return ApplySprite();
        }

        private IEnumerator ApplySprite()
        {
            string localPath = localFilePath;

            var www = new WWW("file://" + localPath); // easier way to get source image size
            yield return www;
            if (string.IsNullOrEmpty(www.error) == false)
            { // failed
                Debug.LogErrorFormat("error to apply file : {0}", localPath);
                ApplyTexture(FailImage);
            }
            else
            {
                target.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.one * 0.5f);
                if (ResizeOption == ResizeOptionEnum.NATIVE_SIZE)
                {
                    target.SetNativeSize();
                }
                else if (ResizeOption == ResizeOptionEnum.PRESERVE_RATIO)
                {
                    target.preserveAspect = true;
                }
                // UNDONE: width height ratio, offset, fitting
            }

        }

        private void ApplyTexture(Texture2D texture)
        {
            if (texture == null) return;
            target.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }

        private void SetLocalPath(string url)
        {
            localFilePath = Path.Combine(Application.persistentDataPath, Hash.Sha1(url.Trim()));
        }
    }
}
