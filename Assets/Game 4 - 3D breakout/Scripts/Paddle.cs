using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

    public float moveSpeed;
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
	
	void Update () {
		if(!pp.AcquireFrame(false)) return;
		if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY,out gnode)){
        normal = new Vector3(gnode.normal.x*moveSpeed,0f,0f);
        confidence = (int)gnode.confidence;
		float moveInput = -gnode.normal.x;
		}
		//transform.position += new Vector3(moveInput, 0, 0);
		transform.Translate(-normal);

        float max = 14.0f;
        if (transform.position.x <= -max || transform.position.x >= max)
        {
            float xPos = Mathf.Clamp(transform.position.x, -max, max); //Clamp between min -5 and max 5
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
     	}
	
		pp.ReleaseFrame();
}
		

    void OnCollisionExit(Collision collisionInfo ) {
        //Add X velocity..otherwise the ball would only go up&down
        Rigidbody rigid = collisionInfo.rigidbody;
        float xDistance = rigid.position.x - transform.position.x;
        rigid.velocity = new Vector3(rigid.velocity.x + xDistance/2, rigid.velocity.y, rigid.velocity.z);
    }
}
