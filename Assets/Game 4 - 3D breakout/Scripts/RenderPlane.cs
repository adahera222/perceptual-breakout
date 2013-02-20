using UnityEngine;
public class RenderPlane: MonoBehaviour {
private Texture2D texture;
private PXCUPipeline.Mode mode=PXCUPipeline.Mode.GESTURE;
private PXCUPipeline pp;
void Start () {
pp=new PXCUPipeline();
pp.Init(mode);
int width, height;
pp.QueryLabelMapSize(out width, out height);
texture=new Texture2D(width,height,TextureFormat.ARGB32,false);
renderer.material.mainTexture = texture;
}
void OnDisable() {
pp.Close();
}
void Update () {
if (!pp.AcquireFrame(false)) return;
if (pp.QueryLabelMapAsImage(texture)) texture.Apply();
pp.ReleaseFrame();
	}
}