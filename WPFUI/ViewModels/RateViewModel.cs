using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    public class RateViewModel : ViewModelBase
    {
        [Model]
        public Rate RateObject
        {
            get { return GetValue<Rate>(RateObjectProperty); }
            set { SetValue(RateObjectProperty, value); }
        }
        public static readonly PropertyData RateObjectProperty = RegisterProperty("RateObject", typeof(Rate));


        [ViewModelToModel("RateObject", "Title")]
        public string RateTitle
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly PropertyData TitleProperty = RegisterProperty("RateTitle", typeof(string));


        [ViewModelToModel("RateObject", "MeasureTitle")]
        public string RateMeasureTitle
        {
            get { return GetValue<string>(MeasureTitleProperty); }
            set { SetValue(MeasureTitleProperty, value); }
        }
        public static readonly PropertyData MeasureTitleProperty = RegisterProperty("RateMeasureTitle", typeof(string));


        [ViewModelToModel("RateObject", "Price")]
        public string RatePrice
        {
            get { return GetValue<string>(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }
        public static readonly PropertyData PriceProperty = RegisterProperty("RatePrice", typeof(string));
        public override string Title { get { return "Rates Management"; } }
        public RateViewModel(Rate rate = null)
        {
            RateObject = rate ?? new Rate();
        }
    }
}
