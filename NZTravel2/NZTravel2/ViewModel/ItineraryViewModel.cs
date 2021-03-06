﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NZTravel2.View;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NZTravel2
{
    public class ItineraryViewModel : BaseFodyObservable
    {
        public int j = -1;
        public ItineraryViewModel(INavigation navigation, int j)
        {
            _navigation = navigation;
            GetGroupedItinerary().ContinueWith(t =>
            {
                GroupedItinerary = t.Result;
            });
            this.j = j;
            l = new ObservableCollection<Itinerary>();
            Delete = new Command<Itinerary>(HandleDelete);
            AddItem = new Command(HandleAddItem);
            DetailsItem = new Command<Itinerary>(HandleDetailItem);
            ShareItinerary = new Command(HandleShare);
        }
        private INavigation _navigation;

        //gets itinerary from the Itinerary table
        private async Task<ILookup<string, Itinerary>> GetGroupedItinerary()
        {
            l.Clear();
            List<Itinerary> k = await App.ItineraryRepository.GetList();
            if (k.Count != 0)
            {
                for (int i = 0; i < k.Count; i++)
                {
                    if (k[i].ItineraryId == j)
                    {
                        l.Add(k[i]);
                    }
                }
            }
            return (await App.ItineraryRepository.GetList())
                                .ToLookup(t => t.IsCompleted ? "Completed" : "Your Itinerary");
        }

        //This function handles what happens when an item is deleted from the database
        public Command Delete { get; set; }
        public async void HandleDelete(Itinerary itemToDelete)
        {
            await App.ItineraryRepository.DeleteItem(itemToDelete);
            // Update displayed list
            GroupedItinerary = await GetGroupedItinerary();
        }

        //This function handles what happens when an item is added to the database.
        public Command AddItem { get; set; }
        public async void HandleAddItem()
        {
            //Takes the user to the attractionregion page to choose a new item to add
            await _navigation.PushModalAsync(new AttractionRegionPage()); 
        }

        //This function handles what happens when the edit button is clicked
        public Command<Itinerary> DetailsItem { get; set; }
        public async void HandleDetailItem(Itinerary itemToView)
        {
            await _navigation.PushModalAsync(new Edit(itemToView));
        }

        //Refreshes the itinerary
        public async Task RefreshTaskList()
        {
            GroupedItinerary = await GetGroupedItinerary(); 
        }

        //This function handles what happens when the Share button is clicked. 
        public Command ShareItinerary { get; set; }
        public async void HandleShare()
        {
            var Itinerary = l;
            //title of the document
            string ItineraryString = "Itinerary\r\n";
            foreach (var item in Itinerary)
            {
                if (Itinerary.Count() == 0)
                {
                    ItineraryString = "Nothing in the itinerary";
                    break;
                }
                string newString = "\r\nName: " + item.Title + "\r\nTime: " + item.time + "\r\nDate: " 
                    + item.date.ToShortDateString() + "\r\n";
                ItineraryString += newString;
            }
            //create new text file
            var Fn = "Itinerary.txt";
            var file = Path.Combine(FileSystem.CacheDirectory, Fn);
            File.WriteAllText(file, ItineraryString);
            //share the text file
            await Share.RequestAsync(new ShareFileRequest
            {
                File = new ShareFile(file)
            });
        }

        public ILookup<string, Itinerary> GroupedItinerary { get; set; }
        public string Title => "Itinerary";
        public ObservableCollection<Itinerary> l { get; set; }
    }
}