using BlazorMovies.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static BlazorMovies.Client.Shared.MainLayout;

namespace BlazorMovies.Client.Pages
{
    public partial class Counter
    {
        [Inject] SingletonService singleton { get; set; }
        [Inject] TransientService transient { get; set; }
        [Inject] IJSRuntime js { get; set; }

        [CascadingParameter]
        public AppState AppState { get; set; }


        private int currentCount = 0;
        private static int staticCurrentCount = 0;
        IJSObjectReference module;

        [JSInvokable]
        public async Task IncrementCount()
        {
            module = await js.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");

            await module.InvokeVoidAsync("displayAlert", "Hello world!");

            currentCount++;
            staticCurrentCount++;
            singleton.Value++;
            transient.Value++;
            await js.InvokeVoidAsync("dotnetStaticInvocation");
        }

        private async Task IncrementCountJavaScript()
        {
            await js.InvokeVoidAsync("dotnetInstanceInvocation", DotNetObjectReference.Create(this));
        }

        private List<Movie> movies;

        protected async override Task OnInitializedAsync()
        {

            await Task.Delay(2000);

            movies = new List<Movie>()
            {
                new Movie(){Title = "Spider-Man: Far From Home", ReleaseDate = new DateTime(2019, 7, 2)},
                new Movie(){Title = "Moana", ReleaseDate = new DateTime(2016, 11, 23)},
                new Movie(){Title = "Inception", ReleaseDate = new DateTime(2010, 7, 16)}
            };
        }

        [JSInvokable]
        public static Task<int> GetCurrentCount()
        {
            return Task.FromResult(staticCurrentCount);
        }
    }
}
