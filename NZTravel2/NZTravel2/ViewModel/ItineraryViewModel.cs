﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZTravel2.View;
using Xamarin.Forms;

namespace NZTravel2
{
    class ItineraryViewModel : BaseFodyObservable
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
        }
        private INavigation _navigation;
        private async Task<ILookup<string, Itinerary>> GetGroupedItinerary()
        {
            return (await App.ItineraryRepository.GetList())
                                .OrderBy(t => t.IsCompleted)
                                .ToLookup(t => t.IsCompleted ? "Completed" : "Active");
        }

        public Command<Itinerary> Delete { get; set; }
        public async void HandleDelete(Itinerary itemToDelete)
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

        public async Task RefreshTaskList()
        {
            GroupedItinerary = await GetGroupedItinerary();
        }

        public ILookup<string, Itinerary> GroupedItinerary { get; set; }
        public string Title => "Itinerary";

        private List<Itinerary> _todoList = new List<Itinerary>
        {
            new Itinerary { Id = 0, Title = "This a trial run lmao"},
            new Itinerary { Id = 1, Title = "Run a Marathon"},
            new Itinerary { Id = 2, Title = "Get good"},
        };
    }
}