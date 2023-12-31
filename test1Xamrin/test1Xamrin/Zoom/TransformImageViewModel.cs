﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace test1Xamrin.zoom
{
	public class TransformImageViewModel : TransformViewModel
	{
		//protected readonly string Path = Device.OnPlatform("images/", "", "Resources/images/");
		protected readonly string Path =
			Device.RuntimePlatform == Device.iOS ? "images/" :
			Device.RuntimePlatform == Device.Android ? "" : "Resources/images/";

		protected string[] images = new[] { "Flusi1.jpg", "Flusi2.jpg", "Flusi3.jpg" };
		//protected string[] images = new[] { "Pic1.png", "Pic2.png", "Pic3.png", "Pic4.png" };
		protected int currentImage = 0;
		public string ImageSource
		{
			get { return Path + images[currentImage]; }
		}

		protected override void OnSwiped(MR.Gestures.SwipeEventArgs e)
		{
			base.OnSwiped(e);

			if (e.Direction == MR.Gestures.Direction.Right)
			{
				currentImage--;
				if (currentImage < 0)
					currentImage = images.Length - 1;
				NotifyPropertyChanged(() => ImageSource);
			}
			else if (e.Direction == MR.Gestures.Direction.Left)
			{
				currentImage++;
				if (currentImage >= images.Length)
					currentImage = 0;
				NotifyPropertyChanged(() => ImageSource);
			}
		}



		public TransformImageViewModel()
			: base()
		{
		}
	}
}
