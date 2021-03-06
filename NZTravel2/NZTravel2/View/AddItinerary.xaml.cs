﻿using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NZTravel2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItinerary : ContentPage
    {
        public AddItinerary(string name)
        {
            InitializeComponent();
            Entry.Placeholder = name;
            //send name, time and date to add to database
            BindingContext = new AddItineraryViewModel(Navigation, name, _timePicker.Time, datePicker.Date);
        }
    }
}