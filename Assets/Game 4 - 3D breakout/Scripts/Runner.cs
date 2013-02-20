using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {
	
private PXCUPipeline pp;
private PXCUPipeline.Mode mode=PXCUPipeline.Mode.GESTURE;
private Texture2D m_Texture;
PXCMGesture.GeoNode gnode;
byte[] labelmap;
int openness;
Vector3 worldPosition,normal;
int confidence;

bool cameraFound;
	

void Start () {
pp=new PXCUPipeline();
//shm=GetComponent<ShadowHandMod>();
int width, height;
if (!pp.Init(mode)) {
print("Unable to initialize the PXCUPipeline");
cameraFound=false;
return;
}
cameraFound=true;

if (pp.QueryLabelMapSize(out width, out height))
print("LabelMap: width=" + width + ", height=" + height);

if (width>0) {
m_Texture = new Texture2D (width, height, TextureFormat.ARGB32,false);
renderer.material.mainTexture = m_Texture;

labelmap=new byte[width*height];

//shm.ZeroImage(m_Texture);
}
}
 

void Update(){

if(!pp.AcquireFrame(false)) return;
if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY,out gnode)){
openness = (int)gnode.openness;
//worldPosition = new Vector3(gnode.positionWorld.x,-gnode.positionWorld.y,gnode.positionWorld.z);
//normal = new Vector3(gnode.normal.x,gnode.normal.y,gnode.normal.z);
normal = new Vector3(gnode.normal.x,0f,0f);
confidence = (int)gnode.confidence;
transform.Translate(-normal);
}
pp.ReleaseFrame();
}
}
