﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace test1Xamrin
{
    public interface ILodingPageService
    {
        void InitLoadingPage(ContentPage loadingIndicatorPage = null);

        void ShowLoadingPage();

        void HideLoadingPage();
    }
}
