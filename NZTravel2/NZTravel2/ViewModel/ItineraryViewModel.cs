﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZTravel2.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NZTravel2
{
    public class ItineraryViewModel : BaseFodyObservable
    {
        public ItineraryViewModel(INavigation navigation)
        {
            _navigation = navigation;
            GetGroupedItinerary().ContinueWith(t =>
            {
                GroupedItinerary = t.Result;
            });
            Delete = new Command<Itinerary>(HandleDelete);
            AddItem = new Command(HandleAddItem);
            EditItem = new Task<Itinerary>(HandleEditItem);
            //StartItem = new Command(HandleStartItem);
        }
        private INavigation _navigation;
        private async Task<ILookup<string, Itinerary>> GetGroupedItinerary()
        {
            return (await App.ItineraryRepository.GetList())
                                //.OrderBy(t => t.IsCompleted)
                                .ToLookup(t => t.IsCompleted ? "Completed" : "Your Itinerary");
        }

        //TODO in sprint 2
        public Command StartItem { get; set; }
        public async void HandleStartItem(Itinerary item)
        {
            //    var location = new Location(this.lat, this.longi);
            //    var options = new MapLaunchOptions
            //    {
            //        Name = item.Title
            //    };
            //    await Map.OpenAsync(location, options);
        }

        //Method to delete from the grouped list
        public Command<Itinerary> Delete { get; set; }
        public async Task HandleDelete(Itinerary itemToDelete)
        {
            await App.ItineraryRepository.DeleteItem(itemToDelete);
            // Update displayed list
            GroupedItinerary = await GetGroupedItinerary();
        }
        public Command AddItem { get; set; }
        public async void HandleAddItem()
        {
            //await _navigation.PushModalAsync(new AddItinerary());
            await _navigation.PushModalAsync(new AttractionRegionPage());
        }

        public Task<Itinerary> EditItem { get; set; }
        public async Task<Itinerary> HandleEditItem(Itinerary itemToEdit)
        {
            //Itinerary bedit = itemToEdit;
            int count = await App.ItineraryRepository.countAsync();
            int acount;
            HandleDelete(itemToEdit);
                await _navigation.PushModalAsync(new AddItinerary(itemToEdit.Title));
                acount = await App.ItineraryRepository.countAsync();
            Console.WriteLine(count+"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"+acount);
            if (count != acount)
            {
                //await App.ItineraryRepository.AddItem(itemToEdit);
                //HandleDelete(itemToEdit);
                //count = await App.ItineraryRepository.countAsync();
            }
            GroupedItinerary = await GetGroupedItinerary();
        }


        public async Task RefreshTaskList()
        {
            GroupedItinerary = await GetGroupedItinerary(); // Refreshes the itinerary
        }

        public ILookup<string, Itinerary> GroupedItinerary { get; set; }
        public string Title => "Itinerary";
        private List<Itinerary> _todoList = new List<Itinerary> { };
    }
}