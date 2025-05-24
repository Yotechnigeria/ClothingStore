using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Firebase.Database;
using static BlacqHubAdmin.Home;
using Newtonsoft.Json;
using CSharpShellCore;

namespace BlacqHubAdmin
{
    public partial class Home : ContentPage
    {
        public class Product
        {
            public string Product_Name { get; set; }
            public string Cost { get; set; }
            public string Season { get; set; }
            public List<string> Colors { get; set; }
            public List<string> Sizes { get; set; }
            public int Available_Stock { get; set; }
            public string Product_Image { get; set; }
        }
        public Home()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ////
            var fb_db_option = new FirebaseOptions();
            fb_db_option.SyncPeriod.Add(TimeSpan.FromMilliseconds(05));
            var fb_db_client = new FirebaseClient("https://hi-chat-20afb-default-rtdb.europe-west1.firebasedatabase.app/", fb_db_option);
            var fb_db_client_general = fb_db_client.Child("BlacqHub");
            var product = new Product();
            var product_list = new ObservableCollection<Product>();
            ////

            ////
            Application.Current.PageAppearing += (s, e) =>
            {
                fb_db_client_general.AsObservable<Product>().Subscribe(data =>
                {
                    if (data.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                    {
                        product_list = new ObservableCollection<Product>();
                        product_list.Add(data.Object);
                        Products_Listview.ItemsSource = product_list;
                    }
                    if (data.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete)
                    {
                        product_list.Remove(data.Object);
                        Products_Listview.ItemsSource = product_list;
                    }
                });
            };
            Done_Button.Clicked += (s, e) =>
            {
                product = new Product();
                product.Product_Name = Product_name_entry.Text;
                product.Cost = Cost_entry.Text;
                product.Season = Season_entry.Text;
                var json_string = JsonConvert.SerializeObject(product, Formatting.Indented);
                var fb_db_client_general_checker = fb_db_client_general.Client.Child(product.Product_Name).PutAsync(json_string, TimeSpan.FromSeconds(05)).GetAwaiter();
                fb_db_client_general_checker.OnCompleted(() =>
                {
                    try
                    {
                        Toast.MakeText("Successfully Uploaded the Product", CSharpShellCore.Toast.ToastLength.Long).Show();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(ex.Message, CSharpShellCore.Toast.ToastLength.Long).Show();
                    }
                });
            };
            ////
        }
    }
}