using System;
using System.Collections;
using KiiCorp.Cloud.Storage;

// Assumes Kii has been initialized and that a user has been signed in
using UnityEngine;

public class AchievementManager {

	public IEnumerator SaveAchievementData(AchievementData ad){
		// if it exists then it's an update
		Debug.Log(ad.Id + " Fetching existing AchievementData");
		KiiObject obj = FetchAchievementDataObject(ad.Id);
		if(obj == null) {
			Debug.Log(ad.Id + " Existing AchievementData not found");
			// create KiiObject
			obj = AchievementsDataBucket.NewKiiObject();

		};
		// set fields
		ad.CopyToObject(obj);
		// save to cloud
		Debug.Log(ad.Id + " Saving AchievementData");
		obj.Save((KiiObject ob, Exception e) => {
			if (e != null) {
				Debug.LogError(ad.Id + e.ToString());
			} else {
				Debug.Log(ad.Id + " AchievementData saved");
			}
		});
		yield return null;
	}

	public IEnumerator LoadAchievementData(AchievementData ad){
		KiiBucket bucket = AchievementsDataBucket;
		KiiQuery query = new KiiQuery(KiiClause.Equals(AchievementData.IdProperty, ad.Id));
		query.Limit = 1;
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>{
			if (e != null) {
				Debug.Log (ad.Id + " Failed to query");
				throw e;
			} else {
				Debug.Log (ad.Id + " Query succeeded");
				foreach (KiiObject obj in list) {
					ad.CopyFromObject(obj);
					return;
				}
			}
		});
		yield return null;
	}

	public IEnumerator SaveAchievement(Achievement ac){ 
		// if it exists then it's an update
		KiiObject obj = FetchAchievementObject(ac.Id);
		if(obj == null) {
			// create KiiObject
			obj = AchievementsBucket.NewKiiObject();
		}
		// set fields
		ac.CopyToObject(obj);
		// save to cloud
		obj.Save((KiiObject ob, Exception e) => {
			if (e != null) {
				Debug.LogError(e.ToString());
			} else {
				Debug.Log("Achievement saved");
			}
		});
		yield return null;
	}

	public IEnumerator LoadAchievement(Achievement ac){
		KiiBucket bucket = AchievementsBucket;
		KiiQuery query = new KiiQuery(KiiClause.Equals(Achievement.IdProperty, ac.Id));
		query.Limit = 1;
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>{
			if (e != null) {
				Debug.Log (ac.Id + " Failed to query");
				throw e;
			} else {
				Debug.Log (ac.Id + " Query succeeded");
				foreach (KiiObject obj in list) {
					ac.CopyFromObject(obj);
					return;
				}
			}
		});
		yield return null;
	}

	private KiiBucket AchievementsBucket {
		get {
			return CurrentUser.Bucket(Constants.ACHIEVEMENTS_BUCKET);
		}
	}

	private KiiBucket AchievementsDataBucket {
		get {
			return Kii.Bucket(Constants.ACHIEVEMENTS_DATA_BUCKET);
		}
	}

	public KiiUser CurrentUser {
		get {
			return KiiUser.CurrentUser;
		}
	}

	public KiiObject FetchAchievementDataObject(string id) {
		KiiBucket bucket = AchievementsDataBucket;
		KiiQuery query = new KiiQuery(KiiClause.Equals(AchievementData.IdProperty, id));
		query.Limit = 1;
		try {
			KiiQueryResult<KiiObject> result = bucket.Query(query);
			foreach (KiiObject obj in result) {
				return obj;
			}
			return null;
		} catch (CloudException e) {
			Debug.Log ("Failed to query " + e.ToString());
			return null;
		}
	}

	public AchievementData FetchAchievementData(string id) {
		KiiObject obj = FetchAchievementDataObject(id);
		if(obj != null) {
			return AchievementData.FromObject(obj);
		}
		return null;
	}

	public KiiObject FetchAchievementObject(string id) {
		KiiBucket bucket = AchievementsBucket;
		KiiQuery query = new KiiQuery(KiiClause.Equals(Achievement.IdProperty, id));
		query.Limit = 1;
		try {
			KiiQueryResult<KiiObject> result = bucket.Query(query);
			foreach (KiiObject obj in result) {
				return obj;
			}
			return null;
		} catch (CloudException e) {
			Debug.Log ("Failed to query " + e.ToString());
			return null;
		}
	}

	public Achievement FetchAchievement(string id) {
		KiiObject obj = FetchAchievementObject(id);
		if(obj != null) {
			return Achievement.FromObject(obj);
		}
		return null;
	}

}
