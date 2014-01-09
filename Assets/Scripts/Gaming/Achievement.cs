using System;
using KiiCorp.Cloud.Storage;
using System.Collections;
using UnityEngine;

public class Achievement {

	internal string mDataId;
	// By default achievements will be visible. Change to CHIEVEMENT_STATE_HIDDEN if you want to start in hidden mode
	internal int mState = Constants.ACHIEVEMENT_STATE_REVEALED;
	internal int mCurrentSteps = 0;
	internal KiiObject mRawObject = null;
	internal AchievementManager mManager = new AchievementManager();
	internal AchievementData mAchievementData = null;

	public Achievement(string dataId) {
		mDataId = dataId;
	}

	public string Id {
		// id must match associated AchievementData id
		get {
			return mDataId;
		}
		set {
			mDataId = value;
		}
	}

	public static string IdProperty {
		get {
			return "Id";
		}
	}
	
	public int State {
		get {
			return mState;
		}
		set {
			mState = value;
		}
	}

	public static string StateProperty {
		get {
			return "State";
		}
	}

	public int CurrentSteps {
		get {
			return mCurrentSteps;
		}
		set {
			mCurrentSteps = value;
		}
	}

	public static string CurrentStepsProperty {
		get {
			return "CurrentSteps";
		}
	}

	public long LastSaved {
		get {
			if(RawObject != null)
				return RawObject.ModifedTime;
			else
				return 0;
		}
	}

	private AchievementManager Manager {
		get {
			return mManager;
		}
	}

	public KiiObject RawObject {
		get {
			if(mRawObject == null)
				mRawObject = Manager.FetchAchievementObject(Id);
			return mRawObject;
		}
	}

	public AchievementData AchievementData {
		// fetch achievement data by id with manager
		get {
			if(mDataId == null)
				return null;
			if(mAchievementData == null) {
				LoadAchievementData();
			}
			return mAchievementData;
		}
	}

	public void LoadAchievementData(){
		mAchievementData = Manager.FetchAchievementData(mDataId);
	}

	public bool IsCompleted() {
		return AchievementData.TotalSteps == mCurrentSteps;
	}

	public bool IsIncremental() {
		return AchievementData.IsIncremental();
	}

	public bool IsHidden() {
		return State == Constants.ACHIEVEMENT_STATE_HIDDEN;
	}

	public bool IsRevealed() {
		return State == Constants.ACHIEVEMENT_STATE_REVEALED;
	}

	public bool IsUnlocked() {
		return State == Constants.ACHIEVEMENT_STATE_UNLOCKED;
	}

	public double PercentCompleted() {
		if (!IsIncremental ())
			throw new Exception ("Not supported in achievements that are not incremental");
		return (CurrentSteps * 100.0) / AchievementData.TotalSteps;
	}

	public void Save() {
		Manager.SaveAchievement(this);
	}

	public void Load() {
		Manager.LoadAchievement(this);
	}

	public void Unlock() {
		State = Constants.ACHIEVEMENT_STATE_UNLOCKED;
	}

	public void Reveal() {
		State = Constants.ACHIEVEMENT_STATE_REVEALED;
	}

	public void Hide() {
		State = Constants.ACHIEVEMENT_STATE_HIDDEN;
	}

	public void Reset() {
		CurrentSteps = 0;
	}

	public int Increment() {
		return Increment (1);
	}

	public int Increment(int numSteps) {
		if (!IsIncremental())
			throw new Exception ("Not supported in achievements that are not incremental");
		int i = numSteps;
		while (!IsCompleted() && i != 0) {;
			CurrentSteps++;
			i--;
		}
		if(IsCompleted())
			Unlock();
		return CurrentSteps;
	}

	public static Achievement FromObject(KiiObject obj) {
		Achievement ac = new Achievement("");
		ac.CopyFromObject(obj);
		return ac;
	}
	
	public void CopyFromObject(KiiObject obj) {
		if(obj.Has(IdProperty))
			Id = obj.GetString(IdProperty);
		if(obj.Has(StateProperty))
			State = obj.GetInt(StateProperty);
		if(obj.Has(CurrentStepsProperty)){
			CurrentSteps = obj.GetInt(CurrentStepsProperty);
		}
		mRawObject = obj;
	}
	
	public void CopyToObject(KiiObject obj) {
		obj[IdProperty] = Id;
		obj[StateProperty] = State;
		obj[CurrentStepsProperty] = CurrentSteps;
		mRawObject = obj;
	}
	
}