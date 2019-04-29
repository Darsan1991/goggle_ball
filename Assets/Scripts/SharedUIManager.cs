﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedUIManager : Singleton<SharedUIManager>
{

    [SerializeField] private LoadingPanel _loadingPanel;
    [SerializeField] private RatingPopUp _ratingPopUp;
    [SerializeField] private DailyRewardPanel _dailyRewardPanel;

    public static LoadingPanel LoadingPanel => Instance?._loadingPanel;
    public static RatingPopUp RatingPopUp => Instance?._ratingPopUp;
    public static DailyRewardPanel DailyRewardPanel => Instance._dailyRewardPanel;
}


