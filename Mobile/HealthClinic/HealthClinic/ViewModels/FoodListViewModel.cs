﻿using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using HealthClinic.Shared;

namespace HealthClinic
{
    public class FoodListViewModel : BaseViewModel
    {
        #region Fields
        bool _isRefreshing;
        List<FoodLogModel> _foodList;
        ICommand _pullToRefreshCommand;
        #endregion

        #region Properties
        public ICommand PullToRefreshCommand => _pullToRefreshCommand ??
            (_pullToRefreshCommand = new Command(async () => await ExecutePullToRefreshCommand()));

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public List<FoodLogModel> FoodList
        {
            get => _foodList;
            set => SetProperty(ref _foodList, value);
        }
        #endregion

        #region Methods
        async Task ExecutePullToRefreshCommand()
        {
            IsRefreshing = true;

            try
            {
                var unsortedFoodList = await FoodListAPIService.GetFoodLogs().ConfigureAwait(false);
                FoodList = unsortedFoodList.OrderBy(x => x.Description).ToList();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        #endregion
    }
}
