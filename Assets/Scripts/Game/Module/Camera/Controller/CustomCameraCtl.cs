using System;
using UnityEngine;
using Framework;

namespace Game
{
	public class CustomCameraPosParam
	{
		public Vector3 Pos;
		public Vector3 Rotate;
		public float FieldOfView;
	}

	public class CustomCameraFocusParam
	{
		public Vector3 Pos;
		public Vector3 Focus;
		public float FieldOfView;
	}

	public class CustomCameraCtl
	{
		public Camera Camera{get;private set;}

		public CustomCameraCtl (Camera camera)
		{
			this.Camera = camera;
		}

		public void SetCameraDirect(CustomCameraPosParam param)
		{
			Camera.transform.localPosition = param.Pos;
			Camera.transform.localRotation = Quaternion.Euler(param.Rotate);
			Camera.fieldOfView = param.FieldOfView;
		}

		public void SetCameraFocus(CustomCameraFocusParam param)
		{
			Camera.transform.localPosition = param.Pos;
			Camera.fieldOfView = param.FieldOfView;
			Camera.transform.LookAt (param.Focus);
		}
	}
}

