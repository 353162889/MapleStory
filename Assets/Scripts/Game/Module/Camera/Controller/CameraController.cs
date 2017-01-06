using System;
using Framework;
using UnityEngine;

namespace Game
{
	public class CameraController : BaseController<CameraController>
	{
		private UnitBase _unit;
		private CustomCameraCtl _cameraCtl;
		private CustomCameraFocusParam _focusParam;
		public override void InitController ()
		{
			_cameraCtl = new CustomCameraCtl (Camera.main);
			_focusParam = new CustomCameraFocusParam ();
			LateUpdateScheduler.Instance.AddScheduler (OnFollowUnit,0);
		}

		public void SetFocusUnit(UnitBase unit)
		{
			_unit = unit;
		}

		private void OnFollowUnit(float dt)
		{
			if (_unit != null)
			{
				_focusParam.Pos = _unit.transform.position + new Vector3 (0,0,_cameraCtl.Camera.transform.position.z);
				_focusParam.FieldOfView = _cameraCtl.Camera.fieldOfView;
				_focusParam.Focus = _unit.transform.position;
				_cameraCtl.SetCameraFocus (_focusParam);
			}
		}
	}
}

