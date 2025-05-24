using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using BlacqHubAdmin;

namespace BlacqHubAdmin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new Home()));
        }
    }
}