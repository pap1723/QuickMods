﻿/* 
QuickRevert
Copyright 2017 Malah

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

namespace QuickRevert {
	public partial class QDataReset {
		protected override void Awake() {
			if (HighLogic.LoadedScene != GameScenes.MAINMENU) {
				Warning ("QDataReset needs to be load on the MainMenu.", "QDataReset");
				Destroy (this);
				return;
			}
			Log ("Awake", "QDataLoad");
		}

		protected override void Start() {
			QFlight.data.Reset (true);
			Log ("Start", "QDataReset");
			Destroy (this);
		}

		protected override void OnDestroy() {
			Log ("OnDestroy", "QDataReset");
		}
	}
}