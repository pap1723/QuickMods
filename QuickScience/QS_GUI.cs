﻿/* 
QuickSAS
Copyright 2016 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using UnityEngine;

namespace QuickSAS {
	public partial class QGUI {
		public static QGUI Instance {
			get;
			private set;
		}

		internal bool WindowSettings = false;

		Rect rectSettings = new Rect();
		Rect RectSettings {
			get {
				rectSettings.x = (Screen.width - rectSettings.width) / 2;
				rectSettings.y = (Screen.height - rectSettings.height) / 2;
				return rectSettings;
			}
			set {
				rectSettings = value;
			}
		}

		Rect rectSetKey = new Rect ();
		Rect RectSetKey {
			get {
				rectSetKey.x = (Screen.width - rectSetKey.width) / 2;
				rectSetKey.y = (Screen.height - rectSetKey.height) / 2;
				return rectSetKey;
			}
			set {
				rectSetKey = value;
			}
		} 

		internal QBlizzyToolbar BlizzyToolbar;
			
		protected override void Awake () {
			if (!HighLogic.LoadedSceneIsGame || QGUI.Instance != null) {
				Destroy (this);
			}
			Instance = this;
			if (BlizzyToolbar == null) {
				BlizzyToolbar = new QBlizzyToolbar ();
			}
			Log ("Awake", "QGUI");
		}

		protected override void Start () {
			BlizzyToolbar.Start ();
			Log ("Start", "QGUI");
		}

		protected override void OnDestroy () {
			BlizzyToolbar.OnDestroy ();
			Log ("OnDestroy", "QGUI");
		}

		void Lock(bool activate, ControlTypes Ctrl) {
			if (HighLogic.LoadedSceneIsFlight) {
				FlightDriver.SetPause (activate);
				if (activate) {
					InputLockManager.SetControlLock (ControlTypes.CAMERACONTROLS | ControlTypes.MAP, "Lock" + MOD);
					return;
				}
			}
			else if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					EditorLogic.fetch.Lock (true, true, true, "EditorLock" + MOD);
					return;
				}
				else {
					EditorLogic.fetch.Unlock ("EditorLock" + MOD);
				}
			}
			if (activate) {
				InputLockManager.SetControlLock (Ctrl, "Lock" + MOD);
				return;
			}
			else {
				InputLockManager.RemoveControlLock ("Lock" + MOD);
			}
			if (InputLockManager.GetControlLock ("Lock" + MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("Lock" + MOD);
			}
			if (InputLockManager.GetControlLock ("EditorLock" + MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("EditorLock" + MOD);
			}
			Log ("Lock " + activate, "QExit");
		}

		public void Settings () {
			if (WindowSettings) {
				HideSettings ();
			}
			else {
				ShowSettings ();
			}
			Log ("Settings", "QGUI");
		}

		internal void ShowSettings () {
			WindowSettings = true;
			Switch (true);
			Log ("ShowSettings", "QGUI");
		}
			
		internal void HideSettings () {
			WindowSettings = false;
			Switch (false);
			Save ();
			Log ("HideSettings", "QGUI");
		}

		internal void Switch (bool set)	{
			QStockToolbar.Instance.Set (WindowSettings, false);
			Lock (WindowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
			Log ("Switch: " + set, "QGUI");
		}

		void Save () {
			QStockToolbar.Instance.Reset ();
			BlizzyToolbar.Reset ();
			QSettings.Instance.Save ();
			Log ("Save", "QGUI");
		}

		void Update() {
			if (QKey.SetKey == QKey.Key.None) {
				return;
			}
			if (Event.current.isKey) {
				KeyCode _key = Event.current.keyCode;
				if (_key != KeyCode.None) {
					QKey.SetCurrentKey (QKey.SetKey, _key);
					QKey.SetKey = QKey.Key.None;
				}
			}
		}

		void OnGUI () {
			if (!WindowSettings) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			if (QKey.SetKey != QKey.Key.None) {
				RectSetKey = GUILayout.Window (1545156, RectSetKey, QKey.DrawSetKey, string.Format ("{0} {1}", QLang.translate ("Set Key:"), QKey.GetText (QKey.SetKey)), GUILayout.ExpandHeight (true));
				return;
			}
			RectSettings = GUILayout.Window (1545175, RectSettings, DrawSettings, MOD + " " + VERSION);
		}

		void DrawSettings (int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Toolbars"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, QLang.translate ("Use the Stock Toolbar"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			if (QBlizzyToolbar.isAvailable) {
				GUILayout.BeginHorizontal ();
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, QLang.translate ("Use the Blizzy Toolbar"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();
			}
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.WarpToEnhanced = GUILayout.Toggle (QSettings.Instance.WarpToEnhanced, QLang.translate ("Warp to 15 seconds before the ½ of the burn time"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Keyboard shortcuts"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			QKey.DrawConfigKey (QKey.Key.Current);
			QKey.DrawConfigKey (QKey.Key.Prograde);
			QKey.DrawConfigKey (QKey.Key.Retrograde);
			QKey.DrawConfigKey (QKey.Key.Normal);
			QKey.DrawConfigKey (QKey.Key.AntiNormal);
			QKey.DrawConfigKey (QKey.Key.RadialIn);
			QKey.DrawConfigKey (QKey.Key.RadialOut);
			QKey.DrawConfigKey (QKey.Key.TargetPrograde);
			QKey.DrawConfigKey (QKey.Key.TargetRetrograde);
			QKey.DrawConfigKey (QKey.Key.Maneuver);
			QKey.DrawConfigKey (QKey.Key.WarpToNode);
			QLang.DrawLang ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (QLang.translate ("Close"), GUILayout.Height (30))) {
				HideSettings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}
	}
}