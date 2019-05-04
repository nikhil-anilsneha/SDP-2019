﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NZTravel2.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
		}

        void MapButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MapPage());
        }

        private void FuelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FuelPage());
        }

        private void AttractionButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AttractionRegionPage());
        }

        private void ItineraryButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ItineraryView());
        }
    }
}