using System;
using KiiCorp.Cloud.Storage;
using System.Collections;

public class AchievementData {
	
	internal string mId;
	internal string mName;
	internal int mType = Constants.ACHIEVEMENT_TYPE_STANDARD;
	internal string mDescription;
	internal int mTotalSteps = 1;
	internal Uri mRevealedImageUri;
	internal Uri mUnlockedImageUri;
	internal KiiObject mRawObject = null;
	internal AchievementManager mManager = new AchievementManager();

	public AchievementData(string id) {
		mId = id;
	}

	public AchievementData(string id, string name) {
		mId = id;
		mName = name;
	}

	public AchievementData(string id, string name, int steps) {
		mId = id;
		mName = name;
		mType = Constants.ACHIEVEMENT_TYPE_INCREMENTAL;
		mTotalSteps = steps;
	}
	
	public string Id {
		get {
			return mId;
		}
		set {
			mId = value;
		}
	}

	public static string IdProperty {
		get {
			return "Id";
		}
	}
	
	public string Name {
		get {
			return mName;
		}
		set {
			mName = value;
		}
	}

	public static string NameProperty {
		get {
			return "Name";
		}
	}

	public string Description {
		get {
			return mDescription;
		}
		set {
			mDescription = value;
		}
	}

	public static string DescriptionProperty {
		get {
			return "Description";
		}
	}
	
	public int Type {
		get {
			return mType;
		}
		set {
			mType = value;
		}
	}

	public static string TypeProperty {
		get {
			return "Type";
		}
	}
	
	public int TotalSteps {
		get {
			if(!IsIncremental())
				return 1;
			return mTotalSteps;
		}
		set {
			mTotalSteps = value;
		}
	}

	public static string TotalStepsProperty {
		get {
			return "TotalSteps";
		}
	}	

	public Uri RevealedImageUri {
		get {
			return mRevealedImageUri;
		}
		set {
			mRevealedImageUri = value;
		}
	}

	public static string RevealedImageUriProperty {
		get {
			return "RevealedImageUri";
		}
	}
	
	public Uri UnlockedImageUri {
		get {
			return mUnlockedImageUri;
		}
		set {
			mUnlockedImageUri = value;
		}
	}

	public static string UnlockedImageUriProperty {
		get {
			return "UnlockedImageUri";
		}
	}

	private AchievementManager Manager {
		get {
			return mManager;
		}
	}

	public KiiObject RawObject {
		get {
			return mRawObject;
		}
	}
	
	public bool IsIncremental() {
		return Type == Constants.ACHIEVEMENT_TYPE_INCREMENTAL;
	}
	
	public void SetIncremental(int totalSteps) {
		Type = Constants.ACHIEVEMENT_TYPE_INCREMENTAL;
		TotalSteps = totalSteps;
	}

	public IEnumerator Save() {
		return Manager.SaveAchievementData(this);
	}

	public IEnumerator Load() {
		return Manager.LoadAchievementData(this);
	}

	public static AchievementData FromObject(KiiObject obj) {
		AchievementData ad = new AchievementData("");
		ad.CopyFromObject(obj);
		return ad;
	}

	public void CopyFromObject(KiiObject obj) {
		if(obj.Has(IdProperty))
			Id = obj.GetString(IdProperty);
		if(obj.Has(NameProperty))
			Name = obj.GetString(NameProperty);
		if(obj.Has(DescriptionProperty))
			Description = obj.GetString(DescriptionProperty);
		if(obj.Has(TypeProperty))
			Type = obj.GetInt(TypeProperty);
		if(obj.Has(TotalStepsProperty))
			TotalSteps = obj.GetInt(TotalStepsProperty);
		if(obj.Has(RevealedImageUriProperty))
			RevealedImageUri = obj.GetUri(RevealedImageUriProperty);
		if(obj.Has(UnlockedImageUriProperty))
			UnlockedImageUri = obj.GetUri(UnlockedImageUriProperty);
		mRawObject = obj;
	}

	public void CopyToObject(KiiObject obj) {
		obj[IdProperty] = Id;
		obj[NameProperty] = Name;
		obj[DescriptionProperty] = Description;
		obj[TypeProperty] = Type;
		obj[TotalStepsProperty] = TotalSteps;
		obj[RevealedImageUriProperty] = RevealedImageUri;
		obj[UnlockedImageUriProperty] = UnlockedImageUri;
		mRawObject = obj;
	}
	
}
